using System;
using System.IO;
using VeeamTestTask.Core;

namespace VeeamTestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int chunkSize =  1024 * 100;
            string inputFile = @"G:\BeautifulLoR.jpg";
            string compressedFile = @"G:\BeautifulLoR_compressed.comp";
            string outputFile = @"G:\BeautifulLoR_copy.jpg";
            Archivator archivator = new Archivator(inputFile, compressedFile, chunkSize, Environment.ProcessorCount);
            archivator.StartCompressing();
            archivator.Dispose();
            archivator = new Archivator(compressedFile, outputFile, chunkSize, Environment.ProcessorCount);
            archivator.StartDecompressing();
            archivator.Dispose();
        }
    }
}
