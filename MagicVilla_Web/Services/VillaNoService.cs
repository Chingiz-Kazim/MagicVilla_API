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

        public Task<T> CreateAsync<T>(VillaNoCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest() 
            { 
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/v1/VillaNoAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int number, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/v1/VillaNoAPI/" + number,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/v1/VillaNoAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int number, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/v1/VillaNoAPI/" + number,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaNoUpdatedDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/v1/VillaNoAPI/" + dto.VillaNo,
                Token = token
            });
        }
    }
}
