using Models;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGOClient
{
    public partial class Inlogform : Form
    {
        MainForm mainForm;
        UserRepository repo = new UserRepository(UserFactory.GetContext(ContextType.MSSQL));
        public User user;
        public Inlogform(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            user = repo.GetByID(repo.LoginWithPass(new User(tbUsername.Text, tbPassword.Text)));

            //To test the rest of the application without a database connection, we create a fake user.
            user = new User(1, "test", false, 999999999);


            if (user != null)
            {
                mainForm.Visible = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Username and password did not match");
            }
        }

        private void Inlogform_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mainForm.Visible == false)
            {
                Application.Exit();
            }
        }
    }
}
