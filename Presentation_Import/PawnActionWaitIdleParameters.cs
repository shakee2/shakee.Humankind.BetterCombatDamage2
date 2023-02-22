namespace Amplitude.Mercury.Presentation
{
	internal class PawnActionWaitIdleParameters : PresentationChoreographyActionParameters
	{
		public TagMatchType Flag;

		public bool UseRelativeTags = true;

		public PresentationSubPawn[] SubPawns;

		public int SubPawnsCount;

		public PawnActionWaitIdleParameters(PresentationSubPawn[] subPawns, int subPawnsCount, TagMatchType flag = TagMatchType.CurrentState, bool useRelativeTags = true)
		{
			Flag = flag;
			UseRelativeTags = useRelativeTags;
			SubPawns = subPawns;
			SubPawnsCount = subPawnsCount;
		}
	}
}
