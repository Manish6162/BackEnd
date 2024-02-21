using Microsoft.Azure.Cosmos;

namespace BackEnd.Entities
{
    public class CosmosDbContext
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName;
        private readonly string _usersContainerName;
        private readonly string _feedsContainerName;
        private readonly string _commentsContainerName;
        private readonly string _chatsContainerName;
        private readonly string _notificationsContainerName;

        public Container UsersContainer { get; private set; }
        public Container FeedsContainer { get; private set; }

        public Container CommentsContainer { get; private set; }

        public Container ChatsContainer { get; private set; }
        public Container NotificationsContainer { get; private set; }


        public CosmosDbContext(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _cosmosClient = cosmosClient;
            _databaseName = configuration["CosmosDbSettings:DatabaseName"];
            _usersContainerName = configuration["CosmosDbSettings:UsersContainerName"];
            _feedsContainerName = configuration["CosmosDbSettings:FeedsContainerName"];
            _commentsContainerName = configuration["CosmosDbSettings:FeedsContainerName"];
            _chatsContainerName = configuration["CosmosDbSettings:FeedsContainerName"];
            _notificationsContainerName = configuration["CosmosDbSettings:FeedsContainerName"];

            UsersContainer = _cosmosClient.GetContainer(_databaseName, _usersContainerName);
            FeedsContainer = _cosmosClient.GetContainer(_databaseName, _feedsContainerName);

            CommentsContainer = _cosmosClient.GetContainer(_databaseName, _commentsContainerName);
            ChatsContainer = _cosmosClient.GetContainer(_databaseName, _chatsContainerName);
            NotificationsContainer = _cosmosClient.GetContainer(_databaseName, _notificationsContainerName);

        }
    }
}
