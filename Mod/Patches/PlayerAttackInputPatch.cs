using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;

namespace GiveEmTheBoot.Patches {
	[HarmonyPatch(typeof(Player), "PlayerAttackInput")]
	public static class PlayerAttackInputPatch {

		//Loaded config values for adjusting the mod as you see fit.
		public static ConfigEntry<string> kickHotkey;
		public static bool mustKick = false;
		public static bool invalidKey = false;

		//In this method we check when pushback is being applied to an enemy, and if we're kicking them, we add force.
		public static void Postfix(Player __instance, float dt) {

			if(__instance.InPlaceMode()) {
				return;
			}


			//Check the kick hotkey first.
			bool kickInput = false;

			try {
				if(kickHotkey.Value.Length > 0 && Input.GetKey(kickHotkey.Value.ToLower())) {
					kickInput = true;
				}
			} catch(Exception e) {
				if(!invalidKey) {
					Debug.Log("GiveEmTheBoot ERROR: You bound an invalid key code that Unity cannot recognize. Check the config file.");
					invalidKey = true;
				}
			}

			//Go ahead with the kick
			if(kickInput) {
				mustKick = true;
				__instance.StartAttack(null, true);

			}

			mustKick = false;
		}
	}
}
