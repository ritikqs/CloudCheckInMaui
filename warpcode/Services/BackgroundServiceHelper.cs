using CCIMIGRATION.Services.ImageUploadService;
namespace CCIMIGRATION.Services
{
    public class BackgroundServiceHelper
    {
        private static BackgroundServiceHelper _instance;
        private List<Func<Task>> _tasks = new List<Func<Task>>();

        public static BackgroundServiceHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BackgroundServiceHelper();
                }
                return _instance;
            }
        }

        public void Clear()
        {
            _tasks.Clear();
        }

        public void Add(Func<Task> task)
        {
            _tasks.Add(task);
        }

        public async Task StartBackgroundService()
        {
            // In MAUI, background services are platform-specific
            // For now, we'll execute the tasks immediately
            // In a production app, you would implement platform-specific background services
            foreach (var task in _tasks)
            {
                await task();
            }
            _tasks.Clear();
        }
    }

    // Temporary compatibility class to minimize code changes
    public static class BackgroundAggregatorService
    {
        public static BackgroundServiceHelper Instance => BackgroundServiceHelper.Instance;

        public static void Add(Func<object> taskFactory)
        {
            // Convert the factory to an async task
            Instance.Add(async () =>
            {
                var service = taskFactory();
                if (service is ImagesSyncService imageSyncService)
                {
                    // Execute the service
                    await Task.Run(() => imageSyncService.StartJob());
                }
            });
        }

        public static void StartBackgroundService()
        {
            Task.Run(async () => await Instance.StartBackgroundService());
        }
    }
}
