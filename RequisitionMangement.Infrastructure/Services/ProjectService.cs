using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Infrastructure.Interface;
using HotelCollection.Infrastructure.Models;

namespace HotelCollection.Infrastructure.Services
{
   public class ProjectService : IProjectService
    {
        private IConfiguration _config;

        public ProjectService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
        {
            List<ProjectModel> projects = new List<ProjectModel>();
            try
            {
                string apiUrl = _config.GetSection("ProjectEndPoint:baseUrl").Value;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl + "GetAllProjectCodes");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(apiUrl + "GetAllProjectCodes");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        projects = JsonConvert.DeserializeObject<List<ProjectModel>>(data);
                    }
                }
                return projects;
            }
            catch (Exception ex)
            {
                return projects;
            }

        }

    }
}
