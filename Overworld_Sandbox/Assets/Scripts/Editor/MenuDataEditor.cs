using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CanEditMultipleObjects]
public class MenuDataEditor : EditorWindow {

    public MenuData menuData;
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    [MenuItem ("Window/Menu Data Editor")]
    static void Init() {

        MenuDataEditor window = (MenuDataEditor)EditorWindow.GetWindow( typeof( MenuDataEditor ) );
        window.Show();

    }

    void OnGUI() {
        if (menuData != null) {

            SerializedObject serializedObject = new SerializedObject( this );
            // this is probably around where the problem is
            SerializedProperty serializedProperty = serializedObject.FindProperty("menuData");
            EditorGUILayout.PropertyField( serializedProperty, true );

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save")) {
                SaveMenuData();
            }
        }

        if (GUILayout.Button( "Load Stuff" )) {
            LoadMenuData();
        }
    }

    private void LoadMenuData() {

        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists( filePath )) {
            string dataAsJson = File.ReadAllText( filePath );
            menuData = JsonUtility.FromJson<MenuData>( dataAsJson );
            Debug.Log( JsonUtility.ToJson( menuData ) );

        } else {
            menuData = new MenuData();
        }

    }
	
    private void SaveMenuData() {

        string dataAsJson = JsonUtility.ToJson( menuData );
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText( filePath, dataAsJson );

    }

}
