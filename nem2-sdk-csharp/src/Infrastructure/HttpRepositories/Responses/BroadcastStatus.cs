using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class BroadcastStatus
    {
        public string Status { get; set; }
    }

    public class ExtendedBroadcastStatus
    {
        public string Group { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }
        public string Deadline { get; set; }
        public string Height { get; set; }
    }
}
