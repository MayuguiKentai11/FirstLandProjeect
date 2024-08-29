using EntityLayer;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IConnectionBridge
    {
        Task<List<Admin>> listAdminsReturn();
        
        bool sendEmail(string email, string header, string message);

        int createAdminReturn(Admin admin, out string message);

        bool editAdminReturn(Admin admin, out string message);

        bool deleteAdminReturn(int ID , out string message);

        List<Category> listCategoriesReturn();

        int createCategoryReturn(Category category, out string message);

        bool editCategoryReturn(Category category, out string message);

        bool deleteCategoryReturn(int id, out string message);

        int createProductReturn(Product product, out string message);

        bool editProductReturn(Product product, out string message);

        bool updateImageReturn(Product product, out string message);

        bool deleteProductReturn(int id, out string message);

        DashBoard returnSummaryDashboard();

        List<ReportSale> returnListReportSales(string dateStart, string dateEnd, string idTransaction);

        bool resetPasswordReturnAdmin(int id, string email, out string message);

        Task<bool> resetPasswordReturnClient(int id, string email, string message);

        Task<List<Client>> listClientsReturn();

        Task<bool>createClientReturn(Client client, string result);

        Task<bool> validateCredentialsClientRequest(Client client);

        List<Product> listProductsReturn();

        int returnQuantityShoppingCartProducts(int idClient);

        bool returnVerifyExistanceShoppingCart(int idClient, int idProduct);

        bool returnOperationsShoppingCartProducts(int idClient, int idProduct, bool sumOperation, out string message);

        List<ShoppingCart> returnListShoppingCartProducts(int idClient);

        bool returnDeleteShoppingCartProducts(int idClient, int idProduct, out string message);

        List<Department> returnListDepartments();

        List<Province> returnListProvinces(string idDepartment);

        List<District> returnListDistricts(string idDepartment, string idProvince);

        bool returnFinishProcessSale(Sale objSale, DataTable detailSale, out string message);

        List<DetailSale> returnListSales(int IdClient);

        Task<List<ReportGraphics>> reportGraphicsDatabaseReturn();

        Task<List<ReportProductGraphics>> reportProductGraphicsreturn();
        
        Task<bool> updatePasswordReturnClient(int id, string emailClient, string password);

        bool sendEmailShopping(string email);

        List<ShoppingProduct> listHistorialShopping(string dateStart, string dateEnd, string idTransaction);

        Task<bool> updatePasswordReturnAdmin(string emailClient, string password);
    }
}
