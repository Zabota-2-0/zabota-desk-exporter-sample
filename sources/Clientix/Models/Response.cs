using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clientix.Models
{
    public class Response<T> 
    {
        [JsonIgnore]
        public int Count
        {
            get => Data.Count;
        }               

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("items")]
        public List<T> Data { get; set; }

        public bool HasError
        {
            get => !string.IsNullOrEmpty(Error); 
        }

        public string Error { get; set; }

    }
}
