using System;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Models.Games;
using Bau.Libraries.LibChessGame.Mvvm;

namespace Bau.Libraries.LibChessGame.ViewModels.Pgn
{
	/// <summary>
	///		ViewModel para un <see cref="LibraryModel"/>
	/// </summary>
	public class PgnLibraryViewModel : Mvvm.BaseObservableObject
	{
		// Variables privadas
		private string _fileName;
		private PgnGameInfoViewModel _selectedGame;

		public PgnLibraryViewModel(Board.GameBoardViewModel gameBoardViewModel)
		{
			GameBoardViewModel = gameBoardViewModel;
			Init();
			NextGameCommand = new BaseCommand(parameter => GoGame(true),
											  parameter => CanGoGame(true))
									.AddListener(this, FileName);
			PreviousGameCommand = new BaseCommand(parameter => GoGame(false),
												  parameter => CanGoGame(false))
									.AddListener(this, FileName);
		}

		/// <summary>
		///		Inicializa el ViewModel
		/// </summary>
		public void Init()
		{
			ConvertToViewModel(new LibraryModel());
		}

		/// <summary>
		///		Carga un archivo
		/// </summary>
		public bool Load(string fileName, out string error)
		{
			// Inicializa los argumentos de salida
			error = "";
			// Inicializa el juego
			Library = new LibraryModel();
			// Carga el juego
			if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
			{
				// Carga el juego
				try
				{
					// Carga el juego
					Library = LoadLibrary(fileName);
					// Asigna el nombre de archivo
					FullFileName = fileName;
					FileName = System.IO.Path.GetFileName(fileName);
				}
				catch (Exception exception)
				{
					error = $"Error al cargar el juego: {exception.Message}";
				}
			}
			else
				error = $"No existe el archivo: {fileName}";
			// y lo muestra (lo hace fuera del try porque debe crear el modelo vacío)
			ConvertToViewModel(Library);
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Carga la librería de un archivo PGN
		/// </summary>
		private LibraryModel LoadLibrary(string fileName)
		{
			LibPgnReader.Models.LibraryPgnModel libraryPgn = new LibPgnReader.GamePgnParser().Read(fileName);

				// Convierte los datos del archivo PGN
				return new LibPgn.Conversor.GamePgnConversor().Convert(libraryPgn);
		}

		/// <summary>
		///		Convierte la librería en ViewModels
		/// </summary>
		private void ConvertToViewModel(LibraryModel library)
		{
			// Asigna el archivo
			Library = library;
			// Limpia la lista
			Games.Clear();
			// Carga los juegos
			foreach (GameModel game in library.Games)
				Games.Add(new PgnGameInfoViewModel(this, game));
			// Selecciona un elemento
			if (Games.Count > 0)
				SelectedGame = Games[0];
			else
				SelectedGame = new PgnGameInfoViewModel(this, new GameModel());
		}

		/// <summary>
		///		Modifica el juego seleccionado
		/// </summary>
		private void UpdateSelectedGame()
		{
			GameBoardViewModel.LoadMovements(SelectedGame?.Game);
		}

		/// <summary>
		///		Pasa a la partida siguiente o anterior
		/// </summary>
		private void GoGame(bool next)
		{
			if (next)
				SelectedGame = Games[SelectedGameIndex + 1];
			else
				SelectedGame = Games[SelectedGameIndex - 1];
		}

		/// <summary>
		///		Indica si puede ir a la partida siguiente o anterior
		/// </summary>
		private bool CanGoGame(bool next)
		{
			return SelectedGameIndex != -1 && ((next && SelectedGameIndex < Games.Count - 1) ||
											   (!next && SelectedGameIndex > 0));
		}

		/// <summary>
		///		ViewModel del tablero
		/// </summary>
		public Board.GameBoardViewModel GameBoardViewModel { get; }

		/// <summary>
		///		Datos del archivo
		/// </summary>
		public LibraryModel Library { get; private set; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FullFileName { get; private set; }

		/// <summary>
		///		Nombre del archivo abierto
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Partidas
		/// </summary>
		public ObservableCollection<PgnGameInfoViewModel> Games { get; } = new ObservableCollection<PgnGameInfoViewModel>();

		/// <summary>
		///		Partida seleccionada
		/// </summary>
		public PgnGameInfoViewModel SelectedGame
		{
			get { return _selectedGame; }
			set 
			{ 
				if (CheckObject(ref _selectedGame, value))
					UpdateSelectedGame();
			}
		}

		/// <summary>
		///		Indice de la partida seleccionada
		/// </summary>
		public int SelectedGameIndex
		{
			get
			{
				if (Games == null || Games.Count == 0 || SelectedGame == null)
					return -1;
				else
					return Games.IndexOf(SelectedGame);
			}
		}

		/// <summary>
		///		Comando para cargar la partida anterior
		/// </summary>
		public BaseCommand PreviousGameCommand { get; }

		/// <summary>
		///		Comando para cargar la siguiente partida
		/// </summary>
		public BaseCommand NextGameCommand { get; }
	}
}