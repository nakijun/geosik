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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ProjNet.CoordinateSystems;

namespace GeoSik.Ogc.Gml.V311
{

#pragma warning disable 3008, 3009
    partial class _Geometry:
        ISimpleGeometry
    {

        internal protected abstract void InternalPopulate(IGeometrySink sink);

        public void Populate(IGeometrySink sink)
        {
            if (srsName!=null)
                sink.SetCoordinateSystem(CoordinateSystem);

            InternalPopulate(sink);
        }

        public Envelope Envelope()
        {
            var ret=new Envelope();
            PopulateEnvelope(ret);
            return ret;
        }

        protected virtual void PopulateEnvelope(Envelope envelope)
        {
            Populate(envelope);
        }

        internal virtual void BeginFigure(double x, double y, double? z)
        {
            //TODO: turn to abstract method
            throw new NotImplementedException();
        }

        internal virtual void AddLine(double x, double y, double? z)
        {
            //TODO: turn to abstract method
            throw new NotImplementedException();
        }

        internal virtual void EndFigure()
        {
        }

        ISimpleGeometry ISimpleGeometry.Envelope()
        {
            return Envelope();
        }

        public ICoordinateSystem CoordinateSystem
        {
            get
            {
                if (srsName==null)
                    return GeographicCoordinateSystem.WGS84;

                return CoordinateSystemProvider.Instance.GetById(Srid.CreateFromCrs(srsName));
            }
            internal set
            {
                if (value!=null)
                {
                    srsName=new Srid((int)value.AuthorityCode).Crs;
                    srsDimension=value.Dimension;
                } else
                {
                    srsName=null;
                    srsDimension=null;
                }
            }
        }

        //public virtual IGeometry Geometry
        //{
        //    get
        //    {
        //        var builder=new SqlGeometryBuilder();
        //        if (srsName!=null)
        //            builder.SetSrid(Srid.CreateFromCrs(srsName).Value);
        //        else
        //            builder.SetSrid(4326); // WGS84

        //        Populate(builder);

        //        return builder.ConstructedGeometry;
        //    }
        //    set
        //    {
        //        var sb=new StringBuilder();
        //        using (XmlWriter xw=XmlWriter.Create(sb))
        //            value.WriteXml(xw);
        //        Untyped=XElement.Parse(sb.ToString(), LoadOptions.None);
        //    }
        //}
    }
#pragma warning restore 3008, 3009
}
