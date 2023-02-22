using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Amplitude;
using Amplitude.Framework.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data;
using Amplitude.Mercury;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.UI.Interactables;
using Amplitude.Mercury.Presentation;

namespace shakee.Humankind.BetterCombatDamage
{
	// Token: 0x02000037 RID: 55
	public static class R
    {
        

        private static PropertyInfo BattleUnit_AttackDamage_PropertyInfo = typeof(BattleUnit).GetProperty("AttackDamage", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

		public static void AttackDamage(this BattleUnit self, FixedPoint value)
		{
			R.BattleUnit_AttackDamage_PropertyInfo.SetValue(self, value);
		}
            private static FieldInfo BattleUnit_CombatHealthRatio_PropertyInfo = typeof(BattleUnit).GetField("CombatHealthRatio", BindingFlags.Instance | BindingFlags.NonPublic);

		public static void CombatHealthRatio(this BattleUnit self, FixedPoint value)
		{
			R.BattleUnit_CombatHealthRatio_PropertyInfo.SetValue(self, value);
		}
		         
        		private static FieldInfo UnitActionRangedFightSequence_attackerBattleUnit = typeof(UnitActionRangedFightSequence).GetField("attackerBattleUnit", BindingFlags.Instance | BindingFlags.NonPublic);
		public static PresentationBattleUnit attackerBattleUnit(this UnitActionRangedFightSequence self)
		{
			return (PresentationBattleUnit)R.UnitActionRangedFightSequence_attackerBattleUnit.GetValue(self);
		}
				public static void attackerBattleUnit(this UnitActionRangedFightSequence self, PresentationBattleUnit value)
		{
			R.UnitActionRangedFightSequence_attackerBattleUnit.SetValue(self, value);
		}
		        private static FieldInfo UnitActionRangedFightSequence_defenderBattleUnit = typeof(UnitActionRangedFightSequence).GetField("defenderBattleUnit", BindingFlags.Instance | BindingFlags.NonPublic);
		public static PresentationBattleUnit defenderBattleUnit(this UnitActionRangedFightSequence self)
		{
			return (PresentationBattleUnit)R.UnitActionRangedFightSequence_defenderBattleUnit.GetValue(self);
		}
				public static void defenderBattleUnit(this UnitActionRangedFightSequence self, PresentationBattleUnit value)
		{
			R.UnitActionRangedFightSequence_defenderBattleUnit.SetValue(self, value);
		}
		        private static FieldInfo UnitActionRangedFightSequence_attackerUnit = typeof(UnitActionRangedFightSequence).GetField("attackerUnit", BindingFlags.Instance | BindingFlags.NonPublic);
		public static PresentationUnit attackerUnit(this UnitActionRangedFightSequence self)
		{
			return (PresentationUnit)R.UnitActionRangedFightSequence_attackerUnit.GetValue(self);
		}
						public static void attackerUnit(this UnitActionRangedFightSequence self, PresentationUnit value)
		{
			R.UnitActionRangedFightSequence_attackerUnit.SetValue(self, value);
		}
		        private static FieldInfo UnitActionRangedFightSequence_defenderUnit = typeof(UnitActionRangedFightSequence).GetField("defenderUnit", BindingFlags.Instance | BindingFlags.NonPublic);
		public static PresentationUnit defenderUnit(this UnitActionRangedFightSequence self)
		{
			return (PresentationUnit)R.UnitActionRangedFightSequence_defenderUnit.GetValue(self);
		}
				public static void defenderUnit(this UnitActionRangedFightSequence self, PresentationUnit value)
		{
			R.UnitActionRangedFightSequence_defenderUnit.SetValue(self, value);
		}
		
		        private static FieldInfo UnitActionRangedFightSequence_attackerAvailablePawns = typeof(UnitActionRangedFightSequence).GetField("attackerAvailablePawns", BindingFlags.Instance | BindingFlags.NonPublic);
		public static List<PresentationPawn> attackerAvailablePawns(this UnitActionRangedFightSequence self)
		{
			return (List<PresentationPawn>)R.UnitActionRangedFightSequence_attackerAvailablePawns.GetValue(self);
		}
				public static void attackerAvailablePawns(this UnitActionRangedFightSequence self, List<PresentationPawn> value)
		{
			R.UnitActionRangedFightSequence_attackerAvailablePawns.SetValue(self, value);
		}
		
		        private static FieldInfo UnitActionRangedFightSequence_defenderAvailablePawns = typeof(UnitActionRangedFightSequence).GetField("defenderAvailablePawns", BindingFlags.Instance | BindingFlags.NonPublic);
		public static List<PresentationPawn> defenderAvailablePawns(this UnitActionRangedFightSequence self)
		{
			return (List<PresentationPawn>)R.UnitActionRangedFightSequence_defenderAvailablePawns.GetValue(self);
		}
				public static void defenderAvailablePawns(this UnitActionRangedFightSequence self, List<PresentationPawn> value)
		{
			R.UnitActionRangedFightSequence_defenderAvailablePawns.SetValue(self, value);
		}
				        private static FieldInfo UnitActionRangedFightSequence_defendersToKill = typeof(UnitActionRangedFightSequence).GetField("defendersToKill", BindingFlags.Instance | BindingFlags.NonPublic);
		public static int defendersToKill(this UnitActionRangedFightSequence self)
		{
			return (int)R.UnitActionRangedFightSequence_defendersToKill.GetValue(self);
		}
				public static void defendersToKill(this UnitActionRangedFightSequence self, int value)
		{
			R.UnitActionRangedFightSequence_defendersToKill.SetValue(self, value);
		}
		
		        private static FieldInfo UnitActionRangedFightSequence_uncompletedWaitProjectileCount = typeof(UnitActionRangedFightSequence).GetField("uncompletedWaitProjectileCount", BindingFlags.Instance | BindingFlags.NonPublic);
		public static int uncompletedWaitProjectileCount(this UnitActionRangedFightSequence self)
		{
			return (int)R.UnitActionRangedFightSequence_uncompletedWaitProjectileCount.GetValue(self);
		}
				public static void uncompletedWaitProjectileCount(this UnitActionRangedFightSequence self, int value)
		{
			R.UnitActionRangedFightSequence_uncompletedWaitProjectileCount.SetValue(self, value);
		}
		
		        private static FieldInfo UnitActionRangedFightSequence_fightData = typeof(UnitActionRangedFightSequence).GetField("fightData", BindingFlags.Instance | BindingFlags.NonPublic);
		public static PresentationUnitsFightData fightData(this UnitActionRangedFightSequence self)
		{
			return (PresentationUnitsFightData)R.UnitActionRangedFightSequence_fightData.GetValue(self);
		}
				public static void fightData(this UnitActionRangedFightSequence self, PresentationUnitsFightData value)
		{
			 R.UnitActionRangedFightSequence_fightData.SetValue(self, value);
		}
					private static FieldInfo UnitAction_ShortActionName_Field = typeof(UnitAction).GetField("shortActionName", BindingFlags.Instance | BindingFlags.NonPublic);
		public static string shortActionName(this UnitAction self)
		{
			return (string)R.UnitAction_ShortActionName_Field.GetValue(self);
		}


			private static FieldInfo BattleUnit_EmpireIndex_Field = typeof(BattleUnit).GetField("EmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		public static int EmpireIndex(this BattleUnit self)
		{
			return (int)R.BattleUnit_EmpireIndex_Field.GetValue(self);
		}
					private static FieldInfo Battle_VisibilityMap_Field = typeof(Battle).GetField("VisibilityMap", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		public static Battle.BattleVisionMap VisibilityMap(this Battle self)
		{
			return (Battle.BattleVisionMap)R.Battle_VisibilityMap_Field.GetValue(self);
		}
		
		        private static MethodInfo Battle_IsVisible_Method = typeof(Battle.BattleVisionMap).GetMethod("IsVisible", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(int),
			typeof(int),
		}, null);

        public static bool IsVisible2(this Battle.BattleVisionMap self, int tileIndex, int empireBits)
		{
			return (bool)R.Battle_IsVisible_Method.Invoke(self, new object[]
			{
				tileIndex,
				empireBits,

			});
		}
		
		        private static MethodInfo Battle_GetEmpireBitsAt_Method = typeof(Battle.BattleVisionMap).GetMethod("GetEmpireBitsAt", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(int),

		}, null);

        public static int GetEmpireBitsAt2(this Battle.BattleVisionMap self, int tileIndex)
		{
			return (int)R.Battle_GetEmpireBitsAt_Method.Invoke(self, new object[]
			{
				tileIndex,


			});
		}

        private static MethodInfo SendBattleEvent_MethodInfo = typeof(BattleAbilityController).GetMethod("SendBattleEvent", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(Battle),
			typeof(BattleEventType),
            typeof(BattleEntity),
			typeof(BattleEntity),
            typeof(int),
		}, null);

        public static void SendBattleEvent(Battle battle, BattleEventType eventType, BattleEntity initiator, BattleEntity target, int layer)
		{
			R.SendBattleEvent_MethodInfo.Invoke(null, new object[]
			{
				battle,
				eventType,
                initiator,
                target,
                layer,
			});
		}
        private static MethodInfo Battle_ForecastCombatStrengthModifiers_MethodInfo = typeof(Battle).GetMethod("ForecastCombatStrengthModifiers", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(BattleUnit),
			typeof(BattleUnit),
		}, null);

        public static void ForecastCombatStrengthModifiers(this Battle self, BattleUnit initiator, BattleUnit target)
		{
			R.Battle_ForecastCombatStrengthModifiers_MethodInfo.Invoke(self, new object[]
			{
				initiator,
				target,
			});
		}
        private static MethodInfo Battle_GiveExperienceToUnits_MethodInfo = typeof(Battle).GetMethod("GiveExperienceToUnits", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(Unit),
			typeof(ISimulationBattleEntityAttackable),
            typeof(FixedPoint),
		}, null);

        public static void GiveExperienceToUnits(this Battle self, Unit attacker, ISimulationBattleEntityAttackable target, FixedPoint targetHealthDifference)
		{
			R.Battle_GiveExperienceToUnits_MethodInfo.Invoke(self, new object[]
			{
				attacker,
				target,
                targetHealthDifference,
			});
		}
        
        private static MethodInfo Battle_RaiseUnitKilledEvents_MethodInfo = typeof(Battle).GetMethod("RaiseUnitKilledEvents", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(Unit),
			typeof(Unit),
		}, null);

        public static void RaiseUnitKilledEvents2(this Battle self, Unit targetUnit, Unit attackerUnit)
		{
			R.Battle_RaiseUnitKilledEvents_MethodInfo.Invoke(self, new object[]
			{
				targetUnit,
				attackerUnit,    
			});
		}
        
        private static MethodInfo BattleUnit_SetIsInvincible_MethodInfo = typeof(BattleUnit).GetMethod("SetIsInvincible", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(bool),
			typeof(int),
		}, null);

        public static void SetIsInvincible2(this BattleUnit self, bool isInvincible, int layer)
		{
			R.BattleUnit_SetIsInvincible_MethodInfo.Invoke(self, new object[]
			{
				isInvincible,
				layer,    
			});
		}
        
        private static MethodInfo BattleUnit_IsRangedUnit_MethodInfo = typeof(BattleUnit).GetMethod("IsRangedUnit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{

		}, null);
        public static bool IsRangedUnit2(this BattleUnit self)
		{
			return (bool)R.BattleUnit_IsRangedUnit_MethodInfo.Invoke(self, new object[]
			{				
				    
			});
		}

        private static MethodInfo BattleUnit_CanRetaliate_MethodInfo = typeof(BattleUnit).GetMethod("CanRetaliate", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(int),

		}, null);
        public static bool CanRetaliate2(this BattleUnit self, int layer)
		{
			return (bool)R.BattleUnit_CanRetaliate_MethodInfo.Invoke(self, new object[]
			{				
				layer,    
			});
		}
			private static MethodInfo BattleUnit_GetBattlePosition_MethodInfo = typeof(BattleUnit).GetMethod("GetBattlePosition", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(int),

		}, null);
        public static WorldPosition GetBattlePosition2(this BattleUnit self, int layer)
		{
			return (WorldPosition)R.BattleUnit_GetBattlePosition_MethodInfo.Invoke(self, new object[]
			{				
				layer,    
			});
		}
					private static MethodInfo BattleUnit_IsIgnoreLineOfSight_MethodInfo = typeof(Unit).GetMethod("IsIgnoreLineOfSight", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
		

		}, null);
        public static bool IsIgnoreLineOfSight2(this Unit self)
		{
			return (bool)R.BattleUnit_IsIgnoreLineOfSight_MethodInfo.Invoke(self, new object[]
			{				
			 
			});
		}
					private static MethodInfo WorldPosition_ToTileIndex_MethodInfo = typeof(WorldPosition).GetMethod("ToTileIndex", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{

		}, null);
        public static int ToTileIndex2(this WorldPosition self)
		{
			return (int)R.WorldPosition_ToTileIndex_MethodInfo.Invoke(self, new object[]
			{				
  
			});
		}
			private static MethodInfo WorldPosition_GetDistance_MethodInfo = typeof(WorldPosition).GetMethod("GetDistance", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(WorldPosition)
		}, null);
        public static int GetDistance2(this WorldPosition self, WorldPosition destination)
		{
			return (int)R.WorldPosition_GetDistance_MethodInfo.Invoke(self, new object[]
			{				
				destination,
			});
		}
			private static MethodInfo LineOfSightHelper_HasLineOfSightTo_Method = typeof(LineOfSightHelper).GetMethod("HasLineOfSightTo", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(Battle),
			typeof(ILineOfSightContext),
			typeof(int),
			typeof(TargetQualification),
			typeof(int),
			typeof(int),

		}, null);
        public static bool HasLineOfSightTo2(Battle battle, ILineOfSightContext lineOfSightContext, int startingTileIndex, TargetQualification blockerTargetQualification, int targetTileIndex, int battleLayer)
		{
			return (bool)R.LineOfSightHelper_HasLineOfSightTo_Method.Invoke(null, new object[]
			{				
				battle,
				lineOfSightContext,
				startingTileIndex,
				blockerTargetQualification,
				targetTileIndex,
				battleLayer
			});
		}
			private static MethodInfo LineOfSightHelper_HasLineOfSightObstacleTo_Method = typeof(LineOfSightHelper).GetMethod("HasLineOfSightObstacleTo", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(Battle),
			typeof(ILineOfSightContext),
			typeof(int),
			typeof(TargetQualification),
			typeof(int),
			typeof(int),
			
		}, null);
        public static bool HasLineOfSightObstacleTo2(Battle battle, ILineOfSightContext lineOfSightContext, int startingTileIndex, TargetQualification blockerTargetQualification, int targetTileIndex, int battleLayer)
		{
			return (bool)R.LineOfSightHelper_HasLineOfSightObstacleTo_Method.Invoke(null, new object[]
			{				
				battle,
				lineOfSightContext,
				startingTileIndex,
				blockerTargetQualification,
				targetTileIndex,
				battleLayer
			});
		}

        private static MethodInfo BattleUnit_RegisterKill_MethodInfo = typeof(BattleUnit).GetMethod("RegisterKill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(int),
		}, null);

        public static void RegisterKill2(this BattleUnit self, int layer)
		{
			R.BattleUnit_RegisterKill_MethodInfo.Invoke(self, new object[]
			{				
				layer,    
			});
		}
        private static MethodInfo PresentationChoreographyController_CreateActionsForMeleeFightSequence_MethodInfo = typeof(PresentationChoreographyController).GetMethod("CreateActionsForMeleeFightSequence", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
            typeof(FightSequence),
		}, null);

        public static void CreateActionsForMeleeFightSequence(PresentationChoreographyController self, ref FightSequence fightSequence)
		{
			R.PresentationChoreographyController_CreateActionsForMeleeFightSequence_MethodInfo.Invoke(self, new object[]
			{				
				fightSequence,    
			});
		}
				private static MethodInfo UnitAction_BaseClass = typeof(UnitActionRangedFightSequence).GetMethod("CreateUnitAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(FightSequence),
			typeof(ActionScope),
			typeof(bool),
		}, null);

        public static void CreateUnitAction2(IntPtr self, ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction = true)
		{
			R.UnitAction_BaseClass.Invoke(self, new object[]
			{				
				fightSequence,
				actionScope,
				blockingAction,
			});
		}

			private static MethodInfo DepartmentOfBattles_GetRangedAttackFailureFlags_Method = typeof(DepartmentOfBattles).GetMethod("GetRangedAttackFailureFlags", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(Battle),
			typeof(BattleUnit),
			typeof(int),
			typeof(int),
			typeof(bool),
		}, null);

        public static BattleAttackFailureFlags GetRangedAttackFailureFlags(Battle battle, BattleUnit attacker, int attackerDestinationTileIndex, int targetTileIndex, bool logTraces)
		{
			return (BattleAttackFailureFlags)R.DepartmentOfBattles_GetRangedAttackFailureFlags_Method.Invoke(null, new object[]
			{				
				battle,
				attacker,
				attackerDestinationTileIndex,
				targetTileIndex,
				logTraces,

			});
		}
		// 	private static MethodInfo PawnActionRangedStartAttackParameters_PawnActionRangedStartAttackParameters_Method = typeof(PawnActionRangedStartAttackParameters).GetMethod("PawnActionRangedStartAttackParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		// {
		// 	typeof(PresentationPawn),
		// 	typeof(PresentationPawn),
		// 	typeof(bool),
		// 	typeof(bool),
		// 	typeof(bool),
		// 	typeof(bool),
		// }, null);

        // public static PresentationChoreographyActionParameters PawnActionRangedStartAttackParameters2(this PawnActionRangedStartAttack self, PresentationPawn shooter, PresentationPawn target, bool useAlternateAttack, bool doStayInIdleAfterLoops, bool lookAtTarget, bool endOnAttackStart = false)
		// {
		// 	return (PresentationChoreographyActionParameters)R.PawnActionRangedStartAttackParameters_PawnActionRangedStartAttackParameters_Method.Invoke(self, new object[]
		// 	{		
		// 		shooter,
		// 		target,
		// 		useAlternateAttack,
		// 		doStayInIdleAfterLoops,
		// 		lookAtTarget,
		// 		endOnAttackStart,
		// 	});
		// }
		// 			private static MethodInfo PawnActionRangedStartAttackParameters_PawnActionRangedStartAttackParameters_Method2 = typeof(PawnActionRangedStartAttackParameters).GetMethod("PawnActionRangedStartAttackParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		// {
		// 	typeof(PawnRangedFightSequence),
		// 	typeof(bool),
		// 	typeof(bool),
		// 	typeof(bool),
		// 	typeof(bool),
		// }, null);

        // public static PresentationChoreographyActionParameters PawnActionRangedStartAttackParameters2(PawnRangedFightSequence sequence, bool useAlternateAttack, bool doStayInIdleAfterLoops, bool lookAtTarget, bool endOnAttackStart = false)
		// {
		// 	return (PresentationChoreographyActionParameters)R.PawnActionRangedStartAttackParameters_PawnActionRangedStartAttackParameters_Method2.Invoke(null, new object[]
		// 	{		
		// 		sequence,
		// 		useAlternateAttack,
		// 		doStayInIdleAfterLoops,
		// 		lookAtTarget,
		// 		endOnAttackStart,
		// 	});
		// }
			private static MethodInfo BattleEntityWithSimulation_HasDescriptor_Method = typeof(BattleEntityWithSimulation<Unit>).GetMethod("HasDescriptor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(StaticString),
		}, null);

        public static bool HasDescriptor2(this BattleEntityWithSimulation<Unit> self, StaticString value)
		{
			return (bool)R.BattleEntityWithSimulation_HasDescriptor_Method.Invoke(self, new object[]
			{				
				value,
			});
		}
			private static MethodInfo PawnAction_SetPawnActionParameters_Method = typeof(PawnAction).GetMethod("SetPawnActionParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(PresentationChoreographyActionParameters),
		}, null);

        public static void SetPawnActionParameters2(this PawnAction self, PresentationChoreographyActionParameters pawnActionParameters)
		{	
			R.PawnAction_SetPawnActionParameters_Method.Invoke(self, new object[]
			{				
				pawnActionParameters,
			});
		}
		// 	private static MethodInfo PawnActionWaitIdleParameters_Class_Method = typeof(PawnActionWaitIdleParameters).GetMethod("PawnActionWaitIdleParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		// {
		// 	typeof(PresentationSubPawn[]),
		// 	typeof(int),
		// 	typeof(TagMatchType),
		// 	typeof(bool),

		// }, null);		

        // public static PresentationChoreographyActionParameters PawnActionWaitIdleParameters2(PresentationSubPawn[] subPawns, int subPawnsCount, TagMatchType flag = TagMatchType.CurrentState, bool useRelativeTags = true)
		// {
		// 	return (PresentationChoreographyActionParameters)R.PawnActionWaitIdleParameters_Class_Method.Invoke(null, new object[]
		// 	{				
		// 		subPawns,
		// 		subPawnsCount,
		// 		flag,
		// 		useRelativeTags,
		// 	});
		// }
		// private static MethodInfo PawnActionTriggerHitParameters_Class_Method = typeof(PawnActionTriggerHitParameters).GetMethod("PawnActionTriggerHitParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		// {
		// 	typeof(bool),
		// 	typeof(bool),
		// 	typeof(PresentationPawn),
		// 	typeof(bool),
		// 	typeof(bool),

		// }, null);		

        // public static PresentationChoreographyActionParameters PawnActionTriggerHitParameters2(bool isFrontHit, bool needsProtection, PresentationPawn striker, bool skipIfProtecting = false, bool isFromCharge = false)
		// {
		// 	return (PresentationChoreographyActionParameters)R.PawnActionTriggerHitParameters_Class_Method.Invoke(null, new object[]
		// 	{				
		// 		isFrontHit,
		// 		needsProtection,
		// 		striker,
		// 		skipIfProtecting,
		// 		isFromCharge,
		// 	});
		// }

							private static MethodInfo PawnActionSetProtectionAnimationParameters_Class_Method = typeof(PawnActionSetProtectionAnimationParameters).GetMethod("PawnActionSetProtectionAnimationParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(ProtectAnimationType),
			typeof(AnimationMetaState),
			typeof(bool),
			typeof(bool),

		}, null);		

        public static PresentationChoreographyActionParameters PawnActionSetProtectionAnimationParameters2(ProtectAnimationType protect, AnimationMetaState requiredMetaState = AnimationMetaState.Unset, bool teleportToAnimation = false, bool delay = false)
		{
			return (PresentationChoreographyActionParameters)R.PawnActionSetProtectionAnimationParameters_Class_Method.Invoke(null, new object[]
			{				
				protect,
				requiredMetaState,
				teleportToAnimation,
				delay,
			});
		}

							private static MethodInfo PawnActionKillPawnParameters_Class_Method = typeof(PawnActionKillPawnParameters).GetMethod("PawnActionKillPawnParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(PresentationPawn),
			typeof(bool),
			typeof(bool),
			typeof(int),

		}, null);		

        public static PresentationChoreographyActionParameters PawnActionKillPawnParameters2(PresentationPawn killer, bool forceKillAnim, bool killExtraNearby, int forceKillChoice = -1)
		{
			return (PresentationChoreographyActionParameters)R.PawnActionKillPawnParameters_Class_Method.Invoke(null, new object[]
			{				
				killer,
				forceKillAnim,
				killExtraNearby,
				forceKillChoice,
			});
		}
		private static MethodInfo PawnActionKillPawnParameters_Class_Method2 = typeof(PawnActionKillPawnParameters).GetMethod("PawnActionKillPawnParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(PresentationPawn),
			typeof(bool),
			typeof(bool),
			typeof(bool),
			typeof(int),
			typeof(int),

		}, null);		

        public static PresentationChoreographyActionParameters PawnActionKillPawnParameters2(PresentationPawn killer, bool forceKillAnim, bool killExtraNearby, bool killExtraIgnoreDistance, int killExtraMaxCount, int forceKillChoice = -1)
		{
			return (PresentationChoreographyActionParameters)R.PawnActionKillPawnParameters_Class_Method2.Invoke(null, new object[]
			{				
				killer,
				forceKillAnim,
				killExtraNearby,
				killExtraIgnoreDistance,
				killExtraMaxCount,
				forceKillChoice,

			});
		}

			private static MethodInfo PawnActionSetAnimationIntParameters_Class_Method = typeof(PawnActionSetAnimationIntParameters).GetMethod("PawnActionSetAnimationIntParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(int),
			typeof(int),
			typeof(PresentationSubPawn[]),
			typeof(int),
		}, null);		

        public static PresentationChoreographyActionParameters PawnActionSetAnimationIntParameters(int variableID, int value, PresentationSubPawn[] subPawns, int subPawnsCount)
		{
			return (PresentationChoreographyActionParameters)R.PawnActionSetAnimationIntParameters_Class_Method.Invoke(null, new object[]
			{				
				variableID,
				value,
				subPawns,
				subPawnsCount,
			});
		}
	
		public static object InvokeNotOverride(this MethodInfo methodInfo, object targetObject, params object[] arguments) 
		{
			var parameters = methodInfo.GetParameters();

			if (parameters.Length == 0) {
				if (arguments != null && arguments.Length != 0) 
					throw new Exception("Arguments cont doesn't match");
			} else {
				if (parameters.Length != arguments.Length)
					throw new Exception("Arguments cont doesn't match");
			}

			Type returnType = null;
			if (methodInfo.ReturnType != typeof(void)) {
				returnType = methodInfo.ReturnType;
			}

			var type = targetObject.GetType();
			var dynamicMethod = new DynamicMethod("", returnType, 
					new Type[] { type, typeof(System.Object) }, type);

			var iLGenerator = dynamicMethod.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0); // this

			for (var i = 0; i < parameters.Length; i++) {
				var parameter = parameters[i];

				iLGenerator.Emit(OpCodes.Ldarg_1); // load array argument

				// get element at index
				iLGenerator.Emit(OpCodes.Ldc_I4_S, i); // specify index
				iLGenerator.Emit(OpCodes.Ldelem_Ref); // get element

				var parameterType = parameter.ParameterType;
				if (parameterType.IsPrimitive) {
					iLGenerator.Emit(OpCodes.Unbox_Any, parameterType);
				} else if (parameterType == typeof(object)) {
					// do nothing
				} else {
					iLGenerator.Emit(OpCodes.Castclass, parameterType);
				}
			}

			iLGenerator.Emit(OpCodes.Call, methodInfo);
			iLGenerator.Emit(OpCodes.Ret);

			return dynamicMethod.Invoke(null, new object[] { targetObject, arguments });
		}
    }
	
}