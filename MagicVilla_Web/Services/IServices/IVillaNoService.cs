using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNoService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int number);
    Task<T> CreateAsync<T>(VillaNoCreateDTO dto);
    Task<T> UpdateAsync<T>(VillaNoUpdatedDTO dto);
    Task<T> DeleteAsync<T>(int number);

}
