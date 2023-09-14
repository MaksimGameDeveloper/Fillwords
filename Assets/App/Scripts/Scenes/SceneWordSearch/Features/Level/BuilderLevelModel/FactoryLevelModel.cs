using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            Dictionary<char, int> resultedChars = new();
            Dictionary<char, int> charsInWord = new();

            for (int i = 0; i < words.Count; i++)
            {
                charsInWord.Clear();
                for (int j = 0; j < words[i].Length; j++)
                {
                    if (charsInWord.ContainsKey(words[i][j]))
                        charsInWord[words[i][j]]++;
                    else
                        charsInWord[words[i][j]] = 1;
                }

                foreach (char wordChar in charsInWord.Keys)
                {
                    if (!resultedChars.ContainsKey(wordChar))
                    {
                        resultedChars[wordChar] = charsInWord[wordChar];
                        continue;
                    }
                    
                    if (resultedChars[wordChar] < charsInWord[wordChar])
                        resultedChars[wordChar] = charsInWord[wordChar];
                }
                
            }

            List<char> chars = new();

            foreach (KeyValuePair<char,int> letterValuePair in resultedChars)
            {
                for(int i = 0; i < letterValuePair.Value; i++)
                    chars.Add(letterValuePair.Key);
            }

            return chars;
        }
    }
}