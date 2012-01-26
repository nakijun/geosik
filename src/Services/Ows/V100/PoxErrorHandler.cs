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
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Ows100=OgcToolkit.Ogc.Ows.V100;

namespace OgcToolkit.Services.Ows.V100
{

    public class PoxErrorHandler:
        ErrorHandler
    {

        protected override Message CreateMessage(FaultException fex, MessageVersion version)
        {
            var fexd=fex as WebFaultException<Ows100.ExceptionReport>;
            if (fexd==null)
            {
                var oex=new OwsException(OwsExceptionCode.NoApplicableCode, fex);
                fexd=new WebFaultException<Ows100.ExceptionReport>((Ows100.ExceptionReport)oex, HttpStatusCode.InternalServerError);
            }

            return WebOperationContext.Current.CreateXmlResponse<Ows100.ExceptionReport>(fexd.Detail);
        }

    }
}
