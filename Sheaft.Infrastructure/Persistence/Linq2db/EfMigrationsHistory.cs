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
	[Table("__EFMigrationsHistory")]
	public class EfMigrationsHistory
	{
		[Column("MigrationId"   , CanBeNull = false, IsPrimaryKey = true)] public string MigrationId    { get; set; } = null!; // nvarchar(150)
		[Column("ProductVersion", CanBeNull = false                     )] public string ProductVersion { get; set; } = null!; // nvarchar(32)
	}
}
