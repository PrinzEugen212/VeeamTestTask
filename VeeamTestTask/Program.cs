using VeeamTestTask.Core;

namespace VeeamTestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int chunkSize = 16;
            string inputFile = @"F:\test.txt";
            //string inputFile = @"F:\OS.pdf";
            string outputFile = @"F:\OS_copy.txt";
            Archivator archivator = new Archivator(inputFile, outputFile, chunkSize);
            archivator.Compress();
        }
    }
}
