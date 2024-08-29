using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Newtonsoft.Json;

namespace WebServicesLayer
{
    public class ClientArea
    {
        public ClientArea() { }

        public async Task<List<Client>> ListClientRequest() // GET
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/Admin/ListClients";
                List<Client> validation = new List<Client>();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<List<Client>>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                List<Client> clientes = new List<Client>();
                return clientes;
            }
        }

        public async Task<bool> ResetPasswordRequest(int id, string emailClient) // POST VALIDADO
        {
            // CREO UN NUEVO OBJETO DE TIPO CLIENTE PARA POSTERIORMENTE SERIALIZARLO DENTRO DEL
            // TRY CATCH
            Client client = new Client();
            
            client.ID = id;
            client.email = emailClient;
            try
            {
                HttpClient clientRequest = new HttpClient();
                string url = "http://localhost:63544/api/Admin/ResetPasswordClient";
                bool validation = false;
                string Data = JsonConvert.SerializeObject(client); // LO CONVIERTO EN OBJETO JSON
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await clientRequest.PostAsync(url, content);
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

        public async Task<bool> UpdatePasswordRequest(int id, string emailClient, string password) // POST VALIDADO
        {
            // CREO UN NUEVO OBJETO DE TIPO CLIENTE PARA POSTERIORMENTE SERIALIZARLO DENTRO DEL
            // TRY CATCH
            Client client = new Client();

            client.ID = id;
            client.email = emailClient;
            client.password = password;

            try
            {
                HttpClient clientRequest = new HttpClient();
                string url = "http://localhost:63544/api/Admin/UpdatePasswordClient";
                bool validation = false;
                string Data = JsonConvert.SerializeObject(client); // LO CONVIERTO EN OBJETO JSON
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await clientRequest.PostAsync(url, content);
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

        public async Task<bool> CreateClientRequest(Client client) // POST
        {
            try
            {
                HttpClient clientRequest = new HttpClient();
                string url = "http://localhost:63544/api/Admin/CreateClient";
                bool validation = false;
                string Data = JsonConvert.SerializeObject(client); // LO CONVIERTO EN OBJETO JSON
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await clientRequest.PostAsync(url, content);
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
