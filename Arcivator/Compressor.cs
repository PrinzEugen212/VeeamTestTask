using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using VeeamTestTask.Core.Interfaces;
using VeeamTestTask.Core.ReadWrite;

namespace VeeamTestTask.Core
{
    public class Compressor : IDisposable, IArchivator
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

        public int Start()
        {
            try
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
            catch (IOException e)
            {
                Console.WriteLine($"Error while working with files:\n{e.Message}");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:\n{e.Message}");
                return 1;
            }

            return 0;
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
