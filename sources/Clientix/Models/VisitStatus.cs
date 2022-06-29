using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZabotaSDK.Attributes;

namespace Clientix.Models
{
    public class VisitStatus
    {
        [ZabotaProperty("visit_id")]
        public Int64 VisitId { get; set; }

        [ZabotaProperty("status_id")]
        public Int64 StatusId { get; set; }

        public VisitStatus()
        {

        }

        public VisitStatus(Int64 VisitId, Int64 StatusId)
        {
            this.VisitId = VisitId;
            this.StatusId = StatusId;
        }
    }
}
