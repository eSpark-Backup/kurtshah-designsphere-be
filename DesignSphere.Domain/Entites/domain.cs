using DesignSphere.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace DesignSphere.Domain.Entites;
public class Address : EntityBase, ICompanyAddress
{
    public string CountryName { get; set; }
    public ushort PostalCode { get; set; }
}
public interface ICompanyAddress : IAddress
{
}

public enum EMUnits
{
    Metric, Imperial
}

public class ProjectPlan : EntityBase, IProjectPlan
{
    [ForeignKey(nameof(Project))]
    public Ulid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<DoorOpening> DoorOpenings { get; set; }
    public virtual Project Project { get; }
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
public interface IDoorMode
{
    string Name { get; set; }
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
public interface IManufacturer : IGeneric
{

}
public class PowerSupply : Product, IPowerSupply
{
    public float Current { get; set; }
    public float VoltageString { get; set; }
    public ushort Type { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; }
}
public interface IPowerSupply
{
    float Current { get; set; }
    float VoltageString { get; set; }
    UInt16 Type { get; set; }

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

public interface IMaterial : IGeneric
{

}
public interface IGeneric : IName
{
    string Description { get; set; }
}
public interface IName
{
    string Name { set; get; }
}
public interface IUnitPrice
{
    float UnitPrice { get; set; }
}
public interface ISize
{
    float Width { get; set; }
    float Height { get; set; }
    float Thickness { get; set; }
}
public class DoorOpening : EntityBase, IDoorOpening
{
    public uint Number { get; set; }
    public float Thickness { get; set; }
    public EDoorCondition ActiveStopCondition { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
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
public interface IDoorOpening : ISize
{
    uint Number { get; set; }
    EDoorCondition ActiveStopCondition { get; set; }
    Ulid? DoorId { get; set; }
    string? DoorDescription { get; set; }
    Ulid? DoorFrameId { get; set; }
    string? DoorFrameDescription { get; set; }
    Ulid ProjectPlanId { get; set; }
    Ulid? HardwareSetId { get; set; }
    string? HardwareSetDescription { get; set; }

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
public interface IProduct : IGeneric, ISize, IUnitPrice, IProductManufacturer
{
    Ulid MaterialId { get; set; }
    string MaterialDescription { get; set; }
}
public interface IDoor
{
    EDoorPanelType PanelType { get; set; }
    EDoorHand Hand { get; set; }
}
public class DoorFrame : Product, IDoorFrame
{
    public ICollection<DoorOpening> DoorOpenings { get; set; }

}
public interface IDoorFrame
{
}

public interface IProjectPlan
{
    Ulid ProjectId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
}
public enum EProjectType
{
    Project = 0x01, DemoTraning, Master
}
public enum EProjectStage
{
    Design = 0x01,
    Construct,
    Operate
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
public interface IProjectDetail
{
    Ulid ProjectId { set; get; }
    Ulid ComapanyId { get; set; }
}
public class ProjectRole : EntityBase, IProjectRole
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<ProjectUserRole> ProjectUserRoles { get; set; }

}
public interface IProjectRole : IGeneric
{

}
public class ProjectUserRole : EntityBase, IProjectUserRole
{
    [ForeignKey(nameof(ProjectRole))]
    public Ulid ProjectRoleId { get; set; }
    [ForeignKey(nameof(Project))]
    public Ulid ProjectId { get; set; }
    public string UserId { get; set; }
}
public enum EMileStoneStatus
{
    Opened = 0, Closed = 1, Completed = 3,
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
public interface IMileStone : IGeneric
{
    Ulid ProjectId { get; set; }
    DateTime DueDate { get; set; }
    EMileStoneStatus Status { get; set; }
    float Percentage { get; set; }

}
public interface IProjectUserRole
{
    string UserId {  get; set; }
    Ulid ProjectRoleId { get; set; }
    Ulid ProjectId { get; set; }
}

public class Project : EntityBase, IProject
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public bool? HelpUs { get; set; }
    public ICollection<ProjectPlan>? ProjectPlans { get; }
    public ICollection<ProjectUserRole>? ProjectUserRoles { get; }
    public ICollection<ProjectLabel>? ProjectLabels { get; set; }
    public ICollection<Post>? Posts { get; set; }

}

public class ProjectLabel : EntityBase, IProjectLabel
{
    public string ColorText { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
}
public interface IProjectLabel : IGeneric
{
    string ColorText { get; set; }
}
public enum IReactionType 
{
    Like, DisLike
}
public class Reaction : EntityBase, IReaction
{
    public IReactionType Type { get; set; }
    [ForeignKey(nameof(Post))]
    public Ulid PostId { get; set; }
}
public interface IReaction
{
    IReactionType Type { get; set; }
    Ulid PostId { get; set; }
}
public enum IPostType
{
    Comment = 0, Story, Issue
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
public interface IPost
{
    string Title { get; set; }
    string Description { get; set; }
    IPostType Type { get; set; }
    Ulid ProjectId { get; set; }
    ICollection<Reaction> Reactions { get; set; }

}
public interface IProject
{
    string Name { get; set; }
    DateTime? PublishDate { get; set; }
    DateTime? DeviverableDate { get; set; }
    EProjectStage Stage { get; set; }
    EProjectType Type { get; set; }
    bool? HelpUs { get; set; }

}
public interface IAddress
{
    string CountryName { get; set; }
    ushort PostalCode { get; set; }
}
public class HardwareSeries : EntityBase, IHardwareSeries
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; set; }
}
public interface IHardwareSeries : IGeneric
{

}
public class HardwareCategory : EntityBase, IHardwareCategory
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ICollection<HardwareSet> HardwareSets { get; set; }
}
public interface IHardwareCategory : IGeneric
{

}
public interface IProductManufacturer
{
    public Ulid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
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
public interface IHardwareSet
{
    Ulid HardwareCategoryId { get; set; }
    string HardwareCategoryDescription { get; set; }
    Ulid HardwareSeriesId { get; set; }
    string HardwareSeriesDescription { get; set; }
    Ulid PowerSupplyId { get; set; }
    Ulid PowerSupplyDescription { get; set; }

}
public enum ECatalogTypes
{
    Master = 0x01,
    Project = 0x02,
    User = 0x03,
}

public class Catalog : EntityBase, ICatalog
{
    public ECatalogTypes Type { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
}
public interface ICatalog : IGeneric
{
    ECatalogTypes Type { get; set; }
}
public class Contarctor : EntityBase, IContactor
{
    public string ContarctorName { get; set; }
    [ForeignKey(nameof(Entites.Address))]
    public Ulid AddressId { get; set; }
    public Address Address { get; set; }
}
public interface IContactor
{
    string ContarctorName { get; set; }
    Address Address { get; set; }
}
public class UserComnpany : EntityBase, IUserComnpany
{
    public string UserId { get; set; }
    [ForeignKey(nameof(Contarctor))]
    public Ulid CompanyId { get; set; }
    public virtual Contarctor AppComnpanyUser { get; }

}
public interface IUserComnpany
{
    string UserId { get; set; }
    Ulid CompanyId { get; set; }
}


