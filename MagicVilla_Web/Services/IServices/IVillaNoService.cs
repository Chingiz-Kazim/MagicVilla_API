using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNoService
{
    Task<T> GetAllAsync<T>(string token);
    Task<T> GetAsync<T>(int number, string token);
    Task<T> CreateAsync<T>(VillaNoCreateDTO dto, string token);
    Task<T> UpdateAsync<T>(VillaNoUpdatedDTO dto, string token);
    Task<T> DeleteAsync<T>(int number, string token);

}
