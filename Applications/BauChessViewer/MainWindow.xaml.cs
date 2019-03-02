using System;
using System.Windows;

using Bau.Libraries.LibChessGame.ViewModels;

namespace BauChessViewer
{
	/// <summary>
	///		Ventana principal
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = ViewModel = new MainViewModel(new Controllers.HostController(AppDomain.CurrentDomain.BaseDirectory));
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			// Inicializa el viewModel principal
			ViewModel.Init();
			// Inicializa el tablero
			udtListMovements.ViewModel = ViewModel.GameBoardViewModel;
			udtMovementInfoView.Init(ViewModel.GameBoardViewModel);
			udtBoard.Init(ViewModel.GameBoardViewModel);
			// Carga el archivo inicial
			if (!string.IsNullOrEmpty(Properties.Settings.Default.Game))
				LoadGame(Properties.Settings.Default.Game);
			// Inicia los directorios de imágenes
			if (!string.IsNullOrEmpty(Properties.Settings.Default.PathBoardImages))
				ViewModel.ComboPathBoard.SelectedPath = Properties.Settings.Default.PathBoardImages;
			if (!string.IsNullOrEmpty(Properties.Settings.Default.PathPieceImages))
				ViewModel.ComboPathPieces.SelectedPath = Properties.Settings.Default.PathPieceImages;
			// Asigna el resto de las propiedades
			ViewModel.MustShowAnimation = Properties.Settings.Default.MustShowAnimations;
		}

		/// <summary>
		///		Carga un juego
		/// </summary>
		private void LoadGame(string fileName)
		{
			if (ViewModel.PgnLibraryViewModel.Load(fileName, out string error))
			{
				Properties.Settings.Default.Game = fileName;
				Properties.Settings.Default.Save();
			}
			else
				MessageBox.Show(error);
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{
			Properties.Settings.Default.PathBoardImages = ViewModel.ComboPathBoard.SelectedPath;
			Properties.Settings.Default.PathPieceImages = ViewModel.ComboPathPieces.SelectedPath;
			Properties.Settings.Default.MustShowAnimations = ViewModel.MustShowAnimation;
			if (!string.IsNullOrWhiteSpace(ViewModel?.PgnLibraryViewModel?.FullFileName))
				Properties.Settings.Default.Game = ViewModel.PgnLibraryViewModel.FullFileName;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		///		Muestra el archivo
		/// </summary>
		private void ShowFile()
		{
			if (string.IsNullOrWhiteSpace(ViewModel.PgnLibraryViewModel.FullFileName))
				MessageBox.Show("Select a file");
			else
			{
				Views.ShowFilePgnView fileView = new Views.ShowFilePgnView();

					// Carga el archivo
					fileView.ShowFile(ViewModel.PgnLibraryViewModel.FullFileName);
					// Muestra la ventana
					fileView.Owner = this;
					fileView.Show();
			}
		}

		/// <summary>
		///		ViewModel para el juego
		/// </summary>
		public MainViewModel ViewModel { get; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}

		private void lstMovements_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (sender is System.Windows.Controls.ListBox lstView && ViewModel.GameBoardViewModel?.MovementsList?.SelectedMovement != null)
				lstView.ScrollIntoView(ViewModel.GameBoardViewModel.MovementsList.SelectedMovement);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveConfiguration();
		}

		private void cmdShowFile_Click(object sender, RoutedEventArgs e)
		{
			ShowFile();
		}
	}
}
