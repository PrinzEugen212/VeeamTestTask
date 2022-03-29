using System;
using System.IO;

namespace VeeamTestTask.Core
{
    internal class Writer
    {
        private string filePath;
        private int chunkSize;
        private object locker = new object();
        private int writedChunks = 0;
        private FileStream fileStream;
        private int orderNumber = 0;

        public Writer(string filePath, int chunkSize)
        {
            this.filePath = filePath;
            this.chunkSize = chunkSize;
            fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        }

        public string FilePath => filePath;

        public bool WriteBytes(byte[] bytes, ref int orderNumber, int orderIncrease)
        {
            if (orderNumber != this.orderNumber)
            {
                return false;
            }

            if (bytes == null)
            {
                return false;
            }

            lock (locker)
            {
                fileStream.Seek(writedChunks++ * chunkSize, SeekOrigin.Begin);
                fileStream.Write(bytes, 0, bytes.Length);
                this.orderNumber++;
                orderNumber += orderIncrease;
                return true;
            }
        }
    }
}
