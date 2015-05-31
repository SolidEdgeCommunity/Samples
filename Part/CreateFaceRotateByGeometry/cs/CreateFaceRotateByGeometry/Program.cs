using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateFaceRotateByGeometry
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
            SolidEdgePart.FaceRotates faceRotates = null;
            SolidEdgePart.FaceRotate faceRotate = null;
            SolidEdgeGeometry.Faces faces = null;
            SolidEdgeGeometry.Face face = null;
            SolidEdgeGeometry.Edges edges = null;
            SolidEdgeGeometry.Edge edge = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            double angle = 0.5;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new part document.
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

                // Call helper method to create the actual geometry.
                PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005);

                // Get a reference to the models collection.
                models = partDocument.Models;
                model = models.Item(1);
                body = (SolidEdgeGeometry.Body)model.Body;
                faces = (SolidEdgeGeometry.Faces)body.Faces[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll];
                face = (SolidEdgeGeometry.Face)faces.Item(2);
                edges = (SolidEdgeGeometry.Edges)body.Edges[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll];
                edge = (SolidEdgeGeometry.Edge)edges.Item(5);

                faceRotates = model.FaceRotates;

                // Add face rotate.
                faceRotate = faceRotates.Add(
                    face,
                    SolidEdgePart.FaceRotateConstants.igFaceRotateByGeometry,
                    SolidEdgePart.FaceRotateConstants.igFaceRotateRecreateBlends,
                    null,
                    null,
                    edge,
                    SolidEdgePart.FaceRotateConstants.igFaceRotateAxisEnd,
                    angle);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new FaceRotate to ActiveSelectSet.
                selectSet.Add(faceRotate);

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView);
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
