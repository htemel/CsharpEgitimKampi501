using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpEgitimKampi501.Dtos;
using Dapper;

namespace CsharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1QU3AO4;Initial Catalog=CsharpEgitimKampi501;Integrated Security=True");

        private async void btnList_Click(object sender, EventArgs e)
        {
            string query="Select * from TblProduct";
            var values=await connection.QueryAsync<ResultProductDto>(query);
            
            dataGridView1.DataSource = values;

        }

      

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "insert into TblProduct(ProductName,ProductStock,ProductPrice,ProductCategory) values(@ProductName,@ProductStock,@ProductPrice,@ProductCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtProductName.Text);
            parameters.Add("@ProductStock", txtProductStock.Text);
            parameters.Add("@ProductPrice", txtProductPrice.Text);
            parameters.Add("@ProductCategory", txtProductCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Ürün Eklendi");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query= "Delete from TblProduct where ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);   
            MessageBox.Show("Ürün Silindi");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "Update TblProduct set ProductName=@ProductName,ProductStock=@ProductStock,ProductPrice=@ProductPrice,ProductCategory=@ProductCategory where ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtProductName.Text);
            parameters.Add("@ProductStock", txtProductStock.Text);
            parameters.Add("@ProductPrice", txtProductPrice.Text);
            parameters.Add("@ProductCategory", txtProductCategory.Text);
            parameters.Add("@ProductId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Ürün Güncellendi","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1= "Select Count(*) from TblProduct";
            var productTotalCount=await connection.QueryFirstAsync<int>(query1);
            lblTotalProductCount.Text = productTotalCount.ToString();

            string query2= "Select ProductName From TblProduct where ProductPrice=(Select Max(ProductPrice) from TblProduct)";
            var maxPriceProductName=await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblMaxPriceProductName.Text = maxPriceProductName;

            string query3 = "Select Count(Distinct(ProductCategory)) From TblProduct";
            var distinctCategoryCount = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblDistinctCategoryCount.Text = distinctCategoryCount.ToString();
        }
    }
}
