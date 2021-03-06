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
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Ows100=GeoSik.Ogc.Ows.V100.Types;

namespace GeoSik.Ogc.Ows.ServiceModel
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Handles OWS exceptions according to [OGC 06-121r9, §8].</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class ErrorHandler:
        IErrorHandler
    {

        /// <summary>Indicates that the current session should not be aborted, whatever exception has been raised.</summary>
        /// <param name="error">The exception that has been raised.</param>
        /// <returns><c>true</c></returns>
        public bool HandleError(Exception error)
        {
            return true;
        }

        /// <summary>Creates a fault message from the specified exception.</summary>
        /// <param name="error">The exception that has been raised.</param>
        /// <param name="version">The SOAP version of the message.</param>
        /// <param name="fault">The fault message that is returned to the client.</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var aex=error as AggregateException;
            if (aex!=null)
                if (aex.InnerExceptions.Count>1)
                    error=new OwsException(OwsExceptionCode.NoApplicableCode, _Reason, error);
                else
                    error=aex.InnerExceptions[0];

            var fex=error as FaultException;
            if (fex==null)
            {
                if (error is NotImplementedException)
                    error=new OwsException(OwsExceptionCode.OperationNotSupported, _Reason, error);

                var oex=error as OwsException;
                if (oex!=null)
                {
                    // [OGC 06-121r9, §8.6]
                    var status=HttpStatusCode.InternalServerError;
                    switch (oex.Code)
                    {
                    case OwsExceptionCode.InvalidParameterValue:
                    case OwsExceptionCode.InvalidUpdateSequence:
                    case OwsExceptionCode.MissingParameterValue:
                        status=HttpStatusCode.BadRequest;
                        break;
                    case OwsExceptionCode.OperationNotSupported:
                    case OwsExceptionCode.OptionNotSupported:
                        status=HttpStatusCode.NotImplemented;
                        break;
                    }
                    fex=new WebFaultException<Ows100.ExceptionReport>((Ows100.ExceptionReport)oex, status);
                } else
                {
                    FaultCode fc=FaultCode.CreateReceiverFaultCode("InternalServerError", "http://schemas.microsoft.com/2009/WebFault");
                    fex=new FaultException(_Reason, fc);
                }
            }

            fault=CreateMessage(fex, version);
        }

        /// <summary>Creates a fault message from the specified parameters.</summary>
        /// <param name="fex">The fault exception to create a message from.</param>
        /// <param name="version">The SOAP version of the message.</param>
        /// <returns>A fault message.</returns>
        protected virtual Message CreateMessage(FaultException fex, MessageVersion version)
        {
            MessageFault mf=fex.CreateMessageFault();
            return Message.CreateMessage(version, mf, null);
        }

        private const string _Reason="A server error has occured.";
    }
}
