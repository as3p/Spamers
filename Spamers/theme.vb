'' Warna Dasar
'  Base   : 251, 251, 251
'  Border : 226, 226, 226	
'  Font   : 124, 133, 142	
'  Theme  : 41, 133, 211

'Jenis Font : "Segoe UI", 10, FontStyle.Bold


Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports System.Drawing


#Region " Helper Methods "

Public Module HelperMethods
    'Tambahan Dark UI
    Public Sub FillWithInnerRectangle(ByVal G As Graphics, ByVal CenterColor As Color, ByVal SurroundColor As Color, ByVal P As Point, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                         Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                         Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        Using PGB As New PathGradientBrush(RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
            With PGB
                .CenterColor = CenterColor
                .SurroundColors = New Color() {SurroundColor}
                .FocusScales = P
                With G
                    Dim GP As New GraphicsPath With {.FillMode = FillMode.Winding}
                    GP.AddRectangle(Rect)
                    .FillPath(PGB, GP)
                    GP.Dispose()
                End With
            End With
        End Using
    End Sub

    Public Sub FillWithInnerEllipse(ByVal G As Graphics, ByVal CenterColor As Color, ByVal SurroundColor As Color, ByVal P As Point, ByVal Rect As Rectangle)
        Dim GP As New GraphicsPath With {.FillMode = FillMode.Winding}
        GP.AddEllipse(Rect)
        Using PGB As New PathGradientBrush(GP)
            With PGB
                .CenterColor = CenterColor
                .SurroundColors = New Color() {SurroundColor}
                .FocusScales = P
                With G
                    .FillPath(PGB, GP)
                    GP.Dispose()
                End With
            End With
        End Using
    End Sub

    Public Sub FillWithInnerRoundedPath(ByVal G As Graphics, ByVal CenterColor As Color, ByVal SurroundColor As Color, ByVal P As Point, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                     Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                     Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        Using PGB As New PathGradientBrush(RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
            With PGB
                .CenterColor = CenterColor
                .SurroundColors = New Color() {SurroundColor}
                .FocusScales = P
                With G
                    .FillPath(PGB, RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
                End With
            End With
        End Using
    End Sub

    Public Sub FillStrokedRectangle(ByVal G As Graphics, ByVal Rect As Rectangle, ByVal RectColor As Color, ByVal StrokeColor As Color, Optional ByVal StrokeSize As Integer = 1)
        Using B As New SolidBrush(RectColor), S As New Pen(StrokeColor, StrokeSize)
            G.FillRectangle(B, Rect)
            G.DrawRectangle(S, Rect)
        End Using
    End Sub

    Public Sub FillRoundedStrokedRectangle(ByVal G As Graphics, ByVal Rect As Rectangle, ByVal RectColor As Color, ByVal StrokeColor As Color, Optional ByVal StrokeSize As Integer = 1, Optional ByVal curve As Integer = 1, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        Using B As New SolidBrush(RectColor)
            FillRoundedPath(G, B, Rect, curve, TopLeft, TopRight, BottomLeft, BottomRight)
            DrawRoundedPath(G, StrokeColor, StrokeSize, Rect, curve, TopLeft, TopRight, BottomLeft, BottomRight)
        End Using
    End Sub

    Public Sub DrawImageWithColor(ByVal G As Graphics, ByVal R As Rectangle, ByVal _Image As Image, ByVal C As Color)
        Dim ptsArray As Single()() = {
            New Single() {Convert.ToSingle(C.R / 255), 0, 0, 0, 0}, _
            New Single() {0, Convert.ToSingle(C.G / 255), 0, 0, 0}, _
            New Single() {0, 0, Convert.ToSingle(C.B / 255), 0, 0}, _
            New Single() {0, 0, 0, Convert.ToSingle(C.A / 255), 0}, _
            New Single() {Convert.ToSingle(C.R / 255), Convert.ToSingle(C.G / 255), Convert.ToSingle(C.B / 255), 0.0F, Convert.ToSingle(C.A / 255)}}
        Dim imgAttribs As New Imaging.ImageAttributes
        imgAttribs.SetColorMatrix(New Imaging.ColorMatrix(ptsArray), Imaging.ColorMatrixFlag.Default, Imaging.ColorAdjustType.Default)
        G.DrawImage(_Image, R, 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel, imgAttribs)
    End Sub

    Public Sub DrawImageWithColor(ByVal G As Graphics, ByVal R As Rectangle, ByVal _Image As String, ByVal C As Color)
        Dim IM As Image = ImageFromBase64(_Image)
        Dim ptsArray As Single()() = {
            New Single() {Convert.ToSingle(C.R / 255), 0, 0, 0, 0}, _
            New Single() {0, Convert.ToSingle(C.G / 255), 0, 0, 0}, _
            New Single() {0, 0, Convert.ToSingle(C.B / 255), 0, 0}, _
            New Single() {0, 0, 0, Convert.ToSingle(C.A / 255), 0}, _
            New Single() {Convert.ToSingle(C.R / 255), Convert.ToSingle(C.G / 255), Convert.ToSingle(C.B / 255), 0.0F, Convert.ToSingle(C.A / 255)}}
        Dim imgAttribs As New Imaging.ImageAttributes
        imgAttribs.SetColorMatrix(New Imaging.ColorMatrix(ptsArray), Imaging.ColorMatrixFlag.Default, Imaging.ColorAdjustType.Default)
        G.DrawImage(IM, R, 0, 0, IM.Width, IM.Height, GraphicsUnit.Pixel, imgAttribs)
    End Sub

    Public Function Triangle(ByVal P1 As Point, ByVal P2 As Point, ByVal P3 As Point) As Point()
        Return New Point() {P1, P2, P3}
    End Function

    Public Function GlowBrush(ByVal CenterColor As Color, ByVal SurroundColor As Color, ByVal P As Point, ByVal Rect As Rectangle) As PathGradientBrush
        Dim GP As New GraphicsPath With {.FillMode = FillMode.Winding}
        GP.AddRectangle(Rect)
        Return New PathGradientBrush(GP) With {.CenterColor = CenterColor, .SurroundColors = New Color() {SurroundColor}, .FocusScales = P}
        GP.Dispose()
    End Function


    Public Function SolidBrushRGBColor(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, Optional ByVal A As Integer = 0) As SolidBrush
        Return New SolidBrush(Color.FromArgb(A, R, G, B))
    End Function

    Public Function PenRGBColor(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, ByVal A As Integer, ByVal Size As Single) As Pen
        Return New Pen(Color.FromArgb(A, R, G, B), Size)
    End Function


    Public Function SetPosition(Optional ByVal Horizontal As StringAlignment = StringAlignment.Center, Optional ByVal Vertical As StringAlignment = StringAlignment.Center) As StringFormat
        Return New StringFormat() With {.Alignment = Horizontal, .LineAlignment = Vertical}
    End Function


    Function ColorToMatrix(ByVal C As Color) As Single()()
        Return New Single()() {
            New Single() {Convert.ToSingle(C.R / 255), 0, 0, 0, 0}, _
            New Single() {0, Convert.ToSingle(C.G / 255), 0, 0, 0}, _
            New Single() {0, 0, Convert.ToSingle(C.B / 255), 0, 0}, _
            New Single() {0, 0, 0, Convert.ToSingle(C.A / 255), 0}, _
            New Single() {Convert.ToSingle(C.R / 255), Convert.ToSingle(C.G / 255), Convert.ToSingle(C.B / 255), 0.0F, Convert.ToSingle(C.A / 255)}}
    End Function

    Public Function ImageFromBase64(ByVal Base64Image As String) As Image
        Using ms As New System.IO.MemoryStream(Convert.FromBase64String(Base64Image))
            Return Image.FromStream(ms)
        End Using
    End Function


    'Tambahan greenui_theme
    Public Function ShadowBrush(ByVal R As Rectangle, ByVal C As Color, ByVal Intesity As Integer, ByVal angle As Integer) As LinearGradientBrush
        Return New LinearGradientBrush(R, Color.FromArgb(Intesity, C), Color.Transparent, angle)
    End Function


    Public GP As GraphicsPath

    Public Enum MouseMode As Byte
        NormalMode
        Hovered
        Pushed
        Normal
        Disabled
    End Enum

    Public Sub DrawImageFromBase64(ByVal G As Graphics, ByVal Base64Image As String, ByVal Rect As Rectangle)
        Dim IM As Image = Nothing
        With G
            Using ms As New System.IO.MemoryStream(Convert.FromBase64String(Base64Image))
                IM = Image.FromStream(ms) : ms.Close()
            End Using
            .DrawImage(IM, Rect)
        End With
    End Sub

    Public Sub FillRoundedPath(ByVal G As Graphics, ByVal C As Color, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        With G
            .FillPath(New SolidBrush(C), RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
        End With
    End Sub

    Public Sub FillRoundedPath(ByVal G As Graphics, ByVal B As Brush, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        With G
            .FillPath(B, RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
        End With
    End Sub

    Public Sub DrawRoundedPath(ByVal G As Graphics, ByVal C As Color, ByVal Size As Single, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        With G
            .DrawPath(New Pen(C, Size), RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
        End With
    End Sub

    Public Sub DrawTriangle(ByVal G As Graphics, ByVal C As Color, ByVal Size As Integer, ByVal P1_0 As Point, ByVal P1_1 As Point, ByVal P2_0 As Point, ByVal P2_1 As Point, ByVal P3_0 As Point, ByVal P3_1 As Point)
        With G
            .DrawLine(New Pen(C, Size), P1_0, P1_1)
            .DrawLine(New Pen(C, Size), P2_0, P2_1)
            .DrawLine(New Pen(C, Size), P3_0, P3_1)
        End With
    End Sub

    Public Function Triangle(ByVal Clr As Color, ByVal P1 As Point, ByVal P2 As Point, ByVal P3 As Point) As Point()
        Return New Point() {P1, P2, P3}
    End Function

    Public Function PenRGBColor(ByVal GR As Graphics, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, ByVal Size As Single) As Pen
        Return New Pen(Color.FromArgb(R, G, B), Size)
    End Function

    Public Sub CentreString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(0, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2) + 0, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Center})
    End Sub

    Public Sub LeftString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(4, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2) + 0, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Near})
    End Sub

    Public Sub RightString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(4, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2), Rect.Width - Rect.Height + 10, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Far})
    End Sub

    Public Function RoundRec(ByVal r As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True) As GraphicsPath
        Dim CreateRoundPath As New GraphicsPath(FillMode.Winding)
        If TopLeft Then
            CreateRoundPath.AddArc(r.X, r.Y, Curve, Curve, 180.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.X, r.Y, r.X, r.Y)
        End If
        If TopRight Then
            CreateRoundPath.AddArc(r.Right - Curve, r.Y, Curve, Curve, 270.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.Right - r.Width, r.Y, r.Width, r.Y)
        End If
        If BottomRight Then
            CreateRoundPath.AddArc(r.Right - Curve, r.Bottom - Curve, Curve, Curve, 0.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.Right, r.Bottom, r.Right, r.Bottom)

        End If
        If BottomLeft Then
            CreateRoundPath.AddArc(r.X, r.Bottom - Curve, Curve, Curve, 90.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.X, r.Bottom, r.X, r.Bottom)
        End If
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function


End Module

#End Region

'=========================[ Youtube ]=========================

'=========================[  Pale  ]=========================

#Region " Button "

'PaleBlueButton
Public Class GabungButton1 : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 30

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(80, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum SideAligin
        Left
        Right
    End Enum

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan

                End Select
            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

'PaleBlueButton
Public Class GabungButton2 : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 30

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(80, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum SideAligin
        Left
        Right
    End Enum

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan

                End Select
            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

'PaleBlueButton
Public Class GabungButton3 : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 30

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(80, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum SideAligin
        Left
        Right
    End Enum

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan

                End Select
            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

'PaleBlueButton
Public Class GabungFlatButton1 : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 1

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(80, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum SideAligin
        Left
        Right
    End Enum

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan

                End Select
            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

'PaleBlueButton
Public Class GabungFlatButton2 : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 1


#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(80, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum SideAligin
        Left
        Right
    End Enum

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan

                End Select
            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

'PaleBlueButton
Public Class GabungFlatButton3 : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 1

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Size = New Size(80, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum SideAligin
        Left
        Right
    End Enum

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect) 'warna tulisan

                End Select
            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

#End Region


#Region " TextBox "

'PaleTextbox
Public Class GabungTextbox1 : Inherits Control

#Region " Variables "

    Protected WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Protected State As MouseMode = MouseMode.NormalMode
    Private _BackColor As Color = Color.Transparent
    Private _RoundRadius As Integer = 1 ' untuk elips
#End Region

#Region " Native Methods "

    Private Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)> ByVal lParam As String) As Int32

#End Region

#Region " Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    ReadOnly Property BorderStyle As BorderStyle
        Get
            Return BorderStyle.None
        End Get
    End Property

    Public Overridable Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If T IsNot Nothing Then
                T.TextAlign = value
            End If
        End Set
    End Property

    'untuk elips
    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

    Public Overridable Shadows Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If T IsNot Nothing Then
                T.MaxLength = value
            End If
        End Set
    End Property
    Public Shadows Property BackColor As Color
        Get
            Return _BackColor
        End Get
        Set(ByVal value As Color)
            MyBase.BackColor = value
            _BackColor = value
            T.BackColor = value
            Invalidate()
        End Set
    End Property

    Public Overridable Shadows Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If T IsNot Nothing Then
                T.ReadOnly = value
            End If
        End Set
    End Property

    Public Overridable Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If T IsNot Nothing Then
                T.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property Multiline() As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable Shadows Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If T IsNot Nothing Then
                T.Text = value
            End If
        End Set
    End Property

    Public Property WatermarkText As String
        Get
            Return _WatermarkText
        End Get
        Set(ByVal value As String)
            _WatermarkText = value
            SendMessage(T.Handle, &H1501, 0, value)
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    Enum SideAligin
        Left
        Right
    End Enum
    Private _SideImageAlign As SideAligin = SideAligin.Left
    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                  ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                  ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        With T
            .Multiline = False
            .Cursor = Cursors.IBeam
            .BackColor = Color.FromArgb(251, 251, 251) 'warna background
            .ForeColor = Color.FromArgb(124, 133, 142) 'warna tulisan
            .BorderStyle = BorderStyle.None
            .Location = New Point(7, 7)
            .Font = Font
            .Size = New Size(Width - 10, 30)
            .UseSystemPasswordChar = _UseSystemPasswordChar
        End With
        Size = New Size(135, 30)
        UpdateStyles()

    End Sub

#End Region

#Region " Events "

    Private Sub T_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles T.TextChanged
        Text = T.Text
    End Sub

    Private Sub T_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles T.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.A Then e.SuppressKeyPress = True
        If e.Control AndAlso e.KeyCode = Keys.C Then
            T.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected NotOverridable Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(T) Then Controls.Add(T)
    End Sub

    Protected NotOverridable Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 30
    End Sub

    Private Sub T_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseHover
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Private Sub T_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseLeave
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles T.MouseUp
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseEnter
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseDown(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseDown
        State = MouseMode.Pushed
        Invalidate()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Height = 30

            With G

                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan

                End Select

                If Not SideImage Is Nothing Then
                    If SideImageAlign = SideAligin.Right Then
                        T.Location = New Point(7, 4.5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(Rect.Width - 24, 6, 16, 16))
                    Else
                        T.Location = New Point(33, 4.5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(8, 6, 16, 16))
                    End If

                Else
                    T.Location = New Point(7, 4.5)
                    T.Width = Width - 13
                End If

                If Not ContextMenuStrip Is Nothing Then T.ContextMenuStrip = ContextMenuStrip

            End With
            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

End Class

'PaleTextbox
Public Class GabungTextbox2 : Inherits Control

#Region " Variables "

    Protected WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Protected State As MouseMode = MouseMode.NormalMode
    Private _BackColor As Color = Color.Transparent
    Private _RoundRadius As Integer = 30 ' untuk elips
#End Region

#Region " Native Methods "

    Private Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)> ByVal lParam As String) As Int32

#End Region

#Region " Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    ReadOnly Property BorderStyle As BorderStyle
        Get
            Return BorderStyle.None
        End Get
    End Property

    Public Overridable Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If T IsNot Nothing Then
                T.TextAlign = value
            End If
        End Set
    End Property

    'untuk elips
    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(ByVal value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property

    Public Overridable Shadows Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If T IsNot Nothing Then
                T.MaxLength = value
            End If
        End Set
    End Property
    Public Shadows Property BackColor As Color
        Get
            Return _BackColor
        End Get
        Set(ByVal value As Color)
            MyBase.BackColor = value
            _BackColor = value
            T.BackColor = value
            Invalidate()
        End Set
    End Property

    Public Overridable Shadows Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If T IsNot Nothing Then
                T.ReadOnly = value
            End If
        End Set
    End Property

    Public Overridable Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If T IsNot Nothing Then
                T.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property Multiline() As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable Shadows Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If T IsNot Nothing Then
                T.Text = value
            End If
        End Set
    End Property

    Public Property WatermarkText As String
        Get
            Return _WatermarkText
        End Get
        Set(ByVal value As String)
            _WatermarkText = value
            SendMessage(T.Handle, &H1501, 0, value)
            Invalidate()
        End Set
    End Property

    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    Enum SideAligin
        Left
        Right
    End Enum
    Private _SideImageAlign As SideAligin = SideAligin.Left
    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(ByVal value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                  ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                  ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        With T
            .Multiline = False
            .Cursor = Cursors.IBeam
            .BackColor = Color.FromArgb(251, 251, 251) 'warna background
            .ForeColor = Color.FromArgb(124, 133, 142) 'warna tulisan
            .BorderStyle = BorderStyle.None
            .Location = New Point(7, 7)
            .Font = Font
            .Size = New Size(Width - 10, 30)
            .UseSystemPasswordChar = _UseSystemPasswordChar
        End With
        Size = New Size(135, 30)
        UpdateStyles()

    End Sub

#End Region

#Region " Events "

    Private Sub T_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles T.TextChanged
        Text = T.Text
    End Sub

    Private Sub T_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles T.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.A Then e.SuppressKeyPress = True
        If e.Control AndAlso e.KeyCode = Keys.C Then
            T.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected NotOverridable Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(T) Then Controls.Add(T)
    End Sub

    Protected NotOverridable Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 30
    End Sub

    Private Sub T_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseHover
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Private Sub T_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseLeave
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles T.MouseUp
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseEnter
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseDown(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseDown
        State = MouseMode.Pushed
        Invalidate()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Height = 30

            With G

                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, Rect, RoundRadius) 'garis tepi              
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Hovered
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan
                    Case MouseMode.Pushed
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(251, 251, 251)), Rect, RoundRadius) 'background
                        DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, Rect, RoundRadius) 'garis tepi 
                        CentreString(G, Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), Rect) 'warna tulisan

                End Select

                If Not SideImage Is Nothing Then
                    If SideImageAlign = SideAligin.Right Then
                        T.Location = New Point(7, 4.5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(Rect.Width - 24, 6, 16, 16))
                    Else
                        T.Location = New Point(33, 4.5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(8, 6, 16, 16))
                    End If

                Else
                    T.Location = New Point(7, 4.5)
                    T.Width = Width - 13
                End If

                If Not ContextMenuStrip Is Nothing Then T.ContextMenuStrip = ContextMenuStrip

            End With
            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

End Class

'NordTextbox
Public Class GabungTextbox3 : Inherits Control

#Region " Variables "

    Private WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Private State As MouseMode = MouseMode.NormalMode

#End Region

#Region " Native Methods "

    Private Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)> ByVal lParam As String) As Int32

#End Region

#Region " Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    ReadOnly Property BorderStyle As BorderStyle
        Get
            Return BorderStyle.None
        End Get
    End Property

    <Category("Appearance"),
    Description("Gets or sets how text is aligned in a System.Windows.Forms.TextBox control.")>
    Public Overridable Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If T IsNot Nothing Then
                T.TextAlign = value
            End If
        End Set
    End Property

    <Category("Behavior"),
     Description("Gets or sets how text is aligned in a System.Windows.Forms.TextBox control.")>
    Public Overridable Shadows Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If T IsNot Nothing Then
                T.MaxLength = value
            End If
        End Set
    End Property



    <Category("Behavior"),
     Description("Gets or sets a value indicating whether text in the text box is read-only.")>
    Public Overridable Shadows Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If T IsNot Nothing Then
                T.ReadOnly = value
            End If
        End Set
    End Property

    <Category("Behavior"),
     Description("Gets or sets a value indicating whether the text in the System.Windows.Forms.TextBox control should appear as the default password character.")>
    Public Overridable Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If T IsNot Nothing Then
                T.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property Multiline() As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
    End Property

    <Category("Appearance"),
    Description("Gets or sets the current text in the System.Windows.Forms.TextBox.")>
    Public Overridable Shadows Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If T IsNot Nothing Then
                T.Text = value
            End If
        End Set
    End Property

    <Category("Custom Properties "),
     Description("Gets or sets the text in the System.Windows.Forms.TextBox while being empty.")>
    Public Property WatermarkText As String
        Get
            Return _WatermarkText
        End Get
        Set(ByVal value As String)
            _WatermarkText = value
            SendMessage(T.Handle, &H1501, 0, value)
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties "),
     Description("Gets or sets the image of the control.")>
    <Browsable(True)> Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(ByVal value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property




#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                  ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                  ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(251, 251, 251)
        With T
            .Multiline = False
            .Cursor = Cursors.IBeam
            .BackColor = Color.FromArgb(251, 251, 251)
            .ForeColor = Color.FromArgb(124, 133, 142)
            .BorderStyle = BorderStyle.None
            .Location = New Point(7, 8)
            .Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .Size = New Size(Width - 10, 30)
            .UseSystemPasswordChar = _UseSystemPasswordChar
        End With
        Size = New Size(135, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Private Sub T_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles T.TextChanged
        Text = T.Text
    End Sub

    Private Sub T_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles T.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.A Then e.SuppressKeyPress = True
        If e.Control AndAlso e.KeyCode = Keys.C Then
            T.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected NotOverridable Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(T) Then Controls.Add(T)
    End Sub

    Protected NotOverridable Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 30
    End Sub

    Private Sub T_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseHover
        State = MouseMode.Hovered
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub
    Private Sub T_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseLeave
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles T.MouseUp
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseEnter
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseDown(ByVal sender As Object, ByVal e As EventArgs) Handles T.MouseDown
        State = MouseMode.Pushed
        Invalidate()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        With G
            'Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            'Height = 30

            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Select Case State

                Case MouseMode.NormalMode
                    .DrawLine(New Pen(Color.FromArgb(213, 213, 213), 1), New Point(0, 29), New Point(Width, 29))
                Case MouseMode.Hovered
                    .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 2), New Point(0, 29), New Point(Width, 29)) 'ketebelan Garis
                Case MouseMode.Pushed
                    .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 2), New Point(0, 29), New Point(Width, 29)) 'ketebelan Garis

            End Select

            If Not SideImage Is Nothing Then
                T.Location = New Point(33, 5)
                T.Width = Width - 60
                .InterpolationMode = InterpolationMode.HighQualityBicubic
                .DrawImage(SideImage, New Rectangle(8, 5, 16, 16))
            Else
                T.Location = New Point(7, 5)
                T.Width = Width - 10
            End If

            If Not ContextMenuStrip Is Nothing Then T.ContextMenuStrip = ContextMenuStrip

        End With

    End Sub

#End Region

End Class



#End Region


#Region " ComboBox "
'PaleComboBox
Public Class GabungComboBox1 : Inherits ComboBox

    Private _StartIndex As Integer = 0

    Sub New()

        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or _
                  ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Regular)
        ' Font = New Font("Myriad Pro", 11)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
        DropDownHeight = 100
        DropDownStyle = ComboBoxStyle.DropDownList
        UpdateStyles()
        Size = New Size(135, 30)

    End Sub

    Private Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            With G
                .SmoothingMode = SmoothingMode.AntiAlias
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), e.Bounds) '0055E5 'Warna kotak dropwon
                    .DrawRectangle(New Pen(Color.FromArgb(251, 251, 251), 1), e.Bounds) '0055E5 'Warna border kotak
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, Brushes.White, 1, e.Bounds.Y + 3)
                Else
                    .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), e.Bounds)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(Color.FromArgb(124, 133, 142)), 1, e.Bounds.Y + 3) 'Warna Font
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(1, 1, Width - 2.5, Height - 2.5)
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                .SmoothingMode = SmoothingMode.AntiAlias
                FillRoundedPath(G, Brushes.White, Rect, 5)
                DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1.5, Rect, 5)  'Border Luar Sebelah biru 
                FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(Width - 30, 1.4, 29, Height - 2.5), 5) 'Background dropdown
                DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1.5, New Rectangle(Width - 30, 1.4, 29, Height - 2.5), 5) 'Border Background dropdown 'FC3955
                DrawTriangle(G, Color.White, 1.5, _
                          New Point(Width - 20, 12), New Point(Width - 16, 16), _
                          New Point(Width - 16, 16), New Point(Width - 12, 12), _
                          New Point(Width - 16, 17), New Point(Width - 16, 16) _
                          )
                .DrawString(Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(7, 1.5, Width - 1, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near}) 'Warna Font setelah di klik
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 27
        Invalidate()
    End Sub

End Class

'AcaciaComboBox
Public Class GabungComboBox2 : Inherits ComboBox

#Region " Variables "

    Private _StartIndex As Integer = 0

#End Region

#Region " Constructors "

    Sub New()

        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or _
                  ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 10, FontStyle.Regular)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
        DropDownStyle = ComboBoxStyle.DropDownList
        UpdateStyles()
        Size = New Size(135, 30)
    End Sub

#End Region

#Region " Properties "

    Private Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property

#End Region

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            With G
                .SmoothingMode = SmoothingMode.AntiAlias
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    .FillRectangle(New SolidBrush(Color.FromArgb(30, Color.FromArgb(41, 133, 211))), e.Bounds) 'Warna Garis drop down jika di select
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), New Font("Segoe UI", 10, FontStyle.Regular), New SolidBrush(Color.FromArgb(124, 133, 142)), 1, e.Bounds.Y + 4)
                Else
                    .FillRectangle(New SolidBrush(Color.FromArgb(120, Color.FromArgb(251, 251, 251))), e.Bounds) 'Warna Garis drop down tidak di select
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), New Font("Segoe UI", 10, FontStyle.Regular), New SolidBrush(Color.FromArgb(124, 133, 142)), 1, e.Bounds.Y + 4)
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(1, 1, Width - 2.5, Height - 2.5)
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1.7, Rect, 1) 'warna kotak 
                .SmoothingMode = SmoothingMode.AntiAlias
                'warna garis panah
                DrawTriangle(G, Color.FromArgb(41, 133, 211), 1.5, _
                          New Point(Width - 20, 12), New Point(Width - 16, 16), _
                          New Point(Width - 16, 16), New Point(Width - 12, 12), _
                          New Point(Width - 16, 17), New Point(Width - 16, 16) _
                          )
                .SmoothingMode = SmoothingMode.None
                .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Regular), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(7, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

#Region " Events "

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

#End Region

End Class

'NordComboBox
Public Class GabungComboBox3 : Inherits ComboBox

#Region " Variables "

    Private _StartIndex As Integer = 0


#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        BackColor = Color.FromArgb(251, 251, 251)
        Font = New Font("Segoe UI", 10, FontStyle.Regular)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
        DropDownHeight = 100
        DropDownStyle = ComboBoxStyle.DropDownList
        UpdateStyles()
        Size = New Size(135, 30)
    End Sub

#End Region

#Region " Properties "

    Private Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property


#End Region

#Region " Draw Control "

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            e.DrawBackground()
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), e.Bounds)

                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    Cursor = Cursors.Hand
                    CentreString(G, Items(e.Index), Font, New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 3, e.Bounds.Width - 2, e.Bounds.Height - 2))
                Else
                    CentreString(G, Items(e.Index), Font, New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2))
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim Rect As New Rectangle(0, 0, Width, Height - 1)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), Rect)

            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 2), New Point(Width - 21, (Height / 2) - 3), New Point(Width - 7, (Height / 2) - 3))
            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 2), New Point(Width - 21, (Height / 2) + 1), New Point(Width - 7, (Height / 2) + 1))
            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 2), New Point(Width - 21, (Height / 2) + 5), New Point(Width - 7, (Height / 2) + 5))

            .DrawLine(New Pen(Color.FromArgb(213, 213, 213), 1), New Point(0, Height - 1), New Point(Width, Height - 1))
            .DrawString(Text, Font, New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(5, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        End With
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

#End Region

End Class

'GreenUIComboBox
Public Class GabungComboBox4 : Inherits ComboBox

#Region " Variables "

    Private _StartIndex As Integer = 0

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
        DropDownHeight = 100
        BackColor = Color.Transparent
        DropDownStyle = ComboBoxStyle.DropDownList
        Size = New Size(135, 30)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
        UpdateStyles()

    End Sub

#End Region

#Region " Properties "

    Private Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), e.Bounds)
                Cursor = Cursors.Hand
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    CentreString(G, Items(e.Index), New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 3, e.Bounds.Width - 2, e.Bounds.Height - 2))
                Else
                    CentreString(G, Items(e.Index), New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 3, e.Bounds.Width - 2, e.Bounds.Height - 2))
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            FillRoundedPath(e.Graphics, Color.FromArgb(251, 251, 251), Rect, 8)
            FillRoundedPath(e.Graphics, Color.FromArgb(251, 251, 251), New Rectangle(Width - 28, 0, Width - 1, Height - 1), 10, False, True, False, True)
            DrawRoundedPath(e.Graphics, Color.FromArgb(200, Color.FromArgb(41, 133, 211)), 1, Rect, 8)


            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 1.5), New Point(Width - 21, (Height / 2) - 4.5), New Point(Width - 7, (Height / 2) - 4.5))
            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 1.5), New Point(Width - 21, (Height / 2) - 0.5), New Point(Width - 7, (Height / 2) - 0.5))
            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 1.5), New Point(Width - 21, (Height / 2) + 3.5), New Point(Width - 7, (Height / 2) + 3.5))
            .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 1), New Point(Width - 28, 1), New Point(Width - 28, Height - 1))
            .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(5, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        End With
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

#End Region

End Class


#End Region




#Region " CheckBox "
'GreenUICheckBox
Public Class GabungCheckBox1 : Inherits Control

#Region " Variables "

    Private _Checked As Boolean = False
    Private State As MouseMode = MouseMode.NormalMode

#End Region

#Region " Properties "

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 20
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        BackColor = Color.Transparent
    End Sub

#End Region

#Region " Initialization "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer _
                   Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UseTextForAccessibility, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(135, 30)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
    End Sub
#End Region

#Region " Draw Control "
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                .PixelOffsetMode = PixelOffsetMode.Half
                If Checked Then
                    FillRoundedPath(G, New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(4, 4, 12, 12), 2)
                    DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 2, New Rectangle(1, 1, 18, 18), 2)
                    .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(22, +1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                Else
                    FillRoundedPath(G, Color.FromArgb(213, 213, 213), New Rectangle(4, 4, 12, 12), 2)
                    DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 2, New Rectangle(1, 1, 18, 18), 2)
                    .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(22, +1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                End If
            End With
            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose() : B.Dispose()
        End Using
    End Sub
#End Region

#Region " Mouse Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

#End Region



#Region " RadioButton "
'GreenUIRadioButton
Public Class GabungRadioButton1 : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Private _Group As Integer = 1
    Event CheckedChanged(ByVal sender As Object)

#End Region

#Region " Properties "

    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Property Group As Integer
        Get
            Return _Group
        End Get
        Set(ByVal value As Integer)
            _Group = value
        End Set
    End Property

#End Region

#Region " Initialization "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Size = New Size(135, 30)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G

                .SmoothingMode = SmoothingMode.AntiAlias
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                If Checked Then
                    .DrawEllipse(New Pen(Color.FromArgb(41, 133, 211), 1.9), 1, 1, 18, 18)
                    .FillEllipse(New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(4, 4, 12, 12))
                    .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(23, -1, Width, Height))
                Else
                    .DrawEllipse(New Pen(Color.FromArgb(213, 213, 213), 1.9), 1, 1, 18, 18)
                    .FillEllipse(New SolidBrush(Color.FromArgb(213, 213, 213)), New Rectangle(4, 4, 12, 12))
                    .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(23, -1, Width, Height))
                End If


            End With

            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose()
            B.Dispose()
        End Using

    End Sub

#End Region

#Region " Events "

    Private Sub UpdateState()
        If Not IsHandleCreated OrElse Not Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is GabungRadioButton1 AndAlso DirectCast(C, GabungRadioButton1).Group = _Group Then
                DirectCast(C, GabungRadioButton1).Checked = False
            End If
        Next
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        UpdateState()
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnCreateControl()
        UpdateState()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        Invalidate()
        MyBase.OnResize(e)
        Height = 21
    End Sub

#End Region

End Class

#End Region



#Region " RichTextBox "
'ElegantThemeRichTextBox
Public Class GabungRichTextBox
    Inherits Control

#Region "Declarations"
    Private WithEvents TB As New RichTextBox

#End Region

#Region "Events"

    Overrides Property Text As String
        Get
            Return TB.Text
        End Get
        Set(ByVal value As String)
            TB.Text = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        TB.BackColor = Color.FromArgb(251, 251, 251)
        Invalidate()
    End Sub

    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        TB.ForeColor = ForeColor
        Invalidate()
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        TB.Size = New Size(Width - 10, Height - 11)
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        TB.Font = Font
    End Sub

    Sub TextChanges() Handles MyBase.TextChanged
        TB.Text = Text
    End Sub

    Sub AppendText(ByVal Text As String)
        TB.Focus()
        TB.AppendText(Text)
        Invalidate()
    End Sub

#End Region

#Region "Draw Control"

    Sub New()
        Font = New Font("Segoe UI", 10, FontStyle.Regular)
        With TB
            .Multiline = True
            .ForeColor = Color.FromArgb(124, 133, 142)
            .Text = String.Empty
            .BorderStyle = BorderStyle.None
            .Location = New Point(5, 5)
            .Font = New Font("Segoe UI", 10, FontStyle.Regular)
            .Size = New Size(Width - 10, Height - 10)
        End With
        Controls.Add(TB)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(Color.FromArgb(251, 251, 251))
            .DrawRectangle(New Pen(Color.FromArgb(213, 213, 213), 1), ClientRectangle)
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

#End Region

End Class

#End Region


#Region " ListBox "
'ElegantThemeListBox
Public Class GabungListBox
    Inherits Control

#Region "Declarations"

    Private WithEvents ListB As New ListBox
    Private _Items As String() = {""}

#End Region

#Region "Properties"

    <Category("Control")> _
    Public Property Items As String()
        Get
            Return _Items
        End Get
        Set(ByVal value As String())
            _Items = value
            ListB.Items.Clear()
            ListB.Items.AddRange(value)
            Invalidate()
        End Set
    End Property


    Public ReadOnly Property SelectedItem() As String
        Get
            Return ListB.SelectedItem
        End Get
    End Property

    Public ReadOnly Property SelectedIndex() As Integer
        Get
            Return ListB.SelectedIndex
            If ListB.SelectedIndex < 0 Then Exit Property
        End Get
    End Property

    Public Sub Clear()
        ListB.Items.Clear()
    End Sub

    Public Sub ClearSelected()
        For i As Integer = (ListB.SelectedItems.Count - 1) To 0 Step -1
            ListB.Items.Remove(ListB.SelectedItems(i))
        Next
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(ListB) Then
            Controls.Add(ListB)
        End If
    End Sub

    Sub AddRange(ByVal items As Object())
        ListB.Items.Remove("")
        ListB.Items.AddRange(items)
    End Sub

    Sub AddItem(ByVal item As Object)
        ListB.Items.Remove("")
        ListB.Items.Add(item)
    End Sub

#End Region

#Region "Draw Control"

    Sub Drawitem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListB.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .InterpolationMode = InterpolationMode.HighQualityBicubic
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            If InStr(e.State.ToString, "Selected,") > 0 Then
                .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                .DrawLine(New Pen(Color.FromArgb(213, 213, 213)), e.Bounds.X, e.Bounds.Y + e.Bounds.Height, e.Bounds.Width, e.Bounds.Y + e.Bounds.Height)
                .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), e.Bounds.X, e.Bounds.Y + 2)
            Else
                .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 8), New SolidBrush(Color.FromArgb(124, 133, 142)), e.Bounds.X, e.Bounds.Y + 2)
            End If
            .Dispose()
        End With
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
            ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        ListB.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ListB.ScrollAlwaysVisible = False
        ListB.HorizontalScrollbar = False
        ListB.BorderStyle = BorderStyle.None
        ListB.BackColor = Color.FromArgb(251, 251, 251)
        ListB.Location = New Point(3, 3)
        ListB.Font = New Font("Segoe UI", 8)
        ListB.ItemHeight = 20
        ListB.Items.Clear()
        ListB.IntegralHeight = False
        Size = New Size(130, 100)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .Clear(Color.FromArgb(248, 250, 249))
            ListB.Size = New Size(Width - 6, Height - 7)
            .FillRectangle(New SolidBrush(Color.FromArgb(251, 251, 251)), Base)
            .DrawRectangle(New Pen((Color.FromArgb(213, 213, 213)), 1), New Rectangle(0, 0, Width, Height - 1))
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

#End Region

End Class

#End Region


#Region " Switch "
'GreenUISwitch
Public Class GabungSwitch1 : Inherits Control

#Region " Variables "

    Private _Switch As Boolean = False
    Private State As MouseMode
    Private _ForeColor As Color = Color.Gray

#End Region

#Region " Properties "

    Public Property Switched() As Boolean
        Get
            Return _Switch
        End Get
        Set(ByVal value As Boolean)
            _Switch = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Initialization "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                    ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(70, 28)
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                If Switched Then

                    FillRoundedPath(G, Color.FromArgb(41, 133, 211), New Rectangle(0, 0, 70, 27), 8)

                    FillRoundedPath(G, Color.FromArgb(251, 251, 251), New Rectangle(Width - 28.5, 1.5, 25, 23), 8)
                    .DrawLine(New Pen(Color.FromArgb(41, 133, 211)), Width - 14, 8, Width - 14, 18)
                    .DrawLine(New Pen(Color.FromArgb(41, 133, 211)), Width - 16, 8, Width - 16, 18)
                    .DrawLine(New Pen(Color.FromArgb(41, 133, 211)), Width - 18, 8, Width - 18, 18)
                    DrawRoundedPath(G, Color.FromArgb(41, 133, 211), 1, New Rectangle(0, 0, 69, 27), 8)
                    .DrawString("ON", New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(251, 251, 251)), New Point(Width - 62, 6))
                Else

                    FillRoundedPath(G, Color.FromArgb(213, 213, 213), New Rectangle(0, 0, 70, 27), 8)

                    FillRoundedPath(G, Color.FromArgb(251, 251, 251), New Rectangle(3, 1.5, 25, 23), 8)
                    .DrawLine(New Pen(Color.FromArgb(213, 213, 213)), 13, 8, 13, 18)
                    .DrawLine(New Pen(Color.FromArgb(213, 213, 213)), 15, 8, 15, 18)
                    .DrawLine(New Pen(Color.FromArgb(213, 213, 213)), 17, 8, 17, 18)
                    DrawRoundedPath(G, Color.FromArgb(213, 213, 213), 1, New Rectangle(0, 0, 69, 27), 8)
                    .DrawString("OFF", New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(41, 133, 211)), New Point(31, 6))

                End If

            End With

            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose() : B.Dispose()

        End Using

    End Sub

#End Region

#Region " Mouse & Other Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Switch = Not _Switch
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(70, 28)
    End Sub

#End Region

End Class

'NordSwitchPower
Public Class GabungSwitch2 : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode

#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent

        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias
            If Checked Then
                FillRoundedPath(e.Graphics, New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(0, 8, 55, 25), 20)
                .FillEllipse(New SolidBrush(Color.FromArgb(213, 213, 213)), New Rectangle(Width - 39, 0, 35, 40))
                .DrawArc(New Pen(Color.FromArgb(41, 133, 211), 2), Width - 31, 10, 19, Height - 23, -62, 300)
                .DrawLine(New Pen(Color.FromArgb(41, 133, 211), 2), Width - 22, 8, Width - 22, 17)
            Else
                FillRoundedPath(e.Graphics, New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(2, 8, 55, 25), 20)
                .FillEllipse(New SolidBrush(Color.FromArgb(213, 213, 213)), New Rectangle(0, 0, 35, 40))
                .DrawArc(New Pen(Color.FromArgb(124, 133, 142), 2), CInt(7.5), 10, Width - 41, Height - 23, -62, 300)
                .DrawLine(New Pen(Color.FromArgb(124, 133, 142), 2), 17, 8, 17, 17)
            End If
        End With

    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Size = New Size(60, 44)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class

'NordSwitchBlue
Public Class GabungSwitch3 : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias

            If Checked Then
                FillRoundedPath(e.Graphics, New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(0, 0, 40, 16), 16)
                .FillEllipse(New SolidBrush(Color.FromArgb(251, 251, 251)), New Rectangle(Width - 14.5, 2.7, 10, 10))
            Else
                DrawRoundedPath(e.Graphics, Color.FromArgb(41, 133, 211), 1.8, New Rectangle(0, 0, 40, 16), 16)
                .FillEllipse(New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(2.7, 2.7, 10, 10))
            End If

        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(42, 18)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class

'NordSwitchBlue
Public Class GabungSwitch4 : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias

            If Checked Then
                DrawRoundedPath(e.Graphics, Color.FromArgb(41, 133, 211), 1.8, New Rectangle(0, 0, 40, 16), 16)
                .FillEllipse(New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(Width - 14.5, 2.7, 10, 10))
            Else
                DrawRoundedPath(e.Graphics, Color.FromArgb(213, 213, 213), 1.8, New Rectangle(0, 0, 40, 16), 16)
                .FillEllipse(New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(2.7, 2.7, 10, 10))
            End If

        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(42, 18)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class

'NordSwitchBlue
Public Class GabungSwitch5 : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias

            If Checked Then
                'background
                FillRoundedPath(e.Graphics, New SolidBrush(Color.FromArgb(41, 133, 211)), New Rectangle(0, 0, 40, 16), 16)
                'border luar
                'DrawRoundedPath(e.Graphics, Color.FromArgb(41, 133, 211), 1.8, New Rectangle(0, 0, 40, 16), 16)
                'buletan
                .FillEllipse(New SolidBrush(Color.FromArgb(251, 251, 251)), New Rectangle(Width - 14.5, 2.7, 10, 10))
                .DrawString("ON", New Font("Segoe UI", 8, FontStyle.Bold), New SolidBrush(Color.FromArgb(251, 251, 251)), New Point(2.7, 2))
            Else
                'background
                FillRoundedPath(e.Graphics, New SolidBrush(Color.FromArgb(213, 213, 213)), New Rectangle(0, 0, 40, 16), 16)
                'border luar
                'DrawRoundedPath(e.Graphics, Color.FromArgb(213, 213, 213), 1.8, New Rectangle(0, 0, 40, 16), 16)
                'buletan
                .FillEllipse(New SolidBrush(Color.FromArgb(124, 133, 142)), New Rectangle(2.7, 2.7, 10, 10))
                .DrawString("OFF", New Font("Segoe UI", 8, FontStyle.Bold), New SolidBrush(Color.FromArgb(124, 133, 142)), New Point(16, 1.5))
            End If

        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(42, 18)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class


#End Region


#Region " TrackBar "
'GreenUITrackBar
Public Class GabungTrackBar1 : Inherits Control

#Region " Variables "

    Private Variable As Boolean
    Private Track As Rectangle
    Private _Maximum As Integer = 100
    Private _Minimum As Integer
    Private _Value As Integer
    Private CurrentValue As Integer = CInt(Value / Maximum * Width)
    Private P1, P2, P3, P4, P5, P6 As Point

#End Region

#Region " Properties "

    Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            _Maximum = value
            RenewCurrentValue()
            MoveTrack()
            Invalidate()
        End Set

    End Property

    Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If Not value < 0 Then
                _Minimum = value
                RenewCurrentValue()
                MoveTrack()
                Invalidate()
            End If
        End Set
    End Property

    Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value <> _Value Then
                _Value = value
                RenewCurrentValue()
                MoveTrack()
                RaiseEvent Scroll(Me)
                Invalidate()
            End If
        End Set

    End Property

#End Region

#Region " Initialization "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)

        BackColor = Color.Transparent
        DoubleBuffered = True

    End Sub

#End Region

#Region " Mouse & Other Events "


    Event Scroll(ByVal sender As Object)

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Subtract OrElse e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Left Then
            If Value = 0 Then Exit Sub
            Value -= 1
        ElseIf e.KeyCode = Keys.Add OrElse e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Right Then
            If Value = Maximum Then Exit Sub
            Value += 1
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left AndAlso Height > 0 Then
            RenewCurrentValue()
            If Width > 0 AndAlso Height > 0 Then Track = New Rectangle(CurrentValue + 0.8, 0, 25, 24)

            Variable = New Rectangle(CurrentValue, 0, 24, Height).Contains(e.Location)
        End If
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If Variable AndAlso e.X > -1 AndAlso e.X < Width + 1 Then Value = Minimum + CInt((Maximum - Minimum) * (e.X / Width))
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        Variable = False : MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            RenewCurrentValue()
            MoveTrack()
            Height = 25
        End If

        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Sub RenewCurrentValue()
        CurrentValue = CInt((Value - Minimum) / (_Maximum - Minimum) * (Width - 25))
    End Sub

    Public Sub MoveTrack()
        If Height > 0 AndAlso Width > 0 Then Track = New Rectangle(CurrentValue - 0.4, 0, 25, 23)
        P1 = New Point(CurrentValue + 9, Track.Y + 5)
        P2 = New Point(CurrentValue + 9, Track.Height - 5)
        P3 = New Point(CurrentValue + 12, Track.Y + 5)
        P4 = New Point(CurrentValue + 12, Track.Height - 5)
        P5 = New Point(CurrentValue + 15, Track.Y + 5)
        P6 = New Point(CurrentValue + 15, Track.Height - 5)
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                'Cursor = Cursors.Hand
                .SmoothingMode = SmoothingMode.AntiAlias
                .PixelOffsetMode = PixelOffsetMode.HighQuality
                .InterpolationMode = InterpolationMode.HighQualityBicubic

                FillRoundedPath(G, Color.FromArgb(251, 251, 251), New Rectangle(0, 5.5, Width, 12), 8)

                DrawRoundedPath(G, Color.FromArgb(200, Color.FromArgb(124, 133, 142)), 1, New Rectangle(0, 5.5, Width, 12), 8)

                If Not CurrentValue = 0 Then
                    FillRoundedPath(G, Color.FromArgb(41, 133, 211), New Rectangle(0, 5.5, CurrentValue + 2, 12), 6)
                End If
                .PixelOffsetMode = PixelOffsetMode.None


                FillRoundedPath(G, Color.FromArgb(213, 213, 213), Track, 6)
                FillRoundedPath(G, New SolidBrush(Color.FromArgb(213, 213, 213)), Track, 6)
                DrawRoundedPath(G, Color.FromArgb(60, Color.FromArgb(124, 133, 142)), 1, Track, 6)
                .DrawLine(New Pen(Color.FromArgb(41, 133, 211)), P1, P2)
                .DrawLine(New Pen(Color.FromArgb(41, 133, 211)), P3, P4)
                .DrawLine(New Pen(Color.FromArgb(41, 133, 211)), P5, P6)

            End With

            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()

        End Using
    End Sub

#End Region

End Class

#End Region


#Region " Tab Control "

'FirefoxMainTabControl
Class GabungTabControl
    Inherits TabControl
#Region " Private "
    Private G As Graphics
    Private TabRect As Rectangle
    Private FC As Color = Nothing
#End Region
#Region " Control "
    Sub New()
        DoubleBuffered = True
        ItemSize = New Size(43, 152)
        Alignment = TabAlignment.Left
        SizeMode = TabSizeMode.Fixed
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint, True)
    End Sub
    Protected Overrides Sub OnControlAdded(ByVal e As ControlEventArgs)
        MyBase.OnControlAdded(e)
        Try
            For i As Integer = 1 To TabPages.Count - 1
                TabPages(i).BackColor = Color.FromArgb(251, 251, 251) 'Warna Background Halaman Tab
                TabPages(i).ForeColor = Color.FromArgb(124, 133, 142) 'Warna Tulisan Halaman Tab
                TabPages(i).Font = New Font("Segoe UI", 10, FontStyle.Regular) 'Jenis Font Halaman Tab
            Next
        Catch
        End Try
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        MyBase.OnPaint(e)

        G.Clear(Color.FromArgb(33, 38, 45)) 'Warna Frame

        For i As Integer = 0 To TabPages.Count - 1
            TabRect = GetTabRect(i)
            If SelectedIndex = i Then
                Using B As New SolidBrush(Color.FromArgb(29, 33, 39))  'Warna Header Tab Klik
                    G.FillRectangle(B, TabRect)
                End Using
                FC = Color.FromArgb(124, 133, 142) 'Warna Font Tab
                Using B As New SolidBrush(Color.FromArgb(41, 133, 211)) 'Warna Tab Klik 
                    G.FillRectangle(B, New Rectangle(TabRect.Location.X - 3, TabRect.Location.Y + 1, 5, TabRect.Height - 2))
                End Using
            Else
                FC = Color.FromArgb(124, 133, 142) 'Warna Font Tab
                Using B As New SolidBrush(Color.FromArgb(33, 38, 45)) 'Warna Header Tab No Klik 
                    G.FillRectangle(B, TabRect)
                End Using
            End If

            Using B As New SolidBrush(Color.FromArgb(124, 133, 142)) 'Warna Font Tab

                G.DrawString(TabPages(i).Text, New Font("Segoe UI", 10, FontStyle.Bold), B, New Point(TabRect.X + 50, TabRect.Y + 12)) 'Jarak Spasi
            End Using


            If Not IsNothing(ImageList) Then
                If Not TabPages(i).ImageIndex < 0 Then
                    G.DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Rectangle(TabRect.X + 19, TabRect.Y + ((TabRect.Height / 2) - 10), 18, 18))
                End If
            End If
        Next
    End Sub
#End Region
End Class
#End Region




#Region "GroupBox"

'ElegantThemeGroupBox
Public Class GabungGroupBox
    Inherits ContainerControl

#Region "Declarations"
    Private _MainColour As Color = Color.FromArgb(251, 251, 251)
    Private _HeaderColour As Color = Color.FromArgb(251, 251, 251)
    Private _TextColour As Color = Color.FromArgb(124, 133, 142)
    Private _BorderColour As Color = Color.FromArgb(213, 213, 213)
#End Region

#Region "Properties"

    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(ByVal value As Color)
            _BorderColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(ByVal value As Color)
            _TextColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property HeaderColour As Color
        Get
            Return _HeaderColour
        End Get
        Set(ByVal value As Color)
            _HeaderColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property MainColour As Color
        Get
            Return _MainColour
        End Get
        Set(ByVal value As Color)
            _MainColour = value
        End Set
    End Property

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(160, 152)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim Circle As New Rectangle(8, 8, 10, 10)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            .FillRectangle(New SolidBrush(_MainColour), New Rectangle(0, 28, Width, Height))
            .FillRectangle(New SolidBrush(_HeaderColour), New Rectangle(0, 0, Width, 28))
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Point(5, 5))
            .DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width, Height))
            .DrawLine(New Pen(_BorderColour), 0, 28, Width, 28)
            .DrawLine(New Pen(_BorderColour, 3), New Point(0, 27), New Point(.MeasureString(Text, Font).Width + 7, 27))
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class
#End Region
