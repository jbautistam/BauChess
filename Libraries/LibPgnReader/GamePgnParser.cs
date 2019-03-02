using System;
using System.Collections.Generic;

using Bau.Libraries.LibPgnReader.Parsers.Sentences;
using Bau.Libraries.LibPgnReader.Models;
using Bau.Libraries.LibPgnReader.Models.Movements;

namespace Bau.Libraries.LibPgnReader
{
	/// <summary>
	///		Parser de un archivo PGN para obtener una lista de juegos
	/// </summary>
	public class GamePgnParser
	{
		/// <summary>
		///		Lee los juegos del archivo
		/// </summary>
		public LibraryPgnModel Read(string fileName, System.Text.Encoding encoding = null)
		{
			LibraryPgnModel library = new LibraryPgnModel();

				// Abre el archivo
				Open(fileName, encoding);
				// Recoge la lista de juegos
				using (IEnumerator<SentenceBaseModel> sentenceEnumerator = Reader.Read().GetEnumerator())
				{
					SentenceBaseModel lastSentence = null;

						// Lee los juegos
						do
						{
							lastSentence = ReadGame(sentenceEnumerator, library);
						}
						while (!(lastSentence is SentenceEndModel) && !(lastSentence is SentenceErrorModel));
						// Añade el error a la librería
						if (lastSentence != null && lastSentence is SentenceErrorModel error)
							library.Errors.Add(error.Content);
				}
				// Cierra el archivo
				Close();
				// Ajusta las jugadas
				foreach (GamePgnModel game in library.Games)
					AdjustTurnPlay(game.Movements);
				// Devuelve la librería
				return library;
		}

		/// <summary>
		///		Lee los datos del juego
		/// </summary>
		private SentenceBaseModel ReadGame(IEnumerator<SentenceBaseModel> sentenceEnumerator, LibraryPgnModel library)
		{
			SentenceBaseModel lastSentence = null;

				// Lee el juego
				do
				{
					GamePgnModel game = new GamePgnModel();

						// Guarda la etiqueta leída como última sentencia de la partida anterior
						if (lastSentence != null && lastSentence is SentenceTagModel sentenceTag)
							game.Headers.Add(new HeaderPgnModel(sentenceTag.Tag, sentenceTag.Content));
						// Lee las cabeceras
						lastSentence = ReadHeaders(sentenceEnumerator, game);
						// Añade los moviemientos
						if (lastSentence is SentenceTurnNumberModel)
						{
							// Lee los movimientos
							lastSentence = ReadMovements(sentenceEnumerator, lastSentence, game.Movements);
							// Añade el juego
							library.Games.Add(game);
						}
						else
							lastSentence = new SentenceErrorModel("Can't find game movements");
				}
				while (!(lastSentence is SentenceEndModel) && !(lastSentence is SentenceErrorModel));
				// Devuelve el juego creado
				return lastSentence;
		}

		/// <summary>
		///		Lee las cabeceras
		/// </summary>
		private SentenceBaseModel ReadHeaders(IEnumerator<SentenceBaseModel> sentenceEnumerator, GamePgnModel game)
		{
			SentenceBaseModel sentence = GetNextSentence(sentenceEnumerator);

				// Obtiene las cabeceras
				do
				{
					// Trata la sentencia
					switch (sentence)
					{
						case SentenceTagModel sentenceTag:
								game.Headers.Add(new HeaderPgnModel(sentenceTag.Tag, sentenceTag.Content));
							break;
						case SentenceCommentModel sentenceComment:
								game.Comments.Add(sentenceComment.Content);
							break;
					}
					// Pasa a la siguiente sentencia
					sentence = GetNextSentence(sentenceEnumerator);
				}
				while (!(sentence is SentenceEndModel) && !(sentence is SentenceErrorModel) && !(sentence is SentenceTurnNumberModel));
				// Devuelve la última sentencia leída
				return sentence;
		}

		/// <summary>
		///		Lee los movimientos
		/// </summary>
		private SentenceBaseModel ReadMovements(IEnumerator<SentenceBaseModel> sentenceEnumerator, SentenceBaseModel previousSentence, 
												List<BaseMovementModel> movements)
		{
			SentenceBaseModel sentence = previousSentence;
			BaseMovementModel lastMovement = null;
			TurnModel lastTurn = null;

				// Añade los movimientos
				do
				{
					// Trata la sentencia
					switch (sentence)
					{
						case SentenceTurnNumberModel sentenceTurn:
								lastMovement = null;
								lastTurn = new TurnModel(sentenceTurn.Content, TurnModel.TurnType.White);
							break;
						case SentenceTurnPlayModel sentencePlay:
								if (lastTurn == null)
									sentence = new SentenceErrorModel($"There is not turn to add the play {sentencePlay.Content}");
								else
								{
									// Crea el movimiento
									lastMovement = new PieceMovementModel(lastTurn, sentencePlay.Content);
									// Añade el movimiento
									movements.Add(lastMovement);
								}
							break;
						case SentenceTurnResultModel sentenceResult:
								if (lastTurn == null)
									sentence = new SentenceErrorModel($"There is not turn to add the result {sentenceResult.Content}");
								else
								{
									// Crea el movimiento
									lastMovement = new ResultMovementModel(lastTurn, sentence.Content);
									// Añade el movimiento
									movements.Add(lastMovement);
								}
							break;
						case SentenceTurnInformationModel sentenceInformation:
								if (lastMovement == null)
									sentence = new SentenceErrorModel($"There is not movement to add the information {sentenceInformation.Content}");
								else
									lastMovement.Info.Add(new InfoMovementModel(sentence.Content));
							break;
						case SentenceCommentModel sentenceComment:
								if (lastMovement == null)
									sentence = new SentenceErrorModel($"There is not sentence to add the comment {sentenceComment.Content}");
								else
									lastMovement.Comments.Add(sentenceComment.Content);
							break;
						case SentenceTurnStartVariationModel sentenceVariation:
								if (lastMovement == null)
									sentence = new SentenceErrorModel("There is not sentence to add a variation");
								else
								{
									VariationModel variation = new VariationModel(lastMovement);

										// Limpia esta sentencia del enumerado
										sentence = GetNextSentence(sentenceEnumerator);
										// Añade los comentarios y la información a la variación
										while (sentence is SentenceTurnInformationModel || sentence is SentenceCommentModel)
										{
											// Añade la información de la sentencia
											switch (sentence)
											{
												case SentenceTurnInformationModel sentenceInfo:
														variation.Info.Add(new InfoMovementModel(sentenceInfo.Content));
													break;
												case SentenceCommentModel sentenceComment:
														variation.Comments.Add(sentenceComment.Content);
													break;
											}
											// Pasa a la siguiente sentencia
											sentence = GetNextSentence(sentenceEnumerator);
										}
										// Lee los movimientos de la variación
										sentence = ReadMovements(sentenceEnumerator, sentence, variation.Movements);
										// Añade la variación a la colección
										if (variation.Movements.Count > 0)
											lastMovement.Variations.Add(variation);
										// Pasa a la siguiente sentencia
										if (!(sentence is SentenceTurnEndVariationModel))
											sentence = new SentenceErrorModel("Can't find the end variation sentence");
								}
							break;
					}
					// Pasa a la siguiente sentencia
					if (!(sentence is SentenceErrorModel))
						sentence = GetNextSentence(sentenceEnumerator);
				}
				while (!(sentence is SentenceEndModel) && !(sentence is SentenceErrorModel) && !(sentence is SentenceTagModel) && !(sentence is SentenceTurnEndVariationModel));
				// Devuelve la última sentencia leída
				return sentence;
		}

		/// <summary>
		///		Obtiene la siguiente sentencia
		/// </summary>
		private SentenceBaseModel GetNextSentence(IEnumerator<SentenceBaseModel> sentenceEnumerator)
		{
			if (sentenceEnumerator.MoveNext())
				return sentenceEnumerator.Current;
			else
				return new SentenceEndModel();
		}

		/// <summary>
		///		Ajusta los turnos de juego
		/// </summary>
		private void AdjustTurnPlay(List<BaseMovementModel> movements)
		{
			bool isWhite = true;

				foreach (BaseMovementModel movement in movements)
					switch (movement)
					{
						case PieceMovementModel pieceMovement:
								// Ajusta para los cambios de turno en las variaciones
								if (pieceMovement.Turn.Number.EndsWith(".."))
									isWhite = false;
								// Asigna el turno
								if (isWhite)
									pieceMovement.SetType(TurnModel.TurnType.White);
								else
									pieceMovement.SetType(TurnModel.TurnType.Black);
								// Cambia el color
								isWhite = !isWhite;
								// Cambia las variaciones
								foreach (VariationModel variation in pieceMovement.Variations)
									AdjustTurnPlay(variation.Movements);
							break;
					}
		}

		/// <summary>
		///		Abre un archivo
		/// </summary>
		private void Open(string fileName, System.Text.Encoding encoding = null)
		{
			// Cierra el archivo abierto previamente
			Close();
			// Abre el archivo
			Reader = new Parsers.SentencesParser();
			Reader.Open(fileName, encoding);
		}

		/// <summary>
		///		Cierra el archivo
		/// </summary>
		internal void Close()
		{
			if (Reader != null)
			{
				Reader.Close();
				Reader = null;
			}
		}

		/// <summary>
		///		Lector de sentencias
		/// </summary>
		private Parsers.SentencesParser Reader { get; set; }
	}
}