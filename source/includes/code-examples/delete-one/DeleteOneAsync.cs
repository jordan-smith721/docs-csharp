using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using static System.Console;

namespace CSharpExamples.UsageExamples.DeleteOne;

public class DeleteOneAsync
{
    private static IMongoCollection<Restaurant> _restaurantsCollection;
    private const string MongoConnectionString = "<Your MongoDB URI>";

    public static async Task Main(string[] args)
    {
        Setup();

        var filter = Builders<Restaurant>.Filter
            .Eq(r => r.Name, "Ready Penny Inn");

        var doc = _restaurantsCollection.Find(filter).First();

        // Delete a document using builders
        WriteLine("Deleting a document with builders...");
        var result = await DeleteARestaurantBuilderAsync();

        WriteLine($"Deleted documents: {result.DeletedCount}");

        Restore(doc);
    }

    private static async Task<DeleteResult> DeleteARestaurantBuilderAsync()
    {
        // start-delete-one-builders-async
        var filter = Builders<Restaurant>.Filter
            .Eq(r => r.Name, "Ready Penny Inn");

        return _restaurantsCollection.DeleteOneAsync(filter);
        // end-delete-one-builders-async
    }

    private static void Restore(Restaurant doc)
    {
        _restaurantsCollection.InsertOne(doc);
    }

    private static void Setup()
    {
        // This allows automapping of the camelCase database fields to our models. 
        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

        // Establish the connection to MongoDB and get the restaurants database
        var mongoClient = new MongoClient(_mongoConnectionString);
        var restaurantsDatabase = mongoClient.GetDatabase("sample_restaurants");
        _restaurantsCollection = restaurantsDatabase.GetCollection<Restaurant>("restaurants");
    }
}