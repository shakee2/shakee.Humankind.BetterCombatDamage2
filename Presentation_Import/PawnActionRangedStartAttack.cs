using System;
using Amplitude.Mercury.Data.World;

namespace Amplitude.Mercury.Presentation
{
	public class PawnActionRangedStartAttack : PawnAction
	{
		public bool HasAttackStarted;

		public int ProjectileLoopIndex = -1;

		private bool isReadyToStart;

		private PawnRangedFightSequence currentSequence;

		private UnitActionRangedFightSequenceDefender parent;

		private int sequenceCount;

		private bool endOnAttackStart;

		private float projectileTime;

		private bool safeguardFromMissedMecanimEvent;

		private bool missedMecanimEventPreviousFrame;

		private int attackStateTag;

		private int previousLoopIndex = -1;

		public event Action<float> FirstProjectileLoopSent;

		public override void StartPawnAction()
		{
			base.StartPawnAction();
			if (pawn == null)
			{
				OnPawnActionEnd();
				return;
			}
			if (pawn.AttackFSM == null)
			{
				OnPawnActionEnd();
				return;
			}
			parent = Parent as UnitActionRangedFightSequenceDefender;
			isReadyToStart = !pawn.AttackFSM.IsRunning;
			if (isReadyToStart)
			{
				OnReadyToStart();
			}
		}

		public override void UpdatePawnAction()
		{
			base.UpdatePawnAction();
			if (pawn == null || pawn.IsDead)
			{
				OnPawnActionEnd();
			}
			else if (!isReadyToStart)
			{
				isReadyToStart = !pawn.AttackFSM.IsRunning;
				if (isReadyToStart)
				{
					OnReadyToStart();
				}
			}
			else if (safeguardFromMissedMecanimEvent)
			{
				bool flag = pawn.MainSubPawn.PawnAnimator.GetCurrentStateTagHash() == attackStateTag;
				float currentStateNormalizeTime = pawn.MainSubPawn.PawnAnimator.GetCurrentStateNormalizeTime();
				float num = (float)(previousLoopIndex + 1) + projectileTime;
				bool flag2 = !flag || currentStateNormalizeTime > num;
				if (flag2 && missedMecanimEventPreviousFrame)
				{
					Diagnostics.LogWarning($"[PresentationChoreography] PawnActionRangedStartAttack {pawn} missed FireProjectile mecanim event. State has tag? {flag}. Current time is {currentStateNormalizeTime} and expected projectile time was {num}.");
					Pawn_ProjectileSent(0f);
				}
				missedMecanimEventPreviousFrame = flag2;
			}
		}

		protected override void OnTimedOut()
		{
			battle.OnTimeOut();
			if (!PresentationChoreographyAction.EndActionOnFailSafeLifeTime)
			{
				Diagnostics.LogError("PawnAction(" + base.ShortPawnActionName + ") has timed out!");
			}
		}

		private void OnReadyToStart()
		{
			PawnActionRangedStartAttackParameters pawnActionRangedStartAttackParameters = base.Parameters as PawnActionRangedStartAttackParameters;
			bool useAlternateAttack = pawnActionRangedStartAttackParameters.UseAlternateAttack;
			currentSequence = pawnActionRangedStartAttackParameters.Sequence;
			sequenceCount = currentSequence.GetSequenceCount();
			endOnAttackStart = pawnActionRangedStartAttackParameters.EndOnAttackStart;
			if (pawnActionRangedStartAttackParameters.LookAtTarget && !pawn.AvoidPawnFacingStaticRotation)
			{
				pawn.RotationFSM.StartPawnToLook(currentSequence.Targets[0], Presentation.PresentationChoreographyController.LookAtPawnDeltaAngle, pawn.SubPawns, RotationAnimationPolicy.NoAnimation);
			}
			PawnActionComplete allStepsCompleted = (endOnAttackStart ? new PawnActionComplete(OnAttackInterrupted) : (((pawn.AnimationCapabilitiesAllFighters & PawnAnimationCapability.PreparedAttackLoop) == 0) ? new PawnActionComplete(OnSimpleAttackEndStep) : new PawnActionComplete(OnAttackCompleted)));
			if (!endOnAttackStart)
			{
				pawn.ProjectileSent += Pawn_ProjectileSent;
				bool isSimpleAttack = (pawn.MainSubPawn.AnimationCapabilities & PawnAnimationCapability.PreparedAttackLoop) == 0;
				bool isPending = !pawn.MainSubPawn.PawnAnimator.GetBool(AnimationVariableNames.Fighting);
				int preparedAttackStateIndex = PresentationChoreographyController.GetPreparedAttackStateIndex(isSimpleAttack, isPending, useAlternateAttack);
				projectileTime = pawn.MainSubPawn.PawnAnimator.GetProjectileTime(preparedAttackStateIndex);
			}
			_ = PresentationChoreographyAction.VerboseLog;
			pawn.AttackFSM.Start(sequenceCount, pawnActionRangedStartAttackParameters.DoStayInIdleAfterLoops, useAlternateAttack, useDelay: true, OnAttackStarted, allStepsCompleted);
		}

		private void Pawn_ProjectileSent(float duration)
		{
			ProjectileLoopIndex = (int)pawn.MainSubPawn.PawnAnimator.GetCurrentStateNormalizeTime();
			_ = PresentationChoreographyAction.VerboseLog;
			if (ProjectileLoopIndex <= previousLoopIndex)
			{
				_ = PresentationChoreographyAction.VerboseLog;
				return;
			}
			int num = ProjectileLoopIndex - previousLoopIndex;
			if (this.FirstProjectileLoopSent != null)
			{
				this.FirstProjectileLoopSent?.Invoke(duration);
			}
			else
			{
				_ = PresentationChoreographyAction.VerboseLog;
			}
			for (int i = 0; i < num; i++)
			{
				if (currentSequence.NextSequence == null)
				{
					safeguardFromMissedMecanimEvent = false;
					break;
				}
				currentSequence = currentSequence.NextSequence;
				int defenderActionGroup = (GroupAction + 1) * 100 + previousLoopIndex + i + 2;
				PawnActionRangedStartAttack rangedAttackAction = ((actionStatus == ActionStatus.Ended) ? null : this);
				parent.AddPawnRangedFightSequence(currentSequence, GroupAction, defenderActionGroup, rangedAttackAction, previousLoopIndex + i + 2);
			}
			previousLoopIndex = ProjectileLoopIndex;
		}

		private void OnAttackStarted(PresentationPawn pawn)
		{
			_ = PresentationChoreographyAction.VerboseLog;
			HasAttackStarted = true;
			if (endOnAttackStart)
			{
				OnPawnActionEnd();
				return;
			}
			safeguardFromMissedMecanimEvent = true;
			attackStateTag = (((base.pawn.AnimationCapabilitiesAllFighters & PawnAnimationCapability.PreparedAttackLoop) != 0) ? AnimationVariableNames.TagAttackLoop : AnimationVariableNames.TagSimpleAttack);
		}

		private void OnAttackInterrupted(PresentationPawn pawn)
		{
			if (base.ActionStatus == ActionStatus.Running)
			{
				if (PresentationChoreographyAction.VerboseLog)
				{
					Diagnostics.LogWarning($"[PresentationChoreography] PawnActionRangedStartAttack {base.pawn} OnAttackInterrupted.");
				}
				OnPawnActionEnd();
			}
		}

		private void OnSimpleAttackEndStep(PresentationPawn pawn)
		{
			_ = PresentationChoreographyAction.VerboseLog;
			base.pawn.DoWaitForAnimationCompletion(base.pawn.MainSubPawn, OnAttackCompleted);
		}

		private void OnAttackCompleted(PresentationPawn pawn)
		{
			_ = PresentationChoreographyAction.VerboseLog;
			base.pawn.ProjectileSent -= Pawn_ProjectileSent;
			if (actionStatus != ActionStatus.Ended)
			{
				OnPawnActionEnd();
			}
			if (safeguardFromMissedMecanimEvent && missedMecanimEventPreviousFrame)
			{
				Diagnostics.LogWarning($"[PresentationChoreography] PawnActionRangedStartAttack {base.pawn} OnAttackCompleted while safeguard has not been catched.");
				Pawn_ProjectileSent(0f);
			}
		}
	}
}
