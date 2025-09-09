// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

global using System.Security.Claims;
global using AutoMapper;
global using AutoMapper.QueryableExtensions;
global using AiplBlazor.Application.Common.Interfaces;
global using AiplBlazor.Application.Common.Interfaces.Identity;
global using AiplBlazor.Application.Common.Models;
global using AiplBlazor.Infrastructure.Persistence;
global using AiplBlazor.Infrastructure.Persistence.Extensions;
global using AiplBlazor.Infrastructure.Services;
global using AiplBlazor.Infrastructure.Services.Identity;
global using AiplBlazor.Domain.Entities;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;