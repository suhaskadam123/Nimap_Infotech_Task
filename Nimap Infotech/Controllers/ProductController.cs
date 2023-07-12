using Microsoft.AspNetCore.Mvc;
using Nimap_Infotech.Models;
using System.Data;
using System.Data.SqlClient;


namespace Nimap_Infotech.Controllers
{
    public class ProductController : Controller
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Nimap_Infotech_Task;Integrated Security=True";

       
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT P.ProductId, P.ProductName, P.CategoryId, C.CategoryName FROM Product AS P INNER JOIN Category AS C ON P.CategoryId = C.CategoryId ORDER BY P.ProductId OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Skip", skip);
                command.Parameters.AddWithValue("@Take", take);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductViewModel product = new ProductViewModel
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            CategoryName = reader["CategoryName"].ToString()
                        };
                        products.Add(product);
                    }
                }
            }

            int totalProducts = GetTotalProductCount();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalProducts = totalProducts
            };

            return View(viewModel);
        }

        private int GetTotalProductCount()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Product";
                SqlCommand command = new SqlCommand(query, connection);
                count = (int)command.ExecuteScalar();
            }

            return count;
        }
    

    public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Product (ProductName, CategoryId, CategoryName) VALUES (@ProductName, @CategoryId, @CategoryName)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                command.Parameters.AddWithValue("@CategoryName", product.CategoryName);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

       
        public ActionResult Edit(int id)
        {
            Product product = new Product();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Product WHERE ProductId = @ProductId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product.ProductId = Convert.ToInt32(reader["ProductId"]);
                    product.ProductName = reader["ProductName"].ToString();
                    product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                    product.CategoryName = reader["CategoryName"].ToString();
                }
                reader.Close();
            }
            return View(product);
        }

      
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Product SET ProductName = @ProductName, CategoryId = @CategoryId, CategoryName = @CategoryName WHERE ProductId = @ProductId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                command.Parameters.AddWithValue("@CategoryName", product.CategoryName);
                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

       
        public ActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Product WHERE ProductId = @ProductId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", id);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        
       
    }

}
