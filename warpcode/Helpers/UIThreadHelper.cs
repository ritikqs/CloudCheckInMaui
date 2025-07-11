using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Threading.Tasks;

namespace CCIMIGRATION.Helpers
{
    public static class UIThreadHelper
    {
        /// <summary>
        /// Executes an action on the UI thread
        /// </summary>
        public static void InvokeOnMainThread(Action action)
        {
            if (Application.Current?.Dispatcher != null)
            {
                Application.Current.Dispatcher.Dispatch(action);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(action);
            }
        }

        /// <summary>
        /// Executes an async function on the UI thread
        /// </summary>
        public static async Task InvokeOnMainThreadAsync(Func<Task> action)
        {
            if (Application.Current?.Dispatcher != null)
            {
                await Application.Current.Dispatcher.DispatchAsync(action);
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(action);
            }
        }
    }
}
