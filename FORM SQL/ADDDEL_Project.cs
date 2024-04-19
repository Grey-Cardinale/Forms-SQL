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

namespace FORM_SQL
{
    public partial class ADDDEL_Project : Form
    {

        public List<Specialisation> Specs = new List<Specialisation>();
        public List<Projects> Projects = new List<Projects>();
        public bool new_data, old_data = false;
        public ADDDEL_Project()
        {
            InitializeComponent();
            button1.Enabled = false;
            button_Ok.Enabled = false;

            textBox1.TextChanged += textBox1_TextChanged;
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.HeaderText = "Project ID";
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.HeaderText = "Spec ID";
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.HeaderText = "Name";
            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.HeaderText = "DDL";
            DataGridViewTextBoxColumn column5 = new DataGridViewTextBoxColumn();
            column5.HeaderText = "Info";
     


            dataGridView1.Columns.Add(column1);
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
            foreach (var s in Projects)
            {
                var SpecName = (from sp in Specs
                                where sp.SpecID == s.SpecID
                                select sp.Type).FirstOrDefault();
                /*
                   public int ProjectId { get; set; }
        public int SpecID { get; set; }
        public string Name { get; set; }
        public string DDL { get; set; }
        public string Info { get; set; }
        public int StudID { get; set; }
                */
                object[] rowData = { s.ProjectId, s.SpecID, s.Name, s.DDL,s.Info};
                //label6.Text += $"{s.Name}, {s.SecondName}, {s.Age}, {s.School}, {s.SpecID}, {s.ProjID}\n";
                dataGridView1.Rows.Add(rowData);
            }
        }

        private void button1_Click(object sender, EventArgs e) //dod
        {
            string temp1 = textBox1.Text;

            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert_str = @"INSERT INTO Spec (Type) VALUES" + $" ('{temp1}');";
                using (SQLiteCommand command = new SQLiteCommand(insert_str, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var ok = new ConfirmForm();
            ok.ShowDialog();
            this.Close();

        }

        private void button_Ok_Click(object sender, EventArgs e) //del
        {
            string temp1 = textBox1.Text;

            string connectionString = "Data Source=C:\\Users\\artem\\Desktop\\projects\\SSQL\\1.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert_str = @"DELETE FROM Spec WHERE Type = " + $" '{temp1}';";
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
            new_data = false;
            old_data = false;
            bool tmp = false;
            foreach (var s in Specs)
            {
                if (s.Type == temp)
                {
                    tmp = true;

                    break;
                }
            }
            if (tmp)
            {
                new_data = false;
                old_data = true;
            }
            else
            {
                new_data = true;
                old_data = false;
            }

        }
        private void button_Ok_Activated(object sender, EventArgs e)
        {
            if (new_data && !old_data)
            {
                button1.Enabled = true;
                button_Ok.Enabled = false;
            }
            else
            {
                button1.Enabled = false;
                button_Ok.Enabled = true;
            }
        }
    }



}

