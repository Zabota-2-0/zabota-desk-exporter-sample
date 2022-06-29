using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZabotaSDK.Attributes;

namespace Clientix.Models
{
    public class Procedure
    {
        [ZabotaProperty("procedure_id")]
        [JsonProperty("id")]
        public string Procedure_Id { get; set; }

        [ZabotaProperty("descr")]
        [JsonIgnore]
        public string Description { get => Name; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
