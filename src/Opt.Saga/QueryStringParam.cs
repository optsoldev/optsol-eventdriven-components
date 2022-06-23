using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class QueryStringParam
    {
        public string Path { get; set; }
        public string Param { get; set; }
        public string DefaultValue { get; set; }
    }
    public class QueryStringParamsTransform : List<QueryStringParam>
    {

    }
}
