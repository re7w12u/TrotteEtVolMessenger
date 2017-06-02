using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TrotteEtVolSMS
{
    class HistoryModel
    {

        public BindingList<Message> Messages { get; set; }
        public BindingList<SimpleMessage> SMS { get; set; }

        public HistoryModel()
        {
            LoadMessages();
        }

        private void LoadMessages()
        {
            Messages = new BindingList<Message>();
            string dirName = ConfigurationManager.AppSettings.Get("historyDir");
            if (Directory.Exists(dirName))
            {
                foreach (string fileName in Directory.EnumerateFiles(dirName))
                {
                    Message m = SerializerHelper<Message>.Deserialize(fileName);
                    if (m != null)
                    {
                        Messages.Add(m);
                    }
                }
            }
        }

        internal bool SaveMessage(Message msg)
        {
            // check if directory exists
            string dirName = ConfigurationManager.AppSettings.Get("historyDir");
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);

            // add to list
            Messages.Add(msg);

            // serialize message
            return SerializerHelper<Message>.Serialize(String.Format(@"{0}\{1}.xml",dirName, msg.SendDate.Ticks), msg);                
        }
    }
}
