namespace GameFoundation.Scripts.Utilities.GameQueueAction
{
    using DG.Tweening;

    public class PlayTweenQueueAction : BaseQueueAction
    {
        private Tween tween;

        public PlayTweenQueueAction(Tween tween, string actionId, string location) : base(actionId, location)
        {
            this.tween = tween;
            TweenExtensions.Pause(this.tween);
        }

        public override void Execute()
        {
            this.tween.OnComplete(this.Complete);
            this.tween.OnKill(this.Complete);
            base.Execute();
        }

        protected override void Action()
        {
            base.Action();
            TweenExtensions.Play(this.tween);
        }
    }
}