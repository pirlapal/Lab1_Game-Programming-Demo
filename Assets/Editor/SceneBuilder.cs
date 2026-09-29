using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneBuilder : Editor
{
    [MenuItem("Lab1/Build Entire Scene")]
    static void Build()
    {
        if (!EditorUtility.DisplayDialog("Build Lab 1 Scene",
            "This will create a new scene with two rooms, a hallway, a player, camera, and 5 moving objects.\n\nContinue?",
            "Build It", "Cancel"))
            return;

        SetupGroundLayer();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        Material floorRoom1Mat = MakeMaterial("Floor_Room1", new Color(0.35f, 0.35f, 0.35f));
        Material floorRoom2Mat = MakeMaterial("Floor_Room2", new Color(0.12f, 0.12f, 0.3f));
        Material wallRoom1Mat = MakeMaterial("Wall_Room1", new Color(0.76f, 0.69f, 0.5f));
        Material wallRoom2Mat = MakeMaterial("Wall_Room2", new Color(0.55f, 0.14f, 0.14f));
        Material hallwayMat = MakeMaterial("Hallway", new Color(0.45f, 0.45f, 0.45f));
        Material platformMat = MakeMaterial("Platform", new Color(0.18f, 0.5f, 0.22f));
        Material movingMat = MakeMaterial("MovingObject", new Color(0.92f, 0.6f, 0.08f));
        Material playerMat = MakeMaterial("Player", new Color(0.2f, 0.4f, 0.85f));
        Material accentMat = MakeMaterial("Accent", new Color(0.6f, 0.2f, 0.6f));

        // ── Room 1 ──────────────────────────────────────────────────
        GameObject room1 = new GameObject("Room1");

        MakeBox("Floor", room1, V(0, 0, 0), V(20, 0.5f, 20), floorRoom1Mat, true);
        MakeBox("Wall_Back", room1, V(0, 3, 10), V(20, 6, 0.5f), wallRoom1Mat, false);
        MakeBox("Wall_Left", room1, V(-10, 3, 0), V(0.5f, 6, 20), wallRoom1Mat, false);
        MakeBox("Wall_Front", room1, V(0, 3, -10), V(20, 6, 0.5f), wallRoom1Mat, false);
        // Right wall split for hallway opening (gap from Z=-2 to Z=2)
        MakeBox("Wall_Right_A", room1, V(10, 3, 6), V(0.5f, 6, 8), wallRoom1Mat, false);
        MakeBox("Wall_Right_B", room1, V(10, 3, -6), V(0.5f, 6, 8), wallRoom1Mat, false);
        MakeBox("Ceiling", room1, V(0, 6, 0), V(20, 0.5f, 20), wallRoom1Mat, false);

        // Room 1 furniture
        MakeBox("Platform_Raised", room1, V(-4, 1, 4), V(4, 0.5f, 4), platformMat, true);
        MakeBox("Step_Small", room1, V(-6, 0.5f, -3), V(3, 0.5f, 3), platformMat, true);
        MakeCylinder("Pillar", room1, V(4, 2, -4), V(1.5f, 2, 1.5f), wallRoom1Mat);
        MakeBox("Bench", room1, V(6, 0.6f, 7), V(4, 0.3f, 1.5f), accentMat, false);

        // ── Hallway ─────────────────────────────────────────────────
        GameObject hallway = new GameObject("Hallway");

        MakeBox("Floor", hallway, V(15, 0, 0), V(10, 0.5f, 4), hallwayMat, true);
        MakeBox("Wall_North", hallway, V(15, 3, 2), V(10, 6, 0.5f), hallwayMat, false);
        MakeBox("Wall_South", hallway, V(15, 3, -2), V(10, 6, 0.5f), hallwayMat, false);
        MakeBox("Ceiling", hallway, V(15, 6, 0), V(10, 0.5f, 4), hallwayMat, false);

        // ── Room 2 ──────────────────────────────────────────────────
        GameObject room2 = new GameObject("Room2");

        MakeBox("Floor", room2, V(30, 0, 0), V(20, 0.5f, 20), floorRoom2Mat, true);
        MakeBox("Wall_Back", room2, V(30, 3, 10), V(20, 6, 0.5f), wallRoom2Mat, false);
        MakeBox("Wall_Right", room2, V(40, 3, 0), V(0.5f, 6, 20), wallRoom2Mat, false);
        MakeBox("Wall_Front", room2, V(30, 3, -10), V(20, 6, 0.5f), wallRoom2Mat, false);
        // Left wall split for hallway opening (gap from Z=-2 to Z=2)
        MakeBox("Wall_Left_A", room2, V(20, 3, 6), V(0.5f, 6, 8), wallRoom2Mat, false);
        MakeBox("Wall_Left_B", room2, V(20, 3, -6), V(0.5f, 6, 8), wallRoom2Mat, false);
        MakeBox("Ceiling", room2, V(30, 6, 0), V(20, 0.5f, 20), wallRoom2Mat, false);

        // Room 2 furniture
        MakeSphere("Decoration_Sphere", room2, V(33, 2, 5), V(2, 2, 2), accentMat);
        MakeBox("Platform_Low", room2, V(25, 0.75f, -4), V(5, 1, 5), platformMat, true);
        MakeBox("Platform_High", room2, V(25, 2, -4), V(3, 0.5f, 3), floorRoom2Mat, true);
        MakeBox("Table", room2, V(35, 1, -5), V(3, 0.3f, 2), hallwayMat, false);
        MakeCylinder("Column_A", room2, V(27, 2, 7), V(1, 2, 1), wallRoom2Mat);
        MakeCylinder("Column_B", room2, V(33, 2, 7), V(1, 2, 1), wallRoom2Mat);

        // ── Player ──────────────────────────────────────────────────
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = V(0, 1.5f, 0);
        player.GetComponent<Renderer>().sharedMaterial = playerMat;

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        PlayerMovement pm = player.AddComponent<PlayerMovement>();
        pm.groundLayer = LayerMask.GetMask("Ground");

        // ── Camera ──────────────────────────────────────────────────
        ThirdPersonCamera tpc = Camera.main.gameObject.AddComponent<ThirdPersonCamera>();
        tpc.target = player.transform;

        // ── 5 Moving Objects ────────────────────────────────────────
        GameObject movingParent = new GameObject("MovingObjects");

        // 1 - Sliding cube in Room 1 (translates on X)
        GameObject mo1Obj = MakeBox("SlidingCube", movingParent, V(-3, 1, 0), V(1, 1, 1), movingMat, false);
        MovingObject mo1 = mo1Obj.AddComponent<MovingObject>();
        mo1.movementType = MovingObject.MovementType.Translate;
        mo1.moveAxis = Vector3.right;
        mo1.moveSpeed = 2f;
        mo1.maxDistance = 5f;

        // 2 - Spinning cylinder in Room 1 (rotates)
        GameObject mo2Obj = MakeCylinder("SpinningCylinder", movingParent, V(5, 3.5f, 5), V(0.5f, 1.5f, 0.5f), movingMat);
        MovingObject mo2 = mo2Obj.AddComponent<MovingObject>();
        mo2.movementType = MovingObject.MovementType.Rotate;
        mo2.rotateAxis = Vector3.up;
        mo2.rotateSpeed = 120f;

        // 3 - Bouncing sphere in Room 2 (translates on Y)
        GameObject mo3Obj = MakeSphere("BouncingSphere", movingParent, V(33, 2, 0), V(1, 1, 1), movingMat);
        MovingObject mo3 = mo3Obj.AddComponent<MovingObject>();
        mo3.movementType = MovingObject.MovementType.Translate;
        mo3.moveAxis = Vector3.up;
        mo3.moveSpeed = 2f;
        mo3.maxDistance = 3f;

        // 4 - Rotating platform in Room 2 (rotates)
        GameObject mo4Obj = MakeBox("RotatingPlatform", movingParent, V(30, 1, 5), V(4, 0.3f, 4), platformMat, false);
        MovingObject mo4 = mo4Obj.AddComponent<MovingObject>();
        mo4.movementType = MovingObject.MovementType.Rotate;
        mo4.rotateAxis = Vector3.up;
        mo4.rotateSpeed = 45f;

        // 5 - Hallway slider (translates on X + rotates)
        GameObject mo5Obj = MakeBox("HallwaySlider", movingParent, V(15, 1.5f, 0), V(1, 1, 1), movingMat, false);
        MovingObject mo5 = mo5Obj.AddComponent<MovingObject>();
        mo5.movementType = MovingObject.MovementType.Both;
        mo5.moveAxis = Vector3.right;
        mo5.moveSpeed = 3f;
        mo5.maxDistance = 4f;
        mo5.rotateAxis = new Vector3(1, 1, 0);
        mo5.rotateSpeed = 90f;

        // ── Lighting ────────────────────────────────────────────────
        RenderSettings.ambientLight = new Color(0.3f, 0.3f, 0.35f);

        GameObject light1 = new GameObject("Room1_Light");
        light1.transform.position = V(0, 5.5f, 0);
        Light l1 = light1.AddComponent<Light>();
        l1.type = LightType.Point;
        l1.range = 30f;
        l1.intensity = 1.5f;
        l1.color = new Color(1f, 0.95f, 0.85f);

        GameObject light2 = new GameObject("Room2_Light");
        light2.transform.position = V(30, 5.5f, 0);
        Light l2 = light2.AddComponent<Light>();
        l2.type = LightType.Point;
        l2.range = 30f;
        l2.intensity = 1.5f;
        l2.color = new Color(0.85f, 0.85f, 1f);

        // ── Save ────────────────────────────────────────────────────
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Lab1.unity");
        AssetDatabase.SaveAssets();

        Debug.Log("Lab 1 scene built successfully! Press Play to test.");
        EditorUtility.DisplayDialog("Scene Built!",
            "Lab 1 scene created successfully!\n\n" +
            "Press Play to test.\n\n" +
            "Controls:\n" +
            "  WASD - Move\n" +
            "  Mouse - Look around\n" +
            "  Spacebar - Jump", "OK");
    }

    // ── Helpers ──────────────────────────────────────────────────────

    static Vector3 V(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    static void SetupGroundLayer()
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");

        if (layers.GetArrayElementAtIndex(6).stringValue != "Ground")
        {
            layers.GetArrayElementAtIndex(6).stringValue = "Ground";
            tagManager.ApplyModifiedProperties();
        }
    }

    static Material MakeMaterial(string name, Color color)
    {
        string path = "Assets/Materials/" + name + ".mat";
        Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null)
            return existing;

        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static GameObject MakeBox(string name, GameObject parent, Vector3 pos, Vector3 scale, Material mat, bool isGround)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.parent = parent.transform;
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = mat;
        if (isGround)
            obj.layer = 6;
        return obj;
    }

    static GameObject MakeCylinder(string name, GameObject parent, Vector3 pos, Vector3 scale, Material mat)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.name = name;
        obj.transform.parent = parent.transform;
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = mat;
        return obj;
    }

    static GameObject MakeSphere(string name, GameObject parent, Vector3 pos, Vector3 scale, Material mat)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = name;
        obj.transform.parent = parent.transform;
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = mat;
        return obj;
    }
}
