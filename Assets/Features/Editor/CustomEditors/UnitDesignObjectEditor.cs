using System.Text;
using Features.GameDesign.Units;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitDesignObject))]
public class UnitDesignObjectEditor : Editor
{
    private static readonly GUIStyle _labelStyle = new();
    private static readonly StringBuilder _stringCache = new();

    private SerializedProperty _key;
    private SerializedProperty _prefabs;

    private SerializedProperty _healthParameter;
    private bool _healthFoldout;

    private SerializedProperty _speedParameter;
    private bool _speedFoldout;

    private SerializedProperty _damageParameter;
    private bool _damageFoldout;

    private SerializedProperty _attackDistanceParameter;
    private bool _attackDistanceFoldout;

    private SerializedProperty _attackCooldownParameter;
    private bool _attackCooldownFoldout;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_key);

        EditorGUILayout.Space(8f);

        DrawUnitParameter("Speed", _speedParameter, ref _speedFoldout);
        DrawUnitParameter("Health", _healthParameter, ref _healthFoldout);
        DrawUnitParameter("Damage", _damageParameter, ref _damageFoldout);
        DrawUnitParameter("Attack Distance", _attackDistanceParameter, ref _attackDistanceFoldout);
        DrawUnitParameter("Attack Cooldown", _attackCooldownParameter, ref _attackCooldownFoldout);

        EditorGUILayout.Space(8f);

        EditorGUILayout.PropertyField(_prefabs);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawUnitParameter(string parameterName, SerializedProperty property, ref bool foldOut)
    {
        _stringCache.Clear();

        EditorGUILayout.LabelField(parameterName, _labelStyle);
        EditorGUILayout.Space(6);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        var baseValue = property.FindPropertyRelative("BaseValue");
        baseValue.floatValue = EditorGUILayout.Slider("Base", baseValue.floatValue, 0.001f, 10000);

        var growthFactor = property.FindPropertyRelative("GrowthFactor");
        growthFactor.floatValue = EditorGUILayout.Slider("Coefficient", growthFactor.floatValue, 0.0001f, 100);
        EditorGUILayout.EndHorizontal();

        foldOut = EditorGUILayout.Foldout(foldOut, "Details", EditorStyles.foldoutHeader);
        if (foldOut)
        {
            EditorGUILayout.Space(6f);
            _stringCache.Append("Tier 1: ").Append(baseValue.floatValue).AppendLine()
                .Append("Tier 2: ").Append(baseValue.floatValue * growthFactor.floatValue).AppendLine()
                .Append("Tier 3: ").Append(baseValue.floatValue * (growthFactor.floatValue * growthFactor.floatValue));

            EditorGUILayout.HelpBox(_stringCache.ToString(), MessageType.Info);
        }

        EditorGUILayout.Space(8f);
    }

    private void OnEnable()
    {
        _labelStyle.alignment = TextAnchor.MiddleCenter;
        _labelStyle.normal.textColor = Color.white;

        _key = serializedObject.FindProperty("_key");

        _healthParameter = serializedObject.FindProperty("_healthParameter");
        _speedParameter = serializedObject.FindProperty("_speedParameter");
        _damageParameter = serializedObject.FindProperty("_damageParameter");
        _attackDistanceParameter = serializedObject.FindProperty("_attackDistanceParameter");
        _attackCooldownParameter = serializedObject.FindProperty("_attackCooldownParameter");

        _prefabs = serializedObject.FindProperty("_prefabs");
    }

    private void OnDisable()
    {
        _stringCache.Clear();
    }
}