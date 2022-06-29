using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZabotaSDK.Attributes;

namespace Clientix.Models
{
    public class StatusType
    {
        [ZabotaProperty("status_id")]
        public Int64 StatusId { get; set; }

        [ZabotaProperty("descr")]
        public string Description { get; set; }

        [ZabotaNoSerialize]
        public Boolean IsCancel { get; set; }

        public StatusType()
        {

        }

        public StatusType(Int64 StatusId, string Description, bool IsCancel = false)
        {
            this.StatusId = StatusId;
            this.Description = Description;
            this.IsCancel = IsCancel;
        }
    }
}
