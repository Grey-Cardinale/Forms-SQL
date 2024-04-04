using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORM_SQL
{
    public class StudentFunc
    {
        public int Student_Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public string School { get; set; }  
        public int SpecID { get; set; }
        public int ProjID { get; set; }

        public StudentFunc()
        {
            Student_Id = 0;
            Name = string.Empty;
            SecondName = string.Empty;
            Age = 0;
            School = string.Empty;
            SpecID = 0;
            ProjID = 0;
        }
        public StudentFunc(int Student_Id, string Name, string SecondName, int Age, string School, int SpecID, int ProjID )
        {
            this.Student_Id = Student_Id;
            this.Name = Name;
            this.SecondName = SecondName;
            this.Age = Age;
            this.School = School;
            this.SpecID = SpecID;
            this.ProjID = ProjID;
        }
    }
}
