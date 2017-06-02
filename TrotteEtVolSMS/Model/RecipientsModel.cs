using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrotteEtVolSMS
{
    class RecipientsModel
    {

        public List<Recipient> Recipients { get; set; }

        public RecipientsModel()
        {
            LoadRecipients();
        }

        private void LoadRecipients()
        {
            Recipients = new List<Recipient>();
            RecipientBuilder builder = new RecipientBuilder();
            string path = ConfigurationManager.AppSettings.Get("recipients");
            using (var fs = File.OpenRead(path))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Recipients.Add(builder.Build(line));
                    }
                }
            }
            
            Recipients.Sort((x, y) => x.Role.CompareTo(y.Role));
            Recipients.Sort((x, y) => x.Team.CompareTo(y.Team));
            
        }

    }
    
}
