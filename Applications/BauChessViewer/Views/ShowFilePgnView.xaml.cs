using System;
using System.Windows;

namespace BauChessViewer.Views
{
	/// <summary>
	/// Lógica de interacción para ShowFilePgnView.xaml
	/// </summary>
	public partial class ShowFilePgnView : Window
	{
		public ShowFilePgnView()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Muestra el archivo
		/// </summary>
		public void ShowFile(string fileName)
		{
			// Limpia el cuadro de texto
			txtFile.Text = string.Empty;
			// Carga el archivo
			if (!System.IO.File.Exists(fileName))
				txtFile.Text = $"Can't find the file {fileName}";
			else
				txtFile.Text = System.IO.File.ReadAllText(fileName);
		}
	}
}
