﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AiplBlazor.Application.Features.PicklistSets.Commands.Import;

public class ImportPicklistSetsCommandValidator : AbstractValidator<ImportPicklistSetsCommand>
{
    public ImportPicklistSetsCommandValidator()
    {
        RuleFor(x => x.Data).NotNull().NotEmpty();
    }
}