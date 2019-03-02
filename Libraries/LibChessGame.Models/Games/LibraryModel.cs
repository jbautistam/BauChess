using System;

namespace Bau.Libraries.LibChessGame.Models.Games
{
	/// <summary>
	///		Modelo con los datos de una librería
	/// </summary>
	public class LibraryModel
	{
		/// <summary>
		///		Colección de juegos
		/// </summary>
		public GameModelCollection Games { get; } = new GameModelCollection();
	}
}
