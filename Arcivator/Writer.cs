using System;
using System.IO;

namespace VeeamTestTask.Core
{
    internal class Writer : IDisposable
    {
        private object locker = new object();
        private FileStream fileStream;

        public Writer(string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
        }

        public long Position => fileStream.Position;

        public void Dispose()
        {
            fileStream.Dispose();
        }

        public void WriteBytes(byte[] bytes, Header header = null)
        {
            lock (locker)
            {
                if (header is not null)
                {
                    header.StartPosition = (int)fileStream.Position + header.HeaderLength;
                    Console.WriteLine(header);
                    fileStream.Write(header.GetByteArray(), 0, header.HeaderLength);
                }

                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
            }
        }

        public void WriteBytes(byte[] bytes, int position)
        {
            lock (locker)
            {
                fileStream.Seek(position, SeekOrigin.Begin);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
            }
        }
    }
}
