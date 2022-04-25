using System;
using System.Collections.Generic;
using VeeamTestTask.Core.Utils;

namespace VeeamTestTask.Core.ReadWrite
{
    internal class ReaderManager : IDisposable
    {
        private object locker = new object();
        private int chunkSize;
        private int orderNumber = 0;
        private Reader reader;

        public bool IsBytesReaded => reader.IsBytesReaded;

        public ReaderManager(string filePath, int chunkSize = 1024)
        {
            this.chunkSize = chunkSize;
            reader = new Reader(filePath);
        }

        public List<Header> ReadHeaders(string fileToDecompress)
        {
            List<Header> headers = new List<Header>();
            long position = 0;
            for (int i = 0; position < reader.Length; i++)
            {
                byte[] buffer = reader.Read(position, Header.HeaderLength);
                headers.Add(Header.Create(buffer));
                position = (int)reader.Position + headers[i].ChunkLength;
            }

            return headers;
        }

        public byte[] ReadChunk(out int orderNumber)
        {
            lock (locker)
            {
                byte[] buffer = reader.ReadChunk(chunkSize);
                orderNumber = this.orderNumber++;
                return buffer;
            }
        }

        public byte[] Read(long position, int length)
        {
            lock (locker)
            {
                return reader.Read(position, length);
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
