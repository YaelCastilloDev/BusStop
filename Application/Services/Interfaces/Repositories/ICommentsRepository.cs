using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.Repositories
{
    public interface ICommentsRepository
    {
        Task<Guid> AddAsync(Comment comment, CancellationToken cancellationToken = default);
        Task UpdateAsync(Comment comment);
        Task<Comment?> GetByIdAsync(Guid id);
    }
}
