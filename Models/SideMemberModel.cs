using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace library.Models
{
    public class SideMemberModel
    {
        public static int MMemberid;
        string constring = "Data Source=DESKTOP-ABS14OI;Initial Catalog=Library;Integrated Security=True";
        public DataTable GetMyIssuedBooks()
        {
            string state = "issued";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member inner Join IssueBook On Member.memberid=IssueBook.memberid inner Join Books On IssueBook.bookid=Books.bookid where IssueBook.state=@state AND Member.memberid=@mid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@mid", MMemberid);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public DataTable GetIBooklist()
        {
            string state = "issued";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from IssueBook where state=@state AND memberid=@mid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@mid", MMemberid);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public DataTable GetMyReturnedbooks()
        {
            string state = "returned";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member inner Join ReturnBook On Member.memberid=ReturnBook.memberid inner Join IssueBook On ReturnBook.issuebookid=IssueBook.issuebookid inner Join Books On ReturnBook.bookid=Books.bookid where IssueBook.state=@state AND Member.memberid=@mid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@mid", MMemberid);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public void Mid(int id)
        {
            MMemberid = id;
        }

    }
}
