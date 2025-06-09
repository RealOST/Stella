using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor {
    public override void OnInspectorGUI() {
        WeaponData data = (WeaponData)target;

        // 绘制 weaponPatterns（Unity 默认绘制）
        DrawDefaultInspector();

        // 限制 weaponPower 范围
        int maxPower = data.weaponPatterns != null ? data.weaponPatterns.Length - 1 : 0;
        data.defaultWeaponPower = EditorGUILayout.IntSlider("Weapon Power", data.defaultWeaponPower, 0, Mathf.Max(0, maxPower));

        // 保存修改
        if (GUI.changed) {
            EditorUtility.SetDirty(data);
        }
    }
}
#endif