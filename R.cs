using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Amplitude;
using Amplitude.Framework.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Simulation;

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
        private static FieldInfo MajorEmpire_Armies_FieldInfo = typeof(MajorEmpire).GetField("Armies", BindingFlags.Instance | BindingFlags.NonPublic);
		public static ReferenceCollection<Army> Armies(this MajorEmpire self)
		{
			return (ReferenceCollection<Army>)R.MajorEmpire_Armies_FieldInfo.GetValue(self);
		}
        private static FieldInfo Army_Units_FieldInfo = typeof(Army).GetField("Units", BindingFlags.Instance | BindingFlags.NonPublic);
		public static ReferenceCollection<Unit> Units(this Army self)
		{
			return (ReferenceCollection<Unit>)R.Army_Units_FieldInfo.GetValue(self);
		}
        private static FieldInfo Army_StealthMax_FieldInfo = typeof(Army).GetField("StealthMax", BindingFlags.Instance | BindingFlags.NonPublic);
		public static Property StealthMax(this Army self)
		{
			return (Property)R.Army_StealthMax_FieldInfo.GetValue(self);
		}
        private static FieldInfo Unit_StealthMax_FieldInfo = typeof(Unit).GetField("StealthMax", BindingFlags.Instance | BindingFlags.NonPublic);
		public static Property StealthMax(this Unit self)
		{
			return (Property)R.Unit_StealthMax_FieldInfo.GetValue(self);
		}
        private static FieldInfo Unit_StealthRegen_FieldInfo = typeof(Unit).GetField("StealthRegen", BindingFlags.Instance | BindingFlags.NonPublic);
		public static Property StealthRegen(this Unit self)
		{
			return (Property)R.Unit_StealthRegen_FieldInfo.GetValue(self);
		}
        private static FieldInfo Unit_StealthValue_FieldInfo = typeof(Unit).GetField("StealthValue", BindingFlags.Instance | BindingFlags.NonPublic);
		public static Property StealthValue(this Unit self)
		{
			return (Property)R.Unit_StealthValue_FieldInfo.GetValue(self);
		}
        private static FieldInfo Army_WorldPosition_FieldInfo = typeof(Army).GetField("WorldPosition", BindingFlags.Instance | BindingFlags.NonPublic);
		public static WorldPosition WorldPosition(this Army self)
		{
			return (WorldPosition)R.Army_WorldPosition_FieldInfo.GetValue(self);
		}      
        private static FieldInfo SandBox_World_FieldInfo = typeof(Sandbox).GetField("World", BindingFlags.Instance | BindingFlags.NonPublic);
		public static World World(this Sandbox self)
		{
			return (World)R.SandBox_World_FieldInfo.GetValue(self);
		}    
        private static FieldInfo World_TileInfo_FieldInfo = typeof(World).GetField("TileInfo", BindingFlags.Instance | BindingFlags.NonPublic);
		public static ArrayWithFrame<TileInfo> TileInfo(this World self)
		{
			return (ArrayWithFrame<TileInfo>)R.World_TileInfo_FieldInfo.GetValue(self);
		}  
        
		private static MethodInfo StealthAncillary_GetStealthFailureFlags_MethodInfo = typeof(StealthAncillary).GetMethod("GetStealthRegenFailureFlag", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(UnitCollection),
			typeof(bool),
		}, null);

        public static StealthRegenFailureFlags GetStealthRegenFailureFlag2(UnitCollection collection, bool ignoreDistrict)
		{
			return (StealthRegenFailureFlags)R.StealthAncillary_GetStealthFailureFlags_MethodInfo.Invoke(null, new object[]
			{
				collection,
				ignoreDistrict,
			});
		}
        private static MethodInfo DiplomaticRelation_GetEmpireEmbassy_MethodInfo = typeof(DiplomaticRelation).GetMethod("GetEmpireEmbassy", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(int),
		}, null);

        public static DiplomaticAmbassy GetEmpireEmbassy(this DiplomaticRelation self, int myEmpireIndex)
		{
			return (DiplomaticAmbassy)R.DiplomaticRelation_GetEmpireEmbassy_MethodInfo.Invoke(self, new object[]
			{
				myEmpireIndex,
			});
		}
        private static MethodInfo StealthAncillary_GetBestStealthDetection_MethodInfo = typeof(StealthAncillary).GetMethod("GetBestStealthDetection", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(MajorEmpire),
			typeof(int),
            typeof(int),
		}, null);

        public static MajorEmpireTerritory GetBestStealthDetection(this StealthAncillary self, MajorEmpire observerEmpire, int territoryIndex, int excludedEmpireIndex = -1)
		{
			return (MajorEmpireTerritory)R.StealthAncillary_GetBestStealthDetection_MethodInfo.Invoke(self, new object[]
			{
				observerEmpire,
				territoryIndex,
                excludedEmpireIndex,
			});
		}
        
        private static FieldInfo MajorEmpire_Index_FieldInfo = typeof(MajorEmpire).GetField("Index", BindingFlags.Instance | BindingFlags.NonPublic);
        public static int Index(this MajorEmpire self)
		{
			return (int)R.MajorEmpire_Index_FieldInfo.GetValue(self);
		} 
        private static FieldInfo DiplomaticRelation_LeftEmbassyIndex_FieldInfo = typeof(DiplomaticRelation).GetField("LeftEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
        public static int LeftEmpireIndex(this DiplomaticRelation self)
		{
			return (int)R.DiplomaticRelation_LeftEmbassyIndex_FieldInfo.GetValue(self);
		} 
        private static FieldInfo DiplomaticRelation_RightEmbassyIndex_FieldInfo = typeof(DiplomaticRelation).GetField("RightEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
        public static int RightEmpireIndex(this DiplomaticRelation self)
		{
			return (int)R.DiplomaticRelation_RightEmbassyIndex_FieldInfo.GetValue(self);
		} 
        private static FieldInfo DiplomaticRelation_LeftEmbassy_FieldInfo = typeof(DiplomaticRelation).GetField("LeftAmbassy", BindingFlags.Instance | BindingFlags.NonPublic);
        public static Reference<DiplomaticAmbassy> LeftAmbassy(this DiplomaticRelation self)
		{
			return (Reference<DiplomaticAmbassy>)R.DiplomaticRelation_LeftEmbassy_FieldInfo.GetValue(self);
		} 
        private static FieldInfo DiplomaticRelation_RightEmbassy_FieldInfo = typeof(DiplomaticRelation).GetField("RightAmbassy", BindingFlags.Instance | BindingFlags.NonPublic);
        public static Reference<DiplomaticAmbassy> RightAmbassy(this DiplomaticRelation self)
		{
			return (Reference<DiplomaticAmbassy>)R.DiplomaticRelation_RightEmbassy_FieldInfo.GetValue(self);
		} 
  
        private static FieldInfo DiplomaticAmbassy_CurrentAbilities_FieldInfo = typeof(DiplomaticAmbassy).GetField("CurrentAbilities", BindingFlags.Instance | BindingFlags.NonPublic);
        public static DiplomaticAbility CurrentAbilities(this DiplomaticAmbassy self)
		{
			return (DiplomaticAbility)R.DiplomaticAmbassy_CurrentAbilities_FieldInfo.GetValue(self);
		}


        private static FieldInfo MajorEmpire_DiplomaticRelationByOtherEmpireIndex_FieldInfo = typeof(MajorEmpire).GetField("DiplomaticRelationByOtherEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
        public static DiplomaticRelation[] DiplomaticRelationByOtherEmpireIndex(this MajorEmpire self)
		{
			return (DiplomaticRelation[])R.MajorEmpire_DiplomaticRelationByOtherEmpireIndex_FieldInfo.GetValue(self);
		}
        private static FieldInfo StealthAncillary_Territories_FieldInfo = typeof(StealthAncillary).GetField("majorEmpireTerritories", BindingFlags.Instance | BindingFlags.NonPublic);
        public static MajorEmpireTerritory[] majorEmpireTerritories(this StealthAncillary self)
		{
			return (MajorEmpireTerritory[])R.StealthAncillary_Territories_FieldInfo.GetValue(self);
		}
        
		private static FieldInfo SandboxManager_Sandbox_FieldInfo = typeof(SandboxManager).GetField("Sandbox", BindingFlags.Static | BindingFlags.NonPublic);

		private static PropertyInfo Sandbox_Turn_PropertyInfo = typeof(Sandbox).GetProperty("Turn", BindingFlags.Instance | BindingFlags.NonPublic);
		private static PropertyInfo Sandbox_SandboxThreadStartSettings_PropertyInfo = typeof(Sandbox).GetProperty("SandboxThreadStartSettings", BindingFlags.Instance | BindingFlags.NonPublic);
		private static PropertyInfo SandboxThreadStartSettings_Parameter_PropertyInfo = typeof(SandboxThreadStartSettings).GetProperty("Parameter", BindingFlags.Instance | BindingFlags.NonPublic);

		public static Sandbox SandboxManager_Sandbox()
		{
			return (Sandbox)R.SandboxManager_Sandbox_FieldInfo.GetValue(null);
		}
		public static int Turn(this Sandbox self)
		{
			return (int)R.Sandbox_Turn_PropertyInfo.GetValue(self);
		}
		public static SandboxThreadStartSettings SandboxThreadStartSettings(Sandbox self)
		{
			return (SandboxThreadStartSettings)R.Sandbox_SandboxThreadStartSettings_PropertyInfo.GetValue(self);
		}
		public static void SandboxThreadStartSettings(Sandbox self, object parameter)
		{
			R.Sandbox_SandboxThreadStartSettings_PropertyInfo.SetValue(self, parameter);
		}
		public static object Parameter(this SandboxThreadStartSettings self)
		{
			return (object)R.SandboxThreadStartSettings_Parameter_PropertyInfo.GetValue(self);
		}
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
				private static MethodInfo UnitAction_BaseClass = typeof(UnitAction).GetMethod("CreateUnitAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(FightSequence),
			typeof(ActionScope),
			typeof(bool),
		}, null);

        public static void CreateUnitAction(this UnitActionRangedFightSequence self, ref FightSequence fightSequence, ActionScope actionScope, bool blockingAction = true)
		{
			R.UnitAction_BaseClass.InvokeNotOverride(self, new object[]
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
			private static MethodInfo PawnActionRangedStartAttackParameters_PawnActionRangedStartAttackParameters_Method = typeof(PawnActionRangedStartAttack).GetMethod("SetPawnActionParameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
		{
			typeof(PresentationChoreographyActionParameters),
		}, null);

        public static void PawnActionRangedStartAttackParameters2(this PawnActionRangedStartAttack self, PresentationChoreographyActionParameters pawnActionParameters)
		{
			R.PawnActionRangedStartAttackParameters_PawnActionRangedStartAttackParameters_Method.Invoke(self, new object[]
			{				
				pawnActionParameters,
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
					new Type[] { type, typeof(Object) }, type);

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