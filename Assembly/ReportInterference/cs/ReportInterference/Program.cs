using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReportInterference
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Occurrences occurrences = null;
            SolidEdgeAssembly.InterferenceStatusConstants interferenceStatus;
            SolidEdgeConstants.InterferenceComparisonConstants compare = SolidEdgeConstants.InterferenceComparisonConstants.seInterferenceComparisonSet1vsAllOther;
            SolidEdgeConstants.InterferenceReportConstants reportType = SolidEdgeConstants.InterferenceReportConstants.seInterferenceReportPartNames;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active assembly document.
                assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (assemblyDocument != null)
                {
                    // Get a reference to the Occurrences collection.
                    occurrences = assemblyDocument.Occurrences;

                    foreach (var occurrence in occurrences.OfType<SolidEdgeAssembly.Occurrence>())
                    {
                        Array set1 = Array.CreateInstance(occurrence.GetType(), 1);
                        object numInterferences = 0;
                        object retSet1 = Array.CreateInstance(typeof(SolidEdgeAssembly.Occurrence), 0);
                        object retSet2 = Array.CreateInstance(typeof(SolidEdgeAssembly.Occurrence), 0);
                        object confirmedInterference = null;
                        object interferenceOccurrence = null;

                        set1.SetValue(occurrence, 0);

                        // Check interference.
                        assemblyDocument.CheckInterference(
                            NumElementsSet1: set1.Length,
                            Set1: ref set1,
                            Status: out interferenceStatus,
                            ComparisonMethod: compare,
                            NumElementsSet2: 0,
                            Set2: Missing.Value,
                            AddInterferenceAsOccurrence: false,
                            ReportFilename: Missing.Value,
                            ReportType: reportType,
                            NumInterferences: out numInterferences,
                            InterferingPartsSet1: ref retSet1,
                            InterferingPartsOtherSet: ref retSet2,
                            ConfirmedInterference: ref confirmedInterference,
                            InterferenceOccurrence: out interferenceOccurrence,
                            IgnoreThreadInterferences: Missing.Value
                            );

                        // Process status.
                        switch (interferenceStatus)
                        {
                            case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusNoInterference:
                                break;
                            case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusConfirmedAndProbableInterference:
                            case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusConfirmedInterference:
                            case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusIncompleteAnalysis:
                            case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusProbableInterference:
                                if (retSet2 != null)
                                {
                                    for (int j = 0; j < (int)numInterferences; j++)
                                    {
                                        object obj1 = ((Array)retSet1).GetValue(j);
                                        object obj2 = ((Array)retSet2).GetValue(j);

                                        // Use helper class to get the object type.
                                        var objectType1 = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgeFramework.ObjectType>(obj1, "Type", (SolidEdgeFramework.ObjectType)0);
                                        var objectType2 = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgeFramework.ObjectType>(obj2, "Type", (SolidEdgeFramework.ObjectType)0);

                                        SolidEdgeFramework.Reference reference1 = null;
                                        SolidEdgeFramework.Reference reference2 = null;
                                        SolidEdgeAssembly.Occurrence occurrence1 = null;
                                        SolidEdgeAssembly.Occurrence occurrence2 = null;

                                        switch (objectType1)
                                        {
                                            case SolidEdgeFramework.ObjectType.igReference:
                                                reference1 = (SolidEdgeFramework.Reference)obj1;
                                                break;
                                            case SolidEdgeFramework.ObjectType.igPart:
                                            case SolidEdgeFramework.ObjectType.igOccurrence:
                                                occurrence1 = (SolidEdgeAssembly.Occurrence)obj1;
                                                break;
                                        }

                                        switch (objectType2)
                                        {
                                            case SolidEdgeFramework.ObjectType.igReference:
                                                reference2 = (SolidEdgeFramework.Reference)obj2;
                                                break;
                                            case SolidEdgeFramework.ObjectType.igPart:
                                            case SolidEdgeFramework.ObjectType.igOccurrence:
                                                occurrence2 = (SolidEdgeAssembly.Occurrence)obj2;
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                    }
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
