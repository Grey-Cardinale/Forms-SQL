using SSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace FORM_SQL
    {

    public partial class StudentDel : Form
    {
        public List<Dictionary<int, int>> keyValuePairs = new List<Dictionary<int, int>>();
        public List<Specialisation> Specs = new List<Specialisation>();
        public List<StudentFunc> Students = new List<StudentFunc>();
        public List<Projects> Projects = new List<Projects>();
        private bool flag1, flag2, flag3 = false;
       // private Dictionary<int, int> linkDict = new Dictionary<int, int>();
        private List<Link> linkDict = new List<Link> ();
        public StudentDel()
        {
            InitializeComponent();

            button_Ok.Enabled = false;
            textBox1.TextChanged += textBox1_TextChanged;
            DataGridViewTextBoxColumn column0 = new DataGridViewTextBoxColumn();
            column0.HeaderText = "ID";
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.HeaderText = "Name";
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.HeaderText = "Surname";
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.HeaderText = "Age";
            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.HeaderText = "School";
            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.HeaderText = "Spec";
            DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn();
            column6.HeaderText = "Project";

            dataGridView1.Columns.Add(column0);
            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);
            dataGridView1.Columns.Add(column5);
            dataGridView1.Columns.Add(column6);
            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

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

                            //linkDict.Add(Convert.ToInt32(reader["Stud ID"]), Convert.ToInt32(reader["Proj ID"]));
                        }
                    }
                }

            }

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
            foreach (var s in Students)
            {
                var SpecName = (from sp in Specs
                                where sp.SpecID == s.SpecID
                                select sp.Type).FirstOrDefault();
                string ProjectName = "";
                int tmpProjectID = -1;
                int StudId = s.Student_Id;
                foreach (Dictionary<int, int> elems in keyValuePairs){
                    if (elems.ContainsKey(StudId))
                    {
                        tmpProjectID = elems[StudId];
                    }
                }
                for(int i = 0; i < Projects.Count; i++) {
                    if (Projects[i].ProjectId == tmpProjectID)
                    {
                        ProjectName = Projects[i].Name;
                    }
                }
                //var ProjectName = (from pr in Projects
                //                   where pr.ProjectId == s.ProjID
                //                   select pr.Name).FirstOrDefault();
                Console.WriteLine(SpecName);

                object[] rowData = { s.Student_Id, s.Name, s.SecondName, s.Age, s.School, SpecName.ToString(), ProjectName.ToString() };
                //label6.Text += $"{s.Name}, {s.SecondName}, {s.Age}, {s.School}, {s.SpecID}, {s.ProjID}\n";
                dataGridView1.Rows.Add(rowData);
            }

        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            int temp1 = Convert.ToInt32(textBox1.Text);

            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";
            //відкрити таблицю лінк (записати дані у словник(інт:інт))
            //знайти ай ді студента
            //якщо ай ді є виконати деліт
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
     
            {
                connection.Open();
                foreach (var elem in linkDict) {
                    if (elem.StudID == temp1)
                    {
                        string delete_str = @"DELETE FROM Link WHERE [Stud ID] = " + $" '{temp1}';";
                        using (SQLiteCommand command = new SQLiteCommand(delete_str, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                        break;
                    }
                }

                string insert_str = @"DELETE FROM Students WHERE [Studen ID] = " + $" '{temp1}';";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1_Validating(sender, e);
            button_Ok_Activated(sender, e);

        }
        private void textBox1_Validating(object sender, EventArgs e)
        {
            flag1 = false;
            var temp = textBox1.Text;
            bool testNums = Regex.IsMatch(temp, @"^[0-9]+$");
            if (temp != "" && temp != " " && testNums){
                StudentFunc TmpIdSearch = Students.FirstOrDefault(p => p.Student_Id == Convert.ToInt32(temp));
                if (TmpIdSearch != null)
                {
                    flag1 = true;
                }
                else
                {
                    flag1 = false;
                } 
            }

        }
        private void button_Ok_Activated(object sender, EventArgs e)
        {
            if (flag1) { 
                button_Ok.Enabled = true;
            }
            else
            {
                button_Ok.Enabled = false;
            }
        }
    }
}
