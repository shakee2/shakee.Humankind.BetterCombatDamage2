using System;
using System.Collections.Generic;
using Amplitude.Mercury.Data.World;
using UnityEngine;
using shakee.Humankind.BetterCombatDamage;

namespace Amplitude.Mercury.Presentation
{
	public class UnitActionRangedFightSequenceDefender : UnitAction
	{
		private PresentationBattleUnit attackerBattleUnit;

		private PresentationBattleUnit defenderBattleUnit;

		private PresentationUnit attackerUnit;

		private PresentationUnit defenderUnit;

		private List<PresentationPawn> attackerAvailablePawns;
		private List<PresentationPawn> defenderAvailablePawns;
		private List<PresentationPawn> attackerAttackPawns;
		private List<PresentationPawn> defenderAttackPawns;

		private int defendersToKill;
		private int attackersToKill;
		private int uncompletedWaitProjectileCount;
		private bool doReturnFire;

		private PresentationUnitsFightData fightData;

		public override void CreateUnitAction(ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction = true)
		{
			base.CreateUnitAction(ref fightSequence, actionScope, blockingAction);
			Diagnostics.LogError("New Class: CreateUnitAction Defender -> ReturnFire = " + ReturnFire.doReturnFire);
			attackerBattleUnit = fightSequence.AttackerBattleUnit;
			defenderBattleUnit = fightSequence.DefenderBattleUnit;
			attackerUnit = attackerBattleUnit.PresentationUnit;
			defenderUnit = defenderBattleUnit.PresentationUnit;
			defendersToKill = fightSequence.DefenderPawnsToKill;
			attackersToKill = fightSequence.AttackerPawnsToKill;			
			doReturnFire = ReturnFire.doReturnFire;
			attackerAvailablePawns = new List<PresentationPawn>(attackerUnit.Pawns);
			defenderAvailablePawns = new List<PresentationPawn>(defenderUnit.Pawns);
			attackerAttackPawns = new List<PresentationPawn>(attackerUnit.Pawns);
			defenderAttackPawns = new List<PresentationPawn>(defenderUnit.Pawns);
			

		}
		

		public override void StartUnitAction()
		{
			
			base.StartUnitAction();		
			Diagnostics.LogError("New Class: StartUnitAction Defender");	

			System.Random random = attackerUnit.Random;
			System.Random random2 = defenderUnit.Random;
			fightData = new PresentationUnitsFightData(attackerBattleUnit, defenderBattleUnit, new List<PawnPair>(), attackersToKill, defendersToKill, attackerAvailablePawns, defenderAvailablePawns);
			Presentation.PresentationChoreographyController.AddFightData(attackerBattleUnit.SimulationEntityGuid, fightData);
			AttackerBattleUnit.FightData = fightData;
			DefenderBattleUnit.FightData = fightData;
			Diagnostics.LogError($"{AttackerBattleUnit} vs {DefenderBattleUnit} ({attackerAttackPawns} vs {defendersToKill})");
			Diagnostics.LogError("Attacker/Defender Pawns: " + attackerAvailablePawns.Count + " / " + defenderAvailablePawns.Count + " | DoReturnFire: " + doReturnFire);
			Diagnostics.LogError("Attackers/Defenders to Kill: " + attackersToKill + " / " + defendersToKill + " | FightData To Kill: " + fightData.AttackerPawnsToKillInitial +
				" (" + fightData.AttackersToKillRemaining + ") / " + fightData.DefenderPawnsToKillInitial + " (" + fightData.DefendersToKillRemaining + ")");

			int attackerCount = attackerAvailablePawns.Count;
			int defenderCount = defenderAvailablePawns.Count;
			//int count = attackerCount;
			//int count2 = defenderCount;

			int extraRounds = 0;
			int num = Mathf.Min(attackerCount, defenderCount);
			int num2 = defendersToKill;
			int num2a = attackersToKill;
			int totalKills = num2 + num2a;
			bool multiKillAttacker = false;
			bool multiKillDefender = false;
			bool defenderDies = false;
			bool attackerDies = false;
			multiKillAttacker = PresentationChoreographyController.CanPawnOnlyMultiKillRanged(attackerAvailablePawns[0]);
			multiKillDefender = PresentationChoreographyController.CanPawnOnlyMultiKillRanged(defenderAvailablePawns[0]);
			
			if (attackerCount < defenderCount && !multiKillAttacker)
			{
				extraRounds = defenderCount - attackerCount;
			}		
			else if (defenderCount < attackerCount && !multiKillDefender)
			{
				extraRounds = attackerCount - defenderCount;
			}	
			PawnRangedFightSequence[] array = null;
			PawnRangedFightSequence[] arrayDefender = null;

			if (doReturnFire)
			{
				array = new PawnRangedFightSequence[attackerCount];
				arrayDefender = new PawnRangedFightSequence[defenderCount];
			}				
			else
			{
				array = new PawnRangedFightSequence[attackerCount];
			}	
				
			Diagnostics.LogError("ArraySize: " + array.Length + " | Multikill: " + multiKillAttacker + " / " + multiKillDefender);
			//int count = 0;	// counter fightsequence
			int count2 = extraRounds;  // counter extrarounds	

			if (multiKillAttacker)
			{
				if (num2 > 0)
					defenderDies = true;
				else
					defenderDies = false;

				array[0] = new PawnRangedFightSequence(attackerAttackPawns[0], defenderUnit, defenderDies, false);				
				if (doReturnFire && multiKillDefender)
				{			
					if (num2a > 0)
						attackerDies = true;
					else
						attackerDies = false;
	
					arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, attackerDies, false);

					if (num2 > 0)
					{
						AddPawnRangedFightSequence(arrayDefender[0], 10, 10, true);
						AddPawnRangedFightSequence(array[0], 100, 100);						
					}
					else
					{
						AddPawnRangedFightSequence(array[0], 10, 10);
						AddPawnRangedFightSequence(arrayDefender[0], 100 , 100, true);							
					}										
					if (num2a > 0)
					{
						num2a = 0;						
					}
				}
				else if (doReturnFire)
				{					
					PresentationPawn[] targets = new PresentationPawn[defenderCount];
					for (int i = 0; i < targets.Length; i++)
					{
						if (i < attackerCount)
							targets[i] = attackerAvailablePawns[attackerCount - 1 - i];
						else
							targets[i] = attackerAvailablePawns[random.Next(0, attackerCount)];
					}
					for (int i = 0; i < arrayDefender.Length; i++)
					{
						bool delay = i != 0;
						attackerDies = false;
						if (num2a > 0)
						{
							attackerDies = true;
							arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], targets[i], attackerDies, delay, false, 0f);	
							num2a--;
						}
						else
						{
							attackerDies = false;
							arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], targets[i], attackerDies, delay, true, 0.5f);	
						}
						
						
					}
					if (num2 > 0)
					{
						
						for (int k = 0; k < arrayDefender.Length; k++)
						{
							AddPawnRangedFightSequence(arrayDefender[k], k, k, true);
							Diagnostics.LogError("Added PawnFight Defender");
						}
						AddPawnRangedFightSequence(array[0], 1000, 1000);
					}
					else
					{								
						for (int k = 0; k < arrayDefender.Length; k++)
						{
							AddPawnRangedFightSequence(arrayDefender[k], k, k, true);
							Diagnostics.LogError("Added PawnFight Defender");
						}
						AddPawnRangedFightSequence(array[0], 1000, 1000);
					}
				}
				else
				{

					AddPawnRangedFightSequence(array[0], 10, 10);
				}
				if (num2 > 0)
				{
					num2 = 0;
				}
				
			}
			else
			{	int count = num2;
				for (int i = 0; i < attackerCount; i++)
				{
					bool delay = i != 0;
					if (count > 0)
					{
						defenderDies = true;
					}						
					else
					{
						defenderDies = false;
					}
					if (i < count)						
						array[i] = new PawnRangedFightSequence(attackerAttackPawns[i], defenderAvailablePawns[i], defenderDies, delay, false, 0f);	
					else
					{
						array[i] = new PawnRangedFightSequence(attackerAttackPawns[i], defenderAvailablePawns[random2.Next(0, count)], defenderDies, delay, true, 0.5f);	
					}			
					if (count > 0)
					{
						count--;;						
					}

				}
				if (multiKillDefender && doReturnFire)
				{
					if (attackersToKill > 0)
						attackerDies = true;
					else 
						attackerDies = false;
					arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, attackerDies, false);				
					if (num2a > 0)
					{
						num2a = 0;
					}
					if (num2a >= 0)
					{
						//attackerAvailablePawns.RemoveRange(0, attackersToKill);
						for (int i = 0; i < array.Length; i++)
						{			
							AddPawnRangedFightSequence(array[i], i + 1, (i + 1));

							Diagnostics.LogError("Added PawnFight");
						}	
						AddPawnRangedFightSequence(arrayDefender[0], 100, 100, true);
					}
					else
					{
						AddPawnRangedFightSequence(arrayDefender[0], 1, 1, true);
						for (int i = 0; i < array.Length; i++)
						{				

							AddPawnRangedFightSequence(array[i], i + 1, (i + 1) * 100);
							Diagnostics.LogError("Added PawnFight");
						}							
					}

				}
				else if (doReturnFire)
				{
										
					for (int i = 0; i < defenderCount; i++)
					{
						bool delay = i != 0;
						PresentationPawn targets = attackerAvailablePawns[attackerAvailablePawns.Count - i - 1];
						attackerDies = false;
						if (i < attackerCount)
						{
							if (num2a > 0)
							{
								attackerDies = true;
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], targets, attackerDies, delay, false, 0f);	
								num2a--;
							}
							else
							{
								attackerDies = false;
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], targets, attackerDies, delay, true, 0f);	
							}						
						}
						else
						{
							targets = attackerAvailablePawns[random.Next(0, attackerCount)];
							if (num2a > 0)
							{
								attackerDies = true;
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], targets, attackerDies, delay, false, 0f);	
								num2a--;
							}
							else
							{
								attackerDies = false;
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], targets, attackerDies, delay, true, 0f);	
							}			
						}
						
						
					}
					if (num2a >= attackerCount)
					{						
						for (int i = 0; i < array.Length; i++)
						{				
							AddPawnRangedFightSequence(array[i], i + 1, (i + 1) * 50);
							Diagnostics.LogError("Added PawnFight");
						}	
						for (int i = 0; i < arrayDefender.Length; i++)
						{		
							AddPawnRangedFightSequence(arrayDefender[i], i, i, true);
							Diagnostics.LogError("Added PawnFight");
						}
					}
					else
					{						
						for (int i = 0; i < arrayDefender.Length; i++)
						{			
							AddPawnRangedFightSequence(arrayDefender[i], i, i, true);
							Diagnostics.LogError("Added PawnFight");
						}
						for (int i = 0; i < array.Length; i++)
						{				
							AddPawnRangedFightSequence(array[i], i + 1, (i + 1) * 50);
							Diagnostics.LogError("Added PawnFight");
						}	
					}
				}
				else
				{
					for (int k = 0; k < array.Length; k++)
					{				
						AddPawnRangedFightSequence(array[k], k, (k));
						Diagnostics.LogError("Added PawnFight");
					}	
				}
		
				
			}
			Diagnostics.LogError("End of StartUnitAction");

		}

		public void AddPawnRangedFightSequence(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup, PawnActionRangedStartAttack rangedAttackAction, int attackLoopIndex, bool defendersTurn = false)
		{
			Console.WriteLine("Trigger AddPawnRanged_Long");
			PresentationPawn shooter = fightSequence.Shooter;
			PresentationPawn[] targets = fightSequence.Targets;
			int num = ((targets != null) ? targets.Length : 0);
			Vector3 position = shooter.Transform.position;
			Vector3 vector;
			if (defendersTurn)
			{
				vector = ((targets != null) ? targets[0].Transform.position : attackerUnit.WorldPosition.ToVector3());
			}
			else
			{
				vector = ((targets != null) ? targets[0].Transform.position : defenderUnit.WorldPosition.ToVector3());
			}
				
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
				rangedAttackAction.SetPawnActionParameters(new PawnActionRangedStartAttackParameters(fightSequence, useAlternateAttack, doStayInIdleAfterLoops: false, lookAtTarget: false));
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
					if (defendersTurn)
					{
						for (int j = 0; j < attackersToKill; j++)
						{
							PresentationPawn nearestPawnTo = PawnHelper.GetNearestPawnTo(vector, attackerAvailablePawns);
							int num3 = defenderActionGroup + j;
							CreateWaitProjectileAction(shooter, rangedAttackAction, 0, isNewRangedAttackAction: false, num3, fightSequence.Miss);
							CreateKillAction(shooter, nearestPawnTo, num3);
							attackerAvailablePawns.Remove(nearestPawnTo);
							StartPawnActions(num3);
						}
					}
					else
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

				}
				int count;
				if (defendersTurn)
				{
					count = attackerAvailablePawns.Count;
				}
				else
				{
					count = defenderAvailablePawns.Count;
				}
				if (defendersTurn)
				{
					for (int k = 0; k < count; k++)
					{
						PresentationPawn defender = attackerAvailablePawns[k];
						int num4 = defenderActionGroup + attackersToKill + k;
						CreateWaitProjectileAction(shooter, rangedAttackAction, 0, isNewRangedAttackAction: false, num4, fightSequence.Miss);
						CreateHitActions(shooter, defender, num4);
						CreateDefenderPostFightAction(shooter, defender, num4);
						StartPawnActions(num4);
					}
					attackerAvailablePawns.Clear();
				}
				else
				{
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
			}
			if (fightSequence.NextSequence == null)
			{
				CreatePawnAction<PawnActionWaitIdle>(shooter, ActionScope.Attacker, blockingAction: true, attackerActionGroup).SetPawnActionParameters(new PawnActionWaitIdleParameters(shooter.FighterSubPawns, shooter.FighterSubPawnsCount));
				CreatePawnAction<PawnActionRangedPostFight>(shooter, ActionScope.Attacker, blockingAction: false, attackerActionGroup);
			}
			StartPawnActions(attackerActionGroup);
			if (defenderActionGroup != attackerActionGroup && targets != null)
			{
				StartPawnActions(defenderActionGroup);
			}
		}

		public void AddPawnRangedFightSequence(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup, bool defendersTurn = false)
		{
			Console.WriteLine("Trigger AddPawnRanged_Short");
			AddPawnRangedFightSequence(fightSequence, attackerActionGroup, defenderActionGroup, null, 0, defendersTurn);
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
				CreatePawnAction<PawnActionTriggerHit>(defender, ActionScope.Defender, blockingAction: true, pawnActionGroup).SetPawnActionParameters(new PawnActionTriggerHitParameters(!attacker.MainSubPawn.IsShotTrajectoryCurved, needsProtection, attacker));
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
