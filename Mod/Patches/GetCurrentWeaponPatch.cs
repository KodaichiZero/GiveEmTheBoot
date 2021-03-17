using HarmonyLib;

namespace GiveEmTheBoot.Patches {

	[HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
	public static class GetCurrentWeaponPatch {

		//We override the get current weapon method in order to kick while holding a weapon.
		public static bool Prefix(ref Humanoid __instance, ref ItemDrop.ItemData __result) {

			if(__instance == Player.m_localPlayer && PlayerAttackInputPatch.mustKick) {
				PlayerAttackInputPatch.mustKick = false;
				__result = __instance.m_unarmedWeapon.m_itemData;
				return false;

			}

			return true;
		}
	}
}
