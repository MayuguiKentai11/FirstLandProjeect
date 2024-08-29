using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using System.Net.Http;
using EntityLayer;
using System.Xml.Schema;

namespace WebServicesLayer
{
    public class LogInArea
    {
        public LogInArea() { }

        public async Task<bool> ValidateCredential(Client client)
        {
            try
            {
                HttpClient RequestClient = new HttpClient();
                string Url = "http://localhost:63544/api/LogIn/Authenticate";
                string Data = JsonConvert.SerializeObject(client); // LO CONVIERTO EN OBJETO JSON
                bool Validation = false;
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await RequestClient.PostAsync(Url, content); // CONSUMO EL API
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    Validation = JsonConvert.DeserializeObject<bool>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                else
                {
                    Validation = false;
                }
                return Validation;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> VerifyTokenCredential(Client client)
        {
            try
            {
                HttpClient RequestClient = new HttpClient();
                string Url = "http://localhost:63544/api/LogIn/Authenticate";
                string Data = JsonConvert.SerializeObject(client); // LO CONVIERTO EN OBJETO JSON
                bool Validation = false;
                HttpContent content = new StringContent(Data, Encoding.UTF8, "application/json"); // ENVIO EL PARAMETRO EN JSON
                var response = await RequestClient.PostAsync(Url, content); // CONSUMO EL API
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    Validation = JsonConvert.DeserializeObject<bool>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                else
                {
                    Validation = false;
                }
                return Validation;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




    }
}
