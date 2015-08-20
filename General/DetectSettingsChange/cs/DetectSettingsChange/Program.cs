using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DetectSettingsChange
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Dictionary variables to hold before and after settings.
            var snapshot1 = new Dictionary<SolidEdgeFramework.ApplicationGlobalConstants, object>();
            var snapshot2 = new Dictionary<SolidEdgeFramework.ApplicationGlobalConstants, object>();

            // Connect to Solid Edge.
            var application = SolidEdgeUtils.Connect();

            // Begin snapshot 1 of application global constants.
            CaptureApplicationGlobalConstants(application, snapshot1);

            // Force break-point. Change the Solid Edge setting in question.
            System.Diagnostics.Debugger.Break();

            // Begin snapshot 2 of application global constants.
            CaptureApplicationGlobalConstants(application, snapshot2);

            // Report the changes from snapshot 1 and snapshot 2.
            var enumerator = snapshot1.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var enumConstant = enumerator.Current.Key;
                var enumValue1 = enumerator.Current.Value;
                var enumValue2 = snapshot2[enumConstant];

                // Check to see if the snapshot 1 and snapshot 2 value is equal.
                if (Object.Equals(enumValue1, enumValue2) == false)
                {
                    Console.WriteLine("{0}: '{1}' '{2}'", enumConstant, enumValue1, enumValue2);
                }
            }

            // This will pause the console window.
            Console.WriteLine();
            Console.WriteLine("Press enter to contine.");
            Console.ReadLine();
        }

        static void CaptureApplicationGlobalConstants(SolidEdgeFramework.Application application, Dictionary<SolidEdgeFramework.ApplicationGlobalConstants, object> dictionary)
        {
            // Get list of SolidEdgeFramework.ApplicationGlobalConstants names and values.
            var enumConstants = Enum.GetNames(typeof(SolidEdgeFramework.ApplicationGlobalConstants)).ToArray();
            var enumValues = Enum.GetValues(typeof(SolidEdgeFramework.ApplicationGlobalConstants)).Cast<SolidEdgeFramework.ApplicationGlobalConstants>().ToArray();

            // Populate the dictionary.
            for (int i = 0; i < enumValues.Length; i++)
            {
                var enumConstant = enumConstants[i];
                var enumValue = enumValues[i];
                object value = null;

                if (enumValue.Equals(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalOpenAsReadOnly3DFile)) continue;

                // We can safely ignore seApplicationGlobalSystemInfo. It's just noise for our purpose.
                if (enumConstant.Equals(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo)) continue;

                application.GetGlobalParameter(enumValue, ref value);
                dictionary.Add(enumValue, value);
            }
        }
    }
}
