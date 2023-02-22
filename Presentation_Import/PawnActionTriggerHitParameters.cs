namespace Amplitude.Mercury.Presentation
{
	internal class PawnActionTriggerHitParameters : PresentationChoreographyActionParameters
	{
		public bool IsFrontHit;

		public bool NeedsProtection;

		public bool SkipIfProtecting;

		public bool IsFromCharge;

		public PresentationPawn Striker;

		public PawnActionTriggerHitParameters(bool isFrontHit, bool needsProtection, PresentationPawn striker, bool skipIfProtecting = false, bool isFromCharge = false)
		{
			IsFrontHit = isFrontHit;
			NeedsProtection = needsProtection;
			Striker = striker;
			SkipIfProtecting = skipIfProtecting;
			IsFromCharge = isFromCharge;
		}
	}
}
