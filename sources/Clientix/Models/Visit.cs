using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZabotaSDK.Attributes;

namespace Clientix.Models
{
    public class Visit
    {
        [ZabotaProperty("visit_Id")]
        [JsonProperty("Id")]
        public Int64 VisitId { get; set; }

        [ZabotaProperty("patient_id")]
        [JsonProperty("client_id")]
        public Int64? PatientId { get; set; }

        [ZabotaProperty("doctor_id")]
        [JsonIgnore]
        public Int64? DoctorId { get; set; }

        [ZabotaProperty("visit_date")]
        [JsonProperty("start_datetime")]
        public DateTime? VisitDate { get; set; }

        [ZabotaProperty("plan_duration")]
        [JsonIgnore]
        public int PlanDuration { get => CheckoutTime.HasValue && CheckinTime.HasValue ? ((int)((CheckoutTime.Value - CheckinTime.Value).TotalMinutes)) : 30; }

        [ZabotaProperty("checkin_time")]
        [JsonIgnore]
        public DateTime? CheckinTime { get => VisitDate; }

        [ZabotaProperty("checkout_time")]
        [JsonProperty("finish_datetime")]
        public DateTime? CheckoutTime { get; set; }

        [ZabotaProperty("appoint_id")]
        [JsonIgnore]
        public Int64 AppointId { get => VisitId; }

       // [ZabotaProperty("chid")]
      //  [JsonProperty("chair_id")]
      //  public String ChairId { get; set; }

        [ZabotaProperty("created")]
        [JsonProperty("created")]
        public DateTime? Created { get; set; }

        [ZabotaProperty("cust_id")]
        [JsonProperty("account_id")]
        public Int64 CustId { get; set; }

        [ZabotaNoSerialize]
        [JsonProperty("appointed_services")]
        public List<Treatment> Treatments { get; set; }

        [ZabotaNoSerialize]
        [JsonProperty("status")]
        public string Status { get; set; }

        [ZabotaProperty("visit_state")]
        [JsonIgnore]
        public string VisitState { get; set; }
    }
}
