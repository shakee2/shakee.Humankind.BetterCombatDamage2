using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using Amplitude;
using UnityEngine;
using Amplitude.Mercury;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Data.World;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Terrain;
using Amplitude.Collections;
using Amplitude.Framework.Simulation.DataStructures;
using Amplitude.Framework.Simulation;
using Amplitude.Mercury.Presentation;


namespace shakee.Humankind.BetterCombatDamage
{

    [HarmonyPatch(typeof(Battle))]
    public class ReturnFire
    {
		public static bool doReturnFire = false;

        [HarmonyPrefix]
        [HarmonyPatch("Attack_UnitPart")]
        public static bool Attack_UnitPart(Battle __instance, ref BattleUnit attacker, ref BattleUnit target, int layer)
		{
			
			Battle.BattleVisionMap visibilityMap = __instance.VisibilityMap();			
			BattleAttackFailureFlags flags = R.GetRangedAttackFailureFlags(__instance, target, target.GetBattlePosition2(layer).ToTileIndex2(), attacker.GetBattlePosition2(layer).ToTileIndex2(),true);
			bool ReturnFireSetting = HumankindModTool.GameOptionHelper.CheckGameOption(BetterCombatDamage.ReturnFire,"true");
			bool flagAttacker = attacker.IsRangedUnit2();
            bool bothRanged = flagAttacker && target.IsRangedUnit2();
			bool isVisible = visibilityMap.IsVisible2(attacker.GetBattlePosition2(layer).ToTileIndex2(),visibilityMap.GetEmpireBitsAt2(target.GetBattlePosition2(layer).ToTileIndex2()));
			bool inRange = target.Unit.GetPropertyValue("AttackRange") >= target.GetBattlePosition2(layer).GetDistance2(attacker.GetBattlePosition2(layer));
			bool targetable = isVisible && (BattleAttackFailureFlags.None == flags || target.Unit.IsIgnoreLineOfSight2());
			bool cannotRetaliate = target.HasDescriptor2(new StaticString("Tag_Unit_CannotReturnFire"));			
			bool canReturnFire = inRange && targetable && bothRanged && ReturnFireSetting;

			// Console.WriteLine("Return Fire: " + canReturnFire.ToString() + " | LOS: " + targetable.ToString() + " | Visible: " + isVisible.ToString());		

			FixedPoint value = attacker.Unit.GetPropertyValue("HealthRatio");
			FixedPoint value2 = target.Unit.GetPropertyValue("HealthRatio");
			attacker.SetIsInvincible2(isInvincible: true, layer);
			target.SetIsInvincible2(isInvincible: true, layer);

			R.SendBattleEvent(__instance, BattleEventType.PreAttack, attacker, target, layer);
			bool flag2 = !cannotRetaliate || target.CanRetaliate2(layer);
			PresentationChoreographyController_Patch.rangedRetaliate = flag2 && (canReturnFire || !flagAttacker);

			if (flag2 && (canReturnFire || !flagAttacker))
				doReturnFire = true;
			else
				doReturnFire = false;

			if (flag2 && (canReturnFire || !flagAttacker))
			{				
				R.SendBattleEvent(__instance, BattleEventType.PreRetaliate, target, attacker, layer);
			}
			if (layer == 1)
			{
				__instance.ForecastCombatStrengthModifiers(attacker, target);
			}
			R.SendBattleEvent(__instance, BattleEventType.Attack, attacker, target, layer);
			if (flag2 && (canReturnFire || !flagAttacker))
			{
				R.SendBattleEvent(__instance, BattleEventType.Retaliate, target, attacker, layer);
			}
			if (layer == 0 && BattleDebug.UseHealthRatio)
			{
				if (flag2 && (canReturnFire || !flagAttacker))
				{
					attacker.CombatHealthRatio((FixedPoint)BattleDebug.AttackerHealthRatio);
				}
				target.CombatHealthRatio((FixedPoint)BattleDebug.DefenderHealthRatio);
			}
			attacker.SetIsInvincible2(isInvincible: false, layer);
			target.SetIsInvincible2(isInvincible: false, layer);
			if (!attacker.IsInvincible)
				Console.WriteLine("Attack UnitPart");
			value -= attacker.Unit.GetPropertyValue("HealthRatio");
			value2 -= target.Unit.GetPropertyValue("HealthRatio");
			R.SendBattleEvent(__instance, BattleEventType.PostAttack, attacker, target, layer);
			if (flag2 && (canReturnFire || !flagAttacker))
			{
				R.SendBattleEvent(__instance, BattleEventType.PostRetaliate, target, attacker, layer);
			}
			if (layer == 0)
			{
                
				__instance.GiveExperienceToUnits(attacker.Unit, target, value2);
				__instance.GiveExperienceToUnits(target.Unit, attacker, value);

				attacker.AttackDamage(value);                
				target.AttackDamage(value2);
                FixedPoint attackerKilled = attacker.Unit.GetPropertyValue("UnitsKilled");
                FixedPoint targetKilled = target.Unit.GetPropertyValue("Unitskilled");
                FixedPoint attackerKilledBattle = attacker.Unit.GetPropertyValue("UnitsKilledInBattle");
                FixedPoint targetKilledBattle = target.Unit.GetPropertyValue("UnitsKilledInBattle");
				if (attacker.Unit.GetPropertyValue("HealthRatio") <= FixedPoint.Zero)
				{
                    Console.WriteLine("Attacker Killed");
					target.Unit.SetEditablePropertyValue("UnitsKilled", targetKilled++);
					target.Unit.SetEditablePropertyValue("UnitsKilledInBattle", targetKilledBattle++);
					target.RegisterKill2(layer);
					__instance.RaiseUnitKilledEvents2(attacker.Unit, target.Unit);
				}
				if (target.Unit.GetPropertyValue("HealthRatio") <= FixedPoint.Zero)
				{
                    Console.WriteLine("Defender Killed");
					attacker.Unit.SetEditablePropertyValue("UnitsKilled", attackerKilled++);
					attacker.Unit.SetEditablePropertyValue("UnitsKilledInBattle", attackerKilledBattle++);
					attacker.RegisterKill2(layer);
					__instance.RaiseUnitKilledEvents2(target.Unit, attacker.Unit);
				}
			}
            return false;
		}

    }
    [HarmonyPatch(typeof(PresentationChoreographyController))]
    public class PresentationChoreographyController_Patch
    {
		public static bool rangedRetaliate = false;
		public static bool secondTry = false;

        [HarmonyPrefix]
        [HarmonyPatch("CreateActionsForRangedFightSequence")]
        public static bool CreateActionsForRangedFightSequence(ref PresentationChoreographyController __instance, ref FightSequence fightSequence)
		{			
			if (HumankindModTool.GameOptionHelper.CheckGameOption(BetterCombatDamage.ReturnFire,"true"))
			{
				Console.WriteLine("Pawn Ranged Combat");
				if (fightSequence.AttackerBattleUnit.IsRangedUnit())
				{
					
					PresentationUnit presentationUnit = fightSequence.AttackerBattleUnit.PresentationUnit;
					PresentationUnit presentationUnit2 = fightSequence.DefenderBattleUnit.PresentationUnit;
					fightSequence.DefenderBattleUnit.FilterFighterSubPawns(ChoreographyCompatibilityFlag.Ranged);
					fightSequence.AttackerBattleUnit.FilterFighterSubPawns(ChoreographyCompatibilityFlag.Ranged);				
					Console.WriteLine("Defender & Attacker are Ranged");
					if (!BattleDebug.UseHealthRatio && fightSequence.AttackerPawnsToKill != 0)
					{
						Diagnostics.LogError($"{fightSequence.AttackerBattleUnit} vs {fightSequence.DefenderBattleUnit} ({fightSequence.AttackerPawnsToKill} vs {fightSequence.DefenderPawnsToKill})");
					}
					
					_ = fightSequence.DefenderBattleUnit.PresentationUnit;
					__instance.CreateAction<UnitActionWaitForPawnsAvailability>(ref fightSequence, ActionScope.Both);
					__instance.CreateAction<UnitActionPrepareRangedChoreography>(ref fightSequence, ActionScope.Both);
					__instance.CreateAction<UnitActionRangedFightSequenceDefender>(ref fightSequence, ActionScope.Both);
					__instance.CreateAction<UnitActionWaitIdle>(ref fightSequence, ActionScope.Both);
					__instance.CreateAction<UnitActionRangedPostFightSequence>(ref fightSequence, ActionScope.None);
					return false;
				}
			}            
			return true; 
		}
    }
}