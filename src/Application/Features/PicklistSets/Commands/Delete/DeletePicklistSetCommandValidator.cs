// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AiplBlazor.Application.Features.PicklistSets.Commands.Delete;

public class DeletePicklistSetCommandValidator : AbstractValidator<DeletePicklistSetCommand>
{
    public DeletePicklistSetCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}