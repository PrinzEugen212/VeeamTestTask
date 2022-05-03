using System;
using System.IO;
using VeeamTestTask.Core;
using VeeamTestTask.Core.Utils;

namespace VeeamTestTask
{
    internal class Program
    {
        static int Main(string[] args)
        {
            int chunkSize = 1024 * 100;
            CommandParser commandParser = new CommandParser();
            Parameters parameters;
            if (args.Length > 0)
            {
                parameters = commandParser.ParseArgumentsArray(args);
            }
            else
            {
                if (commandParser.TryReadStartFromConsole(out parameters) == false)
                {
                    return 1;
                };
            }

            if (parameters.Operation == Operation.Compress)
            {
                Archivator archivator = new Archivator(parameters.InputFile, parameters.OutputFile, chunkSize, Environment.ProcessorCount);
                return archivator.StartCompressing();
            }
            else
            {
                Archivator archivator = new Archivator(parameters.InputFile, parameters.OutputFile, chunkSize, Environment.ProcessorCount);
                return archivator.StartDecompressing();
            }

        }
    }
}
