using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoRentShop
{
   public class VideoShop
    {
        // SQL Objects to access latter db handling
        SqlConnection Connection = new SqlConnection("Data Source=DESKTOP-A1QAS6T  Initial Catalog=VideoShop;Integrated Security=True");
        SqlDataReader Reader;
        SqlCommand Command = new SqlCommand();
        string Query = "";

        #region functions related to Rented Movie table
        public DataTable ListRentedMovies()
        {
            DataTable dtRentedMovies = new DataTable();
            try
            {
                Command.Connection = Connection;
                Query = "SELECT * FROM RentedMovies Order by RMID DESC";
                Command.CommandText = Query;
                Connection.Open();
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtRentedMovies.Load(Reader);
                }
                return dtRentedMovies;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
                return null;
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
                if (Reader != null)
                {
                    Reader.Close();
                }
            }
        }
        public void AddMovieRented(int MovieIDFK, int CustIDFK, DateTime dateRented, int copies, int rented)
        {
            try
            {

                Command.Parameters.Clear();
                Command.Connection = Connection;
                Query = "INSERT INTO RentedMovies(MovieIDFK, CustIDFK, DateRented,Rented) VALUES (@MovieIDFK,@CustIDFK,@DateRented,@Rented)";
                Command.Parameters.AddWithValue("@MovieIDFK", MovieIDFK);
                Command.Parameters.AddWithValue("@CustIDFK", CustIDFK);
                Command.Parameters.AddWithValue("@DateRented", dateRented);
                Command.Parameters.AddWithValue("@Rented", rented);
                Command.CommandText = Query;
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
            }

        }
        public void UpdateMovieRented(int RMID, int MovieID, DateTime dateRented, DateTime dateReturned)
        {
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;
                int rentTotal = 0, cost = 0;
                // movie rented for how many days
                double days = (dateReturned - dateRented).TotalDays;

                string S1 = "SELECT Rental_Cost FROM Movies WHERE MovieID = @MovieIDFK";
                Command.Parameters.AddWithValue("@MovieIDFK", MovieID);

                Command.CommandText = S1;
                Connection.Open();
                cost = Convert.ToInt32(Command.ExecuteScalar());
               
                 
                    // if movie is rented for one day
                
                if (Convert.ToInt32(days) == 0)
                {
                    rentTotal = cost;
                }
                else
                // if movie is rented for more than 1 days
                {
                    rentTotal = Convert.ToInt32(days) * cost;
                }

                Query = "UPDATE RentedMovies SET DateReturned = @DateReturned WHERE RMID = @RMID";
                Command.Parameters.AddWithValue("@DateReturned", dateReturned);
                Command.Parameters.AddWithValue("@RMID", RMID);
                Command.CommandText = Query;
                Command.ExecuteNonQuery();


                Query = "UPDATE Movies SET copies = copies+1 WHERE MovieID = @MovieIDFK";
                Command.CommandText = Query;
                Command.ExecuteNonQuery();
                Query = "UPDATE RentedMovies SET Rented = 0 WHERE RMID = @RMID";
                Command.CommandText = Query;
                Command.ExecuteNonQuery();
                MessageBox.Show("Customer's Total Rent:  " + rentTotal,"Rent");



            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception: " + ex.Message);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
        }
        #endregion

        #region Movies table functions
        public DataTable ListMovies()
        { // populate record in movies list view
            DataTable dt = new DataTable();
            try
            {
                Command.Connection = Connection;
                Query = "Select * from Movies";

                Command.CommandText = Query;
                //open coonection
                Connection.Open();

                // execute reader
                Reader = Command.ExecuteReader();
                // if there is data
                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                    // fill our datatable with that data
                }
                return dt;
            }
            catch (Exception ex)
            {
                // display error
                MessageBox.Show("Database Exception" + ex.Message);
                return null;
            }
            finally
            {
                // close conenctiona and reader
                if (Reader != null)
                {
                    Reader.Close();
                }
       
                if (Connection != null)
                {
                    Connection.Close();
                }
            }

        }



        public void AddMovie(string Rating, string Title, string Year, string Rental_Cost, string Plot, string Genre, int copies)
        {//insert data into movie table
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;



                Query = "Insert into Movies(Rating, Title, Year, Rental_Cost, Plot, Genre, copies) Values( @Rating, @Title, @Year, @Rental_Cost, @Plot, @Genre, @copies)";


                Command.Parameters.AddWithValue("@Rating", Rating);
                Command.Parameters.AddWithValue("@Title", Title);
                Command.Parameters.AddWithValue("@Year", Year);
                Command.Parameters.AddWithValue("@Rental_Cost", Rental_Cost);
                Command.Parameters.AddWithValue("@Plot", Plot);
                Command.Parameters.AddWithValue("@Genre", Genre);
                Command.Parameters.AddWithValue("@copies", copies);

                Command.CommandText = Query;

                //connection opened
                Connection.Open();

                // Executed query
                Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // show error Message
                MessageBox.Show("Database Exception" + ex.Message);
            }
            finally
            {
                // close connection
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
        }

        public void DeleteMovie(int MovieID)
        {//remove data from movie table
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;


                //blow code is to check if the Movie is rented
                String check = "";
                check = "select Count(*) from RentedMovies where MovieIDFK = @MovieID and Rented ='1' ";
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@MovieID", MovieID) };
                Command.Parameters.Add(parameterArray[0]);

                Command.CommandText = check;
                Connection.Open();
                //this code will delete the movie if its not rented otherwise the else statement would work
                int count = Convert.ToInt32(Command.ExecuteScalar());
                if (count == 0)
                {
                    String k = "Delete from Movies where MovieID like @MovieID";
                    Command.CommandText = k;
                    Command.ExecuteNonQuery();
                    MessageBox.Show("Deleted");
                }
                else
                {
                    //display the message if he has a movie on rent 
                    MessageBox.Show("system cannot delete customer. a movie is rented on his name");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
            }
            finally
            {
                if (this.Connection != null)
                {
                    this.Connection.Close();
                }

            }
        }



        public void UpdateMovie(int MovieID, string Rating, string Title, int Year, string Plot, string Genre, int copies)
        {//this method is used to update the movie 
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;
                Query = "Update Movies Set Rating = @Rating, Title = @Title, Year = @Year,  Plot = @Plot, Genre = @Genre, copies = @copies where MovieID like @MovieID";


                Command.Parameters.AddWithValue("@MovieID", MovieID);
                Command.Parameters.AddWithValue("@Rating", Rating);
                Command.Parameters.AddWithValue("@Title", Title);
                Command.Parameters.AddWithValue("@Year", Year);
                Command.Parameters.AddWithValue("@Plot", Plot);
                Command.Parameters.AddWithValue("@Genre", Genre);
                Command.Parameters.AddWithValue("@copies", copies);


                Command.CommandText = Query;

                //connection opened
                Connection.Open();

                // Executed query
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // show error Message
                MessageBox.Show("Database Exception" + ex.Message);
            }
            finally
            {
                // close connection
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
        }
        #endregion


        #region Authentication Functions
        public bool AuthUser(string username, string password)
        {
            try
            {
                Command.Connection = Connection;
                Query = "SELECT user_name, password FROM users WHERE user_name = @user AND password = @pass;";
                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@user", username);
                Command.Parameters.AddWithValue("@pass", password);
                Command.CommandText = Query;
                Connection.Open();
                Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
            finally
            {
                if (Reader != null)
                {
                    Reader.Close();
                }
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
        }
        public bool RegUser(string username, string password)
        {
            try
            {
                if (AuthUser(username, password))
                {
                    return false;
                }
                else
                {
                    Command.Parameters.Clear();
                    Command.Connection = Connection;
                    Command.CommandText = "INSERT into users(user_name,password) VALUES(@username,@password)";
                    Command.Parameters.AddWithValue("@username", username);
                    Command.Parameters.AddWithValue("@password", password);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                    return true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }

            }
        }
        #endregion

        #region customers table functions
        public DataTable get_customers()
        {
            DataTable dtCustomers = new DataTable();
            try
            {
                Command.Connection = Connection;
                Query = "SELECT * from Customer";
                Command.CommandText = Query;
                Connection.Open();
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtCustomers.Load(Reader);
                }
                return dtCustomers;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
                return null;
            }
            finally
            {
                if (Reader != null)
                {
                    Reader.Close();
                }
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
        }
        public void add_customer(string firstname, string lastname, string address, string phone)
        {
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;
                Query = "INSERT INTO Customer(FirstName,LastName,Address,Phone) VALUES (@fname,@lname,@addr,@phone)";
                Command.Parameters.AddWithValue("@fname", firstname);
                Command.Parameters.AddWithValue("@lname", lastname);
                Command.Parameters.AddWithValue("@addr", address);
                Command.Parameters.AddWithValue("@phone", phone);

                Command.CommandText = Query;

                Connection.Open();
                Command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
                if (Reader != null)
                {
                    Reader.Close();
                }
            }
        }
        public void delete_customer(int id)
        {
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;
                Query = "SELECT Count(*) FROM RentedMovies WHERE CustIDFK=@custid";
                Command.Parameters.AddWithValue("@custid", id);
                Command.CommandText = Query;
                Connection.Open();
                int count = Convert.ToInt32(Command.ExecuteScalar());
                if (count == 0)
                {
                    Query = "DELETE FROM Customer WHERE CustID = @custid";
                    Command.CommandText = Query;
                    Command.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Cannot delete customer. A movie has been rented on his name", "Problem");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }

            }
        }
        public void update_customer(int CustID, string FirstName, string LastName, string Address, string Phone)
        {
            //This method updates customer
            try
            {
                Command.Parameters.Clear();
                Command.Connection = Connection;
                Query = "Update Customer Set FirstName = @FirstName, LastName = @LastName, Address = @Address, Phone = @Phone where CustID = @CustID";


                Command.Parameters.AddWithValue("@CustID", CustID);
                Command.Parameters.AddWithValue("@FirstName", FirstName);
                Command.Parameters.AddWithValue("@LastName", LastName);
                Command.Parameters.AddWithValue("@Address", Address);
                Command.Parameters.AddWithValue("@Phone", Phone);

                Command.CommandText = Query;

                //connection opened
                Connection.Open();

                // Executed query
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // show error Message
                MessageBox.Show("Database Exception" + ex.Message);
            }
            finally
            {
                // close connection
                if (Connection != null)
                {
                    Connection.Close();
                }
            }
        }
        #endregion
        #region Connection
        public bool TestConnectionDatabase()
        {
            if(Connection.State!=ConnectionState.Open)
            {
                Connection.Open();
            }
            return true;
        }
        #endregion
    }
}
