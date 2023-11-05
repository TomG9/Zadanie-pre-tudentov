using System.Collections.Generic;
using System.IO;
using System;
using System.Text.Json;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace FolderReader
{
    class Program
    {

        static void Main(string[] args)
        {
            while(true){

                Console.WriteLine("Please provide a folder or a json with folder information:");
                string path = Console.ReadLine().ToLower();

                if (path.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Environment.Exit(0); // Exit the application
                }

                DirectoryReader reader = new DirectoryReader();

                if (Directory.Exists(path))
                {
                    try
                    {
                        var content = reader.ReadDirectory(path);

                        DisplayContent(content, path);
                        Console.WriteLine("Save to JSON ?");
                        string ans = Console.ReadLine().ToLower().Trim();
                        if (ans == "y" || ans == "yes")
                        {
                            Console.WriteLine("Please provide JSON file location"); 
                            string outputPath = Console.ReadLine();
                            var x = SaveToJSON(content, outputPath);   
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else if (File.Exists(path) && Path.GetExtension(path).Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    var jsonFile = File.ReadAllText(path);
                    DirectoryContent directoryContent = JsonSerializer.Deserialize<DirectoryContent>(jsonFile);
                    
                    DisplayContent(directoryContent);

                }
            }
        }
        static void DisplayContent(DirectoryContent content, string rootPath = null, HashSet<string> uniqueExtensions = null)
        {
            if (uniqueExtensions == null)
            {
                uniqueExtensions = new HashSet<string>();
            }

            if (rootPath == null)
            {
                rootPath = content.DirectoryPath;  
            }
            
            foreach (var file in content.Files)
            {
                string extension = Path.GetExtension(file);
                if (!string.IsNullOrEmpty(extension))
                {
                    uniqueExtensions.Add(extension);

                }
            }

            foreach (var subDir in content.SubDirectories)
            {
                DisplayContent(subDir, rootPath, uniqueExtensions);
            }
                // Check if we're at the root level
            if (content.DirectoryPath == rootPath)
            {
                Console.WriteLine("Unique file extensions found in folder:");
                
                foreach (var ext in uniqueExtensions)
                {
                    Console.WriteLine(ext);
                }
            }
        }

        static string SaveToJSON(DirectoryContent content, string pathToWrite)
        {
            var jsonString = JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(pathToWrite+"\\output.json", jsonString);
            return jsonString;
        } 

    }
}