using Microsoft.AspNetCore.Mvc;
using Nimap_Infotech.Models;
using System.Data;
using System.Data.SqlClient;

namespace Nimap_Infotech.Controllers
{
    public class CategoryController : Controller
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Nimap_Infotech_Task;Integrated Security=True";

       
        public ActionResult Index()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Category";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Category category = new Category
                    {
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString()
                    };
                    categories.Add(category);
                }
                reader.Close();
            }
            return View(categories);
        }

      
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(Category category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult Edit(int id)
        {
            Category category = new Category();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Category WHERE CategoryId = @CategoryId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    category.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                    category.CategoryName = reader["CategoryName"].ToString();
                }
                reader.Close();
            }
            return View(category);
        }

        
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

       
        public ActionResult Delete(int id)
        {
            Category category = new Category();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Category WHERE CategoryId = @CategoryId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    category.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                    category.CategoryName = reader["CategoryName"].ToString();
                }
                reader.Close();
            }
            return View(category);
        }

        
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Category WHERE CategoryId = @CategoryId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", id);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }

}
