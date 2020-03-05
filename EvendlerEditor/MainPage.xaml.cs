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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace EvendlerEditor
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public class PresetItem :IComparable, IEqualityComparer<PresetItem>
    {
        public String Label="";
        public String Code="";

   

        public int CompareTo(object obj)
        {
            PresetItem o = (PresetItem)obj;
            if(o.Code.Equals(this.Code) && o.Label.Equals(this.Label))
            {
                return 0;
            }
            else
            {
                return this.Code.CompareTo(o.Code);
            }

        }

        public bool Equals(PresetItem x, PresetItem y)
        {
            if (x.Code.Equals(y.Code) && x.Label.Equals(y.Label))
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(PresetItem obj)
        {
            return this.Code.GetHashCode();
        }
    }
    public sealed partial class MainPage : Page
    {
        public System.Collections.Generic.List<PresetItem> m_Presets = new System.Collections.Generic.List<PresetItem>();
        /// <summary>
        ///  Record Lines and Frames dictionary
        /// </summary>
        EntityElements m_Elements = new EntityElements();
        /// <summary>
        /// Record Line that is not complete
        /// </summary>
        public EntityLine_Model m_TempLine = new EntityLine_Model() { from_Name=null,to_Name=null};
        public MainPage()
        {
            this.InitializeComponent();
            m_TempLine.UIPath = new Windows.UI.Xaml.Shapes.Path();
            m_TempLine.UIPath.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 204, 204, 255));
            m_TempLine.UIPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            m_TempLine.UIPath.StrokeThickness = 4;
            C_MAINGRID.Children.Add(m_TempLine.UIPath);
            m_Presets.Add(new PresetItem() { Label = "EMPTY",Code="" });
            C_PRESETLIST.ItemsSource = m_Presets;
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private void EntityFrame_FrameMove(object sender, dynamic e)
        {

            String e_name= e.ENTITYNAME;
            var entity= (EntityFrame_Model)m_Elements.m_Frames[e_name];
            System.Numerics.Vector3 p = entity.UIEntity.Translation;
            foreach(object t_line_key in entity.m_Lines.Keys)
            {
                String line_name = (String)entity.m_Lines[t_line_key];
                EntityLine_Model line = (EntityLine_Model)m_Elements.m_Lines[line_name];
                if (line.from_Name.Equals(entity.Name))
                {
                    line.from_point.X = (int)p.X;
                    line.from_point.Y = (int)p.Y;
                    line.from_point.Y += EntityFrame_Model.SlotIndexToY(line.from_index);
                    line.from_point.X += 300 * 1-8;
                }
                if (line.to_Name.Equals(entity.Name))
                {
                    line.to_point.X = (int)p.X;
                    line.to_point.Y = (int)p.Y;
                    line.to_point.Y += EntityFrame_Model.SlotIndexToY(line.to_index);
                    line.to_point.X += 300 * 0+8;
                }

                
                

                var pathGeometry1 = new PathGeometry();
                var pathFigureCollection1 = new PathFigureCollection();
                var pathFigure1 = new PathFigure();
                pathFigure1.IsClosed = false;
                pathFigure1.StartPoint = new Windows.Foundation.Point(line.from_point.X, line.from_point.Y);
                pathFigureCollection1.Add(pathFigure1);
                pathGeometry1.Figures = pathFigureCollection1;

                var pathSegmentCollection1 = new PathSegmentCollection();
                var pathSegment1 = new BezierSegment();
                pathSegment1.Point1 = new Point(line.from_point.X + 100, line.from_point.Y);
                pathSegment1.Point2 = new Point(line.to_point.X - 100, line.to_point.Y);
                pathSegment1.Point3 = new Point(line.to_point.X, line.to_point.Y);
                pathSegmentCollection1.Add(pathSegment1);

                pathFigure1.Segments = pathSegmentCollection1;


                line.UIPath.Data = pathGeometry1;


            }


          


          

        }
        
        private void OnEntitySlotClicked(object sender, dynamic e)
        {
            m_TempLine.UIPath.Visibility = Visibility.Collapsed;

            if (m_TempLine.from_Name == null || m_TempLine.to_Name == null)
            {
                String slottype = e.TYPE;
                if (slottype.Equals("OUTPUTSLOT"))
                {
                    m_TempLine.from_Name = e.ENTITYNAME;
                    m_TempLine.from_index = e.SLOT;
                    m_TempLine.from_SlotName = e.SLOTNAME;
                    var frame_from = (EntityFrame_Model)m_Elements.m_Frames[m_TempLine.from_Name];

                    string from_slot_name = m_TempLine.from_SlotName;//frame_from.UIEntity.m_OutputSlot[m_TempLine.from_index];
                    

                    //if this slot is linked with a line ,delete it
                    if (frame_from.m_Lines.Contains(from_slot_name))
                    {
                        var from_line_name = (String)frame_from.m_Lines[from_slot_name];
                        var from_line = (EntityLine_Model)m_Elements.m_Lines[from_line_name];
                        DeleteLine(from_line.Name);
                    }
                }
                if (slottype.Equals("INPUTSLOT"))
                {
                    m_TempLine.to_Name = e.ENTITYNAME;
                    m_TempLine.to_index = e.SLOT;
                    m_TempLine.to_SlotName = e.SLOTNAME;
                    var frame_to = (EntityFrame_Model)m_Elements.m_Frames[m_TempLine.to_Name];
                    string to_slot_name = m_TempLine.to_SlotName;//frame_to.UIEntity.m_InputSlot[m_TempLine.to_index];

                    //if this slot is linked with a line ,delete it
                    if (frame_to.m_Lines.Contains(to_slot_name))
                    {                       
                        var to_line_name = (String)frame_to.m_Lines[to_slot_name];
                        var to_line = (EntityLine_Model)m_Elements.m_Lines[to_line_name];
                        DeleteLine(to_line.Name);
                    }

                }


            }

            if (m_TempLine.from_Name != null && m_TempLine.to_Name != null)
            {

                AddLine(m_TempLine);
                // clear m_Templeteline
                m_TempLine.to_Name = null;
                m_TempLine.from_Name = null;
            }

        }
        public void OnEntityDeleteClicked(object sender, dynamic e)
        {
            String e_name = e.ENTITYNAME;
            DeleteFrame(e_name);
        }
        public void DeleteFrame(String Name)
        {
            String e_name = Name;
            var entity = (EntityFrame_Model)m_Elements.m_Frames[e_name];
            System.Numerics.Vector3 p = entity.UIEntity.Translation;
            var key_list = new System.Collections.ArrayList();
            foreach (String i in entity.m_Lines.Values)
            {
                key_list.Add(i);
            }

            foreach (String t_line_key in key_list)
            {
                EntityLine_Model line = (EntityLine_Model)m_Elements.m_Lines[t_line_key];
                DeleteLine(line.Name);
            }
            this.C_MAINGRID.Children.Remove(entity.UIEntity);
            m_Elements.m_Frames.Remove(entity.Name);

        }

        public void OnEntityPointerEnter(object sender, dynamic e)
        {
            String e_name = e.ENTITYNAME;
            var entity = (EntityFrame_Model)m_Elements.m_Frames[e_name];
            System.Numerics.Vector3 p = entity.UIEntity.Translation;
            

        }
       
        public void OnEntityRefresh(object sender, dynamic e)
        {
            String e_name = e.ENTITYNAME;
            var entity = (EntityFrame_Model)m_Elements.m_Frames[e_name];
            System.Numerics.Vector3 p = entity.UIEntity.Translation;
            var key_list = new System.Collections.ArrayList();
            foreach (String i in entity.m_Lines.Values)
            {
                key_list.Add(i);
            }

            foreach (String t_line_key in key_list)
            {
                EntityLine_Model line = (EntityLine_Model)m_Elements.m_Lines[t_line_key];
                DeleteLine(line.Name);
            }
            //this.C_MAINGRID.Children.Remove(entity.UIEntity);
            //m_Elements.m_Frames.Remove(entity.Name);
        }
        /// <summary>
        /// Delete  A Specific Line
        /// </summary>
        /// <param name="name"> Line's name</param>
        /// 

        private void DeleteLine(String name)
        {
            var line= (EntityLine_Model)m_Elements.m_Lines[name];
            ///

            if (line == null) return;

            var frame_from = (EntityFrame_Model)m_Elements.m_Frames[line.from_Name];
            var frame_to = (EntityFrame_Model)  m_Elements.m_Frames[line.to_Name];
            string from_slot_name = frame_from.UIEntity.m_OutputSlot[line.from_index];
            string to_slot_name = frame_to.UIEntity.m_InputSlot[line.to_index];
            

            frame_from.m_Lines.Remove(from_slot_name);
            frame_to.m_Lines.Remove(to_slot_name);
            m_Elements.m_Frames[line.from_Name]= frame_from;
            m_Elements.m_Frames[line.to_Name]= frame_to;
            m_Elements.m_Lines.Remove(line.Name);
            this.C_MAINGRID.Children.Remove(line.UIPath);
        }
        private EntityLine_Model AddLine(EntityLine_Model EntityLine,String LineName=null)
        {
            var frame_from = (EntityFrame_Model)m_Elements.m_Frames[EntityLine.from_Name];
            var frame_to = (EntityFrame_Model)m_Elements.m_Frames[EntityLine.to_Name];

            string from_slot_name = frame_from.UIEntity.m_OutputSlot[EntityLine.from_index];
            string to_slot_name = frame_to.UIEntity.m_InputSlot[EntityLine.to_index];

            //TODO:/// DELETE Existing Line
            if (frame_from.m_Lines.Contains(from_slot_name))
            {
                var from_line_name = (String)frame_from.m_Lines[from_slot_name];
                DeleteLine(from_line_name);
            }
            if (frame_to.m_Lines.Contains(to_slot_name))
            {
                var to_line_name = (String)frame_to.m_Lines[to_slot_name];
                DeleteLine(to_line_name);
            }
            ////// Refresh Frame
            frame_from = (EntityFrame_Model)m_Elements.m_Frames[EntityLine.from_Name];
            frame_to = (EntityFrame_Model)m_Elements.m_Frames[EntityLine.to_Name];

            ///// Fill Line Params
            var line = new EntityLine_Model();
            line.Name = "LINE" + EntityElements.RandToken();
            if(LineName!=null)
            {
                line.Name = LineName;
            }
            line.from_Name = EntityLine.from_Name;
            line.to_Name = EntityLine.to_Name;
            line.from_SlotName = from_slot_name;
            line.to_SlotName = to_slot_name;
            line.from_point = new System.Drawing.Point((int)frame_from.UIEntity.Translation.X, (int)frame_from.UIEntity.Translation.Y);
            line.to_point = new System.Drawing.Point((int)frame_to.UIEntity.Translation.X, (int)frame_to.UIEntity.Translation.Y);
            line.from_index = EntityLine.from_index;
            line.to_index = EntityLine.to_index;
            line.from_point.Y += EntityFrame_Model.SlotIndexToY(line.from_index);
            line.to_point.Y += EntityFrame_Model.SlotIndexToY(line.to_index);
            line.from_point.X += 300 * 1 - 8;
            line.to_point.X += 300 * 0 + 8;
            Windows.UI.Xaml.Shapes.Path t_path = new Windows.UI.Xaml.Shapes.Path();


            var pathGeometry1 = new PathGeometry();
            var pathFigureCollection1 = new PathFigureCollection();
            var pathFigure1 = new PathFigure();
            pathFigure1.IsClosed = false;
            pathFigure1.StartPoint = new Windows.Foundation.Point(line.from_point.X, line.from_point.Y);
            pathFigureCollection1.Add(pathFigure1);
            pathGeometry1.Figures = pathFigureCollection1;

            var pathSegmentCollection1 = new PathSegmentCollection();
            var pathSegment1 = new BezierSegment();
            pathSegment1.Point1 = new Point(line.from_point.X + 100, line.from_point.Y);
            pathSegment1.Point2 = new Point(line.to_point.X - 100, line.to_point.Y);
            pathSegment1.Point3 = new Point(line.to_point.X, line.to_point.Y);
            pathSegmentCollection1.Add(pathSegment1);

            pathFigure1.Segments = pathSegmentCollection1;

            t_path.Name = line.Name;
            t_path.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 204, 204, 255));
            t_path.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            t_path.StrokeThickness = 4;
            t_path.Data = pathGeometry1;
            line.UIPath = t_path;
            m_Elements.m_Lines[line.Name] =line;

            frame_from.m_Lines[from_slot_name]= line.Name;
            frame_to.m_Lines[to_slot_name]= line.Name;
            m_Elements.m_Frames[EntityLine.from_Name] = frame_from;
            m_Elements.m_Frames[EntityLine.to_Name] = frame_to;


            this.C_MAINGRID.Children.Add(t_path);

            return line;
        }

        private EntityFrame_Model AddFrame(String EntityFrameName=null)
        {
            
            var entity = new EntityFrame_Model();
            entity.Name = "ENTITY_" + EntityElements.RandToken();
            if(EntityFrameName != null)
            {
                if(m_Elements.m_Frames.ContainsKey(EntityFrameName))
                {
                    DeleteFrame(EntityFrameName);
                }
                entity.Name= EntityFrameName;
            }
            entity.UIEntity = new EntityFrame();
            entity.UIEntity.Width = 300;
            entity.UIEntity.Height = 400;
            entity.UIEntity.Translation = new System.Numerics.Vector3(0, 0, 0);

            entity.UIEntity.m_Name = entity.Name;
            entity.UIEntity.e_SlotClicked += this.OnEntitySlotClicked;
            entity.UIEntity.e_EntityMoved += this.EntityFrame_FrameMove;
            entity.UIEntity.e_Deletelicked += this.OnEntityDeleteClicked;
            entity.UIEntity.e_RefreshFrame += this.OnEntityRefresh;
            //entity.UIEntity.e_PointerExit += this.OnEntityPointerExit;
            //entity.UIEntity.e_PointerEnter += this.OnEntityPointerEnter;
            //entity.UIEntity.Shadow = new ThemeShadow();
            //entity.UIEntity.
            this.C_MAINGRID.Children.Add(entity.UIEntity);

            m_Elements.m_Frames[entity.Name]= entity;
            return entity;
        }

        private  void C_BUTTON_ADD_ENTITY_Click(object sender, RoutedEventArgs e)
        {
            var entity=  AddFrame();
            if(C_PRESETLIST.SelectedIndex!=-1)
            {
                String code = m_Presets[C_PRESETLIST.SelectedIndex].Code;
                if(code!=null && !code .Equals(""))
                {
                    entity.UIEntity.SetCodeBox(code);
                    entity.UIEntity.Decode_Talker();
                }
            }

        }
        public String Serialize()
        {
            Serial.EntitySerialize serialize = new Serial.EntitySerialize();
            serialize.Frames = new List<Serial.S_Frame_Model>(); ;
            serialize.Lines = new List<Serial.S_Line_Model>();
            
            foreach (EntityFrame_Model f in m_Elements.m_Frames.Values)
            {
                var s_f = new Serial.S_Frame_Model()
                {
                    Pos_X = (int)f.UIEntity.Translation.X,
                    Pos_Y = (int)f.UIEntity.Translation.Y,
                    Code = f.UIEntity.m_MetaText,
                    Name = f.Name,
                };
                int s_f_paramcount = f.UIEntity.GetParamCount();
                for(int i=0;i<s_f_paramcount;i++)
                {
                    s_f.Params.Add(f.UIEntity.GetParam(i));
                }

                serialize.Frames.Add(s_f);
            }


            foreach (EntityLine_Model f in m_Elements.m_Lines.Values)
            {


                serialize.Lines.Add(new Serial.S_Line_Model()
                {

                    Name = f.Name,
                    from_index = f.from_index,
                    from_Name = f.from_Name,
                    from_SlotName = f.from_SlotName,
                    to_index = f.to_index,
                    to_Name = f.to_Name,
                    to_SlotName = f.to_SlotName,
                    
                }) ;
            }
            String json_str = Serial.JsonHelper.ToJson(serialize, serialize.GetType());
            return json_str;

        }
        public String Deserialize(String json_str)
        {
            Serial.EntitySerialize serialize = (Serial.EntitySerialize)Serial.JsonHelper.FromJson(json_str, typeof(Serial.EntitySerialize));
           
            foreach (Serial.S_Frame_Model f in serialize.Frames)            
            {
                var entity = AddFrame(f.Name);
                entity.UIEntity.Translation = new System.Numerics.Vector3(f.Pos_X, f.Pos_Y, 0);
                entity.UIEntity.SetCodeBox(f.Code);
                entity.UIEntity.Decode_Talker();
                if(f.Params==null)
                {
                    f.Params = new List<string>();
                }
                int f_paramcount = f.Params.Count;
                for (int i = 0; i < f_paramcount; i++)
                {
                    entity.UIEntity.SetParam(i,f.Params[i]);
                }

            }


            foreach (Serial.S_Line_Model f in serialize.Lines)
            {
                AddLine(new EntityLine_Model()
                {
                    Name = f.Name,
                    from_index = f.from_index,
                    from_Name = f.from_Name,
                    from_SlotName = f.from_SlotName,
                    to_index = f.to_index,
                    to_Name = f.to_Name,
                    to_SlotName = f.to_SlotName,

                }, f.Name);

               
            }
            //String json_str = Serial.JsonHelper.ToJson(serialize, serialize.GetType());
            return json_str;

        }
        private async void C_BUTTON_SAVE_ENTITY_Click(object sender, RoutedEventArgs e)
        {
            String json_str=Serialize();
            var picker=new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation =    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            picker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
            // Default file name if the user does not type one in or select a file to replace
            picker.SuggestedFileName = "NewDocument";
            var t_storagefile= await picker.PickSaveFileAsync();
            if(t_storagefile==null)
            {
                return;
            }
            using (StorageStreamTransaction transaction = await t_storagefile.OpenTransactedWriteAsync())
            {
                using (Windows.Storage.Streams.DataWriter dataWriter = new Windows.Storage.Streams.DataWriter(transaction.Stream))
                {
                    
                    dataWriter.WriteString(json_str);
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.Stream.FlushAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        private async void C_BUTTON_LOAD_ENTITY_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            picker.FileTypeFilter.Add(".json");
            // Default file name if the user does not type one in or select a file to replace
            var t_storagefile = await picker.PickSingleFileAsync();
            if (t_storagefile == null) return;
            String json_str;
            using (StorageStreamTransaction transaction = await t_storagefile.OpenTransactedWriteAsync())
            {
                using (Windows.Storage.Streams.DataReader dataReader = new Windows.Storage.Streams.DataReader(transaction.Stream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)transaction.Stream.Size);
                    json_str = dataReader.ReadString(numBytesLoaded);

                    await transaction.Stream.FlushAsync();
                    await transaction.CommitAsync();
                }
            }
            Deserialize(json_str);

        }

        private async void C_BUTTON_GENCODE_ENTITY_Click(object sender, RoutedEventArgs e)
        {
            String res=m_Elements.GenFunctions();
            
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            picker.FileTypeChoices.Add("cs", new List<string>() { ".cs" });
            // Default file name if the user does not type one in or select a file to replace
            picker.SuggestedFileName = "gencode";
            var t_storagefile = await picker.PickSaveFileAsync();
            if (t_storagefile == null)
            {
                return;
            }
            using (StorageStreamTransaction transaction = await t_storagefile.OpenTransactedWriteAsync())
            {
                using (Windows.Storage.Streams.DataWriter dataWriter = new Windows.Storage.Streams.DataWriter(transaction.Stream))
                {

                    dataWriter.WriteString(res);
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.Stream.FlushAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        private void C_BUTTON_IMPORT_ENTITY_Click(object sender, RoutedEventArgs e)
        {
            C_MAINGRID.Children.Clear();
            m_Elements.m_Lines.Clear();
            m_Elements.m_Frames.Clear();
        }

        private  async void Page_KeyDown(object sender, Windows.System.VirtualKey e)
        {
            if(e==Windows.System.VirtualKey.RightShift)
            {
                //C_MAINSPLITVIEW.IsPaneOpen = true;
            }
        }

        private async void Page_KeyUp(object sender, Windows.System.VirtualKey e)
        {
            if (e == Windows.System.VirtualKey.Shift)
            {
              //  C_MAINSPLITVIEW.IsPaneOpen = !C_MAINSPLITVIEW.IsPaneOpen;
            }
        }

        private async void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args)
        {
           if(  args.EventType.ToString().Equals("KeyDown"))
            {
                Page_KeyDown(sender, args.VirtualKey);
            }
            if (args.EventType.ToString().Equals("KeyUp"))
            {
                Page_KeyUp(sender, args.VirtualKey);
                
            }
        }

        private void C_BUTTONADDPRESET_Click(object sender, RoutedEventArgs e)
        {
            String code = C_TEXTADDPRESET.Text;
            int i = code.IndexOf("\r");
            String Label = code.Substring(0, i);
            if (Label.Contains("@"))
            {
                Label = Label.Substring(1);
            }
            C_PRESETLIST.ItemsSource = null;
            m_Presets.Add(new PresetItem()
            {
                Label = Label,
                Code = code,
            }) ;
            C_PRESETLIST.ItemsSource = m_Presets;
        }

        private void C_PRESETLIST_BUTTON_DELETE_Click(object sender, RoutedEventArgs e)
        {
            Button button=(Button)sender;
            PresetItem pi= (PresetItem)button.DataContext;
            C_PRESETLIST.ItemsSource = null;
            m_Presets.Remove(pi);
            C_PRESETLIST.ItemsSource = m_Presets;
        }

        private async void C_BUTTONLOADPRESET_Click(object sender, RoutedEventArgs e)
        {
             LoadPresets();
        }

        private void C_BUTTONSAVEPRESET_Click(object sender, RoutedEventArgs e)
        {
            SavePresets();
        }
    }
}
