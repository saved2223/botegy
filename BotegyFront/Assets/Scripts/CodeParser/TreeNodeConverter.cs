using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Model;
using ValueType = Model.ValueType;

namespace CodeParser
{
    public static class TreeNodeConverter
    {
        private static readonly List<string> BuiltIn = new List<string>()
        {
            "true",
            "false",
            "Warrior",
            "StrongWarrior",
            "Archer",
            "Magician",
            "Producer",
            "Healer"
        };
        public static BlockProgram Convert(ITree tree)
        {
            BlockProgram pr = new BlockProgram();
        
            pr.SetBlocks(ConvertStmtList(tree.GetChild(0)));

            return pr;
        }

        private static List<ICode> ConvertStmtList(ITree tree)
        {
            List<ICode> scope = new List<ICode>();
            for (int i = 0; i < tree.ChildCount; i++)
            {
                scope.Add(ConvertStatement(tree.GetChild(i).GetChild(0)));
            }

            return scope;
        }

        private static IStatement ConvertStatement(ITree tree)
        {
            if (tree.GetType() == typeof(ParserGrammarParser.Simple_stmtContext))
            {
                if (tree.GetChild(0).GetType() == typeof(ParserGrammarParser.AssignContext))
                {

                    return ConvertAssign(tree.GetChild(0));
                }
            
                return ConvertFunction(tree.GetChild(0));
            
            }

            if (tree.GetType() == typeof(ParserGrammarParser.WhileContext))
            {
                return ConvertWhile(tree);
            }
            return ConvertIf(tree);
        }

        private static FunctionBlock ConvertFunction(ITree tree)
        {
            FunctionBlock b = new FunctionBlock();

            b.Function = (Functions)Enum.Parse(typeof(Functions), tree.GetChild(0).GetChild(0).ToStringTree().ToUpper());

            List<IExpression> args = new List<IExpression>();
        
            for (int i = 2; i < tree.ChildCount; i++)
            {
                if (tree.GetChild(i).GetType() == typeof(ParserGrammarParser.ExprContext))
                {
                    args.Add(ConvertExpression(tree.GetChild(i)));
                }
            }

            b.Arguments = args;

            return b;
        }

        private static ConditionBlock ConvertIf(ITree tree)
        {
            ConditionBlock b = new ConditionBlock();

            IExpression cond = ConvertExpression(tree.GetChild(2));
            List<ICode> scope = ConvertStmtList(tree.GetChild(4).GetChild(0).GetChild(1));

            if (tree.ChildCount > 5)
            {
                List<ICode> elseScope = ConvertStmtList(tree.GetChild(6).GetChild(0).GetChild(1));
                b.ElseScope = elseScope;
            }

            b.Condition = cond;
            b.Scope = scope;

            return b;
        }

        private static LoopBlock ConvertWhile(ITree tree)
        {
            LoopBlock b = new LoopBlock
            {
                Cond = ConvertExpression(tree.GetChild(2)),
                Code = ConvertStmtList(tree.GetChild(4).GetChild(0).GetChild(1))
            };

            return b;
        }

        private static AssignBlock ConvertAssign(ITree tree)
        {
            AssignBlock b = new AssignBlock
            {
                VariableBlock = ConvertVariable(tree.GetChild(0)),
                ExpressionBlock = ConvertExpression(tree.GetChild(2))
            };

            return b;
        }

        private static IExpression ConvertExpression(ITree tree)
        {
            if (tree.GetType() == typeof(ParserGrammarParser.CallContext))
            {
                return ConvertFunction(tree);
            }
        
            if (tree.GetType() == typeof(ParserGrammarParser.ValueContext))
            {
                return ConvertValue(tree);
            }
            if (tree.GetType() == typeof(ParserGrammarParser.IdentifierContext))
            {
                if (BuiltIn.Contains(tree.GetChild(0).GetChild(0).ToStringTree()))
                    return ConvertValue(tree.GetChild(0));
            
                return ConvertVariable(tree);
            }
            if (tree.GetType() == typeof(ParserGrammarParser.BracesContext))
            {
                return ConvertBraces(tree);
            }
            if (tree.ChildCount > 1)
            {
                return ConvertBinaryOperator(tree);
            }
            return ConvertExpression(tree.GetChild(0));
        }

        private static VariableBlock ConvertVariable(ITree tree)
        {
            VariableBlock v = new VariableBlock();
            if (tree.GetChild(0).ChildCount > 0)
            {
                v.Name = tree.GetChild(0).GetChild(0).ToStringTree();
            }
            else
                v.Name = tree.GetChild(0).ToStringTree();

            return v;
        }
        private static ValueBlock ConvertValue(ITree tree)
        {
            ValueBlock b = new ValueBlock();

            string value = tree.GetChild(0).ToStringTree();

            if (value == "true" || value == "false")
            {
                b.Type = ValueType.BOOL;
            }
            else if (Int32.TryParse(value, out _))
            {
                b.Type = ValueType.INT; 
            }
            else if (Enum.TryParse<Unites>(value, true, out _))
            {
                b.Type = ValueType.UNITE;
            }
            else
            {
                b.Type = ValueType.FLOAT;
            }

            b.Value = value;

            return b;
        }

        private static BracesBlock ConvertBraces(ITree tree)
        {
            BracesBlock b = new BracesBlock
            {
                ExpressionBlock = ConvertExpression(tree.GetChild(1))
            };

            return b;
        }

        private static BinaryOperatorBlock ConvertBinaryOperator(ITree tree)
        {
            IExpression ex1 = ConvertExpression(tree.GetChild(0));
            IExpression ex2 = ConvertExpression(tree.GetChild(2));
            BinaryOperator op = BinaryOperatorExtensions.GetValueFromDescription(tree.GetChild(1).ToStringTree());
            return new BinaryOperatorBlock(op, ex1, ex2);
        }
    }
}