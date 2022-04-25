using System;

namespace VeeamTestTask.Core.Utils
{
    internal class Header
    {
        public Header(long startPosition, int chunkLength, int orderNumber)
        {
            StartPosition = startPosition;
            ChunkLength = chunkLength;
            OrderNumber = orderNumber;
        }

        public long StartPosition { get; set; }
        public int ChunkLength { get; set; }
        public int OrderNumber { get; set; }
        public static int HeaderLength => 16;

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
            long startPosition = BitConverter.ToInt32(bytes, 0);
            int length = BitConverter.ToInt32(bytes, 8);
            int order = BitConverter.ToInt32(bytes, 12);
            return new Header(startPosition, length, order);
        }

        public override string ToString()
        {
            return $"Start - {StartPosition}, Length - {ChunkLength}, Order - {OrderNumber}";
        }
    }
}
