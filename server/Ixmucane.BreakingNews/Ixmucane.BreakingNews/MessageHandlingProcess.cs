using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DemoSignalR.MessageHandling;
using DemoSignalR.Support;

namespace DemoSignalR
{
    class MessageHandlingProcess
    {
        public static MessageHandlingProcess Instance;

        readonly ILog _log = Logs.As<MessageHandlingProcess>();
        readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        readonly ConcurrentQueue<Envelope<Message>> _queue = new ConcurrentQueue<Envelope<Message>>();
        readonly Action<Envelope<Message>> _handle;

        public MessageHandlingProcess(Action<Envelope<Message>> handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            _handle = handle;
        }

        public void Start()
        {
            Task.Factory.StartNew(ProcessMessages);
        }

        public void Stop()
        {
            _cancel.Cancel();
        }

        public void Handle(Envelope<Message> envelope)
        {
            _queue.Enqueue(envelope);
        }

        void ProcessMessages()
        {
            _log.Info("Started");
            while (!_cancel.IsCancellationRequested)
            {
                Envelope<Message> envelope;

                while (_queue.TryDequeue(out envelope))
                {
                    try
                    {
                        _handle(envelope);
                    }
                    catch (Exception ex)
                    {
                        _log.Info("Failed to handle [{0}] because of [{1}]", envelope, ex);
                    }
                }

                Thread.Sleep(25);
            }
            _log.Info("Stopped");
        }
    }
}