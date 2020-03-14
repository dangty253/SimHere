using System;
namespace SimhereApp.Portable.Interfaces
{
    public interface IFileService
    {
        void SaveFile(string name, byte[] data, string location = "");
        void OpenFile(string fileName, string location = "");
    }
}
