using Bogus;
using DesignSphere.Domain.Entites;
using Microsoft.Extensions.Logging;
using Polly;

namespace DesignSphere.Infrastructure.Persistence;
public class SqlDbContextSeed
{
    private static Ulid creatorID = Ulid.NewUlid();
    private static string creatorName = "Seeder";
    private static int records = 2;
    public static async Task SeedAsync(SQLContext context, ILogger<SqlDbContextSeed> logger)
    {
        //if (context.Doors.Count() == 0)
        //{
        //    var mafucturers = GetManuidfactureFakeData();
        //    var materials = GetMaterialFakeData();

        //    var doors = GetDoorFakeData(materials.FirstOrDefault(), mafucturers.FirstOrDefault());
        //    context.Doors.AddRange(doors);
        //    context.SaveChanges();
        //}
    }
    //private static IEnumerable<Door> GetDoorFakeData(Material mtr, Manufacturer mfp)
    //{
    //    return new Faker<Door>()
    //        .UseSeed(42)
    //        .Ignore(m => m.Id)
    //.RuleFor(m => m.CreatorId, m => creatorID)
    //.RuleFor(m => m.CreatorName, m => creatorName)
    //.RuleFor(m => m.ManufacturerId, m => mfp.Id)
    //.RuleFor(m => m.ManufacturerName, m => mfp.Name)
    //.RuleFor(m => m.MaterialId, m => mtr.Id)
    //.RuleFor(m => m.MaterialDescription, m => mtr.Description)
    //.Generate(5);
    //}

    //private static IEnumerable<Manufacturer> GetManuidfactureFakeData()
    //{
    //    return new Faker<Manufacturer>()
    //        .UseSeed(42)
    //        .Ignore(m => m.Id)
    //        .RuleFor(m => m.CreatorId, m => creatorID)
    //        .RuleFor(m => m.CreatorName, m => creatorName)
    //        .Generate(5);
    //}

    //private static IEnumerable<Material> GetMaterialFakeData()
    //{
    //    return new Faker<Material>()
    //        .UseSeed(42)
    //        .RuleFor(m => m.CreatorId, m => creatorID)
    //        .RuleFor(m => m.CreatorName, m => creatorName)
    //        .Ignore(m => m.Id)
    //        .Generate(5);
    //}
}
