using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace EventBus.Messages.Contract;
public interface IConversationDetail
{
    Ulid ConversationId { get; set; }
    string Sender { get; set; }
    string Receiver { get; set; }
    MessageType MessageType { get; set; }
    string MessageContent { get; set; }
}