using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitSyncFramework.Models
{
    public  class RequestDto
    {
        public string ProjectName { get; set; }

        public string FileName { get; set; }
        public List<DoorData> DoorPlans { get; set; }

    }
}
