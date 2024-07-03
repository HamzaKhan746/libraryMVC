using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace library.Models
{
    public class BookModel
    {
        public static int Bookid;
        string constring = "Data Source=DESKTOP-ABS14OI;Initial Catalog=Library;Integrated Security=True";

        public int InsertBookRecord(string bname, string author, string publisher, string category, int price, int quantity)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                string available = "true";
                con.Open();
                string query = "insert into Books(bookname,authorname,publisher,category,price,quantity,noofavailablebooks,availability)values(@bname,@author,@publisher,@category,@price,@quantity,@quantity,@available)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@bname", bname);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@publisher", publisher);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@available", available);
                int a=cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    string query1 = "Select max(bookid) from Books";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd1);
                    DataSet ds = new DataSet();
                    adp1.Fill(ds);
                    int a1 = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    return a1;
                }
                else
                    return 0;
            }
        }
        public DataTable GetBookbyId(int bid)
        {
            Bookid = bid;
            string available = "true";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Books where bookid=@bid AND availability=@available";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@bid", Bookid);
                cmd.Parameters.AddWithValue("@available", available);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int EditBookRecord(string bname, string author, string publisher, string category, int price, int quantity)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "UPDATE Books SET bookname=@bname,authorname=@author,publisher=@publisher,category=@category,price=@price,quantity=@quantity where bookid=@bid";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@bname", bname);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@publisher", publisher);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@bid", Bookid);
                return cmd.ExecuteNonQuery();
            }
        }
        public int DeleteBookRecord(int id)
        {
            string available = "false";
            int bid = id;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Update Books SET availability=@available where bookid=@bid";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@bid", bid);
                cmd.Parameters.AddWithValue("@available", available);
                return cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetAllCategories()
        {
            string available = "true";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Books where availability=@available";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@available", available);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public DataTable GetAllBooks()
        {
            string available = "true";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Books where availability=@available";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@available", available);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public DataTable GetBookbyCategory(string category)
        {
            string available = "true";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Books where availability=@available AND category=@category";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@available", available);
                cmd.Parameters.AddWithValue("@category", category);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
