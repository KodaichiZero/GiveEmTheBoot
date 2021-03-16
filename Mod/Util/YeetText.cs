using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace GiveEmTheBoot.Util {
	public class YeetText {

		//Different random words that might appear.
		internal static ConfigEntry<string> dialogSelection;

		//Create a floating block of special kick text.
		public static void AddYeetText(Vector3 pos) {
			if(dialogSelection.Value.Length > 0) {
				string[] selection = dialogSelection.Value.Split('|');
				string text = new string(selection[UnityEngine.Random.Range(0, selection.Length - 1)].ToCharArray());
				DamageText.WorldTextInstance worldTextInstance = new DamageText.WorldTextInstance();
				worldTextInstance.m_worldPos = pos;
				worldTextInstance.m_gui = UnityEngine.Object.Instantiate<GameObject>(DamageText.instance.m_worldTextBase, DamageText.instance.GetComponent<Transform>());
				worldTextInstance.m_textField = worldTextInstance.m_gui.GetComponent<Text>();
				DamageText.instance.m_worldTexts.Add(worldTextInstance);
				Color white = new Color(1f, 0.75f, 0f, 1f);
				worldTextInstance.m_textField.color = white;
				worldTextInstance.m_textField.fontSize = DamageText.instance.m_largeFontSize + 4;

				worldTextInstance.m_textField.text = text;
				worldTextInstance.m_timer = 0f;
			}
		}
	}
}
