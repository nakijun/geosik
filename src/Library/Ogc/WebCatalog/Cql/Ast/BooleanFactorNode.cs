﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace OgcToolkit.Ogc.WebCatalog.Cql.Ast
{

#pragma warning disable 3001, 3009
    public sealed class BooleanFactorNode:
        AstNode,
        IExpressionBuilder
    {

        internal class BooleanExpressionCreator:
            ExpressionCreator<BooleanFactorNode>
        {

            public BooleanExpressionCreator(BooleanFactorNode op):
                base(op)
            {
            }

            protected override Expression CreateStandardExpression(IEnumerable<Expression> subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                Expression ret=null;
                foreach (Expression ex in subexpr)
                {
                    if (ret!=null)
                        ret=Expression.MakeBinary(
                            Node.ExpressionType,
                            ret,
                            ex
                        );
                    else
                        ret=ex;
                }
                return ret;
            }

            protected override string GetCustomImplementationName(List<Type> paramTypes, List<object> paramValues)
            {
                return Node.ExpressionType.ToString();
            }

            protected override Expression CreateCustomExpression(MethodInfo method, object instance, IEnumerable<Expression> subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                Expression ret=null;
                foreach (Expression exp in subexpr)
                {
                    if (ret!=null)
                    {
                        if (instance!=null)
                            ret=Expression.Call(
                                Expression.Constant(instance),
                                method,
                                ret,
                                exp
                            );
                        else
                            ret=Expression.Call(
                                method,
                                ret,
                                exp
                            );

                        Type rt=Nullable.GetUnderlyingType(method.ReturnType)??method.ReturnType;
                        if (method.ReturnType!=typeof(bool))
                            ret=Expression.Equal(
                                ret,
                                Expression.Constant(Convert.ChangeType(true, rt, CultureInfo.InvariantCulture), method.ReturnType)
                            );
                    } else
                        ret=exp;
                }
                return ret;
            }
        }

        public BooleanFactorNode()
        {
            ExpressionType=ExpressionType.Not;
        }

        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            foreach (ParseTreeNode ptn in treeNode.MappedChildNodes)
            {
                AstNode node=(AstNode)ptn.AstNode;

                var on=node as NotKeywordNode;
                if (on!=null)
                    _OptionalNot=on;
                else
                    _Primary=AddChild("", ptn);
            }

            AsString="(boolean factor)";
        }

        public Expression CreateExpression(ExpressionBuilderParameters parameters, Type expectedStaticType, Func<Expression, Expression> operatorCreator)
        {
            if (_OptionalNot!=null)
                return GetExpressionCreator().CreateExpression(parameters);
            else
                return ((IExpressionBuilder)_Primary).CreateExpression(parameters, expectedStaticType, null);
        }

        Type IExpressionBuilder.GetExpressionStaticType(ExpressionBuilderParameters parameters)
        {
            return typeof(bool);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode=this;

            object ret=_Primary.Evaluate(thread);
            if (_OptionalNot!=null)
                ret=thread.Runtime.ExecuteUnaryOperator(ExpressionType.Not, ret, ref _LastUsed);

            thread.CurrentNode=Parent;
            return ret;
        }

        private IExpressionCreator GetExpressionCreator()
        {
            return new BooleanExpressionCreator(this);
        }

        private NotKeywordNode _OptionalNot;
        private AstNode _Primary;
        private OperatorImplementation _LastUsed;
    }
#pragma warning restore 3001, 3009
}
