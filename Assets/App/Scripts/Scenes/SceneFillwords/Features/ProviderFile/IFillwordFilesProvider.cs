namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderFile
{
    public interface IFillwordFilesProvider
    {
        string LoadLine(FileType fileType, int line);
    }
}