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
		private const int DustRange = 1000; /* maksimalna udaljenost za dust */

		#endregion

		private static void Main()
		{
			_loaded = false;
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
