﻿using System;

using Bau.Libraries.LibChessGame.Mvvm.Media;

namespace Bau.Libraries.LibChessGame.ViewModels.Board.Movements
{
	/// <summary>
	///		ViewModel de un par de movimientos de figura
	/// </summary>
	public class MovementFigureDoubleViewModel : BaseMovementViewModel
	{
		// Variables privadas
		private MovementFigureViewModel _blackMovement, _whiteMovement;
		private int _movementIndex;
		private string _movementNumber;
		private MvvmColor _foreground;

		public MovementFigureDoubleViewModel(int movementIndex, int movementLineIndex)
		{
			MovementIndex = movementIndex;
			MovementNumber = $"{movementLineIndex}. ";
			Foreground = MvvmColor.Black;
		}

		/// <summary>
		///		Número de movimiento
		/// </summary>
		public int MovementIndex
		{
			get { return _movementIndex; }
			set { CheckProperty(ref _movementIndex, value); }
		}

		/// <summary>
		///		Texto con el número de movimiento
		/// </summary>
		public string MovementNumber
		{
			get { return _movementNumber; }
			set { CheckProperty(ref _movementNumber, value); }
		}

		/// <summary>
		///		Movimiento de negras
		/// </summary>
		public MovementFigureViewModel BlackMovement 
		{ 
			get { return _blackMovement; }
			set { CheckProperty(ref _blackMovement, value); }
		}

		/// <summary>
		///		Movimiento de blancas
		/// </summary>
		public MovementFigureViewModel WhiteMovement 
		{ 
			get { return _whiteMovement; }
			set { CheckProperty(ref _whiteMovement, value); }
		}

		/// <summary>
		///		Color de texto
		/// </summary>
		public MvvmColor Foreground
		{ 
			get { return _foreground; }
			set { CheckObject(ref _foreground, value); }
		}
	}
}