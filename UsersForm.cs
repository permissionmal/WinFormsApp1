using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp1
{
    public partial class UsersForm : Form
    {
        private SqlConnection conn;

        public UsersForm()
        {
            InitializeComponent();       
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {

           conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $" SELECT Id, Name ," +
                              $" IIF(IsGlobalUser = 1, 'Global User', 'Not Global User') AS 'UserType' FROM Users";

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
      
            dataGridView2.AutoGenerateColumns = false;

            //Set Columns Count
            dataGridView2.ColumnCount = 3;

            //Add Columns
            dataGridView2.Columns[0].Name = "Id";
            dataGridView2.Columns[0].HeaderText = "User Id";
            dataGridView2.Columns[0].DataPropertyName = "Id";
            dataGridView2.Columns[0].Visible = false;

            dataGridView2.Columns[1].HeaderText = "UserName";
            dataGridView2.Columns[1].Name = "Name";
            dataGridView2.Columns[1].DataPropertyName = "Name";
            dataGridView2.Columns[1].Width = 380;

            dataGridView2.Columns[2].Name = "UserType";
            dataGridView2.Columns[2].HeaderText = "User Type";
            dataGridView2.Columns[2].DataPropertyName = "UserType";
            dataGridView2.Columns[2].Width = 380;
            dataGridView2.DataSource = dt;

            conn.Close();

        
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            txtHideID.Clear();
            var rowIndex = e.RowIndex;
            var currentCellValue = dataGridView2.Rows[rowIndex].Cells[1].Value.ToString();
            MenuNoTwo.Text = "Assign User to Client Group - " + currentCellValue;
            txtHideID.Text = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();

            conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $" SELECT c.Name from Users a INNER JOIN ClientGroupUsers b ON a.Id = b.UserId INNER JOIN ClientGroups c ON b.ClientGroupId = c.Id Where a.Id  = {txtHideID.Text}";
           
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                listBox2.Items.Add(dr[0]);               
            }

            conn.Close();


            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = conn;
            conn.Open();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = $" select Distinct b.Name FROM ClientGroupUsers a INNER JOIN ClientGroups b ON a.ClientGroupId = b.Id Where b.Name NOT IN (select b.Name FROM ClientGroupUsers a INNER JOIN ClientGroups b ON a.ClientGroupId = b.Id Where a.userId = {txtHideID.Text})";

            cmd2.ExecuteNonQuery();
            DataTable dt2 = new DataTable();

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow dr2 = dt2.Rows[i];

                listBox1.Items.Add(dr2[0]);      
            }

            conn.Close();
        }

        List<string> registrationsList = new List<string>();
        List<string> listCollection = new List<string>();
        public void AssignUsers()
        {
            listBox1.Items.Clear();
            conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $" SELECT Distinct b.Name FROM ClientGroupUsers a INNER JOIN ClientGroups b ON a.ClientGroupId = b.Id Where b.Name NOT IN (select b.Name FROM ClientGroupUsers a INNER JOIN ClientGroups b ON a.ClientGroupId = b.Id Where a.userId = {txtHideID.Text})";

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            listCollection.Clear();
            registrationsList.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                listCollection.Add((string)dr[0]);
            }
            registrationsList = listCollection.ToList();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                listBox1.Items.Clear();
                foreach (string str in registrationsList)
                {
                    if (str.StartsWith(textBox1.Text))
                    {
                        listBox1.Items.Add(str);
                    }
                }
            }
            else if (textBox1.Text == "")
            {
                listBox1.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    listBox1.Items.Add(dr[0]);
                }
            }
            conn.Close();
        }

        public void UnAssignUsers()
        {
            listBox2.Items.Clear();
            conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $" SELECT c.Name from Users a INNER JOIN ClientGroupUsers b ON a.Id = b.UserId INNER JOIN ClientGroups c ON b.ClientGroupId = c.Id Where a.Id  = {txtHideID.Text}";

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            listCollection.Clear();
            registrationsList.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                listCollection.Add((string)dr[0]);
            }
            registrationsList = listCollection.ToList();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                listBox2.Items.Clear();
                foreach (string str in registrationsList)
                {
                    if (str.StartsWith(textBox1.Text))
                    {
                        listBox2.Items.Add(str);
                    }
                }
            }
            else if (textBox1.Text == "")
            {
                listBox2.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    listBox2.Items.Add(dr[0]);
                }
            }
            conn.Close();
        }
     
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

            AssignUsers();
            UnAssignUsers();
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            string selectedClientGroup;
            if (listBox1.SelectedItems.Count > 0)
            {

                selectedClientGroup = listBox2.SelectedItems[0].ToString();

                conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $" INSERT INTO ClientGroupUsers VALUES( (Select Id From ClientGroups Where Name = '{selectedClientGroup}'), {txtHideID.Text})";
                cmd.ExecuteNonQuery();
                conn.Close();

                AssignUsers();
                UnAssignUsers();           
            }
            else
            {
                MessageBox.Show("No Select User Client Group");
            }
        }

        private void btnUnassign_Click(object sender, EventArgs e)
        {

            string selectedClientGroup;
            if (listBox1.SelectedItems.Count > 0)
            {

                selectedClientGroup = listBox1.SelectedItems[0].ToString();

                conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $" DELETE FROM ClientGroupUsers WHERE UserId = {txtHideID.Text} and Id =  (Select Id From ClientGroups Where Name = '{selectedClientGroup}')";
                cmd.ExecuteNonQuery();
                conn.Close();
                txtHideID.Clear();

                AssignUsers();
                UnAssignUsers();
            }
            else
            {
                MessageBox.Show("No Select User Client Group");
            }         
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
