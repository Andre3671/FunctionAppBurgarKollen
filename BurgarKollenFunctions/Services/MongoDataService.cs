using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using BurgarKollenFunctions;
using BurgarKollen.Lib2;

namespace BurgarKollenFunctions.Services;

public class DataServiceOptions
{
    public string? ConnectionString { get; set; } = "mongodb+srv://BurgarKollen:l8Rwh7gRDT76jDra@burgarkollen.2z6elbx.mongodb.net/?retryWrites=true&w=majority";
    public string? Database { get; set; } = "BurgarKollen";
}

public static class DataServiceExtentions
{
    public static void AddDataService(this IServiceCollection services, Action<DataServiceOptions>? options = null)
    {
        var DataOptions = new DataServiceOptions();
        options?.Invoke(DataOptions);
        if (options is not null)
        {
            services.Configure(options);
        }
        services.AddTransient<MongoDataService>();
    }
}

public class MongoDataService
{
    private readonly IOptions<DataServiceOptions> _options;
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDataService(IOptions<DataServiceOptions> options)
    {
        _options = options;
        _mongoClient = new MongoClient(_options.Value.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(_options.Value.Database);
    }

    private IMongoCollection<T> GetCollection<T>()
    {
        return _mongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    public async Task<List<T>> GetAllAsync<T>()
    {
        return await GetCollection<T>().Find(_ => true).ToListAsync();
    }

    public async Task<List<T>> GetWhereAsync<T>(FilterDefinition<T> filter)
    {
        return await GetCollection<T>().Find(filter).ToListAsync();
    }

    public IMongoQueryable<T> GetQuerableCollection<T>()
    {
        return GetCollection<T>().AsQueryable();
    }

    public async Task AddUpdateAsync<T>(T item) where T : IMongoDBModel
    {
        if (item.Id is null)
        {
            GetCollection<T>().InsertOne(item);
        }
        else
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
            await GetCollection<T>().ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true });
        }
    }

    public async Task DeleteAsync<T>(T item) where T : IMongoDBModel
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, item.Id);
        await GetCollection<T>().DeleteOneAsync(filter);
    }
}
