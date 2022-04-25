using System;
using System.IO;

namespace VeeamTestTask.Core.ReadWrite
{
    internal class Reader : IDisposable
    {
        private long readedBytes;
        private int orderNumber = 0;
        private FileStream fileStream;

        public Reader(string filePath)
        {
            fileStream = File.OpenRead(filePath);
            readedBytes = 1;
            Length = fileStream.Length;
        }

        public bool IsBytesReaded => readedBytes != 0;

        public long Position => fileStream.Position;

        public long Length;

        public byte[] ReadChunk(int chunkSize)
        {
            byte[] buffer;
            if (fileStream.Length - fileStream.Position < chunkSize)
            {
                buffer = new byte[fileStream.Length - fileStream.Position];
            }
            else
            {
                buffer = new byte[chunkSize];
            }

            readedBytes = fileStream.Read(buffer, 0, buffer.Length);
            orderNumber = this.orderNumber++;
            return buffer;
        }

        public byte[] Read(long position, int length)
        {
            byte[] buffer = new byte[length];
            fileStream.Seek(position, SeekOrigin.Begin);
            readedBytes = fileStream.Read(buffer, 0, length);
            return buffer;
        }

        public void Dispose()
        {
            fileStream.Dispose();
        }
    }
}
