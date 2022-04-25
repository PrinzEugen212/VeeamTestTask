using System;
using System.IO;

namespace VeeamTestTask.Core.ReadWrite
{
    internal class Writer : IDisposable
    {
        private FileStream fileStream;

        public Writer(string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.Create);
        }

        public long Position => fileStream.Position;

        public void WriteBytes(byte[] bytes)
        {

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();

        }

        public void WriteBytes(byte[] bytes, int position)
        {

            fileStream.Seek(position, SeekOrigin.Begin);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();

        }

        public void Dispose()
        {
            fileStream.Dispose();
        }

    }
}
