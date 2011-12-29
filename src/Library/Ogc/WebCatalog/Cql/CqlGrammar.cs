﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Irony.Parsing;
using Irony.Interpreter.Ast;
using Csw202=OgcToolkit.Services.Csw.V202;

namespace OgcToolkit.Ogc.WebCatalog.Cql
{

    [Language("CQL", "1.0", "OGC Common Catalogue Query Language 1.0 grammar")]
    public class CqlGrammar:
        Grammar
    {

        public CqlGrammar():
            base(true)
        {
            GrammarComments="OGC Common Catalogue Query Language grammar as defined in [OGC 07-006r1 §6.2.2].";

            // Terminals
            var identifier=new IdentifierTerminal("identifier", ":", "");
            identifier.StartCharCategories.AddRange(
                new UnicodeCategory[] {
                    UnicodeCategory.UppercaseLetter, //Ul
                    UnicodeCategory.LowercaseLetter  //Ll
                }
            );
            identifier.CharCategories.AddRange(
                new UnicodeCategory[] {
                    UnicodeCategory.UppercaseLetter, //Ul
                    UnicodeCategory.LowercaseLetter, //Ll
                    UnicodeCategory.DecimalDigitNumber //Nd
                }
            );
            var digit_literal=new FixedLengthLiteral("digit", 1, TypeCode.Byte);
            var numeric_literal=new NumberLiteral("numeric literal", NumberOptions.AllowSign);
            var unsigned_numeric_literal=new NumberLiteral("unsigned numeric literal", NumberOptions.None);
            var integer_literal=new NumberLiteral("integer literal", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
            var character_literal=new StringLiteral("character literal", "'", StringOptions.IsChar | StringOptions.AllowsDoubledQuote);
            var string_literal=new StringLiteral("string literal", "'", StringOptions.AllowsDoubledQuote);
            var left_paren=ToTerm("(", "left paren");
            var right_paren=ToTerm(")", "right paren");
            var period=ToTerm(".", "period");
            var comma=ToTerm(",", "comma");
            var AND=ToTerm("AND");
            var OR=ToTerm("OR");
            var NOT=ToTerm("NOT");
            var TRUE=ToTerm("TRUE");
            var FALSE=ToTerm("FALSE");
            var LIKE=ToTerm("LIKE");
            var NULL=ToTerm("NULL");
            var UNKNOWN=ToTerm("UNKNOWN");
            var EXISTS=ToTerm("EXISTS");
            var DOESNOTEXIST=ToTerm("DOESNOTEXIST");
            var equals_operator=ToTerm("=");
            var greater_than_operator=ToTerm(">");
            var less_than_operator=ToTerm("<");
            var not_equals_operator=ToTerm("<>");
            var greater_than_or_equals_operator=ToTerm(">=");
            var less_than_or_equals_operator=ToTerm("<=");
            var date_time=new RegexBasedTerminal(name: "date-time", pattern: @"(?<fulldate>(?<dateyear>\d{4})-(?<datemonth>\d{2})-(?<dateday>\d{2}))T(?<UTCtime>(?<timehour>\d{2}):(?<timeminute>\d{2}):(?<timesecond>\d{2}(\.\d+)?)Z)");
            var duration=new RegexBasedTerminal(name: "duration", pattern: @"P((?<duryear>\d+)Y)?((?<durmonth>\d+)M)?((?<durday>\d+)D)?(T((?<durhour>\d+)H)?((?<durminute>\d+)M)?((?<dursecond>\d+)S)?)?");

            NOT.AstNodeType=typeof(Ast.NotKeywordNode);
            date_time.AstNodeType=typeof(Ast.DateTimeLiteralNode);
            duration.AstNodeType=typeof(Ast.DurationLiteralNode);

            // Non terminals
            var optional_not=new NonTerminal("optional not", Empty | NOT);

            var attribute_name=new NonTerminal("attribute name") { AstNodeType=typeof(Ast.AttributeNameNode) };
            var search_condition=new NonTerminal("search condition");
            var boolean_value_expression=new NonTerminal("boolean value expression") { AstNodeType=typeof(Ast.BooleanValueExpressionNode) };
            var boolean_term=new NonTerminal("boolean term") { AstNodeType=typeof(Ast.BooleanTermNode) };
            var boolean_factor=new NonTerminal("boolean factor") { AstNodeType=typeof(Ast.BooleanFactorNode) };
            var boolean_primary=new NonTerminal("boolean primary");
            var predicate=new NonTerminal("predicate");
            var temporal_predicate=new NonTerminal("temporal predicate") { AstNodeType=typeof(Ast.TemporalPredicateNode) };
            var date_time_expression=new NonTerminal("date-time expression");
            var existence_predicate=new NonTerminal("existence predicate");
            var comparison_predicate=new NonTerminal("comparison predicate") { AstNodeType=typeof(Ast.ComparisonPredicateNode) };
            var text_predicate=new NonTerminal("text predicate");
            var null_predicate=new NonTerminal("null predicate");
            var comp_op=new NonTerminal("comp op", equals_operator | not_equals_operator | less_than_operator | greater_than_operator | less_than_or_equals_operator | greater_than_or_equals_operator);
            var general_literal=new NonTerminal("general literal");
            var boolean_literal=new NonTerminal("boolean literal", TRUE | FALSE | UNKNOWN);
            var routine_invocation=new NonTerminal("routine invocation");
            var geoop_routine=new NonTerminal("geoop routine");
            var relgeoop_routine=new NonTerminal("relgeoop routine");
            var routine=new NonTerminal("routine");
            var geoop_name=new NonTerminal("geoop name");
            var relgeoop_name=new NonTerminal("relgeoop name");
            var argument=new NonTerminal("argument");
            var positional_arguments=new NonTerminal("positional arguments");
            var tolerance=new NonTerminal("tolerance");
            var distance_units=new NonTerminal("distance units");

            var geometry_literal=new NonTerminal("geometry literal");
            var geometry_literal_series=new NonTerminal("geometry literal series");
            var point_tagged_text=new NonTerminal("Point Tagged Text");
            var linestring_tagged_text=new NonTerminal("LineString Tagged Text");
            var polygon_tagged_text=new NonTerminal("Polygon Tagged Text");
            var multipoint_tagged_text=new NonTerminal("MultiPoint Tagged Text");
            var multilinestring_tagged_text=new NonTerminal("MultiLineString Tagged Text");
            var multipolygon_tagged_text=new NonTerminal("MultiPolygon Tagged Text");
            var geometrycollection_tagged_text=new NonTerminal("GeometryCollection Tagged Text");
            var point_text=new NonTerminal("Point Text");
            var point_text_series=new NonTerminal("Point Text series");
            var point=new NonTerminal("Point");
            var point_series=new NonTerminal("Point series");
            var x=new NonTerminal("x");
            var y=new NonTerminal("y");
            var linestring_text=new NonTerminal("LineString Text");
            var linestring_text_series=new NonTerminal("LineString Text series");
            var polygon_text=new NonTerminal("Polygon Text");
            var polygon_text_series=new NonTerminal("Polygon Text series");
            var multipoint_text=new NonTerminal("MultiPoint Text");
            var multilinestring_text=new NonTerminal("MultiLineString Text");
            var multipolygon_text=new NonTerminal("MultiPolygon Text");
            var geometrycollection_text=new NonTerminal("GeometryCollection Text");
            var envelope_tagged_text=new NonTerminal("Envelope Tagged Text");
            var envelope_text=new NonTerminal("Envelope Text");
            var westboundlongitude=new NonTerminal("WestBoundLongitude", numeric_literal);
            var eastboundlongitude=new NonTerminal("EastBoundLongitude", numeric_literal);
            var northboundlongitude=new NonTerminal("NorthBoundLongitude", numeric_literal);
            var southboundlongitude=new NonTerminal("SouthBoundLongitude", numeric_literal);

            var date_time_period=new NonTerminal("date-time period") { AstNodeType=typeof(Ast.DateTimePeriodNode) };

            attribute_name.Rule=MakePlusRule(attribute_name, period, identifier);

            search_condition.Rule=boolean_value_expression;
            boolean_value_expression.Rule=MakePlusRule(boolean_value_expression, OR, boolean_term);
            boolean_term.Rule=MakePlusRule(boolean_term, AND, boolean_factor);
            boolean_factor.Rule=optional_not + boolean_primary;
            boolean_primary.Rule=predicate | routine_invocation | (left_paren + search_condition + right_paren);
            predicate.Rule=comparison_predicate | text_predicate | null_predicate | temporal_predicate | existence_predicate;
            temporal_predicate.Rule=(attribute_name + "BEFORE" + date_time_expression) | (attribute_name + "BEFORE" + "OR" + "DURING" + date_time_period) | (attribute_name + "DURING" + date_time_period) | (attribute_name + "DURING" + "OR" + "AFTER" + date_time_period) | (attribute_name + "AFTER" + date_time_expression);
            date_time_expression.Rule=date_time | date_time_period;
            existence_predicate.Rule=(attribute_name + EXISTS) | (attribute_name + DOESNOTEXIST);
            comparison_predicate.Rule=attribute_name + comp_op + general_literal;
            text_predicate.Rule=attribute_name + optional_not + LIKE + string_literal;
            null_predicate.Rule=attribute_name + "IS" + optional_not + NULL;
            general_literal.Rule=numeric_literal | string_literal | boolean_literal | date_time_expression | geometry_literal;
            routine_invocation.Rule=geoop_routine | relgeoop_routine | routine;
            geoop_name.Rule=ToTerm("EQUALS") | "DISJOINT" | "INTERSECTS" | "TOUCHES" | "CROSSES" | "WITHIN" | "CONTAINS" | "OVERLAPS" | "RELATE";
            relgeoop_name.Rule=ToTerm("DWITHIN") | "BEYOND";
            argument.Rule=general_literal | attribute_name;
            positional_arguments.Rule=MakePlusRule(positional_arguments, comma, argument);
            geoop_routine.Rule=geoop_name + left_paren + attribute_name + comma + geometry_literal + right_paren;
            relgeoop_routine.Rule=relgeoop_name + left_paren + attribute_name + comma + geometry_literal + comma + tolerance + right_paren;
            routine.Rule=attribute_name + left_paren + positional_arguments + right_paren;
            tolerance.Rule=unsigned_numeric_literal + comma + distance_units;
            distance_units.Rule=ToTerm("feet") | "meters" | ("statute" + "miles") | ("nautical" + "miles") | "kilometers";

            geometry_literal.Rule=point_tagged_text | linestring_tagged_text | polygon_tagged_text | multipoint_tagged_text | multilinestring_tagged_text | multipolygon_tagged_text | geometrycollection_tagged_text | envelope_tagged_text;
            geometry_literal_series.Rule=MakePlusRule(geometry_literal_series, comma, geometry_literal);
            point_tagged_text.Rule=ToTerm("POINT") + point_text;
            linestring_tagged_text.Rule=ToTerm("LINESTRING") + linestring_text;
            polygon_tagged_text.Rule=ToTerm("POLYGON") + polygon_text;
            multipoint_tagged_text.Rule=ToTerm("MULTIPOINT") + multipoint_text;
            multilinestring_tagged_text.Rule=ToTerm("MULTILINESTRING") + multilinestring_text;
            multipolygon_tagged_text.Rule=ToTerm("MULTIPOLYGON") + multipolygon_text;
            geometrycollection_tagged_text.Rule=ToTerm("GEOMETRYCOLLECTION") + geometrycollection_text;
            point_text.Rule=Empty | (left_paren + point + right_paren);
            point_text_series.Rule=MakePlusRule(point_text_series, comma, point_text);
            point.Rule=x + y;
            x.Rule=numeric_literal;
            y.Rule=numeric_literal;
            point_series.Rule=MakePlusRule(point_series, comma, point);
            linestring_text.Rule=Empty | (left_paren + point_series + right_paren);
            linestring_text_series.Rule=MakePlusRule(linestring_text_series, comma, linestring_text);
            polygon_text.Rule=Empty | (left_paren + linestring_text_series + right_paren);
            polygon_text_series.Rule=MakePlusRule(polygon_text_series, comma, polygon_text);
            multipoint_text.Rule=Empty | (left_paren + point_text_series + right_paren);
            multilinestring_text.Rule=Empty | (left_paren + linestring_text_series + right_paren);
            multipolygon_text.Rule=Empty | (left_paren + polygon_text_series + right_paren);
            geometrycollection_text.Rule=Empty | (left_paren + geometry_literal_series + right_paren);
            envelope_tagged_text.Rule=ToTerm("ENVELOPE") + envelope_text;
            envelope_text.Rule=Empty | left_paren + westboundlongitude + comma + eastboundlongitude + comma + northboundlongitude + comma + southboundlongitude + right_paren;

            date_time_period.Rule=(date_time + "/" + date_time) | (date_time + "/" + duration) | (duration + "/" + date_time);
            date_time_expression.Rule=date_time | date_time_period;

            // Operators
            OperatorMappings.Add(equals_operator.Text, ExpressionType.Equal, 10);
            OperatorMappings.Add(not_equals_operator.Text, ExpressionType.NotEqual, 10);
            OperatorMappings.Add(greater_than_operator.Text, ExpressionType.GreaterThan, 10);
            OperatorMappings.Add(greater_than_or_equals_operator.Text, ExpressionType.GreaterThanOrEqual, 10);
            OperatorMappings.Add(less_than_operator.Text, ExpressionType.LessThan, 10);
            OperatorMappings.Add(less_than_or_equals_operator.Text, ExpressionType.LessThanOrEqual, 10);
            OperatorMappings.Add(NOT.Text, ExpressionType.Not, 3);
            OperatorMappings.Add(OR.Text, ExpressionType.OrElse, 2);
            OperatorMappings.Add(AND.Text, ExpressionType.AndAlso, 1);

            // Grammar
            DefaultIdentifierNodeType=typeof(IdentifierNode);
            DefaultLiteralNodeType=typeof(LiteralValueNode);
            DefaultNodeType=typeof(NotSupportedNode);
            Delimiters="\"%&'()*+,-./:;<=>?[]^_|{}";
            MarkMemberSelect(":");
            MarkPunctuation(left_paren, right_paren, period, comma);
            MarkTransient(argument, boolean_primary, comp_op, date_time_expression, general_literal, geometry_literal, optional_not, predicate, search_condition);

            Root=search_condition;
            LanguageFlags=LanguageFlags.CreateAst;
        }
    }
}
