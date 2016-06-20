using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Android.Content;

namespace Rx.Xamarin.Android.Core
{
    public class RxBroadcastReceiver
    {
        private RxBroadcastReceiver () {}

        class InternalBroadcastReceiver : BroadcastReceiver
        {
            private IObserver<Intent> observer;

            public InternalBroadcastReceiver (IObserver<Intent> observer)
            {
                this.observer = observer;
            }

            public override void OnReceive (Context context, Intent intent)
            {
                this.observer.OnNext (intent);
            }
        }


        public static IObservable<Intent> Create (Context context, IntentFilter intentFilter)
        {
            if (context == null) {
                throw new ArgumentNullException (nameof (context));
            }

            if (intentFilter == null) {
                throw new ArgumentNullException (nameof (intentFilter));
            }

            return Observable.Create<Intent> ((subscriber) => {
                BroadcastReceiver receiver = new InternalBroadcastReceiver (subscriber);
                context.RegisterReceiver (receiver, intentFilter);
                return Disposable.Create (() => {
                    context.UnregisterReceiver (receiver);
                });

            });
        }
    }
}

