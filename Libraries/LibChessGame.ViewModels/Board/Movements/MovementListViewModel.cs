using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Models.Board.Movements;

namespace Bau.Libraries.LibChessGame.ViewModels.Board.Movements
{
	/// <summary>
	///		Lista de movimientos
	/// </summary>
	public class MovementListViewModel : Mvvm.BaseObservableObject
	{
		// Variables privadas
		private BaseMovementViewModel _selectedMovement;
		private MovementFigureViewModel _actualMovement;

		public MovementListViewModel(GameBoardViewModel gameBoardViewModel)
		{
			GameBoardViewModel = gameBoardViewModel;
		}

		/// <summary>
		///		Carga los movimientos de la variación
		/// </summary>
		internal void LoadMovements(List<BaseMovementViewModel> movements)
		{
			// Limpia los movimientos
			Movements.Clear();
			// Inicializa los movimientos
			if (movements?.Count == 0)
				Movements.Add(new MovementRemarkViewModel("No hay ningún movimiento en este juego"));
			else
				LoadVariationMovements(movements);
		}

		/// <summary>
		///		Carga en la lista una variación de movimientos
		/// </summary>
		private void LoadVariationMovements(List<BaseMovementViewModel> movements)
		{
			MovementFigureDoubleViewModel lastMovement = null;
			int movementIndex = 1;

				foreach (BaseMovementViewModel movement in movements)
					switch (movement)
					{
						case MovementFigureViewModel move:
								// Añade a la lista un movimiento real o un movimiento recursivo
								if (lastMovement == null)
								{
									lastMovement = new MovementFigureDoubleViewModel(movementIndex);
									Movements.Add(lastMovement);
								}
								// Asigna el movimiento de negras o blancas
								if (move.Color == Models.Pieces.PieceBaseModel.PieceColor.Black)
								{
									// Asigna el movimiento de negras
									lastMovement.BlackMovement = move;
									// Incrementa el índice y vacía el último movimiento
									movementIndex++;
									lastMovement = null;
								}
								else
									lastMovement.WhiteMovement = move;
							break;
						case MovementGameEndViewModel move:
								Movements.Add(move);
								lastMovement = null;
							break;
					}
		}

		/// <summary>
		///		Selecciona un movimiento en la lista
		/// </summary>
		internal void SelectMovement(MovementFigureModel movement)
		{
			if (movement == null)
			{
				SelectedMovement = null;
				ActualMovement = null;
			}
			else
				foreach (BaseMovementViewModel baseMovement in Movements)
					if (baseMovement is MovementFigureDoubleViewModel movementDouble)
					{
						// Selecciona el movimiento blanco
						if (movementDouble.WhiteMovement != null)
						{
							movementDouble.WhiteMovement.Selected = movementDouble.WhiteMovement.Movement.Id == movement.Id;
							if (movementDouble.WhiteMovement.Selected)
							{
								SelectedMovement = movementDouble;
								ActualMovement = movementDouble.WhiteMovement;
							}
						}
						// Selecciona el movimiento negro
						if (movementDouble.BlackMovement != null)
						{
							movementDouble.BlackMovement.Selected = movementDouble.BlackMovement.Movement.Id == movement.Id;
							if (movementDouble.BlackMovement.Selected)
							{
								SelectedMovement = movementDouble;
								ActualMovement = movementDouble.BlackMovement;
							}
						}
					}
		}

		/// <summary>
		///		ViewModel del tablero
		/// </summary>
		public GameBoardViewModel GameBoardViewModel { get; }

		/// <summary>
		///		Movimientos para mostrar en la vista: combina movimientos con comentarios y resultados
		/// </summary>
		public ObservableCollection<BaseMovementViewModel> Movements { get; } = new ObservableCollection<BaseMovementViewModel>();

		/// <summary>
		///		Movimiento seleccionado
		/// </summary>
		public BaseMovementViewModel SelectedMovement
		{
			get { return _selectedMovement; }
			set { CheckObject(ref _selectedMovement, value); }
		}

		/// <summary>
		///		Movimiento actual
		/// </summary>
		public MovementFigureViewModel ActualMovement
		{
			get { return _actualMovement; }
			set { CheckObject(ref _actualMovement, value); }
		}
	}
}