using GameFoundation.Scripts.GameManager;
using GameFoundation.Scripts.Utilities.LogService;
using Zenject;

namespace GameFoundation.Scripts.BlueprintFlow.BlueprintControlFlow
{
    public class DebugBlueprintReaderManager : BlueprintReaderManager
    {
        public DebugBlueprintReaderManager(SignalBus signalBus, ILogService logService, DiContainer diContainer, GameFoundationLocalData localData, HandleLocalDataServices handleLocalDataServices,BlueprintDownloader blueprintDownloader, BlueprintConfig blueprintConfig) : base(signalBus, logService, diContainer, localData, handleLocalDataServices, blueprintDownloader, blueprintConfig)
        {
        }

        protected override bool IsLoadLocalBlueprint(string url, string hash) => true;
    }
}