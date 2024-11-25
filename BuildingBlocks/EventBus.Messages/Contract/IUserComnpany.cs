using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Contract;
public interface IUserComnpany
{
    string UserId { get; set; }
    Ulid CompanyId { get; set; }
}

