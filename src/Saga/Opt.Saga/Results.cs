using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public enum SagaEventResultType
    {
        Success,
        Failure,
        NotExecuted
    }
    public class Message
    {
        public string Content { get; set; }
        public Message(string content)
        {
            Content = content;
        }
    }
    public class SagaProcessEventArgs : EventArgs
    {
        public Message Message { get; set; }
        public SagaEventResultType EventType { get; set; }
        public SagaProcessEventArgs()
        {

        }
        public SagaProcessEventArgs(string eventResultType, string content):this(Enum.Parse<SagaEventResultType>(eventResultType),content)
        {
            
        }
        public SagaProcessEventArgs(SagaEventResultType eventResultType, string content)
        {
            EventType = (eventResultType);
            Message = new Message(content);
        }
    }


}
