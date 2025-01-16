using DesignSphere.Domain.Common;
using EventBus.Messages.Contract;
using SieveCustom.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Domain.Entites;
public class Address : EntityBase, ICompanyAddress
{
    public string CountryName { get; set; }
    public ushort PostalCode { get; set; }
}



public class ProjectPlan : EntityBase, IProjectPlan
{
    [Sieve(CanFilter = true)]
    [ForeignKey(nameof(Project))]
    public Ulid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<DoorOpening> DoorOpenings { get; set; }
    public virtual Project Project { get; }
}

public class Manufacturer : EntityBase, IManufacturer
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; set; }
    public ICollection<PowerSupply> PowerSupplies { get; set; }
    public ICollection<Door> Doors { get; set; }
    public ICollection<DoorFrame> DoorFrames { get; set; }
}

public class PowerSupply : Product, IPowerSupply
{
    public float Current { get; set; }
    public float VoltageString { get; set; }
    public ushort Type { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; }
}

public class Material : EntityBase, IMaterial
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; set; }
    public ICollection<PowerSupply> PowerSupplies { get; set; }
    public ICollection<Door> Doors { get; set; }
    public ICollection<DoorFrame> DoorFrames { get; set; }
}

public class DoorOpening : EntityBase, IDoorOpening
{
    public uint Number { get; set; }
    public float Thickness { get; set; }

    public EDoorMode Mode { get; set; }

    public EDoorCondition ActiveStopCondition { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public EDoorMaterial DoorMaterial { get; set; }

    public EDoorFrameMaterial FrameMaterial { get; set; }


    [ForeignKey(nameof(DoorFrame))]
    public Ulid? DoorFrameId { get; set; }
    public string? DoorFrameDescription { get; set; }
    public DoorFrame? DoorFrame { get; }
    [ForeignKey(nameof(ProjectPlan))]
    public Ulid ProjectPlanId { get; set; }
    public ProjectPlan ProjectPlan { get; }
    [ForeignKey(nameof(Door))]
    public Ulid? DoorId { get; set; }
    public string? DoorDescription { get; set; }
    public Door? Door { get; }
    [ForeignKey(nameof(HardwareSet))]
    public Ulid? HardwareSetId { get; set; }
    public string? HardwareSetDescription { get; set; }
    public HardwareSet? HardwareSet { get; }
}

public class Door : Product, IDoor
{
    public EDoorPanelType PanelType { get; set; }
    public EDoorHand Hand { get; set; }
    public ICollection<DoorOpening> DoorOpenings { get; set; }

}
public abstract class Product : EntityBase, IProduct
{
    [ForeignKey(nameof(Material))]
    public Ulid MaterialId { get; set; }
    public string MaterialDescription { get; set; }
    public virtual Material Material { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Thickness { get; set; }
    public float UnitPrice { get; set; }
    [ForeignKey(nameof(Manufacturer))]
    public Ulid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
    public virtual Manufacturer Manufacturer { get; set; }
}


public class DoorFrame : Product, IDoorFrame
{
    public ICollection<DoorOpening> DoorOpenings { get; set; }

}

public class ProjectDetail : EntityBase, IProjectDetail
{
    [ForeignKey(nameof(Comnpany))]
    public Ulid ComapanyId { get; set; }
    public EMUnits Unit { get; set; }
    public DateTime? DueDate { get; set; }
    public virtual Contarctor Comnpany { get; }
    public Ulid ProjectId { get; set; }
}

public class ProjectRole : EntityBase, IProjectRole
{
    public string Name { get; set; }
    public ICollection<ProjectContributer> ProjectContributers { get; set; }
}

public class Contact  : EntityBase, IContact
{
    public string UserId { get; set; }

    [Sieve(CanFilter = true)]
    public string Name { get; set; }
    public string EmailAddress { get; set; }


}

public class ProjectContributer : EntityBase, IProjectContributer
{
    [ForeignKey(nameof(Contact))]
    public Ulid ContactId { get; set; }
    public virtual Contact Contact { get; set; } = default!;

    [ForeignKey(nameof(Project))]
    public Ulid ProjectId { get; set; }
    public virtual Project Project { get; set; } = default!;

    [ForeignKey(nameof(ProjectRole))]
    public Ulid ProjectRoleId { get; set; }
    public virtual ProjectRole ProjectRole { get; set; } = default!;


    public bool IsContributerRemoved { get; set; } = false;

}

public class MileStone : EntityBase, IMileStone
{
    [ForeignKey(nameof(ProjectDetail))]
    public Ulid ProjectDetailId { get; set; }
    public DateTime DueDate { get; set; }
    public EMileStoneStatus Status { get; set; }
    public float Percentage { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    [ForeignKey(nameof(Project))]
    public Ulid ProjectId { get; set; }
}



public class Project : EntityBase, IProject
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public HelpOption HelpUs { get; set; }
    public string Comment { get; set; }
    [Sieve(CanFilter = true)]
    public string? OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public string? CompanyName { get; set; }

    public ICollection<ProjectPlan>? ProjectPlans { get; }
    public ICollection<ProjectContributer>? ProjectContributers { get; }
    public ICollection<ProjectLabel>? ProjectLabels { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public ICollection<Task>? Tasks { get; set; }

}

public class ProjectLabel : EntityBase, IProjectLabel
{
    public string ColorText { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
}

public class Reaction : EntityBase, IReaction
{
    public IReactionType Type { get; set; }
    [ForeignKey(nameof(Post))]
    public Ulid PostId { get; set; }
}

public class Post : EntityBase, IPost
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IPostType Type { get; set; }
    [ForeignKey(nameof(Project))]
    public Ulid ProjectId { get; set; }
    public ICollection<Reaction> Reactions { get; set; }
}



public class HardwareSeries : EntityBase, IHardwareSeries
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; set; }
}

public class HardwareCategory : EntityBase, IHardwareCategory
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; set; }
}


public class HardwareSet : Product, IHardwareSet
{
    [ForeignKey(nameof(HardwareCategory))]
    public Ulid HardwareCategoryId { get; set; }
    public string HardwareCategoryDescription { get; set; }
    [ForeignKey(nameof(HardwareSeries))]
    public Ulid HardwareSeriesId { get; set; }
    public string HardwareSeriesDescription { get; set; }
    [ForeignKey(nameof(PowerSupply))]
    public Ulid PowerSupplyId { get; set; }
    public Ulid PowerSupplyDescription { get; set; }
    public ICollection<DoorOpening> DoorOpenings { get; set; }

}


public class Catalog : EntityBase, ICatalog
{
    public ECatalogTypes Type { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
}

public class Contarctor : EntityBase, IContactor
{
    public string ContarctorName { get; set; }
    [ForeignKey(nameof(Entites.Address))]
    public Ulid AddressId { get; set; }
    public Address Address { get; set; }
}

public class UserComnpany : EntityBase, IUserComnpany
{
    public string UserId { get; set; }
    [ForeignKey(nameof(Contarctor))]
    public Ulid CompanyId { get; set; }
    public virtual Contarctor AppComnpanyUser { get; }

}
public class ProjectStage : EntityBase, IProjectStage
{
    public long? Order { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

}
public class Task : EntityBase ,ITask
{

    [ForeignKey("Project")]
    public Ulid ProjectId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; } 

    public DateTime Deadline { get; set; } 

    public decimal TaskWeight { get; set; } 

    [Required]
    public ETaskStatus Status { get; set; } 

    public  ICollection<TaskDetail> TaskDetail { get; set; }
}

public class TaskDetail: EntityBase//,ITaskDetail
{

    [ForeignKey("Task")]
    public Ulid TaskId { get; set; } 

    [ForeignKey("TaskStages")]
    public Ulid TaskStageId { get; set; }  


    public virtual ICollection<TaskAssignee> TaskAssignees { get; set; } = new List<TaskAssignee>();
}
public class TaskStages : EntityBase//, ITaskStage
{

    public string Name { get; set; } //enumsType ,system,user,Project
    public int OrderNo { get; set; }
    public string? Reference { get; set; } //ulid store here TaskStageId

}
public class TaskAssignee :EntityBase//,ITaskAssignee
{
    [ForeignKey("TaskDetail")]
    public Ulid TaskDetailId { get; set; }

    [ForeignKey("ProjectContributer")]
    public Ulid ContributerId { get; set; }

    [Required]
    public ETaskStatus Status { get; set; } 
}

public class Conversation : EntityBase, IConversation
{

    public Ulid? TaskId { get; set; }

    [Required]
    [MaxLength(500)]
    public string TaskDescription { get; set; }

    public ICollection<ConversationDetail> ConversationDetails { get; set; } = new List<ConversationDetail>();
}

public class ConversationDetail //: IConversationDetail
{

    [Required]
    [ForeignKey("Conversation")]
    public Ulid ConversationId { get; set; }

    public Conversation conversation { get; set; }

    [Required]
    [MaxLength(100)]
    public string Sender { get; set; }

    [Required]
    [MaxLength(100)]
    public string ReceiverID { get; set; }

    [Required]
    public MessageType MessageType { get; set; }

    [Required]
    [MaxLength(1000)]

    public string MessageContent { get; set; }

    public string? Reference { get; set; }

}
