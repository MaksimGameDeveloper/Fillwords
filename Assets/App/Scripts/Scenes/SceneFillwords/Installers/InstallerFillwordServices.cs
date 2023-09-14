using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Libs.Installer;
using App.Scripts.Libs.ServiceLocator;
using App.Scripts.Scenes.SceneFillwords.Features.FileParser;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderFile;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Installers
{
    public class InstallerFillwordServices : MonoInstaller
    {
        [SerializeField] private ConfigLevelSelection configLevelSelection;
        
        [Header("MonoBehaviour Services")]
        [SerializeField] private FillwordFilesProvider _fillwordFilesProvider;

        public override void InstallBindings(ServiceContainer container)
        {
            container.SetService<IServiceLevelSelection, ServiceLevelSelection>(new ServiceLevelSelection(configLevelSelection));
            
            container.SetService<IInitializable, FillwordFilesProvider>(_fillwordFilesProvider);
            container.SetService<IFillwordFilesProvider, FillwordFilesProvider>(_fillwordFilesProvider);
                
            container.SetService<IFillwordFileParser, FillwordFileParser>(new FillwordFileParser(container.Get<IFillwordFilesProvider>()));
            container.SetService<IProviderFillwordLevel, ProviderFillwordLevel>(new ProviderFillwordLevel(container.Get<IFillwordFileParser>()));

            container.SetServiceSelf(new ContainerGrid());
        }
    }
}