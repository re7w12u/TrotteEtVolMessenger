using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TrotteEtVolSMS
{
    static class SerializerHelper<T> where T : class
    {

        internal static bool Serialize(string path, T data)
        {
            try
            {

                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamWriter wr = new StreamWriter(path))
                {
                    xs.Serialize(wr, data);
                }
                return true;
            }
            catch (Exception err)
            {
                return false;
            }

        }

        internal static T Deserialize(string path)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamReader wr = new StreamReader(path))
                {
                    object result = xs.Deserialize(wr);
                    return result == null ? null : (T)result;
                }
            }
            catch (Exception err)
            {
                return null;
            }

        }
    }
}
