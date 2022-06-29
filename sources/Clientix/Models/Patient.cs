using Clientix.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZabotaSDK.Attributes;

namespace Clientix.Models
{

    public class Patient
    {
        [ZabotaProperty("patient_id")]
        [JsonProperty("id")]
        public Int64 PatientId { get; set; }

        [JsonProperty("phone")]
        public string Cellphone { get; set; }

        public string Email { get; set; }

        [ZabotaProperty("cust_id")]
        [JsonProperty("account_id")]
        public Int64 CustId { get; set; }

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
                Firstname = _Name.WordByIndex(1, ' ');
                Midname = _Name.WordByIndex(2, ' ');
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

        [ZabotaProperty("midname", true)]
        [JsonIgnore]
        public string Midname
        {
            get; set;
        }

        [ZabotaNoSerialize]
        [JsonProperty("birth_date")]
        public string BirthDay { get; set; }

        [JsonIgnore]
        public DateTime? Dob
        {
            get
            {
                if (string.IsNullOrEmpty(BirthDay))
                {
                    return null;
                }

                DateTime validDateTime;
                if (DateTime.TryParseExact(BirthDay, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out validDateTime))
                {
                    return validDateTime;
                }

                return null;
            }
        }

    }
}
