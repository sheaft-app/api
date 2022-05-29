namespace Sheaft.Domain.BatchManagement;

public class Batch : Entity
{
    private Batch(){}
    
    public Batch(BatchNumber number, BatchDateKind dateKind, DateOnly date, SupplierId supplierId)
    {
        Id = BatchId.New();
        Number = number;
        DateKind = dateKind;
        Date = date;
        SupplierId = supplierId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }
    
    public BatchId Id { get; }
    public BatchNumber Number { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public BatchDateKind DateKind { get; private set; }
    public DateOnly Date { get; private set; }
    public SupplierId SupplierId { get; }

    public Result Update(BatchNumber number, BatchDateKind dateKind, DateOnly date)
    {
        Number = number;
        DateKind = dateKind;
        Date = date;
        UpdatedOn = DateTimeOffset.UtcNow;

        return Result.Success();
    }
}