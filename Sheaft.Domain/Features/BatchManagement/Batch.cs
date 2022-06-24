namespace Sheaft.Domain.BatchManagement;

public class Batch : Entity
{
    private Batch(){}
    
    public Batch(BatchNumber number, BatchDateKind dateKind, DateTime expirationDate, DateTime? productionDate, SupplierId supplierId)
    {
        Id = BatchId.New();
        Number = number;
        DateKind = dateKind;
        ExpirationDate = DateOnly.FromDateTime(expirationDate);
        ProductionDate = productionDate.HasValue ? DateOnly.FromDateTime(productionDate.Value) : null;
        SupplierId = supplierId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }
    
    public BatchId Id { get; }
    public BatchNumber Number { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public BatchDateKind DateKind { get; private set; }
    public DateOnly ExpirationDate { get; private set; }
    public DateOnly? ProductionDate { get; private set; }
    public SupplierId SupplierId { get; }

    public void Update(BatchNumber number, BatchDateKind dateKind, DateTime expirationDate, DateTime? productionDate)
    {
        Number = number;
        DateKind = dateKind;
        ExpirationDate = DateOnly.FromDateTime(expirationDate);
        ProductionDate = productionDate.HasValue ? DateOnly.FromDateTime(productionDate.Value) : null;
        UpdatedOn = DateTimeOffset.UtcNow;
    }
}