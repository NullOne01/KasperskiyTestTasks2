using System;
using System.IO;

namespace scan_service.Input
{
    public class ArgumentReader
    {
        public DirectoryInfo ReadDirectoryInfo(string[] args, int index)
        {
            if (args.Length <= index)
            {
                throw new ArgumentException("How to use: scan_util PATH_TO_DIRECTORY");
            }

            string path = args[index];
            // Load environment variables.
            path = Environment.ExpandEnvironmentVariables(path);
            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                throw new ArgumentException("Directory cannot be found");
            }

            return directory;
        }
    }
}