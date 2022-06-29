using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZabotaSDK.Attributes;

namespace Clientix.Models
{
    public class Treatment
    {
        [ZabotaProperty("treatment_Id")]
        [JsonProperty("id")]
        public Int64 TreatmentId { get; set; }

        [ZabotaProperty("patient_id")]
        [JsonIgnore]
        public Int64? PatientId { get; set; }

        [ZabotaProperty("procedure_id")]
        [JsonProperty("service_id")]
        public Int64? ProcedureId { get; set; }

        [ZabotaProperty("treatment_date")]
        [JsonIgnore]
        public DateTime? TreatmentDate { get; set; }

        [JsonProperty("price")]
        public double Total { get; set; }

        [ZabotaProperty("doctor_id")]
        [JsonIgnore]
        public Int64? DoctorId { get; set; }

        [ZabotaProperty("cust_id")]
        [JsonIgnore]
        public Int64? CustId { get; set; }

        [JsonProperty("appointed_services_executions")]
        [ZabotaNoSerialize]
        public List<Doctor> Doctors { get; set; }
    }
}
