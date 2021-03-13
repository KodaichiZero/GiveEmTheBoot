using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GiveEmTheBoot.Util {
	public class YeetText {

		//Different random words that might appear.
		private static readonly string[] exclamation = {"PUNT", "WHAM", "BAM", "YEET", "SEEYA", "BYE", "HOOF", "BOOT", "GTFO", "BEGONE"};

		//Create a floating block of special kick text.
		public static void AddYeetText(Vector3 pos) {
			DamageText.WorldTextInstance worldTextInstance = new DamageText.WorldTextInstance();
			worldTextInstance.m_worldPos = pos;
			worldTextInstance.m_gui = UnityEngine.Object.Instantiate<GameObject>(DamageText.instance.m_worldTextBase, DamageText.instance.GetComponent<Transform>());
			worldTextInstance.m_textField = worldTextInstance.m_gui.GetComponent<Text>();
			DamageText.instance.m_worldTexts.Add(worldTextInstance);
			Color white = new Color(1f, 0.75f, 0f, 1f);
			worldTextInstance.m_textField.color = white;
			worldTextInstance.m_textField.fontSize = DamageText.instance.m_largeFontSize + 4;
			string text = new string(exclamation[UnityEngine.Random.Range(0, exclamation.Length - 1)].ToCharArray());
			worldTextInstance.m_textField.text = text;
			worldTextInstance.m_timer = 0f;
		}
	}
}
