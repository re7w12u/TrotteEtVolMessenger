using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrotteEtVolSMS
{

    class RecipientBuilder
    {
        public Recipient Build(string data)
        {
            var values = data.Split(';');
            return new Recipient
            {
                Name = values[0],
                Role = values[1],
                Phone = values[2].Replace(" ",""),
                Team = Convert.ToInt32(values[3]),
                Parcours = values[4]
            };
        }
    }

    [Serializable]
    public class Recipient
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public int Team { get; set; }
        public string Parcours { get; set; }
    }
}
