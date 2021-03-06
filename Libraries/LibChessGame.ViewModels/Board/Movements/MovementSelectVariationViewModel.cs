﻿using System;

using Bau.Libraries.LibChessGame.Models.Board.Movements;
using Bau.Libraries.LibChessGame.Models.Pieces;

namespace Bau.Libraries.LibChessGame.ViewModels.Board.Movements
{
	/// <summary>
	///		ViewModel de una variación de un movimiento
	/// </summary>
	public class MovementSelectVariationViewModel : BaseMovementViewModel
	{
		// Variables privadas
		private MovementFigureModel _movement;
		private PieceBaseModel.PieceType _piece;
		private PieceBaseModel.PieceColor _color;
		private string _text;

		public MovementSelectVariationViewModel(GameBoardViewModel gameBoardViewModel, MovementVariationModel variation, MovementFigureModel movement)
		{
			// Inicializa los objetos
			GameBoardViewModel = gameBoardViewModel;
			Movement = movement;
			Variation = variation;
			// Inicializa las propiedades
			Piece = movement.OriginPiece;
			Color = movement.Color;
			Text = movement.Content;
			// Inicializa los comandos
			SelectMovementCommand = new Mvvm.BaseCommand(parameter => ExecuteMovement(), parameter => CanExecuteMovement());
		}

		/// <summary>
		///		Ejecuta un movimiento
		/// </summary>
		private void ExecuteMovement()
		{
			GameBoardViewModel.GoToVariation(Variation, Movement);
		}

		/// <summary>
		///		Indica si puede ejecutar el movimiento
		/// </summary>
		private bool CanExecuteMovement()
		{
			return Movement != null;
		}

		/// <summary>
		///		ViewModel del tablero
		/// </summary>
		public GameBoardViewModel GameBoardViewModel { get; }

		/// <summary>
		///		Variación
		/// </summary>
		public MovementVariationModel Variation { get; }

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementFigureModel Movement 
		{ 
			get { return _movement; }
			set { CheckProperty(ref _movement, value); }
		}

		/// <summary>
		///		Nombre de la pieza
		/// </summary>
		public PieceBaseModel.PieceType Piece 
		{ 
			get { return _piece; }
			set { CheckProperty(ref _piece, value); }
		}

		/// <summary>
		///		Color de la pieza
		/// </summary>
		public PieceBaseModel.PieceColor Color
		{
			get { return _color; }
			set { CheckProperty(ref _color, value); }
		}

		/// <summary>
		///		Texto del movimiento
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { CheckProperty(ref _text, value); }
		}

		/// <summary>
		///		Comando para selección del movimiento
		/// </summary>
		public Mvvm.BaseCommand SelectMovementCommand { get; }
	}
}