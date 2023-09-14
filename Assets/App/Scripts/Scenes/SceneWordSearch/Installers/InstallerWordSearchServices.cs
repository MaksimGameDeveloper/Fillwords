using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Libs.Factory;
using App.Scripts.Libs.Installer;
using App.Scripts.Libs.ServiceLocator;
using App.Scripts.Scenes.SceneWordSearch.Features.FileReader;
using App.Scripts.Scenes.SceneWordSearch.Features.JsonParser;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.LevelContainer;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Installers
{
    public class InstallerWordSearchServices : MonoInstaller
    {
        [SerializeField] private ConfigLevelSelection configLevelSelection;

        public override void InstallBindings(ServiceContainer serviceContainer)
        {
            InstallServices(serviceContainer);
        }

        private void InstallServices(ServiceContainer container)
        {
            container.SetService<IServiceLevelSelection, ServiceLevelSelection>(
                new ServiceLevelSelection(configLevelSelection));

            container.SetService<IJsonParser, JsonParser>(new JsonParser());
            container.SetService<IFileReader, FileReader>(new FileReader());
            
            container.SetService<IProviderWordLevel, ProviderWordLevel>(new ProviderWordLevel(
                                    container.Get<IJsonParser>(),
                                    container.Get<IFileReader>()));
            
            container.SetService<IFactory<LevelModel, LevelInfo, int>, FactoryLevelModel>(new FactoryLevelModel());
            container.SetServiceSelf(new LevelContainer());
        }
    }
}