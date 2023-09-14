using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;

namespace App.Scripts.Scenes.SceneFillwords.Features.FileParser
{
    public interface IFillwordFileParser
    {
        GridFillWords ParsePack(int levelIndex);
    }
}