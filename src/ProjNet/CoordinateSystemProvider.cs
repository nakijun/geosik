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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ProjNetCS=ProjNet.CoordinateSystems;

namespace GeoSik.ProjNet
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Represents a coordinate system provider.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class CoordinateSystemProvider:
        ICoordinateSystemProvider
    {

        /// <summary>Creates a new instance of the <see cref="CoordinateSystemProvider" /> class.</summary>
        public CoordinateSystemProvider()
        {
            _CoordinateSystemFactory=new ProjNetCS.CoordinateSystemFactory();
            _WktDictionary=new Dictionary<Srid, string>();
        }

        /// <summary>Creates a coordinate system transformer from the specified coordinate systems.</summary>
        /// <param name="source">The source coordinate system.</param>
        /// <param name="target">The target coordinate system.</param>
        /// <returns>The resulting coordinate transformer.</returns>
        public CoordinatesTransformer CreateTransformer(ICoordinateSystem source, ICoordinateSystem target)
        {
            return new CoordinatesTransformer(source, target);
        }

        /// <summary>Creates the coordinate system from the specified WKT representation.</summary>
        /// <param name="text">The WKT representation of the coordinate system to create.</param>
        /// <returns>The coordinate system.</returns>
        public ICoordinateSystem CreateFromWkt(string text)
        {
            return new CoordinateSystem(_CoordinateSystemFactory.CreateFromWkt(text));
        }

        /// <summary>Gets the coordinate system with the specified <paramref name="id" />.</summary>
        /// <param name="id">The identifier of the coordinate system to get.</param>
        /// <returns>The coordinate system with the specified <paramref name="id" />.</returns>
        public CoordinateSystem GetById(Srid id)
        {
            // WGS84 by default
            if (id.Value==0)
                return Wgs84;

            CoordinateSystem ret=null;

            // Try custom implementation
            var args=new CreatingCoordinateSystemEventArgs(id);
            OnCreatingCoordinateSystem(args);

            if (args.CoordinateSystem!=null)
                ret=(CoordinateSystem)args.CoordinateSystem;
            else if (!string.IsNullOrEmpty(args.WellKnownText))
                ret=new CoordinateSystem(_CoordinateSystemFactory.CreateFromWkt(args.WellKnownText));

            if (ret==null)
            {
                // Has the id already been used ?
                if (_WktDictionary.ContainsKey(id))
                    ret=new CoordinateSystem(_CoordinateSystemFactory.CreateFromWkt(_WktDictionary[id]));
            }

            if (ret==null)
            {
                // Load resources in memory
                if (!_InternalRead)
                {
                    using (StreamReader sr=new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("GeoSik.ProjNet.Srid.csv")))
                        while (!sr.EndOfStream)
                        {
                            string[] def=sr.ReadLine().Split(new char[] { ';' }, 2);
                            if (def.Length>1)
                            {
                                Srid srid=new Srid(int.Parse(def[0], CultureInfo.InvariantCulture));
                                if (!_WktDictionary.ContainsKey(srid))
                                    _WktDictionary.Add(srid, def[1]);
                            }
                        }
                    _InternalRead=true;

                    if (_WktDictionary.ContainsKey(id))
                        ret=new CoordinateSystem(_CoordinateSystemFactory.CreateFromWkt(_WktDictionary[id]));
                }
            }

            if (ret!=null)
            {
                OnCreatedCoordinateSystem(new CreatedCoordinateSystemEventArgs(id, ret));
                return ret;
            }

            throw new InvalidOperationException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    SR.CouldNotFindCoordinateSystemDefinitionException,
                    id.Value
                )
            );
        }

        ICoordinateSystem ICoordinateSystemProvider.CreateFromWkt(string text)
        {
            return CreateFromWkt(text);
        }

        ICoordinatesTransformer ICoordinateSystemProvider.CreateTransformer(ICoordinateSystem source, ICoordinateSystem target)
        {
            return CreateTransformer(source, target);
        }

        ICoordinateSystem ICoordinateSystemProvider.GetById(Srid id)
        {
            return GetById(id);
        }

        private void OnCreatingCoordinateSystem(CreatingCoordinateSystemEventArgs e)
        {
            var eh=CreatingCoordinateSystem;
            if (eh!=null)
                eh(this, e);
        }

        private void OnCreatedCoordinateSystem(CreatedCoordinateSystemEventArgs e)
        {
            var eh=CreatedCoordinateSystem;
            if (eh!=null)
                eh(this, e);
        }

        /// <summary>Gets the WGS84 coordinate system.</summary>
        public CoordinateSystem Wgs84
        {
            get
            {
                return GetById(new Srid(4326));
            }
        }

        ICoordinateSystem ICoordinateSystemProvider.Wgs84
        {
            get
            {
                return Wgs84;
            }
        }

        /// <summary>Event triggered when a coordinate system has to be created.</summary>
        public event EventHandler<CreatingCoordinateSystemEventArgs> CreatingCoordinateSystem;
        /// <summary>Event triggered when a coordinate system has been created.</summary>
        public event EventHandler<CreatedCoordinateSystemEventArgs> CreatedCoordinateSystem;

        private Dictionary<Srid, string> _WktDictionary;
        private ProjNetCS.CoordinateSystemFactory _CoordinateSystemFactory;
        private bool _InternalRead;
    }
}
