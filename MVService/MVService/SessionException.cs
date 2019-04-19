using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVService
{
    [Serializable]
    public class SessionException : Exception
    {
        private readonly string _message;
        public SessionException()
        { }
        public SessionException(string Message)
            : base(Message)
        {
            _message = Message;
        }

        public SessionException(string Message, Exception InterException)
            : base(Message, InterException)
        {
            if (Message == null) throw new ArgumentNullException(nameof(Message));
            _message = Message;
        }
    }
}
