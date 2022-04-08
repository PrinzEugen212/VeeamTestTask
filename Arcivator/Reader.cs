using System;
using System.IO;
using System.Linq;

namespace VeeamTestTask.Core
{
    internal class Reader : IDisposable
    {
        private int chunkSize;
        private object locker = new object();
        private FileStream fileStream;
        private long readedBytes;
        private int orderNumber = 0;

        public Reader(string filePath, int chunkSize = 1024)
        {
            this.chunkSize = chunkSize;
            fileStream = File.OpenRead(filePath);
            readedBytes = fileStream.Length;
        }

        public bool IsBytesLeft => readedBytes != 0;

        public void Dispose()
        {
            fileStream.Dispose();
        }

        public byte[] ReadChunk(out int orderNumber)
        {
            byte[] buffer = new byte[chunkSize];
            lock (locker)
            {
                readedBytes = fileStream.Read(buffer, 0, chunkSize);
            }

            orderNumber = this.orderNumber++;
            return buffer;
        }

        public byte[] Read(int position, int length)
        {
            byte[] buffer = new byte[length];
            lock (locker)
            {
                fileStream.Seek(position, SeekOrigin.Begin);
                readedBytes = fileStream.Read(buffer, 0, length);
            }

            return buffer;
        }
    }
}
