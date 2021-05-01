using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityServiceDBTracker
{
    [Serializable]
    public class QueryParameter
    {
        [Description("the name"), ReadOnly(true)]
        public string ParameterName { get; set; }
        
        [Description("the value"), ReadOnly(false)]
        public string ParameterValue { get; set; }
    }
}
