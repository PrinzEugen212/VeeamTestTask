using System;
using System.Collections.Generic;
using System.Linq;

namespace VeeamTestTask.Core.Utils
{
    public class CommandParser
    {
        private readonly Dictionary<Operation, string[]> operationStringValue = new Dictionary<Operation, string[]>()
        {
            { Operation.Compress, new string[] {"compress" , "c" } },
            { Operation.Decompress, new string[] {"decompress" , "d" }}
        };

        public bool TryReadStartFromConsole(out Parameters parameters)
        {
            Console.WriteLine("Please, print \"compress/decompress/c/d [source file name] [result file name]\"");
            try
            {
                parameters = ParseStart(Console.ReadLine());
            }
            catch
            {
                parameters = null;
                return false;
            }

            return true;
        }

        public Parameters ParseStart(string commandLine)
        {
            string[] parameters = commandLine.Split();
            return ParseArgumentsArray(parameters);
        }

        public Parameters ParseArgumentsArray(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Invalid arguments length");
            }

            Parameters outParameters = new Parameters();
            outParameters.Operation = GetOperation(args[0]);
            outParameters.InputFile = args[1];
            outParameters.OutputFile = args[2];
            return outParameters;
        }

        private Operation GetOperation(string command)
        {
            foreach (var commandString in operationStringValue)
            {
                if (commandString.Value.Contains(command))
                {
                    return commandString.Key;
                }
            }

            throw new Exception("Unknown command");
        }
    }
}
