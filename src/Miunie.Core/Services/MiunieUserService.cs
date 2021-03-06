using Miunie.Core.Storage;

namespace Miunie.Core
{
    public class MiunieUserService
    {
        private const string KeyFormat = "u{0}";
        private const string CollectionFormat = "g{0}";
        private readonly IDataStorage _dataStorage;

        public MiunieUserService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public MiunieUser GetById(ulong userId, ulong guildId)
        {
            var user = _dataStorage.RestoreObject<MiunieUser>(
                GetCollectionById(guildId),
                GetKeyById(userId)
            );

            return EnsureExistence(user, userId, guildId);
        }

        private MiunieUser EnsureExistence(
            MiunieUser user, 
            ulong userId, 
            ulong guildId)
        {
            if(user is null)
            {
                user = new MiunieUser
                {
                    GuildId = guildId,
                    Id = userId
                };
                StoreUser(user);
            }

           return user;
        }

        private void StoreUser(MiunieUser u)
            => _dataStorage.StoreObject(u,
                GetCollectionById(u.GuildId),
                GetKeyById(u.Id));

        private static string GetKeyById(ulong userId)
            => string.Format(KeyFormat, userId);

        private static string GetCollectionById(ulong guildId)
            => string.Format(CollectionFormat, guildId);
    }
}

