using SubModels.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace SubModels.Extensions
{
    public static class ApiExtensions
    {
        public static PingModel ToPingModel(this PingReply pingReply)
        {
            return new PingModel
            {
                Status = pingReply.Status.ToString(),
                ExecutionTime = pingReply.RoundtripTime
            };
        }
    }
}
