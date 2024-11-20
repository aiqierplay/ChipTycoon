using System;

namespace Aya.Security
{
    // using var lock = new AutoCancelLock(value => IsBusy = value);
    public struct AutoCancelLock : IDisposable
    {
        [NonSerialized] public Action<bool> LockSetter;
        [NonSerialized] public Action OnDone;

        public AutoCancelLock(Action<bool> lockSetter, Action onDone = null)
        {
            LockSetter = lockSetter;
            OnDone = onDone;
            LockSetter?.Invoke(true);
        }

        public void Dispose()
        {
            LockSetter?.Invoke(false);
            OnDone?.Invoke();
        }
    }
}