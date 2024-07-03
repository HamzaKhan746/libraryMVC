using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace library.Models
{
    public class FunctionModel
    {
        public static int Memberid;
        public static int Issuebookid;
        string constring = "Data Source=DESKTOP-ABS14OI;Initial Catalog=Library;Integrated Security=True";
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
                cmd.Parameters.AddWithValue("@mid", Memberid);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int InsertIBookRecord(int bookid, string idate, string rdate,string user)
        {
            string state = "issued";
            string istate = "returned";
            string mstate = "requested";
            string available = "true";
            int no = 0;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query4 = "Select * from IssueBook where memberid=@mid AND state!=@istate";
                SqlCommand cmd4 = new SqlCommand(query4, con);
                cmd4.Parameters.AddWithValue("@mid", Memberid);
                cmd4.Parameters.AddWithValue("@istate", istate);
                SqlDataAdapter adp4 = new SqlDataAdapter(cmd4);
                DataTable dt4 = new DataTable();
                adp4.Fill(dt4);
                int a4 = dt4.Rows.Count;
                if (a4 <2)
                {
                    string query2 = "Select * from Books where bookid=@bid AND availability=@available";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    cmd2.Parameters.AddWithValue("@bid", bookid);
                    cmd2.Parameters.AddWithValue("@available", available);
                    SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    int a2 = dt2.Rows.Count;
                    if (a2 > 0)
                    {
                        string query3 = "Select * from Books where bookid=@bid AND noofavailablebooks>@no";
                        SqlCommand cmd3 = new SqlCommand(query3, con);
                        cmd3.Parameters.AddWithValue("@bid", bookid);
                        cmd3.Parameters.AddWithValue("@no", no);
                        SqlDataAdapter adp3 = new SqlDataAdapter(cmd3);
                        DataTable dt3 = new DataTable();
                        adp3.Fill(dt3);
                        int a3 = dt3.Rows.Count;
                        if (a3 > 0)
                        {
                            string query;
                            if (user == "lib")
                            {
                                query = "insert into IssueBook(memberid,bookid,issuedate,returndate,state)values(@mid,@bid,@idate,@rdate,@state)";
                            }
                            else
                            {
                                query = "insert into IssueBook(memberid,bookid,issuedate,returndate,state)values(@mid,@bid,@idate,@rdate,@mstate)";
                            }
                            string query1 = "update Books SET noofavailablebooks=noofavailablebooks-1 where bookid=@bid";
                            SqlCommand cmd = new SqlCommand(query, con);
                            SqlCommand cmd1 = new SqlCommand(query1, con);
                            cmd.Parameters.AddWithValue("@idate", idate);
                            cmd.Parameters.AddWithValue("@rdate", rdate);
                            cmd.Parameters.AddWithValue("@bid", bookid);
                            cmd.Parameters.AddWithValue("@mid", Memberid);
                            cmd.Parameters.AddWithValue("@state", state);
                            cmd.Parameters.AddWithValue("@mstate", mstate);
                            cmd1.Parameters.AddWithValue("@bid", bookid);
                            int a = cmd.ExecuteNonQuery();
                            int a1 = cmd1.ExecuteNonQuery();
                            if (a > 0 && a1 > 0)
                            {
                                    return 1;
                             }
                                else
                                    return 0;
                        }
                        else
                        {
                            return 3;
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 4;
                }
            }
        }
        public DataTable GetMemberbyIssuebook(int ibookid)
        {
            Issuebookid= ibookid;
            string state = "issued";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from IssueBook inner Join Member On Member.memberid=IssueBook.memberid where IssueBook.issuebookid=@ibookid AND state=@state";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ibookid", ibookid);
                cmd.Parameters.AddWithValue("@state", state);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int InsertRBookRecord(int bookid, int mid, int daysdelay)
        {
            string state = "returned";
            int fine;
            fine = daysdelay * 50;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "insert into ReturnBook(issuebookid,bookid,memberid,daysdelay,fine)values(@ibookid,@bid,@mid,@ddelay,@fine)";
                string query1 = "update IssueBook SET state=@state where issuebookid=@ibookid";
                string query2 = "update Books SET noofavailablebooks=noofavailablebooks+1 where bookid=@bid";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlCommand cmd1 = new SqlCommand(query1, con);
                SqlCommand cmd2 = new SqlCommand(query2, con);
                cmd.Parameters.AddWithValue("@mid", mid);
                cmd.Parameters.AddWithValue("@bid", bookid);
                cmd.Parameters.AddWithValue("@fine", fine);
                cmd.Parameters.AddWithValue("@ddelay", daysdelay);
                cmd.Parameters.AddWithValue("@ibookid", Issuebookid);
                cmd1.Parameters.AddWithValue("@state", state);
                cmd1.Parameters.AddWithValue("@ibookid", Issuebookid);
                cmd2.Parameters.AddWithValue("@bid", bookid);
                int a=cmd.ExecuteNonQuery();
                int a1 = cmd1.ExecuteNonQuery();
                int a2 = cmd2.ExecuteNonQuery();
                if (a>0 && a1>0 && a2>0)
                {
                    return 1;
                }
                return 0;
            }
        }
        public DataTable GetAllIssuedBooks()
        {
            string state = "issued";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member inner Join IssueBook On Member.memberid=IssueBook.memberid inner Join Books On IssueBook.bookid=Books.bookid where IssueBook.state=@state";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@state", state);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public DataTable GetAllReturnedBooks()
        {
            string state = "returned";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member inner Join ReturnBook On Member.memberid=ReturnBook.memberid inner Join IssueBook On ReturnBook.issuebookid=IssueBook.issuebookid inner Join Books On ReturnBook.bookid=Books.bookid where IssueBook.state=@state";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@state", state);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int Findissuebookid()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query5 = "Select max(issuebookid) from IssueBook";
                SqlCommand cmd5 = new SqlCommand(query5, con);
                SqlDataAdapter adp5 = new SqlDataAdapter(cmd5);
                DataSet ds = new DataSet();
                adp5.Fill(ds);
                int b1 = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                return b1;
            }
        }
        public DataTable GetIssueBookRequest()
        {
            string state = "requested";
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Select * from Member inner Join IssueBook On Member.memberid=IssueBook.memberid inner Join Books On IssueBook.bookid=Books.bookid where IssueBook.state=@state";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@state", state);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        public int AcceptIssuebookRequest(int Ibookid)
        {
            string state = "issued";
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "Update IssueBook SET state=@state where issuebookid=@ibookid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ibookid", Ibookid);
                cmd.Parameters.AddWithValue("@state", state);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
