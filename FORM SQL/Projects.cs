using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSQL
{
    public class Projects
    {
        public int ProjectId { get; set; }
        public int SpecID { get; set; }
        public string Name { get; set; }
        public string DDL { get; set; }
        public string Info { get; set; }
        public int StudID { get; set; }

        public Projects() 
        { 
            ProjectId = 0;
            SpecID = 0;
            Name = string.Empty;
            DDL = string.Empty;
            Info = string.Empty;
            StudID = 0;
        }
        public Projects(int ProjectId, int SpecID, string Name, string DDL, string info, int StudID)
        {
            this.ProjectId = ProjectId;
            this.SpecID = SpecID;
            this.Name = Name;
            this.DDL = DDL;
            this.Info = info;
            this.StudID = StudID;
        }

    }
}
