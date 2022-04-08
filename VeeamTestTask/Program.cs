using System;
using System.IO;
using VeeamTestTask.Core;

namespace VeeamTestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int chunkSize = 16;
            string inputFile = @"F:\test.txt";
            string inputFileRes = @"F:\test_copy.txt";
            string outputFile = @"F:\OS_copy.txt";
            //WriteBytes();
            Archivator archivator = new Archivator(inputFile, outputFile, chunkSize, Environment.ProcessorCount);
            archivator.StartCompressing();
            archivator = new Archivator(outputFile, inputFileRes, chunkSize, Environment.ProcessorCount);
            archivator.StartDecompressing();
        }

        static void WriteBytes()
        {
            string inputFileRes = @"F:\OS_copy.txt";
            string file = @"F:\bytes.txt";
            byte[] bytes = File.ReadAllBytes(inputFileRes);
            string[] bytesAsString = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytesAsString[i] = $"{i}. {bytes[i]}";
            }

            File.WriteAllLines(file, bytesAsString);
        }
    }
}
