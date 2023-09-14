using App.Scripts.Scenes.SceneFillwords.Features.FileParser;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private readonly IFillwordFileParser _fillwordFileParser;

        public ProviderFillwordLevel(IFillwordFileParser fillwordFileParser)
        {
            _fillwordFileParser = fillwordFileParser;
        }
        
        public GridFillWords LoadModel(int index)
        {
            GridFillWords grid = ParsePackLineToGridFillWord(index);
            return grid;
        }

        private GridFillWords ParsePackLineToGridFillWord(int index)
        {
            GridFillWords grid = _fillwordFileParser.ParsePack(ConvertIndexToFileLine(index));
            return grid;
        }

        private int ConvertIndexToFileLine(int index)
        {
            return index-1;
        }
    }
}