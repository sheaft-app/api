﻿using System;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Exceptions;

namespace Sheaft.Domain.Models
{
    public abstract class Transaction : IEntity
    {
        protected Transaction()
        {
        }

        protected Transaction(Guid id, TransactionKind kind, User author)
        {
            Id = id;
            Kind = kind;
            Author = author;
            Status = TransactionStatus.Waiting;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public TransactionKind Kind { get; private set; }
        public TransactionStatus Status { get; private set; }
        public DateTimeOffset? ExecutedOn { get; private set; }
        public DateTimeOffset? ExpiredOn { get; private set; }
        public string ResultCode { get; private set; }
        public string ResultMessage { get; private set; }
        public decimal Fees { get; protected set; }
        public string Reference { get; protected set; }
        public decimal Debited { get; protected set; }
        public decimal Credited { get; protected set; }
        public virtual User Author { get; private set; }

        public void SetProcessedOn(DateTimeOffset? processedOn)
        {
            if (ExecutedOn.HasValue)
                return;

            ExecutedOn = processedOn;
        }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetStatus(TransactionStatus status)
        {
            switch (status)
            {
                case TransactionStatus.Expired:
                    if (Status == TransactionStatus.Failed || Status == TransactionStatus.Succeeded || Status == TransactionStatus.Expired)
                        throw new ValidationException();

                    ExpiredOn = DateTimeOffset.UtcNow;
                    break;
                default:
                    ExpiredOn = null;
                    break;
            }

            Status = status;
        }

        public void SetCreditedAmount(decimal amount)
        {
            Credited = amount;
        }

        public void SetDebitedAmount(decimal amount)
        {
            Debited = amount;
        }

        public void SetResult(string code, string message)
        {
            ResultCode = code;
            ResultMessage = message;
        }
    }
}