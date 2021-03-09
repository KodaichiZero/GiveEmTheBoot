using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using GiveEmTheBoot.Utils;
using UnityEngine;

namespace GiveEmTheBoot.Patches
{
    [HarmonyPatch(typeof(Character), "ApplyPushback")]
    public static class PushbackPatch
    {
        
        public static void Postfix(ref Player __instance, HitData hit)
        {

            if (hit == null || !hit.HaveAttacker())
            {
                //No Hitdata found, exiting processing
                return;
            }

            Character character = hit.GetAttacker();

            if(character == null || character.GetType() != typeof(Player))
            {
                //No processing needed if there is no attacker or it's not a player.
                return;
            }

            Player player = (Player)character;

            //Check to see if the player is attacking using their foot.
            if(player.m_currentAttack != null && player.m_currentAttack.m_attackAnimation == "unarmed_kick")
            {
                float pushForce = 0f;

                //Add in the player's unarmed level to the additional push force.
                if(player.GetSkills().m_skillData.ContainsKey(Skills.SkillType.Unarmed))
                {
                    pushForce += player.GetSkills().m_skillData.GetValueSafe(Skills.SkillType.Unarmed).m_level * 0.75F;
                }

                //Add the weight of the player's foot armor to the push force.
                List<ItemDrop.ItemData> equipList = new List<ItemDrop.ItemData>();
                player.GetInventory().GetWornItems(equipList);
                foreach(ItemDrop.ItemData item in equipList)
                {
                    if (item.GetTooltip().Contains("legs"))
                    {
                        pushForce += item.GetWeight() * 5F;
                        break;
                    }
                }

                //Apply additional poush force to the hit data.
                if(pushForce > 0f)
                {

                    Vector2 horizontalVector = new Vector2(player.GetMoveDir().x, player.GetMoveDir().z);
                    horizontalVector.Normalize();
                    horizontalVector *= pushForce;
                    Vector3 yeetVector = new Vector3(horizontalVector.x, pushForce * 0.375F, horizontalVector.y);

                    //Add a large amount of force with mass taken into account
                    __instance.GetComponent<Rigidbody>().AddForce(yeetVector, ForceMode.Impulse);

                    //Add a small amount of velocity regardless of mass
                    __instance.GetComponent<Rigidbody>().AddForce(yeetVector * 0.1F, ForceMode.VelocityChange);
                }
            }
        }
    }
}
