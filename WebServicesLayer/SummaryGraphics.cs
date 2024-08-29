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
    public class SummaryGraphics
    {
        public SummaryGraphics() { }

        public async Task<List<ReportGraphics>> ListReportGraphicsRequest() // GET VALIDADO
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/SymmaryGraphics/ListReportGraphics";
                List<ReportGraphics> validation = new List<ReportGraphics>();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<List<ReportGraphics>>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                List<ReportGraphics> reports = new List<ReportGraphics>();
                return reports;
            }
        }

        public async Task<List<ReportProductGraphics>> ListReportProductsRequest() // GET VALIDADO
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "http://localhost:63544/api/SymmaryGraphics/ListProductGraphics";
                List<ReportProductGraphics> validation = new List<ReportProductGraphics>();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync(); // FORMATO JSON
                    validation = JsonConvert.DeserializeObject<List<ReportProductGraphics>>(result); // DESEREALIZAR EL RESULTADO EN BOOL
                }
                return validation;
            }
            catch (Exception ex)
            {
                List<ReportProductGraphics> reports = new List<ReportProductGraphics>();
                return reports;
            }
        }

    }
}
