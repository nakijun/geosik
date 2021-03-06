﻿////////////////////////////////////////////////////////////////////////////////
//
// This file is part of GeoSIK.
// Copyright (C) 2012 Isogeo
//
// GeoSIK is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
//
// GeoSIK is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with GeoSIK. If not, see <http://www.gnu.org/licenses/>.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Common.Logging;
using Xml.Schema.Linq;
using GeoSik.Ogc.Ows;
using CqlQueryable=GeoSik.Ogc.WebCatalog.Cql.CqlQueryable;

namespace GeoSik.Ogc.WebCatalog.Csw.V202
{
    partial class Discovery
    {

        /// <summary>Base implementation of a GetRecordById request processor.</summary>
        public class GetRecordByIdProcessorBase:
            OgcRequestProcessor<Types.GetRecordById, Types.GetRecordByIdResponse>
        {

            /// <summary>Creates a new instance of the <see cref="GetRecordByIdProcessorBase" /> type.</summary>
            /// <param name="service">The discovery service this processor is associated to.</param>
            public GetRecordByIdProcessorBase(Discovery service) :
                base(service)
            {
            }

            /// <summary>Creates a <see cref="Types.GetRecordById" /> instance from the specified key/value parameters.</summary>
            /// <param name="parameters">The key/value parameters.</param>
            /// <returns>The request.</returns>
            protected override Types.GetRecordById CreateRequest(NameValueCollection parameters)
            {
                var request=new Types.GetRecordById();

                // [OCG 07-006r1 §10.9.4.1]
                string elementSetName=parameters[ElementSetNameParameter];
                if (!string.IsNullOrEmpty(elementSetName))
                    request.ElementSetName=new Types.ElementSetName() {
                        TypedValue=elementSetName
                    };

                // [OCG 07-006r1 §10.9.4.2]
                string[] ids=parameters.GetValues(IdParameter);
                if (ids!=null)
                {
                    IList<string> idl=string.Join(",", ids).Split(',').Where<string>(s => !string.IsNullOrWhiteSpace(s)).ToList<string>();
                    try
                    {
                        request.Id=idl.Select<string, Uri>(s => new Uri(s)).ToList<Uri>();
                    } catch (UriFormatException ufex)
                    {
                        var ex=new OwsException(OwsExceptionCode.InvalidParameterValue, ufex) {
                            Locator=IdParameter
                        };
                        ex.Data.Add(IdParameter, string.Join(",", idl ));
                        throw ex;
                    }
                }

                // [OCG 07-006r1 §10.9.4.3]
                string outputFormat=parameters[OutputFormatParameter];
                if (!string.IsNullOrEmpty(outputFormat))
                    request.outputFormat=outputFormat;

                // [OCG 07-006r1 §10.9.4.4]
                string outputSchema=parameters[OutputSchemaParameter];
                if (!string.IsNullOrEmpty(outputSchema))
                    try
                    {
                        request.outputSchema=new Uri(outputSchema);
                    } catch (UriFormatException ufex)
                    {
                        var ex=new OwsException(OwsExceptionCode.InvalidParameterValue, ufex) {
                            Locator=OutputSchemaParameter
                        };
                        ex.Data.Add(OutputSchemaParameter, outputSchema);
                        throw ex;
                    }

                return request;
            }

            /// <summary>Checks that the specified request is valid.</summary>
            /// <param name="request">The request to check.</param>
            protected override void CheckRequest(Types.GetRecordById request)
            {
                base.CheckRequest(request);

                if ((request.outputFormat!=null) && (Array.IndexOf<string>(OgcService.XmlMimeTypes, request.outputFormat)<0))
                {
                    var ex=new OwsException(OwsExceptionCode.InvalidParameterValue) {
                        Locator=OutputFormatParameter
                    };
                    ex.Data.Add(OutputFormatParameter, request.outputFormat);
                    throw ex;
                }

                if ((request.Id==null) || (request.Id.Count==0))
                    throw new OwsException(OwsExceptionCode.MissingParameterValue) {
                        Locator=IdParameter
                    };

                if (request.outputSchema==null)
                    request.outputSchema=new Uri(Namespaces.OgcWebCatalogCswV202);

                if (!((Discovery)Service).SupportedRecordTypes.Select<IXMetaData, XNamespace>(s => s.SchemaName.Namespace).Contains<XNamespace>(request.outputSchema.ToString()))
                {
                    var ex=new OwsException(OwsExceptionCode.InvalidParameterValue) {
                        Locator=OutputSchemaParameter
                    };
                    ex.Data.Add(OutputSchemaParameter, request.outputSchema);
                    throw ex;
                }

                //if (request.ElementSetName!=null)
                if (!request.Untyped.Elements("{http://www.opengis.net/cat/csw/2.0.2}ElementSetName").Any<XElement>() || string.IsNullOrEmpty(request.ElementSetName.TypedValue))
                    request.ElementSetName=new Types.ElementSetName(
                        new Types.ElementSetNameType() {
                            TypedValue="summary"
                        }
                    );

                //TODO: implement additional checks
            }

            /// <summary>Processes the specified request.</summary>
            /// <param name="request">The request to process.</param>
            /// <returns>The response to the specified request.</returns>
            protected override async Task<Types.GetRecordByIdResponse> ProcessRequestAsync(Types.GetRecordById request)
            {
                var ret=new Types.GetRecordByIdResponse();

                var namespaceManager=new XmlNamespaceManager(new NameTable());
                XNamespace dn=request.Untyped.GetDefaultNamespace();
                if (dn!=XNamespace.None)
                    namespaceManager.AddNamespace(string.Empty, dn.NamespaceName);
                var namespaces=from at in request.Untyped.Attributes()
                               where at.IsNamespaceDeclaration
                               select new {
                                   Prefix=at.Parent.GetPrefixOfNamespace(at.Value),
                                   Uri=at.Value
                               };
                namespaces.ToList().ForEach(n => namespaceManager.AddNamespace(n.Prefix, n.Uri));

                IQueryable records=((Discovery)Service).GetRecordsSource(null);
                records=Where(records, request.Id, namespaceManager, ((Discovery)Service).GetOperatorImplementationProvider());

                if (Logger.IsDebugEnabled)
                {
                    string t=records.ToTraceString();
                    if (!string.IsNullOrEmpty(t))
                        Logger.Debug(t);
                }

                // Performs the query
                var results=(await ((Discovery)Service).ConvertRecords((await records.ToListAsync()).Cast<IRecord>(), request.outputSchema, namespaceManager, request.ElementSetName.TypedValue))
                    .ToArray();

                // Core Record types
                var arl=results.OfType<Types.AbstractRecord>();
                ret.AbstractRecord=arl.ToList<Types.AbstractRecord>();

                // Other types
                var xsl=results.Except<IXmlSerializable>(arl.Cast<IXmlSerializable>());

                XmlWriterSettings xws=new XmlWriterSettings();
                xws.Indent=false;
                xws.NamespaceHandling=NamespaceHandling.OmitDuplicates;
                xws.OmitXmlDeclaration=true;

                foreach (IXmlSerializable xs in xsl)
                {
                    StringBuilder sb=new StringBuilder();
                    using (XmlWriter xw=XmlWriter.Create(sb))
                        xs.WriteXml(xw);

                    XElement element=XElement.Parse(sb.ToString(), LoadOptions.None);
                    ret.Untyped.Add(element);
                }

                return ret;
            }

            /// <summary>Gets a converter between a <see cref="Uri" /> and the specified type.</summary>
            /// <param name="destinationType">The type the converter should be able to convert <see cref="Uri" />s into.</param>
            /// <returns>The type converter.</returns>
            protected virtual TypeConverter GetIdentifierUriConverter(Type destinationType)
            {
                return new UriTypeConverter();
            }

            //TODO: compile LINQ expression?
            internal IQueryable Where(IQueryable source, IEnumerable<Uri> ids, XmlNamespaceManager namespaceManager=null, IOperatorImplementationProvider operatorImplementationProvider=null)
            {
                var parameters=new ParameterExpression[] {
                    Expression.Parameter(source.ElementType)
                };

                var xpqn=new XPathQueryableNavigator(source.ElementType, namespaceManager);
                XPathNodeIterator xpni=xpqn.Select(CoreQueryable.Identifier.Name, namespaceManager);
                if (xpni.MoveNext())
                {
                    var idn=(XPathQueryableNavigator)xpni.Current;
                    Type idType=idn.Type;
                    TypeConverter converter=GetIdentifierUriConverter(idType);

                    // Convert ids from Uris to identifier type
                    // urip => (idType)converter.ConvertTo(urip, idType)
                    var urip=Expression.Parameter(typeof(Uri));
                    var conex=Expression.Lambda(
                        typeof(Func<,>).MakeGenericType(typeof(Uri), idType),
                        Expression.Convert(
                            Expression.Call(
                                Expression.Constant(converter),
                                "ConvertTo",
                                null,
                                urip,
                                Expression.Constant(idType)
                            ),
                            idType
                        ),
                        urip
                    );
                    // var convertedIds=ids.Select<Uri, idType>(uri => (idType)converter.ConvertTo(uri, idType))
                    var urilistp=Expression.Parameter(typeof(IEnumerable<Uri>));
                    var convertids=Expression.Lambda(
                        Expression.Call(
                            typeof(Enumerable),
                            "Select",
                            new Type[] { typeof(Uri), idType },
                            Expression.Constant(ids),
                            conex
                        ),
                        urilistp
                    ).Compile();
                    var convertedIds=convertids.DynamicInvoke(ids);

                    // Creates the Where clause
                    LambdaExpression lambda=Expression.Lambda(
                        Expression.Call(
                            typeof(Enumerable),
                            "Contains",
                            new Type[] { idType },
                            Expression.Constant(convertedIds),
                            idn.CreateExpression(parameters[0])
                        ),
                        parameters
                    );
                    return source.Provider.CreateQuery(
                        Expression.Call(
                            typeof(Queryable),
                            "Where",
                            new Type[] { source.ElementType },
                            source.Expression,
                            Expression.Quote(lambda)
                        )
                    );
                }

                throw new InvalidOperationException();
            }
        }
    }
}
