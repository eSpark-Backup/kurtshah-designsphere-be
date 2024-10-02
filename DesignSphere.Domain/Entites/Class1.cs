using DesignSphere.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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
    public Guid ProjectId { get; set; }
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
public class PowerSupply : EntityBase, IPowerSupply
{
    public float Current { get; set; }
    public float VoltageString { get; set; }
    public ushort Type { get; set; }
    public Guid MaterialId { get; set; }
    public string MaterialDescription { get; set; }
    public virtual Material Material { get; }
    public string Description { get; set; }
    public string Name { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Thickness { get; set; }
    public float UnitPrice { get; set; }
    public Guid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
    public virtual Manufacturer Manufacturer { get; }
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
    public Guid? DoorFrameId { get; set; }
    public string? DoorFrameDescription { get; set; }
    public DoorFrame? DoorFrame { get; }
    [ForeignKey(nameof(ProjectPlan))]
    public Guid ProjectPlanId { get; set; }
    public ProjectPlan ProjectPlan { get; }
    [ForeignKey(nameof(Door))]
    public Guid? DoorId { get; set; }
    public string? DoorDescription { get; set; }
    public Door? Door { get; }
    [ForeignKey(nameof(HardwareSet))]
    public Guid? HardwareSetId { get; set; }
    public string? HardwareSetDescription { get; set; }
    public HardwareSet? HardwareSet { get; }
}
public interface IDoorOpening : ISize
{
    uint Number { get; set; }
    EDoorCondition ActiveStopCondition { get; set; }
    Guid? DoorId { get; set; }
    string? DoorDescription { get; set; }
    Guid? DoorFrameId { get; set; }
    string? DoorFrameDescription { get; set; }
    Guid ProjectPlanId { get; set; }
    Guid? HardwareSetId { get; set; }
    string? HardwareSetDescription { get; set; }

}
public class Door : Product, IDoor
{
    public EDoorPanelType PanelType { get; set; }
    public EDoorHand Hand { get; set; }
}
public class Product : EntityBase, IProduct
{
    [ForeignKey(nameof(Material))]
    public Guid MaterialId { get; set; }
    public string MaterialDescription { get; set; }
    public virtual Material Material { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Thickness { get; set; }
    public float UnitPrice { get; set; }
    [ForeignKey(nameof(Manufacturer))]
    public Guid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
    public virtual Manufacturer Manufacturer { get; set; }
    public ICollection<DoorOpening> DoorOpenings { get; set; }
}
public interface IProduct : IGeneric, ISize, IUnitPrice, IProductManufacturer
{
    Guid MaterialId { get; set; }
    string MaterialDescription { get; set; }
}
public interface IDoor
{
    EDoorPanelType PanelType { get; set; }
    EDoorHand Hand { get; set; }
}
public class DoorFrame : Product, IDoorFrame
{

}
public interface IDoorFrame
{
}

public interface IProjectPlan
{
    Guid ProjectId { get; set; }
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
    public Guid ComapanyId { get; set; }
    public EMUnits Unit { get; set; }
    public DateTime? DueDate { get; set; }
    public virtual Contarctor Comnpany { get; }
}
public interface IProjectDetail
{
    Guid ComapanyId { get; set; }
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
    public Guid ProjectRoleId { get; set; }
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
}
public interface IProjectUserRole
{
    Guid ProjectRoleId { get; set; }
    Guid ProjectId { get; set; }
}

public class Project : EntityBase, IProject
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public bool? HelpUs { get; set; }
    public string? Comment { get; set; }
    public ICollection<ProjectPlan> ProjectPlans { get; }
    public ICollection<ProjectUserRole> ProjectUserRoles { get; }
}

public interface IProject
{
    string Name { get; set; }
    DateTime? PublishDate { get; set; }
    DateTime? DeviverableDate { get; set; }
    EProjectStage Stage { get; set; }
    EProjectType Type { get; set; }
    bool? HelpUs { get; set; }
    string? Comment { get; set; }
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
    public Guid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
}
public class HardwareSet : Product, IHardwareSet
{
    [ForeignKey(nameof(HardwareCategory))]
    public Guid HardwareCategoryId { get; set; }
    public string HardwareCategoryDescription { get; set; }
    [ForeignKey(nameof(HardwareSeries))]
    public Guid HardwareSeriesId { get; set; }
    public string HardwareSeriesDescription { get; set; }
    [ForeignKey(nameof(PowerSupply))]
    public Guid PowerSupplyId { get; set; }
    public Guid PowerSupplyDescription { get; set; }
}
public interface IHardwareSet
{
    Guid HardwareCategoryId { get; set; }
    string HardwareCategoryDescription { get; set; }
    Guid HardwareSeriesId { get; set; }
    string HardwareSeriesDescription { get; set; }
    Guid PowerSupplyId { get; set; }
    Guid PowerSupplyDescription { get; set; }

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
    public Guid AddressId { get; set; }
    public Address Address { get; set; }
}
public interface IContactor
{
    string ContarctorName { get; set; }
    Address Address { get; set; }
}
public class UserComnpany : EntityBase, IUserComnpany
{
    [ForeignKey(nameof(AppUser))]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(Contarctor))]
    public Guid CompanyId { get; set; }
    public virtual Contarctor AppComnpanyUser { get; }
    public virtual AppUser AppUser { get; }

}
public interface IUserComnpany
{
    Guid UserId { get; set; }
    Guid CompanyId { get; set; }
}
public interface IAppUserExtention
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string ProfilePictureUrl { get; set; }
    bool NewsLetterSubscription { get; set; }
}

public class AppUser : IdentityUser, IAppUserExtention
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public bool NewsLetterSubscription { get; set; } = true;
}
