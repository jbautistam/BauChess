using System;
using System.Collections.Generic;

using Bau.Libraries.LibPgnReader.Parsers.Tokens;

namespace Bau.Libraries.LibPgnReader.Parsers
{
	/// <summary>
	///		Lector de tokens
	/// </summary>
	internal class TokenParser : IDisposable
	{
		/// <summary>
		///		Abre un archivo
		/// </summary>
		internal void Open(string fileName, System.Text.Encoding encoding = null)
		{
			// Cierra el archivo abierto previamente
			Close();
			// Abre el archivo
			CharReader = new CharFileReader();
			CharReader.Open(fileName, encoding);
		}

		/// <summary>
		///		Lee los tokens del archivo
		/// </summary>
		internal IEnumerable<TokenModel> Read()
		{
			string content = string.Empty;
			TokenModel.TokenType tokenReaded = TokenModel.TokenType.Unknown;
			bool endToken = false, specialChar = false;

				// Lee los caracteres y crea los tokens
				foreach ((CharFileReader.CharType type, char character) readed in CharReader.Read())
				{
					// Añade el carácter al contenido y cierra el token si es necesario
					switch (readed.type)
					{
						case CharFileReader.CharType.Character:
								// Lee el carácter
								if (readed.character == ' ' || readed.character == '\t' || readed.character == '\r' || readed.character == '\n')
									endToken = true;
								else // Trata los caracteres especiales
									switch (readed.character)
									{
										case '[':
												tokenReaded = TokenModel.TokenType.StartTag;
											break;
										case ']':
										case '}':
										case ')':
										case '>':
												specialChar = true;
											break;
										case '{':
												tokenReaded = TokenModel.TokenType.StartComment;
											break;
										case ';':
												tokenReaded = TokenModel.TokenType.StartCommentToEoL;
											break;
										case '(':
												tokenReaded = TokenModel.TokenType.StartRecursiveVariation;
											break;
										case '<':
												tokenReaded = TokenModel.TokenType.StartExpansion;
											break;
										case '"':
												specialChar = true;
											break;
										default:
												content += readed.character;
											break;
									}
								// Si se ha leído un carácter especial, se marca el final del token
								if (tokenReaded != TokenModel.TokenType.Unknown)
									endToken = true;
							break;
						case CharFileReader.CharType.EoL:
						case CharFileReader.CharType.EoF:
								specialChar = true;
							break;
					}
					// Devuelve el token
					if (specialChar)
					{
						// Devuelve el token que teníamos hasta ahora
						if (tokenReaded != TokenModel.TokenType.Unknown || !string.IsNullOrEmpty(content))
							yield return new TokenModel(tokenReaded, content);
						// Devuelve un token con el carácter especial
						switch (readed.type)
						{
							case CharFileReader.CharType.EoL:
									yield return new TokenModel(TokenModel.TokenType.EoL, string.Empty);
								break;
							case CharFileReader.CharType.EoF:
									yield return new TokenModel(TokenModel.TokenType.EoF, string.Empty);
								break;
							default:
									switch (readed.character)
									{
										case '"':
												yield return new TokenModel(TokenModel.TokenType.Quote, string.Empty);
											break;
										case '}':
												yield return new TokenModel(TokenModel.TokenType.EndComment, string.Empty);
											break;
										case ']':
												yield return new TokenModel(TokenModel.TokenType.EndTag, string.Empty);
											break;
										case ')':
												yield return new TokenModel(TokenModel.TokenType.EndRecursiveVariation, string.Empty);
											break;
										case '>':
												yield return new TokenModel(TokenModel.TokenType.EndExpansion, string.Empty);
											break;
									}
								break;
						}
					}
					// Si hemos llegado al final del token, lo devuelve
					if (endToken && (tokenReaded != TokenModel.TokenType.Unknown || !string.IsNullOrEmpty(content)))
						yield return new TokenModel(tokenReaded, content);
					// Inicializa los datos si es necesario
					if (specialChar || endToken)
					{
						tokenReaded = TokenModel.TokenType.Unknown;
						content = string.Empty;
						endToken = false;
						specialChar = false;
					}
				}
		}

		/// <summary>
		///		Cierra el archivo
		/// </summary>
		internal void Close()
		{
			if (CharReader != null)
			{
				CharReader.Close();
				CharReader = null;
			}
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				// Cierra el archivo
				if (disposing)
					Close();
				// Indica que se ha liberado la memoria
				Disposed = true;
			}
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Indica si se ha liberado el archivo
		/// </summary>
		internal bool Disposed { get; private set; }

		/// <summary>
		///		Lector de caracteres
		/// </summary>
		private CharFileReader CharReader { get; set; }
	}
}