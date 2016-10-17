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

namespace BudgetApp
{
    public partial class form_Home : Form
    {
        // SQL Connection object
        SqlConnection sql_Connection;

        // Screen flags
        bool isAddItemScreen;
            // Add others...

        public form_Home()
        {
            InitializeComponent();
        }

        // Initialize home page content
        private void form_Home_Load(object sender, EventArgs e)
        {
            // Initialize SQL connection
            InitializeSQLConnection();

            // Initialize variables
            isAddItemScreen = false;
        }

        #region SQL Functions
        private void InitializeSQLConnection()
        {
            try
            {
                sql_Connection = new SqlConnection("server=localhost;database=Budget_Primary;Integrated Security=True;");
                sql_Connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
            }

            sql_Connection.Close();
        }

        private List<string> GetCategoriesFromDatabase()
        {
            // Open the connection
            sql_Connection.Open();
            
            // Create query command
            string query = "SELECT CategoryName FROM dbo.Category";
            SqlCommand category_query = new SqlCommand(query, sql_Connection);

            // Read data and store in a list
            SqlDataReader dataReader = category_query.ExecuteReader();
            List<string> categories = new List<string>();
            while (dataReader.Read())
                categories.Add(dataReader.GetString(0));

            // Close the connection
            sql_Connection.Close();

            return categories;
        }

        private List<string> GetSubCategoriesFromDatabase(string parentCategory)
        {
            // Open the connection
            sql_Connection.Open();

            // Create query command
            string query = "SELECT SubCategoryName FROM dbo.SubCategory WHERE CategoryID IN (SELECT CategoryID FROM dbo.Category WHERE CategoryName = '" + parentCategory + "');";
            SqlCommand subcategory_query = new SqlCommand(query, sql_Connection);

            // Read data and store in a list
            SqlDataReader dataReader = subcategory_query.ExecuteReader();
            List<string> subcategories = new List<string>();
            while (dataReader.Read())
                subcategories.Add(dataReader.GetString(0));

            // Close the connection
            sql_Connection.Close();

            return subcategories;
        }
        #endregion

        private void btnAddItemToBudget_Click(object sender, EventArgs e)
        {
            if (!isAddItemScreen)
            {
                // Clear gb_Home and change text
                gb_Home.Controls.Clear();
                gb_Home.Text = "Add Item To Budget";

                // Load Add Item controls
                LoadAddItemControls();
                isAddItemScreen = true;
            }
        }

        #region LoadGroupBox Functions
        // Add Item
        private void LoadAddItemControls()
        {
            /***************************
             Create and add controls
            ****************************/
            // Select month
            Label lbl_SelectMonth = new Label();
            lbl_SelectMonth.AutoSize = true;
            lbl_SelectMonth.Text = "Select Month:";
            gb_Home.Controls.Add(lbl_SelectMonth);
            lbl_SelectMonth.Location = new Point(6, 27);

            ComboBox cb_SelectMonth = new ComboBox();
            cb_SelectMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_SelectMonth.FlatStyle = FlatStyle.Flat;
            cb_SelectMonth.Width = gb_Home.Width / 3;
            cb_SelectMonth.Items.Add("January");
            cb_SelectMonth.Items.Add("February");
            cb_SelectMonth.Items.Add("March");
            cb_SelectMonth.Items.Add("April");
            cb_SelectMonth.Items.Add("May");
            cb_SelectMonth.Items.Add("June");
            cb_SelectMonth.Items.Add("July");
            cb_SelectMonth.Items.Add("August");
            cb_SelectMonth.Items.Add("September");
            cb_SelectMonth.Items.Add("October");
            cb_SelectMonth.Items.Add("November");
            cb_SelectMonth.Items.Add("December");
            gb_Home.Controls.Add(cb_SelectMonth);
            cb_SelectMonth.Location = new Point(6, 43);

            // Set default month to current month
            cb_SelectMonth.SelectedIndex = (DateTime.Today.Month - 1);

            // Select category
            Label lbl_SelectCategory = new Label();
            lbl_SelectCategory.AutoSize = true;
            lbl_SelectCategory.Text = "Select Category:";
            gb_Home.Controls.Add(lbl_SelectCategory);
            int lbl_SelectCategoryX = lbl_SelectMonth.Location.X;
            int lbl_SelectCategoryY = cb_SelectMonth.Location.Y + 35;
            lbl_SelectCategory.Location = new Point(lbl_SelectCategoryX, lbl_SelectCategoryY);

            ComboBox cb_SelectCategory = new ComboBox();
            cb_SelectCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_SelectCategory.FlatStyle = FlatStyle.Flat;
            cb_SelectCategory.Width = gb_Home.Width / 3;

            // Populate categories from database
            List<string> categories = GetCategoriesFromDatabase();
            for (int i = 0; i < categories.Count; ++i)
                cb_SelectCategory.Items.Add(categories[i]);   

            gb_Home.Controls.Add(cb_SelectCategory);
            int cb_SelectCategoryX = lbl_SelectMonth.Location.X;
            int cb_SelectCategoryY = lbl_SelectCategory.Location.Y + 16;
            cb_SelectCategory.Location = new Point(cb_SelectCategoryX, cb_SelectCategoryY);

            // Select sub-category
            Label lbl_SelectSubCategory = new Label();
            lbl_SelectSubCategory.AutoSize = true;
            lbl_SelectSubCategory.Text = "Select Sub-category:";
            gb_Home.Controls.Add(lbl_SelectSubCategory);
            int lbl_SelectSubCategoryX = lbl_SelectCategory.Location.X;
            int lbl_SelectSubCategoryY = cb_SelectCategory.Location.Y + 35;
            lbl_SelectSubCategory.Location = new Point(lbl_SelectSubCategoryX, lbl_SelectSubCategoryY);

            ComboBox cb_SelectSubCategory = new ComboBox();
            cb_SelectSubCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_SelectSubCategory.FlatStyle = FlatStyle.Flat;
            cb_SelectSubCategory.Width = gb_Home.Width / 3;
            cb_SelectSubCategory.Enabled = false;
            cb_SelectSubCategory.Items.Add("Test");
            gb_Home.Controls.Add(cb_SelectSubCategory);
            int cb_SelectSubCategoryX = lbl_SelectMonth.Location.X;
            int cb_SelectSubCategoryY = lbl_SelectSubCategory.Location.Y + 16;
            cb_SelectSubCategory.Location = new Point(cb_SelectSubCategoryX, cb_SelectSubCategoryY);
        }

        // ...
        #endregion
    }
}
