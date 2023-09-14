namespace App.Scripts.Scenes.SceneWordSearch.Features.JsonParser
{
    public interface IJsonParser
    {
        SerializableLevel.SerializableLevel ParseJsonString(string jsonString);
    }
}