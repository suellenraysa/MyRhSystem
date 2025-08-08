using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Models;

namespace MyRhSystem.APP.Shared.Services
{
    public class CompanyRegisterApiService
    {
        [Inject] protected CompanyRegisterApiService CompanyService { get; set; } = default!;
        private readonly HttpClient _http;

        public CompanyRegisterApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> RegisterCompanyAsync(CompanyRegisterModel model)
        {
            var response = await _http.PostAsJsonAsync("api/company", model);
            return response.IsSuccessStatusCode;
        }
    }
}
