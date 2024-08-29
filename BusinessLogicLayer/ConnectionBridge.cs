using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataBaseLayer;
using EntityLayer;
using WebServicesLayer;

namespace BusinessLogicLayer
{
    public partial class ConnectionBridge : IConnectionBridge
    {
        private Connection managementDataBase = new Connection();
        private LogInArea logInArea = new LogInArea();
        private ClientArea clientArea = new ClientArea();
        private AdminArea adminArea = new AdminArea();
        private SummaryGraphics summaryArea = new SummaryGraphics();
    }
    public partial class ConnectionBridge // BUSINESS LOGIC FOR ADMINS ACTIONS 
    {
        public static string encriptationSHA256Security(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 hashing = SHA256Managed.Create())
            {
                Encoding encriptyngCode = Encoding.UTF8;
                byte[] result = hashing.ComputeHash(encriptyngCode.GetBytes(password));

                foreach(byte b in result)
                    stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static string generateCodeAccess()
        {
            string code = Guid.NewGuid().ToString("N").Substring(1,11);
            return code;
        }

        public bool sendEmail(string email,string header,string message)
        {
            bool result = false;
            try
            {
                // CONFIGURAR EL CORREO A ENVIAR
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(email); // EMAIL DEL RECEPTOR DEL CORREO
                mailMessage.From = new MailAddress("mauriciorojasanchez@gmail.com"); // CORREO EMISOR
                mailMessage.Subject= header; // DECLARAR EL ASUNTO DEL CORREO
                mailMessage.Body = message; // DECLARAR EL MENSAJE DEL CORREO
                mailMessage.IsBodyHtml = true;

                var server = new SmtpClient()
                {
                    Credentials = new NetworkCredential("mauriciorojasanchez@gmail.com", "vxyljgzrklfcosxj"), // CREDENCIALES DE NOMBREY CONTRASEÑA
                    Host = "smtp.gmail.com", // SERVIDOR DE GMAIL PARA ENVIAR CORREOS
                    Port = 587, // EL PUERTO QUE SE USA PARA ENVIAR LOS CORREOS
                    EnableSsl = true // HABILITAR CERTIFICADO DE SEGURIDAD 
                };
                server.Send(mailMessage);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<List<Admin>> listAdminsReturn()
        {
            return await adminArea.ListAdminRequest();
        }

        public int createAdminReturn(Admin admin,out string message)
        {
            message = string.Empty;
            if(string.IsNullOrEmpty(admin.nameAdmin) || string.IsNullOrWhiteSpace(admin.nameAdmin))
            {
                message = "EL NOMBRE NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(admin.lastNameAdmin) || string.IsNullOrWhiteSpace(admin.lastNameAdmin))
            {
                message = "EL APELLIDO NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(admin.emailAdmin) || string.IsNullOrWhiteSpace(admin.emailAdmin))
            {
                message = "EL CORREO NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(message))
            {
                string password = generateCodeAccess();
                string headerEmail = "ACCOUNT REGISTERED SUCCESFULLY!";
                string messsageEmail = "<h3>Your account was created succesfully in the system</h3></br><p>Your access code is !clave!</p>";
                messsageEmail = messsageEmail.Replace("!clave!", password);

                bool answer = sendEmail(admin.emailAdmin, headerEmail, messsageEmail);
                if (answer)
                {
                    admin.passwordAdmin = encriptationSHA256Security(password);
                    return managementDataBase.CreateAdmin(admin, out message);
                }
                else
                {
                    message = "THERE WAS AN ERROR SENDING THE EMAIL";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool sendEmailShopping(string email)
        {
            string headerEmail = "THANK YOU FOR YOUR SUPPORT!";
            string messsageEmail = "<p>Your purchase has been done correctly! Thank you for your support </p>"+
                    "<p>Atte. Maui Store</p>";

            bool answer = sendEmail(email, headerEmail, messsageEmail);

            if (answer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool editAdminReturn(Admin admin, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrEmpty(admin.nameAdmin) || string.IsNullOrWhiteSpace(admin.nameAdmin))
            {
                message = "EL NOMBRE NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(admin.lastNameAdmin) || string.IsNullOrWhiteSpace(admin.lastNameAdmin))
            {
                message = "EL APELLIDO NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(admin.emailAdmin) || string.IsNullOrWhiteSpace(admin.emailAdmin))
            {
                message = "EL CORREO NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(message))
            {
                return managementDataBase.EditAdmin(admin, out message);
            }
            else
            {
                return false;
            }
        }

        public bool deleteAdminReturn(int id,out string message)
        {
            return managementDataBase.DeleteAdmin(id,out message);
        }

        public bool resetPasswordReturnAdmin(int id, string emailclient, out string message)
        {
            message = string.Empty;
            string _newPassword = generateCodeAccess();
            string newPassword = encriptationSHA256Security(_newPassword);
            bool result = managementDataBase.ResetPasswordAdmin(id, newPassword, out message);
            bool answer = false;

            if (result)
            {
                string headerEmail = "PASSWORD RESETED SUCCESFULLY!";
                string messsageEmail = "<h3>Your new password account was created succesfully in the system</h3></br><p>Your new access code is !clave!</p>";
                messsageEmail = messsageEmail.Replace("!clave!", _newPassword);

               answer = sendEmail(emailclient, headerEmail, messsageEmail);
                if (!answer)
                    message = "NO SE PUDO ENVIAR EL CORREO";
            }
            else
            {
                message = "ERROR AL GENERAR CONTRASEÑA";
            }

            return answer;
        }

        public async Task<bool> updatePasswordReturnAdmin(string email, string password)
        {
            return await adminArea.UpdatePasswordAdmin(email, password);
        }
    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR CLIENTS ACTIONS
    {
        public async Task<List<Client>> listClientsReturn()
        {
            return await clientArea.ListClientRequest();
        }
      
        // VERIFICADO
        public async Task<bool> createClientReturn(Client client, string result)
        {
            result = string.Empty;
            if (string.IsNullOrEmpty(client.name) || string.IsNullOrWhiteSpace(client.name))
            {
                result = "EL NOMBRE NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(client.email) || string.IsNullOrWhiteSpace(client.email))
            {
                result = "EL CORREO NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(client.password) || string.IsNullOrWhiteSpace(client.password))
            {
                result = "LA CONTRASEÑA NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(result))
            {
                string header = "CUENTA REGISTRADA EXITOSAMENTE!";
                string message = "<h1>Te damos la bienvenida a Maui Store</h1></br>" +
                    "</br></br>Oficialmente cuentas con nuestro servicio" +
                    " a tu disposición. Somos un supermercado que cuenta con un alta gama de productos" +
                    " y también con servicio de delivery.</br>" +
                    "Nuestro centro de atención es: mauriciorojasanchez@gmail.com</p>" +
                    "</br>" +
                    "<img src=\"https://i.ytimg.com/vi/ghpe2at1NEs/maxresdefault.jpg\" alt=\"Welcome\">" +
                    "<p>Atte. Maui Store</p>";

                bool answer = sendEmail(client.email, header, message);

                if (answer)
                {
                    client.password = encriptationSHA256Security(client.password);
                    return await clientArea.CreateClientRequest(client);
                }
                else
                {
                    message = "THERE WAS AN ERROR SENDING THE EMAIL";
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> resetPasswordReturnClient(int id, string emailclient,string message)
        {
            message = string.Empty;
            
            string _newPassword = generateCodeAccess();
            
            string newPassword = encriptationSHA256Security(_newPassword);
            
            bool result = await clientArea.ResetPasswordRequest(id, emailclient);

            return result;
        }

        public async Task<bool> validateCredentialsClientRequest(Client client)
        {
            return await logInArea.ValidateCredential(client);
        }

        public async Task<bool> updatePasswordReturnClient(int id, string emailClient, string password)
        {
            bool result = await clientArea.UpdatePasswordRequest(id,emailClient,password);
            
            return result;
        }
    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR CATEGORY CRUD
    {
        public List<Category> listCategoriesReturn()
        {
            return managementDataBase.listCategoriesReturn();
        }

        public int createCategoryReturn(Category category, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrEmpty(category.descriptionCategory) || string.IsNullOrWhiteSpace(category.descriptionCategory))
            {
                message = "EL TEXTO NO DEBE SER VACIO O TENER ESPACIOS";
            }
           
            if (string.IsNullOrEmpty(message))
            {
                return managementDataBase.CreateCategory(category, out message);
            }
            else
            {
                return 0;
            }
        }

        public bool editCategoryReturn(Category category, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrEmpty(category.descriptionCategory) || string.IsNullOrWhiteSpace(category.descriptionCategory))
            {
                message = "EL TEXTO NO DEBE SER VACIO O TENER ESPACIOS";
            }
            if (string.IsNullOrEmpty(message))
            {
                return managementDataBase.EditCategory(category, out message);
            }
            else
            {
                return false;
            }
        }

        public bool deleteCategoryReturn(int id, out string message)
        {
            return managementDataBase.DeleteCategory(id, out message);
        }

    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR PRODUCTS ACTIONS
    {
        public List<Product> listProductsReturn()
        {
            return managementDataBase.listProductsReturn();
        }

        public int createProductReturn(Product product, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(product.nameProduct) || string.IsNullOrWhiteSpace(product.nameProduct))
            {
                message = "EL NOMBRE NO DEBE SER VACIO O TENER ESPACIOS";
            }

            else if (string.IsNullOrEmpty(product.descriptionProduct) || string.IsNullOrWhiteSpace(product.descriptionProduct))
            {
                message = "LA DESCRIPCIÓN NO DEBE SER VACIO O TENER ESPACIOS";
            }

            else if (product.priceProduct == 0)
            {
                message = "INGRESE EL PRECIO DEL PRODUCTO";
            }

            else if (product.stockProduct == 0)
            {
                message = "INGRESE EL STOCK DEL PRODUCTO";
            }

            else if (product.idCategory.Id == 0)
            {
                message = "SELECCIONE UNA CATEGORIA";
            }

            if (string.IsNullOrEmpty(message))
            {
                return managementDataBase.CreateProduct(product, out message);
            }

            else
            {
                return 0;
            }
        }

        public bool editProductReturn(Product product, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrEmpty(product.nameProduct) || string.IsNullOrWhiteSpace(product.nameProduct))
            {
                message = "EL NOMBRE NO DEBE SER VACIO O TENER ESPACIOS";
            }

            else if (string.IsNullOrEmpty(product.descriptionProduct) || string.IsNullOrWhiteSpace(product.descriptionProduct))
            {
                message = "LA DESCRIPCIÓN NO DEBE SER VACIO O TENER ESPACIOS";
            }

            else if (product.priceProduct == 0)
            {
                message = "INGRESE EL PRECIO DEL PRODUCTO";
            }

            else if (product.stockProduct == 0)
            {
                message = "INGRESE EL STOCK DEL PRODUCTO";
            }

            else if (product.idCategory.Id == 0)
            {
                message = "SELECCIONE UNA CATEGORIA";
            }

            if (string.IsNullOrEmpty(message))
            {
                return managementDataBase.EditProduct(product, out message);
            }
            else
            {
                return false;
            }
        }

        public bool updateImageReturn(Product product, out string message)
        {
            return managementDataBase.updateImageData(product, out message);
        }

        public bool deleteProductReturn(int id, out string message)
        {
            return managementDataBase.DeleteProduct(id, out message);
        }

        public static string convertBase64(string routeImage, out bool result)
        {
            string textBase64 = string.Empty;
            result = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(routeImage);
                textBase64 = Convert.ToBase64String(bytes);

            }catch(Exception e)
            {
                result = false;
            }
            return textBase64;
        }
    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR DASHBOARD SUMMARY
    {
        public DashBoard returnSummaryDashboard()
        {
            return managementDataBase.SeeSummaryProyect();
        }

        public List<ReportSale> returnListReportSales(string dateStart,string dateEnd,string idTransaction)
        {
            return managementDataBase.listReportSaleReturn(dateStart, dateEnd, idTransaction);
        }
    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR SHOPPINGCART
    {
        public int returnQuantityShoppingCartProducts(int idClient)
        {
            return managementDataBase.QuantityShoppingCartProducts(idClient);
        }

        public bool returnVerifyExistanceShoppingCart(int idClient,int idProduct)
        {
            return managementDataBase.VerifyExistanceShoppingCart(idClient, idProduct);
        }

        public bool returnOperationsShoppingCartProducts(int idClient,int idProduct,bool sumOperation,out string message)
        {
            return managementDataBase.OperationsShoppingCartProducts(idClient, idProduct, sumOperation, out message);
        }

        public List<ShoppingCart> returnListShoppingCartProducts(int idClient)
        {
            return managementDataBase.ListShoppingCartProducts(idClient);
        }

        public bool returnDeleteShoppingCartProducts(int idClient, int idProduct,out string message)
        {
            return managementDataBase.DeleteShoppingCartProduct(idProduct,idClient,out message); 
        }
    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR LOCATIONS
    {
        public List<Province> returnListProvinces(string idDepartment)
        {
            return managementDataBase.listProvinceReturn(idDepartment);
        }

        public List<Department> returnListDepartments()
        {
            return managementDataBase.listDepartmentReturn();
        }

        public List<District> returnListDistricts(string idDepartment,string idProvince)
        {
            return managementDataBase.listDistrictReturn(idDepartment, idProvince);
        }
    }

    public partial class ConnectionBridge // BUSINESS LOGIC FOR SALE AND DETAILSALE
    {
        public bool returnFinishProcessSale(Sale objSale,DataTable detailSale, out string message)
        {
            return managementDataBase.FinishProcessSale(objSale, detailSale, out message);
        }

        public List<DetailSale> returnListSales(int IdClient)
        {
            return managementDataBase.ListSales(IdClient);
        }

        public List<ShoppingProduct> listHistorialShopping(string dateStart, string dateEnd, string idTransaction)
        {
            return managementDataBase.listHistorialShopping(dateStart, dateEnd, idTransaction);
        }
    }

    public partial class ConnectionBridge // REPORT SALE GRAPHICS DASHBOARD
    {
        public async Task<List<ReportGraphics>> reportGraphicsDatabaseReturn()
        {
            return await summaryArea.ListReportGraphicsRequest();
        }

        public async Task<List<ReportProductGraphics>> reportProductGraphicsreturn()
        {
            return await summaryArea.ListReportProductsRequest();
        }

    }
}
