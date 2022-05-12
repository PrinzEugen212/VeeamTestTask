using System;

namespace VeeamTestTask.Core.Interfaces
{
    public interface IArchivator : IDisposable
    {
        public int Start();
    }
}
