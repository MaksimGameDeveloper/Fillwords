using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using App.Scripts.Libs.CustomExtensions;
using App.Scripts.Libs.CustomLogger;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderFile;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.FileParser
{
    public class FillwordFileParser : IFillwordFileParser
    {
        private readonly Dictionary<int, char> _lettersByIndex = new();
        private readonly Regex _packRegex = new (@"(\d+)\s(\d+)(;\d+)+");
    
        private readonly IFillwordFilesProvider _fillwordFilesProvider;

        public FillwordFileParser(IFillwordFilesProvider fillwordFilesProvider)
        {
            _fillwordFilesProvider = fillwordFilesProvider;
        }
    
        public GridFillWords ParsePack(int levelIndex)
        {
            ClearArray();
            string packLine = ReadLineFromFile(levelIndex);
            MatchCollection matches = GetRegexMatches(packLine);
            SpliteElementsWithRegexAndWriteToArray(matches);
            ValidateLettersForGame();
            GridFillWords gridFillWords = InitializeGridFillWordsAndFillInLetters();
            return gridFillWords;
        }

        private void ClearArray()
        {
            _lettersByIndex.Clear();
        }

        private MatchCollection GetRegexMatches(string packLine)
        {
            MatchCollection matches = _packRegex.Matches(packLine);
            return matches;
        }

        private string ReadLineFromFile(int levelIndex)
        {
            string packLine = _fillwordFilesProvider.LoadLine(FileType.Pack, levelIndex);
            return packLine;
        }

        private void SpliteElementsWithRegexAndWriteToArray(MatchCollection matches)
        {
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string matchValue = match.Value;
                    string[] wordAndPositions = matchValue.Split(' ');
                    string[] positions = wordAndPositions[1].Split(';');
                    string word = _fillwordFilesProvider.LoadLine(FileType.WordsList, Int32.Parse(wordAndPositions[0]));

                    if (word.Length != positions.Length)
                    {
                        CustomLogger.LogError<IncorrectLevelException>("letters indexes in file don't match to word");
                    }

                    for (int i = 0; i < positions.Length; i++)
                    {
                        TryWriteElementToArray(Int32.Parse(positions[i]), word[i]);
                    }
                }
            }
        }

        private GridFillWords InitializeGridFillWordsAndFillInLetters()
        {
            int size = (int)Mathf.Sqrt(_lettersByIndex.Count);
            GridFillWords gridFillWords = new GridFillWords(new Vector2Int(size, size));

            foreach (int index in _lettersByIndex.Keys)
            {
                int i, j;
                (i, j) = GetSquareDimensionsByIndex(index, size);
                gridFillWords.Set(i, j, new CharGridModel(_lettersByIndex[index]));
            }

            return gridFillWords;
        }

        private (int, int) GetSquareDimensionsByIndex(int index, int squareSize)
        {
            int i = index / squareSize;
            int j = index - (i * squareSize);
            return (i, j);
        }

        private void TryWriteElementToArray(int position, char value)
        {
            if (_lettersByIndex.ContainsKey(position))
            {
                CustomLogger.LogError<IncorrectLevelException>("Letter seat already setuped!");
            }

            _lettersByIndex.Add(position,value);
        }

        private void ValidateLettersForGame()
        {
            int wordsByIndexCount = _lettersByIndex.Count;
            int sqrt = Convert.ToInt32(Math.Sqrt(wordsByIndexCount));
            if (sqrt * sqrt != wordsByIndexCount)
            {
                CustomLogger.LogError<IncorrectLevelException>("Indexes doesn't form a square!");
            }

            for (int i = 0; i < wordsByIndexCount; i++)
            {
                if (!_lettersByIndex.ContainsKey(i))
                {
                    CustomLogger.LogError<IncorrectLevelException>("Has letter outside square!");
                }
            }
        }
    }
}
