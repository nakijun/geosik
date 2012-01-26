﻿////////////////////////////////////////////////////////////////////////////////
//
// This file is part of OgcToolkit.
// Copyright (C) 2012 Isogeo
//
// OgcToolkit is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
//
// OgcToolkit is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with OgcToolkit. If not, see <http://www.gnu.org/licenses/>.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using Xunit.Extensions;

namespace OgcToolkit.Ogc.Filter.V110.Tests
{

    public class PropertyIsEqualToTests
    {

        [Theory]
        [InlineData(11, "10")]
        [InlineData(20, "10")]
        public void Filter_ShouldFindStringConstraint(int number, string selection)
        {
            string constraint=string.Format(
                CultureInfo.InvariantCulture,
                "<ogc:PropertyIsEqualTo><ogc:PropertyName>/SimpleType/String</ogc:PropertyName><ogc:Literal>{0}</ogc:Literal></ogc:PropertyIsEqualTo>",
                selection
            );

            var Filter=FilterTests.CreateFilter(constraint);
            var selected=FilterTests.CreateCollection(number).AsQueryable<FilterTests.SimpleType>().Where<FilterTests.SimpleType>(Filter);

            Assert.Equal<int>(1, selected.Count<FilterTests.SimpleType>());
            Assert.Equal<string>(selection, selected.First<FilterTests.SimpleType>().String);
        }
    }
}
