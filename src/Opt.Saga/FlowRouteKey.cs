using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class FlowRouteKey
    {
        public string FlowKey { get; set; }
        public string ResponsePropertyName { get; set; }
        public IEnumerable<PropertyMap>? OutputMappings { get; set; }
    }
    public class AggregateRouteKeys : List<FlowRouteKey> { }
}
