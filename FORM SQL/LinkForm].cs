using LinqToDB.Common;
using Microsoft.VisualBasic;
using SSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FORM_SQL
{
    public partial class LinkForm_ : Form
    {
        public List<Specialisation> Specs = new List<Specialisation>();
        public List<Projects> Projects = new List<Projects>();
        public List<StudentFunc> Students = new List<StudentFunc>();
        public bool new_data, old_data, flag2, flag1 = false;
        private List<Link> linkDict = new List<Link>();
        public List<Dictionary<int, int>> keyValuePairs = new List<Dictionary<int, int>>();
        bool DodFlag, DelFlag = false;
        public LinkForm_()
        {

            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox2.TextChanged += textBox2_TextChanged;

            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.HeaderText = "Project ";
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.HeaderText = "Spec";
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.HeaderText = "Name";
            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.HeaderText = "DDL";
            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.HeaderText = "Info";
            DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn();
            column6.HeaderText = "Students";


            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);
            dataGridView1.Columns.Add(column5);
            dataGridView1.Columns.Add(column6);

            DataGridViewTextBoxColumn column01 = new DataGridViewTextBoxColumn();
            column01.HeaderText = "ID";
            DataGridViewTextBoxColumn column11 = new DataGridViewTextBoxColumn();
            column11.HeaderText = "Name";
            DataGridViewTextBoxColumn column21 = new DataGridViewTextBoxColumn();
            column21.HeaderText = "Surname";
            DataGridViewTextBoxColumn column31 = new DataGridViewTextBoxColumn();
            column31.HeaderText = "Age";
            DataGridViewTextBoxColumn column41 = new DataGridViewTextBoxColumn();
            column41.HeaderText = "School";
            DataGridViewTextBoxColumn column51 = new DataGridViewTextBoxColumn();
            column51.HeaderText = "Spec";
            DataGridViewTextBoxColumn column61 = new DataGridViewTextBoxColumn();
            column61.HeaderText = "Project";

            dataGridView2.Columns.Add(column01);
            dataGridView2.Columns.Add(column11);
            dataGridView2.Columns.Add(column21);
            dataGridView2.Columns.Add(column31);
            dataGridView2.Columns.Add(column41);
            dataGridView2.Columns.Add(column51);
            dataGridView2.Columns.Add(column61);


            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectData = @"
               SELECT * FROM Spec;
                );";

                using (SQLiteCommand command = new SQLiteCommand(selectData, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Specialisation tmp = new Specialisation(Convert.ToInt32(reader["Spec ID"]), reader["Type"].ToString());
                            Specs.Add(tmp);
                        }
                    }
                }
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectData = @"
               SELECT * FROM Projects;
                );";

                using (SQLiteCommand command = new SQLiteCommand(selectData, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Projects tmp = new Projects(Convert.ToInt32(reader["Project ID"]), Convert.ToInt32(reader["Spec ID"]), reader["Name"].ToString(), reader["DDL"].ToString(), reader["Info"].ToString());
                            Projects.Add(tmp);
                        }
                    }
                }
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectData = @"
               SELECT * FROM Students;
                );";

                using (SQLiteCommand command = new SQLiteCommand(selectData, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentFunc tmp = new StudentFunc(Convert.ToInt32(reader["Studen ID"]), reader["Name"].ToString(), reader["Surname"].ToString(), Convert.ToInt32(reader["Age"]), reader["School"].ToString(), Convert.ToInt32(reader["Spec ID"]));
                            Students.Add(tmp);
                        }
                    }
                }
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectData = @"
               SELECT * FROM Link;
                );";

                using (SQLiteCommand command = new SQLiteCommand(selectData, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Link tmp = new Link(Convert.ToInt32(reader["ID"]), Convert.ToInt32(reader["Stud ID"]), Convert.ToInt32(reader["Proj ID"]));
                            linkDict.Add(tmp);

                        }
                    }
                }

            }
            foreach (var s in Projects)
            {
                var SpecName = (from sp in Specs
                                where sp.SpecID == s.SpecID
                                select sp.Type).FirstOrDefault();
                string studList = "";
                foreach (var elem in linkDict)
                {
                    if (elem.ProjID == s.ProjectId)
                    {
                        var StudName = (from st in Students
                                        where st.Student_Id == elem.StudID
                                        select st.Name).FirstOrDefault();
                        studList += StudName + ", ";
                    }
                }

                object[] rowData = { s.ProjectId, SpecName, s.Name, s.DDL, s.Info, studList };
                //label6.Text += $"{s.Name}, {s.SecondName}, {s.Age}, {s.School}, {s.SpecID}, {s.ProjID}\n";
                dataGridView1.Rows.Add(rowData);
            }
            foreach (var s in Students)
            {
                var SpecName = (from sp in Specs
                                where sp.SpecID == s.SpecID
                                select sp.Type).FirstOrDefault();
                string ProjectName = "";
                int tmpProjectID = -1;
                int StudId = s.Student_Id;
                foreach (Dictionary<int, int> elems in keyValuePairs)
                {
                    if (elems.ContainsKey(StudId))
                    {
                        tmpProjectID = elems[StudId];
                    }
                }
                for (int i = 0; i < Projects.Count; i++)
                {
                    if (Projects[i].ProjectId == tmpProjectID)
                    {
                        ProjectName = Projects[i].Name;
                    }
                }

                Console.WriteLine(SpecName);

                object[] rowData = { s.Student_Id, s.Name, s.SecondName, s.Age, s.School, SpecName.ToString(), ProjectName.ToString() };
                dataGridView2.Rows.Add(rowData);
            }
        }
      

        private void button1_Click(object sender, EventArgs e) //додавання
        {
            var projID = Convert.ToInt32(textBox1.Text) ; //1
            var StudID = Convert.ToInt32(textBox2.Text) ; //2

            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert_str = @"INSERT INTO Link ([Proj ID], [Stud ID]) VALUES" + $" ('{projID}', '{StudID}');";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e) //видалення
        {
            var projID = Convert.ToInt32(textBox1.Text); //1
            var StudID = Convert.ToInt32(textBox2.Text); //2
            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))

            {
                connection.Open();

                string insert_str = @"DELETE FROM Link WHERE [Stud ID] = " + $" '{StudID}'" +" AND [Proj ID] = " + $"'{projID}';";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();
        }
        private void validator (object sender, EventArgs e)
        {
            var projID = textBox1.Text; //1
            var StudID = textBox2.Text; //2
            DodFlag = false;
            DelFlag = false;
            errorProvider1.SetError(textBox1, null);
            errorProvider2.SetError(textBox2, null);
            bool text1Check = false, text2Check = false;


            if (!Information.IsNumeric(projID))
            {
                errorProvider1.SetError(textBox1, "Isnt num");
                
            }

            else
            {
                foreach (var k in Projects)
                {
                    if (k.ProjectId == Convert.ToInt32(projID))
                    {
                        text1Check = true;
                        break;
                    }
                }
            }

            if (!Information.IsNumeric(StudID))
            {
                errorProvider2.SetError(textBox2, "Isnt num");
            }
            else
            {
                foreach (var k in Students)
                {
                    if (k.Student_Id == Convert.ToInt32(StudID))
                    {
                        text2Check = true;
                        break;
                    }
                }

            }


            if (text1Check && text2Check)
            {
                bool tmp = false;

                foreach (var key in linkDict)
                {
                    if (key.StudID == Convert.ToInt32(StudID))
                    {
                        if (key.ProjID == Convert.ToInt32(projID))
                        {
                            tmp = true;
                            DodFlag = false;
                            DelFlag = true;
                            break;
                        }
                    }

                }
                if (!tmp)
                {
                    DodFlag = true;
                    DelFlag = false;
                }
            }
           
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            validator(sender, e);
            buttons_Activated(sender, e);

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            validator(sender, e);
            buttons_Activated(sender, e);

        }
        private void buttons_Activated(object sender, EventArgs e)
        {
            if (DodFlag && !DelFlag)
            {
                button1.Enabled = true;
                button2.Enabled = false;
            }
            else if (!DodFlag && DelFlag)
            {
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

    }
}
