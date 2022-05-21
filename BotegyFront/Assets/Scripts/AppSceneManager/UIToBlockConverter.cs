using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace AppSceneManager
{
    public static class UIToBlockConverter
    {
        public static BlockProgram Convert(Transform codePanel)
        {
            BlockProgram program = new BlockProgram();

            program.SetBlocks(ConvertStmtContainer(codePanel));

            return program;
        }

        private static List<ICode> ConvertStmtContainer(Transform block)
        {
            List<ICode> blocks = new List<ICode>();

            foreach (Transform child in block)
            {
                if (child.gameObject.activeSelf)
                {
                    switch (child.name)
                    {
                        case "StmtBlock":
                        {
                            blocks.Add(ConvertStatement(child));
                            break;
                        }
                        case "ConditionBlock":
                        {
                            blocks.Add(ConvertCondition(child));
                            break;
                        }
                        case "LoopBlock":
                        {
                            blocks.Add(ConvertLoop(child));
                            break;
                        }
                        case "FunctionBlock":
                        {
                            blocks.Add(ConvertFunction(child));
                            break;
                        }
                    }
                }
            }

            return blocks;
        }

        private static AssignBlock ConvertStatement(Transform block)
        {
            AssignBlock stmt = new AssignBlock();

            stmt.VariableBlock = ConvertVariable(block.Find("Image/Variable/VarBlock"));
            stmt.ExpressionBlock = ConvertExpression(block.Find("Image/ExprContainerBlock"));

            return stmt;
        }

        private static ConditionBlock ConvertCondition(Transform block)
        {
            ConditionBlock c = new ConditionBlock();

            foreach (Transform child in block.Find("Image"))
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.name == "Option")
                    {
                        IExpression ifExpr = ConvertExpression(child.Find("Condition/ExprContainerBlock"));
                        List<ICode> ifScope = ConvertStmtContainer(child.Find("Scope/StatementContainerBlock"));

                        c.Condition = ifExpr;
                        c.Scope = ifScope;
                    }

                    else
                    {
                        List<ICode> elseScope = ConvertStmtContainer(child.Find("Scope/StatementContainerBlock"));
                        c.ElseScope = elseScope;
                    }
                }
            }

            return c;
        }

        private static LoopBlock ConvertLoop(Transform block)
        {
            LoopBlock c = new LoopBlock();

            c.Cond = ConvertExpression(block.Find("Image/Condition/ExprContainerBlock"));
            c.Code = ConvertStmtContainer(block.Find("Image/Scope/StatementContainerBlock"));

            return c;
        }

        private static FunctionBlock ConvertFunction(Transform block)
        {
            FunctionBlock f = new FunctionBlock();

            f.Function = (Functions) Enum.Parse(typeof(Functions),
                block.Find("Image/Text").GetComponent<Text>().text.ToUpper());

            foreach (Transform child in block.Find("Image/Arguments"))
            {
                if (child.gameObject.activeSelf && child.name.Contains("Block"))
                {
                    f.AddArgument(ConvertExpression(child));
                }
            }

            return f;
        }

        private static VariableBlock ConvertVariable(Transform block)
        {
            if (block == null)
                return new VariableBlock();

            VariableBlock var = new VariableBlock();

            var.Name = block.Find("Image/Text").GetComponent<Text>().text;

            return var;
        }

        private static ValueBlock ConvertValue(Transform block)
        {
            if (block == null)
                return new ValueBlock();

            ValueBlock val = new ValueBlock();

            val.Value = block.Find("Image/Text").GetComponent<Text>().text;
            var script1 = block.GetComponent<ValueTypeScript>();
            val.Type = script1.ValueType;

            return val;
        }

        private static BinaryOperatorBlock ConvertBinaryOperator(Transform block)
        {
            BinaryOperatorBlock bin = new BinaryOperatorBlock();

            bin.Operator =
                BinaryOperatorExtensions.GetValueFromDescription(block.Find("Image/Text").GetComponent<Text>().text);

            bin.Expr1 = ConvertExpression(block.Find("Image/ExprContainerBlock1"));
            bin.Expr2 = ConvertExpression(block.Find("Image/ExprContainerBlock2"));

            return bin;
        }

        private static BracesBlock ConvertBraces(Transform block)
        {
            BracesBlock br = new BracesBlock();

            br.ExpressionBlock = ConvertExpression(block.Find("Image/ExprContainerBlock"));

            return br;
        }

        private static IExpression ConvertExpression(Transform block)
        {
            if (block.childCount > 0)
            {
                switch (block.GetChild(0).name)
                {
                    case "VarBlock":
                        return ConvertVariable(block.GetChild(0));
                    case "ValBlock":
                        return ConvertValue(block.GetChild(0));
                    case "BinOpBlock":
                        return ConvertBinaryOperator(block.GetChild(0));
                    case "BracesBlock":
                        return ConvertBraces(block.GetChild(0));
                    case "FunctionBlock":
                        return ConvertFunction(block.GetChild(0));
                }
            }

            return null;
        }
    }
}