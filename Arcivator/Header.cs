using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeeamTestTask.Core
{
    internal class Header
    {
        public Header(int startPosition, int chunkLength, int orderNumber)
        {
            StartPosition = startPosition;
            ChunkLength = chunkLength;
            OrderNumber = orderNumber;
        }

        public int StartPosition { get; set; }
        public int ChunkLength { get; set; }
        public int OrderNumber { get; set; }
        public int HeaderLength => 12;

        public byte[] GetByteArray()
        {
            byte[] startBytes = BitConverter.GetBytes(StartPosition);
            byte[] lengthBytes = BitConverter.GetBytes(ChunkLength);
            byte[] orderBytes = BitConverter.GetBytes(OrderNumber);

            byte[] header = new byte[HeaderLength];

            startBytes.CopyTo(header, 0);
            lengthBytes.CopyTo(header, startBytes.Length);
            orderBytes.CopyTo(header, startBytes.Length + lengthBytes.Length);

            return header;
        }

        public static Header Create(byte[] bytes)
        {
            int startPosition = BitConverter.ToInt32(bytes, 0);
            int length = BitConverter.ToInt32(bytes, 4);
            int order = BitConverter.ToInt32(bytes, 8);
            return new Header(startPosition, length, order);
        }

        public override string ToString()
        {
            return $"Start - {StartPosition}, Length - {ChunkLength}, Order - {OrderNumber}";
        }
    }
}
