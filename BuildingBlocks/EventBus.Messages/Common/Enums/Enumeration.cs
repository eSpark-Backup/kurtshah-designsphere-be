
namespace EventBus.Messages.Common.Enums;
public  class Enumeration
{

    public enum EProjectStage
    {
        Design = 0x01,
        Construct,
        Operate
    }

    public enum EProjectType
    {
        Project = 0x01, DemoTraning, Master
    }

    public enum IReactionType
    {
        Like, DisLike
    }

    public enum IPostType
    {
        Comment = 0, Story, Issue
    }

    public enum ECatalogTypes
    {
        Master = 0x01,
        Project = 0x02,
        User = 0x03,
    }

    public enum EMUnits
    {
        Metric, Imperial
    }

    public enum EMileStoneStatus
    {
        Opened = 0, Closed = 1, Completed = 3,
    }

    public enum EDoorHand
    {
        Left, Right, Sliding
    }
    public enum EDoorCondition
    {
        Door, Wall
    }
    public enum EDoorPanelType
    {
        Flush, FullGlass
    }

}
