using Newtonsoft.Json;

namespace App.Scripts.Scenes.SceneWordSearch.Features.JsonParser
{
    public class JsonParser : IJsonParser
    {
        public SerializableLevel.SerializableLevel ParseJsonString(string jsonString)
        {
            SerializableLevel.SerializableLevel list = JsonConvert.DeserializeObject<SerializableLevel.SerializableLevel>(jsonString);
            return list;
        }
    }
}