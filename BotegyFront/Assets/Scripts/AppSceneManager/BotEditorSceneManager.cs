using System;
using System.Collections.Generic;
using ButtonScript;
using CodeParser;
using Dialog;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ValueType = Model.ValueType;

namespace AppSceneManager
{
    public class BotEditorSceneManager : MonoBehaviour
    {
        [SerializeField] private Transform codePanel;

        [SerializeField] private GameObject varPrefab;
        [SerializeField] private GameObject valPrefab;
        [SerializeField] private GameObject binOpPrefab;
        [SerializeField] private GameObject bracesPrefab;
        [SerializeField] private GameObject loopPrefab;
        [SerializeField] private GameObject ifPrefab;
        [SerializeField] private GameObject stmtPrefab;
        [SerializeField] private GameObject funcPrefab;
        [SerializeField] private GameObject exprContainer;

        [SerializeField] private TextInputWindow inputTextDialog;
        [SerializeField] private DropDownWindow dropDownDialog;
        [SerializeField] private ValueInputDialog valueInputDialog;
        [SerializeField] private ConditionWindow conditionDialog;

        [SerializeField] private HeaderManager headerManager;

        private GameObject _selectedObject;
        private RequestProcessor _processor;


        private void Awake()
        {
            AppMetrica.Instance.ReportEvent("BotEditorOpened");
            _processor = gameObject.AddComponent<RequestProcessor>();
            if (SceneInfo.EditedBot.code != null)
                DrawProgram(ProgramConverter.Convert(SceneInfo.EditedBot.code));
        }

        public void SetSelectedObject(GameObject obj)
        {
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
                if (category != "Function" &&
                    category != _selectedObject.GetComponent<CategoryScript>().Category) return;
                if (_selectedObject.name.Contains("Container"))
                {
                    InstantiateBlock(tClass, _selectedObject.transform);
                }
                else
                {
                    Transform parent = _selectedObject.transform.parent;

                    if (_selectedObject.GetComponent<CategoryScript>().Category == "Expression" &&
                        parent.childCount > 0)
                        return;

                    InstantiateBlock(tClass, parent);

                    parent.GetChild(parent.childCount - 1)
                        .SetSiblingIndex(_selectedObject.transform.GetSiblingIndex() + 1);
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

        private void DrawProgram(BlockProgram program)
        {
            Clear();
            foreach (var block in program.GetBlocks())
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
                InstantiateFunctionBlock(parent, (FunctionBlock) b);

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

            else if (b.GetType() == typeof(BracesBlock))
                InstantiateBracesBlock(parent, (BracesBlock) b);

            else if (b.GetType() == typeof(ValueBlock))
                InstantiateValueBlock(parent, (ValueBlock) b);

            else if (b.GetType() == typeof(BinaryOperatorBlock))
                InstantiateBinaryOperatorBlock(parent, (BinaryOperatorBlock) b);
        }

        private void InstantiateBlock(IStatement b, Transform parent)
        {
            if (b == null)
                return;

            if (b.GetType() == typeof(AssignBlock))
                InstantiateStatementBlock(parent, (AssignBlock) b);

            else if (b.GetType() == typeof(ConditionBlock))
                InstantiateConditionBlock(parent, (ConditionBlock) b);

            else if (b.GetType() == typeof(LoopBlock))
                InstantiateLoopBlock(parent, (LoopBlock) b);
        }


        /* Statements Instantiate */
        private void InstantiateStatementBlock(Transform parent, AssignBlock b = null)
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
            //save type to get it when constructing edit dialog
            RectTransform tr = Instantiate(valPrefab, parent).GetComponent<RectTransform>();
            tr.gameObject.SetActive(true);

            tr.name = "ValBlock";

            DeleteEmptyTrigger(parent);

            Text t = tr.Find("Image/Text").GetComponent<Text>();

            t.text = b == null ? "0" : b.Value;

            var script1 = tr.GetComponent<ValueTypeScript>();
            script1.ValueType = b == null ? ValueType.INT : b.Type;

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

        private void InstantiateBinaryOperatorBlock(Transform parent, BinaryOperatorBlock b = null)
        {
            RectTransform tr = Instantiate(binOpPrefab, parent).GetComponent<RectTransform>();
            tr.gameObject.SetActive(true);

            tr.name = "BinOpBlock";

            DeleteEmptyTrigger(parent);

            if (b != null && b.Expr1 != null)
            {
                InstantiateBlock(b.Expr1, tr.Find("Image/ExprContainerBlock1"));
            }
            else
            {
                SetTriggers(tr.Find("Image/ExprContainerBlock1"), "Expression");
            }

            if (b != null && b.Expr2 != null)
            {
                InstantiateBlock(b.Expr2, tr.Find("Image/ExprContainerBlock2"));
            }
            else
            {
                SetTriggers(tr.Find("Image/ExprContainerBlock2"), "Expression");
            }

            Text text = tr.Find("Image/Text").GetComponent<Text>();

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
                if (b.Condition == null)
                    SetTriggers(tr.Find("Image/Option/Condition/ExprContainerBlock"), "Expression");
                else
                    InstantiateBlock(b.Condition, tr.Find("Image/Option/Condition/ExprContainerBlock"));

                if (b.Scope.Count > 0)
                {
                    SetTriggers(tr.Find("Image/Option/Scope/StatementContainerBlock"), "Statement");

                    foreach (var c in b.Scope)
                    {
                        InstantiateBlock(c, tr.Find("Image/Option/Scope/StatementContainerBlock"));
                    }
                }
                
                SetTriggers(tr.Find("Image/Option/Scope/StatementContainerBlock"), "Statement");

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

            SetTriggers(tr, "Statement");
        }

        /* Loops Instantiation */
        private void InstantiateLoopBlock(Transform parent, LoopBlock b = null)
        {
            RectTransform tr = Instantiate(loopPrefab, parent).GetComponent<RectTransform>();
            tr.gameObject.SetActive(true);

            tr.name = "LoopBlock";


            if (b != null && b.Cond != null)
            {
                InstantiateBlock(b.Cond, tr.Find("Image/Condition/ExprContainerBlock"));
            }
            else
            {
                SetTriggers(tr.Find("Image/Condition/ExprContainerBlock"), "Expression");
            }

            if (b != null && b.Code.Count > 0)
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
                tr.Find("Image/Text").GetComponent<Text>().text = Functions.GET_SELF_HP.ToString().ToLower();
                CreateArguments(new FunctionBlock(), tr.Find("Image/Arguments"));
            }

            SetTriggers(tr, "Function");
        }

        private void CreateArguments(FunctionBlock b, Transform parent)
        {
            Transform text = parent.Find("Comma");

            int n = b.Function.GetArgumentsNum();
            for (int i = 0; i < n; i++)
            {
                GameObject tr = Instantiate(exprContainer, parent);
                tr.SetActive(true);

                SetTriggers(tr.transform, "Expression");

                if (b.Arguments.Count > i)
                {
                    InstantiateBlock(b.Arguments[i], tr.transform);
                }

                if (i != n - 1)
                {
                    Transform t = Instantiate(text, parent);
                    t.gameObject.SetActive(true);
                }
            }
        }

        private void CreateArguments(string func)
        {
            Functions function = (Functions) Enum.Parse(typeof(Functions), func, true);
            Transform tr = _selectedObject.transform.Find("Image/Arguments");

            foreach (Transform child in tr)
            {
                if (child.gameObject.activeSelf)
                    Destroy(child.gameObject);
            }

            FunctionBlock f = new FunctionBlock(function);

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
                    if (_selectedObject.name.Contains("VarBlock") &&
                        _selectedObject.transform.parent.name.Equals("Variable"))
                        return;

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
                        ShowValueInputWindow();
                        break;
                    }
                    case "BinOpBlock":
                    {
                        ShowDropDownWindow(typeof(BinaryOperatorBlock), _selectedObject.transform.Find("Image/Text"));
                        break;
                    }
                    case "FunctionBlock":
                    {
                        ShowDropDownWindow(typeof(FunctionBlock), _selectedObject.transform.Find("Image/Text"));
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
            bool hasElse = false;
            foreach (Transform child in _selectedObject.transform.Find("Image"))
            {
                if (child.name == "ElseOption" && child.gameObject.activeSelf)
                    hasElse = true;
            }

            conditionDialog.Show(hasElse, () => { }, (e) => { AddElse(e, hasElse); });
        }

        private void AddElse(bool input, bool hasElse)
        {
            if (input && !hasElse)
            {
                Transform elseOp = _selectedObject.transform.Find("Image/ElseOption");
                RectTransform tr1 = Instantiate(elseOp, _selectedObject.transform.Find("Image"))
                    .GetComponent<RectTransform>();
                tr1.gameObject.SetActive(true);

                tr1.name = "ElseOption";

                SetTriggers(tr1.Find("Scope/StatementContainerBlock"), "Statement");
            }

            if (!input && hasElse)
            {
                Transform e = _selectedObject.transform.Find("Image")
                    .GetChild(_selectedObject.transform.Find("Image").childCount - 1);
                Destroy(e.gameObject);
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

        private void ShowValueInputWindow()
        {
            ValueType t = _selectedObject.GetComponent<ValueTypeScript>().ValueType;

            valueInputDialog.SetType(t);
            string text = _selectedObject.GetComponentInChildren<Text>().text;

            valueInputDialog.Show(text, () => { }, (s) =>
            {
                _selectedObject.GetComponent<ValueTypeScript>().ValueType = valueInputDialog.GetValueType();

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


        public void SaveButton()
        {
            string name = headerManager.header.text;
            string code = UIToBlockConverter.Convert(codePanel).GetString();

            if (SceneInfo.EditedBot.id != null)
            {
                _processor.GeneratePutRequest("updateBot",
                    new Dictionary<string, string>()
                    {
                        {"botId", SceneInfo.EditedBot.id},
                        {"name", RequestProcessor.PrepareName(name)},
                        {"code", RequestProcessor.PrepareURL(code)}
                    }, SaveCoroutine);
            }
            else
            {
                _processor.GeneratePutRequest("addBot",
                    new Dictionary<string, string>()
                    {
                        {"name", RequestProcessor.PrepareName(name)},
                        {"userId", SceneInfo.CurrUser.id},
                        {"code",  RequestProcessor.PrepareURL(code)}
                    }, SaveCoroutine);
            }
        }

        private void SaveCoroutine()
        {
            SceneManager.LoadScene("BotListScene");

        }

        public void BackButtonClicked()
        {
            SceneManager.LoadScene("BotListScene");
        }
    }
}