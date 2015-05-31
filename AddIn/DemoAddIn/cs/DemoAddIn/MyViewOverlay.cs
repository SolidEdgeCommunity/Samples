using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoAddIn
{
    class MyViewOverlay : SolidEdgeCommunity.AddIn.ViewOverlay
    {
        private BoundingBoxInfo _boundingBoxInfo = default(BoundingBoxInfo);
        private bool _showOpenGlBoxes = false;
        private bool _showGdiPlus = false;

        public MyViewOverlay()
        {
            // Set the defaults.
            _boundingBoxInfo.LineColor = Color.Yellow;
            _boundingBoxInfo.LineWidth = 2f;
        }

        public override void BeginOpenGLMainDisplay(SolidEdgeSDK.IGL gl)
        {
            if (gl == null) return;

            DrawBoundingBox(gl);

            if (_showOpenGlBoxes)
            {
                float fSize = 0.025f;
                double[] matrix0 = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
                double[] matrix1 = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, fSize, 1 };
                double[] matrix2 = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, fSize, -fSize, 1 };
                double[] matrix3 = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, fSize, -fSize, 0, 1 };

                gl.glMatrixMode(SharpGL.OpenGL.GL_MODELVIEW);

                int mode = 0;
                int depth = 0;
                uint error;

                error = gl.glGetError();

                gl.glGetIntegerv(SharpGL.OpenGL.GL_MATRIX_MODE, ref mode);
                gl.glGetIntegerv(SharpGL.OpenGL.GL_MODELVIEW_STACK_DEPTH, ref depth);
                gl.glPushMatrix();
                gl.glGetIntegerv(SharpGL.OpenGL.GL_MODELVIEW_STACK_DEPTH, ref depth);

                gl.glLoadMatrixd(matrix0);
                gl.glColor3f(1, 0, 0);

                DrawCube(gl, fSize / 2.0f);

                gl.glPopMatrix();
                gl.glPushMatrix();

                {
                    gl.glMultMatrixd(matrix1);
                    gl.glColor3f(0, 1, 0);
                    DrawCube(gl, fSize / 2.0f);
                }

                {
                    gl.glMultMatrixd(matrix2);
                    gl.glColor3f(0, 0, 1);
                    DrawCube(gl, fSize / 2.0f);
                }

                {
                    gl.glMultMatrixd(matrix3);
                    gl.glColor4f(1, 1, 0, .25f);
                    DrawCube(gl, fSize / 2.0f);
                }

                gl.glPopMatrix();
            }
        }

        public override void EndDeviceContextMainDisplay(IntPtr hDC, ref double modelToDC, ref int rect)
        {
            if (_showGdiPlus)
            {
                //Demonstrate using GDI+ to write text on the device context (DC).
                using (Graphics graphics = Graphics.FromHdc(hDC))
                {
                    Point point = new Point(0, 0);

                    using (Font font = SystemFonts.DialogFont)
                    {
                        Color color = Color.Yellow;
                        string lastUpdate = DateTime.Now.ToString();

                        lastUpdate = String.Format("Last update: {0}", lastUpdate);

                        TextRenderer.DrawText(graphics, lastUpdate, font, point, color, Color.Black);
                        Size size = TextRenderer.MeasureText(lastUpdate, font);

                        point.Offset(0, size.Height);
                    }

                    using (var pen = new Pen(Color.Red, 2))
                    {

                        var clipBounds = graphics.VisibleClipBounds;

                        //Draw a line
                        //graphics.DrawLine(pen, 10, 5, 110, 15);
                        graphics.DrawLine(pen, this.Window.Left, this.Window.Top, this.Window.Width, this.Window.Height);
                    }

                    //Draw an ellipse
                    graphics.DrawEllipse(Pens.Blue, 10, 20, 110, 45);

                    //Draw a rectangle
                    graphics.DrawRectangle(Pens.Green, 10, 70, 110, 45);

                    //Fill an ellipse
                    graphics.FillEllipse(Brushes.Blue, 130, 20, 110, 45);

                    //Fill a rectangle
                    graphics.FillRectangle(Brushes.Green, 130, 70, 110, 45);

                }
            }
        }

        private void DrawBoundingBox(SolidEdgeSDK.IGL gl)
        {
            if (_boundingBoxInfo.Visible == false) return;

            if (gl == null) return;

            Vector3d min = new Vector3d();
            Vector3d max = new Vector3d();

            this.View.GetModelRange(out min.X, out min.Y, out min.Z, out max.X, out max.Y, out max.Z);

            gl.glColor3i(_boundingBoxInfo.LineColor.R, _boundingBoxInfo.LineColor.G, _boundingBoxInfo.LineColor.B);
            gl.glLineWidth(_boundingBoxInfo.LineWidth);
            gl.glHint(SharpGL.OpenGL.GL_LINE_SMOOTH_HINT, SharpGL.OpenGL.GL_NICEST);

            {
                gl.glBegin(SharpGL.OpenGL.GL_LINE_LOOP);

                gl.glVertex3d(min.X, min.Y, max.Z);
                gl.glVertex3d(max.X, min.Y, max.Z);
                gl.glVertex3d(max.X, max.Y, max.Z);
                gl.glVertex3d(min.X, max.Y, max.Z);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_LINE_LOOP);

                gl.glVertex3d(min.X, min.Y, min.Z);
                gl.glVertex3d(max.X, min.Y, min.Z);
                gl.glVertex3d(max.X, max.Y, min.Z);
                gl.glVertex3d(min.X, max.Y, min.Z);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_LINES);

                gl.glVertex3d(min.X, min.Y, min.Z);
                gl.glVertex3d(min.X, min.Y, max.Z);

                gl.glVertex3d(max.X, max.Y, min.Z);
                gl.glVertex3d(max.X, max.Y, max.Z);

                gl.glVertex3d(min.X, max.Y, min.Z);
                gl.glVertex3d(min.X, max.Y, max.Z);

                gl.glVertex3d(max.X, min.Y, min.Z);
                gl.glVertex3d(max.X, min.Y, max.Z);

                gl.glEnd();
            }

            {
                gl.glColor3f(1, 0, 0);
                gl.glBegin(SharpGL.OpenGL.GL_LINES);

                // Diagonal line between min & max points.
                gl.glVertex3d(min.X, min.Y, min.Z);
                gl.glVertex3d(max.X, max.Y, max.Z);

                gl.glEnd();
            }
        }

        private void DrawCube(SolidEdgeSDK.IGL gl, float fSize)
        {
            float[][] p0 = new float[][] 
            {
                new float[] { 0.0f, 0.0f, 0.0f },
                new float[] { 0.0f, fSize, 0.0f },
                new float[] { fSize, 0.0f, 0.0f },
                new float[] { fSize, fSize, 0.0f }
            };

            float[][] p1 = new float[][] 
            {
                new float[] { 0.0f, 0.0f, fSize },
                new float[] { 0.0f, fSize, fSize },
                new float[] { fSize, 0.0f, fSize },
                new float[] { fSize, fSize, fSize }
            };

            float[][] p2 = new float[][] 
            {
                new float[] { 0.0f, 0.0f, 0.0f },
                new float[] { 0.0f, 0.0f, fSize },
                new float[] { 0.0f, fSize, 0.0f },
                new float[] { 0.0f, fSize, fSize }
            };

            float[][] p3 = new float[][] 
            {
                new float[] { fSize, 0.0f, 0.0f },
                new float[] { fSize, 0.0f, fSize },
                new float[] { fSize, fSize, 0.0f },
                new float[] { fSize, fSize, fSize }
            };

            float[][] p4 = new float[][] 
            {
                new float[] { 0.0f, 0.0f, 0.0f },
                new float[] { 0.0f, 0.0f, fSize },
                new float[] { fSize, 0.0f, 0.0f },
                new float[] { fSize, 0.0f, fSize }
            };

            float[][] p5 = new float[][] 
            {
                new float[] { 0.0f, fSize, 0.0f },
                new float[] { 0.0f, fSize, fSize },
                new float[] { fSize, fSize, 0.0f },
                new float[] { fSize, fSize, fSize }
            };

            // Normals
            float[][] n0 = new float[][] 
            {
                new float[] { 0.0f, 0.0f, -1.0f },
                new float[] { 0.0f, 0.0f, 1.0f},
                new float[] { 0.0f, -1.0f, 0.0f},
                new float[] { 0.0f, 1.0f, 0.0f},
                new float[] { 0.0f, 0.0f, 1.0f},
                new float[] { 0.0f, 1.0f, 0.0f},
                new float[] { 0.0f, -1.0f, 0.0f }
            };

            {
                gl.glBegin(SharpGL.OpenGL.GL_TRIANGLES);
                gl.glNormal3fv(n0[0]);

                gl.glEdgeFlag((byte)SharpGL.OpenGL.GL_TRUE);
                gl.glVertex3fv(p0[0]);

                gl.glEdgeFlag(0);
                gl.glVertex3fv(p0[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p0[2]);

                gl.glNormal3fv(n0[1]);

                gl.glEdgeFlag(0);
                gl.glVertex3fv(p0[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p0[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p0[3]);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP);

                gl.glNormal3fv(n0[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p1[0]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p1[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p1[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p1[3]);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP);
                gl.glNormal3fv(n0[3]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p2[0]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p2[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p2[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p2[3]);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP);
                gl.glNormal3fv(n0[4]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p3[0]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p3[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p3[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p3[3]);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP);

                gl.glNormal3fv(n0[5]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p4[0]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p4[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p4[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p4[3]);

                gl.glEnd();
            }

            {
                gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP);
                gl.glNormal3fv(n0[6]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p5[0]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p5[1]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p5[2]);

                gl.glEdgeFlag(1);
                gl.glVertex3fv(p5[3]);

                gl.glEnd();
            }
        }

        public bool ShowBoundingBox
        {
            get { return _boundingBoxInfo.Visible; }
            set
            {
                _boundingBoxInfo.Visible = value;

                // Force the view to update.
                this.View.Update();
            }
        }

        public bool ShowOpenGlBoxes
        {
            get { return _showOpenGlBoxes; }
            set
            {
                _showOpenGlBoxes = value;

                // Force the view to update.
                this.View.Update();
            }
        }

        public bool ShowGDIPlus
        {
            get { return _showGdiPlus; }
            set
            {
                _showGdiPlus = value;

                // Force the view to update.
                this.View.Update();
            }
        }
    }
}
