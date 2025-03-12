using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Repository.IRepository;

public interface IVillaNoRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}
