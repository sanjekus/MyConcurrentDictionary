using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;
using System.Diagnostics;
using System.Threading;

namespace ConcurrentDictionary.Source.Utilities
{
    [Serializable]
    public sealed class MyTraceAttribute : PostSharp.Aspects.OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);
            Trace.WriteLine(string.Format("Entering: {0}, current thread Id: {1}",
                args.Method.Name,
                Thread.CurrentThread.ManagedThreadId.ToString()));
        }
    }
}
