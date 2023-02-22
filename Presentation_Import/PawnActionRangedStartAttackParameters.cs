namespace Amplitude.Mercury.Presentation
{
	internal class PawnActionRangedStartAttackParameters : PresentationChoreographyActionParameters
	{
		public bool UseAlternateAttack;

		public bool DoStayInIdleAfterLoops;

		public bool LookAtTarget;

		public PawnRangedFightSequence Sequence;

		public bool EndOnAttackStart;

		public PawnActionRangedStartAttackParameters(PawnRangedFightSequence sequence, bool useAlternateAttack, bool doStayInIdleAfterLoops, bool lookAtTarget, bool endOnAttackStart = false)
		{
			Initialize(sequence, useAlternateAttack, doStayInIdleAfterLoops, lookAtTarget, endOnAttackStart);
		}

		public PawnActionRangedStartAttackParameters(PresentationPawn shooter, PresentationPawn target, bool useAlternateAttack, bool doStayInIdleAfterLoops, bool lookAtTarget, bool endOnAttackStart = false)
		{
			PawnRangedFightSequence pawnRangedFightSequence = new PawnRangedFightSequence(shooter, target, dies: true, delay: false, miss: false, 0f);
			if (Presentation.PresentationChoreographyController.RangedFightSequencesByPawn.ContainsKey(shooter))
			{
				Presentation.PresentationChoreographyController.RangedFightSequencesByPawn[shooter] = pawnRangedFightSequence;
			}
			else
			{
				Presentation.PresentationChoreographyController.RangedFightSequencesByPawn.Add(shooter, pawnRangedFightSequence);
			}
			Initialize(pawnRangedFightSequence, useAlternateAttack, doStayInIdleAfterLoops, lookAtTarget, endOnAttackStart);
		}

		private void Initialize(PawnRangedFightSequence sequence, bool useAlternateAttack, bool doStayInIdleAfterLoops, bool lookAtTarget, bool endOnAttackStart = false)
		{
			Sequence = sequence;
			UseAlternateAttack = useAlternateAttack;
			DoStayInIdleAfterLoops = doStayInIdleAfterLoops;
			LookAtTarget = lookAtTarget;
			EndOnAttackStart = endOnAttackStart;
		}
	}
}
