
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IPost
{
    string Title { get; set; }
    string Description { get; set; }
    IPostType Type { get; set; }
    Ulid ProjectId { get; set; }
   // ICollection<Reaction> Reactions { get; set; }

}