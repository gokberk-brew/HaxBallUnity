using UnityEngine;
using UnityEditor;

public class JSONEditor : EditorWindow {
    public string playerPrefKey = "Not Selected" ;
	public string JSON = "" ;
	JSONObject j;
	Vector2 scroll ;
	static void Init() {
		GetWindow(typeof(JSONEditor));
	}
	void OnGUI() {
        GUILayout.Label(playerPrefKey, GUILayout.Width(300));



		scroll = EditorGUILayout.BeginScrollView(scroll);
		JSON = EditorGUILayout.TextArea(JSON, GUILayout.ExpandHeight(true));
//		JSON = EditorGUILayout.TextArea(JSON, GUILayout.MaxHeight(500), GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();

		GUI.enabled = !string.IsNullOrEmpty(JSON);

		if(GUILayout.Button("Save to Player Prefs")) {
#if PERFTEST
            Profiler.BeginSample("JSONParse");
			j = JSONObject.Create(JSON);
            Profiler.EndSample();
            Profiler.BeginSample("JSONStringify");
            j.ToString(true);
            Profiler.EndSample();
#else
			j = JSONObject.Create(JSON);
#endif
//			Debug.Log(j.ToString(true));
		}
		if(j) {
			//Debug.Log(System.GC.GetTotalMemory(false) + "");
			if(j.type == JSONObject.Type.NULL)
				GUILayout.Label("JSON fail");
			else {
                PlayerPrefs.SetString(playerPrefKey, j.ToString()) ;
                Close() ;
            }

		}
	}
}
