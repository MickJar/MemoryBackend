using System;
using System.Collections.Generic;
using System.Text;

namespace Memory.Core.Services
{
    public interface IDelayHelper
    {

        event Action flipEvent;
        void Sleep(int sleepDuration);

        bool isUnlocked();
    }
}
