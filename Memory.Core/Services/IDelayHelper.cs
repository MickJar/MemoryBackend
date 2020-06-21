using System;

namespace Memory.Core.Services
{
    public interface IDelayHelper
    {

        event Action flipEvent;
        void Sleep(int sleepDuration);

        bool isUnlocked();
    }
}
