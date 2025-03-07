using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// FSM代码生成器窗口：只需配置一个名字及状态列表，自动生成主FSM类及各个状态类
/// 例如：名字为 "Player"，状态为 "Idle,Move,Attack"
/// 则生成：PlayerFSM、PlayerStateType、PlayerParameters、PlayerIdleState 等
/// </summary>
public class FSMGenerator : EditorWindow
{
    // 配置项
    private string entityName = "Player";
    private string stateNames = "Idle,Move,Attack";

    [MenuItem("Tools/FSM代码生成器")]
    public static void ShowWindow()
    {
        GetWindow<FSMGenerator>("FSM代码生成器");
    }

    private void OnGUI()
    {
        GUILayout.Label("FSM生成设置", EditorStyles.boldLabel);
        entityName = EditorGUILayout.TextField("名字", entityName);
        stateNames = EditorGUILayout.TextField("状态名称（逗号分隔）", stateNames);

        if (GUILayout.Button("生成FSM代码"))
        {
            GenerateFSM();
        }
    }

    private void GenerateFSM()
    {
        // 解析状态名称
        string[] states = stateNames.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = states[i].Trim();
        }

        // 定义各个类名称
        string fsmClassName = entityName + "FSM";
        string stateEnumName = entityName + "StateType";
        string parameterClassName = entityName + "Parameters";
        string statePrefix = entityName;
        string stateSuffix = "State";

        string folderPath = $"Assets/Scripts/Enemy/{entityName}";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 生成FSM主类代码
        string fsmContent = GenerateFSMContent(fsmClassName, stateEnumName, parameterClassName, statePrefix, stateSuffix, states);
        string fsmFilePath = Path.Combine(folderPath, fsmClassName + ".cs");
        File.WriteAllText(fsmFilePath, fsmContent);

        // 生成每个状态类代码文件
        foreach (string state in states)
        {
            string stateFileContent = GenerateStateContent(state, fsmClassName, parameterClassName, statePrefix, stateSuffix);
            string stateFilePath = Path.Combine(folderPath, statePrefix + state + stateSuffix + ".cs");
            File.WriteAllText(stateFilePath, stateFileContent);
        }

        AssetDatabase.Refresh();
        Debug.Log("FSM代码生成完成，生成目录：" + folderPath);
    }

    /// <summary>
    /// 生成FSM主类代码，包含状态枚举、参数类及 Start 中的状态注册代码
    /// </summary>
    private string GenerateFSMContent(string fsmName, string enumName, string paramName, string statePrefix, string stateSuffix, string[] states)
    {
        // 生成状态枚举代码
        string enumContent = "public enum " + enumName + "\n{\n";
        for (int i = 0; i < states.Length; i++)
        {
            enumContent += "    " + states[i];
            if (i != states.Length - 1)
                enumContent += ",";
            enumContent += "\n";
        }
        enumContent += "}\n";

        // 生成 Start 方法中状态注册代码（使用带前缀和后缀的状态类名）
        string registrationContent = "";
        for (int i = 0; i < states.Length; i++)
        {
            string stateClassName = statePrefix + states[i] + stateSuffix;
            registrationContent += "        state.Add(" + enumName + "." + states[i] + ", new " + stateClassName + "(this));\n";
        }

        // FSM主类模板代码
        string fsmTemplate =
$@"using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

{enumContent}
[Serializable]
public class {paramName}
{{
    public {enumName} currentState;
}}

public class {fsmName} : EnemyFSM
{{
    public {paramName} parameters;
    public IState currentState;
    public Dictionary<{enumName}, IState> state = new Dictionary<{enumName}, IState>();

    void Start()
    {{
{registrationContent}        ChangeState({enumName}.{states[0]});
    }}

    void Update()
    {{
        currentState.OnUpdate();
    }}

    void FixedUpdate()
    {{
        currentState.OnFixedUpdate();
    }}

    public void ChangeState({enumName} stateType)
    {{
        if (currentState != null)
        {{
            currentState.OnExit();
        }}
        currentState = state[stateType];
        currentState.OnEnter();
        parameters.currentState = stateType;
    }}
}}
";
        return fsmTemplate;
    }

    /// <summary>
    /// 生成单个状态类代码模板，类名格式：{entityName}{state}{stateSuffix} 例如 PlayerIdleState
    /// </summary>
    private string GenerateStateContent(string state, string fsmName, string paramName, string statePrefix, string stateSuffix)
    {
        string stateClassName = statePrefix + state + stateSuffix;
        string stateTemplate =
$@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class {stateClassName} : IState
{{
    private {fsmName} fSM;
    private {paramName} parameters;

    public {stateClassName}({fsmName} fSM)
    {{
        this.fSM = fSM;
        this.parameters = fSM.parameters;
    }}

    public void OnEnter()
    {{
        
    }}

    public void OnExit()
    {{
    }}

    public void OnFixedUpdate()
    {{
    }}

    public void OnUpdate()
    {{
    }}
}}
";
        return stateTemplate;
    }
}
