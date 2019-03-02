using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bau.Libraries.LibChessGame.ViewModels
{
	/// <summary>
	///		ViewModel para los combos de imágenes
	/// </summary>
	public class PathComboImagesViewModel : Mvvm.BaseObservableObject
	{
		// Enumerados públicos
		/// <summary>
		///		Tipo de directorio que contiene las imágenes
		/// </summary>
		public enum PathType
		{
			/// <summary>Imágenes de tablero</summary>
			Board,
			/// <summary>Imágenes de piezas</summary>
			Pieces
		}

		// Variables privadas
		private Dictionary<string, string> _dctPaths = new Dictionary<string, string>();
		private string _selectedPath;

		/// <summary>
		///		Inicializa los elementos del viewModel
		/// </summary>
		public void Init(PathType type)
		{
			// Limpia los directorios
			Paths.Clear();
			_dctPaths.Clear();
			// Obtiene los directorios del controlador
			foreach (string path in GetPaths(type))
				if (System.IO.Directory.Exists(path))
				{
					string text = System.IO.Path.GetFileName(path);

						if (!_dctPaths.ContainsKey(text))
						{
							Paths.Add(text);
							_dctPaths.Add(text, path);
						}
				}
			Paths.Add("Predeterminado");
			// Selecciona el primer elemento
			if (Paths.Count > 0)
				SelectedPath = Paths[0];
		}

		/// <summary>
		///		Obtiene los directorios
		/// </summary>
		private List<string> GetPaths(PathType type)
		{
			switch (type)
			{
				case PathType.Board:
					return MainViewModel.HostController.GetPathsBoard();
				default:
					return MainViewModel.HostController.GetPathsPieces();
			}
		}

		/// <summary>
		///		Obtiene el directorio completo
		/// </summary>
		public string GetFullPathName()
		{
			if (string.IsNullOrEmpty(SelectedPath) || !_dctPaths.ContainsKey(SelectedPath))
				return string.Empty;
			else
				return _dctPaths[SelectedPath];
		}

		/// <summary>
		///		Archivos
		/// </summary>
		public ObservableCollection<string> Paths { get; } = new ObservableCollection<string>();

		/// <summary>
		///		Nombre completo del directorio de imágenes
		/// </summary>
		public string SelectedPath
		{
			get { return _selectedPath; }
			set { CheckProperty(ref _selectedPath, value); }
		}
	}
}
