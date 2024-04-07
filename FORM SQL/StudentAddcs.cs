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
using System.Text.RegularExpressions;

namespace FORM_SQL
{
    public partial class StudentAddcs : Form
    {
        public List<Specialisation> Specs = new List<Specialisation>();
        public List<StudentFunc> Students = new List<StudentFunc>();
        public List<Projects> Projects = new List<Projects>();
        private bool flag1, flag2, flag3 = false;
        public StudentAddcs()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox3.TextChanged += textBox3_TextChanged;
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


            button_Ok.Enabled = false;
            timer1.Enabled = true;
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
            temp1.ProjID = 1;
            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert_str = @"INSERT INTO Students (Name, Surname, Age, School, [Spec ID], [Project ID]) VALUES" + $" ('{temp1.Name}', '{temp1.SecondName}', '{temp1.Age}', '{temp1.School}', '{temp1.SpecID}', '{temp1.ProjID}');";
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
            var temp = textBox1.Text;
            string pattern = @"^[а-яА-ЯёЁіІїЇґҐ]+$";
            errorProvider1.Clear();

            if (!Regex.IsMatch(temp, pattern))
            {
                //MessageBox.Show("Введіть ім'я українською мовою", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBox1.Focus();
                textBox1.SelectAll();
                flag1 = false;
                errorProvider1.SetError(textBox1, "1223");

            }
            else
            {
                flag1 = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2_Validating(sender, e);
            button_Ok_Activated(sender, e);

        }
        private void textBox2_Validating(object sender, EventArgs e)
        {
            var temp = textBox2.Text;
            string pattern = @"^[а-яА-ЯёЁіІїЇґҐ]+$";
            errorProvider2.Clear();

            if (!Regex.IsMatch(temp, pattern))
            {
                //MessageBox.Show("Введіть ім'я українською мовою", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBox2.Focus();
                textBox2.SelectAll();
                flag2 = false;
                errorProvider2.SetError(textBox2, "1223");

            }
            else
            {
                flag2 = true;
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3_Validating(sender, e);
            button_Ok_Activated(sender, e);

        }

        private void textBox3_Validating(object sender, EventArgs e)
        {
            var temp = textBox3.Text;
            int age;

            // Перевірка чи введене значення може бути конвертоване в число
            if (!int.TryParse(temp, out age))
            {
                // Якщо не вдалося конвертувати введене значення в число, вивести помилку
                flag3 = false;
                errorProvider3.SetError(textBox3, "Введіть коректний вік");
                // e.Cancel = true;
            }
            else
            {
                // Перевірка чи вік в межах припустимого діапазону (наприклад, від 18 до 100)
                if (age < 12 || age > 100)
                {
                    // Якщо вік не в межах припустимого діапазону, вивести помилку
                    flag3 = false;
                    errorProvider3.SetError(textBox3, "Введіть вік від 12 до 100");
                    //e.Cancel = true;
                }
                else
                {
                    // Якщо введене значення відповідає усім умовам, очистити помилку
                    errorProvider3.Clear();

                    flag3 = true;

                }
            }
        }

        private void button_Ok_Activated(object sender, EventArgs e)
        {
            if (flag1 && flag2 && flag3)
            {
                button_Ok.Enabled = true;
            }
            else
            {
                button_Ok.Enabled = false;
            }
        }

    }
}
