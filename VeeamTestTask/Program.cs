using System;
using VeeamTestTask.Core;
using VeeamTestTask.Core.Interfaces;
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
            try
            {
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
            }
            catch
            {
                Console.WriteLine("Invalid arguments");
                return 1;
            }

            IArchivator archivator;
            if (parameters.Operation == Operation.Compress)
            {
                archivator = new Compressor(parameters.InputFile, parameters.OutputFile, chunkSize, Environment.ProcessorCount);
            }
            else
            {
                archivator = new Decompressor(parameters.InputFile, parameters.OutputFile, chunkSize, Environment.ProcessorCount);
            }

            return new Archivator(archivator).Start();
        }
    }
}
