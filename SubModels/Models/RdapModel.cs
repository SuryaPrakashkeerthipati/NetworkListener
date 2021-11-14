using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubModels.Models
{
    public class Link
    {
        public string value { get; set; }
        public string rel { get; set; }
        public string href { get; set; }
        public string type { get; set; }
    }

    public class Notice
    {
        public string title { get; set; }
        public List<string> description { get; set; }
        public List<Link> links { get; set; }
    }

    public class Event
    {
        public string eventAction { get; set; }
        public DateTime eventDate { get; set; }
    }

    public class Remark
    {
        public List<string> description { get; set; }
        public string title { get; set; }
    }

    public class Entity
    {
        public List<string> roles { get; set; }
        public List<Event> events { get; set; }
        public List<Link> links { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public List<object> vcardArray { get; set; }
        public string objectClassName { get; set; }
        public string handle { get; set; }
        public List<Remark> remarks { get; set; }
    }

    public class Cidr0Cidrs
    {
        public string v4prefix { get; set; }
        public int length { get; set; }
    }

    public class RdapModel
    {
        public List<string> rdapConformance { get; set; }
        public List<Notice> notices { get; set; }
        public string country { get; set; }
        public List<Event> events { get; set; }
        public string name { get; set; }
        public List<Remark> remarks { get; set; }
        public List<Link> links { get; set; }
        public string type { get; set; }
        public string endAddress { get; set; }
        public string ipVersion { get; set; }
        public string startAddress { get; set; }
        public string objectClassName { get; set; }
        public string handle { get; set; }
        public List<Entity> entities { get; set; }
        public List<Cidr0Cidrs> cidr0_cidrs { get; set; }
        public string port43 { get; set; }
    }
}
