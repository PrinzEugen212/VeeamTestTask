using System;
using System.Collections.Generic;
using System.Threading;

namespace VeeamTestTask.Core
{
    public class Archivator
    {

        private Reader reader;
        private Writer writer;
        private List<Thread> threads;

        public Archivator(string fileToCompress, string fileToSave, int chunkSize)
        {
            reader = new Reader(fileToCompress, chunkSize);
            writer = new Writer(fileToSave, chunkSize);
            threads = new List<Thread>();
        }

        public int ThreadCount = Environment.ProcessorCount;

        public void Compress()
        {
            for (int i = 0; i < ThreadCount; i++)
            {
                //threads.Add(new Thread(CompressSingleChunk));
                threads.Add(new Thread(CompressSingleChunk));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            threads.Clear();
        }

        private void CompressSingleChunk()
        {
            while (reader.IsBytesLeft)
            {
                byte[] bytes = reader.ReadChunk(out int number);
                while (writer.WriteBytes(bytes, ref number, ThreadCount) == false)
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
