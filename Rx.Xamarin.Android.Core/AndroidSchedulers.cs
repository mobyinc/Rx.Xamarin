using System;
using System.Reactive.Concurrency;

using Android.OS;

namespace Rx.Xamarin.Android.Core
{
    public static class AndroidSchedulers
    {
        private static IScheduler mainThreadScheduler;

        public static IScheduler MainThread ()
        {
            if (mainThreadScheduler == null)
                mainThreadScheduler = new LooperScheduler (new Handler (Looper.MainLooper));

            return mainThreadScheduler;
        }

        public static IScheduler From (Looper looper)
        {
            if (looper == null)
                throw new ArgumentNullException (nameof(looper));
            
            return new LooperScheduler (looper);
        }
    }
}

