using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class SagaConfiguration
    {
        public List<global::SagaEndpoint> Endpoints { get; set; }
    }
}
public class SagaEndpoint
{
    public string FlowKey { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
