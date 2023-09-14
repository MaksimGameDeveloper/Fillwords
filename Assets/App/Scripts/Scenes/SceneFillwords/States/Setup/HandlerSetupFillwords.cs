using System.Threading.Tasks;
using App.Scripts.Infrastructure.GameCore.States.SetupState;
using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Libs.CustomExtensions;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels.View.ViewGridLetters;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel;

namespace App.Scripts.Scenes.SceneFillwords.States.Setup
{
    public class HandlerSetupFillwords : IHandlerSetupLevel
    {
        private readonly IProviderFillwordLevel _providerFillwordLevel;
        private readonly IServiceLevelSelection _serviceLevelSelection;
        private readonly ViewGridLetters _viewGridLetters;
        private readonly ContainerGrid _containerGrid;
        private int _lastLevel = 1;

        public HandlerSetupFillwords(IProviderFillwordLevel providerFillwordLevel,
            IServiceLevelSelection serviceLevelSelection,
            ViewGridLetters viewGridLetters, ContainerGrid containerGrid)
        {
            _providerFillwordLevel = providerFillwordLevel;
            _serviceLevelSelection = serviceLevelSelection;
            _viewGridLetters = viewGridLetters;
            _containerGrid = containerGrid;
        }

        public Task Process()
        {
            GridFillWords model;
            
            while (true)
            {
                try
                {
                    model = TryLoadLevelModel();
                    WriteLastSuccessfullyLoadedLevel();
                    break;
                }
                catch (IncorrectLevelException)
                {
                    SkipIncorrectLevel();
                }
            }
            
            _viewGridLetters.UpdateItems(model);
            _containerGrid.SetupGrid(model, _serviceLevelSelection.CurrentLevelIndex);
            return Task.CompletedTask;
        }

        private void WriteLastSuccessfullyLoadedLevel()
        {
            _lastLevel = _serviceLevelSelection.CurrentLevelIndex;
        }

        private GridFillWords TryLoadLevelModel()
        {
            GridFillWords model;
            model = _providerFillwordLevel.LoadModel(_serviceLevelSelection.CurrentLevelIndex);
            return model;
        }

        private void SkipIncorrectLevel()
        {
            int nextLvl = _serviceLevelSelection.CurrentLevelIndex;
            nextLvl += _lastLevel < _serviceLevelSelection.CurrentLevelIndex ? 1 : -1;
            _serviceLevelSelection.UpdateSelectedLevel(nextLvl);
        }
    }
}
            
