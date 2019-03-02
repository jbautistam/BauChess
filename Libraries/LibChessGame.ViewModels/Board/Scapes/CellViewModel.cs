using System;

using Bau.Libraries.LibChessGame.Models.Pieces;

namespace Bau.Libraries.LibChessGame.ViewModels.Board.Scapes
{
	/// <summary>
	///		ViewModel para las celdas en el tablero
	/// </summary>
	public class CellViewModel : ScapeBaseViewModel
	{
		// Variables privadas
		private string _fileImage;

		public CellViewModel(int row, int column, PieceBaseModel.PieceColor color) : base(row, column, color) 
		{
			if (color == PieceBaseModel.PieceColor.White)
				FileImage = "BoardLight.gif";
			else
				FileImage = "BoardDark.gif";
		}

		/// <summary>
		///		Archivo de imagen
		/// </summary>
		public string FileImage
		{
			get { return _fileImage; }
			set { CheckProperty(ref _fileImage, value); }
		}
	}
}
