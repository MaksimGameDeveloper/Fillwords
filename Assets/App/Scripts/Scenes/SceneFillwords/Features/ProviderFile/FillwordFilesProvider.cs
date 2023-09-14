using System;
using System.Collections.Generic;
using App.Scripts.Libs.Installer;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderFile
{
    public class FillwordFilesProvider : MonoBehaviour, IFillwordFilesProvider, IInitializable
    {
        [SerializeField] private List<SerializableFile> _files;
    
        private readonly Dictionary<FileType, string[]> _fileLines = new();

        public void Init()
        {
            LoadAllFiles();
        }

        public string LoadLine(FileType fileType, int line)
        {
            return _fileLines[fileType][line];
        }

        private void LoadAllFiles()
        {
            TextAsset textAsset;
            foreach (SerializableFile file in _files)
            {
                textAsset = file.File;
                _fileLines[file.FileType] = textAsset.text.Split("\r\n");
            }
        }
    }

    public enum FileType
    {   
        Pack,
        WordsList,
    }

    [Serializable]
    public class SerializableFile
    {
        public TextAsset File => _file;
        public FileType FileType => _fileType;
    
        [SerializeField] private TextAsset _file;
        [SerializeField] private FileType _fileType;
    }
}