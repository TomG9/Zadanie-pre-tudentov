using System.IO;

namespace FolderReader
{public class DirectoryReader
    {
 public DirectoryContent ReadDirectory(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"The directory {path} was not found.");
            }

            var content = new DirectoryContent
            {
                DirectoryPath = path
            };

            foreach (var file in Directory.GetFiles(path))
            {
                content.Files.Add(file);
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                content.SubDirectories.Add(ReadDirectory(dir));
            }
            
            return content;
        }
    
    }

}