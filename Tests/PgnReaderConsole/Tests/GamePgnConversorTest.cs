using System;
using System.Collections.Generic;

using Bau.Libraries.LibChessGame.Models.Board.Movements;
using Bau.Libraries.LibChessGame.Models.Games;

namespace PgnReaderConsole.Tests
{
	/// <summary>
	///		Prueba para el conversor de archivos Pgn
	/// </summary>
	internal class GamePgnConversorTest
	{
		/// <summary>
		///		Prueba de la librería de conversión
		/// </summary>
		internal void Process(string fileName)
		{
			Bau.Libraries.LibPgnReader.Models.LibraryPgnModel libraryPgn = new Bau.Libraries.LibPgnReader.GamePgnParser().Read(fileName);
			LibraryModel library = new Bau.Libraries.LibPgn.Conversor.GamePgnConversor().Convert(libraryPgn);

				// Cabecera
				Console.WriteLine($"File: {fileName}. Games: {library.Games.Count.ToString()}");
				// Datos de los juegos
				foreach (GameModel game in library.Games)
				{
					// Datos del juego
					Console.WriteLine($"Game {library.Games.IndexOf(game).ToString()}");
					Console.WriteLine($"Event: {game.Event}");
					Console.WriteLine($"Round: {game.Round}");
					Console.WriteLine($"Site: {game.Site}");
					Console.WriteLine($"White player: {game.WhitePlayer}");
					Console.WriteLine($"Black player: {game.BlackPlayer}");
					Console.WriteLine($"Result: {game.Result.ToString()}");
					Console.WriteLine($"Date: {game.Date}");
					Console.WriteLine($"Event date: {game.EventDate}");
					Console.WriteLine($"Setup: {game.Fen}");
					// Comentarios
					if (game.Comments.Count > 0)
						foreach (string comment in game.Comments)
							Console.WriteLine($"Comment: {comment}");
					// Etiquetas
					foreach (KeyValuePair<string, string> tag in game.Tags)
						Console.WriteLine($"Tag: {tag.Key} -- {tag.Value}");
					// Configuración del tablero
					if (game.Board == null)
						Console.WriteLine("No board setup");
					else
					{
						Console.WriteLine("Board setup");
						Console.WriteLine(game.Board.GetText());
						Console.WriteLine();
					}
					// Separador
					Console.WriteLine(new string('-', 80));
					Console.WriteLine();
					// Movimientos
					WriteMovements(0, game.Variation.Movements);
					// Separador
					Console.WriteLine(new string('-', 80));
					Console.WriteLine("Press any key to show the next game...");
					Console.ReadKey();
				}
		}

		/// <summary>
		///		Muestra los movimientos de un juego
		/// </summary>
		private void WriteMovements(int indent, MovementModelCollection movements)
		{
			foreach (MovementBaseModel movement in movements)
			{
				// Escribe el movimiento
				WriteMovement(indent, movement);
				// Escribe las variaciones
				if (movement.Variations.Count > 0)
					foreach (MovementVariationModel variation in movement.Variations)
						WriteVariation(indent + 1, movement.Variations.IndexOf(variation), variation);
			}
		}

		/// <summary>
		///		Escribe los datos de una variación
		/// </summary>
		private void WriteVariation(int indent, int index, MovementVariationModel variation)
		{
			// Cabecera
			Console.WriteLine(new string('\t', indent) + $"Variation {index.ToString()}");
			// Movimientos
			WriteMovements(indent + 1, variation.Movements);
		}

		/// <summary>
		///		Escribe los datos de un movimiento
		/// </summary>
		private void WriteMovement(int indent, MovementBaseModel baseMovement)
		{
			// Cabecera
			Console.Write(new string('\t', indent));
			// Número de movimiento
			Console.Write($"{baseMovement.Turn.Number} - {baseMovement.Turn.Type.ToString()} ");
			// Datos del movimiento
			switch (baseMovement)
			{
				case MovementGameEndModel result:
						Console.Write(result.Result.ToString());
					break;
				case MovementFigureModel movement:
						Console.Write(movement.ToString());
					break;
			}
			// Información
			if (baseMovement.Info.Count > 0)
				foreach (MovementInfoModel info in baseMovement.Info)
					Console.Write($" --> Info {baseMovement.Info.IndexOf(info).ToString()} - [{info.Content}]");
			// Comentarios
			if (baseMovement.Comments.Count > 0)
				foreach (string comment in baseMovement.Comments)
					Console.Write($" --> Comment {baseMovement.Comments.IndexOf(comment).ToString()} - [{comment}]");
			else
				Console.Write(" No comments");
			// Salto de línea
			Console.WriteLine();
		}
	}
}
