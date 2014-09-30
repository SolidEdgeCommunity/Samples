using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ApiDemos
{
    class SampleProxy : MarshalByRefObject
    {
        public void RunSample(MethodInfo method, object[] parameters, TextWriter consoleOut)
        {
            if (consoleOut != null)
            {
                Console.SetOut(consoleOut);
            }

            method.Invoke(null, parameters);
        }
    }
}
