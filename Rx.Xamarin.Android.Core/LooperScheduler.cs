
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

using Android.OS;

namespace Rx.Xamarin.Android.Core
{
    public class LooperScheduler : IScheduler
    {
        private Handler handler;

        public LooperScheduler (Looper looper)
        {
            this.handler = new Handler (looper);
        }

        public LooperScheduler (Handler handler)
        {
            this.handler = handler;
        }

        public DateTimeOffset Now {
            get { return DateTimeOffset.Now; }
        }

        public IDisposable Schedule<TState> (TState state, Func<IScheduler, TState, IDisposable> action)
        {
            bool isCancelled = false;
            var innerDisp = new SerialDisposable () { Disposable = Disposable.Empty };

            //if (looperId > 0 && looperId == Java.Lang.Thread.CurrentThread ().Id) {
            //    return action (this, state);
            //}

            handler.Post (() => {
                if (isCancelled) return;
                innerDisp.Disposable = action (this, state);
            });

            return new CompositeDisposable (
                Disposable.Create (() => isCancelled = true),
                innerDisp);
        }

        public IDisposable Schedule<TState> (TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            bool isCancelled = false;
            var innerDisp = new SerialDisposable () { Disposable = Disposable.Empty };

            handler.PostDelayed (() => {
                if (isCancelled) return;
                innerDisp.Disposable = action (this, state);
            }, dueTime.Ticks / 10 / 1000);

            return new CompositeDisposable (
                Disposable.Create (() => isCancelled = true),
                innerDisp);    
        }

        public IDisposable Schedule<TState> (TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (dueTime <= Now) {
                return Schedule (state, action);
            }

            return Schedule (state, dueTime - Now, action);
        }
    }
}

