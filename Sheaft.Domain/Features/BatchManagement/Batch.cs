namespace Sheaft.Domain.BatchManagement;

public class Batch : Entity
{
    private Batch(){}
    
    public Batch(BatchNumber number, BatchDateKind dateKind, DateOnly date, SupplierId supplierIdentifier)
    {
        Identifier = BatchId.New();
        Number = number;
        DateKind = dateKind;
        Date = date;
        SupplierIdentifier = supplierIdentifier;
    }
    
    public BatchId Identifier { get; }
    public BatchNumber Number { get; private set; }
    public BatchDateKind DateKind { get; private set; }
    public DateOnly Date { get; private set; }
    public SupplierId SupplierIdentifier { get; }

    public Result Update(BatchNumber number, BatchDateKind dateKind, DateOnly date)
    {
        Number = number;
        DateKind = dateKind;
        Date = date;

        return Result.Success();
    }
}