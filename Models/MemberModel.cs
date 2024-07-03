using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace library.Models
{
    public class MemberModel
    {
        public static int Memberid;
        public static string libusername;
        string constring = "Data Source=DESKTOP-ABS14OI;Initial Catalog=Library;Integrated Security=True";
        public int Loginvalidation(string Username, string Password,string choice)
        {
            string status = "Active";
            
            using (SqlConnection con = new SqlConnection(constring))
            {    con.Open();
                string query;
                if (choice == "lib")
                {
                    libusername = Username;
                    query = "Select * from Librarian where username=@username AND password=@pas";
                }
                else
                {
                    Memberid = Convert.ToInt32(Username);
                    query = "Select * from Member where memberid=@username AND password=@pas AND status=@status";
                }
                SqlCommand cmd = new SqlCommand(query,con);
                 cmd.Parameters.AddWithValue("@username", Username);
                 cmd.Parameters.AddWithValue("@pas", Password);
                cmd.Parameters.AddWithValue("@status", status);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt.Rows.Count;
            }
        }
        public DataTable GetAllMembers()
        {
            string memstatus = "Active";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member where status=@mstatus";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mstatus", memstatus);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int InsertMemberRecord(string fname, string lname, string email, string contact, string address, string password)
        {
            string memstatus = "Active";
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                
                string query = "insert into Member(firstname,lastname,email,contact,address,password,status)values(@mfname,@mlname,@memail,@mcontact,@maddress,@mpassword,@mstatus)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@mfname", fname);
                cmd.Parameters.AddWithValue("@mlname", lname);
                cmd.Parameters.AddWithValue("@memail", email);
                cmd.Parameters.AddWithValue("@mcontact", contact);
                cmd.Parameters.AddWithValue("@maddress", address);
                cmd.Parameters.AddWithValue("@mpassword", password);
                cmd.Parameters.AddWithValue("@mstatus", memstatus);
                int a= cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    string query1 = "Select max(memberid) from Member";
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
        public DataTable GetMemberbyId(int id)
        {
            Memberid = id;
            string memstatus = "Active";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member where status=@mstatus AND memberid=@mid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mstatus", memstatus);
                cmd.Parameters.AddWithValue("@mid", id);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int EditMemberRecord(string fname, string lname, string email, string contact, string address, string password)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Update Member SET firstname=@mfname,lastname=@mlname,email=@memail,contact=@mcontact,address=@maddress,password=@mpassword where memberid=@mid";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@mfname", fname);
                cmd.Parameters.AddWithValue("@mlname", lname);
                cmd.Parameters.AddWithValue("@memail", email);
                cmd.Parameters.AddWithValue("@mcontact", contact);
                cmd.Parameters.AddWithValue("@maddress", address);
                cmd.Parameters.AddWithValue("@mpassword", password);
                cmd.Parameters.AddWithValue("@mid", Memberid);

                return cmd.ExecuteNonQuery();
            }
        }
            public int DeleteMemberRecord(int id)
            {
                string memstatus = "Inactive";
                int memid = id;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string query = "Update Member SET status=@mstatus where memberid=@mid";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@mid", memid);
                    cmd.Parameters.AddWithValue("@mstatus", memstatus);
                    return cmd.ExecuteNonQuery();
                }
            }
        public DataTable SearchLibraian()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Librarian where username=@username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", libusername);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int UpdateLibrarianRecord(string libname,string email,string contact,string address,string uname,string password)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Update Librarian SET libname=@libname,email=@email,contact=@contact,username=@uname,address=@address,password=@password where username=@libuname";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@libname", libname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@contact", contact);
                cmd.Parameters.AddWithValue("@maddress", address);
                cmd.Parameters.AddWithValue("@uname", uname);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@libuname", libusername);

                return cmd.ExecuteNonQuery();
            }
            }
        public DataTable GetLibrarian()
        {
            string memstatus = "Active";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Librarian where username=@uname";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@uname", libusername);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
