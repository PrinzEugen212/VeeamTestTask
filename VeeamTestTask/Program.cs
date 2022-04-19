using System;
using System.IO;
using VeeamTestTask.Core;

namespace VeeamTestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int chunkSize =  1024;
            string inputFile = @"D:\OS.txt";
            string compressedFile = @"D:\OS_compressed.zip";
            string outputFile = @"D:\OS_copy.txt";
            //WriteBytes();
            Archivator archivator = new Archivator(inputFile, compressedFile, chunkSize, Environment.ProcessorCount);
            archivator.StartCompressing();
            archivator = new Archivator(compressedFile, outputFile, chunkSize, Environment.ProcessorCount);
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
