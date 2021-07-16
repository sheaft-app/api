using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class BatchDefinition : IEntity
    {
        protected BatchDefinition()
        {
        }

        public BatchDefinition(Guid id, string name, Producer producer, IEnumerable<BatchField> fields = null)
        {
            Id = id;
            Name = name;
            ProducerId = producer.Id;
            Producer = producer;

            SetFields(fields);
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsDefault { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string JsonDefinition { get; set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public byte[] RowVersion { get; private set; }
        public IEnumerable<BatchField> FieldDefinitions => JsonConvert.DeserializeObject<IEnumerable<BatchField>>(JsonDefinition);
        
        public void SetFields(IEnumerable<BatchField> fields)
        {
            if (fields == null)
                JsonDefinition = "[]";
            
            JsonDefinition = JsonConvert.SerializeObject(fields);
        }
        
        public void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");
            
            Name = name;
        }
        
        public void SetIsDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }
        
        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}