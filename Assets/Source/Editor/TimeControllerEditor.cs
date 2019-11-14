using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (TimeController))]
public class TimeControllerEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TimeController timeSystem = (TimeController)target;
        EditorGUILayout.LabelField("Current Time: " + string.Format("{0:00}:{1:00}", timeSystem.hours, timeSystem.minutes), EditorStyles.boldLabel);
        timeSystem.timeRange = EditorGUILayout.Slider(timeSystem.timeRange, 0f, 1f);

        if (timeSystem.duration <= 0.5f)
        {
            timeSystem.duration = 0.5f;
        }
    }
}
