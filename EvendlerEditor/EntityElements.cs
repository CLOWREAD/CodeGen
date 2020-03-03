using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace EvendlerEditor
{
    public class EntityElements
    {
        public System.Collections.Hashtable m_Lines = new System.Collections.Hashtable();
        public System.Collections.Hashtable m_Frames = new System.Collections.Hashtable();

        public String GetLinkedEntityName(String Name,String Slot)
        {
            var f = (EntityFrame_Model)m_Frames[Name];
            if (f == null) return null;
            var s=f.m_Lines[Slot];
            if (s == null) return null;
            var l = (EntityLine_Model)m_Lines[s];
            if (l == null) return null;

            String linked_name = "";
            if(l.from_Name.Equals(Name))
            {
                linked_name = l.to_Name;
            }
            else
            {
                linked_name = l.from_Name;
            }

            var f_l = (EntityFrame_Model)m_Frames[linked_name];
            if (f_l == null) return null;

            return f_l.Name;
        }
        public EntityFrame_Model GetLinkedEntity(String Name, String Slot)
        {
            var f = (EntityFrame_Model)m_Frames[Name];
            if (f == null) return null;
            var s = f.m_Lines[Slot];
            if (s == null) return null;
            var l = (EntityLine_Model)m_Lines[s];
            if (l == null) return null;

            String linked_name = "";
            if (l.from_Name.Equals(Name))
            {
                linked_name = l.to_Name;
            }
            else
            {
                linked_name = l.from_Name;
            }

            var f_l = (EntityFrame_Model)m_Frames[linked_name];
            if (f_l == null) return null;

            return f_l;
        }
        public String GetLinkedEntitySlot(String Name, String Slot)
        {
            var f = (EntityFrame_Model)m_Frames[Name];
            if (f == null) return null;
            var s = f.m_Lines[Slot];
            if (s == null) return null;
            var l = (EntityLine_Model)m_Lines[s];
            if (l == null) return null;

            String linked_name = "";
            String linked_slot = "";
            if (l.from_Name.Equals(Name))
            {
                linked_name = l.to_Name;
                linked_slot = l.to_SlotName;
            }
            else
            {
                linked_name = l.from_Name;
                linked_slot = l.from_SlotName;
            }

            

            return linked_slot;
        }

        public String GenFunctions()
        {
            String res="";
            foreach(EntityFrame_Model f in m_Frames.Values)
            {
                var t_res = f.UIEntity.m_CodeText;
                t_res=t_res.Replace("@FUNCTION ", f.UIEntity.m_Label + "_" + f.Name +" ");
                for (int i=0;i< f.UIEntity.m_ParamsName.Count;i++)
                {
                    String p_name = f.UIEntity.m_ParamsName[i];
                    String p_value = f.UIEntity.GetParam(i);
                    t_res = t_res.Replace("@" + p_name, p_value);
                }

                for (int i = 0; i < f.UIEntity.m_OutputSlot.Count; i++)
                {
                    String o_name = f.UIEntity.m_OutputSlot[i];
                    var linkedframe=GetLinkedEntity(f.Name, o_name);
                    

                    if (o_name.Contains("#"))
                    {
                        if (linkedframe == null)
                        {
                            t_res = t_res.Replace("@" + o_name, "");
                            continue;
                        }
                        String o_value = linkedframe.Name;
                        String o_label = linkedframe.UIEntity.m_Label;
                        t_res = t_res.Replace("@" + o_name, o_label+"_"+o_value);
                    }



                }
                for (int i = 0; i < f.UIEntity.m_InputSlot.Count; i++)
                {
                    String i_name = f.UIEntity.m_InputSlot[i];
                    var linkedframe = GetLinkedEntity(f.Name, i_name);


                   
                    if (linkedframe == null)
                    {
                        t_res = t_res.Replace("@" + i_name, i_name+"=null;");
                        continue;
                    }
                    String o_name = linkedframe.Name;
                    String o_slot = GetLinkedEntitySlot(f.Name, i_name);
                    String o_label = linkedframe.UIEntity.m_Label;
                    t_res = t_res.Replace("@" + i_name, "var "+i_name+"=((dynamic)GLOBAL_OUTPUT[\"" + o_label + "_" + o_name + "\"])."+ o_slot+";");
                    



                }

                res += t_res;
            }
            return res;
        }

        public void Line(String FromName,String ToName, String FromSlotName, String ToSlotName)
        {
            var f= (EntityFrame_Model) m_Frames[FromName];
            if (f == null) { return;}
            var t = (EntityFrame_Model)m_Frames[FromName];
            if (t == null) { return; }


        }


        public static String RandToken()
        {
            //DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //var utc_str = (Math.Floor(((DateTime)i.OpenTime - Epoch).TotalMilliseconds).ToString());

            Random r=new Random();
            //r.Next();

            String res = "";
            for (int i = 0; i < 16; i++)
            {
                int ri = r.Next() % 256;
                res += ri.ToString("X2").Replace("0x", "");
            }
            return res;
        }
    }
    public class EntityLine_Model
    {
        public String Name;
        public System.Drawing.Point from_point, to_point;
        public int from_index = 0, to_index=0;
        public String from_SlotName = "", to_SlotName = "";
        public String from_Name = "", to_Name = "";
        public Windows.UI.Xaml.Shapes.Path UIPath;


        public void Refresh()
        {
            var line = this;           

            var pathGeometry1 = new PathGeometry();
            var pathFigureCollection1 = new PathFigureCollection();
            var pathFigure1 = new PathFigure();
            pathFigure1.IsClosed = false;
            pathFigure1.StartPoint = new Windows.Foundation.Point(line.from_point.X, line.from_point.Y);
            pathFigureCollection1.Add(pathFigure1);
            pathGeometry1.Figures = pathFigureCollection1;

            var pathSegmentCollection1 = new PathSegmentCollection();
            var pathSegment1 = new BezierSegment();
            pathSegment1.Point1 = new Point(line.from_point.X + 200, line.from_point.Y);
            pathSegment1.Point2 = new Point(line.to_point.X - 200, line.to_point.Y);
            pathSegment1.Point3 = new Point(line.to_point.X, line.to_point.Y);
            pathSegmentCollection1.Add(pathSegment1);

            pathFigure1.Segments = pathSegmentCollection1;


            line.UIPath.Data = pathGeometry1;
        }
 

    }

    public class EntityFrame_Model
    {
        public String Name;
        public System.Drawing.Point Pos;
        public EntityFrame UIEntity;
        /// <summary>
        /// KEY: slot name ; Value: line name
        /// </summary>
        public System.Collections.Hashtable m_Lines = new System.Collections.Hashtable(); /// KEY: slot name ; Value: line name
        public static int SlotIndexToY(int i)
        {
            return 48 + 32 + 38 * i;
        }
    }
}
