using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Amplitude;
using Amplitude.Framework.Options;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.GameOptions;
using HumankindModTool;


namespace shakee.Humankind.BetterCombatDamage
{

    [BepInPlugin(PLUGIN_GUID, "Better Combat Damage", "1.1.0")]
    public class BetterCombatDamage : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "shakee.Humankind.BetterCombatDamage";
        void Awake()
        {
            var harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
        }

        public const string GameOptionGroup_LobbyPaceOptions = "GameOptionGroup_LobbyPaceOptions";   

#region GameoOptions
		public static GameOptionInfo CombatDamageType = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_DamageTable",
			DefaultValue = "0",
            editbleInGame = true,
			Title = "[CombatStrength] Combat Damage type",
			Description = "Sets how damage is calculated by units, airstrike, bombardments and so on.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Default",
                    Description = "Uses the default (vanilla) damage table and calculation.",
                    Value = "0"
                },
                new GameOptionStateInfo{
                    Title = "Modified Table",
                    Description = "Uses an extended damage table done by Bruno.",
                    Value = "1"
                },
                new GameOptionStateInfo{
                    Title = "Comparison Based",
                    Description = "Amount of damage is directly calculatd based on CS difference.",
                    Value = "2"
                },
			}
		};
        #endregion
    }


    [HarmonyPatch(typeof(OptionsManager<GameOptionDefinition>))]
	public class OptionsManager_Patch
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002914 File Offset: 0x00000B14
		[HarmonyPatch("Load")]
		[HarmonyPrefix]
		public static bool Load(OptionsManager<GameOptionDefinition> __instance)
		{
			Console.WriteLine("Adding GameOptions...");
		    GameOptionHelper.Initialize(new GameOptionInfo[]
			{
				BetterCombatDamage.CombatDamageType,

			});
			return true;
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
            if (GameOptionHelper.GetGameOption(BetterCombatDamage.CombatDamageType) == "2")
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
                __result.MinimumDamage = FixedPoint.Clamp(modAttack - 5, 0, 100);
                __result.MaximumDamage = FixedPoint.Clamp(modAttack + 5, 5, 100);
                return false;
            }
            else if (GameOptionHelper.GetGameOption(BetterCombatDamage.CombatDamageType) == "1")
            {
                float attackDifference = (int)FixedPoint.Clamp(attackerStrength - defenderStrength,-12,25);
                
 
                    if (attackDifference == 25)
                        __result.MinimumDamage = 100;
                    else
                        __result.MinimumDamage = FixedPoint.Clamp(35 + (int)Math.Floor(attackDifference / 2) * 5,5,100);                    
                    __result.MaximumDamage = FixedPoint.Clamp(40 + (int)Math.Ceiling(attackDifference / 2) * 5,5,100);
                    Console.WriteLine("MinClamp: " + (int)Math.Floor(attackDifference / 2) + " / Max Clamp: " + (int)Math.Ceiling(attackDifference / 2));
                
                Console.WriteLine("Difference: " + attackDifference.ToString() + " / Min:" + __result.MinimumDamage.ToString() + " / Max: " + __result.MaximumDamage.ToString());
                
                return false;   
            }
            else
                return true;            
            
        }        
    };

    
            
}