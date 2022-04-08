using System;
using System.Text;
using System.Threading;

namespace VeeamTestTask.Core
{
    public class Archivator
    {
        private int threadCount;
        private string fileToCompress;
        private string fileToSave;
        private int chunkSize;

        public Archivator(string fileToCompress, string fileToSave, int chunkSize, int threadCount)
        {
            this.threadCount = threadCount;
            this.fileToCompress = fileToCompress;
            this.fileToSave = fileToSave;
            this.chunkSize = chunkSize;
        }

        public void StartCompressing()
        {
            Compressor compressor = new Compressor(fileToCompress, fileToSave, chunkSize, threadCount);
            compressor.StartCompressing();
        }

        public void StartDecompressing()
        {
            Decompressor decompressor = new Decompressor(fileToCompress, fileToSave);
            decompressor.StartDecompressing();
        }
    }
}
