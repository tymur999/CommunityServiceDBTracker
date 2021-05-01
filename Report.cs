using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CommunityServiceDBTracker
{
    [Serializable]
    public class Report
    {
        [Browsable(false)]
        public string ReportName { get; set; }
        
        [Browsable(true), ReadOnly(false)]
        public string ReportTitle { get; set; }
        [Browsable(false)]
        public string QueryText{ get; set; }
        [Browsable(true)]
        [ReadOnly(false)]
        public List<QueryParameter> Parameters { get; set; } = new List<QueryParameter>();

        public static string ObjectToXMLGeneric<T>(T filter)
        {

            string xml = null;
            using (StringWriter sw = new StringWriter())
            {

                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(sw, filter);
                try
                {
                    xml = sw.ToString();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return xml;
        }

        public static T XMLToObject<T>(string xml)
        {

            var serializer = new XmlSerializer(typeof(T));

            using (var textReader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(textReader);
            }
        }
    }

}

