﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoRentShop
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        VideoShop rentingSystem = new VideoShop();
        private void Button1_Click(object sender, EventArgs e)
        {
            if(rentingSystem.AuthUser(tbUsername.Text,tbPassword.Text))
            {
                new frmMain().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username or Password is Wrong","Error");
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if(tbUsername.Text!="" && tbPassword.Text!="")
            {
                string username = Convert.ToString(tbUsername.Text);
                string password = Convert.ToString(tbPassword.Text);
                if(rentingSystem.RegUser(username, password))
                {
                    MessageBox.Show("User Added", "Success");
                }else
                {
                    MessageBox.Show("User already Added.");
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

     
    }
}
