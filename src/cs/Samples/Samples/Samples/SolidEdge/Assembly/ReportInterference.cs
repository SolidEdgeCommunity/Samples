using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Assembly
{
    /// <summary>
    /// Reports interference between all occurrences of the active assembly.
    /// </summary>
    class ReportInterference
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Occurrences occurrences = null;
            SolidEdgeAssembly.Occurrence occurrence = null;
            SolidEdgeAssembly.InterferenceStatusConstants interferenceStatus;
            SolidEdgeConstants.InterferenceComparisonConstants compare = SolidEdgeConstants.InterferenceComparisonConstants.seInterferenceComparisonSet1vsAllOther;
            SolidEdgeConstants.InterferenceReportConstants reportType = SolidEdgeConstants.InterferenceReportConstants.seInterferenceReportPartNames;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = ApplicationHelper.Connect(true, true);

                // Get a reference to the active assembly document.
                assemblyDocument = application.TryActiveDocumentAs<SolidEdgeAssembly.AssemblyDocument>();

                if (assemblyDocument != null)
                {
                    // Get a reference to the Occurrences collection.
                    occurrences = assemblyDocument.Occurrences;

                    for (int i = 1; i <= occurrences.Count; i++)
                    {
                        // Get a reference to the occurrence.
                        occurrence = occurrences.Item(i);

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

                                        // Use ReflectionHelper class to get the object type.
                                        SolidEdgeFramework.ObjectType objectType1 = ReflectionHelper.GetObjectType(obj1);
                                        SolidEdgeFramework.ObjectType objectType2 = ReflectionHelper.GetObjectType(obj2);

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
                    throw new System.Exception(Resources.NoActiveAssemblyDocument);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                OleMessageFilter.Unregister();
            }
        }
    }
}
