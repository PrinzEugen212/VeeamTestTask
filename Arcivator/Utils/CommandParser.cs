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

        public bool TryReadStartCommandFromConsole(out Parameters starParameters)
        {
            string commandLine = Console.ReadLine();
            try
            {
                starParameters = ParseStart(commandLine);
            }
            catch
            {
                starParameters = null;
                return false;
            }
            return true;
        }

        public Parameters ParseStart(string commandLine)
        {
            Parameters outParameters = new Parameters();
            string[] parameters = commandLine.Split();
            if (!operationStringValue.ContainsKey(parameters[0]))
            {
                throw new ArgumentException("Invalid command");
            }

            outParameters.Operation = operationStringValue[parameters[0]];
            outParameters.InputFile = parameters[1];
            outParameters.OutputFile = parameters[2];

            return outParameters;
        }
    }
}
