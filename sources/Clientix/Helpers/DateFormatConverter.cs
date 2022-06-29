using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clientix.Helpers
{
    public class DateFormatConverter : IsoDateTimeConverter
    {    
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
