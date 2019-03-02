using System;

using Bau.Libraries.LibChessGame.Models.Pieces;

namespace Bau.Libraries.LibChessGame.ViewModels.Board.Actions
{
	/// <summary>
	///		Acción de promocionar una pieza
	/// </summary>
	public class ActionPromoteModel : ActionBaseModel
	{
		public ActionPromoteModel(PieceBaseModel.PieceType type, PieceBaseModel.PieceColor color,
								  int toRow, int toColumn) : base(type, color)
		{
			ToRow = toRow;
			ToColumn = toColumn;
		}

		/// <summary>
		///		Fila donde se coloca la pieza promocionada
		/// </summary>
		public int ToRow { get; }

		/// <summary>
		///		Columna donde se coloca la pieza promocionada
		/// </summary>
		public int ToColumn { get; }
	}
}
