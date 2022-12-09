// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;
using System;
using System.Collections.Generic;

#pragma warning disable 1573, 1591
#nullable enable

namespace DataModel
{
	[Table("Returnable")]
	public class Returnable
	{
		[Column("Id"        , CanBeNull = false, IsPrimaryKey = true)] public string         Id         { get; set; } = null!; // nvarchar(12)
		[Column("Name"      , CanBeNull = false                     )] public string         Name       { get; set; } = null!; // nvarchar(80)
		[Column("Reference" , CanBeNull = false                     )] public string         Reference  { get; set; } = null!; // nvarchar(20)
		[Column("UnitPrice"                                         )] public decimal        UnitPrice  { get; set; } // decimal(18, 2)
		[Column("Vat"                                               )] public decimal        Vat        { get; set; } // decimal(18, 2)
		[Column("SupplierId", CanBeNull = false                     )] public string         SupplierId { get; set; } = null!; // nvarchar(12)
		[Column("CreatedOn"                                         )] public DateTimeOffset CreatedOn  { get; set; } // datetimeoffset(7)
		[Column("UpdatedOn"                                         )] public DateTimeOffset UpdatedOn  { get; set; } // datetimeoffset(7)

		#region Associations
		/// <summary>
		/// FK_Product_Returnable_ReturnableId backreference
		/// </summary>
		[Association(ThisKey = nameof(Id), OtherKey = nameof(Product.ReturnableId))]
		public IEnumerable<Product> ProductReturnableIds { get; set; } = null!;

		/// <summary>
		/// FK_Returnable_Supplier_SupplierId
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(SupplierId), OtherKey = nameof(DataModel.Supplier.Id))]
		public Supplier Supplier { get; set; } = null!;
		#endregion
	}
}