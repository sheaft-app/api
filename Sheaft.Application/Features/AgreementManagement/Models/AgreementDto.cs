﻿using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AgreementDto(string Id, AgreementStatus Status, DateTimeOffset UpdatedOn, string TargetName,
    string OwnerId, AgreementOwner Owner);