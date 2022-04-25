using System;
using VeeamTestTask.Core.Utils;

namespace VeeamTestTask.Core.ReadWrite
{
    internal class WriterManager : IDisposable
    {
        private object locker = new object();
        private Writer writer;

        public WriterManager(string filePath)
        {
            writer = new Writer(filePath);
        }

        public void WriteBytes(byte[] bytes, int orderNumber)
        {
            lock (locker)
            {
                Header header = new Header(writer.Position + Header.HeaderLength, bytes.Length, orderNumber);
                writer.WriteBytes(header.GetByteArray());
                writer.WriteBytes(bytes);
            }
        }

        public void WriteBytes(byte[] bytes)
        {
            lock (locker)
            {
                writer.WriteBytes(bytes);
            }
        }

        public void WriteBytesAtPosition(byte[] bytes, int position)
        {
            lock (locker)
            {
                writer.WriteBytes(bytes, position);
            }
        }

        public void SetHeaderStartPosition(Header header)
        {
            header.StartPosition = (int)writer.Position + Header.HeaderLength;
        }

        public void Dispose()
        {
            writer.Dispose();
        }
    }
}
