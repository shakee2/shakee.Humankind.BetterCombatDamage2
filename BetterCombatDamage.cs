using System;
using BepInEx;
using HarmonyLib;
using Amplitude;
using Amplitude.Framework.Options;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.GameOptions;
using HumankindModTool;


namespace shakee.Humankind.BetterCombatDamage
{

    [BepInPlugin(PLUGIN_GUID, "Better Combat Damage", "1.2.0")]
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
            editbleInGame = false,
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
                    Description = "Amount of damage is directly calculated based on CS difference.",
                    Value = "2"
                },
			}
		};
        public static GameOptionInfo CombatDamageModifier = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_DamageTableModifier",
			DefaultValue = "0",
            editbleInGame = false,
			Title = "[CombatStrength] Combat Damage Baseline modifier",
			Description = "This will move the damage of the baseline 0 cs difference to a value more in line with vanilla. This will affect how fast you get to the lowest or highest damage values.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",	
            States = 
			{
                new GameOptionStateInfo{
                    Title = "Default",
                    Description = "No change.",
                    Value = "0"
                },
                new GameOptionStateInfo{
                    Title = "Close to Vanilla",
                    Description = "Will move the damage into the vanilla baseline damage. Reduced avg. damage by -7 (my) / -15 (brunos).",
                    Value = "1"
                },
			}
		
		};
        #endregion
    }


    [HarmonyPatch(typeof(OptionsManager<GameOptionDefinition>))]
	public class OptionsManager_Patch
	{
		
		[HarmonyPatch("Load")]
		[HarmonyPrefix]
		public static bool Load(OptionsManager<GameOptionDefinition> __instance)
		{
			Console.WriteLine("Adding GameOptions...");
		    GameOptionHelper.Initialize(new GameOptionInfo[]
			{
				BetterCombatDamage.CombatDamageType,
                BetterCombatDamage.CombatDamageModifier,

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
                int damageModifier = 0;
                if (GameOptionHelper.CheckGameOption(BetterCombatDamage.CombatDamageModifier,"1"))
                    damageModifier = -7;                

                FixedPoint sumCS = attackerStrength + defenderStrength;
                FixedPoint baseAttack = attackerStrength / sumCS * (56 + damageModifier);
                float multi;
                if ((attackerStrength - defenderStrength) < 0) {
                    multi = 0.70f;
                }
                else {
                    multi = 1f;
                }
                
                FixedPoint modAttack = FixedPoint.Round(FixedPoint.Clamp(baseAttack + (attackerStrength - defenderStrength) * multi, 0, 100) + FixedPoint.Clamp(attackerStrength - defenderStrength - 10, 0, 100) * 2);
                //FixedPoint modAttack = FixedPoint.Clamp(baseAttack * (1 + 0.05f * (attackerStrength - defenderStrength)) * multi, 0, 100) + FixedPoint.Clamp(attackerStrength - defenderStrength - 10, 0, 100) * 2;
                __result.MinimumDamage = FixedPoint.Clamp(modAttack - 5, 0, 100);
                __result.MaximumDamage = FixedPoint.Clamp(modAttack + 5, 5, 100);
                return false;
            }
            else if (GameOptionHelper.GetGameOption(BetterCombatDamage.CombatDamageType) == "1")
            {
                int damageModifier = 0;
                if (GameOptionHelper.CheckGameOption(BetterCombatDamage.CombatDamageModifier,"1"))
                    damageModifier = 6;

                float attackDifference = (int)FixedPoint.Clamp(attackerStrength - defenderStrength - damageModifier,-12,25);                
 
                    if (attackDifference == 25)
                        __result.MinimumDamage = 100;
                    else
                        __result.MinimumDamage = FixedPoint.Clamp(35 + (int)Math.Floor(attackDifference / 2) * 5,5,100);    

                    __result.MaximumDamage = FixedPoint.Clamp(40 + (int)Math.Ceiling(attackDifference / 2) * 5,5,100);
                
                return false;   
            }
            else
                return true;            
            
        }        
    };

    
            
}