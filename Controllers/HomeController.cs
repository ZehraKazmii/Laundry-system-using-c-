using Online_Laundry_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Laundry_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllAdmins()
        {
            List<Admin> admins = new List<Admin>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT admin_name, admin_password FROM admins";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Admin admin = new Admin
                    {
                        AdminName = reader["admin_name"].ToString(),
                        AdminPassword = reader["admin_password"].ToString()
                    };
                    admins.Add(admin);
                }

                reader.Close();
            }

            return View(admins);
        }
        public ActionResult AllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM customers WHERE customer_status = 1";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerName = reader["customer_name"].ToString(),
                        CustomerAddress = reader["customer_address"].ToString(),
                        CustomerEmail = reader["customer_email"].ToString(),
                        CustomerWhatsapp = reader["customer_phone"].ToString(),
                        CustomerDues = Convert.ToDouble(reader["customer_dues"]),
                        CustomerStatus = Convert.ToInt32(reader["customer_status"]),
                        CustomerId = Convert.ToInt32(reader["customer_id"])
                    };
                    customers.Add(customer);
                }

                reader.Close();
            }

            return View(customers);
        }
        public ActionResult AllInvoices()
        {
            List<Invoice> invoices = new List<Invoice>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM invoices INNER JOIN customers ON invoices.invoice_customer = customers.customer_id INNER JOIN invoice_types ON invoices.invoice_type = invoice_types.itype_name";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Invoice invoice = new Invoice
                    {
                        Customer = reader["customer_name"].ToString(),
                        Type = reader["itype_name"].ToString(),
                        Code = reader["invoice_code"].ToString(),
                        Amount = Convert.ToDecimal(reader["invoice_amount"]),
                    };
                    invoices.Add(invoice);
                }

                reader.Close();
            }

            return View(invoices);
        }
        public ActionResult AllGarments()
        {
            List<Garments> garments = new List<Garments>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM garments WHERE garment_status = 1";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Garments garment = new Garments
                    {
                        Name = reader["garment_name"].ToString(),
                        Weight = Convert.ToDecimal(reader["garment_weight"]),
                        Rate = Convert.ToDecimal(reader["garment_rate"]),
                        Status = Convert.ToInt32(reader["garment_status"]),
                        Id = Convert.ToInt32(reader["garment_id"])
                    };
                    garments.Add(garment);
                }

                reader.Close();
            }

            return View(garments);
        }
        public ActionResult Orders()
        {
            List<Deliveries> order = new List<Deliveries>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT delivery_id,customer_name,invoice_code,dstat_status FROM deliveries INNER JOIN invoices ON deliveries.delivery_invoice = invoices.invoice_id INNER JOIN customers ON deliveries.delivery_customer = customers.customer_id INNER JOIN delivery_stats ON deliveries.delivery_status = delivery_stats.dstat_id";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Deliveries delivery = new Deliveries
                    {
                        Customer = reader["customer_name"].ToString(),
                        Invoice = reader["invoice_code"].ToString(),
                        Status = reader["dstat_status"].ToString(),
                        Id = Convert.ToInt32(reader["delivery_id"])
                    };
                    order.Add(delivery);
                }

                reader.Close();
            }

            return View(order);
        }
        public ActionResult NewPayment()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string customerQuery = "SELECT customer_id, customer_name FROM customers WHERE customer_status = 1";
                SqlCommand customerCommand = new SqlCommand(customerQuery, connection);
                connection.Open();
                SqlDataReader customerReader = customerCommand.ExecuteReader();

                List<SelectListItem> customers = new List<SelectListItem>();
                while (customerReader.Read())
                {
                    customers.Add(new SelectListItem
                    {
                        Text = customerReader["customer_name"].ToString(),
                        Value = customerReader["customer_id"].ToString()
                    });
                }
                customerReader.Close();
                string invoiceQuery = "SELECT invoice_id, invoice_code FROM invoices WHERE invoice_customer = @CustomerId";
                SqlCommand invoiceCommand = new SqlCommand(invoiceQuery, connection);
                SqlDataReader invoiceReader;

                Dictionary<string, List<SelectListItem>> customerInvoicesDict = new Dictionary<string, List<SelectListItem>>();
                foreach (var customer in customers)
                {
                    invoiceCommand.Parameters.Clear();
                    invoiceCommand.Parameters.AddWithValue("@CustomerId", customer.Value);
                    invoiceReader = invoiceCommand.ExecuteReader();

                    List<SelectListItem> customerInvoices = new List<SelectListItem>();
                    while (invoiceReader.Read())
                    {
                        customerInvoices.Add(new SelectListItem
                        {
                            Text = invoiceReader["invoice_code"].ToString(),
                            Value = invoiceReader["invoice_id"].ToString()
                        });
                    }
                    invoiceReader.Close();

                    customerInvoicesDict.Add(customer.Value, customerInvoices);
                }
                string paymentTypeQuery = "SELECT ptype_id, ptype_name FROM payment_types WHERE ptype_status = 1";
                SqlCommand paymentTypeCommand = new SqlCommand(paymentTypeQuery, connection);
                SqlDataReader paymentTypeReader = paymentTypeCommand.ExecuteReader();

                List<SelectListItem> paymentTypes = new List<SelectListItem>();
                while (paymentTypeReader.Read())
                {
                    paymentTypes.Add(new SelectListItem
                    {
                        Text = paymentTypeReader["ptype_name"].ToString(),
                        Value = paymentTypeReader["ptype_id"].ToString()
                    });
                }
                paymentTypeReader.Close();

                ViewBag.Customers = customers;
                ViewBag.CustomerInvoices = customerInvoicesDict;
                ViewBag.PaymentTypes = paymentTypes;

                return View();
            }

        }
        public ActionResult NewInvoice()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string customerQuery = "SELECT customer_id, customer_name FROM customers WHERE customer_status = 1";
                SqlCommand customerCommand = new SqlCommand(customerQuery, connection);
                connection.Open();
                SqlDataReader customerReader = customerCommand.ExecuteReader();

                List<SelectListItem> customers = new List<SelectListItem>();
                while (customerReader.Read())
                {
                    customers.Add(new SelectListItem
                    {
                        Text = customerReader["customer_name"].ToString(),
                        Value = customerReader["customer_id"].ToString()
                    });
                }
                customerReader.Close();
                string invoiceTypeQuery = "SELECT itype_id, itype_name FROM invoice_types WHERE itype_status = 1";
                SqlCommand invoiceTypeCommand = new SqlCommand(invoiceTypeQuery, connection);
                SqlDataReader invoiceTypeReader = invoiceTypeCommand.ExecuteReader();

                List<SelectListItem> invoiceTypes = new List<SelectListItem>();
                while (invoiceTypeReader.Read())
                {
                    invoiceTypes.Add(new SelectListItem
                    {
                        Text = invoiceTypeReader["itype_name"].ToString(),
                        Value = invoiceTypeReader["itype_id"].ToString()
                    });
                }
                invoiceTypeReader.Close();

                ViewBag.Customers = customers;
                ViewBag.InvoiceTypes = invoiceTypes;

                return View();
            }

        }
        public ActionResult PaymentHistory()
        {
            List<Payments> payHistory = new List<Payments>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT payment_dateTime,customer_name,invoice_code,invoice_amount FROM payments INNER JOIN invoices ON payments.payment_invoice = invoices.invoice_id INNER JOIN customers ON payments.payment_customer = customers.customer_id";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Payments pay = new Payments
                    {
                        Customer = reader["customer_name"].ToString(),
                        Invoice = reader["invoice_code"].ToString(),
                        PaidAt = Convert.ToDateTime(reader["payment_dateTime"]),
                        Amount = Convert.ToDecimal(reader["invoice_amount"])

                    };
                    payHistory.Add(pay);
                }

                reader.Close();
            }

            return View(payHistory);
        }
        public ActionResult PaymentTypes()
        {
            List<PaymentTypes> paymentTypes = new List<PaymentTypes>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM payment_types WHERE ptype_status = 1";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PaymentTypes pType = new PaymentTypes
                    {
                        Id = Convert.ToInt32(reader["ptype_id"]),
                        Type = reader["ptype_name"].ToString()
                    };
                    paymentTypes.Add(pType);
                }

                reader.Close();
            }

            return View(paymentTypes);
        }
        public ActionResult InvoiceTypes()
        {
            List<InvoiceTypes> invoiceTypes = new List<InvoiceTypes>();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM invoice_types WHERE itype_status = 1";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    InvoiceTypes iType = new InvoiceTypes
                    {
                        Id = Convert.ToInt32(reader["itype_id"]),
                        Type = reader["itype_name"].ToString()
                    };
                    invoiceTypes.Add(iType);
                }

                reader.Close();
            }

            return View(invoiceTypes);
        }
        public ActionResult DeleteCustomer(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE customers SET customer_status = 0 WHERE customer_id = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error deleting customer!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return RedirectToAction("AllCustomers");
        }
        public ActionResult DeleteGarment(int Id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE garments SET garment_status = 0 WHERE garment_id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error deleting garment!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return RedirectToAction("AllGarments");
        }
        public ActionResult DeletePaymentType(int PayTypeId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE payment_types SET ptype_status = 0 WHERE ptype_id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", PayTypeId);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error deleting Payment Type!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return RedirectToAction("PaymentTypes");
        }
        public ActionResult DeleteInvoiceType(int InvoiceTypeId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE invoice_types SET itype_status = 0 WHERE itype_id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", InvoiceTypeId);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error deleting Invoice Type!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return RedirectToAction("InvoiceTypes");
        }
        public ActionResult UpdateCustomer(int customerId)
        {
            Customer customer = GetCustomerFromDatabase(customerId); 
            return View(customer);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(Customer customer)
        {
            UpdateCustomerInDatabase(customer);
            return RedirectToAction("AllCustomers");
        }
        private Customer GetCustomerFromDatabase(int customerId)
    {
            Customer customer = new Customer();
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM customers WHERE customer_id = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            customer.CustomerId = Convert.ToInt32(reader["customer_id"]);
                            customer.CustomerName = reader["customer_name"].ToString();
                            customer.CustomerEmail = reader["customer_email"].ToString();
                            customer.CustomerWhatsapp = reader["customer_phone"].ToString();
                            customer.CustomerAddress = reader["customer_address"].ToString();
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

        return customer;
        }
        private void UpdateCustomerInDatabase(Customer customer)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE customers SET customer_name = @Name, customer_email = @Email, customer_phone = @Whatsapp, customer_address = @Address WHERE customer_id = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    command.Parameters.AddWithValue("@Name", customer.CustomerName);
                    command.Parameters.AddWithValue("@Email", customer.CustomerEmail);
                    command.Parameters.AddWithValue("@Whatsapp", customer.CustomerWhatsapp);
                    command.Parameters.AddWithValue("@Address", customer.CustomerAddress);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error updating data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        public ActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            InsertCustomerInDatabase(customer);
            return RedirectToAction("AllCustomers");
        }
        private void InsertCustomerInDatabase(Customer customer)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO customers (customer_name,customer_email,customer_phone,customer_address) VALUES (@Name,@Email,@Whatsapp,@Address)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.CustomerName);
                    command.Parameters.AddWithValue("@Email", customer.CustomerEmail);
                    command.Parameters.AddWithValue("@Whatsapp", customer.CustomerWhatsapp);
                    command.Parameters.AddWithValue("@Address", customer.CustomerAddress);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        public ActionResult UpdateGarment(int Id)
        {
            Garments garment = GetGarmentFromDatabase(Id);
            return View(garment);
        }
        [HttpPost]
        public ActionResult UpdateGarment(Garments garment)
        {
            UpdateGarmentInDatabase(garment);
            return RedirectToAction("AllGarments");
        }
        private Garments GetGarmentFromDatabase(int Id)
        {
            Garments garment = new Garments();
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM garments WHERE garment_id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            garment.Id = Convert.ToInt32(reader["garment_id"]);
                            garment.Name = reader["garment_name"].ToString();
                            garment.Rate = Convert.ToDecimal(reader["garment_rate"]);
                            garment.Weight = Convert.ToDecimal(reader["garment_weight"]);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return garment;
        }
        private void UpdateGarmentInDatabase(Garments garment)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE garments SET garment_name = @Name, garment_rate = @Rate, garment_weight = @Weight WHERE garment_id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", garment.Id);
                    command.Parameters.AddWithValue("@Name", garment.Name);
                    command.Parameters.AddWithValue("@Rate", garment.Rate);
                    command.Parameters.AddWithValue("@Weight", garment.Weight);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error updating data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
       
        [HttpPost]
        public ActionResult NewPayment(PaymentIn payment)
        {
            InsertPaymentTypeInDatabase(payment);
            return RedirectToAction("PaymentHistory");
        }
        private void InsertPaymentTypeInDatabase(PaymentIn payment)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO payments (payment_customer,payment_invoice,payment_type) VALUES (@Name,@Invoice,@Type)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", payment.Customer);
                    command.Parameters.AddWithValue("@Invoice", payment.Invoice);
                    command.Parameters.AddWithValue("@Type", payment.Type);
                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        public ActionResult AddGarment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddGarment(Garments garment)
        {
            InsertGarmentInDatabase(garment);
            return RedirectToAction("AllGarments");
        }
        private void InsertGarmentInDatabase(Garments garment)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO garments (garment_name,garment_rate,garment_weight) VALUES (@Name,@Rate,@Weight)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", garment.Name);
                    command.Parameters.AddWithValue("@Weight", garment.Weight);
                    command.Parameters.AddWithValue("@Rate", garment.Rate);
                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult AddPaymentType(PaymentTypes paymentType)
        {
            InsertPaymentTypeInDatabase(paymentType);
            return RedirectToAction("PaymentTypes");
        }
        private void InsertPaymentTypeInDatabase(PaymentTypes paymentType)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO payment_types (ptype_name) VALUES (@Type);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Type", paymentType.Type);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult AddInvoiceType(InvoiceTypes invoiceType)
        {
            InsertInvoiceTypeInDatabase(invoiceType);
            return RedirectToAction("InvoiceTypes");
        }
        private void InsertInvoiceTypeInDatabase(InvoiceTypes invoiceType)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO invoice_types (itype_name) VALUES (@Type);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Type", invoiceType.Type);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        public ActionResult UpdateStatusOrder(int Id, string Status)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (Status == "Ready")
                {
                    string query = "UPDATE deliveries SET delivery_status = 2 WHERE delivery_id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);

                        try
                        {
                            connection.Open();
                            int result = command.ExecuteNonQuery();
                            if (result < 0)
                                Console.WriteLine("Error updating order!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    string query = "UPDATE deliveries SET delivery_status = 3 WHERE delivery_id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);

                        try
                        {
                            connection.Open();
                            int result = command.ExecuteNonQuery();
                            if (result < 0)
                                Console.WriteLine("Error updating order!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }  
            }
            return RedirectToAction("Orders");
        }
        [HttpPost]
        public ActionResult NewInvoice(InvoiceIn invoice)
        {
            int invoiceId = InsertInvoiceInDatabase(invoice);
            return RedirectToAction("AddItemsInInvoice");
        }
        private int InsertInvoiceInDatabase(InvoiceIn invoice)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineLaundryConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO invoices (invoice_customer,invoice_type) VALUES (@Customer,@Type);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Customer", invoice.Customer);
                    command.Parameters.AddWithValue("@Type", invoice.Type);
                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                       
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                return 1;
            }
        }
    }
}