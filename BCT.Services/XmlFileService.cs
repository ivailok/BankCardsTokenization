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
        private XmlSerializer xmlSer;

        public XmlFileService(string filename, Type type)
        {
            this.filename = filename;
            this.xmlSer = new XmlSerializer(type);
        }

        public void Save(object data)
        {
            using (TextWriter writer = new StreamWriter(filename))
            {
                this.xmlSer.Serialize(writer, data);
            }
        }

        public object Load()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return this.xmlSer.Deserialize(fs);
            }
        }
    }
}
