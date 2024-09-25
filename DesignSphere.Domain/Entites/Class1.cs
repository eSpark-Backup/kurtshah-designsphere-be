using DesignSphere.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DesignSphere.Domain.Entites;
public class CompanyAddress : EntityBase, ICompanyAddress
{
    public string CountryName { get; set; }
    public ushort PostalCode { get; set; }
}
public interface ICompanyAddress : IAddress
{
}
public class Building : EntityBase, IBuilding
{
    public string Name { get; }
}
public interface IBuilding
{
    string Name { get; }
}
public class ProjectCollaborator : EntityBase, IProjectCollaborator
{
    [ForeignKey(nameof(Project))]
    public string ProjectId { get; set; }
    [ForeignKey(nameof(AppUser))]
    public string UserId { get; set; }
    public virtual AppUser AppUser { get; }
    public virtual Project Project { get; }
}
public interface IProjectCollaborator
{
    string ProjectId { get; set; }
    string UserId { get; set; }
}
public class ProjectDetail : EntityBase, IProjectDetail
{
    [ForeignKey(nameof(Comnpany))]
    public Guid ComapanyId { get; set; }
    public virtual Comnpany Comnpany { get; }

}
public interface IProjectDetail
{
    Guid ComapanyId { get; set; }
}
public class ProjectPlan : EntityBase, IProjectPlan
{
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual Project Project { get; }

}
public interface IProjectPlan
{
    Guid ProjectId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
}
public enum EProjectType 
{
    A, B , C
}
public enum EProjectStage
{
    A,
    B,
    C
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
    public virtual ICollection<ProjectPlan> GetProjectPlans { get; }

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
public class Comnpany : EntityBase, IComnpany
{
    public string CompanyName { get; set; }
    [ForeignKey(nameof(CompanyAddress))]
    public Guid AddressId { get; set; }
    public CompanyAddress Address { get; set; }
}
public interface IComnpany
{
    string CompanyName { get; set; }
    CompanyAddress Address { get; set; }
}
public class UserComnpany : EntityBase, IUserComnpany
{
    [ForeignKey(nameof(AppUser))]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(Comnpany))]
    public Guid CompanyId { get; set; }
    public virtual Comnpany AppComnpanyUser { get; }
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
public interface IContact 
{

}
public class AppUser : IdentityUser, IAppUserExtention
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public bool NewsLetterSubscription { get; set; } = true;
}
