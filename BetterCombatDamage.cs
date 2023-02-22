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

    [BepInPlugin(PLUGIN_GUID, "Better Combat Damage", "2.0.3")]
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
			Description = "Sets how damage is calculated by units, airstrike, bombardments and so on. Also offers additional options.",
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
                    Title = "Mod.Table (Vanilla Baseline)",
                    Description = "Uses an extended damage table done by Bruno. Baseline damage (0 cs difference) is inside vanilla damage range.",
                    Value = "3"
                },
                new GameOptionStateInfo{
                    Title = "Comparison Based",
                    Description = "Amount of damage is directly calculated based on CS difference.",
                    Value = "2"
                },
                new GameOptionStateInfo{
                    Title = "Comp. Based (Vanilla Baseline)",
                    Description = "Amount of damage is directly calculated based on CS difference. Baseline damage (0 cs difference) is inside vanilla damage range.",
                    Value = "4"
                },

			}
		};
        public static GameOptionInfo ReturnFire = new GameOptionInfo
		{            
			ControlType = 0,
			Key = "GameOption_shakee_ReturnFire",
			DefaultValue = "false",
            editbleInGame = true,
			Title = "[CombatStrength] Enable Return Fire",
			Description = "Enabling this will allow ranged units to return fire when they get attacked by other ranged units.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Disabled",
                    Description = "Disabled -> Vanilla behavior.",
                    Value = "false"
                },
                new GameOptionStateInfo{
                    Title = "Enabled",
                    Description = "Enabled.",
                    Value = "true"
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
                BetterCombatDamage.ReturnFire,
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
            int damageType = int.Parse(GameOptionHelper.GetGameOption(BetterCombatDamage.CombatDamageType));
            bool comparison = damageType == 2 || damageType == 4;
            bool extendedTable = damageType == 1 || damageType == 3;
            if (comparison)
            {         
                int damageModifier = 0;
                if (damageType == 4)
                    damageModifier = 14;                

                FixedPoint sumCS = attackerStrength + defenderStrength;
                FixedPoint baseAttack = attackerStrength / sumCS * (56 - damageModifier);
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
            else if (extendedTable)
            {
                int damageModifier = 0;
                if (damageType == 3)
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