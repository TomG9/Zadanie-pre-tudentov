using System.Collections.Generic;

namespace FolderReader{

    public class DirectoryContent
    {
        public string DirectoryPath { get; set; }
        public List<string> Files { get; set; } = new List<string>();
        public List<DirectoryContent> SubDirectories { get; set; } = new List<DirectoryContent>();
    }

}
