using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneBuilder : Editor
{
    [MenuItem("Lab1/Fix Input Settings (Run This First)")]
    static void FixInputSettings()
    {
        SerializedObject projectSettings = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        SerializedProperty inputHandler = projectSettings.FindProperty("activeInputHandler");
        if (inputHandler != null && inputHandler.intValue != 2)
        {
            inputHandler.intValue = 2;
            projectSettings.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("Input Settings Fixed",
                "Active Input Handling changed to 'Both'.\n\n" +
                "Unity needs to restart for this to take effect.\n" +
                "Please close and reopen the project now.",
                "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Input Settings",
                "Input settings are already correct.", "OK");
        }
    }

    [MenuItem("Lab1/Build Entire Scene")]
    static void Build()
    {
        if (!EditorUtility.DisplayDialog("Build Lab 1 Scene",
            "This will create a new scene with two rooms, a hallway, a player, camera, and 5 moving objects.\n\nContinue?",
            "Build It", "Cancel"))
            return;

        FixInputSettingsQuiet();
        SetupGroundLayer();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        Material floorRoom1Mat = MakeTexturedMaterial("Floor_Room1", new Color(0.4f, 0.38f, 0.35f), "tile");
        Material floorRoom2Mat = MakeTexturedMaterial("Floor_Room2", new Color(0.15f, 0.15f, 0.35f), "tile");
        Material wallRoom1Mat = MakeTexturedMaterial("Wall_Room1", new Color(0.76f, 0.69f, 0.5f), "brick");
        Material wallRoom2Mat = MakeTexturedMaterial("Wall_Room2", new Color(0.55f, 0.14f, 0.14f), "brick");
        Material hallwayMat = MakeTexturedMaterial("Hallway", new Color(0.5f, 0.5f, 0.5f), "tile");
        Material platformMat = MakeTexturedMaterial("Platform", new Color(0.2f, 0.5f, 0.22f), "wood");
        Material movingMat = MakeTexturedMaterial("MovingObject", new Color(0.92f, 0.6f, 0.08f), "metal");
        Material playerMat = MakeTexturedMaterial("Player", new Color(0.2f, 0.4f, 0.85f), "stripe");
        Material accentMat = MakeTexturedMaterial("Accent", new Color(0.6f, 0.2f, 0.6f), "dots");

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
        MakeBox("Shelf", room1, V(-8, 2, 7), V(3, 0.2f, 1), hallwayMat, false);
        MakeBox("Crate_A", room1, V(-7, 0.5f, 6), V(1, 1, 1), movingMat, false);
        MakeBox("Crate_B", room1, V(-7, 1.5f, 6), V(0.8f, 0.8f, 0.8f), movingMat, false);
        MakeSphere("Lamp_Orb", room1, V(-8, 2.8f, 7), V(0.5f, 0.5f, 0.5f), accentMat);
        MakeBox("StairStep1", room1, V(7, 0.4f, -7), V(3, 0.4f, 1.5f), platformMat, true);
        MakeBox("StairStep2", room1, V(7, 0.8f, -8.5f), V(3, 0.4f, 1.5f), platformMat, true);
        MakeBox("StairStep3", room1, V(7, 1.2f, -10), V(3, 0.4f, 1.5f), platformMat, false);

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
        MakeBox("Ramp", room2, V(37, 0.5f, 4), V(4, 1, 2), platformMat, true);
        MakeBox("WallShelf_A", room2, V(39, 2.5f, -8), V(2, 0.2f, 1), hallwayMat, false);
        MakeBox("WallShelf_B", room2, V(39, 3.5f, -8), V(2, 0.2f, 1), hallwayMat, false);
        MakeSphere("Orb_Small", room2, V(39, 2.9f, -8), V(0.4f, 0.4f, 0.4f), movingMat);
        MakeSphere("Orb_Large", room2, V(39, 3.9f, -8), V(0.6f, 0.6f, 0.6f), accentMat);
        MakeBox("Pedestal", room2, V(30, 0.5f, 8), V(2, 1, 2), floorRoom2Mat, false);
        MakeCylinder("Vase", room2, V(30, 1.5f, 8), V(0.6f, 0.8f, 0.6f), accentMat);
        MakeBox("Barrier", room2, V(35, 0.5f, 0), V(0.5f, 1, 6), wallRoom2Mat, false);

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

    static void FixInputSettingsQuiet()
    {
        SerializedObject projectSettings = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        SerializedProperty inputHandler = projectSettings.FindProperty("activeInputHandler");
        if (inputHandler != null && inputHandler.intValue != 2)
        {
            inputHandler.intValue = 2;
            projectSettings.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }
    }

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
        return MakeTexturedMaterial(name, color, "checker");
    }

    static Material MakeTexturedMaterial(string name, Color color, string pattern)
    {
        string matPath = "Assets/Materials/" + name + ".mat";
        string texPath = "Assets/Materials/" + name + "_tex.png";

        Material existing = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (existing != null)
            AssetDatabase.DeleteAsset(matPath);

        Texture2D oldTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        if (oldTex != null)
            AssetDatabase.DeleteAsset(texPath);

        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");

        Texture2D tex = GenerateTexture(color, pattern, 128);
        byte[] pngData = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(texPath, pngData);
        AssetDatabase.ImportAsset(texPath);

        TextureImporter importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
        if (importer != null)
        {
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Bilinear;
            importer.SaveAndReimport();
        }

        Texture2D loadedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");

        Material mat = new Material(shader);
        mat.SetColor("_BaseColor", Color.white);
        mat.color = Color.white;

        Vector2 tiling = new Vector2(4f, 4f);

        if (shader.name.Contains("Universal"))
        {
            mat.SetTexture("_BaseMap", loadedTex);
            mat.SetTextureScale("_BaseMap", tiling);
        }
        else
        {
            mat.SetTexture("_MainTex", loadedTex);
            mat.SetTextureScale("_MainTex", tiling);
        }

        AssetDatabase.CreateAsset(mat, matPath);
        return mat;
    }

    static Texture2D GenerateTexture(Color baseColor, string pattern, int size)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color darkColor = baseColor * 0.45f;
        darkColor.a = 1f;
        Color lightColor = baseColor * 1.4f;
        lightColor.a = 1f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Color pixel = baseColor;

                switch (pattern)
                {
                    case "checker":
                        int cx = x / (size / 4);
                        int cy = y / (size / 4);
                        pixel = (cx + cy) % 2 == 0 ? baseColor : darkColor;
                        break;

                    case "brick":
                        int brickH = size / 4;
                        int brickW = size / 2;
                        int row = y / brickH;
                        int offsetX = (row % 2 == 0) ? 0 : brickW / 2;
                        int bx = (x + offsetX) % brickW;
                        int by = y % brickH;
                        bool isMortar = bx < 2 || by < 2;
                        pixel = isMortar ? darkColor * 0.8f : baseColor;
                        pixel.a = 1f;
                        break;

                    case "tile":
                        int tileSize = size / 4;
                        int tx = x % tileSize;
                        int ty = y % tileSize;
                        bool isGrout = tx < 2 || ty < 2;
                        pixel = isGrout ? darkColor * 0.6f : baseColor;
                        pixel.a = 1f;
                        break;

                    case "stripe":
                        int stripeW = size / 8;
                        pixel = ((x + y) / stripeW) % 2 == 0 ? baseColor : darkColor;
                        break;

                    case "wood":
                        float wave = Mathf.Sin(y * 0.3f + Mathf.Sin(x * 0.05f) * 4f) * 0.5f + 0.5f;
                        pixel = Color.Lerp(darkColor, lightColor, wave);
                        pixel.a = 1f;
                        break;

                    case "metal":
                        float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                        pixel = Color.Lerp(darkColor, lightColor, noise);
                        pixel.a = 1f;
                        break;

                    case "dots":
                        int dotSpacing = size / 4;
                        int dx = x % dotSpacing - dotSpacing / 2;
                        int dy = y % dotSpacing - dotSpacing / 2;
                        float dist = Mathf.Sqrt(dx * dx + dy * dy);
                        pixel = dist < dotSpacing * 0.3f ? lightColor : baseColor;
                        break;
                }

                tex.SetPixel(x, y, pixel);
            }
        }

        tex.Apply();
        return tex;
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
