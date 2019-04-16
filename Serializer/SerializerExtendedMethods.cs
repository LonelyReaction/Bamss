using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BAMSS.Serializer
{
    public static class SerializerExtendedMethods
    {
        /// <summary>
        /// リストの内容を指定のXMLファイルにシリアライズします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public static void SerializeToXmlFile<T>(this IList<T> list, string filePath)
        {
            var serializer = new XmlSerializer<IList<T>>(filePath);
            serializer.Serialize(list);
        }
        /// <summary>
        /// リストの内容を指定のXMLファイルにシリアライズします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        public static void SerializeToXmlFile<T>(this IList<T> list, string dirPath, string fileName)
        {
            var serializer = new XmlSerializer<IList<T>>(string.Format(@"{0}\{1}.xml", dirPath, fileName));
            serializer.Serialize(list);
        }
        /// <summary>
        /// 指定のXMLファイルからデシリアライズしたデータをリストに追加します。（既存のリストはクリアしません）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public static void DeserializeFromXmlFile<T>(this List<T> list, string filePath)
        {
            if (!File.Exists(filePath)) return;
            var serializer = new XmlSerializer<List<T>>(filePath);
            //if (list != null) list.Clear();
            list.AddRange(serializer.Deserialize());
        }
        /// <summary>
        /// 指定のXMLファイルからデシリアライズしたデータをリストに追加します。（既存のリストはクリアしません）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        public static void DeserializeFromXmlFile<T>(this List<T> list, string dirPath, string fileName)
        {
            var filePath = string.Format(@"{0}\{1}.xml", dirPath, fileName);
            if (!File.Exists(filePath)) return;
            var serializer = new XmlSerializer<List<T>>(filePath);
            //if (list != null) list.Clear();
            list.AddRange(serializer.Deserialize());
        }
    }
}
