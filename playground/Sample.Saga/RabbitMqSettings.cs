using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Saga;

public class RabbitMqSettings
{
    public string Host { get; set; }
    public string Vhost { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
