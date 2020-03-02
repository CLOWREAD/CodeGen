using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace EvendlerEditor.Serial
{
    class EntitySerializeOut
    {
    }
    class EntitySerializeIn
    {
    }
    [System.Runtime.Serialization.DataContract]
    public class EntitySerialize
    {
        [System.Runtime.Serialization.DataMember]
        public List<S_Line_Model> Lines=new List<S_Line_Model>();
        [System.Runtime.Serialization.DataMember]
        public List<S_Frame_Model> Frames=new List<S_Frame_Model>();
        
    }
    [System.Runtime.Serialization.DataContract]
    public class S_Line_Model
    {
        [System.Runtime.Serialization.DataMember]
        public String Name;
        [System.Runtime.Serialization.DataMember]
        public int from_index = 0;
        [System.Runtime.Serialization.DataMember]
        public int to_index = 0;
        [System.Runtime.Serialization.DataMember]
        public String from_SlotName = "";
        [System.Runtime.Serialization.DataMember]
        public String to_SlotName = "";
        [System.Runtime.Serialization.DataMember]
        public String from_Name = "";
        [System.Runtime.Serialization.DataMember]
        public String to_Name = "";
    }
    [System.Runtime.Serialization.DataContract]
    public class S_Frame_Model
    {
        [System.Runtime.Serialization.DataMember]
        public String Name;


        [System.Runtime.Serialization.DataMember]
        public int Pos_X;
        [System.Runtime.Serialization.DataMember]
        public int Pos_Y;
        [System.Runtime.Serialization.DataMember]
        public String Code;
        [System.Runtime.Serialization.DataMember]
        public List<String> Params=new List<string>();

    }
    public class JsonHelper
    {
        public static string ToJson(Object obj, Type type)
        {

            MemoryStream ms = new MemoryStream();

            DataContractJsonSerializer seralizer = new DataContractJsonSerializer(type);


            seralizer.WriteObject(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);

            StreamReader sr = new StreamReader(ms);
            string jsonstr = sr.ReadToEnd();

            //jsonstr = jsonstr.Replace("\"", "\\\"");

            sr.Close();
            ms.Close();
            return jsonstr;
        }
        public static Object FromJson(String jsonstr, Type type)
        {

            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonstr));

            DataContractJsonSerializer seralizer = new DataContractJsonSerializer(type);

            ms.Seek(0, SeekOrigin.Begin);

            Object res = seralizer.ReadObject(ms);


            ms.Close();
            return res;
        }

    }
}
