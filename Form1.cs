using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentRegistrationManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load();
        }
        SqlConnection con = new SqlConnection(@"data source=DESKTOP-GI82C9B\SQLEXPRESS;initial catalog=gcbt;integrated security=sspi");
        SqlCommand cmd;
        SqlDataReader read;
        SqlDataAdapter drr;
        string id;
        bool mode = true;
        string sql;

        public void Load()
        {
            try
            {
                sql = "select * from Student";
                cmd = new SqlCommand(sql, con);
                con.Open();
                read = cmd.ExecuteReader();
                
                dataGridView1.Rows.Clear();

                while(read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getID(string id)
        {
            sql = "select * from Student where Id='" + id + "'  ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();

            while(read.Read())
            {
                txtstname.Text = read[1].ToString();
                txtcourse.Text = read[2].ToString();
                txtfee.Text = read[3].ToString();
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtstname.Text;
            string course = txtcourse.Text;
            string fee = txtfee.Text;

            if(mode==true)
            {
                sql = "insert into Student(Stname,Course,Fee) Values(@Stname,@Course,@Fee)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Stname", name);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@Fee", fee);
                MessageBox.Show("Record Add");
                cmd.ExecuteNonQuery();

                txtstname.Clear();
                txtcourse.Clear();
                txtfee.Clear();
                txtstname.Focus();
            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update Student set Stname=@Stname,Course=@Course,Fee=@Fee where Id=@Id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Stname", name);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@Fee", fee);
                cmd.Parameters.AddWithValue("@id", id);
                MessageBox.Show("Record Updated");
                cmd.ExecuteNonQuery();

                txtstname.Clear();
                txtcourse.Clear();
                txtfee.Clear();
                txtstname.Focus();
                button1.Text = "Save";
                mode = true;
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index&&e.RowIndex>=0)
            {
                mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button1.Text = "Edit";
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from Student where Id=@Id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted");
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtstname.Clear();
            txtcourse.Clear();
            txtfee.Clear();
            txtstname.Focus();
            button1.Text = "Save";
            mode = true;
        }
    }
}
