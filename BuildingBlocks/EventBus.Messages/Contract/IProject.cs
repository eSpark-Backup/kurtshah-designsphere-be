using SieveCustom.Attributes;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;

    public interface IProject 
{
    [Sieve(CanFilter = true)]
    string Name { get; set; }

        DateTime? PublishDate { get; set; }
        DateTime? DeviverableDate { get; set; }
        EProjectStage Stage { get; set; }
        EProjectType Type { get; set; }
        HelpOption HelpUs { get; set; }
         string? OwnerId { get; set; }
         string? OwnerName { get; set; }
           string? CompanyName { get; set; }


}

