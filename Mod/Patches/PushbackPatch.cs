using BepInEx.Configuration;
using GiveEmTheBoot.Util;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace GiveEmTheBoot.Patches {
	[HarmonyPatch(typeof(Character), "ApplyPushback")]
	public static class PushbackPatch {

		//Loaded config values for adjusting the mod as you see fit.
		public static ConfigEntry<float> skillBonus;
		internal static ConfigEntry<float> bootBonus;
		internal static ConfigEntry<float> staggerBonus;
		internal static ConfigEntry<float> flatBonus;
		internal static ConfigEntry<float> weightFactor;
		internal static ConfigEntry<bool> showDialog;

		//In this method we check when pushback is being applied to an enemy, and if we're kicking them, we add force.
		public static void Prefix(ref Character __instance, HitData hit) {

			//Make sure the hit is an attacking player.
			if(!hit.HaveAttacker() || !(hit.GetAttacker() is Player)) {
				return;
			}

			Player player = (Player)hit.GetAttacker();

			//Check to see if the player is attacking using their foot.
			if(player.m_currentAttack != null && player.m_currentAttack.m_attackAnimation == "unarmed_kick") {
				float pushForce = 0f;

				//Add in the player's unarmed level to the additional push force, max of 75F extra force at level 100.
				if(player.GetSkills().m_skillData.ContainsKey(Skills.SkillType.Unarmed)) {
					float level = player.GetSkills().m_skillData.GetValueSafe(Skills.SkillType.Unarmed).m_level;
					pushForce += (level / 100F) * skillBonus.Value;
				}

				//Add the weight of the player's foot armor to the push force, max bonus of 50F with 15 pound boots.
				List<ItemDrop.ItemData> equipList = new List<ItemDrop.ItemData>();
				player.GetInventory().GetWornItems(equipList);
				foreach(ItemDrop.ItemData item in equipList) {
					if(item.GetTooltip().Contains("legs")) {
						pushForce += (item.GetWeight() / 15F) * bootBonus.Value;
						break;
					}
				}

				//Also add a flat 50F bonus if the enemy is staggered.
				if(__instance.IsStaggering()) {
					pushForce += staggerBonus.Value;
				}

				//Finally, add a flat bonus if one is configured.
				pushForce += flatBonus.Value;

				//Apply additional poush force to the hit data.
				if(pushForce > 0f) {

					//Make the funny text.
					if(showDialog.Value) {
						YeetText.AddYeetText(__instance.GetTopPoint());
					}

					Vector2 horizVector = new Vector2(player.GetMoveDir().x, player.GetMoveDir().z);
					horizVector.Normalize();
					horizVector *= pushForce;
					Vector3 fullVector = new Vector3(horizVector.x, pushForce * 0.375F, horizVector.y);

					float weight;
					if(weightFactor.Value > 1F) {
						weight = 1F;
					} else if(weightFactor.Value < 0F) {
						weight = 0F;
					} else {
						weight = weightFactor.Value;
					}

					//Add a large amount of force with mass taken into account
					__instance.GetComponent<Rigidbody>().AddForce(fullVector * weight, ForceMode.Impulse);

					//Add a small amount of velocity regardless of mass
					__instance.GetComponent<Rigidbody>().AddForce(fullVector * (1.0F - weight), ForceMode.VelocityChange);

				}
			}
		}
	}
}
