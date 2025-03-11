using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository;

public class VillaNoRepository : Repository<VillaNumber>, IVillaNoRepository
{
    private readonly ApplicationDbContext _db;
    public VillaNoRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdateDate = DateTime.Now;
        _db.VillaNumbers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
