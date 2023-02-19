/* using System;
using System.Collections.Generic;
using Amplitude.Mercury.Data.World;
using UnityEngine;
using shakee.Humankind.BetterCombatDamage;

namespace Amplitude.Mercury.Presentation
{
	public class UnitActionRangedFightSequence : UnitAction
	{
		private PresentationBattleUnit attackerBattleUnit;

		private PresentationBattleUnit defenderBattleUnit;

		private PresentationUnit attackerUnit;

		private PresentationUnit defenderUnit;

		private List<PresentationPawn> attackerAvailablePawns;

		private List<PresentationPawn> defenderAvailablePawns;

		private int defendersToKill;
		private int attackersToKill;
		private bool rangedRetaliate;

		private int uncompletedWaitProjectileCount;

		private PresentationUnitsFightData fightData;

		public override void CreateUnitAction(ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction = true)
		{
			if (PresentationChoreographyController_Patch.rangedRetaliate && PresentationChoreographyController_Patch.secondTry)
				this.rangedRetaliate = true;
			else	
				this.rangedRetaliate = false;
			base.CreateUnitAction(ref fightSequence, actionScope, blockingAction);
			Console.WriteLine("After CreateUnitAction NEW");
			attackerBattleUnit = fightSequence.AttackerBattleUnit;
			defenderBattleUnit = fightSequence.DefenderBattleUnit;
			attackerUnit = attackerBattleUnit.PresentationUnit;
			defenderUnit = defenderBattleUnit.PresentationUnit;
			defendersToKill = fightSequence.DefenderPawnsToKill;
			attackersToKill = fightSequence.AttackerPawnsToKill;
			attackerAvailablePawns = new List<PresentationPawn>(attackerUnit.Pawns);
			defenderAvailablePawns = new List<PresentationPawn>(defenderUnit.Pawns);

		}

		public override void StartUnitAction()
		{
			Console.WriteLine("Trigger StartUnitAction");
			base.StartUnitAction();
			
			int count = attackerAvailablePawns.Count;
			int count2 = defenderAvailablePawns.Count;
			int num = Mathf.Min(count, count2);
			int num2 = defendersToKill;
			System.Random random = attackerUnit.Random;
			fightData = new PresentationUnitsFightData(attackerBattleUnit, defenderBattleUnit, new List<PawnPair>(), 0, defendersToKill, attackerAvailablePawns, defenderAvailablePawns);
			Presentation.PresentationChoreographyController.AddFightData(attackerBattleUnit.SimulationEntityGuid, fightData);
			AttackerBattleUnit.FightData = fightData;
			DefenderBattleUnit.FightData = fightData;
			bool flag = PresentationChoreographyController.CanPawnOnlyMultiKillRanged(attackerAvailablePawns[0]);
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
			if (flag2)
			{
				array = new PawnRangedFightSequence[count];
			}
			for (int i = 0; i < num; i++)
			{
				PresentationPawn presentationPawn = attackerAvailablePawns[0];
				float num6 = (float)num2 / (float)(num - i);
				bool flag3 = num6 >= 1f || random.NextDouble() <= (double)num6;
				bool delay = i != 0;
				PawnRangedFightSequence pawnRangedFightSequence = null;
				if (flag)
				{
					pawnRangedFightSequence = new PawnRangedFightSequence(presentationPawn, defenderUnit, flag3, delay);
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
						PresentationPawn nearestPawn = PawnHelper.GetNearestPawn(referencePawn, defenderAvailablePawns);
						pawnRangedFightSequence.Targets[j] = nearestPawn;
						defenderAvailablePawns.Remove(nearestPawn);
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
					AddPawnRangedFightSequence(pawnRangedFightSequence, i, i);
				}
				attackerAvailablePawns.RemoveAt(0);
			}
			if (count == count2 || flag)
			{
				return;
			}
			if (count2 > count)
			{
				if (!flag2)
				{
					return;
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
							PresentationPawn item = (array2[l] = PawnHelper.GetNearestPawn(shooter, defenderAvailablePawns));
							defenderAvailablePawns.Remove(item);
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
					AddPawnRangedFightSequence(array[m], m, (m + 1) * 100);
				}
			}
			else
			{
				int count3 = attackerAvailablePawns.Count;
				int count4 = defenderUnit.Pawns.Count;
				for (int n = 0; n < count3; n++)
				{
					PresentationPawn shooter2 = attackerAvailablePawns[n];
					int index = random.Next(0, count4);
					PresentationPawn presentationPawn2 = defenderUnit.Pawns[index];
					float projectileSpread = presentationPawn2.HalfRadius + Presentation.PresentationChoreographyController.ProjectileMissSpread;
					PawnRangedFightSequence fightSequence = new PawnRangedFightSequence(shooter2, presentationPawn2, dies: false, delay: true, miss: true, projectileSpread);
					AddPawnRangedFightSequence(fightSequence, n + num, n + num);
				}
			}
		}

		public void AddPawnRangedFightSequence(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup, PawnActionRangedStartAttack rangedAttackAction, int attackLoopIndex)
		{
			Console.WriteLine("Trigger AddPawnRanged_Long");
			PresentationPawn shooter = fightSequence.Shooter;
			PresentationPawn[] targets = fightSequence.Targets;
			int num = ((targets != null) ? targets.Length : 0);
			Vector3 position = shooter.Transform.position;
			Vector3 vector = ((targets != null) ? targets[0].Transform.position : defenderUnit.WorldPosition.ToVector3());
			if (!Presentation.PresentationChoreographyController.RangedFightSequencesByPawn.ContainsKey(shooter))
			{
				Presentation.PresentationChoreographyController.RangedFightSequencesByPawn.Add(shooter, fightSequence);
			}
			else
			{
				Presentation.PresentationChoreographyController.RangedFightSequencesByPawn[shooter] = fightSequence;
			}
			for (int i = 0; i < num; i++)
			{
				Presentation.PresentationChoreographyController.AddBattlingPairsForMaterials(new PawnPair(shooter, targets[i]));
			}
			if (rangedAttackAction == null)
			{
				shooter.SetCurrentFightSequence(FightType.Ranged);
				shooter.ChoreographyRole = ChoreographyRoleType.Attacker;
				shooter.StopIdleAlt();
			}
			fightSequence.CallOnTargets(InitializeDefender, defenderActionGroup);
			bool useAlternateAttack = false;
			int facingAngleOffset = shooter.PresentationUnit.PresentationUnitDefinition.FacingAngleOffset;
			if (facingAngleOffset != 0)
			{
				float num2 = 0f - Mathf.Sign(Vector3.SignedAngle(shooter.Transform.forward, vector - position, Vector3.up));
				Vector3 vector2 = Quaternion.AngleAxis(facingAngleOffset, Vector3.up) * (vector - position) * num2;
				vector = position + vector2;
				useAlternateAttack = num2 == 1f;
			}
			CreatePawnAction<PawnActionLookAt>(shooter, ActionScope.Attacker, blockingAction: false, defenderActionGroup).SetPawnActionParameters(new PawnActionLookAtParameters(vector, prepareMovement: false, delay: true, fastRotate: false, 30f, RotationAnimationPolicy.NoAnimation, ignoreAngleOffset: true));
			bool isNewRangedAttackAction = rangedAttackAction == null;
			if (rangedAttackAction == null)
			{
				rangedAttackAction = CreatePawnAction<PawnActionRangedStartAttack>(shooter, ActionScope.Attacker, attackerActionGroup != defenderActionGroup, attackerActionGroup);				
				rangedAttackAction.SetPawnActionParameters(R.PawnActionRangedStartAttackParameters2(fightSequence, useAlternateAttack, doStayInIdleAfterLoops: false, lookAtTarget: false));
			}
			if (targets != null)
			{
				CreateWaitProjectileAction(shooter, rangedAttackAction, attackLoopIndex, isNewRangedAttackAction, defenderActionGroup, fightSequence.Miss);
				if (fightSequence.Dies)
				{
					fightSequence.CallOnTargets(CreateKillAction, defenderActionGroup);
				}
				else if (!fightSequence.Miss)
				{
					fightSequence.CallOnTargets(CreateHitActions, defenderActionGroup);
					fightSequence.CallOnTargets(CreateDefenderPostFightAction, defenderActionGroup);
				}
			}
			else
			{
				if (fightSequence.Dies)
				{
					for (int j = 0; j < defendersToKill; j++)
					{
						PresentationPawn nearestPawnTo = PawnHelper.GetNearestPawnTo(vector, defenderAvailablePawns);
						int num3 = defenderActionGroup + j;
						CreateWaitProjectileAction(shooter, rangedAttackAction, 0, isNewRangedAttackAction: false, num3, fightSequence.Miss);
						CreateKillAction(shooter, nearestPawnTo, num3);
						defenderAvailablePawns.Remove(nearestPawnTo);
						StartPawnActions(num3);
					}
				}
				int count = defenderAvailablePawns.Count;
				for (int k = 0; k < count; k++)
				{
					PresentationPawn defender = defenderAvailablePawns[k];
					int num4 = defenderActionGroup + defendersToKill + k;
					CreateWaitProjectileAction(shooter, rangedAttackAction, 0, isNewRangedAttackAction: false, num4, fightSequence.Miss);
					CreateHitActions(shooter, defender, num4);
					CreateDefenderPostFightAction(shooter, defender, num4);
					StartPawnActions(num4);
				}
				defenderAvailablePawns.Clear();
			}
			if (fightSequence.NextSequence == null)
			{
				CreatePawnAction<PawnActionWaitIdle>(shooter, ActionScope.Attacker, blockingAction: true, attackerActionGroup).SetPawnActionParameters(R.PawnActionWaitIdleParameters2(shooter.FighterSubPawns, shooter.FighterSubPawnsCount));
				CreatePawnAction<PawnActionRangedPostFight>(shooter, ActionScope.Attacker, blockingAction: false, attackerActionGroup);
			}
			StartPawnActions(attackerActionGroup);
			if (defenderActionGroup != attackerActionGroup && targets != null)
			{
				StartPawnActions(defenderActionGroup);
			}
		}

		public void AddPawnRangedFightSequence(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup)
		{
			Console.WriteLine("Trigger AddPawnRanged_Short");
			AddPawnRangedFightSequence(fightSequence, attackerActionGroup, defenderActionGroup, null, 0);
		}

		public override void UpdateUnitAction()
		{
			base.UpdateUnitAction();
			UpdatePawnActions();
		}

		protected override void OnTimedOut()
		{
			battle.OnTimeOut();
			if (!PresentationChoreographyAction.EndActionOnFailSafeLifeTime)
			{
				Diagnostics.LogError("UnitAction(" + base.ShortActionName + ") has timed out!");
			}
		}

		protected override void OnPawnActionsEnd()
		{
			Console.WriteLine("Trigger OnPawnActionsEnd");
			base.OnPawnActionsEnd();
			OnUnitActionEnd();
		}

		private void InitializeDefender(PresentationPawn attacker, PresentationPawn defender, int index)
		{
			defender.SetCurrentFightSequence(FightType.Ranged);
			defender.ChoreographyRole = ChoreographyRoleType.Defender;
			defender.StopIdleAlt();
		}

		private void CreateWaitProjectileAction(PresentationPawn attacker, PawnActionRangedStartAttack rangedAttackAction, int attackLoopIndex, bool isNewRangedAttackAction, int defenderActionGroup, bool miss)
		{
			PawnActionWaitProjectile pawnActionWaitProjectile = CreatePawnAction<PawnActionWaitProjectile>(attacker, ActionScope.Both, blockingAction: true, defenderActionGroup);
			pawnActionWaitProjectile.SetPawnActionParameters(new PawnActionWaitProjectileParameters(isNewRangedAttackAction, rangedAttackAction, attackLoopIndex));
			if (!miss)
			{
				uncompletedWaitProjectileCount++;
				pawnActionWaitProjectile.UponCompletion = (System.Action)Delegate.Combine(pawnActionWaitProjectile.UponCompletion, new System.Action(OnWaitProjectileActionCompleted));
			}
		}

		private void CreateKillAction(PresentationPawn attacker, PresentationPawn pawn, int pawnActionGroup)
		{
			CreatePawnAction<PawnActionKillPawn>(pawn, ActionScope.Defender, blockingAction: false, pawnActionGroup).SetPawnActionParameters(new PawnActionKillPawnParameters(attacker, forceKillAnim: true, killExtraNearby: false));
		}

		private void CreateHitActions(PresentationPawn attacker, PresentationPawn defender, int pawnActionGroup)
		{
			if ((defender.AnimationCapabilitiesAny & PawnAnimationCapability.Hit) != 0)
			{
				bool needsProtection = !defender.Definition.IgnoreProtectOnRangedAttack;
				CreatePawnAction<PawnActionTriggerHit>(defender, ActionScope.Defender, blockingAction: true, pawnActionGroup).SetPawnActionParameters(R.PawnActionTriggerHitParameters2(!attacker.MainSubPawn.IsShotTrajectoryCurved, needsProtection, attacker));
				CreatePawnAction<PawnActionSetProtectionAnimation>(defender, ActionScope.Defender, blockingAction: false, pawnActionGroup).SetPawnActionParameters(new PawnActionSetProtectionAnimationParameters(ProtectAnimationType.None));
				CreatePawnAction<PawnActionSetAnimationInt>(defender, ActionScope.Defender, blockingAction: false, pawnActionGroup).SetPawnActionParameters(new PawnActionSetAnimationIntParameters(AnimationVariableNames.DefenseChoice, 0, defender.SubPawns, defender.SubPawnCount));
			}
		}

		private void CreateDefenderPostFightAction(PresentationPawn attacker, PresentationPawn defender, int pawnActionGroup)
		{
			CreatePawnAction<PawnActionRangedPostFight>(defender, ActionScope.Defender, blockingAction: false, pawnActionGroup);
		}

		private void OnWaitProjectileActionCompleted()
		{
			uncompletedWaitProjectileCount--;
			if (uncompletedWaitProjectileCount <= 0)
			{
				Presentation.PresentationChoreographyController.OnUISynchronizationPointReached(fightData);
			}
		}
	}
}
 */