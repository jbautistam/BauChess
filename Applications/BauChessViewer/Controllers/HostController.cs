using System;
using System.Collections.Generic;

namespace BauChessViewer.Controllers
{
	/// <summary>
	///		Controlador principal
	/// </summary>
	public class HostController : Bau.Libraries.LibChessGame.Mvvm.Controllers.IHostController
	{
		// Constantes privadas
		private const string SubPathBoard = @"Data\Graphics\Boards";
		private const string SubPathPiecess = @"Data\Graphics\Pieces";

		public HostController(string pathBase)
		{
			PathBase = pathBase;
		}

		/// <summary>
		///		Abre la ventana de selección de un archivo
		/// </summary>
		public string OpenFile(string pathBase, string filter, string defaultExt)
		{
			Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();

				// Asigna las propiedades
				file.Multiselect = false;
				file.InitialDirectory = System.IO.Path.Combine(PathBase, "Data\\Samples");
				// file.FileName = defaultFileName;
				file.DefaultExt = "pgn";
				file.Filter = "Archivos PGN (*.pgn)|*.pgn|Todos los archivos|*.*";
				// Muestra el cuadro de diálogo y devuelve los archivos
				if (file.ShowDialog() ?? false && file.FileNames.Length > 0)
					return file.FileNames[0];
				else
					return string.Empty;
		}

		/// <summary>
		///		Muestra un mensaje
		/// </summary>
		public void ShowMessage(string message)
		{
			System.Windows.MessageBox.Show(message);
		}

		/// <summary>
		///		Obtiene los directorios donde se encuentran las imágenes del tablero
		/// </summary>
		public List<string> GetPathsBoard()
		{
			return GetSubPaths(System.IO.Path.Combine(PathBase, SubPathBoard));
		}

		/// <summary>
		///		Obtiene los directorios donde se encuentran las imágenes de las piezas
		/// </summary>
		public List<string> GetPathsPieces()
		{
			return GetSubPaths(System.IO.Path.Combine(PathBase, SubPathPiecess));
		}

		/// <summary>
		///		Obtiene los subdirectorios de un directorio
		/// </summary>
		private List<string> GetSubPaths(string path)
		{
			List<string> directories = new List<string>();

				// Obtiene los directorios hijo
				if (System.IO.Directory.Exists(path))
					foreach (string directory in System.IO.Directory.GetDirectories(path))
						directories.Add(directory);
				// Devuelve la lista de directorios
				return directories;
		}

		/// <summary>
		///		Directorio base de la aplicación
		/// </summary>
		public string PathBase { get; }
	}
}
