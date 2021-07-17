using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events.Batch;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Batch : IEntity, IHasDomainEvent
    {
        protected Batch()
        {
        }

        public Batch(Guid id, string number, Producer producer, BatchDefinition definition)
        {
            Id = id;
            Number = number;
            ProducerId = producer.Id;
            Producer = producer;
            DefinitionId = definition.Id;
            Definition = definition;
            JsonFields = "[]";
        }

        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? DLC { get; private set; }
        public DateTimeOffset? DDM { get; private set; }
        public string JsonFields { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid DefinitionId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual BatchDefinition Definition { get; private set; }
        public virtual ICollection<BatchObservation> Observations { get; private set; }
        public byte[] RowVersion { get; private set; }
        public IEnumerable<BatchField> Fields => JsonConvert.DeserializeObject<IEnumerable<BatchField>>(JsonFields);
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void SetNumber(string number)
        {
            Number = number;
        }

        public void SetDLC(DateTimeOffset? dlc)
        {
            DLC = dlc;
        }

        public void SetDDM(DateTimeOffset? ddm)
        {
            DDM = ddm;
        }

        public void SetValues(IEnumerable<BatchField> fields)
        {
            if (fields == null)
                fields = new List<BatchField>();
            
            foreach (var fieldDefinition in Definition.FieldDefinitions)
            {
                var field = fields.SingleOrDefault(f => f.Identifier == fieldDefinition.Identifier);
                if (field == null)
                    field = new BatchField {Identifier = fieldDefinition.Identifier, Value = fieldDefinition.Value};

                field.Required = fieldDefinition.Required;
                field.Type = fieldDefinition.Type;
            }

            JsonFields = JsonConvert.SerializeObject(fields);
        }

        public void AddObservation(string comment, User user, bool visibleToAll = false)
        {
            if (Observations == null)
                Observations = new List<BatchObservation>();

            var observation = new BatchObservation(Guid.NewGuid(), comment, user);
            observation.SetVisibility(visibleToAll);
            
            Observations.Add(observation);
            DomainEvents.Add(new BatchObservationAddedEvent(Id, observation.Id));
        }

        public void UpdateObservation(Guid observationId, string comment, bool visibleToAll = false)
        {
            if (Observations == null)
                throw SheaftException.NotFound("L'observation est introuvable.");

            var observation = Observations.SingleOrDefault(o => o.Id == observationId);
            if (observation == null)
                throw SheaftException.NotFound("L'observation est introuvable.");

            observation.SetVisibility(visibleToAll);
            observation.SetComment(comment);
        }

        public void RemoveObservation(Guid observationId)
        {
            if (Observations == null)
                throw SheaftException.NotFound("L'observation est introuvable.");

            var observation = Observations.SingleOrDefault(o => o.Id == observationId);
            if (observation == null)
                throw SheaftException.NotFound("L'observation est introuvable.");

            Observations.Remove(observation);
        }
    }
}