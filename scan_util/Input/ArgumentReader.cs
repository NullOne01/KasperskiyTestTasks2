using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace scan_util.Input
{
    public class ArgumentReader
    {
        public DirectoryInfo ReadDirectoryInfo(string[] args, int index)
        {
            if (args.Length <= index)
            {
                throw new ArgumentException("Not enough args");
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
        
        public int ReadInt(string[] args, int index)
        {
            if (args.Length <= index)
            {
                throw new ArgumentException("Not enough args");
            }
            
            return int.Parse(args[index]);
        }

        public void CheckCommand(string[] args, int index, Dictionary<string, Action> commands)
        {
            if (args.Length <= index)
            {
                throw new ArgumentException("Not enough args");
            }

            string commandName = args[index];
            if (!commands.ContainsKey(commandName))
            {
                throw new ArgumentException("Unknown command");
            }
            
            commands[commandName].Invoke();
        }
    }
}