namespace GameFoundation.Scripts.ScreenFlow.Managers
{
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Every Mono Scene Installer will be inherited this class
    /// </summary>
    public class BaseSceneInstaller : MonoInstaller
    {
        /// <summary>
        /// Instance of Root UI Canvas on Scene
        /// </summary>
        [SerializeField] protected RootUICanvas rootUICanvas;
        
        [Inject] private IScreenManager screenManager;
        
        public override void InstallBindings()
        {
            //todo this should be setup automatically
            if(this.rootUICanvas == null) return;
            this.screenManager.CurrentRootScreen  = this.rootUICanvas.RootUIShowTransform;
            this.screenManager.CurrentHiddenRoot  = this.rootUICanvas.RootUIClosedTransform;
            this.screenManager.CurrentOverlayRoot = this.rootUICanvas.RootUIOverlayTransform;
        }
    }
}