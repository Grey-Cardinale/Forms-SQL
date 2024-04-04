using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FORM_SQL
{
    public class Specialisation
    {
        public int SpecID { get; set; }
        public string Type { get; set; }

        public Specialisation()
        {
            SpecID = 0;
            Type = string.Empty;
        }
        public Specialisation(int SpecID, string Type)
        {
            this.SpecID = SpecID;
            this.Type = Type;
        }
    }
}
