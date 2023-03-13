using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapToolMain : MonoBehaviour
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    Vector3 CursorPosition = new Vector3(0, 0, 0);

    public GameObject TempTile;
    public GameObject MapTile;

    [MenuItem("Window/MapTool")]
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MapToolMain));
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnGUI()
    {
        GUILayout.Label("Base Setting", EditorStyles.boldLabel);
        TempTile = (GameObject)EditorGUILayout.ObjectField("오브젝트 필드 11", TempTile, typeof(GameObject), true);
        MapTile = (GameObject)EditorGUILayout.ObjectField("오브젝트 필드", MapTile, typeof(GameObject), true);
        myString = EditorGUILayout.TextField("Text Field", myString);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("버튼 생성", GUILayout.Width(120), GUILayout.Height(30)))
        {

        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);

        EditorGUILayout.EndToggleGroup(); //토글 그룹 끝
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.activeGameObject != null) //Plane 포서된 activeGameObject를 없애주거나 TempTile에 포커스 되도록 설정
        {
            if (Selection.activeGameObject.name == "Plane")
            {
                Debug.Log(Selection.activeGameObject.name);
                Selection.activeGameObject = TempTile;
            }
        }
        if (Event.current.type != EventType.MouseDown || Event.current.button !=0) return; //마우스 입력 외에는 받지 않겠다

        var mousePosition = Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint; //pixel 포인트에서 마우스 포지션을 가져오는
        mousePosition.y = Camera.current.pixelHeight - mousePosition.y;

        var Ray = Camera.current.ScreenPointToRay(mousePosition); //Scene 카메라에서 Ray를 쓴다

        if(Physics.Raycast(Ray, out RaycastHit hit)) //받은 Hit 포인트를 out
        {
            Vector3Int TempVector = new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
            GameObject Temp = (GameObject)PrefabUtility.InstantiatePrefab(MapTile);
            Temp.transform.position = TempVector;
        }

        Selection.activeGameObject = TempTile;
    }
}
