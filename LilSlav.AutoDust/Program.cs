using System;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;

namespace LilSlav.AutoDust
{
	class Program
	{
		#region globalne_varijable

		private static bool _loaded;
		private static Hero _myHero;
		private static Player _player;
		private const int DustRange = 1000;					/* maksimalna udaljenost za dust */
		private const string ScriptVersion = "1.0.0.1";		// za ispis

		#endregion

		private static void Main()
		{
			Game.OnUpdate += Game_OnUpdate;
			_loaded = false;
		}

		private static void Game_OnUpdate(EventArgs args)
		{
			// ako je loaded false, onda ukljucujemo skriptu
			if (!_loaded)
			{
				_myHero = ObjectManager.LocalHero;          // ucitamo naseg champ
				_player = ObjectManager.LocalPlayer;
				
				// proveravamo da li se game ucitao
				if (!Game.IsInGame || _myHero == null)
				{
					return;		// izlaz ako nismo jos usli u game
				}

				_loaded = true;
				PrintSuccess($"> LilSlav.AutoDust Loaded v{ScriptVersion}");	// ispis u consolu ako je uspesno pokrenuta
			}
		}

		#region pomocno

		public static void PrintInfo(string text, params object[] arguments)
		{
			PrintEncolored(text, ConsoleColor.White, arguments);
		}

		public static void PrintSuccess(string text, params object[] arguments)
		{
			PrintEncolored(text, ConsoleColor.Green, arguments);
		}

		public static void PrintError(string text, params object[] arguments)
		{
			PrintEncolored(text, ConsoleColor.Red, arguments);
		}

		public static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
		{
			var _clr = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(text, arguments);
			Console.ForegroundColor = _clr;
		}

		#endregion
	}
}
