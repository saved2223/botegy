using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodePanelManager : MonoBehaviour
{
    [SerializeField] private Transform codePanel;
    
    [SerializeField] private GameObject varPrefab;
    [SerializeField] private GameObject valPrefab;
    [SerializeField] private GameObject binOpPrefab;
    [SerializeField] private GameObject bracesPrefab;
    [SerializeField] private GameObject arrAccPrefab;
    [SerializeField] private GameObject loopPrefab;
    [SerializeField] private GameObject ifPrefab;
    [SerializeField] private GameObject stmtPrefab;
    [SerializeField] private GameObject funcPrefab;
    [SerializeField] private GameObject exprContainer;

    [SerializeField] private TextInputWindow inputTextDialog;
    [SerializeField] private DropDownWindow dropDownDialog;
    [SerializeField] private IndicesInputWindow indicesInputDialog;
    [SerializeField] private ConditionWindow conditionDialog;
   
    //program comes from server, now it is created by hand
    private BlockProgram _program;

    private GameObject _selectedObject;

    private void Awake()
    {
        _program = new BlockProgram();

        VariableBlock v1 = new VariableBlock("str1");
        ValueBlock v2 = new ValueBlock("'2'");
        StatementBlock b = new StatementBlock(v1, v2);

        ConditionBlock c = new ConditionBlock();
        c.Conditions.Add(v1);
        c.Conditions.Add(v2);
        c.Scopes.Add(new List<ICode>());
        c.Scopes.Add(new List<ICode>());
        c.Scopes[0].Add(b);
        c.ElseScope.Add(b);
        _program.AddBlock(c);
        // //is called if program came from server
        DrawProgram();
    }

    public void SetSelectedObject(GameObject obj)
    {
        //add deselect by click on codepanel
        if (obj == null)
        {
            if (_selectedObject != null)
            {
                Color c = _selectedObject.GetComponent<Image>().color;
                c.a = 0f;
                _selectedObject.GetComponent<Image>().color = c;
                _selectedObject = null;
            }
        }
        
        else if (_selectedObject == obj)
        {
            Color c = _selectedObject.GetComponent<Image>().color;
            c.a = 0f;
            _selectedObject.GetComponent<Image>().color = c;
            _selectedObject = null;
        }
        else
        {
            if (_selectedObject != null)
            {
                Color c = _selectedObject.GetComponent<Image>().color;
                c.a = 0f;
                _selectedObject.GetComponent<Image>().color = c;
            }            
            
            _selectedObject = obj;
            Color c1 = _selectedObject.GetComponent<Image>().color;
            c1.a = 0.25f;
            _selectedObject.GetComponent<Image>().color = c1;
        }
    }
    
    public void SetProgram(BlockProgram program)
    {
        _program = program;
    }
    
    public void AddBlock(string category, string tClass)
    {
        if (_selectedObject == null)
        {
            if (category != "Expression")
            {
                InstantiateBlock(tClass, codePanel);
            }
        }
        else
        {
            if (category == "Function" || category == _selectedObject.GetComponent<CategoryScript>().Category)
            {
                if (_selectedObject.name.Contains("Container"))
                {
                    InstantiateBlock(tClass, _selectedObject.transform);
                }
                else
                {
                    Transform parent = _selectedObject.transform.parent;
                    
                    if (_selectedObject.GetComponent<CategoryScript>().Category == "Expression" && parent.childCount > 0)
                        return;
                    
                    InstantiateBlock(tClass, parent);

                    parent.GetChild(parent.childCount - 1)
                        .SetSiblingIndex(_selectedObject.transform.GetSiblingIndex() + 1);
                    
                }
            }
        }
    }

    private void Clear()
    {
        foreach (Transform child in codePanel)
        {
            Destroy(child.gameObject);
        }
    }
    private void DrawProgram()
    {
        foreach (var block in _program.GetBlocks())
        {
            InstantiateBlock(block, codePanel);
        }
    }

    private void InstantiateBlock(string tClass, Transform parent)
    {
        if (tClass == null)
            return;

        switch (tClass)
        {
            case "VariableBlock":
                InstantiateVariableBlock(parent);
                break;
            case "ValueBlock":
                InstantiateValueBlock(parent);
                break;
            case "BinaryOperatorBlock":
                InstantiateBinaryOperatorBlock(parent);
                break;
            case "BracesBlock":
                InstantiateBracesBlock(parent);
                break;
            case "ArrayAccessBlock":
                InstantiateArrayAccessBlock(parent);
                break;

            case "StatementBlock":
                InstantiateStatementBlock(parent);
                break;
            case "ConditionBlock":
                InstantiateConditionBlock(parent);
                break;
            case "LoopBlock":
                InstantiateLoopBlock(parent);
                break;
            case "FunctionBlock":
                InstantiateFunctionBlock(parent);
                break;
        }
    }

    private void InstantiateBlock(ICode b, Transform parent)
    {
        if (b.GetType() == typeof(FunctionBlock))
            InstantiateFunctionBlock(parent, (FunctionBlock)b);
            
        else if (typeof(IExpression).IsAssignableFrom(b.GetType())) 
            InstantiateBlock((IExpression) b, parent);
        
        else if (typeof(IStatement).IsAssignableFrom(b.GetType()))
            InstantiateBlock((IStatement) b, parent);
    }
    /* Expressions Instantiate */
    private void InstantiateBlock(IExpression b, Transform parent)
    {
        if (b == null)
            return;

        if (b.GetType() == typeof(VariableBlock))
            InstantiateVariableBlock(parent, (VariableBlock) b);
        
        if (b.GetType() == typeof(GlobalVariableBlock))
            InstantiateVariableBlock(parent, (VariableBlock) b);

        else if (b.GetType() == typeof(BracesBlock)) 
            InstantiateBracesBlock(parent, (BracesBlock) b);
        
        else if (b.GetType() == typeof(ValueBlock)) 
            InstantiateValueBlock(parent, (ValueBlock) b);
        
        else if (b.GetType() == typeof(BinaryOperatorBlock)) 
            InstantiateBinaryOperatorBlock(parent, (BinaryOperatorBlock) b);
        
        else if (b.GetType() == typeof(ArrayAccessBlock)) 
            InstantiateArrayAccessBlock(parent, (ArrayAccessBlock) b);
    }

    private void InstantiateBlock(IStatement b, Transform parent)
    {
        if (b == null)
            return;
        
        if (b.GetType() == typeof(StatementBlock))
            InstantiateStatementBlock(parent, (StatementBlock) b);

        else if (b.GetType() == typeof(ConditionBlock))
            InstantiateConditionBlock(parent, (ConditionBlock) b);
        
        else if (b.GetType() == typeof(LoopBlock)) 
            InstantiateLoopBlock(parent, (LoopBlock) b);
    }
    
    
    /* Statements Instantiate */ 
    private void InstantiateStatementBlock(Transform parent, StatementBlock b = null)
    {
        RectTransform tr = Instantiate(stmtPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "StmtBlock";
        
        if (b == null)
            InstantiateBlock("VariableBlock", tr.Find("Image/Variable"));
        else
            InstantiateBlock(b.VariableBlock, tr.Find("Image/Variable"));

        if (b != null && b.ExpressionBlock != null)
        {
            InstantiateBlock(b.ExpressionBlock, tr.Find("Image/ExprContainerBlock"));
        }
        else
        {
            SetTriggers(tr.Find("Image/ExprContainerBlock"), "Expression");
        }

        
        SetTriggers(tr, "Statement");

    }
    
    private void InstantiateVariableBlock(Transform parent, VariableBlock b = null)
    {
        RectTransform tr = Instantiate(varPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);

        tr.name = "VarBlock";

        DeleteEmptyTrigger(parent);
        
        Text t = tr.Find("Image/Text").GetComponent<Text>();

        t.text = b == null ? "new_var" : b.Name;
        
        SetTriggers(tr, "Expression");
    }

    private void InstantiateValueBlock(Transform parent, ValueBlock b = null)
    {
        RectTransform tr = Instantiate(valPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "ValBlock";
        
        DeleteEmptyTrigger(parent);

        Text t = tr.Find("Image/Text").GetComponent<Text>();

        t.text = b == null ? "0" : b.Value;
        
        SetTriggers(tr, "Expression");
    }
    
    private void InstantiateBracesBlock(Transform parent, BracesBlock b = null)
    {
        RectTransform tr = Instantiate(bracesPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "BracesBlock";

        DeleteEmptyTrigger(parent);

        if (b != null && b.ExpressionBlock != null)
        {
            InstantiateBlock(b.ExpressionBlock, tr.Find("Image/ExprContainerBlock"));
        }
        else
        {
            SetTriggers(tr.Find("Image/ExprContainerBlock"), "Expression");
        }
        
        SetTriggers(tr, "Expression");
    }
    
    private void InstantiateArrayAccessBlock(Transform parent, ArrayAccessBlock b = null)
    {
        RectTransform tr = Instantiate(arrAccPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "ArrayAccessBlock";

        DeleteEmptyTrigger(parent);

        Transform brace1 = tr.Find("Image/Text1");
        Transform brace2 = tr.Find("Image/Text2");
        
        if (b != null)
        {
            tr.Find("Image/Text").GetComponent<Text>().text = b.Name;
            
            foreach (IExpression index in b.Indices)
            {
                GameObject b1 = Instantiate(brace1, tr.Find("Image/Indices")).gameObject;
                b1.SetActive(true);
                
                GameObject tr1 = Instantiate(exprContainer, tr.Find("Image/Indices"));
                tr1.SetActive(true);
            
                SetTriggers(tr1.transform, "Expression");

                if (index != null)
                {
                    InstantiateBlock(index, tr1.transform) ;
                }
                
                GameObject b2 = Instantiate(brace2, tr.Find("Image/Indices")).gameObject;
                b2.SetActive(true);
            }
        }
        else
        {
            GameObject b1 = Instantiate(brace1, tr.Find("Image/Indices")).gameObject;
            b1.SetActive(true);
                
            GameObject tr1 = Instantiate(exprContainer, tr.Find("Image/Indices"));
            tr1.SetActive(true);
            
            SetTriggers(tr1.transform, "Expression");

            GameObject b2 = Instantiate(brace2, tr.Find("Image/Indices")).gameObject;
            b2.SetActive(true);
        }
        
        SetTriggers(tr, "Expression");
    }
    
    private void InstantiateBinaryOperatorBlock(Transform parent, BinaryOperatorBlock b = null)
    {
        RectTransform tr = Instantiate(binOpPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "BinOpBlock";

        DeleteEmptyTrigger(parent);

        if (b!= null && b.Expr1 != null)
        {
            InstantiateBlock(b.Expr1, tr.Find("Image/ExprContainerBlock1"));
        }
        else
        {
            SetTriggers(tr.Find("Image/ExprContainerBlock1"), "Expression");
        }
        
        if (b!= null && b.Expr2 != null)
        {
            InstantiateBlock(b.Expr2, tr.Find("Image/ExprContainerBlock2"));
        }
        else
        {
            SetTriggers(tr.Find("Image/ExprContainerBlock2"), "Expression");
        }
        
        Text text = tr.GetComponentInChildren<Text>();
        
        text.text = b != null ? b.Operator.GetOperatorText() : BinaryOperator.ADD.GetOperatorText();
        
        SetTriggers(tr, "Expression");
    }

    /* Conditions Instantiation */
    private void InstantiateConditionBlock(Transform parent, ConditionBlock b = null)
    {
        RectTransform tr = Instantiate(ifPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "ConditionBlock";

        if (b != null)
        {
            if (b.Conditions.Count == 0 && b.Scopes.Count == 0)
            {
                SetTriggers(tr.Find("Image/Option/Condition/ExprContainerBlock"), "Expression");
                SetTriggers(tr.Find("Image/Option/Scope/StatementContainerBlock"), "Statement");
            }
            else
            {
                for (int i = 0; i < b.Conditions.Count; i++)
                {
                    string condPath = "";
                    string scopePath = "";

                    if (i == 0)
                    {
                        condPath = "Image/Option/Condition/ExprContainerBlock";
                        scopePath = "Image/Option/Scope/StatementContainerBlock";
                    }
                    else
                    {
                        Transform elif = tr.Find("Image/ElifOption");
                        RectTransform tr1 = Instantiate(elif, tr.Find("Image")).GetComponent<RectTransform>();
                        tr1.gameObject.SetActive(true);
                        tr1.name = "ElifOption" + i;

                        condPath = "Image/ElifOption" + i + "/Condition/ExprContainerBlock";
                        scopePath = "Image/ElifOption" + i + "/Scope/StatementContainerBlock";
                    }

                    if (b.Conditions[i] == null)
                        SetTriggers(tr.Find(condPath), "Expression");
                    else
                        InstantiateBlock(b.Conditions[i], tr.Find(condPath));

                    SetTriggers(tr.Find(scopePath), "Statement");

                    foreach (var c in b.Scopes[i])
                    {
                        InstantiateBlock(c, tr.Find(scopePath));
                    }
                }
            }

            if (b.ElseScope.Count > 0)
            {
                Transform elseOp = tr.Find("Image/ElseOption");
                RectTransform tr1 = Instantiate(elseOp, tr.Find("Image")).GetComponent<RectTransform>();
                tr1.gameObject.SetActive(true);

                tr1.name = "ElseOption";
                
                SetTriggers(tr1.Find("Scope/StatementContainerBlock"), "Statement");
                
                foreach (var c in b.ElseScope)
                {
                    InstantiateBlock(c, tr1.Find("Scope/StatementContainerBlock"));
                }
            }
            
        }

        else
        {
            SetTriggers(tr.Find("Image/Option/Condition/ExprContainerBlock"), "Expression");
            SetTriggers(tr.Find("Image/Option/Scope/StatementContainerBlock"), "Statement");
        }
        // if (b!= null && b.Cond != null)
        // {
        //     InstantiateBlock(b.Cond, tr.Find("Image/Condition/ExprContainerBlock"));
        // }
        // else
        // {
        //     BlockEventTrigger trig1 = tr.Find("Image/Condition/ExprContainerBlock").gameObject.AddComponent<BlockEventTrigger>();
        //     trig1.Manager = this;
        //     
        //     var script1 = tr.Find("Image/Condition/ExprContainerBlock").GetComponent<CategoryScript>();
        //     script1.Category = "Expression";
        // }
        //
        // if (b!= null && b.Code.Count > 0)
        // {
        //     foreach (var c in b.Code)
        //     {
        //         InstantiateBlock(c, tr.Find("Image/Scope/StatementContainerBlock"));
        //     }
        // }
        //
        // else
        // {
        //     BlockEventTrigger trig1 = tr.Find("Image/Scope/StatementContainerBlock").gameObject.AddComponent<BlockEventTrigger>();
        //     trig1.Manager = this;
        //     
        //     var script1 = tr.Find("Image/Scope/StatementContainerBlock").GetComponent<CategoryScript>();
        //     script1.Category = "Statement";
        // }
        
        SetTriggers(tr, "Statement");
    }
    
    /* Loops Instantiation */
    private void InstantiateLoopBlock(Transform parent, LoopBlock b = null)
    {
        RectTransform tr = Instantiate(loopPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);
        
        tr.name = "LoopBlock";
        
        tr.Find("Image/Condition/Text").GetComponent<Text>().text = b!= null ? 
            b.Type.ToString().ToLower() : LoopBlock.LoopType.FOR.ToString().ToLower();

        if (b!= null && b.Cond != null)
        {
            InstantiateBlock(b.Cond, tr.Find("Image/Condition/ExprContainerBlock"));
        }
        else
        {
            SetTriggers(tr.Find("Image/Condition/ExprContainerBlock"), "Expression");
        }

        if (b!= null && b.Code.Count > 0)
        {
            foreach (var c in b.Code)
            {
                InstantiateBlock(c, tr.Find("Image/Scope/StatementContainerBlock"));
            }
        }
        else
        {
            SetTriggers(tr.Find("Image/Scope/StatementContainerBlock"), "Statement");
        }

        SetTriggers(tr, "Statement"); 
    }


    private void InstantiateFunctionBlock(Transform parent, FunctionBlock b = null)
    {
        RectTransform tr = Instantiate(funcPrefab, parent).GetComponent<RectTransform>();
        tr.gameObject.SetActive(true);

        tr.name = "FunctionBlock";

        if (_selectedObject != null && _selectedObject.name == "ExprContainerBlock")
        {
            DeleteEmptyTrigger(_selectedObject.transform);
        }

        if (b != null)
        {
            tr.Find("Image/Text").GetComponent<Text>().text = b.Function.ToString().ToLower();
            CreateArguments(b, tr.Find("Image/Arguments"));
        }
        else
        {
            tr.Find("Image/Text").GetComponent<Text>().text = FunctionBlock.Functions.GETINFO.ToString().ToLower();
            CreateArguments(new FunctionBlock(), tr.Find("Image/Arguments"));
        }
        
        SetTriggers(tr, "Function");
    }

    private void CreateArguments(FunctionBlock b, Transform parent)
    {
        Transform text = parent.Find("Comma");
        
        for (int i = 0; i < (int) b.Function; i++)
        {
            GameObject tr = Instantiate(exprContainer, parent);
            tr.SetActive(true);
            
            SetTriggers(tr.transform, "Expression");

            if (b.Arguments.Count > i)
            {
                InstantiateBlock(b.Arguments[i], tr.transform);
            }
            
            if (i != (int) b.Function - 1)
            {
                Transform t = Instantiate(text, parent);
                t.gameObject.SetActive(true);
            } 
        }
    }

    private void CreateArguments(string func)
    {
        int num = (int) Enum.Parse(typeof(FunctionBlock.Functions), func.ToUpper());

        Transform tr = _selectedObject.transform.Find("Image/Arguments");

        int i = 0;
        
        foreach (Transform child in tr)
        {
            if (i > 1 && i < tr.childCount - 1)
                Destroy(child.gameObject);
            i++;
        }

        FunctionBlock f = new FunctionBlock((FunctionBlock.Functions)num);
        
        CreateArguments(f, tr);
    }

    public void DeleteBlock()
    {
        if (_selectedObject != null)
        {
            if (_selectedObject.name.Contains("Container"))
            {
                foreach (Transform child in _selectedObject.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            else
            {
                if (_selectedObject.GetComponent<CategoryScript>().Category == "Expression")
                {
                    SetTriggers(_selectedObject.transform.parent, "Expression");
                    

                    
                }

                Destroy(_selectedObject);
                _selectedObject = null;
            }
        }
    }

    public void EditBlock()
    {
        if (_selectedObject != null)
        {
            switch (_selectedObject.name)
            {
                case "VarBlock":
                {
                    ShowTextInputWindow("ВВЕДИТЕ ИМЯ ПЕРЕМЕННОЙ");
                    break;
                }
                case "ValBlock":
                {
                    ShowTextInputWindow("ВВЕДИТЕ ЗНАЧЕНИЕ");
                    break;
                }
                case "BinOpBlock":
                {
                    ShowDropDownWindow(typeof(BinaryOperatorBlock), _selectedObject.transform.Find("Image/Text"));
                    break;
                }
                case "LoopBlock":
                {
                    ShowDropDownWindow(typeof(LoopBlock), _selectedObject.transform.Find("Image/Condition/Text"));
                    break;
                }
                case "FunctionBlock":
                {
                    ShowDropDownWindow(typeof(FunctionBlock), _selectedObject.transform.Find("Image/Text"));
                    break;
                }
                case "ArrayAccessBlock":
                {
                    ShowArrayIndicesWindow();
                    break;
                }
                case "ConditionBlock":
                {
                    ShowConditionWindow();
                    break;
                }
            }
        }
    }

    private void ShowConditionWindow()
    {
        int counter = 0;
        bool hasElse = false;
        foreach (Transform child in _selectedObject.transform.Find("Image"))
        {
            if (child.name.Contains("ElifOption") && child.gameObject.activeSelf)
                counter++;

            if (child.name == "ElseOption" && child.gameObject.activeSelf)
                hasElse = true;
        }
        
        conditionDialog.Show(counter.ToString(), hasElse, () => { }, (s, e) =>
        {
            int input = Int32.Parse(s);
            if(input >= 0 && input < counter)
                DeleteElif(input, counter, hasElse);
            else if (input > counter)
                AddElif(input, counter, hasElse);
            
            AddElse(e, hasElse);
        });
    }

    private void AddElif(int input, int elifCount, bool hasElse)
    {
        Transform block = _selectedObject.transform.Find("Image/ElifOption");
        int i = elifCount;
        
        while (i < input)
        {
            Transform tr = Instantiate(block, _selectedObject.transform.Find("Image"));
            tr.gameObject.SetActive(true);
            
            SetTriggers(tr.Find("Condition/ExprContainerBlock"), "Expression");
            SetTriggers(tr.Find("Scope/StatementContainerBlock"), "Statement");

            i++;
        }

        if (hasElse)
        {
            _selectedObject.transform.Find("Image").
                GetChild(_selectedObject.transform.Find("Image").childCount - 1 - (input - elifCount)).SetAsLastSibling();
        }
    }

    private void DeleteElif(int input, int elifCount, bool hasElse)
    {
        Transform t = _selectedObject.transform.Find("Image");

        int i = t.childCount - 1;
        
        if (hasElse)
            i--;
        
        while (elifCount > input)
        {
            Destroy(t.GetChild(i).gameObject);
            elifCount--;
            i--;
        }
    }

    private void AddElse(bool input, bool hasElse)
    {
        if (input && !hasElse)
        {
            Transform elseOp = _selectedObject.transform.Find("Image/ElseOption");
            RectTransform tr1 = Instantiate(elseOp, _selectedObject.transform.Find("Image")).GetComponent<RectTransform>();
            tr1.gameObject.SetActive(true);

            tr1.name = "ElseOption";
                
            SetTriggers(tr1.Find("Scope/StatementContainerBlock"), "Statement");
        }

        if (!input && hasElse)
        {
            Transform e = _selectedObject.transform.Find("Image").GetChild(_selectedObject.transform.Find("Image").childCount - 1);
            Destroy(e.gameObject);
        }
    }

    private void ShowArrayIndicesWindow()
    {
        int children = _selectedObject.transform.Find("Image/Indices").childCount / 3;
        
        indicesInputDialog.Show(children.ToString(), () => { }, (s) =>
        {
            int input = Int32.Parse(s);
            if(input > 0 && input < children)
                DeleteIndices(input);
            else if (input > children)
                AddIndices(input);
        });
    }

    private void DeleteIndices(int input)
    {
        Transform t = _selectedObject.transform.Find("Image/Indices");
        int i = t.childCount - 1;
        while (i >= input * 3)
        {
            Destroy(t.GetChild(i).gameObject);
            i--;
        }
    }

    private void AddIndices(int input)
    {
        Transform brace1 = _selectedObject.transform.Find("Image/Text1");
        Transform brace2 = _selectedObject.transform.Find("Image/Text2");
        
        Transform t = _selectedObject.transform.Find("Image/Indices");
        int i = t.childCount / 3;
        while (i < input)
        {
            GameObject b1 = Instantiate(brace1, t).gameObject;
            b1.SetActive(true);
                
            GameObject tr1 = Instantiate(exprContainer, t);
            tr1.SetActive(true);
            
            SetTriggers(tr1.transform, "Expression");

            GameObject b2 = Instantiate(brace2, t).gameObject;
            b2.SetActive(true);
            
            i++;
        }
    }
    
    private void ShowTextInputWindow(string title)
    {
        string text = _selectedObject.GetComponentInChildren<Text>().text;

        inputTextDialog.Show(title, text, () => { }, (s) =>
        {
            if (s.Length > 0)
                _selectedObject.GetComponentInChildren<Text>().text = s;
        });
    }

    private void ShowDropDownWindow(Type tType, Transform parent)
    {
        string selected = parent.GetComponent<Text>().text;

        dropDownDialog.Show(selected, tType, () => { }, (str) =>
        {
            parent.GetComponent<Text>().text = str;
            
            if (tType == typeof(FunctionBlock))
                CreateArguments(_selectedObject.transform.Find("Image/Text").GetComponent<Text>().text);
        });
    }

    public void TestConvertToProgram()
    {
        Debug.Log(ConvertToProgram().toString());
    }
    
    public BlockProgram ConvertToProgram()
    {
        UIToBlockConverter c = new UIToBlockConverter();
        
        return c.Convert(codePanel);

    }

    private void SetTriggers(Transform parent, string category)
    {
        if (parent.GetComponent<BlockEventTrigger>() != null)
        {
            parent.GetComponent<BlockEventTrigger>().enabled = true;
        }

        else
        {
            BlockEventTrigger trig1 = parent.gameObject.AddComponent<BlockEventTrigger>();
            trig1.Manager = this;

            var script1 = parent.GetComponent<CategoryScript>();
            script1.Category = category;
        }
    }

    private void DeleteEmptyTrigger(Transform parent)
    {
        BlockEventTrigger clickTrigger = parent.GetComponent<BlockEventTrigger>();
        
        if (clickTrigger != null)
        {
            clickTrigger.enabled = false;
            SetSelectedObject(null);
        }
    }

}

