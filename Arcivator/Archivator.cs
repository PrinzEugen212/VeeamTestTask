using System;

namespace VeeamTestTask.Core
{
    public class Archivator : IDisposable
    {
        private int threadCount;
        private string fileToCompress;
        private string fileToSave;
        private int chunkSize;
        private Compressor compressor;
        private Decompressor decompressor;

        public Archivator(string fileToCompress, string fileToSave, int chunkSize, int threadCount)
        {
            this.threadCount = threadCount;
            this.fileToCompress = fileToCompress;
            this.fileToSave = fileToSave;
            this.chunkSize = chunkSize;
        }

        public int StartCompressing()
        {
            try
            {

                Compressor compressor = new Compressor(fileToCompress, fileToSave, chunkSize, threadCount);
                this.compressor = compressor;
                compressor.StartCompressing();
            }
            catch
            {
                return 1;
            }

            return 0;
        }

        public int StartDecompressing()
        {
            try
            {
                Decompressor decompressor = new Decompressor(fileToCompress, fileToSave, chunkSize, threadCount);
                this.decompressor = decompressor;
                decompressor.StartDecompressing();
            }
            catch
            {
                return 1;
            }

            return 0;
        }

        public void Dispose()
        {
            compressor?.Dispose();
            decompressor?.Dispose();
        }
    }
}
