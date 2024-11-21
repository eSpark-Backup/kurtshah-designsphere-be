using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;

    public interface IProject
    {
        string Name { get; set; }
        DateTime? PublishDate { get; set; }
        DateTime? DeviverableDate { get; set; }
        EProjectStage Stage { get; set; }
        EProjectType Type { get; set; }
        bool? HelpUs { get; set; }

    }

