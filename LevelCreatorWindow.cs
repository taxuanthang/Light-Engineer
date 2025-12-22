using UnityEngine;
using UnityEditor;

public class LevelCreator2D : EditorWindow
{
    private LevelData2D data;
    private int selectedIndex = 0;
    private bool paintMode = false;
    private float gridSize = 1.0f;
    private Transform parentFolder;

    [MenuItem("Tools/2D Level Creator")]
    public static void ShowWindow() => GetWindow<LevelCreator2D>("2D Level Tool");

    private void OnGUI()
    {
        GUILayout.Label("2D LEVEL EDITOR SETTINGS", EditorStyles.boldLabel);
        
        data = (LevelData2D)EditorGUILayout.ObjectField("Level Data", data, typeof(LevelData2D), false);
        gridSize = EditorGUILayout.FloatField("Grid Size (Ô vuông)", gridSize);
        parentFolder = (Transform)EditorGUILayout.ObjectField("Parent Folder", parentFolder, typeof(Transform), true);

        if (data == null) {
            EditorGUILayout.HelpBox("Kéo file Level Data vào đây để bắt đầu!", MessageType.Warning);
            return;
        }

        // Hiển thị danh sách ảnh preview của Prefab
        string[] options = data.prefabs.ConvertAll(p => p != null ? p.name : "Empty").ToArray();
        selectedIndex = GUILayout.SelectionGrid(selectedIndex, options, 4);

        GUILayout.Space(10);
        paintMode = GUILayout.Toggle(paintMode, "BẬT CHẾ ĐỘ VẼ (SHIFT + CLICK)", "Button", GUILayout.Height(40));

        if (paintMode) EditorGUILayout.HelpBox("Shift + Click: Đặt vật thể\nCtrl + Click: Xóa vật thể", MessageType.Info);
        
        if (GUI.changed) SceneView.RepaintAll();
    }

    private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!paintMode || data == null || data.prefabs.Count <= selectedIndex) return;

        Event e = Event.current;

        // Chặn chọn trúng object khác khi đang vẽ
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        // 1. Tính toán vị trí chuột trong không gian 2D (Z=0)
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        float distance = -ray.origin.z / ray.direction.z;
        Vector3 worldPos = ray.GetPoint(distance);

        // 2. Snap vào lưới 2D
        Vector3 spawnPos = new Vector3(
            Mathf.Round(worldPos.x / gridSize) * gridSize,
            Mathf.Round(worldPos.y / gridSize) * gridSize,
            0
        );

        // 3. Vẽ Preview để dễ hình dung
        DrawPreview(spawnPos);

        // 4. Xử lý Click
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (e.shift) // Shift + Click = Vẽ
            {
                PlaceObject(spawnPos);
                e.Use();
            }
            else if (e.control) // Ctrl + Click = Xóa
            {
                RemoveObjectAt(spawnPos);
                e.Use();
            }
        }

        sceneView.Repaint();
    }

    private void DrawPreview(Vector3 pos)
    {
        Handles.color = new Color(0, 1, 0, 0.3f);
        Handles.DrawSolidDisc(pos, Vector3.forward, gridSize * 0.2f); // Vẽ tâm điểm
        Handles.DrawWireCube(pos, Vector3.one * gridSize); // Vẽ khung ô vuông
    }

    private void PlaceObject(Vector3 pos)
    {
        GameObject prefab = data.prefabs[selectedIndex];
        if (prefab == null) return;

        // Tránh đặt chồng lên nhau tại cùng 1 tọa độ
        if (CheckIfOccupied(pos)) return;

        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newObj.transform.position = pos;
        
        if (parentFolder != null) newObj.transform.parent = parentFolder;

        Undo.RegisterCreatedObjectUndo(newObj, "Place 2D Object");
    }

    private void RemoveObjectAt(Vector3 pos)
    {
        // Tìm object gần vị trí click nhất và xóa
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (Vector3.Distance(obj.transform.position, pos) < 0.1f && obj.scene.name != null)
            {
                Undo.DestroyObjectImmediate(obj);
                return;
            }
        }
    }

    private bool CheckIfOccupied(Vector3 pos)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (Vector3.Distance(obj.transform.position, pos) < 0.1f && obj.scene.name != null) return true;
        }
        return false;
    }
}
