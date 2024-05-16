namespace FORM_SQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Student = new Student();
            this.Hide();
            Student.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Student = new SpecForm();
            this.Hide();
            Student.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var a = new ADDDEL_Project();
            this.Hide();
            a.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var a = new LinkForm_();
            this.Hide();
            a.ShowDialog();
            this.Show();
        }
    }
}