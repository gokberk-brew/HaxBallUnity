using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

using System.IO ;
using System.Collections.Generic ;

namespace UnityToolbarExtender.Examples
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle, popupButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold,
				fixedWidth = 70
			};
			popupButtonStyle = new GUIStyle("Popup")
			{
			
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchLeftButton
	{

		static int selectedPopup = 0;
//		public static int testLevelPopup = 0;

		static SceneSwitchLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static float buttonTimer = 0 ;
		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(new GUIContent("Del Prefs", "Double Click to Delete All PlayerPrefs"), GUILayout.Width(70) ))
			{
				if (Time.realtimeSinceStartup - buttonTimer < 0.3f) {
					PlayerPrefs.DeleteAll();
					Debug.LogWarning("All PlayerPrefs Deleted") ;
				}
				buttonTimer = Time.realtimeSinceStartup ;
			}

			int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
			string[] scenes = new string[sceneCount+1];
			scenes[0] = "Scene" ;
			for( int i = 0; i < sceneCount; i++ ) {
				scenes[i+1] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
			}			
			selectedPopup = EditorGUILayout.Popup(selectedPopup, scenes, GUILayout.Width(70)); 
			if (selectedPopup > 0) {
				SceneHelper.StartScene(scenes[selectedPopup]);
				selectedPopup = 0 ;
			}
			
			if (Directory.Exists(Application.dataPath + "/../PlayerPrefs")) {
				List<string> _files = new List<string>(Directory.GetFiles(Application.dataPath + "/../PlayerPrefs")) ;
				if (_files.Count > 0) {
					List<string> _options = new List<string>() ;
					_options.Add("All Prefs") ;
					for (int i = 0; i < _files.Count; i++) {
						_options.Add(Path.GetFileNameWithoutExtension(_files[i])) ;				
					}

					int _fileIndex = EditorGUILayout.Popup(0, _options.ToArray(), GUILayout.Width(70)); 
					if (_fileIndex > 0) {
						string path = _files[_fileIndex - 1];
						if (path.Length != 0)
						{
							var fileContent = File.ReadAllText(path);
							JSONObject _loadedJson = JSONObject.Create(fileContent) ;                        
							if (_loadedJson != null) 
							{
								PlayerPrefs.DeleteAll() ;
								for (int i = 0; i < _loadedJson.Count; i++)
								{
									if (_loadedJson.keys[i].EndsWith("_isFloat")) {
										continue ;
									}

									if (_loadedJson[i].IsObject) {
										PlayerPrefs.SetString(_loadedJson.keys[i], _loadedJson[i].ToString()) ;                                
									} else if (_loadedJson[i].IsNumber) {
										if (_loadedJson.HasField(_loadedJson.keys[i] + "_isFloat")) {
											PlayerPrefs.SetFloat(_loadedJson.keys[i], _loadedJson[i].f) ;
										} else {
											PlayerPrefs.SetInt(_loadedJson.keys[i], _loadedJson[i].i) ;
										}
									} else {
										PlayerPrefs.SetString(_loadedJson.keys[i], _loadedJson[i].str) ;
									}
								}
								Debug.LogWarning(path + " Loaded") ;
							}
						}

					}
				}
			}
			
		}
	}

	static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(string sceneName)
		{
			if(EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (sceneToOpen == null ||
			    EditorApplication.isPlaying || EditorApplication.isPaused ||
			    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				// need to get scene via search because the path to the scene
				// file contains the package version so it'll change over time
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(scenePath);
					// EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}
	}
}