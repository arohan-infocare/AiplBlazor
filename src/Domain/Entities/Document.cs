﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AiplBlazor.Domain.Common.Entities;
using AiplBlazor.Domain.Common.Enums;
using AiplBlazor.Domain.Identity;


namespace AiplBlazor.Domain.Entities;

public class Document : BaseAuditableEntity, IMayHaveTenant, IAuditTrial
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public JobStatus Status { get; set; } = default!;
    public string? Content { get; set; }
    public bool IsPublic { get; set; }
    public string? URL { get; set; }
    public DocumentType DocumentType { get; set; } = default!;
    public virtual Tenant? Tenant { get; set; }
    public string? TenantId { get; set; }

    public virtual ApplicationUser? CreatedBy { get;set;}
    public virtual ApplicationUser? LastModifiedBy { get; set; }
}

public enum DocumentType
{
    Document,
    Excel,
    Image,
    PDF,
    Others
}