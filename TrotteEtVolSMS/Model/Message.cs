using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TrotteEtVolSMS
{
    [Serializable]
    public class Message
    {
        public List<Recipient> Recipients { get; set; }
        [XmlIgnore]
        internal DateTime SendDate { get; set; }
        public string ProxyDate
        {
            get { return SendDate.ToString(); }
            set { SendDate = DateTime.Parse(value); }
        }
        public string Body { get; set; }

    }

    public class SimpleMessage
    {        
        public string Body { get; set; }

    }
}
