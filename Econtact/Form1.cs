using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Econtact
{
    public partial class Econtact : Form

    {
        string connectionString = "server=localhost;database=contact_db;uid=root;pwd=00700;";

        DataTable contactTable = new DataTable();

        public object FirstName { get; private set; }

        public Econtact()
        {
            InitializeComponent();
        }

        private void Econtact_Load(object sender, EventArgs e)
        {

            // Create DataTable columns
            contactTable.Columns.Add("Full Name");
            contactTable.Columns.Add("Contact Number");
            contactTable.Columns.Add("Address");
            contactTable.Columns.Add("Gender");

            // Bind DataTable to DataGridView
            dataGridView1.DataSource = contactTable;

            LoadData();
        }

        private void LoadData()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM contacts", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void bttnAdd_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text == "" || txtContactNum.Text == "")
            {
                MessageBox.Show("Please fill in Full Name and Contact Number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO contacts (full_name, contact_number, address, gender) VALUES (@name, @number, @address, @gender)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@number", txtContactNum.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Record added successfully!", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadData();
        }


        private void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE contacts SET full_name=@name, contact_number=@number, address=@address, gender=@gender WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@number", txtContactNum.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadData();
        }

        private void bttnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "DELETE FROM contacts WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Record deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadData();
        }

        private void bttnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM contacts WHERE full_name LIKE @search OR contact_number LIKE @search";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtFullName.Text = row.Cells["full_name"].Value.ToString();
                txtContactNum.Text = row.Cells["contact_number"].Value.ToString();
                txtAddress.Text = row.Cells["address"].Value.ToString();
                cmbGender.Text = row.Cells["gender"].Value.ToString();
            }

        }

        private void ClearFields()
        {
            txtFullName.Clear();
            txtContactNum.Clear();
            txtAddress.Clear();
            cmbGender.SelectedIndex = -1;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
    
}
