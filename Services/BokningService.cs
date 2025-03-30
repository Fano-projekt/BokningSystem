using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BokningSystem.Models;
using BokningSystem.Data;

namespace BokningSystem.Services
{
    public class BokningService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bokningApiUrl = "https://informatik8.ei.hv.se/BokningAPI/api/bokning/";
        private readonly string _patientApiUrl = "https://informatik12.ei.hv.se/PatientAPI/api/Patient/";
        private readonly string _staffApiUrl = "https://external-api.com/api/staff";
        private readonly string _ledigaTiderApiUrl = "https://informatik8.ei.hv.se/LedigaTiderAPI/api/LedigaTider/";

        public BokningService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LedigaTider>> GetLedigaTiderAsync()
        {
            var response = await _httpClient.GetAsync(_ledigaTiderApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<LedigaTider>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<LedigaTider>();
        }

        // Hämtar ledig tid
        public async Task<LedigaTider> GetLedigTidByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_ledigaTiderApiUrl}{id}/");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LedigaTider>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        // 🔹 Lägger till en ny ledig tid 
        public async Task<bool> AddLedigTidAsync(LedigaTider ledigTid)
        {
            var json = JsonSerializer.Serialize(ledigTid);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_ledigaTiderApiUrl, content);
            return response.IsSuccessStatusCode;
        }

        // 🔹 Tar bort en ledig tid 
        public async Task<bool> DeleteLedigTidAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_ledigaTiderApiUrl}{id}/"); 
            return response.IsSuccessStatusCode;
        }

        // 🔹 Uppdatera en ledig tid
        public async Task<bool> UpdateLedigTidAsync(LedigaTider ledigTid)
        {
            var json = JsonSerializer.Serialize(ledigTid);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_ledigaTiderApiUrl}{ledigTid.Id}/", content); 
            return response.IsSuccessStatusCode;
        }

        // 🔹 Hämtar patientinfo från API
        public async Task<Patient> GetPatientInfoAsync(string personnummer)
        {
            var response = await _httpClient.GetAsync($"{_patientApiUrl}{personnummer}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Patient>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        //  vårdpersonal-kategorier 
        public async Task<List<string>> GetStaffCategoriesAsync()
        {
            await Task.Delay(500); // Simulera API-fördröjning

            return new List<string>
            {
                "Läkare",
                "Sjuksköterska",
                "Fysioterapeut",
                "Psykolog"
            };
        }

        //  Hämtar personal
        public async Task<List<StaffMember>> GetStaffMembersAsync(string category)
        {
            await Task.Delay(500); 

            return new List<StaffMember>
            {
                new StaffMember { StaffMemberId = "1", Name = "Dr. Eriksson", Position = "Läkare" },
                new StaffMember { StaffMemberId = "2", Name = "Sjuksköterska Anna", Position = "Sjuksköterska" },
                new StaffMember { StaffMemberId = "3", Name = "Fysioterapeut Johan", Position = "Fysioterapeut" }
            };
        }
        public async Task<bool> DeleteBokningAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_bokningApiUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        //  Skapa bokning i API
        public async Task<bool> AddBokningAsync(Bokning bokning)
        {
            var json = JsonSerializer.Serialize(bokning);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_bokningApiUrl, content);
            return response.IsSuccessStatusCode;
        }

        // Hämtar alla bokningar från API
        public async Task<List<Bokning>> GetAllBokningarAsync()
        {
            var response = await _httpClient.GetAsync(_bokningApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Bokning>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Bokning>();
        }
    }
}
