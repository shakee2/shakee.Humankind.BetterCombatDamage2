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
		bool multiKillAttacker;
		bool multiKillDefender;

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
			multiKillAttacker = PresentationChoreographyController.CanPawnOnlyMultiKillRanged(attackerAvailablePawns[0]);
			multiKillDefender = PresentationChoreographyController.CanPawnOnlyMultiKillRanged(defenderAvailablePawns[0]);
			

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
			Diagnostics.LogError($"{AttackerBattleUnit} vs {DefenderBattleUnit} ({attackersToKill} vs {defendersToKill})");
			Diagnostics.LogError("Attacker/Defender Pawns: " + attackerAvailablePawns.Count + " / " + defenderAvailablePawns.Count + " | DoReturnFire: " + doReturnFire);
			Diagnostics.LogError("Attackers/Defenders to Kill: " + attackersToKill + " / " + defendersToKill + " | HP: " + attackerBattleUnit.GetHealthRatio() + " / " + defenderBattleUnit.GetHealthRatio());

			int attackerCount = attackerAvailablePawns.Count;
			int defenderCount = defenderAvailablePawns.Count;
			//int count = attackerCount;
			//int count2 = defenderCount;

			int extraRounds = 0;
			int num = Mathf.Min(attackerCount, defenderCount);
			int num2 = defendersToKill;
			int num2a = attackersToKill;
			int totalKills = num2 + num2a;
			multiKillAttacker = false;
			multiKillDefender = false;
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
				
			//int count = 0;	// counter fightsequence
			//int count2 = extraRounds;  // counter extrarounds	
			PresentationPawn[] defTargets = new PresentationPawn[defenderCount];
			PresentationPawn[] attTargets = new PresentationPawn[attackerCount];
			for (int i = 0; i < Mathf.Max(defenderCount, attackerCount); i++)
			{
				if (i < defenderCount)
					defTargets[i] = defenderAvailablePawns[i];
				if (i < attackerCount)
					attTargets[i] = attackerAvailablePawns[i];
			}
			int count2 = 0;
			if (attackersToKill >= attackerCount && AttackerBattleUnit.GetHealthRatio() > 0)
			{
				attackersToKill = attackerCount - 1;
			}
			if (defendersToKill >= defenderCount && DefenderBattleUnit.GetHealthRatio() > 0)
			{
				defendersToKill = defenderCount - 1;
			}

			//---------------
			if (multiKillAttacker)
			{
				if (num2 > 0)
					defenderDies = true;
				else
					defenderDies = false;		
				bool defenderFirst = false;
					if (num2 >= defenderCount)
					{
						defenderFirst = true;
					}				
				if (doReturnFire)
				{			
					if (num2a > 0)
						attackerDies = true;
					else
						attackerDies = false;	
					
					if (multiKillDefender)
					{
						arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, attackerDies, true);
					}
					else
					{
						for (int i = 0; i < defenderCount; i++)
						{
							bool delay = i != 0;
							bool miss = num2a <= 0;
							arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], attTargets, attackerDies, delay, miss, 0f);
							num2a--;
							if (num2a <= 0)
								attackerDies = false;
						}						
					}				
					
					array[0] = new PawnRangedFightSequence(attackerAttackPawns[0], defenderUnit, defenderDies, true);
					
					for (int i = 0; i < arrayDefender.Length; i++)
					{
						AddPawnRangedFightSequence(arrayDefender[i], i, (1 + i)*10, true);	
					}								
					AddPawnRangedFightSequence(array[0], 50, 0);	
				}				
				else
				{			
					array[count2] = new PawnRangedFightSequence(attackerAttackPawns[0], defenderUnit, defenderDies, true);		
					Diagnostics.LogError("Add Pawns only Attacker | Multikill: " + multiKillAttacker + " / " + multiKillDefender);
					AddPawnRangedFightSequence(array[0], 0, 0);
					if (num2 > 0)
					{
						num2 = 0;
					}
				}
				
				
			}
			else
			{	
				int count = num2;
				bool repeatAttackAttacker = false;
				if (attackerCount < num2)
					repeatAttackAttacker = true;	

				if (attackerCount > defenderCount)
				{
					array = new PawnRangedFightSequence[defenderCount]; 
					for (int i = 0; i < array.Length; i++)
					{
						bool delay = i != 0;
						if (num2a > 0)
							attackerDies = true;
						else
							attackerDies = false;

						arrayDefender[i] = new PawnRangedFightSequence(attackerAttackPawns[i], defenderAvailablePawns[defenderAvailablePawns.Count - 1], defenderDies, delay, false, 0f);
						defenderAvailablePawns.RemoveAt(defenderAvailablePawns.Count - 1);
						num2--;
					}
				}
				else 
				{
					array = new PawnRangedFightSequence[attackerAvailablePawns.Count];
					for (int i = 0; i < array.Length; i++)
					{
						if (num2 > 0)
							defenderDies = true;
						else
							defenderDies = false;	
						bool delay = i != 0;
						bool miss = num2 <= 0;
						array[i] = new PawnRangedFightSequence(attackerAttackPawns[i], defTargets[defTargets.GetUpperBound(0)], defenderDies, delay, miss, 0f);
						num2--;
						attTargets.RemoveAt(attTargets.GetUpperBound(0));							
					}
				}	
				if (repeatAttackAttacker)
				{
					while (num2 > 0)
					{
						for (int j = 0; j < array.Length; j++)
						{
							PawnRangedFightSequence obj = array[j];
							PawnRangedFightSequence sequence = obj.GetFinalSequence();
							PresentationPawn[] target = new PresentationPawn[1];
							target[0] = defTargets[defTargets.GetUpperBound(0)];
							defTargets.RemoveAt(defTargets.GetUpperBound(0));
							new PawnRangedFightSequence(array[j].Shooter, target, defenderDies, sequence);
							num2--;
							if (num2 == 0)
							{
								defenderDies = false;
								break;
							}										
						}
					}
				}				
				if (doReturnFire)
				{
					arrayDefender = new PawnRangedFightSequence[defenderAvailablePawns.Count];
					if (num2a > 0)
						attackerDies = true;
					else
						attackerDies = false;	
					
					if (multiKillDefender)
					{	
						if (num2a > 0)
						{
							PresentationPawn[] array2 = new PresentationPawn[num2a];
							for (int i = 0; i < array2.Length; i++)
							{
								PresentationPawn item = attackerAvailablePawns[attackerAvailablePawns.Count - 1];
								array2[i] = item;
								attackerAvailablePawns.Remove(item);
							}
							arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], array2, attackerDies, true, false, 0f);
						}
						else 
						{
							arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, false, false);
						}					
						
					}
					else
					{
						bool repeatAttackDefender = false;
						if (defenderCount < num2a)
							repeatAttackDefender = true;
						if (defenderCount > attackerCount)
						{
							arrayDefender = new PawnRangedFightSequence[attackerCount]; 
							for (int i = 0; i < attackerCount; i++)
							{
								if (num2a > 0)
									attackerDies = true;
								else
									attackerDies = false;	
								bool delay = i != 0;
								bool miss = num2a <= 0;
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], attackerAvailablePawns[attackerAvailablePawns.Count - 1], attackerDies, delay, miss, 0f);
								attackerAvailablePawns.RemoveAt(attackerAvailablePawns.Count - 1);
								num2a--;
							}
						}
						else 
						{
							arrayDefender = new PawnRangedFightSequence[defenderAvailablePawns.Count];
							for (int i = 0; i < defenderCount; i++)
							{
								if (num2a > 0)
									attackerDies = true;
								else
									attackerDies = false;	
								bool delay = i != 0;
								bool miss = num2a <= 0;
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], attTargets[attTargets.GetUpperBound(0)], attackerDies, delay, miss, 0f);
								num2a--;
								attTargets.RemoveAt(attTargets.GetUpperBound(0));							
							}
						}						
						if (repeatAttackDefender)
						{
							while (num2a > 0)
							{
								for (int j = 0; j < arrayDefender.Length; j++)
								{
									PawnRangedFightSequence obj = arrayDefender[j];
									PawnRangedFightSequence sequence = obj.GetFinalSequence();
									PresentationPawn[] target = new PresentationPawn[1];
									target[0] = attTargets[attTargets.GetUpperBound(0)];
									attTargets.RemoveAt(attTargets.GetUpperBound(0));
									new PawnRangedFightSequence(arrayDefender[j].Shooter, target, attackerDies, sequence);
									num2a--;
									if (num2a == 0)
									{
										attackerDies = false;
										break;
									}										
								}
							}
						}						
					}			
					for (int i = 0; i < arrayDefender.Length; i++)
					{
						AddPawnRangedFightSequence(arrayDefender[i], (i) * 2, (1 + i));	
					}
					for (int i = 0; i < array.Length; i++)
					{
						AddPawnRangedFightSequence(array[i], i + 100, i);
					}									
					
				}
				else
				{
					for (int k = 0; k < array.Length; k++)
					{				
						Diagnostics.LogError("Add Pawns only Attacker | Multikill: " + multiKillAttacker + " / " + multiKillDefender);			
						AddPawnRangedFightSequence(array[k], k, (k));
					}	
				}
		
				
			}
			Diagnostics.LogError("End of StartUnitAction - Attacker/Defender Pawns: " + attackerAvailablePawns.Count + " / " + defenderAvailablePawns.Count);

		}

		public void AddPawnRangedFightSequence(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup, PawnActionRangedStartAttack rangedAttackAction, int attackLoopIndex, bool defendersTurn = false)
		{
			Diagnostics.LogError("Trigger AddPawnRanged_Long: " + fightSequence.Shooter);
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
				int redo;
				if (defendersTurn)
				{
					for (int k = 0; k < count; k++)
					{
						PresentationPawn defender = attackerAvailablePawns[k];
						int num4 = attackerActionGroup + attackersToKill + k;
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
				CreatePawnAction<PawnActionWaitIdle>(shooter, ActionScope.Attacker, blockingAction: false, attackerActionGroup).SetPawnActionParameters(new PawnActionWaitIdleParameters(shooter.FighterSubPawns, shooter.FighterSubPawnsCount));
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
