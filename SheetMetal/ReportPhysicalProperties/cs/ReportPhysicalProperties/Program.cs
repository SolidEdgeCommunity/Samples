using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportPhysicalProperties
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            double density = 0;
            double accuracy = 0;
            double volume = 0;
            double area = 0;
            double mass = 0;
            Array cetnerOfGravity = Array.CreateInstance(typeof(double), 3);
            Array centerOfVolumne = Array.CreateInstance(typeof(double), 3);
            Array globalMomentsOfInteria = Array.CreateInstance(typeof(double), 6);     // Ixx, Iyy, Izz, Ixy, Ixz and Iyz 
            Array principalMomentsOfInteria = Array.CreateInstance(typeof(double), 3);  // Ixx, Iyy and Izz
            Array principalAxes = Array.CreateInstance(typeof(double), 9);              // 3 axes x 3 coords
            Array radiiOfGyration = Array.CreateInstance(typeof(double), 9);            // 3 axes x 3 coords
            double relativeAccuracyAchieved = 0;
            int status = 0;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active document.
                sheetMetalDocument = application.GetActiveDocument<SolidEdgePart.SheetMetalDocument>(false);

                if (sheetMetalDocument != null)
                {
                    density = 1;
                    accuracy = 0.05;

                    // Get a reference to the Models collection.
                    models = sheetMetalDocument.Models;

                    // Get a reference to the Model.
                    model = models.Item(1);

                    // Compute the physical properties.
                    model.GetPhysicalProperties(
                        Status: out status,
                        Density: out density,
                        Accuracy: out accuracy,
                        Volume: out volume,
                        Area: out area,
                        Mass: out mass,
                        CenterOfGravity: ref cetnerOfGravity,
                        CenterOfVolume: ref centerOfVolumne,
                        GlobalMomentsOfInteria: ref globalMomentsOfInteria,
                        PrincipalMomentsOfInteria: ref principalMomentsOfInteria,
                        PrincipalAxes: ref principalAxes,
                        RadiiOfGyration: ref radiiOfGyration,
                        RelativeAccuracyAchieved: out relativeAccuracyAchieved);

                    Console.WriteLine("GetPhysicalProperties() results:");

                    // Write results to screen.

                    Console.WriteLine("Density: {0}", density);
                    Console.WriteLine("Accuracy: {0}", accuracy);
                    Console.WriteLine("Volume: {0}", volume);
                    Console.WriteLine("Area: {0}", area);
                    Console.WriteLine("Mass: {0}", mass);

                    // Convert from System.Array to double[].  double[] is easier to work with.
                    double[] m = cetnerOfGravity.OfType<double>().ToArray();

                    Console.WriteLine("CenterOfGravity:");
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[0], m[1], m[2]);

                    m = centerOfVolumne.OfType<double>().ToArray();

                    Console.WriteLine("CenterOfVolume:");
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[0], m[1], m[2]);

                    m = globalMomentsOfInteria.OfType<double>().ToArray();

                    Console.WriteLine("GlobalMomentsOfInteria:");
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[0], m[1], m[2]);
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[3], m[4], m[5]);

                    m = principalMomentsOfInteria.OfType<double>().ToArray();

                    Console.WriteLine("PrincipalMomentsOfInteria:");
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[0], m[1], m[2]);

                    m = principalAxes.OfType<double>().ToArray();

                    Console.WriteLine("PrincipalAxes:");
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[0], m[1], m[2]);
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[3], m[4], m[5]);
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[6], m[7], m[8]);

                    m = radiiOfGyration.OfType<double>().ToArray();

                    Console.WriteLine("RadiiOfGyration:");
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[0], m[1], m[2]);
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[3], m[4], m[5]);
                    Console.WriteLine("\t|{0}, {1}, {2}|", m[6], m[7], m[8]);

                    Console.WriteLine("RelativeAccuracyAchieved: {0}", relativeAccuracyAchieved);
                    Console.WriteLine("Status: {0}", status);
                    Console.WriteLine();

                    // Show physical properties window.
                    application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalToolsPhysicalProperties);
                }
                else
                {
                    throw new System.Exception("No active document.");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
