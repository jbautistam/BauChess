using System;

namespace Bau.Libraries.LibChessGame.ViewModels.Board.Actions
{
	/// <summary>
	///		Acción para resetear el tablero
	/// </summary>
	public class ActionResetBoardModel : ActionBaseModel
	{
		public ActionResetBoardModel() : base(Models.Pieces.PieceBaseModel.PieceType.Pawn, Models.Pieces.PieceBaseModel.PieceColor.White) {}
	}
}
