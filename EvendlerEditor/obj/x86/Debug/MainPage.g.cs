﻿#pragma checksum "C:\Users\clow_\source\repos\EvendlerEditor\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9EB72F7359536536EE052CC7EB39C1B2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EvendlerEditor
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class MainPage_obj3_Bindings :
            global::Windows.UI.Xaml.IDataTemplateExtension,
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::EvendlerEditor.PresetItem dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private bool removedDataContextHandler = false;

            // Fields for each control that has bindings.
            private global::System.WeakReference obj3;
            private global::Windows.UI.Xaml.Controls.TextBlock obj4;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj4TextDisabled = false;

            public MainPage_obj3_Bindings()
            {
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 16 && columnNumber == 24)
                {
                    isobj4TextDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3: // MainPage.xaml line 14
                        this.obj3 = new global::System.WeakReference((global::Windows.UI.Xaml.Controls.StackPanel)target);
                        break;
                    case 4: // MainPage.xaml line 16
                        this.obj4 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    default:
                        break;
                }
            }

            public void DataContextChangedHandler(global::Windows.UI.Xaml.FrameworkElement sender, global::Windows.UI.Xaml.DataContextChangedEventArgs args)
            {
                 if (this.SetDataRoot(args.NewValue))
                 {
                    this.Update();
                 }
            }

            // IDataTemplateExtension

            public bool ProcessBinding(uint phase)
            {
                throw new global::System.NotImplementedException();
            }

            public int ProcessBindings(global::Windows.UI.Xaml.Controls.ContainerContentChangingEventArgs args)
            {
                int nextPhase = -1;
                ProcessBindings(args.Item, args.ItemIndex, (int)args.Phase, out nextPhase);
                return nextPhase;
            }

            public void ResetTemplate()
            {
                Recycle();
            }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                nextPhase = -1;
                switch(phase)
                {
                    case 0:
                        nextPhase = -1;
                        this.SetDataRoot(item);
                        if (!removedDataContextHandler)
                        {
                            removedDataContextHandler = true;
                            (this.obj3.Target as global::Windows.UI.Xaml.Controls.StackPanel).DataContextChanged -= this.DataContextChangedHandler;
                        }
                        this.initialized = true;
                        break;
                }
                this.Update_((global::EvendlerEditor.PresetItem) item, 1 << phase);
            }

            public void Recycle()
            {
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::EvendlerEditor.PresetItem)newDataRoot;
                    return true;
                }
                return false;
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::EvendlerEditor.PresetItem obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_Label(obj.Label, phase);
                    }
                }
            }
            private void Update_Label(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // MainPage.xaml line 16
                    if (!isobj4TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj4, obj, null);
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 5: // MainPage.xaml line 17
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.C_PRESETLIST_BUTTON_DELETE_Click;
                }
                break;
            case 6: // MainPage.xaml line 30
                {
                    this.C_MAINSPLITVIEW = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 7: // MainPage.xaml line 83
                {
                    this.C_BUTTON_PANEL_SWITCH = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.C_BUTTON_PANEL_SWITCH).Click += this.C_BUTTON_PANEL_SWITCH_Click;
                }
                break;
            case 8: // MainPage.xaml line 85
                {
                    this.C_BUTTON_ADD_ENTITY = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.C_BUTTON_ADD_ENTITY).Click += this.C_BUTTON_ADD_ENTITY_Click;
                }
                break;
            case 9: // MainPage.xaml line 86
                {
                    this.C_BUTTON_SAVE_ENTITY = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.C_BUTTON_SAVE_ENTITY).Click += this.C_BUTTON_SAVE_ENTITY_Click;
                }
                break;
            case 10: // MainPage.xaml line 87
                {
                    this.C_BUTTON_LOAD_ENTITY = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.C_BUTTON_LOAD_ENTITY).Click += this.C_BUTTON_LOAD_ENTITY_Click;
                }
                break;
            case 11: // MainPage.xaml line 88
                {
                    this.C_BUTTON_IMPORT_ENTITY = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.C_BUTTON_IMPORT_ENTITY).Click += this.C_BUTTON_IMPORT_ENTITY_Click;
                }
                break;
            case 12: // MainPage.xaml line 89
                {
                    this.C_BUTTON_GENCODE_ENTITY = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.C_BUTTON_GENCODE_ENTITY).Click += this.C_BUTTON_GENCODE_ENTITY_Click;
                }
                break;
            case 13: // MainPage.xaml line 40
                {
                    this.PaneHeader = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14: // MainPage.xaml line 41
                {
                    this.C_PRESETLIST = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 15: // MainPage.xaml line 49
                {
                    this.C_BUTTONLOADPRESET = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.C_BUTTONLOADPRESET).Click += this.C_BUTTONLOADPRESET_Click;
                }
                break;
            case 16: // MainPage.xaml line 50
                {
                    this.C_BUTTONSAVEPRESET = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.C_BUTTONSAVEPRESET).Click += this.C_BUTTONSAVEPRESET_Click;
                }
                break;
            case 17: // MainPage.xaml line 44
                {
                    this.C_TEXTADDPRESET = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 18: // MainPage.xaml line 45
                {
                    this.C_BUTTONADDPRESET = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.C_BUTTONADDPRESET).Click += this.C_BUTTONADDPRESET_Click;
                }
                break;
            case 19: // MainPage.xaml line 66
                {
                    this.C_MAINGRID = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                    ((global::Windows.UI.Xaml.Controls.Canvas)this.C_MAINGRID).PointerMoved += this.C_MAINGRID_PointerMoved;
                    ((global::Windows.UI.Xaml.Controls.Canvas)this.C_MAINGRID).PointerPressed += this.C_MAINGRID_PointerPressed;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 3: // MainPage.xaml line 14
                {                    
                    global::Windows.UI.Xaml.Controls.StackPanel element3 = (global::Windows.UI.Xaml.Controls.StackPanel)target;
                    MainPage_obj3_Bindings bindings = new MainPage_obj3_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(element3.DataContext);
                    element3.DataContextChanged += bindings.DataContextChangedHandler;
                    global::Windows.UI.Xaml.DataTemplate.SetExtensionInstance(element3, bindings);
                    global::Windows.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element3, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

