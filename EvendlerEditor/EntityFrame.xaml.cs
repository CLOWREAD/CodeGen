using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace EvendlerEditor
{
    public sealed partial class EntityFrame : UserControl
    {
        public delegate void SlotEventHandler(object sender, dynamic e);
        public event SlotEventHandler EntityMoved;
        public event SlotEventHandler SlotClicked;
        public event SlotEventHandler Deletelicked;
        public event SlotEventHandler e_PointerEnter;
        public event SlotEventHandler e_PointerExit;
        public event SlotEventHandler e_RefreshFrame;
        public String m_Name,m_Label="LABEL";
        public List<String> m_InputSlot=new List<string>();
        public List<String> m_OutputSlot = new List<string>();
        public List<String> m_ParamsName = new List<string>();
        public EntityLine_Model m_TempLine = new EntityLine_Model();
        public String m_CodeText;
        public String m_MetaText;
        public EntityFrame()
        {
            this.InitializeComponent();
        }
        bool m_Pressed=false;
        bool m_EditEnabled = false;
        Point m_OldPoint = new Point(0, 0);
        Pointer m_CapturedPointer;
        System.Numerics.Vector3 m_OldTranslation = new System.Numerics.Vector3(0, 0, 0);
        private void EntityFrameControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.Translation =new System.Numerics.Vector3(this.Translation.X, this.Translation.Y ,256);
            Canvas.SetZIndex(this, 256);
            dynamic d = new System.Dynamic.ExpandoObject();
            d.ENTITYNAME = m_Name;
            if (e_PointerEnter != null)
            {
                e_PointerEnter.Invoke(this, d);

            }
        }

        private void EntityFrameControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if(m_Pressed)
            {
                var t = this.m_OldTranslation;

                t.X = (float)(e.GetCurrentPoint(this).Position.X - m_OldPoint.X + m_OldTranslation.X);
                t.Y = (float)(e.GetCurrentPoint(this).Position.Y - m_OldPoint.Y + m_OldTranslation.Y);
                t.Z = 256;
                //m_OldPoint = e.GetCurrentPoint(this).Position;
                m_OldTranslation =t;
                this.Translation = t;



                var pos = e.GetCurrentPoint(this).Position;
                pos.X = this.Translation.X;
                pos.Y = this.Translation.Y;
                dynamic d = new System.Dynamic.ExpandoObject();
                d.ENTITYNAME = m_Name;
                if (EntityMoved!=null)
                {
                    EntityMoved.Invoke(this, d);

                }
            }
        }

        private void EntityFrameControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            m_Pressed = true;
            m_OldPoint = e.GetCurrentPoint(this).Position;
            m_OldTranslation = this.Translation;
            m_CapturedPointer = e.Pointer;
            C_MAINSTACKVIEW.CapturePointer(e.Pointer);
        }

        private void EntityFrameControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            m_Pressed = false;
            C_MAINSTACKVIEW.ReleasePointerCapture(m_CapturedPointer);
            
        }
        private void EntityFrameControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.Translation = new System.Numerics.Vector3(this.Translation.X, this.Translation.Y, 1);
            Canvas.SetZIndex(this, 1);
            dynamic d = new System.Dynamic.ExpandoObject();
            d.ENTITYNAME = m_Name;
            if (e_PointerExit != null)
            {
                e_PointerExit.Invoke(this, d);

            }
        }

        private void C_BUTTONEDIT_Click(object sender, RoutedEventArgs e)
        {
            if(m_EditEnabled)
            {
                dynamic d = new System.Dynamic.ExpandoObject();
                d.ENTITYNAME = m_Name;
                if (e_RefreshFrame != null)
                {
                    e_RefreshFrame.Invoke(this, d);

                }
                C_CODEBOX.Visibility = Visibility.Collapsed;
                C_BUTTONEDIT.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(240, 240, 240, 240));
                Decode_Talker();
            }
            else
            {
                C_BUTTONEDIT.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(240, 120, 120, 240));
                C_CODEBOX.Visibility = Visibility.Visible;
                this.Height = 400;
                Grid.SetRowSpan(C_CODEBOX, 3);
                C_A_ROWHEIGTH1.Height = new GridLength(200);
                C_A_ROWHEIGTH2.Height = new GridLength(120);
            }
            m_EditEnabled = !m_EditEnabled;
        }
        private void OnSlotButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int slot_i = m_InputSlot.IndexOf(button.Content.ToString());
            int slot_o = m_OutputSlot.IndexOf(button.Content.ToString());
            dynamic d=new System.Dynamic.ExpandoObject();
            d.TYPE = "SLOTCLICKED";
            d.ENTITYNAME = m_Name;
            if(slot_i!=-1)
            {
                d.TYPE = "INPUTSLOT";
                d.SLOT = slot_i;
            }
            if (slot_o != -1)
            {
                d.TYPE = "OUTPUTSLOT";
                d.SLOT = slot_o;
            }

            if (SlotClicked != null)
            {
                this.SlotClicked.Invoke(this, d);
            }


        }

        private void C_BUTTONDELETE_Click(object sender, RoutedEventArgs e)
        {
            dynamic d = new System.Dynamic.ExpandoObject();
            d.TYPE = "DETELECLICKED";
            d.ENTITYNAME = m_Name;

            if ( Deletelicked!= null)
            {
                this.Deletelicked.Invoke(this, d);
            }
        }

        public void SetCodeBox(String code)
        {
            C_CODEBOX.Text=code;
        }
        public String GetParam(int i)
        {
            String res = "";
            if (i< C_PARAMSTACK.Children.Count)
            {
              res=  ((TextBox)C_PARAMSTACK.Children[i]).Text;
              return res;
            }
                return "";
        }
        public int GetParamCount()
        {
            return m_ParamsName.Count;
        }
        public String GetParamByName(String name)
        {
            var index= m_ParamsName.IndexOf(name);
            if(index!=-1)
            {
                return GetParam(index);
            }
            return null;
        }
        public void SetParamByName(String name , String value)
        {
            var index = m_ParamsName.IndexOf(name);
            if (index != -1)
            {
                SetParam(index,value);
            }
           
        }
        public void SetParam(int i ,String value)
        {
            if (i < C_PARAMSTACK.Children.Count)
            {
                var tb = ((TextBox)C_PARAMSTACK.Children[i]);
                tb.Text = value;
;            }
        }
        public void Decode_Talker()
        {

           String code= C_CODEBOX.Text;
           m_MetaText = code;
           int i= code.IndexOf("\r");

            m_InputSlot.Clear();
            m_OutputSlot.Clear();
            m_ParamsName.Clear();
           while (i>0)
            {


                String param= code.Substring(0, i);
                code = code.Substring(i+1);
                i = code.IndexOf("\r");
                if(param.Contains("----"))
                {
                    break;
                }


                if(param.Contains("@"))
                {
                    m_Label = param.Substring(1);
                }

                if (param.Contains(">") && (!param.Contains(">>")))
                {
                    m_OutputSlot.Add( param.Substring(1));
                }
                if (param.Contains("<")&& (!param.Contains("<<")))
                {
                    m_InputSlot.Add(param.Substring(1));
                }
                if (param.Contains("$") && (!param.Contains("<<")))
                {
                    m_ParamsName.Add(param.Substring(1));
                }
                


            }

            m_CodeText = code;
            C_LABEL.Text = m_Label;
            C_INPUTSTACK.Children.Clear();
            /////////////////////////////////////////////////////////
            foreach (String str in m_InputSlot)
            {
                var button = new Button() { Content = str, 
                    HorizontalAlignment = HorizontalAlignment.Stretch, Height = 30, 
                    Margin = new Thickness(4),
                    CornerRadius = new CornerRadius(8)
                };
                button.Click += this.OnSlotButtonClicked;
                C_INPUTSTACK.Children.Add(button);

            }
            C_OUTPUTSTACK.Children.Clear();
            foreach (String str in m_OutputSlot)
            {
                var button = new Button() { Content = str,
                    HorizontalAlignment = HorizontalAlignment.Stretch , Height = 30,
                    Margin = new Thickness(4),
                    CornerRadius = new CornerRadius(8)
                 };
                if(str.Contains("#"))
                {
                    button.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 128, 255, 128));
                }
                button.Click += this.OnSlotButtonClicked;
                C_OUTPUTSTACK.Children.Add(button);

            }
            C_PARAMNAMESTACK.Children.Clear();
            C_PARAMSTACK.Children.Clear();
            foreach (String str in m_ParamsName)
            {
                var label = new TextBlock() {  Text = str, HorizontalAlignment = HorizontalAlignment.Stretch, Height = 36, Margin = new Thickness(4) };
                var textbox = new TextBox() { BorderThickness=new Thickness(1),
                    BorderBrush=new SolidColorBrush(Windows.UI.Color.FromArgb(255,128,128,128)),
                    CornerRadius = new CornerRadius(4),
                    HorizontalAlignment = HorizontalAlignment.Stretch, Height = 36 ,
                    Margin = new Thickness(4) 
                };
                textbox.Name = str;
                C_PARAMNAMESTACK.Children.Add(label);
                C_PARAMSTACK.Children.Add(textbox);
                
            }

            int FrameHeight = 0;
            FrameHeight += 48;
            int FrameHeight_io = Math.Max(m_InputSlot.Count, m_OutputSlot.Count) * 40 + 8;
            int FrameHeight_param = m_ParamsName.Count * 44 +16;
            this.Height = FrameHeight + FrameHeight_io + FrameHeight_param + 18;
            C_A_ROWHEIGTH1.Height = new GridLength(FrameHeight_io);
            C_A_ROWHEIGTH2.Height = new GridLength(FrameHeight_param);
        }


    }
}
