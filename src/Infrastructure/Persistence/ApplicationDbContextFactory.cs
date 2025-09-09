﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiplBlazor.Infrastructure.Persistence;
internal sealed class ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> efFactory) : IApplicationDbContextFactory
{
    public ValueTask<IApplicationDbContext> CreateAsync(CancellationToken ct = default)
    {
        var dbContext = efFactory.CreateDbContext();
        return new ValueTask<IApplicationDbContext>(dbContext);
    }
}
