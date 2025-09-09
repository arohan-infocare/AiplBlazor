﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiplBlazor.Application.Common.Interfaces;
public interface IApplicationDbContextFactory
{
    ValueTask<IApplicationDbContext> CreateAsync(CancellationToken ct = default);
}
