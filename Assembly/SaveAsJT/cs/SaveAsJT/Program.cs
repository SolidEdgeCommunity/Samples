using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveAsJT
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument document = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (document != null)
                {
                    // Note: Some of the parameters are obvious by their name but we need to work on getting better descriptions for some.
                    var NewName = String.Empty;
                    var Include_PreciseGeom = 0;
                    var Prod_Structure_Option = 1;
                    var Export_PMI = 0;
                    var Export_CoordinateSystem = 0;
                    var Export_3DBodies = 0;
                    var NumberofLODs = 1;
                    var JTFileUnit = 0;
                    var Write_Which_Files = 1;
                    var Use_Simplified_TopAsm = 0;
                    var Use_Simplified_SubAsm = 0;
                    var Use_Simplified_Part = 0;
                    var EnableDefaultOutputPath = 0;
                    var IncludeSEProperties = 0;
                    var Export_VisiblePartsOnly = 0;
                    var Export_VisibleConstructionsOnly = 0;
                    var RemoveUnsafeCharacters = 0;
                    var ExportSEPartFileAsSingleJTFile = 0;

                    if (document == null)
                    {
                        throw new ArgumentNullException("document");
                    }

                    switch (document.Type)
                    {
                        case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                        case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                        case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument:
                        case SolidEdgeFramework.DocumentTypeConstants.igWeldmentAssemblyDocument:
                        case SolidEdgeFramework.DocumentTypeConstants.igWeldmentDocument:
                            NewName = System.IO.Path.ChangeExtension(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), document.Name), ".jt");
                            document.SaveAsJT(
                                            NewName,
                                            Include_PreciseGeom,
                                            Prod_Structure_Option,
                                            Export_PMI,
                                            Export_CoordinateSystem,
                                            Export_3DBodies,
                                            NumberofLODs,
                                            JTFileUnit,
                                            Write_Which_Files,
                                            Use_Simplified_TopAsm,
                                            Use_Simplified_SubAsm,
                                            Use_Simplified_Part,
                                            EnableDefaultOutputPath,
                                            IncludeSEProperties,
                                            Export_VisiblePartsOnly,
                                            Export_VisibleConstructionsOnly,
                                            RemoveUnsafeCharacters,
                                            ExportSEPartFileAsSingleJTFile);
                            break;
                        default:
                            throw new System.Exception(String.Format("'{0}' cannot be converted to JT.", document.Type));
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
