using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Amplitude;
using Amplitude.Mercury.Simulation;


namespace shakee.Humankind.BetterCombatDamage
{

    [BepInPlugin(PLUGIN_GUID, "Better Combat Damage", "1.0.0.0")]
    public class BetterCombatDamage : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "shakee.Humankind.BetterCombatDamage";
        void Awake()
        {
            var harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

//Add Harmony patches here

    //This tells Harmony which class you are patching
    [HarmonyPatch(typeof(BattleAbilityHelper))]
    public class BattleAbilityHelper_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("GetDamages")]

        public static bool GetDamages(ref Damage __result, FixedPoint attackerStrength, FixedPoint defenderStrength)    
        {

            FixedPoint sumCS = attackerStrength + defenderStrength;
            FixedPoint baseAttack = attackerStrength / sumCS * 56;
            float multi;
            if ((attackerStrength - defenderStrength) < 0) {
                multi = 0.70f;
            }
            else {
                multi = 1f;
            }
            FixedPoint modAttack = FixedPoint.Clamp(baseAttack + (attackerStrength - defenderStrength) * multi, 0, 100) + FixedPoint.Clamp(attackerStrength - defenderStrength - 10, 0, 100) * 2;
            //FixedPoint modAttack = FixedPoint.Clamp(baseAttack * (1 + 0.05f * (attackerStrength - defenderStrength)) * multi, 0, 100) + FixedPoint.Clamp(attackerStrength - defenderStrength - 10, 0, 100) * 2;
            __result.MinimumDamage = FixedPoint.Clamp(modAttack - 5, 2, 100);
            __result.MaximumDamage = FixedPoint.Clamp(modAttack + 5, 7, 100);
            return false;
            
        }
    };
            
}

