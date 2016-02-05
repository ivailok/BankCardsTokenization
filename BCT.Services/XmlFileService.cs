using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BCT.Services
{
    public class XmlFileService
    {
        private string filename;

        public XmlFileService(string filename)
        {
            this.filename = filename;
        }

        public void Save(object data, Type type)
        {
            XmlSerializer xmlSer = new XmlSerializer(type);
            TextWriter writer = new StreamWriter(filename);
            xmlSer.Serialize(writer, data);
            writer.Close();
        }

        public object Load(Type type)
        {
            XmlSerializer xmlSer = new XmlSerializer(type);
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return xmlSer.Deserialize(fs);
            }
        }
    }
}
