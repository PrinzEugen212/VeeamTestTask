using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using VeeamTestTask.Core.ReadWrite;
using VeeamTestTask.Core.Utils;

namespace VeeamTestTask.Core
{
    internal class Decompressor : IDisposable
    {
        private string fileToDecompress;
        private int chunkSize;
        private int threadCount;
        private ReaderManager readerManager;
        private WriterManager writerManager;
        private List<Header> headers = new List<Header>();
        private Thread[] threads;

        public Decompressor(string fileToDecompress, string fileToSave, int chunkSize, int threadCount)
        {
            this.fileToDecompress = fileToDecompress;
            this.chunkSize = chunkSize;
            this.threadCount = threadCount;
            threads = new Thread[threadCount];
            readerManager = new ReaderManager(fileToDecompress);
            writerManager = new WriterManager(fileToSave);
        }

        public void StartDecompressing()
        {
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(DecompressSingleChunk));
            }

            headers = readerManager.ReadHeaders(fileToDecompress);

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Start(i);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            readerManager.Dispose();
            writerManager.Dispose();
        }

        private void DecompressSingleChunk(object index)
        {
            if (index is not int)
            {
                return;
            }

            int i = (int)index;
            while (i < headers.Count)
            {
                byte[] buffer = readerManager.Read(headers[i].StartPosition, headers[i].ChunkLength);
                buffer = Decompress(buffer);
                writerManager.WriteBytesAtPosition(buffer, chunkSize * headers[i].OrderNumber);
                i += threadCount;
            }
        }

        private byte[] Decompress(byte[] bytes)
        {
            using MemoryStream compressedStream = new MemoryStream(bytes);
            using GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using MemoryStream resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }

        public void Dispose()
        {
            readerManager.Dispose();
            writerManager.Dispose();
        }
    }
}
