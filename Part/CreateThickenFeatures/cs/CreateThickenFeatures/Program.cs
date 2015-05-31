using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateThickenFeatures
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            SolidEdgeGeometry.Body body = null;
            SolidEdgeGeometry.Faces faces = null;
            SolidEdgeGeometry.Face face = null;
            SolidEdgePart.Thickens thickens = null;
            SolidEdgePart.Thicken thicken = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Add a new part document.
                partDocument = documents.AddPartDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Get a reference to the RefPlanes collection.
                refPlanes = partDocument.RefPlanes;

                // Get a reference to a RefPlane.
                refPlane = refPlanes.GetFrontPlane();

                List<double[]> linesArray = new List<double[]>();
                linesArray.Add(new double[] { 0, 0, 0.08, 0 });
                linesArray.Add(new double[] { 0.08, 0, 0.08, 0.06 });
                linesArray.Add(new double[] { 0.08, 0.06, 0.064, 0.06 });
                linesArray.Add(new double[] { 0.064, 0.06, 0.064, 0.02 });
                linesArray.Add(new double[] { 0.064, 0.02, 0.048, 0.02 });
                linesArray.Add(new double[] { 0.048, 0.02, 0.048, 0.06 });
                linesArray.Add(new double[] { 0.048, 0.06, 0.032, 0.06 });
                linesArray.Add(new double[] { 0.032, 0.06, 0.032, 0.02 });
                linesArray.Add(new double[] { 0.032, 0.02, 0.016, 0.02 });
                linesArray.Add(new double[] { 0.016, 0.02, 0.016, 0.06 });
                linesArray.Add(new double[] { 0.016, 0.06, 0, 0.06 });
                linesArray.Add(new double[] { 0, 0.06, 0, 0 });

                // Get a reference to the models collection.
                models = partDocument.Models;

                // Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005);

                body = (SolidEdgeGeometry.Body)model.Body;

                faces = (SolidEdgeGeometry.Faces)body.Faces[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll];

                thickens = model.Thickens;

                Type type = typeof(SolidEdgePart.FeaturePropertyConstants);
                var fields = type.GetFields();

                for (int i = 1; i <= faces.Count; i++)
                {
                    model = models.Item(1);
                    body = (SolidEdgeGeometry.Body)model.Body;
                    faces = (SolidEdgeGeometry.Faces)body.Faces[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll];
                    thickens = model.Thickens;

                    face = faces.Item(i) as SolidEdgeGeometry.Face;

                    Array facesArray = Array.CreateInstance(typeof(SolidEdgeGeometry.Face), 1);

                    facesArray.SetValue(face, 0);

                    foreach (var field in fields)
                    {
                        if (field.IsSpecialName) continue;
                        SolidEdgePart.FeaturePropertyConstants side = (SolidEdgePart.FeaturePropertyConstants)field.GetRawConstantValue();

                        try
                        {
                            thicken = thickens.Add(side, 0.0254, 1, ref facesArray);

                            thicken.Delete();
                            break;
                        }
                        catch
                        {
                        }
                    }
                    //thicken = thickens.Add(SolidEdgePart.FeaturePropertyConstants.igReverseNormalSideDummy, 0.0254, 1, ref facesArray);

                    //selectSet = application.ActiveSelectSet;
                    //selectSet.RemoveAll();
                    //selectSet.Add(faces.Item(1));
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
