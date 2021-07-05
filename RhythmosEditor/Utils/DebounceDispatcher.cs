using System;
using System.Threading;
using UnityEngine;

namespace RhythmosEditor.Utils
{
    internal class DebounceDispatcher
    {
        private bool running = false;
        private SynchronizationContext syncContext;
        private Timer timer;
        private Action action;
        private object state;

        public DebounceDispatcher()
        {
            syncContext = SynchronizationContext.Current;
        }

        public DebounceDispatcher Debounce(Action action, int interval)
        {
            timer?.Dispose();

            if (state == null)
            {
                state = new object();
            }

            running = true;
            timer = new Timer(OnTick, state, interval, 0);
            this.action = action;

            return this;
        }

        public void Stop(bool callRegisteredAction = false)
        {
            if (running == true)
            {
                timer?.Dispose();
                timer = null;
                running = false;
                if (callRegisteredAction)
                {
                    syncContext.Post(state => {
                        try
                        {
                            action?.Invoke();
                        }
                        catch (Exception exception)
                        {
                            Debug.LogError(exception);
                        }
                    }, null);
                }
            }
        }

        private void OnTick(object state)
        {
            running = false;
            timer?.Dispose();

            if (action != null && timer != null)
            {
                syncContext.Post(syncState => {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(exception);
                    }
                }, null);

            }

            timer = null;
        }


    }
}
