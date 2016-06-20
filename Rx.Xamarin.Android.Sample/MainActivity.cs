using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using System;
using System.Threading;

using System.Reactive;
using System.Reactive.Linq;

using Rx.Xamarin.Android.Core;

namespace Rx.Xamarin.Android.Sample
{
    [Activity (Label = "Rx.Xamarin.Android.Sample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private Looper backgroundLooper;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            BackgroundThread backgroundThread = new BackgroundThread ();
            backgroundThread.Start ();
            backgroundLooper = backgroundThread.Looper;

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button> (Resource.Id.myButton);

            button.Click += delegate {
                StartBackgroundWork ();
            };

            Button sendBroadcastButton = FindViewById<Button> (Resource.Id.buttonSendBroadcast);
            sendBroadcastButton.Click += delegate {
                SendBroadcast (new Intent ("com.ksachdeva.oss.TEST_RX_INTENT"));
            };

            RxBroadcastReceiver.Create (this, new IntentFilter ("com.ksachdeva.oss.TEST_RX_INTENT")).Subscribe ((Intent obj) => {
                Console.WriteLine (obj.Action);
            });
        }

        static IObservable<string> SampleObservable ()
        {
            return Observable.Defer<String> (() => {
                Thread.Sleep (1 * 1000);    // Sleep for one second
                return Observable.Return<string> ("one");
            });
        }

        public void StartBackgroundWork ()
        {
            SampleObservable ().SubscribeOn (AndroidSchedulers.From (this.backgroundLooper))
                              .ObserveOn (AndroidSchedulers.MainThread ()).Subscribe ((string obj) => Console.WriteLine ("OnNext:" + obj));
        }

        class BackgroundThread : HandlerThread
        {
            public BackgroundThread () : base ("background-thread")
            {
            }
        }
    }
}


