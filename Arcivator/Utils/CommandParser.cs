using System;
using System.Collections.Generic;

namespace VeeamTestTask.Core.Utils
{
    public class CommandParser
    {
        private readonly Dictionary<string, Operation> operationStringValue = new Dictionary<string, Operation>()
        {
            {"compress", Operation.Compress},
            {"decompress", Operation.Decompress}
        };

        public bool TryReadStartFromConsole(out Parameters parameters)
        {
            Console.WriteLine("Please, print \"compress/decompress [source file name] [result file name]\"");
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

        public Parameters ParseArgumentsArray(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Invalid arguments");
            }

            Parameters outParameters = new Parameters();
            outParameters.Operation = operationStringValue[args[0]];
            outParameters.InputFile = args[1];
            outParameters.OutputFile = args[2];
            return outParameters;
        }

        public Parameters ParseStart(string commandLine)
        {
            string[] parameters = commandLine.Split();
            if (!operationStringValue.ContainsKey(parameters[0]))
            {
                throw new ArgumentException("Invalid command");
            }
            return ParseArgumentsArray(parameters);
        }
    }
}
