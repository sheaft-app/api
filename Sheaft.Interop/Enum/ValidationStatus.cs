﻿namespace Sheaft.Interop.Enums
{
    public enum ValidationStatus
    {
        NotSpecified = 0,
        Created = 1,
        ValidationAsked = 2,
        Validated = 4,
        Refused = 8,
        OutOfDate = 16,
        WaitingForCreation = 100,
        WaitingForFirstOrder,
    }
}