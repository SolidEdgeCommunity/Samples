using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Assembly
{
    /// <summary>
    /// Reports all 3D relationships of the active assembly.
    /// </summary>
    class ReportRelations3d
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Relations3d relations3d = null;
            SolidEdgeAssembly.GroundRelation3d groundRelation3d = null;
            SolidEdgeAssembly.AxialRelation3d axialRelation3d = null;
            SolidEdgeAssembly.PlanarRelation3d planarRelation3d = null;
            SolidEdgeAssembly.Occurrence occurrence1 = null;
            SolidEdgeAssembly.Occurrence occurrence2 = null;
            SolidEdgeFramework.ObjectType relationObjectType;
            SolidEdgeAssembly.Relation3dDetailedStatusConstants detailedStatus;
            SolidEdgeAssembly.Relation3dStatusConstants status;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the active document.
                assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                // Get a reference to the Relations3d collection.
                relations3d = assemblyDocument.Relations3d;

                for (int i = 1; i <= relations3d.Count; i++)
                {
                    object relation3d = relations3d.Item(i);

                    try
                    {
                        // Use ReflectionHelper class to get the object type.
                        relationObjectType = ReflectionHelper.GetObjectType(relation3d);

                        // Reset statuses.
                        detailedStatus = (SolidEdgeAssembly.Relation3dDetailedStatusConstants)0;
                        status = (SolidEdgeAssembly.Relation3dStatusConstants)0;

                        // Handle specific object type. There are other possible relation types...
                        switch (relationObjectType)
                        {
                            case SolidEdgeFramework.ObjectType.igGroundRelation3d:
                                // Cast relation3d object to GroundRelation3d type.
                                groundRelation3d = (SolidEdgeAssembly.GroundRelation3d)relation3d;

                                // Get a reference to the grounded occurrence.
                                occurrence1 = groundRelation3d.Occurrence;

                                // Get the detailed status.
                                detailedStatus = groundRelation3d.DetailedStatus;

                                // Get the status.
                                status = groundRelation3d.Status;

                                break;
                            case SolidEdgeFramework.ObjectType.igAxialRelation3d:
                                // Cast relation3d object to AxialRelation3d type.
                                axialRelation3d = (SolidEdgeAssembly.AxialRelation3d)relation3d;

                                // Get a reference to the related occurrences.
                                occurrence1 = axialRelation3d.Occurrence1;
                                occurrence2 = axialRelation3d.Occurrence2;

                                // Get the detailed status.
                                detailedStatus = axialRelation3d.DetailedStatus;

                                // Get the status.
                                status = axialRelation3d.Status;

                                break;
                            case SolidEdgeFramework.ObjectType.igPlanarRelation3d:
                                // Cast relation3d object to PlanarRelation3d type.
                                planarRelation3d = (SolidEdgeAssembly.PlanarRelation3d)relation3d;

                                // Get a reference to the related occurrences.
                                occurrence1 = planarRelation3d.Occurrence1;
                                occurrence2 = planarRelation3d.Occurrence2;

                                // Get the detailed status.
                                detailedStatus = planarRelation3d.DetailedStatus;

                                // Get the status.
                                status = planarRelation3d.Status;
                                break;
                            default:
                                break;
                        }

                        // Analyze the detailed status.
                        switch (detailedStatus)
                        {
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusBetweenFixed:
                                break;
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusBetweenSetMembers:
                                break;
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusMissingGeometry:
                                break;
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusSolved:
                                break;
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusSuppressed:
                                break;
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusUnknown:
                                break;
                            case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusUnsatisfied:
                                break;
                        }

                        // Analyze the status.
                        switch (status)
                        {
                            case SolidEdgeAssembly.Relation3dStatusConstants.igRelation3dStatusSolved:
                                break;
                            case SolidEdgeAssembly.Relation3dStatusConstants.igRelation3dStatusUnsolved:
                                break;
                        }
                    }
                    catch
                    {
                    }
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
