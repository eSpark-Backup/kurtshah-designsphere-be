
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
    public enum EDoorMode
    {
        SingleDoor, SlidingDoor ,DoubleActingDoor
    }

    public enum EDoorMaterial
    {
        HollowMetalDoor, Gate, GlassDoor
    }
    public enum EDoorFrameMaterial
    {
        HollowMetalFrame, MetalFrame, GlassFrame
    }
    public enum HelpOption
    {
        Yes,
        No,
        NotSure
    }

    public enum EContributerRole
    {
        Owner = 0, Contributer =1, Devloper = 2, Viewer = 3,
    }

  public enum EProjectRole
    {
        Editor = 0, Viewer = 1
    }

    public enum ETaskStatus
    {
        completed = 0, Incompleted = 1 , InProgress = 2, NotStarted = 3, Deleted = 4

    }
    public enum ETaskStages
    {
        Initial = 0, Final = 1, Review = 2

    }

    public enum MessageType
    {
        Text =0,
        File=1,
        Image=2,
        Video=3,
        Other=4
    }
}
