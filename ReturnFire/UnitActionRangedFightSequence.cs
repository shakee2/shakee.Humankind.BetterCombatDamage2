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
			System.Random RNG = new System.Random(234);
			fightData = new PresentationUnitsFightData(attackerBattleUnit, defenderBattleUnit, new List<PawnPair>(), attackersToKill, defendersToKill, attackerAvailablePawns, defenderAvailablePawns);
			Presentation.PresentationChoreographyController.AddFightData(attackerBattleUnit.SimulationEntityGuid, fightData);
			AttackerBattleUnit.FightData = fightData;
			DefenderBattleUnit.FightData = fightData;
			Diagnostics.LogError($"{AttackerBattleUnit} vs {DefenderBattleUnit} ({attackersToKill} vs {defendersToKill})");
			Diagnostics.LogError("Attacker/Defender Pawns: " + attackerAvailablePawns.Count + " / " + defenderAvailablePawns.Count + " | DoReturnFire: " + doReturnFire);
			Diagnostics.LogError("Attackers/Defenders to Kill: " + attackersToKill + " / " + defendersToKill + " | HP: " + attackerBattleUnit.GetHealthRatio() + " / " + defenderBattleUnit.GetHealthRatio());

			int attackerCount = attackerAvailablePawns.Count;
			int defenderCount = defenderAvailablePawns.Count;
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
				
			PresentationPawn[] defTargets = new PresentationPawn[defenderCount];
			PresentationPawn[] attTargets = new PresentationPawn[attackerCount];
			for (int i = 0; i < Mathf.Max(defenderCount, attackerCount); i++)
			{
				if (i < defenderCount)
					defTargets[i] = defenderAvailablePawns[i];
				if (i < attackerCount)
					attTargets[i] = attackerAvailablePawns[i];
			}

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
				{
					PresentationPawn[] array2 = new PresentationPawn[num2];
					for (int i = 0; i < array2.Length; i++)
					{
						PresentationPawn item = defenderAvailablePawns[defenderAvailablePawns.Count - 1];
						array2[i] = item;
						defenderAvailablePawns.Remove(item);
					}
					array[0] = new PawnRangedFightSequence(attackerAttackPawns[0], array2, true, true, false, 0f);
				}
				else 
				{
					array[0] = new PawnRangedFightSequence(attackerAttackPawns[0], defenderUnit, false, true);				
				}

				// ---- Defenders Turn ----			
				if (doReturnFire)
				{			
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
							arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], array2, true, true, false, 0f);
						}
						else 
						{
							arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, false, true);
						}				
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
					
					
					int delayCount = 10;
					for (int i = 0; i < arrayDefender.Length; i++)
					{
						AddPawnRangedFightSequence(arrayDefender[i], i, delayCount, true);		// 1 + i * 10
						delayCount += 5 * i;	
					}								
					AddPawnRangedFightSequence(array[0], delayCount, 1);	// 50, 0
				}				
				else
				{			
					array[0] = new PawnRangedFightSequence(attackerAttackPawns[0], defenderUnit, defenderDies, true);		
					Diagnostics.LogError("Add Pawns only Attacker | Multikill: " + multiKillAttacker + " / " + multiKillDefender);
					AddPawnRangedFightSequence(array[0], 0, 0);
					if (num2 > 0)
					{
						num2 = 0;
					}
				}
				
				
			}
			// ---- Attackers Turn ----
			else
			{	
				Console.WriteLine("--- Attackers Turn ---");
				int count = num2;
				bool repeatAttackAttacker = false;
				if (attackerCount < num2)
					repeatAttackAttacker = true;

				PresentationPawn[] killTargets = new PresentationPawn[defendersToKill];
				PresentationPawn shooter = attackerAttackPawns[0];
				bool attackerKills = false;
				if (num2 > 0)
				{
					for (int i = 0; i < defendersToKill; i++)
					{
						killTargets[i] = defenderAvailablePawns[defenderCount - 1 - i];
					}
					num2 = 0;
					attackerKills = true;
				}
					

				if (attackerCount > defenderCount)
				{
					array = new PawnRangedFightSequence[attackerCount]; 

					for (int i = 0; i < defenderCount; i++)
					{
						bool delay = i != 0;						
						PresentationPawn target = defenderAvailablePawns[defenderAvailablePawns.Count - 1];
						if (num2 > 0)
						{
							array[i] = new PawnRangedFightSequence(attackerAttackPawns[0], target, true, true, false, 0f);
							Console.WriteLine("Attackers Adding Pawn: " + attackerAttackPawns[0] + " vs. " + target + " | Defender Dies: true");	
						}							
						else
						{
							array[i] = new PawnRangedFightSequence(attackerAttackPawns[0], target, false, true, true, 0.25f);
							Console.WriteLine("Attackers Adding Pawn: " + attackerAttackPawns[0] + " vs. " + target + " | Defender Dies: false");	
						}						
						defenderAvailablePawns.Remove(target);
						attackerAttackPawns.RemoveAt(0);
						num2--;
					}					
					for (int i = defenderCount; i < attackerCount; i++)
					{						
						int rngInt = RNG.Next(0, defenderCount - 1);
						PresentationPawn target = defTargets[rngInt];
						Console.WriteLine("Attackers Adding Pawn: " + attackerAttackPawns[0] + " vs. " + target + " | Defender Dies: false");
						array[i] = new PawnRangedFightSequence(attackerAttackPawns[0], target, false, true, true, 0.25f);						
						attackerAttackPawns.RemoveAt(0);
					}					
				}
				else 
				{
					array = new PawnRangedFightSequence[attackerCount];
					for (int i = 0; i < array.Length; i++)
					{
						if (num2 > 0)
							defenderDies = true;
						else
							defenderDies = false;	
						bool delay = i != 0;
						bool miss = num2 <= 0;
						PresentationPawn target = defenderAvailablePawns[defenderAvailablePawns.Count - 1];
						array[i] = new PawnRangedFightSequence(attackerAttackPawns[i], target, defenderDies, true, miss, 0f);
						Console.WriteLine("Attackers Adding Pawn: " + attackerAttackPawns[i] + " vs. " + target + " | Defender Dies: " + defenderDies);
						num2--;
						defenderAvailablePawns.Remove(target);						
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
							Console.WriteLine("Debug Looop - Attacks left of Attacker: " + num2 + " | Pawn: " + target[0].PawnId);
							num2--;
							if (num2 == 0)
							{
								defenderDies = false;
								break;
							}										
						}
					}
				}				
				// ---- Defenders Turn ----
				if (doReturnFire)
				{
					
					arrayDefender = new PawnRangedFightSequence[defenderAttackPawns.Count];
					if (num2a > 0)
						attackerDies = true;
					else
						attackerDies = false;	

					PresentationPawn[] killTargets2 = new PresentationPawn[attackersToKill];
					PresentationPawn shooter2 = defenderAttackPawns[0];
					bool defenderKills = false;
					if (num2a > 0)
					{
						for (int i = 0; i < attackersToKill; i++)
						{
							killTargets2[i] = attackerAvailablePawns[attackerCount - 1 - i];
						}
						num2a = 0;
						defenderKills = true;
					}
					
					if (multiKillDefender)
					{	
						Console.WriteLine("--- Defenders Turn: Multikill ---");
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
							arrayDefender[0] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, false, true);
						}					
						
					}
					else
					{
						bool repeatAttackDefender = false;
						if (defenderCount < num2a)
							repeatAttackDefender = true;
						if (defenderCount > attackerCount)
						{		
							Console.WriteLine("--- Defenders Turn: defender > attacker ---");					
							arrayDefender = new PawnRangedFightSequence[defenderCount]; 
							for (int i = 0; i < attackerCount; i++)
							{
								if (num2a > 0)
									attackerDies = true;
								else
									attackerDies = false;	
								bool delay = i != 0;
								bool miss = num2a <= 0;
								PresentationPawn target = attackerAvailablePawns[attackerAvailablePawns.Count - 1];
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[0], target, attackerDies, true, miss, 0f);
								Console.WriteLine("Defenders Adding Pawn: " + defenderAttackPawns[0] + " vs. " + target + " | Attacker Dies: " + attackerDies);
								attackerAvailablePawns.RemoveAt(attackerAvailablePawns.Count - 1);
								defenderAttackPawns.RemoveAt(0);
								num2a--;
							}
							for (int i = attackerCount; i < defenderCount; i++)							
							{
								
								int rngInt = RNG.Next(attackerCount);
								PresentationPawn target = attTargets[rngInt];
								Console.WriteLine("Defenders Adding Pawn: " + defenderAttackPawns[0] + " vs. " + target + " | Attacker Dies: false");
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[0], attackerUnit, false, true);	
								defenderAttackPawns.RemoveAt(0);
							}
						}
						else 
						{
							Console.WriteLine("--- Defenders Turn: defender <= attacker ---");
							arrayDefender = new PawnRangedFightSequence[defenderAttackPawns.Count];
							for (int i = 0; i < arrayDefender.Length; i++)
							{
								if (num2a > 0)
									attackerDies = true;
								else
									attackerDies = false;	
								bool delay = i != 0;
								bool miss = num2a <= 0;
								PresentationPawn target = attackerAvailablePawns[attackerAvailablePawns.Count - 1];
								arrayDefender[i] = new PawnRangedFightSequence(defenderAttackPawns[i], target, attackerDies, true, miss, 0f);
								Console.WriteLine("Defenders Adding Pawn: " + defenderAttackPawns[i] + " vs. " + target + " | Death: " + attackerDies + " | Miss: " + miss);
								num2a--;
								attackerAvailablePawns.Remove(target);						
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
									Console.WriteLine("Debug Looop - Attacks left of Defender: " + num2a + " | Pawn: " + target[0].PawnId);
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
					if (attackerKills)
						array[0] = new PawnRangedFightSequence(shooter, killTargets, true, true, false, 0f);
					if (defenderKills)
						arrayDefender[0] = new PawnRangedFightSequence(shooter2, killTargets2, true, true, false, 0f);

					int delayCount = 10;
					for (int i = 0; i < arrayDefender.Length; i++)
					{
						if (i < arrayDefender.Length)
						AddPawnRangedFightSequence(arrayDefender[i], (delayCount), (i), true);	
						delayCount += 2 * i;
					}
					for (int i = 0; i < array.Length; i++)
					{
						AddPawnRangedFightSequence(array[i], delayCount, (i));
						delayCount += 2 * i;
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
		public void AddPawnRangedFightSequence_NEW(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup, PawnActionRangedStartAttack rangedAttackAction, int attackLoopIndex, bool defendersTurn = false)
		{
			Diagnostics.LogError("Trigger AddPawnRanged_Long: " + fightSequence.Shooter);
			PresentationPawn shooter = fightSequence.Shooter;			
			PresentationPawn[] targets = fightSequence.Targets;
			int num = ((targets != null) ? targets.Length : 0);
			Vector3 position = shooter.Transform.position;
			Vector3 position2 = targets[0].Transform.position;
			Vector3 vector;
			Vector3 vectora;

				vectora = ((targets != null) ? shooter.Transform.position : defenderUnit.WorldPosition.ToVector3());			
				vector = ((targets != null) ? targets[0].Transform.position : defenderUnit.WorldPosition.ToVector3());

				
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
			bool useAlternateAttack2 = false;
			int facingAngleOffset = shooter.PresentationUnit.PresentationUnitDefinition.FacingAngleOffset;
			int facingAngleOffset2 = targets[0].PresentationUnit.PresentationUnitDefinition.FacingAngleOffset;
			if (facingAngleOffset != 0)
			{
				float num2 = 0f - Mathf.Sign(Vector3.SignedAngle(shooter.Transform.forward, vector - position, Vector3.up));
				Vector3 vector2 = Quaternion.AngleAxis(facingAngleOffset, Vector3.up) * (vector - position) * num2;
				vector = position + vector2;
				useAlternateAttack = num2 == 1f;
			}
			if (facingAngleOffset2 != 0)
			{
				float num2 = 0f - Mathf.Sign(Vector3.SignedAngle(targets[0].Transform.forward, vectora - position2, Vector3.up));
				Vector3 vector2 = Quaternion.AngleAxis(facingAngleOffset2, Vector3.up) * (vectora - position2) * num2;
				vectora = position2 + vector2;
				useAlternateAttack2 = num2 == 1f;
			}
			CreatePawnAction<PawnActionLookAt>(shooter, ActionScope.Attacker, blockingAction: true, defenderActionGroup).SetPawnActionParameters(new PawnActionLookAtParameters(vector, prepareMovement: false, delay: true, fastRotate: false, 30f, RotationAnimationPolicy.NoAnimation, ignoreAngleOffset: true));
			CreatePawnAction<PawnActionLookAt>(targets[0], ActionScope.Attacker, blockingAction: true, defenderActionGroup).SetPawnActionParameters(new PawnActionLookAtParameters(vectora, prepareMovement: false, delay: true, fastRotate: false, 30f, RotationAnimationPolicy.NoAnimation, ignoreAngleOffset: true));
			bool isNewRangedAttackAction = rangedAttackAction == null;
			PawnActionRangedStartAttack rangedAttackAction2;
			if (rangedAttackAction == null)
			{
				rangedAttackAction = CreatePawnAction<PawnActionRangedStartAttack>(shooter, ActionScope.Attacker, attackerActionGroup != defenderActionGroup, attackerActionGroup);				
				rangedAttackAction.SetPawnActionParameters(new PawnActionRangedStartAttackParameters(fightSequence, useAlternateAttack, doStayInIdleAfterLoops: false, lookAtTarget: true));
				rangedAttackAction2 = CreatePawnAction<PawnActionRangedStartAttack>(shooter, ActionScope.Defender, attackerActionGroup != defenderActionGroup, defenderActionGroup);				
				rangedAttackAction2.SetPawnActionParameters(new PawnActionRangedStartAttackParameters(fightSequence, useAlternateAttack, doStayInIdleAfterLoops: false, lookAtTarget: true));
				
			}			
			int attackerCount = attackerAttackPawns.Count;
			int defenderCount = defenderAttackPawns.Count;
			int sequenceCounter = attackerActionGroup;
			if (doReturnFire)
			{
				for (int i = 0; i < attackerCount; i++)
				{
					bool defenderKill = false;
					PresentationPawn target = defenderAvailablePawns[0];
					PresentationPawn attacker = attackerAttackPawns[i];
					sequenceCounter++;
					
					if (defendersToKill > 0)
					{
						CreateWaitProjectileAction(attacker, rangedAttackAction, 0, isNewRangedAttackAction: false, sequenceCounter, false);
						CreateKillAction(attacker, target, sequenceCounter);					
					}
					else
					{
						CreateWaitProjectileAction(attacker, rangedAttackAction, 0, isNewRangedAttackAction: false, sequenceCounter, true);
					}
					StartPawnActions(sequenceCounter);


				}
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
			
			if (defenderActionGroup != attackerActionGroup && targets != null)
			{
				StartPawnActions(defenderActionGroup);
			}
			StartPawnActions(attackerActionGroup);

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
				vector = ((targets != null) ? shooter.Transform.position : attackerUnit.WorldPosition.ToVector3());
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
			CreatePawnAction<PawnActionLookAt>(shooter, ActionScope.Attacker, blockingAction: true, defenderActionGroup).SetPawnActionParameters(new PawnActionLookAtParameters(vector, prepareMovement: false, delay: true, fastRotate: false, 30f, RotationAnimationPolicy.NoAnimation, ignoreAngleOffset: true));
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
							int num3 = attackerActionGroup + j;
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
			
			if (defenderActionGroup != attackerActionGroup && targets != null)
			{
				StartPawnActions(defenderActionGroup);
			}
			StartPawnActions(attackerActionGroup);

		}

		public void AddPawnRangedFightSequence(PawnRangedFightSequence fightSequence, int attackerActionGroup, int defenderActionGroup, bool defendersTurn = false)
		{
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
			PawnActionWaitProjectile pawnActionWaitProjectile = CreatePawnAction<PawnActionWaitProjectile>(attacker, ActionScope.Both, blockingAction: false, defenderActionGroup);
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
