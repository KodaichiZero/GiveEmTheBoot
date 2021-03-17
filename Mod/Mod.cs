using BepInEx;
using GiveEmTheBoot.Patches;
using GiveEmTheBoot.Util;
using HarmonyLib;
using System.Linq;

namespace GiveEmTheBoot {
	[BepInPlugin("KZ.GiveEmTheBoot", "GiveEmTheBoot", "1.2")]
	public class Mod : BaseUnityPlugin {
		private static readonly Harmony harmony = new(typeof(Mod).GetCustomAttributes(typeof(BepInPlugin), false)
			.Cast<BepInPlugin>()
			.First()
			.GUID);

		private void Awake() {
			PushbackPatch.skillBonus = base.Config.Bind<float>("Adjustments", "Skill Bonus", 50F, "How much bonus knocbkack force to add at 100 Unarmed skill. Scales proportionally with different skill levels.");
			PushbackPatch.bootBonus = base.Config.Bind<float>("Adjustments", "Boot Bonus", 50F, "How much bonus knocbkack force to add when wearing boots weighing 15 pounds. Scales proportionally with different boots.");
			PushbackPatch.staggerBonus = base.Config.Bind<float>("Adjustments", "Stagger Bonus", 25F, "How much bonus knocbkack force to add when the enemy is staggered.");
			PushbackPatch.flatBonus = base.Config.Bind<float>("Adjustments", "Flat Bonus", 0F, "An unconditional knockback bonus to the kick for when you just want to kick a troll a mile away. Try setting this to 500.");
			PushbackPatch.weightFactor = base.Config.Bind<float>("Adjustments", "Weight Factor", 0.8F, "How much of the enemy's weight to factor into the knockback. 1 means heavy creatures will basically not move. 0 means heavy creatures will be knocked just as far as light creatures.");
			PushbackPatch.showDialog = base.Config.Bind<bool>("Features", "Show Dialog", true, "Whether or not to show a funny message when your kick connects to an enemy.");
			YeetText.dialogSelection = base.Config.Bind<string>("Features", "Dialog Selection", "PUNT|WHAM|BAM|YEET|SEEYA|BYE|HOOF|BOOT|GTFO|BEGONE", "Words that can appear if the Show Dialog option is enabled, separated by | character.");
			PlayerAttackInputPatch.kickHotkey = base.Config.Bind<string>("Features", "Kick Hotkey", "", "Customizable kick hotkey so you can kick while holding a weapon. If you want to use a mouse key, include a space: mouse 3, for example. Valid inputs: https://docs.unity3d.com/ScriptReference/KeyCode.html");
			harmony.PatchAll();
		}

		private void OnDestroy() {
			harmony.UnpatchSelf();
		}
	}
}
