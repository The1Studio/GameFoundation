namespace GameFoundation.Scripts.Utilities.ApplicationServices
{
    using System;
    using GameFoundation.Scripts.GameManager;
    using UnityEngine;
    using Zenject;

    /// <summary>Catch application event ex pause, focus and more.... </summary>
    public class MinimizeAppService : MonoBehaviour
    {
        [Inject] private SignalBus               signalBus;
        [Inject] private HandleLocalDataServices localDataServices;

        private readonly ApplicationPauseSignal     applicationPauseSignal     = new ApplicationPauseSignal(false);
        private readonly UpdateTimeAfterFocusSignal updateTimeAfterFocusSignal = new UpdateTimeAfterFocusSignal();

        private DateTime timeBeforeAppPause = DateTime.Now;

        //Todo need
        private const int MinimizeTimeToReload = 5;

        private void OnApplicationPause(bool pauseStatus)
        {
            this.applicationPauseSignal.PauseStatus = pauseStatus;
            this.signalBus.Fire(this.applicationPauseSignal);


            if (pauseStatus)
            {
                this.timeBeforeAppPause = DateTime.Now;

                // save local data to storage
                this.localDataServices.StoreAllToLocal();
            }
            else
            {
                var intervalTimeMinimize = DateTime.Now - this.timeBeforeAppPause;

                if (MinimizeTimeToReload > 0 && intervalTimeMinimize.TotalMinutes >= MinimizeTimeToReload)
                {
                    // Reload game
                }

                this.updateTimeAfterFocusSignal.MinimizeTime = intervalTimeMinimize.TotalSeconds;
                this.signalBus.Fire(this.updateTimeAfterFocusSignal);
            }
        }

        private void OnApplicationQuit() { this.localDataServices.StoreAllToLocal(); }
    }
}