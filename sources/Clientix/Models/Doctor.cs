using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZabotaSDK.Attributes;
using Clientix.Helpers;

namespace Clientix.Models
{
    public class Doctor
    {
        [JsonProperty("id")]
        [ZabotaProperty("doctor_id")]
        public Int64 DoctorID { get; set; }

        [JsonIgnore]
        public Int64 Code { get => DoctorID; }

        private string _Name;

        [ZabotaNoSerialize]
        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;

                Surname = _Name.WordByIndex(0, ' ');
                Firstname = Name.WordByIndex(1, ' ');
                Midname = Name.WordByIndex(2, ' ');
            }
        }

        [ZabotaProperty("surname", true)]
        [JsonIgnore]
        public string Surname
        {
            get; set;
        }

        [ZabotaProperty("firstname", true)]
        [JsonIgnore]
        public string Firstname
        {
            get; set;
        }

        [ZabotaProperty("middlename", true)]
        [JsonIgnore]
        public string Midname
        {
            get; set;
        }
    }
}
