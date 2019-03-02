using System;

namespace Bau.Libraries.LibChessGame.ViewModels.Pgn
{
	/// <summary>
	///		ViewModel para los elementos de <see cref="PgnGameInfoTagListViewModel"/>
	/// </summary>
	public class PgnGameInfoTagItemListViewModel : Mvvm.BaseObservableObject
	{
		// Variables privadas
		private string _header, _text;

		public PgnGameInfoTagItemListViewModel(string header, string text)
		{
			Header = header;
			Text = text;
		}

		/// <summary>
		///		Cabecera
		/// </summary>
		public string Header
		{
			get { return _header; }
			set { CheckProperty(ref _header, value); }
		}

		/// <summary>
		///		Texto
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { CheckProperty(ref _text, value); }
		}
	}
}
