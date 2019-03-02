using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibChessGame.ViewModels.EventArguments
{
	/// <summary>
	///		Argumentos del evento de mostrar movimientos
	/// </summary>
	public class ShowMovementEventArgs : EventArgs
	{
		public ShowMovementEventArgs(List<Board.Actions.ActionBaseModel> actions, bool showAnimation)
		{
			Actions = actions;
			ShowAnimation = showAnimation;
		}

		/// <summary>
		///		Muestra las acciones
		/// </summary>
		public List<Board.Actions.ActionBaseModel> Actions { get; }

		/// <summary>
		///		Indica si se debe mostrar la animación
		/// </summary>
		public bool ShowAnimation { get; }
	}
}
