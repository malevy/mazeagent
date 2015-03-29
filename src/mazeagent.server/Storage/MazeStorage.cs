using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using mazeagent.core.Creation;
using mazeagent.core.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace mazeagent.server.Storage
{
    public class MazeRepository
    {

#if DEBUG
        private readonly IMazeStorage _storage = new NullMemoryStorage();
#else
        private readonly IMazeStorage _storage = new AzureBlobStorage();
#endif

        private Maze _maze;
        private readonly ReaderWriterLockSlim _lockObject = new ReaderWriterLockSlim();

        public static readonly MazeRepository Instance = new MazeRepository();

        public Maze CurrentMaze
        {
            get
            {
                try
                {
                    _lockObject.EnterReadLock();
                    return _maze;
                }
                finally
                {
                    _lockObject.ExitReadLock();
                }
            }
        }

        public async Task InitAsync()
        {
            var maze = await _storage.ReadAsync();
            if (null == maze)
            {
                maze = await this.BuildMaze();
            }

            try
            {
                _lockObject.EnterWriteLock();
                _maze = maze;
            }
            finally
            {
                _lockObject.ExitWriteLock();
            }
        }

        public async Task<Maze> BuildMaze()
        {
            var height = int.Parse(ConfigurationManager.AppSettings["maze.height"]);
            var width = int.Parse(ConfigurationManager.AppSettings["maze.width"]);
            var maze = MazeBuilder.Build(new Size(height, width));
            await _storage.WriteAsync(maze);
            return maze;
        }

        ~MazeRepository()
        {
            if (null != _lockObject) _lockObject.Dispose();
        }
    }

    public interface IMazeStorage
    {
        Task< Maze> ReadAsync();
        Task WriteAsync(Maze mazeDocument);
    }

    public class NullMemoryStorage : IMazeStorage
    {

        public Task<Maze> ReadAsync()
        {
            return Task.FromResult((Maze)null);
        }

        public Task WriteAsync(Maze mazeDocument)
        {
            return Task.FromResult(1);
        }
    }

    public class AzureBlobStorage : IMazeStorage
    {
        public async Task<Maze> ReadAsync()
        {
            var mazeBlob = await GetReferenceToBlob();
            if (! (await mazeBlob.ExistsAsync())) return null;
 
            var serializedContent = await mazeBlob.DownloadTextAsync();

            if (string.IsNullOrWhiteSpace(serializedContent)) return null;

            var restoredMemento = JsonConvert.DeserializeObject<Maze.Memento>(serializedContent);
            var restoredMaze = Maze.FromMemento(restoredMemento);
            return restoredMaze;

        }

        public async Task WriteAsync(Maze mazeDocument)
        {
            var memento = mazeDocument.CreateMemento();
            var serializedMaze = JsonConvert.SerializeObject(memento);
            
            var mazeBlob = await GetReferenceToBlob();
            await mazeBlob.UploadTextAsync(serializedMaze);
        }

        private static async Task<CloudBlockBlob> GetReferenceToBlob()
        {
            var storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["maze.storage"].ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var mazeAgentContainer = blobClient.GetContainerReference("mazeagent");
            await mazeAgentContainer.CreateIfNotExistsAsync();

            var mazeBlob = mazeAgentContainer.GetBlockBlobReference("maze.json");
            return mazeBlob;
        }
    }
}