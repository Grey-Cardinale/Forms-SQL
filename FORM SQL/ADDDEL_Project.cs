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

namespace FORM_SQL
{
    public partial class ADDDEL_Project : Form
    {

        public List<Specialisation> Specs = new List<Specialisation>();
        public List<Projects> Projects = new List<Projects>();
        public List<StudentFunc> Students = new List<StudentFunc>();
        public bool new_data, old_data, flag2, flag1 = false;
        private List<Link> linkDict = new List<Link>();
        public ADDDEL_Project()
        {
            InitializeComponent();
            button1.Enabled = false;
            button_Ok.Enabled = false;

            textBox4.TextChanged += textBox4_TextChanged;
            textBox1.TextChanged += textBox1_TextChanged;
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
        }

        private void button1_Click_1(object sender, EventArgs e) //del
        {
            int temp1 = Convert.ToInt32(textBox4.Text);


            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert_str = @"DELETE FROM Projects WHERE [Project ID] = " + $" ('{temp1}');";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }

                insert_str = @"DELETE FROM Link WHERE [Proj ID] = " + $" ('{temp1}');";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();

        }
        private void textBox1_Validating(object sender, EventArgs e)
        {
            var temp = textBox1.Text;
            string pattern = @"^[а-яА-ЯёЁіІїЇґҐ]+$";
            errorProvider1.Clear();

            if (!Regex.IsMatch(temp, pattern))
            {
                //MessageBox.Show("Введіть ім'я українською мовою", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBox1.Focus();
                textBox1.SelectAll();
                flag2 = false;
                errorProvider1.SetError(textBox1, "1223");

            }
            else
            {
                flag2 = true;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1_Validating(sender, e);
            button_Ok_Activated(sender, e);

        }
        private void button_Ok_Activated(object sender, EventArgs e)
        {
            if (flag2)
            {
                button_Ok.Enabled = true;
            }
            else
            {
                button_Ok.Enabled = false;
            }
        }
        private void button_Ok_Click(object sender, EventArgs e) //dod
        {
            Projects temp1 = new Projects();
            temp1.Name = textBox1.Text;
            var specTmp = comboBox1.SelectedItem.ToString();
            var tmpSpec = Specs.Find(n => n.Type == specTmp);
            temp1.SpecID = tmpSpec.SpecID;
            temp1.DDL = textBox2.Text;
            temp1.Info = textBox3.Text;

            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert_str = @"INSERT INTO Projects ([Spec ID], Name, DDL, Info) VALUES" + $" ('{temp1.SpecID}', '{temp1.Name}', '{temp1.DDL}', '{temp1.Info}');";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4_Validating(sender, e);
            button1_Activated(sender, e);

        }
        private void textBox4_Validating(object sender, EventArgs e)
        {
            bool numFlag = true;
            flag1 = false;
            foreach (var c in textBox4.Text)
            {
                if (!char.IsDigit(c))
                {
                    numFlag = false; break;
                }
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                numFlag = false;
            }
            if (numFlag)
            {
                var temp = Convert.ToInt32(textBox4.Text);

                flag1 = false;
                bool tmp = false;
                foreach (var s in Projects)
                {
                    if (s.ProjectId == temp)
                    {
                        tmp = true;

                        break;
                    }
                }
                if (tmp)
                {
                    flag1 = true;
                }
                else
                {
                    flag1 = false;
                }
            }


        }
        private void button1_Activated(object sender, EventArgs e)
        {
            if (flag1)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

      
    }



}

