﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34011
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeoSik.WebSample.Models.LinqToSql
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Csw202TestData")]
	public partial class RecordsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Définitions de méthodes d'extensibilité
    partial void OnCreated();
    #endregion
		
		public RecordsDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RecordsSource"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public RecordsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RecordsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RecordsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RecordsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Record> Records
		{
			get
			{
				return this.GetTable<Record>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STDisjoint", IsComposable=true)]
		public System.Nullable<int> Geometry_STDisjoint([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STContains", IsComposable=true)]
		public System.Nullable<int> Geometry_STContains([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STWithin", IsComposable=true)]
		public System.Nullable<int> Geometry_STWithin([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STCrosses", IsComposable=true)]
		public System.Nullable<int> Geometry_STCrosses([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STEquals", IsComposable=true)]
		public System.Nullable<int> Geometry_STEquals([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STIntersects", IsComposable=true)]
		public System.Nullable<int> Geometry_STIntersects([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STOverlaps", IsComposable=true)]
		public System.Nullable<int> Geometry_STOverlaps([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STTouches", IsComposable=true)]
		public System.Nullable<int> Geometry_STTouches([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geometry_STDistance", IsComposable=true)]
		public System.Nullable<double> Geometry_STDistance([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geom2)
		{
			return ((System.Nullable<double>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geom1, geom2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].String_Equals", IsComposable=true)]
		public System.Nullable<int> String_Equals([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(MAX)")] string string1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(MAX)")] string string2, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> comparison)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), string1, string2, comparison).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].String_NotEqual", IsComposable=true)]
		public System.Nullable<int> String_NotEqual([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(MAX)")] string string1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(MAX)")] string string2, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> comparison)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), string1, string2, comparison).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].String_Like", IsComposable=true)]
		public System.Nullable<int> String_Like([global::System.Data.Linq.Mapping.ParameterAttribute(Name="string", DbType="NVarChar(MAX)")] string @string, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(MAX)")] string pattern, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NChar(1)")] System.Nullable<char> escape, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> comparison)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), @string, pattern, escape, comparison).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STContains", IsComposable=true)]
		public System.Nullable<int> Geography_STContains([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STWithin", IsComposable=true)]
		public System.Nullable<int> Geography_STWithin([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STCrosses", IsComposable=true)]
		public System.Nullable<int> Geography_STCrosses([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STDisjoint", IsComposable=true)]
		public System.Nullable<int> Geography_STDisjoint([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STDistance", IsComposable=true)]
		public System.Nullable<double> Geography_STDistance([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<double>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STEquals", IsComposable=true)]
		public System.Nullable<int> Geography_STEquals([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STIntersects", IsComposable=true)]
		public System.Nullable<int> Geography_STIntersects([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STOverlaps", IsComposable=true)]
		public System.Nullable<int> Geography_STOverlaps([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="[Ogc.Filter].Geography_STTouches", IsComposable=true)]
		public System.Nullable<int> Geography_STTouches([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarBinary(MAX)")] System.Data.Linq.Binary geog2)
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), geog1, geog2).ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="[Ogc.Csw].Records")]
	public partial class Record
	{
		
		private string _Identifier;
		
		private string _Title;
		
		private string _Subject;
		
		private string _Description;
		
		private string _Date;
		
		private string _Type;
		
		private string _Format;
		
		private string _Relation;
		
		private string _Spatial;
		
		private System.Data.Linq.Binary _Coverage;
		
		private string _AnyText;
		
		public Record()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Identifier", DbType="VarChar(45)")]
		public string Identifier
		{
			get
			{
				return this._Identifier;
			}
			set
			{
				if ((this._Identifier != value))
				{
					this._Identifier = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(256)")]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this._Title = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Subject", DbType="NVarChar(512)")]
		public string Subject
		{
			get
			{
				return this._Subject;
			}
			set
			{
				if ((this._Subject != value))
				{
					this._Subject = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(MAX)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this._Description = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="VarChar(10)")]
		public string Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this._Date = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(50)")]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this._Type = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Format", DbType="VarChar(50)")]
		public string Format
		{
			get
			{
				return this._Format;
			}
			set
			{
				if ((this._Format != value))
				{
					this._Format = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Relation", DbType="VarChar(45)")]
		public string Relation
		{
			get
			{
				return this._Relation;
			}
			set
			{
				if ((this._Relation != value))
				{
					this._Relation = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Spatial", DbType="VarChar(50)")]
		public string Spatial
		{
			get
			{
				return this._Spatial;
			}
			set
			{
				if ((this._Spatial != value))
				{
					this._Spatial = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Coverage", DbType="VarBinary(MAX)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary Coverage
		{
			get
			{
				return this._Coverage;
			}
			set
			{
				if ((this._Coverage != value))
				{
					this._Coverage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnyText", DbType="NVarChar(MAX)")]
		public string AnyText
		{
			get
			{
				return this._AnyText;
			}
			set
			{
				if ((this._AnyText != value))
				{
					this._AnyText = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
