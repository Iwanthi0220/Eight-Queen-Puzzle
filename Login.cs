using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace test
{
    public partial class Login : Form
    {
        // private string connectionString = "Data Source=DESKTOP-BDQFKCF;Initial Catalog=EightQueen;Integrated Security=True";
        public static string SetValueForText = "";


        public Login()
        {
            InitializeComponent();
        }

         
        

        private void btnSignUp_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            SetValueForText = txtUsername.Text;

            Form1 form = new Form1();
            form.Show();
            this.Hide();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }
    
}

   

