using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoSocial.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace DemoSocial.Persistence;

internal sealed class UnitOfWork(DataContext context) : IUnitOfWork
{
    private readonly DataContext _context = context;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = _context.Database.BeginTransactionAsync(cancellationToken);

        return transaction;
    }
}
