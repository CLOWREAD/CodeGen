using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace EvendlerEditor
{
  
    public sealed partial class MainPage : Page
    {
        private void C_BUTTON_PANEL_SWITCH_Click(object sender, RoutedEventArgs e)
        {
            C_MAINSPLITVIEW.IsPaneOpen = !C_MAINSPLITVIEW.IsPaneOpen;
        }

        private void C_MAINGRID_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (m_TempLine.to_Name == null && m_TempLine.from_Name == null)
            {
                m_TempLine.UIPath.Visibility = Visibility.Collapsed;
                return;
            }

            m_TempLine.UIPath.Visibility = Visibility.Visible;
            EntityLine_Model t_line = new EntityLine_Model();
            t_line.from_index = m_TempLine.from_index;
            t_line.to_index = m_TempLine.to_index;



            if (m_TempLine.from_Name == null && m_TempLine.to_Name != null)
            {
                var frame_to = (EntityFrame_Model)m_Elements.m_Frames[m_TempLine.to_Name];
                t_line.from_point.X = (int)e.GetCurrentPoint(C_MAINGRID).Position.X;
                t_line.from_point.Y = (int)e.GetCurrentPoint(C_MAINGRID).Position.Y;
                t_line.to_point = new System.Drawing.Point((int)frame_to.UIEntity.Translation.X, (int)frame_to.UIEntity.Translation.Y);

                t_line.to_point.Y += EntityFrame_Model.SlotIndexToY(t_line.to_index);
                t_line.to_point.X += 300 * 0 + 8;
            }
            if (m_TempLine.to_Name == null && m_TempLine.from_Name != null)
            {
                var frame_from = (EntityFrame_Model)m_Elements.m_Frames[m_TempLine.from_Name];
                t_line.to_point.X = (int)e.GetCurrentPoint(C_MAINGRID).Position.X;
                t_line.to_point.Y = (int)e.GetCurrentPoint(C_MAINGRID).Position.Y;
                t_line.from_point = new System.Drawing.Point((int)frame_from.UIEntity.Translation.X, (int)frame_from.UIEntity.Translation.Y);

                t_line.from_point.Y += EntityFrame_Model.SlotIndexToY(t_line.from_index);
                t_line.from_point.X += 300 * 1 - 8;
            }




            //create new path
            /////////////////////////////////////////////////////////////////
            var pathGeometry1 = new PathGeometry();
            var pathFigureCollection1 = new PathFigureCollection();
            var pathFigure1 = new PathFigure();
            pathFigure1.IsClosed = false;
            pathFigure1.StartPoint = new Windows.Foundation.Point(t_line.from_point.X, t_line.from_point.Y);
            pathFigureCollection1.Add(pathFigure1);
            pathGeometry1.Figures = pathFigureCollection1;

            var pathSegmentCollection1 = new PathSegmentCollection();
            var pathSegment1 = new BezierSegment();
            pathSegment1.Point1 = new Point(t_line.from_point.X + 100, t_line.from_point.Y);
            pathSegment1.Point2 = new Point(t_line.to_point.X - 100, t_line.to_point.Y);
            pathSegment1.Point3 = new Point(t_line.to_point.X, t_line.to_point.Y);
            pathSegmentCollection1.Add(pathSegment1);

            pathFigure1.Segments = pathSegmentCollection1;
            ///////////////////////////////////////////////////////////////////

            m_TempLine.UIPath.Data = pathGeometry1;

        }

        private void C_MAINGRID_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //clear m_TempLine
            m_TempLine.from_Name = null;
            m_TempLine.to_Name = null;
            m_TempLine.UIPath.Visibility = Visibility.Collapsed;
        }
    }
}
