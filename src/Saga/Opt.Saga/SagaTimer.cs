using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class SagaTimer
    {
        private readonly int _elapsed;
        private readonly TimeSpan timeout;
        public Stopwatch _chronometer;
        public SagaTimer(TimeSpan timeout, bool autoStart = true)
        {
            this.timeout = timeout;
            if (autoStart)
                this._chronometer = Stopwatch.StartNew();
            else
                this._chronometer = new Stopwatch();
        }
        public void Start()
        {
            this._chronometer.Start();
        }
        public TimeSpan Ellapsed => this._chronometer.Elapsed;
        public bool IsTimedOut => this._chronometer.Elapsed > timeout;
    }
}
