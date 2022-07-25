using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrilogyAvivaTest.Services.Misc
{
    //https://stackoverflow.com/questions/32654509/awaitable-autoresetevent

    public class AsyncAutoResetEvent
    {
        private static readonly Task _Completed = Task.FromResult(true);

        private readonly Queue<TaskCompletionSource<bool>> _waits = new Queue<TaskCompletionSource<bool>>();
        private bool _signalled;

        public AsyncAutoResetEvent(bool initialState = false)
        {
            _signalled = initialState;
        }

        public Task WaitAsync()
        {
            lock (_waits)
            {
                if (_signalled)
                {
                    _signalled = false;
                    return _Completed;
                }
                else
                {
                    var tcs = new TaskCompletionSource<bool>();
                    _waits.Enqueue(tcs);
                    return tcs.Task;
                }
            }
        }

        public void Set()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (_waits)
            {
                if (_waits.Count > 0)
                    toRelease = _waits.Dequeue();
                else if (!_signalled)
                    _signalled = true;
            }

            toRelease?.SetResult(true);
        }
    }
}