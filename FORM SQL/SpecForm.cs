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
    public partial class SpecForm : Form
    {
        public List<Specialisation> Specs = new List<Specialisation>();
        public bool new_data, old_data = false;
        public SpecForm()
        {
            InitializeComponent();
            button1.Enabled = false;
            button_Ok.Enabled = false;

            textBox1.TextChanged += textBox1_TextChanged;
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.HeaderText = "Name";


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


                object[] rowData = { s.Type };
                //label6.Text += $"{s.Name}, {s.SecondName}, {s.Age}, {s.School}, {s.SpecID}, {s.ProjID}\n";
                dataGridView1.Rows.Add(rowData);
            }
        }

        private void button1_Click(object sender, EventArgs e) //dod
        {

        }

        private void button_Ok_Click(object sender, EventArgs e) //del
        {

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
                if(s.Type == temp)
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
          if(new_data && !old_data)
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
