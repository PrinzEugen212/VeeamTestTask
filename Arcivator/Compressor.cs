using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace VeeamTestTask.Core
{
    internal class Compressor
    {
        private int threadCount;
        private Reader reader;
        private Writer writer;
        private Thread[] threads;
        private int startPosition = 0;

        public Compressor(string fileToCompress, string fileToSave, int chunkSize, int threadCount)
        {
            this.threadCount = threadCount;
            reader = new Reader(fileToCompress, chunkSize);
            writer = new Writer(fileToSave);
            threads = new Thread[threadCount];
        }

        public void StartCompressing()
        {
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(Compress);
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            reader.Dispose();
            writer.Dispose();
        }

        private byte[] CompressBytes(byte[] bytes)
        {
            using MemoryStream compressedStream = new MemoryStream();
            using GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }

        private void Compress()
        {
            while (reader.IsBytesLeft)
            {
                byte[] bytes = reader.ReadChunk(out int orderNumber);
                bytes = CompressBytes(bytes);
                if (bytes.Length == 0)
                {
                    return;
                }

                writer.WriteBytes(bytes, new Header(startPosition, bytes.Length, orderNumber));
            }
        }
    }
}
