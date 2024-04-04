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
            var Student = new Student();
            this.Hide();
            Student.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Student = new Student();
            this.Hide();
            Student.ShowDialog();
            this.Show();
        }
    }
}