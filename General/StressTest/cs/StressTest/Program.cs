using SolidEdgeCommunity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start Solid Edge.
            var application = SolidEdgeUtils.Connect(true, true);

            // Disable (most) prompts.
            application.DisplayAlerts = false;

            var process = Process.GetProcessById(application.ProcessID);
            var initialHandleCount = process.HandleCount;
            var initialWorkingSet = process.WorkingSet64;
            var trainingFolderPath = SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath();

            foreach (var fileName in Directory.EnumerateFiles(trainingFolderPath, "*.*", SearchOption.AllDirectories))
            {
                var extension = Path.GetExtension(fileName);

                if (
                    (extension.Equals(".asm", StringComparison.OrdinalIgnoreCase)) ||
                    (extension.Equals(".dft", StringComparison.OrdinalIgnoreCase)) ||
                    (extension.Equals(".par", StringComparison.OrdinalIgnoreCase)) ||
                    (extension.Equals(".psm", StringComparison.OrdinalIgnoreCase)) ||
                    (extension.Equals(".pwd", StringComparison.OrdinalIgnoreCase))
                   )
                {
                    Console.WriteLine("Opening & closing '{0}'", fileName);

                    var startHandleCount = process.HandleCount;
                    var startWorkingSet = process.WorkingSet64;

                    using (var task = new IsolatedTask<OpenCloseTask>())
                    {
                        task.Proxy.Application = application;
                        task.Proxy.DoOpenClose(fileName);
                    }

                    process.Refresh();

                    var endHandleCount = process.HandleCount;
                    var endWorkingSet = process.WorkingSet64;

                    Console.WriteLine("Handle count start\t\t'{0}'", startHandleCount);
                    Console.WriteLine("Handle count end\t\t'{0}'", endHandleCount);
                    Console.WriteLine("Handle count change\t\t'{0}'", endHandleCount - startHandleCount);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Handle count total change\t'{0}'", endHandleCount - initialHandleCount);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Working set start\t\t'{0}'", startWorkingSet);
                    Console.WriteLine("Working set end\t\t\t'{0}'", endWorkingSet);
                    Console.WriteLine("Working set change\t\t'{0}'", endWorkingSet - startWorkingSet);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Working set total change\t'{0}'", endWorkingSet - initialWorkingSet);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    

                    Console.WriteLine();
                }
            }

            if (application != null)
            {
                application.DisplayAlerts = true;
            }

#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
        }
    }
}
