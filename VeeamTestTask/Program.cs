using System;
using System.IO;
using VeeamTestTask.Core;
using VeeamTestTask.Core.Utils;

namespace VeeamTestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int chunkSize =  1024 * 100;
            CommandParser commandParser = new CommandParser();
            Parameters parameters = commandParser.ParseStart(Console.ReadLine());
            if (parameters.Operation == Operation.Compress)
            {
                Archivator archivator = new Archivator(parameters.InputFile, parameters.OutputFile, chunkSize, Environment.ProcessorCount);
                archivator.StartCompressing();
                archivator.Dispose();
            }
            else
            {
                Archivator archivator = new Archivator(parameters.InputFile, parameters.OutputFile, chunkSize, Environment.ProcessorCount);
                archivator.StartDecompressing();
                archivator.Dispose();
            }
            
        }
    }
}
