using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MeuDbContext db) : base(db)
        {
        }

        public async Task<Autor> ObterAutorPorUserId(string userId)
        {
            return await _db.Autores.FirstOrDefaultAsync(a => a.UserId == userId);
        }


    }
}
