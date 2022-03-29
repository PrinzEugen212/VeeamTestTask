using System.IO;
using System.Linq;

namespace VeeamTestTask.Core
{
    internal class Reader
    {
        private string filePath;
        private int chunkSize;
        private object locker = new object();
        private int readedChunks = 0;
        private FileStream fileStream;
        private long readedBytes;

        public Reader(string filePath, int chunkSize)
        {
            this.filePath = filePath;
            this.chunkSize = chunkSize;
            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            readedBytes = fileStream.Length;
        }

        public bool IsBytesLeft => readedBytes != 0;

        public string FilePath => filePath;

        public byte[] ReadChunk(out int orderNumber)
        {
            byte[] buffer = new byte[chunkSize];
            lock (locker)
            {
                orderNumber = readedChunks;
                fileStream.Seek(readedChunks++ * chunkSize, SeekOrigin.Begin);
                readedBytes = fileStream.Read(buffer, 0, chunkSize);
            }

            buffer = buffer.Select(b => b).Where(b => b != 0).ToArray();
            return buffer;
        }
    }
}
