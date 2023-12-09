using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TransparentReflection))]

public class TransparentReflectionEditor : Editor
{
    [MenuItem("GameObject/添加地板反射效果", false, -100)]
    public static void AddReflectionEffect()
    {
        if (null == Selection.activeGameObject) return;

        if (null == Selection.activeGameObject.GetComponent<TransparentReflection>())
            Selection.activeGameObject.AddComponent<TransparentReflection>();
    }

    protected TransparentReflection m_data = null;

    //序列化属性
    protected SerializedProperty _backGround;
    protected SerializedProperty _targetObjectList;
    protected SerializedProperty _invisibleRenderList;
    protected SerializedProperty _BlurSpreadSize;
    protected SerializedProperty _blurShader;

    protected SerializedProperty _RTSize;

    protected void OnEnable()
    {
        _targetObjectList = serializedObject.FindProperty("m_TargetObjectList");
        _invisibleRenderList = serializedObject.FindProperty("m_InvisibleRenderList");
        _blurShader = serializedObject.FindProperty("m_GaussBlurShader");
        _BlurSpreadSize = serializedObject.FindProperty("m_BlurSpreadSize");
        _RTSize = serializedObject.FindProperty("m_RTSize");
        m_data = target as TransparentReflection;
    }



    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        int selectindx = _RTSize.intValue == 512 ? 0 : 1;
        selectindx = EditorGUILayout.Popup("RTSize",selectindx, new string[] { "512", "1024" });
        if(EditorGUI.EndChangeCheck())
        {
            _RTSize.intValue = selectindx == 0 ? 512 : 1024;
        }
        EditorGUILayout.PropertyField(_targetObjectList, new GUIContent("镜像目标（仅美术预览添加）"), true);
        EditorGUILayout.PropertyField(_blurShader, new GUIContent("高斯Shader"), true);
        EditorGUILayout.PropertyField(_BlurSpreadSize, new GUIContent("模糊程度"), true);

        serializedObject.ApplyModifiedProperties();
        

        if (GUILayout.Button("刷新"))
        {
            m_data.UpdateAllRender();
        }
        if (GUILayout.Button("添加"))
        {
            m_data.AddSelection();
        }
        if (GUILayout.Button("删除"))
        {
            m_data.RemoveSelection();
        }
        
    }

}
