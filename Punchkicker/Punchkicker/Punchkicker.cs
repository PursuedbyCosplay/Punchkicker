using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using static ItemDrop;
using static Skills;

namespace Punchkicker
{
    [BepInPlugin("pursuedbycosplay.Punchkicker", "Punchkicker", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class ValheimMod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("pursuedbycosplay.Punchkicker");

        void Awake()
        {
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
        public static class ModifyCurrentWeapon
        {

            private static ItemData Postfix(ItemData __weapon, ref Character __instance)
            {


                if (__weapon != null && __weapon.m_shared.m_name == "Unarmed")
                {
                    Player val = (Player)__instance;
                    List<ItemData> list = new List<ItemData>();
                    val.GetInventory().GetWornItems(list);
                    float legweight = 1f;
                    foreach (ItemData item in list)
                    {
                        if (item.GetTooltip().Contains("legs"))
                        {
                            legweight = item.GetWeight();
                            break;
                        }
                    }

                    __weapon.m_shared.m_damages.m_blunt = ((Character)val).GetSkillFactor((SkillType)11) * 50 + (2*legweight);
                    if (__weapon.m_shared.m_damages.m_blunt <= 2f)
                    {
                        __weapon.m_shared.m_damages.m_blunt = 2f;
                    }
                }
                return __weapon;
            }
        }
    }
}
