using System;

namespace Bau.Libraries.LibChessGame.ViewModels
{
	/// <summary>
	///		ViewModel de la ventana principal
	/// </summary>
	public class MainViewModel : Mvvm.BaseObservableObject
	{
		// Variables privadas
		private bool _mustShowAnimation;
		private PathComboImagesViewModel _comboPathBoard, _comboPathPieces;
		private Board.GameBoardViewModel _gameBoardViewModel;
		private Pgn.PgnLibraryViewModel _pgnLibraryViewModel;

		public MainViewModel(Mvvm.Controllers.IHostController hostController)
		{
			// Inicializa los objetos estáticos
			HostController = hostController;
			// Inicializa los objetos
			ComboPathBoard = new PathComboImagesViewModel();
			ComboPathPieces = new PathComboImagesViewModel();
			GameBoardViewModel = new Board.GameBoardViewModel(this);
			PgnLibraryViewModel = new Pgn.PgnLibraryViewModel(GameBoardViewModel);
			// Inicializa las propiedades
			MustShowAnimation = true;
			// Inicializa los comandos
			LoadCommand = new Mvvm.BaseCommand(parameter => LoadFile());
		}

		/// <summary>
		///		Inicializa el viewModel
		/// </summary>
		public void Init()
		{
			ComboPathBoard.Init(PathComboImagesViewModel.PathType.Board);
			ComboPathPieces.Init(PathComboImagesViewModel.PathType.Pieces);
		}

		/// <summary>
		///		Carga un archivo
		/// </summary>
		private void LoadFile()
		{
			string fileName = HostController.OpenFile("Data\\Samples", "Archivos PGN (*.pgn)|*.pgn|Todos los archivos|*.*", "pgn");

				if (!string.IsNullOrWhiteSpace(fileName) && System.IO.File.Exists(fileName) &&
						!PgnLibraryViewModel.Load(fileName, out string error))
					HostController.ShowMessage(error);
		}

		/// <summary>
		///		Controlador principal
		/// </summary>
		public static Mvvm.Controllers.IHostController HostController { get; private set; }

		/// <summary>
		///		Librería de archivos PGN
		/// </summary>
		public Pgn.PgnLibraryViewModel PgnLibraryViewModel
		{
			get { return _pgnLibraryViewModel; }
			set { CheckObject(ref _pgnLibraryViewModel, value); }
		}

		/// <summary>
		///		ViewModel del tablero de juego
		/// </summary>
		public Board.GameBoardViewModel GameBoardViewModel
		{
			get { return _gameBoardViewModel; }
			set { CheckProperty(ref _gameBoardViewModel, value); }
		}

		/// <summary>
		///		Combo para los directorios de imágenes del tablero
		/// </summary>
		public PathComboImagesViewModel ComboPathBoard 
		{ 
			get { return _comboPathBoard; }
			set { CheckObject(ref _comboPathBoard, value); }
		}

		/// <summary>
		///		Combo para los directorios de imágenes de las piedas
		/// </summary>
		public PathComboImagesViewModel ComboPathPieces 
		{ 
			get { return _comboPathPieces; }
			set { CheckObject(ref _comboPathPieces, value); }
		}

		/// <summary>
		///		Indica si se deben mostrar las animaciones
		/// </summary>
		public bool MustShowAnimation 
		{ 
			get { return _mustShowAnimation; }
			set { CheckProperty(ref _mustShowAnimation, value); }
		}

		/// <summary>
		///		Comando de carga de archivos
		/// </summary>
		public Mvvm.BaseCommand LoadCommand { get; }
	}
}
