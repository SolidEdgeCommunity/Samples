Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Friend Class MyViewOverlay
    Inherits SolidEdgeCommunity.AddIn.ViewOverlay

    Private _boundingBoxInfo As BoundingBoxInfo = Nothing
    Private _showOpenGlBoxes As Boolean = False
    Private _showGdiPlus As Boolean = False

    Public Sub New()
        ' Set the defaults.
        _boundingBoxInfo.LineColor = Color.Yellow
        _boundingBoxInfo.LineWidth = 2F
    End Sub

    Public Overrides Sub BeginOpenGLMainDisplay(ByVal gl As SolidEdgeSDK.IGL)
        If gl Is Nothing Then
            Return
        End If

        DrawBoundingBox(gl)

        If _showOpenGlBoxes Then
            Dim fSize As Single = 0.025F
            Dim matrix0() As Double = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 }
            Dim matrix1() As Double = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, fSize, 1 }
            Dim matrix2() As Double = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, fSize, -fSize, 1 }
            Dim matrix3() As Double = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, fSize, -fSize, 0, 1 }

            gl.glMatrixMode(SharpGL.OpenGL.GL_MODELVIEW)

            Dim mode As Integer = 0
            Dim depth As Integer = 0
            Dim [error] As UInteger

            [error] = gl.glGetError()

            gl.glGetIntegerv(SharpGL.OpenGL.GL_MATRIX_MODE, mode)
            gl.glGetIntegerv(SharpGL.OpenGL.GL_MODELVIEW_STACK_DEPTH, depth)
            gl.glPushMatrix()
            gl.glGetIntegerv(SharpGL.OpenGL.GL_MODELVIEW_STACK_DEPTH, depth)

            gl.glLoadMatrixd(matrix0)
            gl.glColor3f(1, 0, 0)

            DrawCube(gl, fSize / 2.0F)

            gl.glPopMatrix()
            gl.glPushMatrix()

            If True Then
                gl.glMultMatrixd(matrix1)
                gl.glColor3f(0, 1, 0)
                DrawCube(gl, fSize / 2.0F)
            End If

            If True Then
                gl.glMultMatrixd(matrix2)
                gl.glColor3f(0, 0, 1)
                DrawCube(gl, fSize / 2.0F)
            End If

            If True Then
                gl.glMultMatrixd(matrix3)
                gl.glColor4f(1, 1, 0,.25F)
                DrawCube(gl, fSize / 2.0F)
            End If

            gl.glPopMatrix()
        End If
    End Sub

    Public Overrides Sub EndDeviceContextMainDisplay(ByVal hDC As IntPtr, ByRef modelToDC As Double, ByRef rect As Integer)
        If _showGdiPlus Then
            'Demonstrate using GDI+ to write text on the device context (DC).
            Using graphics As Graphics = System.Drawing.Graphics.FromHdc(hDC)
                Dim point As New Point(0, 0)

                Using font As Font = SystemFonts.DialogFont
                    Dim color As Color = System.Drawing.Color.Yellow
                    Dim lastUpdate As String = Date.Now.ToString()

                    lastUpdate = String.Format("Last update: {0}", lastUpdate)

                    TextRenderer.DrawText(graphics, lastUpdate, font, point, color, System.Drawing.Color.Black)
                    Dim size As Size = TextRenderer.MeasureText(lastUpdate, font)

                    point.Offset(0, size.Height)
                End Using

                Using pen = New Pen(Color.Red, 2)

                    Dim clipBounds = graphics.VisibleClipBounds

                    'Draw a line
                    'graphics.DrawLine(pen, 10, 5, 110, 15);
                    graphics.DrawLine(pen, Me.Window.Left, Me.Window.Top, Me.Window.Width, Me.Window.Height)
                End Using

                'Draw an ellipse
                graphics.DrawEllipse(Pens.Blue, 10, 20, 110, 45)

                'Draw a rectangle
                graphics.DrawRectangle(Pens.Green, 10, 70, 110, 45)

                'Fill an ellipse
                graphics.FillEllipse(Brushes.Blue, 130, 20, 110, 45)

                'Fill a rectangle
                graphics.FillRectangle(Brushes.Green, 130, 70, 110, 45)

            End Using
        End If
    End Sub

    Private Sub DrawBoundingBox(ByVal gl As SolidEdgeSDK.IGL)
        If _boundingBoxInfo.Visible = False Then
            Return
        End If

        If gl Is Nothing Then
            Return
        End If

        Dim min As New Vector3d()
        Dim max As New Vector3d()

        Me.View.GetModelRange(min.X, min.Y, min.Z, max.X, max.Y, max.Z)

        gl.glColor3i(_boundingBoxInfo.LineColor.R, _boundingBoxInfo.LineColor.G, _boundingBoxInfo.LineColor.B)
        gl.glLineWidth(_boundingBoxInfo.LineWidth)
        gl.glHint(SharpGL.OpenGL.GL_LINE_SMOOTH_HINT, SharpGL.OpenGL.GL_NICEST)

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_LINE_LOOP)

            gl.glVertex3d(min.X, min.Y, max.Z)
            gl.glVertex3d(max.X, min.Y, max.Z)
            gl.glVertex3d(max.X, max.Y, max.Z)
            gl.glVertex3d(min.X, max.Y, max.Z)

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_LINE_LOOP)

            gl.glVertex3d(min.X, min.Y, min.Z)
            gl.glVertex3d(max.X, min.Y, min.Z)
            gl.glVertex3d(max.X, max.Y, min.Z)
            gl.glVertex3d(min.X, max.Y, min.Z)

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_LINES)

            gl.glVertex3d(min.X, min.Y, min.Z)
            gl.glVertex3d(min.X, min.Y, max.Z)

            gl.glVertex3d(max.X, max.Y, min.Z)
            gl.glVertex3d(max.X, max.Y, max.Z)

            gl.glVertex3d(min.X, max.Y, min.Z)
            gl.glVertex3d(min.X, max.Y, max.Z)

            gl.glVertex3d(max.X, min.Y, min.Z)
            gl.glVertex3d(max.X, min.Y, max.Z)

            gl.glEnd()
        End If

        If True Then
            gl.glColor3f(1, 0, 0)
            gl.glBegin(SharpGL.OpenGL.GL_LINES)

            ' Diagonal line between min & max points.
            gl.glVertex3d(min.X, min.Y, min.Z)
            gl.glVertex3d(max.X, max.Y, max.Z)

            gl.glEnd()
        End If
    End Sub

    Private Sub DrawCube(ByVal gl As SolidEdgeSDK.IGL, ByVal fSize As Single)
        Dim p0()() As Single = { _
            New Single() { 0.0F, 0.0F, 0.0F }, _
            New Single() { 0.0F, fSize, 0.0F }, _
            New Single() { fSize, 0.0F, 0.0F }, _
            New Single() { fSize, fSize, 0.0F } _
        }

        Dim p1()() As Single = { _
            New Single() { 0.0F, 0.0F, fSize }, _
            New Single() { 0.0F, fSize, fSize }, _
            New Single() { fSize, 0.0F, fSize }, _
            New Single() { fSize, fSize, fSize } _
        }

        Dim p2()() As Single = { _
            New Single() { 0.0F, 0.0F, 0.0F }, _
            New Single() { 0.0F, 0.0F, fSize }, _
            New Single() { 0.0F, fSize, 0.0F }, _
            New Single() { 0.0F, fSize, fSize } _
        }

        Dim p3()() As Single = { _
            New Single() { fSize, 0.0F, 0.0F }, _
            New Single() { fSize, 0.0F, fSize }, _
            New Single() { fSize, fSize, 0.0F }, _
            New Single() { fSize, fSize, fSize } _
        }

        Dim p4()() As Single = { _
            New Single() { 0.0F, 0.0F, 0.0F }, _
            New Single() { 0.0F, 0.0F, fSize }, _
            New Single() { fSize, 0.0F, 0.0F }, _
            New Single() { fSize, 0.0F, fSize } _
        }

        Dim p5()() As Single = { _
            New Single() { 0.0F, fSize, 0.0F }, _
            New Single() { 0.0F, fSize, fSize }, _
            New Single() { fSize, fSize, 0.0F }, _
            New Single() { fSize, fSize, fSize } _
        }

        ' Normals
        Dim n0()() As Single = { _
            New Single() { 0.0F, 0.0F, -1.0F }, _
            New Single() { 0.0F, 0.0F, 1.0F}, _
            New Single() { 0.0F, -1.0F, 0.0F}, _
            New Single() { 0.0F, 1.0F, 0.0F}, _
            New Single() { 0.0F, 0.0F, 1.0F}, _
            New Single() { 0.0F, 1.0F, 0.0F}, _
            New Single() { 0.0F, -1.0F, 0.0F } _
        }

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_TRIANGLES)
            gl.glNormal3fv(n0(0))

            gl.glEdgeFlag(CByte(SharpGL.OpenGL.GL_TRUE))
            gl.glVertex3fv(p0(0))

            gl.glEdgeFlag(0)
            gl.glVertex3fv(p0(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p0(2))

            gl.glNormal3fv(n0(1))

            gl.glEdgeFlag(0)
            gl.glVertex3fv(p0(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p0(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p0(3))

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP)

            gl.glNormal3fv(n0(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p1(0))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p1(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p1(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p1(3))

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP)
            gl.glNormal3fv(n0(3))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p2(0))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p2(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p2(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p2(3))

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP)
            gl.glNormal3fv(n0(4))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p3(0))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p3(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p3(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p3(3))

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP)

            gl.glNormal3fv(n0(5))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p4(0))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p4(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p4(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p4(3))

            gl.glEnd()
        End If

        If True Then
            gl.glBegin(SharpGL.OpenGL.GL_TRIANGLE_STRIP)
            gl.glNormal3fv(n0(6))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p5(0))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p5(1))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p5(2))

            gl.glEdgeFlag(1)
            gl.glVertex3fv(p5(3))

            gl.glEnd()
        End If
    End Sub

    Public Property ShowBoundingBox() As Boolean
        Get
            Return _boundingBoxInfo.Visible
        End Get
        Set(ByVal value As Boolean)
            _boundingBoxInfo.Visible = value

            ' Force the view to update.
            Me.View.Update()
        End Set
    End Property

    Public Property ShowOpenGlBoxes() As Boolean
        Get
            Return _showOpenGlBoxes
        End Get
        Set(ByVal value As Boolean)
            _showOpenGlBoxes = value

            ' Force the view to update.
            Me.View.Update()
        End Set
    End Property

    Public Property ShowGDIPlus() As Boolean
        Get
            Return _showGdiPlus
        End Get
        Set(ByVal value As Boolean)
            _showGdiPlus = value

            ' Force the view to update.
            Me.View.Update()
        End Set
    End Property
End Class
