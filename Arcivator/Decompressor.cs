using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Linq;

namespace VeeamTestTask.Core
{
    internal class Decompressor
    {
        private Reader reader;
        private Writer writer;
        List<Header> table = new List<Header>();
        string fileToCompress;

        public Decompressor(string fileToDecompress, string fileToSave)
        {
            this.fileToCompress = fileToDecompress;
            CreateOrderTable();
            reader = new Reader(fileToDecompress, 0);
            writer = new Writer(fileToSave);
        }

        public void StartDecompressing()
        {
            //CreateOrderTable();
            for (int i = 0; i < table.Count; i++)
            {
                byte[] buffer = reader.Read(table[i].StartPosition, table[i].ChunkLength);
                buffer = Decompress(buffer);
                writer.WriteBytes(buffer);
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

        private void CreateOrderTable()
        {
            using (FileStream fileStream = new FileStream(fileToCompress, FileMode.Open))
            {
                for (int i = 0, c = 0; fileStream.Position < fileStream.Length; i++)
                {
                    byte[] buffer = new byte[12];
                    fileStream.Read(buffer, 0, buffer.Length);
                    table.Add(Header.Create(buffer));
                    c = (int)fileStream.Position;
                    fileStream.Seek(c + table[i].ChunkLength, SeekOrigin.Begin);
                }
            }
            table = table.OrderBy(h => h.OrderNumber).ToList();
        }
    }
}
