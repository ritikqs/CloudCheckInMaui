using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCIMIGRATION.Models;
using SQLite;
namespace CCIMIGRATION.Data
{
    public class OptraxDataBase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection( ConstantHelper.Database.DatabasePath,ConstantHelper.Database.Flags);
        });

        public SQLiteAsyncConnection Database { get { return lazyInitializer.Value; } }

        static bool initialized = false;
        public OptraxDataBase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(LocationModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(LocationModel)).ConfigureAwait(false);
                }

                initialized = true;
            }
        }
    }
    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
