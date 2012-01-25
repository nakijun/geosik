﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.Types;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace OgcToolkit.Ogc.WebCatalog.Cql.Ast
{

#pragma warning disable 3001, 3009
    public sealed class GeoOperatorRoutineNode:
        AstNode,
        IExpressionBuilder
    {

        internal class GeoOperatorExpressionCreator:
            ExpressionCreator<GeoOperatorRoutineNode>
        {

            public GeoOperatorExpressionCreator(GeoOperatorRoutineNode op):
                base(op)
            {
            }

            protected override Expression CreateStandardExpression(IEnumerable<Expression> subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                throw new NotSupportedException("Only custom implementations are supported");
            }

            protected override string GetCustomImplementationName(List<Type> paramTypes, List<object> paramValues)
            {
                return Node._OperatorName;
            }

            protected override Expression CreateCustomExpression(MethodInfo method, object instance, IEnumerable<Expression> subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                Expression op=base.CreateCustomExpression(method, instance, subexpr, parameters, subType);

                Type rt=Nullable.GetUnderlyingType(method.ReturnType) ?? method.ReturnType;
                if (method.ReturnType==typeof(bool))
                    return op;
                else
                    return Expression.Equal(
                        op,
                        Expression.Constant(Convert.ChangeType(true, rt, CultureInfo.InvariantCulture), method.ReturnType)
                    );
            }
        }

        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            _OperatorName=((OperatorNameNode)treeNode.MappedChildNodes[0].AstNode).Name;
            _AttributeName=(AttributeNameNode)AddChild("", treeNode.MappedChildNodes[1]);
            _GeometryLiteral=(GeometryLiteralNode)AddChild("", treeNode.MappedChildNodes[2]);

            AsString=_OperatorName;
        }

        public Expression CreateExpression(ExpressionBuilderParameters parameters, Type expectedStaticType, Func<Expression, Expression> operatorCreator)
        {
            return GetExpressionCreator().CreateExpression(parameters);
        }

        private IExpressionCreator GetExpressionCreator()
        {
            return new GeoOperatorExpressionCreator(this);
        }

        Type IExpressionBuilder.GetExpressionStaticType(ExpressionBuilderParameters parameters)
        {
            return typeof(bool);
        }

        private string _OperatorName;
        private AttributeNameNode _AttributeName;
        private GeometryLiteralNode _GeometryLiteral;
    }
#pragma warning restore 3001, 3009
}
