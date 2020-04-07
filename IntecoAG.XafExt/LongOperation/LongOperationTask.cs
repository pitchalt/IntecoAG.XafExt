using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo.DB;

namespace IntecoAG.XafExt.LongOperation
{
    [DomainComponent]
    public class LongOperationManager
    {
        private ConcurrentQueue<LongOperationTask> _LongOperationQueue;

        private BindingList<LongOperationTask> _Operations;
        public BindingList<LongOperationTask> Operations
        {
            get
            {
                if (_Operations == null)
                {
                    _Operations = new BindingList<LongOperationTask>(_LongOperationQueue.ToList());
                }

                return _Operations;
            }
        }
        //private BackgroundWorker Worker;
        public Task Run(LongOperationTask longOperationTask)
        {
            var tokenSource = new CancellationTokenSource();
            _LongOperationQueue.Enqueue(longOperationTask);
            return Task.Run(() => longOperationTask.DoWorkCore(tokenSource), tokenSource.Token);
        }

        public async Task RunAsync(LongOperationTask longOperationTask)
        {
            var tokenSource = new CancellationTokenSource();
            _LongOperationQueue.Enqueue(longOperationTask);
            await Task.Run(() => longOperationTask.DoWorkCore(tokenSource), tokenSource.Token);
        }

        public LongOperationManager()
        {
            _LongOperationQueue = new ConcurrentQueue<LongOperationTask>();
        }
    }

    public enum LongOperationState
    {
        CREATED = 0,
        PREPARED = 1,
        RUNNING = 2,
        CANCELLING = 3,
        CANCELLED = 4,
        COMPLETED = 5,
    }

    [DomainComponent]
    public abstract class LongOperationTask: IDisposable
    {

        private Int32 _CurrentWorkItem;
        public Int32 CurrentWorkItem {
            get { return Thread.VolatileRead(ref _CurrentWorkItem); }
            protected set { Thread.VolatileWrite(ref _CurrentWorkItem, value); }
        }

        private Int32 _MaxWorkItem;
        public Int32 MaxWorkItem {
            get { return Thread.VolatileRead(ref _MaxWorkItem); }
            protected set { Thread.VolatileWrite(ref _MaxWorkItem, value); }
        }

        private Int32 _State;
        public LongOperationState State {
            get { return (LongOperationState) Thread.VolatileRead(ref _State); }
            protected set { Thread.VolatileWrite(ref _State, (Int32) value); }
        }

        //private readonly LongOperationManager _Manager;
        protected CancellationTokenSource TokenSource { get; private set; }
        protected CancellationToken Token { get; private set; }

        protected LongOperationTask()
        {
            State = LongOperationState.CREATED;
        }

        protected Boolean IsCancelRequested {
            get { return Token.IsCancellationRequested; }
        }

        protected void ThrowIfCancellationRequested()
        {
            if (Token.IsCancellationRequested)
                throw new OperationCanceledException(Token);
        }

        public void Cancel()
        {
            State = LongOperationState.CANCELLING;
            TokenSource.Cancel();
        }

        internal void DoWorkCore(CancellationTokenSource tokenSource)
        {
            TokenSource = tokenSource;
            Token = tokenSource.Token;
            State = LongOperationState.PREPARED;
            MaxWorkItem = Prepare();
            State = LongOperationState.RUNNING;
            try
            {
                DoWork();
            }
            catch (OperationCanceledException)
            {
                State = LongOperationState.CANCELLED;
                return;
            }
            State = LongOperationState.COMPLETED;
        }

        public abstract Int32 Prepare();

        public abstract void DoWork();

        public virtual void Dispose()
        {
            TokenSource?.Dispose();
        }
    }

}
