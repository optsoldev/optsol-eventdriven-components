using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Saga;

public class SagaMongoSettings
{
    public string Connection { get; set; }
    public string DatabaseName { get; set; }
}
