namespace Amplitude.Mercury.Presentation
{
	public class PawnActionWaitProjectileParameters : PresentationChoreographyActionParameters
	{
		public bool SafeguardFromMissingMecanimEvent;

		public PawnActionRangedStartAttack RangedAttackAction;

		public int ProjectileLoopIndex;

		public PawnActionWaitProjectileParameters(bool safeguardFromMissingMecanimEvent, PawnActionRangedStartAttack rangedAttackAction, int projectileLoopIndex)
		{
			SafeguardFromMissingMecanimEvent = safeguardFromMissingMecanimEvent;
			RangedAttackAction = rangedAttackAction;
			ProjectileLoopIndex = projectileLoopIndex;
		}
	}
}
