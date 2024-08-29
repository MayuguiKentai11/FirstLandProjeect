using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Newtonsoft.Json;

namespace WebServicesLayer
{
    public class AdminArea
    {
        public AdminArea() { }

        public async Task<List<Admin>> ListAdminRequest() // GET VALIDADO
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/Admin/ListAdmins";
                List<Admin> validation = new List<Admin>();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<List<Admin>>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                List<Admin> admins = new List<Admin>();
                return admins;
            }
        }

        public async Task<bool> CreateAdminRequest(Admin admin) // POST VALIDADO
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/Admin/CreateAdmin";
                bool validation = false;
                string Data = JsonConvert.SerializeObject(admin); // LO CONVIERTO EN OBJETO JSON
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await client.PostAsync(url,content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<bool>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAdminRequest(Admin admin) // POST 
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/Admin/EditAdmin";
                bool validation = false;
                string Data = JsonConvert.SerializeObject(admin); // LO CONVIERTO EN OBJETO JSON
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<bool>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAdminRequest(int id) // POST
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/Admin/DeleteAdmin";
                bool validation = false;
                string Data = JsonConvert.SerializeObject(id); // LO CONVIERTO EN OBJETO JSON
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<bool>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePasswordAdmin(string email, string password)
        {
            try
            {
                bool validation = false;

                HttpClient client = new HttpClient();

                string url = "http://localhost:63544/api/Admin/UpdatePasswordAdmin";

                Admin admin = new Admin()
                {
                    emailAdmin = email,
                    passwordAdmin = password
                };

                var data = JsonConvert.SerializeObject(admin);

                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON

                    validation = JsonConvert.DeserializeObject<bool>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
