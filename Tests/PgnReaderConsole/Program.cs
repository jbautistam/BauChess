using System;

namespace PgnReaderConsole
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// string fileName = System.IO.Path.Combine(DataPath, "chess-informant-sample.pgn");
			string fileName = System.IO.Path.Combine(DataPath, "111probs.pgn");

				// Obtiene el nombre de archivo de la consola
				if (args.Length == 1)
					fileName = args[0];
				// Realiza las pruebas
				if (string.IsNullOrWhiteSpace(fileName) || !System.IO.File.Exists(fileName))
					Console.WriteLine($"Error when load {fileName}");
				else
				{
					// Prueba del lector de movimientos
					new Tests.GamePgnParserTest().Process(fileName);
					// Espera que se pulse una tecla
					Console.WriteLine("Press any key to start test conversor ...");
					Console.ReadLine();
					// Prueba la conversión
					new Tests.GamePgnConversorTest().Process(fileName);
				}
				// Espera que se pulse una tecla
				Console.WriteLine("Press any key...");
				Console.ReadLine();
		}

		/// <summary>
		///		Directorio base de la aplicación
		/// </summary>
		private static string ApplicationPath
		{
			get 
			{ 
				string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase); 

					// Le quita la cabecera si es necesario
					if (path.StartsWith("file:\\", StringComparison.CurrentCultureIgnoreCase))
						path = path.Substring("file:\\".Length);
					// Devuelve el directorio
					return path;
			}
		}

		/// <summary>
		///		Directorio base de la aplicación
		/// </summary>
		private static string DataPath
		{
			get { return System.IO.Path.Combine(ApplicationPath, "Data"); }
		}
	}
}
