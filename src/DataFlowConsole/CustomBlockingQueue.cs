using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DataFlowConsole
{
    /// <summary>
    /// Blocking Queue implemented with monitor.wait and monitor.pulseall
    /// </summary>
    public class CustomBlockingQueue<TIn>
    {
        private readonly object _objForLock = new Object();
        private Queue<TIn> _queue = new Queue<TIn>();
        private readonly int _size;
        private bool _shutdown = false;

        public CustomBlockingQueue(int size)
        {
            _size = size;
        }

        public void Stop()
        {
            lock (_objForLock)
            {
                _shutdown = true;
                Monitor.PulseAll(_objForLock);
            }
        }
        public bool Enqueue(TIn item)
        {
            lock(_objForLock)
            {
                while (_queue.Count > _size && !_shutdown)
                {
                    Monitor.Wait(_objForLock);
                }

                if (_shutdown)
                    return false;

                _queue.Enqueue(item);
                Monitor.PulseAll(_objForLock);
            }

            return true;
        }

        public bool Dequeue(out TIn item)
        {
            item = default(TIn);

            lock(_objForLock)
            {
                while(_queue.Count == 0 && !_shutdown)
                {
                    Monitor.Wait(_objForLock);
                }

                if (!_queue.TryDequeue(out item))
                    return false;

                Monitor.PulseAll(_objForLock);
            }

            return true;
        }
    }
}