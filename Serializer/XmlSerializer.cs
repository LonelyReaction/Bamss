using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BAMSS.Serializer
{
    public class XmlSerializer<T> : ISerializer<T>
    {
        public string LatestDeserializedData { get; private set; }
        public string FilePath { get; private set; }
        public XmlSerializer(string filePath)
        {
            this.FilePath = filePath;
        }
        public void Serialize(T target)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(this.FilePath, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, target);
            sw.Close();
        }
        public T Deserialize()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamReader sr = new System.IO.StreamReader(this.FilePath, new System.Text.UTF8Encoding(false));
            this.LatestDeserializedData = File.ReadAllText(this.FilePath);
            T result = (T)serializer.Deserialize(sr);
            sr.Close();
            return result;
        }
    }
}
