// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace DataModel
{
	[Table("DeliveryAdjustment_Batches")]
	public class DeliveryAdjustmentBatch
	{
		[Column("BatchIdentifier"     , CanBeNull    = false, IsPrimaryKey    = true, PrimaryKeyOrder = 1)] public string BatchIdentifier      { get; set; } = null!; // nvarchar(12)
		[Column("DeliveryAdjustmentId", IsPrimaryKey = true , PrimaryKeyOrder = 0                        )] public long   DeliveryAdjustmentId { get; set; } // bigint

		#region Associations
		/// <summary>
		/// FK_DeliveryAdjustment_Batches_Delivery_Adjustments_DeliveryAdjustmentId
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(DeliveryAdjustmentId), OtherKey = nameof(DataModel.DeliveryAdjustment.Id))]
		public DeliveryAdjustment DeliveryAdjustment { get; set; } = null!;
		#endregion
	}
}