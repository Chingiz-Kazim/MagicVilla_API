using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNoService : BaseService, IVillaNoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public VillaNoService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaNoCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest() 
            { 
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/VillaNoAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int number)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/VillaNoAPI/" + number
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNoAPI"
            });
        }

        public Task<T> GetAsync<T>(int number)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNoAPI/" + number
            });
        }

        public Task<T> UpdateAsync<T>(VillaNoUpdatedDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/VillaNoAPI/" + dto.VillaNo
            });
        }
    }
}
