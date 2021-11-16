namespace GameFoundation.Scripts.ScreenFlow.Signals
{
    using GameFoundation.Scripts.ScreenFlow.BaseScreen.Presenter;

    public class ScreenHideSignal
    {
        public IScreenPresenter ScreenPresenter;
    }
    
    public class ScreenShowSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class ManualInitScreenSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class ScreenSelfDestroyedSignal
    {
        public IScreenPresenter ScreenPresenter;
    }
    
    public class ForceDestroyScreenSignal
    {
        public IScreenPresenter ScreenPresenter;
    }
}