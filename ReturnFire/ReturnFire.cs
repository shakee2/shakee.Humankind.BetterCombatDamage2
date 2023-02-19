using System;
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

			bool flagAttacker = attacker.IsRangedUnit2();
            bool bothRanged = flagAttacker && target.IsRangedUnit2();
			bool isVisible = visibilityMap.IsVisible2(attacker.GetBattlePosition2(layer).ToTileIndex2(),visibilityMap.GetEmpireBitsAt2(target.GetBattlePosition2(layer).ToTileIndex2()));
			bool inRange = target.Unit.GetPropertyValue("AttackRange") >= target.GetBattlePosition2(layer).GetDistance2(attacker.GetBattlePosition2(layer));
			bool targetable = isVisible && (BattleAttackFailureFlags.None == flags || target.Unit.IsIgnoreLineOfSight2());
			bool cannotRetaliate = target.HasDescriptor2(new StaticString("Tag_Unit_CannotReturnFire"));			
			bool canReturnFire = inRange && targetable && bothRanged;
			
			// Console.WriteLine("Return Fire: " + canReturnFire.ToString() + " | LOS: " + targetable.ToString() + " | Visible: " + isVisible.ToString());		

			FixedPoint value = attacker.Unit.GetPropertyValue("HealthRatio");
			FixedPoint value2 = target.Unit.GetPropertyValue("HealthRatio");
			attacker.SetIsInvincible2(isInvincible: true, layer);
			target.SetIsInvincible2(isInvincible: true, layer);

			R.SendBattleEvent(__instance, BattleEventType.PreAttack, attacker, target, layer);
			bool flag2 = !cannotRetaliate || target.CanRetaliate2(layer);
			PresentationChoreographyController_Patch.rangedRetaliate = flag2 && (canReturnFire || !flagAttacker);

			if (flag2 && (canReturnFire || !flagAttacker))
			{
				doReturnFire = true;
				R.SendBattleEvent(__instance, BattleEventType.PreRetaliate, target, attacker, layer);
			}
			else
				doReturnFire = false;
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
			
            Console.WriteLine("Pawn Ranged Combat");
            if (fightSequence.DefenderBattleUnit.IsRangedUnit() && fightSequence.AttackerBattleUnit.IsRangedUnit())
            {
				
                PresentationUnit presentationUnit = fightSequence.AttackerBattleUnit.PresentationUnit;
                PresentationUnit presentationUnit2 = fightSequence.DefenderBattleUnit.PresentationUnit;
                bool flag = fightSequence.AttackerPawnsToKill >= presentationUnit.Pawns.Count;
			    bool flag2 = fightSequence.DefenderPawnsToKill >= presentationUnit2.Pawns.Count;
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
                // __instance.CreateAction<UnitActionTriggerAttack>(ref fightSequence, ActionScope.Attacker);
                // __instance.CreateAction<UnitActionWaitIdle>(ref fightSequence, ActionScope.Attacker);
                // __instance.CreateAction<UnitActionTriggerAttack>(ref fightSequence, ActionScope.Defender);
                __instance.CreateAction<UnitActionRangedFightSequence>(ref fightSequence, ActionScope.Attacker);
                // __instance.CreateAction<UnitActionWaitIdle>(ref fightSequence, ActionScope.Both);
				// if (rangedRetaliate)
				// {
                // 	Console.WriteLine("Calling Defender Sequence");
				// 	__instance.CreateAction<UnitActionRangedFightSequence>(ref fightSequence, ActionScope.Attacker);
                // }
				//__instance.CreateAction<UnitActionMeleeFightSequence>(ref fightSequence, ActionScope.Attacker);
			    //__instance.CreateAction<UnitActionMeleePostFightSequence>(ref fightSequence, ActionScope.Attacker);
                __instance.CreateAction<UnitActionRangedPostFightSequence>(ref fightSequence, ActionScope.None);
				return false;
            }
			return true;
		}

        [HarmonyPostfix]
        [HarmonyPatch("ShouldChoreographyContinue")]
        public static void ShouldChoreographyContinue(bool __result, PresentationUnitsFightData fightData, PresentationUnit attackerUnit, PresentationUnit defenderUnit, bool attackerIsCavalry = false)
        {
            if (!fightData.HasAttackerReturnStarted && attackerUnit != null && !attackerIsCavalry && defenderUnit != null && defenderUnit.PresentationEntityHolder.IsRangedUnit() && attackerUnit.PresentationEntityHolder.IsRangedUnit())
            {
                //__result = true;
            }

        }
    }

	[HarmonyPatch(typeof(UnitAction))]
    public class UnitAction_Patch
	{
				
		[HarmonyPrefix]
        [HarmonyPatch("CreateUnitAction")]
		public static bool CreateUnitAction(UnitAction __instance, ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction = true)
		{

			Console.WriteLine("Unit Action Called: ");
			return true;
		}
	}
# region patch UnitActionRangedFightSequence

	[HarmonyPatch(typeof(UnitActionRangedFightSequence))]
    public class UnitActionRangedFightSequence_Patch : UnitAction
	{
		public static int attackersToKill = 0;
		public static FightSequence fight;
		//public static List<PresentationPawn> defenderAvailablePawns;
		//public static List<PresentationPawn> attackerAvailablePawns;

		[HarmonyPrefix]
        [HarmonyPatch("CreateUnitAction")]
		public static bool CreateUnitAction(UnitActionRangedFightSequence __instance, ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction = true)
		{
			if (!ReturnFire.doReturnFire)
				return true;
			Console.WriteLine("Return Fire: " + PresentationChoreographyController_Patch.rangedRetaliate + " | SecondTry: " + PresentationChoreographyController_Patch.secondTry);

/* 				var val = new UnitActionRangedFightSequence_Patch();
			    val.BaseCreateUnit(ref fightSequence, actionScope, blockingAction);
				// typeof(UnitAction).GetMethod("CreateUnitAction").InvokeNotOverride(__instance,new object[]
				// {				
				// 	fightSequence,
				// 	actionScope,
				// 	blockingAction,
				// });

				//var obj = new UnitActionRangedFightSequence();
				//obj.CreateUnitAction(ref fightSequence, actionScope, blockingAction); 
				//var method = typeof(UnitActionRangedFightSequence).GetMethod("CreateUnitAction", AccessTools.all);

				//var ftn = method.MethodHandle.GetFunctionPointer();
				//Console.WriteLine(method.ToString() + " | " + ftn.ToString());
				//var func = (Func<string>)Activator.CreateInstance(typeof(Func<string>), __instance, ftn);
				//R.CreateUnitAction2(ftn, ref fightSequence, actionScope, blockingAction);

				//func.Invoke(ref fightSequence, actionScope, blockingAction);
				// Console.WriteLine(func());
				Console.WriteLine("Retaliation Triggered Attackers To Kill: " + fightSequence.AttackerPawnsToKill + " | Defenders to Kill: " + fightSequence.DefenderPawnsToKill);
				//typeof(UnitActionRangedFightSequence).GetMethod("CreateUnitAction").InvokeNotOverride(__instance, new object[] {fightSequence, actionScope, blockingAction});
				//__instance.CreateUnitAction(ref fightSequence, actionScope, blockingAction);
				__instance.defenderBattleUnit(fightSequence.DefenderBattleUnit);
				__instance.attackerBattleUnit(fightSequence.AttackerBattleUnit);
				__instance.defenderUnit(__instance.defenderBattleUnit().PresentationUnit);
				__instance.attackerUnit(__instance.attackerBattleUnit().PresentationUnit);
				__instance.defendersToKill(fightSequence.DefenderPawnsToKill);	
				
				__instance.defenderAvailablePawns(new List<PresentationPawn>(__instance.defenderUnit().Pawns));
				__instance.attackerAvailablePawns(new List<PresentationPawn>(__instance.attackerUnit().Pawns));
				Console.WriteLine("Check End");
				PresentationChoreographyController_Patch.secondTry = false; */
				//defenderAvailablePawns = __instance.defenderAvailablePawns();
				//attackerAvailablePawns = __instance.attackerAvailablePawns();
				attackersToKill = fightSequence.AttackerPawnsToKill;
				fight = fightSequence;
				return true;				
		}
		public void BaseCreateUnit(ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction)
		{
			base.CreateUnitAction(ref fightSequence, actionScope, blockingAction);
		}
		protected override void OnTimedOut()
		{
			battle.OnTimeOut();
			if (!PresentationChoreographyAction.EndActionOnFailSafeLifeTime)
			{
				Diagnostics.LogError("UnitAction(" + base.ShortActionName + ") has timed out!");
			}
		}

		[HarmonyPrefix]
        [HarmonyPatch("StartUnitAction")]
		public static bool StartUnitAction(UnitActionRangedFightSequence __instance)
		{
			if (!ReturnFire.doReturnFire)
				return true;
			//__instance.defenderAvailablePawns(new List<PresentationPawn>(__instance.defenderUnit().Pawns));
			//__instance.attackerAvailablePawns(new List<PresentationPawn>(__instance.attackerUnit().Pawns));
			//__instance.attackerAvailablePawns().AddRange(__instance.attackerUnit().Pawns);
			//__instance.defenderAvailablePawns().AddRange(__instance.defenderUnit().Pawns);

			Console.WriteLine("StartUnitAction");
			int count = __instance.attackerAvailablePawns().Count;
			int count2 = __instance.defenderAvailablePawns().Count;
			int num = Mathf.Min(count, count2);
			int num2 = attackersToKill;
			System.Random random = __instance.attackerUnit().Random;
			__instance.fightData(new PresentationUnitsFightData(__instance.attackerBattleUnit(), __instance.defenderBattleUnit(), 
				new List<PawnPair>(), attackersToKill, __instance.defendersToKill(), __instance.attackerAvailablePawns(), __instance.defenderAvailablePawns()));
			Presentation.PresentationChoreographyController.AddFightData(__instance.attackerBattleUnit().SimulationEntityGuid, __instance.fightData());
			__instance.AttackerBattleUnit.FightData = __instance.fightData();
			__instance.DefenderBattleUnit.FightData = __instance.fightData();
			Console.WriteLine("Attacker/Defender Pawns: " + __instance.attackerAvailablePawns().Count + " / " + __instance.defenderAvailablePawns().Count);
			Console.WriteLine("Attackers/Defenders to Kill: " + attackersToKill + " / " + __instance.defendersToKill() + " | FightData To Kill: " + __instance.fightData().AttackerPawnsToKillInitial +
				"(" + __instance.fightData().AttackersToKillRemaining + ") / " + __instance.fightData().DefenderPawnsToKillInitial + "(" + __instance.fightData().DefendersToKillRemaining + ")");
			bool flag = PresentationChoreographyController.CanPawnOnlyMultiKillRanged(__instance.defenderAvailablePawns()[0]);			
			int num3 = 1;
			int num4 = 0;
			bool flag2 = false;
			if (!flag && count2 > count)
			{
				float num5 = (float)num2 / (float)count;
				flag2 = num5 > 1f;
				int multiKillSituationalThreshold = Presentation.PresentationChoreographyController.MultiKillSituationalThreshold;
				if (num5 > (float)multiKillSituationalThreshold)
				{
					num3 = Mathf.Min(multiKillSituationalThreshold, num2 / (count + 1));
					num4 = num2 % num3;
					if (num4 == 1 && num3 == multiKillSituationalThreshold)
					{
						num3--;
					}
				}
			}
			PawnRangedFightSequence[] array = null;
			Console.WriteLine("flag2: " + flag2);
			if (flag2)
			{
				array = new PawnRangedFightSequence[count];
			}
			for (int i = 0; i < num; i++)
			{
				PresentationPawn presentationPawn = __instance.defenderAvailablePawns()[0];
				float num6 = (float)num2 / (float)(num - i);
				bool flag3 = num6 >= 1f || random.NextDouble() <= (double)num6;
				bool delay = i != 0;
				PawnRangedFightSequence pawnRangedFightSequence = null;
				if (flag)
				{
					pawnRangedFightSequence = new PawnRangedFightSequence(presentationPawn, __instance.attackerUnit(), flag3, delay);
					if (flag3)
					{
						num2 = 0;
					}
				}
				else
				{
					int num7 = num3;
					if (num4 > 0)
					{
						num7++;
						num4--;
					}
					if (num2 > 0 && num2 < num7)
					{
						num7 = num2;
					}
					for (int j = 0; j < num7; j++)
					{
						PresentationPawn referencePawn;
						if (pawnRangedFightSequence == null)
						{
							PresentationPawn[] targets = new PresentationPawn[num7];
							pawnRangedFightSequence = new PawnRangedFightSequence(presentationPawn, targets, flag3, delay, miss: false, 0f);
							referencePawn = presentationPawn;
						}
						else
						{
							referencePawn = pawnRangedFightSequence.Targets[0];
						}
						PresentationPawn nearestPawn = PawnHelper.GetNearestPawn(referencePawn, __instance.attackerAvailablePawns());
						pawnRangedFightSequence.Targets[j] = nearestPawn;
						__instance.attackerAvailablePawns().Remove(nearestPawn);
						if (flag3)
						{
							num2--;
						}
					}
					if (flag2)
					{
						array[i] = pawnRangedFightSequence;
					}
				}
				
				if (!flag2)
				{
					__instance.AddPawnRangedFightSequence(pawnRangedFightSequence, i, i);
					
				}
				__instance.defenderAvailablePawns().RemoveAt(0); 
			}
			if (count == count2 || flag)
			{
				return false;
			}
			if (count2 > count)
			{
				if (!flag2)
				{
					return false;
				}
				while (num2 > 0)
				{
					for (int k = 0; k < count; k++)
					{
						PawnRangedFightSequence obj = array[k];
						PresentationPawn shooter = obj.Shooter;
						num3 = Mathf.Min(num3, num2);
						PresentationPawn[] array2 = new PresentationPawn[num3];
						PawnRangedFightSequence finalSequence = obj.GetFinalSequence();
						new PawnRangedFightSequence(shooter, array2, dies: true, finalSequence);
						for (int l = 0; l < num3; l++)
						{
							PresentationPawn item = (array2[l] = PawnHelper.GetNearestPawn(shooter, __instance.attackerAvailablePawns()));
							__instance.attackerAvailablePawns().Remove(item);
							num2--;
						}
						if (num2 == 0)
						{
							break;
						}
					}
				}
				for (int m = 0; m < count; m++)
				{
					__instance.AddPawnRangedFightSequence(array[m], m, (m + 1) * 100);
					
				}
			}
			else
			{
				int count3 = __instance.defenderAvailablePawns().Count;
				int count4 = __instance.attackerUnit().Pawns.Count;
				for (int n = 0; n < count3; n++)
				{
					PresentationPawn shooter2 = __instance.defenderAvailablePawns()[n];
					int index = random.Next(0, count4);
					PresentationPawn presentationPawn2 = __instance.attackerUnit().Pawns[index];
					float projectileSpread = presentationPawn2.HalfRadius + Presentation.PresentationChoreographyController.ProjectileMissSpread;
					PawnRangedFightSequence fightSequence = new PawnRangedFightSequence(shooter2, presentationPawn2, dies: false, delay: true, miss: true, projectileSpread);
					__instance.AddPawnRangedFightSequence(fightSequence, n + num, n + num);
				}
			}	
			Console.WriteLine("End Attackers/Defenders to Kill: " + attackersToKill + " / " + __instance.defendersToKill() + " | FightData To Kill: " + __instance.fightData().AttackerPawnsToKillInitial +
				"(" + __instance.fightData().AttackersToKillRemaining + ") / " + __instance.fightData().DefenderPawnsToKillInitial + "(" + __instance.fightData().DefendersToKillRemaining + ")");		
			return false;
		}
	}	
# endregion
}