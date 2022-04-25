using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToBlockConverter
{
    public BlockProgram Convert(Transform _codePanel)
    {
        BlockProgram program = new BlockProgram();
        
        program.SetBlocks(ConvertStmtContainer(_codePanel));

        return program;
    }

    private List<ICode> ConvertStmtContainer(Transform block)
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

    private StatementBlock ConvertStatement(Transform block)
    {
        StatementBlock stmt = new StatementBlock();

        stmt.VariableBlock = ConvertVariable(block.Find("Image/Variable/VariableBlock"));
        stmt.ExpressionBlock = ConvertExpression(block.Find("Image/ExprContainerBlock"));

        return stmt;
    }

    private ConditionBlock ConvertCondition(Transform block)
    {
        ConditionBlock c = new ConditionBlock();

        c.Cond = ConvertExpression(block.Find("Image/Condition/ExprContainerBlock"));
        c.Code = ConvertStmtContainer(block.Find("Image/Scope/StatementContainerBlock"));

        return c;
    }

    private LoopBlock ConvertLoop(Transform block)
    {
        LoopBlock c = new LoopBlock();

        c.Cond = ConvertExpression(block.Find("Image/Condition/ExprContainerBlock"));
        
        c.Type = (LoopBlock.LoopType) Enum.Parse(typeof(LoopBlock.LoopType),
            block.Find("Image/Condition/Text").GetComponent<Text>().text.ToUpper());
        c.Code = ConvertStmtContainer(block.Find("Image/Scope/StatementContainerBlock"));

        return c;
    }

    private FunctionBlock ConvertFunction(Transform block)
    {
        FunctionBlock f = new FunctionBlock();

        f.Function = (FunctionBlock.Functions) Enum.Parse(typeof(FunctionBlock.Functions),
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
    private VariableBlock ConvertVariable(Transform block)
    {
        if (block == null)
            return new VariableBlock();
        
        VariableBlock var = new VariableBlock();

        var.Name = block.Find("Image/Text").GetComponent<Text>().text;

        return var;
    }
    
    private GlobalVariableBlock ConvertGlobalVariable(Transform block)
    {
        if (block == null)
            return new GlobalVariableBlock();
        
        GlobalVariableBlock var = new GlobalVariableBlock();


        var.GlobalVariable = (GlobalVariableBlock.Globals)Enum.Parse(typeof(GlobalVariableBlock.Globals),
            block.Find("Image/Text").GetComponent<Text>().text.ToUpper());
        
        return var;
    }

    private ValueBlock ConvertValue(Transform block)
    {
        if (block == null)
            return new ValueBlock();
        
        ValueBlock val = new ValueBlock();

        val.Value = block.Find("Image/Text").GetComponent<Text>().text; 
        return val;
    }

    private BinaryOperatorBlock ConvertBinaryOperator(Transform block)
    {
        BinaryOperatorBlock bin = new BinaryOperatorBlock();

        bin.Operator =
            BinaryOperatorExtensions.GetValueFromDescription(block.Find("Image/Text").GetComponent<Text>().text);
        
        bin.Expr1 = ConvertExpression(block.Find("Image/ExprContainerBlock1"));
        bin.Expr2 = ConvertExpression(block.Find("Image/ExprContainerBlock2"));

        return bin;
    }

    private BracesBlock ConvertBraces(Transform block)
    {
        BracesBlock br = new BracesBlock();

        br.ExpressionBlock = ConvertExpression(block.Find("Image/ExprContainerBlock"));

        return br;
    }

    private IExpression ConvertExpression(Transform block)
    {
        if (block.childCount > 0)
        {
            switch (block.GetChild(0).name)
            {
                case "VarBlock":
                    return ConvertVariable(block.GetChild(0));
                case "GlobalVarBlock":
                    return ConvertGlobalVariable(block.GetChild(0));
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