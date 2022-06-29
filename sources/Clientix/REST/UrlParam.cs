using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clientix.REST
{
    public class UrlParam
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public UrlParam()
        {
            Key = "";
            Value = "";
        }

        public UrlParam(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", Key, Value);
        }
    }
}
