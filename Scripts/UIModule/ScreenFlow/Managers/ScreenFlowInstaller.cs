namespace GameFoundation.Scripts.ScreenFlow.Managers
{
    using GameFoundation.Scripts.ScreenFlow.Signals;
    using GameFoundation.Scripts.Utilities.Extension;
    using Zenject;

    public class ScreenFlowInstaller: Installer<ScreenFlowInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<SceneDirector>().AsCached();
            this.Container.BindInterfacesAndSelfTo<ScreenManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            this.Container.DeclareSignal<StartLoadingNewSceneSignal>();
            this.Container.DeclareSignal<FinishLoadingNewSceneSignal>();
            this.Container.DeclareSignal<ScreenCloseSignal>();
            this.Container.DeclareSignal<ScreenShowSignal>();
            this.Container.DeclareSignal<ScreenHideSignal>();
            this.Container.DeclareSignal<ManualInitScreenSignal>();
            this.Container.DeclareSignal<ScreenSelfDestroyedSignal>();
            this.Container.DeclareSignal<PopupShowedSignal>();
            this.Container.DeclareSignal<PopupHiddenSignal>();
            this.Container.DeclareSignal<PopupBlurBgShowedSignal>();
            
            this.Container.BindIFactory<AutoCooldownTimer>().FromPoolableMemoryPool();
        }
    }
}