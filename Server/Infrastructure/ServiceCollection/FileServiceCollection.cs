//using Minio;
//using System.Reflection;
//using tusdotnet.Stores;
//using UFile.Server;
//using UFile.Shared;

//namespace Server.Infrastructure.ServiceCollection
//{

//    internal class TusUploadServer : ITusUpload
//    {
//        private readonly IOnCreateCompleteEvent _onCreateCompleteEvent;

//        public TusDiskStore Store { get; }

//        public event Action<string>? UploadProgress;

//        public event Action<string>? Completed;

//        public event Action<Exception>? HadError;

//        public TusUploadServer(TusDiskStore store, IOnCreateCompleteEvent onCreateCompleteEvent)
//        {
//            Store = store;
//            _onCreateCompleteEvent = onCreateCompleteEvent;
//        }

//        public async Task Upload(Stream file, Dictionary<string, string> metadata, CancellationToken cancellationToken)
//        {
//            bool complete = false;
//            string fileId = await Store.CreateFileAsync(file.Length, metadata.Serialize(), cancellationToken);
//            await _onCreateCompleteEvent.InvokeAsync(new UCreateCompleteContext
//            {
//                FileId = fileId,
//                FilePath = "/uploads/" + fileId,
//                FileSize = file.Length,
//                Metadata = metadata
//            });
//            Store.AppendDataAsync(fileId, file, cancellationToken).ContinueWith(delegate
//            {
//                complete = true;
//                this.Completed?.Invoke(fileId);
//            }, cancellationToken);
//            while (!complete)
//            {
//                await Task.Delay(10, cancellationToken);
//                long length = await Store.GetUploadOffsetAsync(fileId, cancellationToken);
//                this.UploadProgress?.Invoke(((double)length / (double)file.Length * 100.0).ToString() ?? "");
//            }
//        }
//    }
//    public static class FileServiceCollection

//    {


//        public static IServiceCollection AddFileServer(this IServiceCollection services, UploadType uploadType, IConfiguration cfg)
//        {
//            IConfiguration cfg2 = cfg;
//            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
           
//                services.AddSingleton(delegate
//                {

//                    string currentDirectory = Directory.GetCurrentDirectory();
//                    string path = "wwwroot\\files";
//                    string text = Path.Combine(currentDirectory, path);
//                    if (!Directory.Exists(text))
//                    {
//                        Directory.CreateDirectory(text);
//                    }

//                    return new TusDiskStore(text);
//                });
//                services.AddSingleton(TusConfiguration.CreateTusConfiguration);
//                services.AddScoped<ITusUpload, TusUploadServer>();
//                services.AddHostedService<ExpiredFilesCleanupService>();
//                services.AddScoped<IUFileService, TusUploadHelper>();

//            return services;
//        }
//    }
//}
