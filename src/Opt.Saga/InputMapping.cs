using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class InputMapping
    {
        public string StepName { get; set; }
        public IEnumerable<PropertyMap> PropertyChanges { get; set; }
    }
}
