using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibChessGame.Mvvm.Controllers
{
	/// <summary>
	///		Interface para el controlador de la ventana principal
	/// </summary>
	public interface IHostController
	{
		/// <summary>
		///		Abre la ventana de selección de un archivo
		/// </summary>
		string OpenFile(string pathBase, string filter, string defaultExt);

		/// <summary>
		///		Muestra un mensaje
		/// </summary>
		void ShowMessage(string message);

		/// <summary>
		///		Obtiene los directorios donde se encuentran las imágenes del tablero
		/// </summary>
		List<string> GetPathsBoard();

		/// <summary>
		///		Obtiene los directorios donde se encuentran las imágenes de las piezas
		/// </summary>
		List<string> GetPathsPieces();
	}
}
