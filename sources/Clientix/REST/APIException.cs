using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clientix.REST
{
    public class APIException : Exception
    {
        public APIException(string message) : base(message) { }
    }
}
