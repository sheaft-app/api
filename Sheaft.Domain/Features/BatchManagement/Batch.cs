namespace Sheaft.Domain.BatchManagement;

public class Batch : Entity
{
    private Batch(){}
    
    public Batch(string number, DateTime? dlc, DateTime? ddm, SupplierId supplierIdentifier)
    {
        Identifier = BatchId.New();
        Number = number;
        DLC = dlc;
        DDM = ddm;
        SupplierIdentifier = supplierIdentifier;
    }
    
    public BatchId Identifier { get; }
    public string Number { get; private set; }
    public DateTime? DLC { get; private set; }
    public DateTime? DDM { get; private set; }
    public SupplierId SupplierIdentifier { get; }
}