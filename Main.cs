using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace VideoRentShop
{
    public partial class frmMain : Form
    {
        // create an instance of class movie shop

        VideoShop movieShop = new VideoShop();
  
        public frmMain()
        {
            InitializeComponent();
  
        }
        private void populate_customer_view(DataTable d)
        {
            lvCustomers.Items.Clear();

            for (int i =0; i<d.Rows.Count;i++)
            {
            
               
                lvCustomers.Items.Add(d.Rows[i].ItemArray[0].ToString());
                lvCustomers.Items[i].SubItems.Add(d.Rows[i].ItemArray[1].ToString());
                lvCustomers.Items[i].SubItems.Add(d.Rows[i].ItemArray[2].ToString());
                lvCustomers.Items[i].SubItems.Add(d.Rows[i].ItemArray[3].ToString());
                lvCustomers.Items[i].SubItems.Add(d.Rows[i].ItemArray[4].ToString());
            }
            
        
        }
        private void populate_movies_view(DataTable d)
        {
            lvMovies.Items.Clear();

            foreach (DataRow row in d.Rows)
            {
                string[] items = { row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(),row[5].ToString(),row[6].ToString(),row[7].ToString() };
                lvMovies.Items.Add(new ListViewItem(items));
            }

        }
        private void populate_rents_view(DataTable d)
        {
            lvRentedMovies.Items.Clear();
            foreach (DataRow row in d.Rows)
            {
                string[] items = { row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(),row[5].ToString() };
                lvRentedMovies.Items.Add(new ListViewItem(items));
            }

        }
        private void BtnUpdateCust_Click(object sender, EventArgs e)
        {
            if (tbCustFirstName.Text != "" && tbCustLastName.Text != "" && tbCustAddress.Text != "" && tbCustPhone.Text != "")
            {
                string firstname = tbCustFirstName.Text;
                string lastname = tbCustLastName.Text;
                string address = tbCustAddress.Text;
                string phone = tbCustPhone.Text;
                int custId = Convert.ToInt32(tbCustID.Text);
                movieShop.update_customer(custId, firstname, lastname, address, phone);
                populate_customer_view(movieShop.get_customers());
                clear_customer_boxes();    
            } else
            {
                MessageBox.Show("Ensure customer fields are not empty");
            }
        }
        // this method clears text boxes created for customer view
        private void clear_customer_boxes()
        {
            tbCustID.Text = "";
            tbCustFirstName.Text = "";
            tbCustAddress.Text = "";
            tbCustLastName.Text = "";
            tbCustPhone.Text = "";
        }
        private void BtnAddCust_Click(object sender, EventArgs e)
        {
            if (tbCustFirstName.Text != "" && tbCustLastName.Text != "" && tbCustAddress.Text != "" && tbCustPhone.Text != "")
            {
        
                movieShop.add_customer(tbCustFirstName.Text, tbCustLastName.Text, tbCustAddress.Text, tbCustPhone.Text);
                populate_customer_view(movieShop.get_customers()); 
                clear_customer_boxes();
            }
            else
            {
                MessageBox.Show("ensure customer fields are not empty");
            }
        }

        private void BtnDeleteCust_Click(object sender, EventArgs e)
        {
            if(tbCustID.Text!="")
            {
                // confirm dialog for deletion
                int custId = Convert.ToInt32(tbCustID.Text);
                DialogResult mbresult = MessageBox.Show("confirm delete?", "customer", MessageBoxButtons.YesNo);
                if(mbresult.ToString()=="Yes")
                {
                    movieShop.delete_customer(custId);
                    MessageBox.Show("Record Deleted!");
                    populate_customer_view(movieShop.get_customers());
                    clear_customer_boxes();
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // refresh data in customer, movies and rented movies list view
         
            populate_customer_view(movieShop.get_customers());
            populate_movies_view(movieShop.ListMovies());
            populate_rents_view(movieShop.ListRentedMovies()); // update list view

            timerLogo.Start();
        }

    
        // method to clear text boxes created for movies view
        private void clear_movies_boxes()
        {
            tbMovieId.Text = ""; tbMovieName.Text = ""; tbMovieGenre.Text = ""; tbMovieRating.Text = ""; tbMovieYear.Text = ""; tbMovieCopies.Text = ""; tbMoviePlot.Text = "";
        }
        private void BtnAddMovie_Click(object sender, EventArgs e)
        {
            // inputed data is not empty
            if(tbMovieName.Text!="" && tbMovieGenre.Text!="" && tbMovieRating.Text!="" && tbMovieYear.Text!="" && tbMovieCopies.Text!="" && tbMoviePlot.Text!="")
            {
                int movieYear = Convert.ToInt32(tbMovieYear.Text);
                int copies = Convert.ToInt32(tbMovieCopies.Text);
                string rent;
               // movie 5 years older rent= 2
               // if movie is new rent is 5
               
                if(DateTime.Now.Date.Year-movieYear > 5) 
                {
                    rent = "2";
                }else
                {
                    rent = "5";
                }
                movieShop.AddMovie(tbMovieRating.Text, tbMovieName.Text, tbMovieYear.Text, rent, tbMoviePlot.Text, tbMovieGenre.Text, copies);
                populate_movies_view(movieShop.ListMovies());
                clear_movies_boxes();
            }else
            {
                MessageBox.Show("Ensure movie fields are not empty");
            }
        }

        private void BtnUpdateMovie_Click(object sender, EventArgs e)
        {
            // check for empty input field
            if (tbMovieId.Text != "" && tbMovieName.Text != "" && tbMovieGenre.Text != "" && tbMovieRating.Text != "" && tbMovieYear.Text != "" && tbMovieCopies.Text != "" && tbMoviePlot.Text != "")
            {
                // iinput from movie text boxes
                int movieId = Convert.ToInt32(tbMovieId.Text);
                int copies = Convert.ToInt32(tbMovieCopies.Text);
                int year = Convert.ToInt32(tbMovieYear.Text);
                string title = tbMovieName.Text;
                string rating = tbMovieRating.Text;
                string genre = tbMovieGenre.Text;
                string plot = tbMoviePlot.Text;
                //Update record in movies table
                movieShop.UpdateMovie(movieId, rating, title, year, plot, genre, copies);
                MessageBox.Show("Movie record Updated!", "Success");
                populate_movies_view(movieShop.ListMovies());
                clear_movies_boxes();
            }else
            {
                MessageBox.Show("Ensure movie fields are not empty");
            }

        }

        private void BtnDeleteMovie_Click(object sender, EventArgs e)
        {
            if(tbMovieId.Text!="")
            {
                // dialog message to confirm deletion of record
                DialogResult result = MessageBox.Show("Confirm delete?", "Confirm", MessageBoxButtons.YesNo);
                if(result.ToString()=="Yes")
                {
                    int movieId = Convert.ToInt32(tbMovieId.Text);
                    movieShop.DeleteMovie(movieId); // delete a movie by id
                    populate_movies_view(movieShop.ListMovies());
                    clear_movies_boxes();
                }
            }
            else
            {
                MessageBox.Show("Movie is not selected");
            }
        }


        private void BtnIssueMovie_Click(object sender, EventArgs e)
        {
            if(tbMovieId.Text !="" && tbCustID.Text!="")
            {
                if(tbMovieCopies.Text!="0")
                {
                    int movieId = Convert.ToInt32(tbMovieId.Text);
                    int custId = Convert.ToInt32(tbCustID.Text);
                    int copies = Convert.ToInt32(tbMovieCopies.Text);
                    // inserted rented =1 and issue date  = current date
                    movieShop.AddMovieRented(movieId, custId, DateTime.Now, copies, 1); // adds rented movie
                    populate_rents_view(movieShop.ListRentedMovies()); // update list view
                    // reset all text boxes for customer and movie
                    clear_customer_boxes();
                    clear_movies_boxes();

                }
                else
                {
                    MessageBox.Show("all copies of movie are out", "Problem");
                }
            }else
            {
                MessageBox.Show("select customer and movie from list then Rent a movie");
            }
        }

        private void BtnReturnMovie_Click(object sender, EventArgs e)
        {
            // check for empty fields
            if(tbMovieId.Text!="" && tbDateRented.Text!="" && tbRMID.Text!="")
            {
                int rmid = Convert.ToInt32(tbRMID.Text);
                int movieId = Convert.ToInt32(tbMovieId.Text);
                String date = tbDateRented.Text;
                // return movie by reseting renting =0  and date of return  = current date
                movieShop.UpdateMovieRented(rmid, movieId, Convert.ToDateTime(date), DateTime.Now);
                dgvRentals.DataSource = movieShop.ListRentedMovies().DefaultView;
                populate_customer_view(movieShop.get_customers());
                populate_movies_view(movieShop.ListMovies());
                populate_rents_view(movieShop.ListRentedMovies());
                // clear all input fields
                clear_customer_boxes(); 
                clear_movies_boxes(); 
                tbRMID.Text = ""; 
                tbDateRented.Text = "";
            }else
            {
                MessageBox.Show("Movie is not select to return");
            }
        }


       

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Exit application
            Application.Exit(); 
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        TopCustMovie f = new TopCustMovie();

        private void BtnBestCustomer_Click(object sender, EventArgs e)
        {
            f.best_customer();
        }

        private void BtnBestMoie_Click(object sender, EventArgs e)
        {
            f.best_movie();
        }
        ListViewItem item;
        private void LvRentedMovies_ItemActivate(object sender, EventArgs e)
        {
            item= lvRentedMovies.SelectedItems[0];
            tbRMID.Text = item.SubItems[0].Text;
            tbDateRented.Text = item.SubItems[3].Text;
            tbMovieId.Text = item.SubItems[2].Text;
        }
      
        private void LvMovies_ItemActivate(object sender, EventArgs e)
        {
     

            item = lvMovies.SelectedItems[0];
            tbMovieId.Text = item.SubItems[0].Text;
            tbMovieRating.Text = item.SubItems[1].Text;
            tbMovieName.Text = item.SubItems[2].Text;
            tbMovieYear.Text = item.SubItems[3].Text;
            tbMovieCopies.Text = item.SubItems[5].Text;
            tbMoviePlot.Text =  item.SubItems[6].Text;
            tbMovieGenre.Text = item.SubItems[7].Text;

        }

        private void LvCustomers_ItemActivate(object sender, EventArgs e)
        {
            item = lvCustomers.SelectedItems[0];
            tbCustID.Text = item.SubItems[0].Text;
            tbCustFirstName.Text = item.SubItems[1].Text;
            tbCustLastName.Text = item.SubItems[2].Text;
            tbCustAddress.Text = item.SubItems[3].Text;
            tbCustPhone.Text = item.SubItems[4].Text;
        }
        Random rnd = new Random();
        private void TimerLogo_Tick(object sender, EventArgs e)
        {
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            lblLogo.ForeColor = randomColor;

        }
    }
}
