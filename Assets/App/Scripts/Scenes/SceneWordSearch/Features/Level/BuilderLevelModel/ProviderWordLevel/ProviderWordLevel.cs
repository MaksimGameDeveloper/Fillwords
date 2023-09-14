using App.Scripts.Scenes.SceneWordSearch.Features.FileReader;
using App.Scripts.Scenes.SceneWordSearch.Features.JsonParser;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private readonly IJsonParser _jsonParser;
        private readonly IFileReader _fileReader;

        public ProviderWordLevel(IJsonParser jsonParser, IFileReader fileReader)
        {
            _jsonParser = jsonParser;
            _fileReader = fileReader;
        }
        
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string loadLevel = _fileReader.LoadLevel(levelIndex);
            var jsonString = _jsonParser.ParseJsonString(loadLevel);
            LevelInfo levelInfo = new LevelInfo();
            levelInfo.words = jsonString.words;
            return levelInfo;
        }
    }
}