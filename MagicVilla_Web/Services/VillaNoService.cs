using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNoService : IVillaNoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        private readonly IBaseService _baseService;
        public VillaNoService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(VillaNoCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNoAPI"
            });
        }

        public async Task<T> DeleteAsync<T>(int number)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNoAPI/" + number
            });
        }
        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNoAPI"
            });
        }

        public async Task<T> GetAsync<T>(int number)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNoAPI/" + number
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNoUpdatedDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNoAPI/" + dto.VillaNo
            });
        }
    }
}
