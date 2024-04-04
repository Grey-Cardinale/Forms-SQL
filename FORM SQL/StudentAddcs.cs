using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using LinqToDB.SqlQuery;
using SSQL;

namespace FORM_SQL
{
    public partial class StudentAddcs : Form
    {
        public List<Specialisation> Specs = new List<Specialisation>();
        public List<StudentFunc> Students = new List<StudentFunc>();
        public List<Projects> Projects = new List<Projects>();
        public StudentAddcs()
        {
            InitializeComponent();

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

            foreach (var s in Specs)
            {
                comboBox1.Items.Add(s.Type);
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
                            Projects tmp = new Projects(Convert.ToInt32(reader["Project ID"]), Convert.ToInt32(reader["Spec ID"]), reader["Name"].ToString(), reader["DDL"].ToString(), reader["Info"].ToString(), Convert.ToInt32(reader["Stud ID"]));
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
                            StudentFunc tmp = new StudentFunc(Convert.ToInt32(reader["Studen ID"]), reader["Name"].ToString(), reader["Surname"].ToString(), Convert.ToInt32(reader["Age"]), reader["School"].ToString(), Convert.ToInt32(reader["Spec ID"]), Convert.ToInt32(reader["Project ID"]));
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
                var ProjectName = (from pr in Projects
                                   where pr.ProjectId == s.ProjID
                                   select pr.Name).FirstOrDefault();
                Console.WriteLine(SpecName);

                object[] rowData = { s.Name, s.SecondName, s.Age, s.School, SpecName.ToString(), ProjectName.ToString() };
                //label6.Text += $"{s.Name}, {s.SecondName}, {s.Age}, {s.School}, {s.SpecID}, {s.ProjID}\n";
                dataGridView1.Rows.Add(rowData);
            }
        }

        private void StudentAddcs_Load(object sender, EventArgs e)
        {

        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            StudentFunc temp1 = new StudentFunc();
            temp1.Name = textBox1.Text;
            temp1.SecondName = textBox2.Text;
            temp1.Age = Convert.ToInt32(textBox3.Text);
            temp1.School = textBox4.Text;
            var specTmp = comboBox1.SelectedItem.ToString();
            var tmpSpec = Specs.Find(n => n.Type == specTmp);
            temp1.SpecID = tmpSpec.SpecID;

            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();



        }
    }
}
