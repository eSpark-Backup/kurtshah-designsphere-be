using Bogus;
using Bogus.DataSets;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using Microsoft.Extensions.Logging;
using Polly;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Infrastructure.Persistence;
public class SqlDbContextSeed
{
    private static Ulid creatorID = Ulid.NewUlid();
    private static string creatorName = "Seeder";
    public static void Seed(SQLContext context, int projectCount = 5, int plansPerProject = 3, int doorsPerPlan = 10)
    {
        // Check if ProjectPlans or DoorOpenings already contain data
        if (context.ProjectPlans.Any() || context.DoorOpenings.Any())
        {
            Console.WriteLine("Data already exists. Skipping seeding process.");
            return;
        }

        Console.WriteLine("Seeding data...");

        // Bogus configuration
        var projectFaker = new Faker<Project>()
            .RuleFor(p => p.Id, f => Ulid.NewUlid())
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.PublishDate, f => f.Date.Past(1))
            .RuleFor(p => p.DeviverableDate, f => f.Date.Future(1))
            .RuleFor(p => p.Stage, f => f.PickRandom<EProjectStage>())
            .RuleFor(p => p.Type, f => f.PickRandom<EProjectType>())
            .RuleFor(p => p.HelpUs, f => f.PickRandom<HelpOption>())
            .RuleFor(p => p.Comment, f => f.Lorem.Sentence())
            .RuleFor(p => p.OwnerId, f => "8366a889-fe66-401b-9b59-05c8b957c250")
            .RuleFor(p => p.OwnerName, f => f.Person.FullName)
            .RuleFor(p => p.CompanyName, f => f.Company.CompanyName());

        var projectPlanFaker = new Faker<ProjectPlan>()
            .RuleFor(pp => pp.Id, f => Ulid.NewUlid())
            .RuleFor(pp => pp.Name, f => f.Commerce.ProductName())
            .RuleFor(pp => pp.Description, f => f.Lorem.Paragraph());

        var doorOpeningFaker = new Faker<DoorOpening>()
            .RuleFor(d => d.Id, f => Ulid.NewUlid())
            .RuleFor(d => d.Number, f => f.Random.UInt(1, 100))
            .RuleFor(d => d.Thickness, f => f.Random.Float(1.5f, 5.0f))
            .RuleFor(d => d.Mode, f => f.PickRandom<EDoorMode>())
            .RuleFor(d => d.ActiveStopCondition, f => f.PickRandom<EDoorCondition>())
            .RuleFor(d => d.Width, f => f.Random.Float(70.0f, 150.0f))
            .RuleFor(d => d.Height, f => f.Random.Float(200.0f, 250.0f))
            .RuleFor(d => d.DoorMaterial, f => f.PickRandom<EDoorMaterial>())
            .RuleFor(d => d.FrameMaterial, f => f.PickRandom<EDoorFrameMaterial>());

        // Generate mock data
        var projects = projectFaker.Generate(projectCount);

        foreach (var project in projects)
        {
            var projectPlans = projectPlanFaker.Generate(plansPerProject);
            foreach (var projectPlan in projectPlans)
            {
                projectPlan.ProjectId = project.Id;

                var doorOpenings = doorOpeningFaker.Generate(doorsPerPlan);
                foreach (var doorOpening in doorOpenings)
                {
                    doorOpening.ProjectPlanId = projectPlan.Id;
                    context.DoorOpenings.Add(doorOpening);
                }

                context.ProjectPlans.Add(projectPlan);
            }

            context.Projects.Add(project);
        }

        // Save changes to the database
        context.SaveChanges();

        Console.WriteLine("Seeding completed.");
    }
}

