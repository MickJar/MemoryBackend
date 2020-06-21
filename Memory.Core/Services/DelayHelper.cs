using System;
using System.Threading.Tasks;

namespace Memory.Core.Services
{
    public class DelayHelper : IDelayHelper
    {
        private int _sleepLock = 0;
        public event Action flipEvent;
        public void Sleep(int sleepDuration)
        {
            _sleepLock++;
            Task.Delay(TimeSpan.FromMilliseconds(sleepDuration*1000)).ContinueWith(task => waitAndUnlock());
        }

        public void waitAndUnlock()
        {
            _sleepLock--;
            flipEvent();
        }

        public bool isUnlocked()
        {
            return _sleepLock == 0;
        }
    }
}
