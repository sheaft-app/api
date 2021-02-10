﻿using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public class StoreTag
    {
        protected StoreTag()
        {
        }

        public StoreTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException(MessageKind.User_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}