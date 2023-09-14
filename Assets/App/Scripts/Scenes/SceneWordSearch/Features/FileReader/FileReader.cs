using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.FileReader
{
    public class FileReader : IFileReader
    {
        private const string _levelsPath = "WordSearch/Levels";
    
        public string LoadLevel(int levelIndex)
        {
            string load = Resources.Load(_levelsPath + '/' + levelIndex).ToString();
            load = load.Replace("\r", "");
            load = load.Replace("\n", "");
            return load;
        }
    }
}
