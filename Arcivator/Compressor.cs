using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using VeeamTestTask.Core.ReadWrite;
using VeeamTestTask.Core.Utils;

namespace VeeamTestTask.Core
{
    internal class Compressor : IDisposable
    {
        private int threadCount;
        private ReaderManager readerManager;
        private WriterManager writerManager;
        private Thread[] threads;

        public Compressor(string fileToCompress, string fileToSave, int chunkSize, int threadCount)
        {
            this.threadCount = threadCount;
            readerManager = new ReaderManager(fileToCompress, chunkSize);
            writerManager = new WriterManager(fileToSave);
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

            readerManager.Dispose();
            writerManager.Dispose();
        }

        private void Compress()
        {
            try
            {
                while (readerManager.IsBytesReaded)
                {
                    byte[] bytes = readerManager.ReadChunk(out int orderNumber);
                    bytes = CompressBytes(bytes);
                    if (bytes.Length == 0)
                    {
                        return;
                    }
                    throw new Exception();
                    writerManager.WriteBytes(bytes, orderNumber);
                }
            }
            catch (Exception ex)
            {
                HandleThreadException(ex);
            }
        }

        private byte[] CompressBytes(byte[] bytes)
        {
            using MemoryStream compressedStream = new MemoryStream();
            using GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }

        private void HandleThreadException(Exception exception)
        {
            throw exception;
        }

        public void Dispose()
        {
            readerManager.Dispose();
            writerManager.Dispose();
        }
    }
}
