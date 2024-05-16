using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORM_SQL
{
    public class Link
    {
        
            public int ID;
            public int StudID;
            public int ProjID;

            public Link(int ID, int StudID, int ProjID)
            {
                this.ID = ID;
                this.StudID = StudID;
                this.ProjID = ProjID;
            }
      
    }
}
