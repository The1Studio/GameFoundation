namespace GameFoundation.Scripts.BlueprintFlow.BlueprintControlFlow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.BlueprintFlow.BlueprintReader;
    using GameFoundation.Scripts.BlueprintFlow.Signals;
    using GameFoundation.Scripts.GameManager;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.LogService;
    using UnityEngine;
    using Zenject;

    /// <summary>
    ///  The main manager for reading blueprints pipeline/>.
    /// </summary>
    public class BlueprintReaderManager
    {
        public BlueprintReaderManager(SignalBus signalBus, ILogService logService, DiContainer diContainer,
            GameFoundationLocalData localData, HandleLocalDataServices handleLocalDataServices, BlueprintDownloader blueprintDownloader, BlueprintConfig blueprintConfig)
        {
            this.signalBus               = signalBus;
            this.logService              = logService;
            this.diContainer             = diContainer;
            this.localData               = localData;
            this.handleLocalDataServices = handleLocalDataServices;
            this.blueprintDownloader     = blueprintDownloader;
            this.blueprintConfig         = blueprintConfig;
        }

        public virtual async void LoadBlueprint(string url, string hash = "test")
        {
            if (!this.IsLoadLocalBlueprint(url, hash))
            {
                //Download new blueprints version from remote
                await this.blueprintDownloader.DownloadBlueprintAsync(url);

                this.localData.BlueprintModel.BlueprintDownloadUrl = url;
                this.handleLocalDataServices.Save(this.localData, true);
            }

            // Unzip file to memory
            this.listRawBlueprints = await UniTask.RunOnThreadPool(this.UnzipBlueprint);

            //Load all blueprints to instances
            try
            {
                await this.ReadAllBlueprint();
            }
            catch (FieldDontExistInBlueprint e)
            {
                this.logService.Error(e.Message);
            }

            this.logService.Log("[BlueprintReader] All blueprint are loaded");

            this.signalBus.Fire<LoadBlueprintDataSuccessedSignal>();
        }

        protected virtual bool IsLoadLocalBlueprint(string url, string hash) =>
            this.localData.BlueprintModel.BlueprintDownloadUrl == url &&
            MD5Utils.GetMD5HashFromFile(this.blueprintConfig.BlueprintZipFilepath) == hash &&
            File.Exists(this.blueprintConfig.BlueprintZipFilepath);

        private async UniTask<Dictionary<string, string>> UnzipBlueprint()
        {
            var result = new Dictionary<string, string>();
            using (var archive = ZipFile.OpenRead(this.blueprintConfig.BlueprintZipFilepath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!entry.FullName.EndsWith(BlueprintConfig.BlueprintFileType, StringComparison.OrdinalIgnoreCase))
                        continue;
                    using var streamReader = new StreamReader(entry.Open());
                    result.Add(entry.Name, await streamReader.ReadToEndAsync());
                }
            }

            return result;
        }

        private UniTask ReadAllBlueprint()
        {
            if (!File.Exists(this.blueprintConfig.BlueprintZipFilepath))
            {
                this.logService.Error(
                    $"[BlueprintReader] {this.blueprintConfig.BlueprintZipFilepath} is not exists!!!");
                return UniTask.CompletedTask;
            }

            var listReadTask    = new List<UniTask>();
            var allDerivedTypes = ReflectionUtils.GetAllDerivedTypes<IGenericBlueprintReader>();
            foreach (var blueprintType in allDerivedTypes)
            {
                var blueprintInstance = (IGenericBlueprintReader)this.diContainer.Resolve(blueprintType);
                if (blueprintInstance != null)
                {
                    listReadTask.Add(UniTask.RunOnThreadPool(() => this.OpenReadBlueprint(blueprintInstance)));
                }
                else
                {
                    this.logService.Log($"Can not resolve blueprint {blueprintType.Name}");
                }
            }

            return UniTask.WhenAll(listReadTask);
        }

        private async UniTask OpenReadBlueprint(IGenericBlueprintReader blueprintReader)
        {
            var bpAttribute = blueprintReader.GetCustomAttribute<BlueprintReaderAttribute>();
            if (bpAttribute != null)
            {
                if (bpAttribute.BlueprintScope == BlueprintScope.Server) return;

                // Try to load a raw blueprint file from local or resource folder
                string rawCsv;
                if (!bpAttribute.IsLoadFromResource)
                {
                    if (!this.listRawBlueprints.TryGetValue(bpAttribute.DataPath + BlueprintConfig.BlueprintFileType,
                            out rawCsv))
                    {
                        this.logService.Warning(
                            $"[BlueprintReader] Blueprint {bpAttribute.DataPath} is not exists at the local folder, try to load from resource folder");
                        rawCsv = await LoadRawCsvFromResourceFolder();
                    }
                }
                else
                {
                    rawCsv = await LoadRawCsvFromResourceFolder();
                }

                async UniTask<string> LoadRawCsvFromResourceFolder()
                {
                    await UniTask.SwitchToMainThread();
                    try
                    {
                        return ((TextAsset)await Resources.LoadAsync<TextAsset>(BlueprintConfig.ResourceBlueprintPath +
                            bpAttribute.DataPath)).text;
                    }
                    catch (Exception e)
                    {
                        this.logService.Error($"Load {bpAttribute.DataPath} blueprint error!!!");
                        this.logService.Exception(e);
                        return null;
                    }
                }

                // Deserialize the raw blueprint to the blueprint reader instance
                if (!string.IsNullOrEmpty(rawCsv))
                    await blueprintReader.DeserializeFromCsv(rawCsv);
                else
                    this.logService.Warning(
                        $"[BlueprintReader] Unable to load {bpAttribute.DataPath} from {(bpAttribute.IsLoadFromResource ? "resource folder" : "local folder")}!!!");
            }
            else
            {
                this.logService.Warning(
                    $"[BlueprintReader] Class {blueprintReader} does not have BlueprintReaderAttribute yet");
            }
        }

        #region zeject

        private readonly SignalBus                  signalBus;
        private readonly ILogService                logService;
        private readonly DiContainer                diContainer;
        private readonly GameFoundationLocalData    localData;
        private readonly HandleLocalDataServices    handleLocalDataServices;
        private readonly BlueprintDownloader        blueprintDownloader;
        private          Dictionary<string, string> listRawBlueprints;
        private readonly BlueprintConfig            blueprintConfig;

        #endregion
    }
}