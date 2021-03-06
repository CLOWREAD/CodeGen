﻿using System;
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
    public class PresetItem : IComparable, IEqualityComparer<PresetItem>
    {
        public String Label = "";
        public String Code = "";



        public int CompareTo(object obj)
        {
            PresetItem o = (PresetItem)obj;
            if (o.Code.Equals(this.Code) && o.Label.Equals(this.Label))
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

        public async void LoadPresets()
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
            List<PresetItem> list = null;

            try
            {

            list=(List < PresetItem >) Serial.JsonHelper.FromJson(json_str, m_Presets.GetType());
            }catch(Exception e)
            {
                return;
            }
            if(list == null) { return; }

            C_PRESETLIST.ItemsSource = null;
            foreach(PresetItem item in  list)
            {
                m_Presets.Add(item);
            }
            C_PRESETLIST.ItemsSource = m_Presets;
        }
        public async void SavePresets()
        {
            String json_str = Serial.JsonHelper.ToJson(m_Presets,m_Presets.GetType());
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            picker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
            // Default file name if the user does not type one in or select a file to replace
            picker.SuggestedFileName = "GenCodePreset";
            var t_storagefile = await picker.PickSaveFileAsync();
            if (t_storagefile == null)
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

        public System.Collections.Generic.List<PresetItem> m_Presets = new System.Collections.Generic.List<PresetItem>();

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
            });
            C_PRESETLIST.ItemsSource = m_Presets;
        }

        private void C_PRESETLIST_BUTTON_DELETE_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PresetItem pi = (PresetItem)button.DataContext;
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
