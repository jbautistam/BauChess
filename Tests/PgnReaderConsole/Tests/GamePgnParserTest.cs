using System;
using System.Collections.Generic;

using Bau.Libraries.LibPgnReader;
using Bau.Libraries.LibPgnReader.Models;
using Bau.Libraries.LibPgnReader.Models.Movements;

namespace PgnReaderConsole.Tests
{
	/// <summary>
	///		Pruebas para el lector de archivos PGN
	/// </summary>
	internal class GamePgnParserTest
	{
		/// <summary>
		///		Procesa el archivo
		/// </summary>
		internal void Process(string fileName)
		{
			LibraryPgnModel library = new GamePgnParser().Read(fileName);

				// Muestra los errores
				if (library.Errors.Count > 0)
				{
					// Cabecera
					Console.WriteLine($"Errors when read {fileName}");
					// Muestra los errores
					foreach (string error in library.Errors)
						Console.WriteLine(error);
					// Pausa
					Console.ReadKey();
				}
				// Debug de los datos del juego
				foreach (GamePgnModel game in library.Games)
				{
					// Cabecera
					Console.WriteLine($"Game {library.Games.IndexOf(game).ToString()}");
					// Etiquetas
					foreach (HeaderPgnModel header in game.Headers)
						Console.WriteLine($"{header.Tag}: {header.Content}");
					// Comentarios
					if (game.Comments.Count == 0)
						Console.WriteLine("No comments");
					else
						foreach (string comment in game.Comments)
							Console.WriteLine($"Comment: {comment}");
					// Separador
					Console.WriteLine(new string('=', 80));
					Console.WriteLine();
					// Movimientos
					WriteMovements(1, game.Movements);
					// Separador
					Console.WriteLine(new string('-', 80));
					Console.WriteLine("Press any key to show the next game...");
					Console.ReadKey();
				}
		}

		/// <summary>
		///		Escribe los movimientos
		/// </summary>
		private void WriteMovements(int indent, List<BaseMovementModel> movements)
		{
			foreach (BaseMovementModel movement in movements)
			{
				// Escribe el movimiento
				WriteMovement(indent, movement);
				// Escribe las variaciones
				if (movement.Variations.Count > 0)
					foreach (VariationModel variation in movement.Variations)
						WriteVariation(indent + 1, movement.Variations.IndexOf(variation), variation);
			}
		}

		/// <summary>
		///		Escribe los datos de una variación
		/// </summary>
		private void WriteVariation(int indent, int index, VariationModel variation)
		{
			// Cabecera
			Console.WriteLine(new string('\t', indent) + $"Variation {index.ToString()}");
			// Movimientos
			WriteMovements(indent + 1, variation.Movements);
		}

		/// <summary>
		///		Escribe los datos de un movimiento
		/// </summary>
		private void WriteMovement(int indent, BaseMovementModel movement)
		{
			// Cabecera
			Console.Write(new string('\t', indent));
			// Número de movimiento
			Console.Write($"{movement.Turn.Number} - {movement.Turn.Type.ToString()} ");
			// Datos del movimiento
			switch (movement)
			{
				case ResultMovementModel result:
					Console.Write(result.Result.ToString());
					break;
				case PieceMovementModel piece:
					Console.Write(piece.Content);
					break;
			}
			// Información
			if (movement.Info.Count > 0)
				foreach (InfoMovementModel info in movement.Info)
					Console.Write($" --> Info {movement.Info.IndexOf(info).ToString()} - [{info.Content}]");
			// Comentarios
			if (movement.Comments.Count > 0)
				foreach (string comment in movement.Comments)
					Console.Write($" --> Comment {movement.Comments.IndexOf(comment).ToString()} - [{comment}]");
			else
				Console.Write(" No comments");
			// Salto de línea
			Console.WriteLine();
		}
	}
}
