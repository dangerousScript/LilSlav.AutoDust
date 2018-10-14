using System;
using System.Drawing;
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
		private const string ScriptVersion = "1.0.2.3";		// za ispis

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
				if (!Game.IsInGame || _myHero == null) return;		// izlaz ako nismo jos usli u game
				
				_loaded = true;
				PrintSuccess($">>> LilSlav.AutoDust Loaded v{ScriptVersion} <<<");	// ispis u consolu ako je uspesno pokrenuta
			}

			// ako nije u game-u onda unload
			if (!Game.IsInGame || _myHero == null)
			{
				_loaded = false;
				PrintInfo($">>> LilSlav.AutoDust unLoaded <<<");
				return;
			}

			// provera da li je game pauziran
			if (Game.IsPaused || !Utils.SleepCheck("delay")) return;

			// provera da li smo ucitali player-a
			// ili je nas player posmatrac i onda return
			if (_player == null || _player.Team == Team.Observer) return;

			// provera da li imamo dust i torbici
			var itemDust = _myHero.FindItem("item_dust");
			if (itemDust == null || itemDust.CanBeCasted() || _myHero.IsInvisible()) return;

			// pravimo listu enemy-ja
			// da nije iluzija
			// da je suprotan tim od naseg heroja
			// da je enemy ziv
			// da je visible
			// i da nam je u range-u
			var listEnemy = ObjectManager.GetEntities<Hero>().Where(
				enemy =>	!enemy.IsIllusion &&
							enemy.Team != _player.Team &&
							enemy.IsAlive &&
							enemy.IsVisible &&
							_myHero.Distance2D(enemy) <= DustRange);
			
			// prolazimo kroz listu za svakog heroja i proveravamo sve situacije
			foreach (var enemy in listEnemy)
			{
				// dodati logiku
				//if (enemy.Modifiers.Any(
				//	x => (x.Name == "modifier_clinkz_wind_walk")))
				//{
				//	itemDust.UseAbility();
				//	Utils.Sleep(250, "delay");
				//}
				{
					if (enemy.Modifiers.Any(
						x =>
							(x.Name == "modifier_bounty_hunter_wind_walk" ||
							 x.Name == "modifier_riki_permanent_invisibility" ||
							 x.Name == "modifier_mirana_moonlight_shadow" || x.Name == "modifier_treant_natures_guise" ||
							 x.Name == "modifier_weaver_shukuchi" ||
							 x.Name == "modifier_broodmother_spin_web_invisible_applier" ||
							 x.Name == "modifier_item_invisibility_edge_windwalk" || x.Name == "modifier_rune_invis" ||
							 x.Name == "modifier_clinkz_wind_walk" || x.Name == "modifier_item_shadow_amulet_fade" ||
							 x.Name == "modifier_bounty_hunter_track" || x.Name == "modifier_bloodseeker_thirst_vision" ||
							 x.Name == "modifier_slardar_amplify_damage" || x.Name == "modifier_item_dustofappearance") ||
							x.Name == "modifier_invoker_ghost_walk_enemy"))
					{
						itemDust.UseAbility();
						Utils.Sleep(250, "delay");
					}

					if ((enemy.Name == ("npc_dota_hero_templar_assassin") || enemy.Name == ("npc_dota_hero_sand_king")) &&
						(enemy.Health / enemy.MaximumHealth < 0.3))
					{
						itemDust.UseAbility();
						Utils.Sleep(250, "delay");

					}
					if (enemy.Name != ("npc_dota_hero_nyx_assassin") || !enemy.Spellbook.Spell4.CanBeCasted()) continue;
					itemDust.UseAbility();
					Utils.Sleep(250, "delay");
				}
			}

		}

		#region pomocno

		public static void PrintInfo(string text, params object[] arguments)
		{
			PrintEncolored(text, ConsoleColor.Cyan, arguments);
		}

		public static void PrintSuccess(string text, params object[] arguments)
		{
			PrintEncolored(text, ConsoleColor.Magenta, arguments);
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
