using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

// Independent scene generator for an oval recreation park with a pond and physics court.
public static class CrescentParkFactory
{
    const string SceneFile = "Assets/Scenes/ParkPhysicsEnvironment.unity";
    static Transform landscape, furniture, plants, court;
    static Material lawn, paving, timber, metal, trunk, foliage, pink, water, cream;

    [MenuItem("Crescent Park/Generate New Park")]
    public static void Generate()
    {
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Surfaces");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        PlayerSettings.productName = "Crescent Park - Physics Practical";
        PlayerSettings.companyName = "Park Studies";
        PlayerSettings.defaultScreenWidth = 1440;
        PlayerSettings.defaultScreenHeight = 900;
        QualitySettings.shadows = ShadowQuality.All;
        QualitySettings.shadowResolution = ShadowResolution.High;
        QualitySettings.shadowDistance = 90;
        QualitySettings.antiAliasing = 4;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.53f, 0.60f, 0.66f);
        Physics.gravity = new Vector3(0, -9.81f, 0);
        lawn = Surface("Sage meadow", "537965");
        paving = Surface("Terracotta paving", "BC8065");
        timber = Surface("Light oak", "D9B989");
        metal = Surface("Ink metal", "293C46");
        trunk = Surface("Cedar bark", "674A3B");
        foliage = Surface("Evergreen", "284E48");
        pink = Surface("Coral blossoms", "DD817B");
        water = Surface("Still blue water", "4F9EA7");
        cream = Surface("Limestone", "E2D9BD");
        landscape = Group("Landscape - Lawn, Oval Plaza and Pond");
        furniture = Group("Picnic Garden - Cube Furniture");
        plants = Group("Botanical Border - Cedars and Flowers");
        court = Group("Physics Court - Dynamic Spheres");

        var ground = Shape(PrimitiveType.Plane, "Park Ground Plane", landscape,
            Vector3.zero, new Vector3(2.4f, 1, 1.8f), lawn);
        ground.GetComponent<Collider>().sharedMaterial = ContactSurface("Meadow friction", 0, 0.55f);
        Shape(PrimitiveType.Cube, "Floating Landscape Foundation", landscape,
            new Vector3(0, -0.35f, 0), new Vector3(24, 0.65f, 18), cream);
        // A cylinder rotated/scaled into a broad oval plaza, with a smaller inset.
        Shape(PrimitiveType.Cylinder, "Oval Promenade", landscape,
            new Vector3(-2.5f, 0.04f, 0.2f), new Vector3(12, 0.04f, 10), paving);
        Shape(PrimitiveType.Cylinder, "Plaza Limestone Inset", landscape,
            new Vector3(-2.5f, 0.095f, 0.2f), new Vector3(9.8f, 0.015f, 7.8f), cream);
        var entrance = Shape(PrimitiveType.Cube, "Diagonal Entrance Walk", landscape,
            new Vector3(-5.4f, 0.04f, -5.3f), new Vector3(2.4f, 0.08f, 8), paving);
        entrance.transform.localRotation = Quaternion.Euler(0, -24, 0);

        Shape(PrimitiveType.Cylinder, "Pond Stone Rim", landscape,
            new Vector3(-6.5f, 0.15f, 4.8f), new Vector3(6.5f, 0.15f, 4.2f), cream);
        Shape(PrimitiveType.Cylinder, "Decorative Pond Water", landscape,
            new Vector3(-6.5f, 0.31f, 4.8f), new Vector3(5.8f, 0.015f, 3.5f), water);
        for (int i = 0; i < 4; i++)
            Shape(PrimitiveType.Cylinder, "Pond Stepping Stone " + (i + 1), landscape,
                new Vector3(-8.1f + i * 1.05f, 0.36f, 4.8f), new Vector3(0.65f, 0.09f, 0.65f), paving);

        PicnicTable(new Vector3(-2.5f, 0.13f, 0), 22);
        PicnicTable(new Vector3(-7.5f, 0, -2), -18);
        for (int i = 0; i < 5; i++)
        {
            Vector3 pos = new Vector3(-10 + i * 4.5f, 0, 7.2f);
            Cedar(pos, 2.6f + (i % 2) * 0.7f, i + 1);
        }
        Cedar(new Vector3(-10, 0, -6), 2.8f, 6);
        Cedar(new Vector3(10, 0, -6), 3.2f, 7);
        FlowerBed(new Vector3(-1, 0, -6.4f), 0);
        FlowerBed(new Vector3(3.5f, 0, 5.7f), 1);
        for (int i = 0; i < 3; i++)
        {
            var post = Shape(PrimitiveType.Cylinder, "Garden Lamp Post " + i, furniture,
                new Vector3(-9.5f + i * 4, 1.2f, -4.7f), new Vector3(0.12f, 1.2f, 0.12f), metal);
            Shape(PrimitiveType.Cube, "Lamp Lantern " + i, furniture,
                post.transform.position + Vector3.up * 1.35f, new Vector3(0.45f, 0.6f, 0.45f), cream);
        }

        var courtSurface = Surface("Physics court teal", "326B75");
        var platform = Shape(PrimitiveType.Cube, "Bounce Court - Box Collider", court,
            new Vector3(6, 0.08f, -0.5f), new Vector3(6.5f, 0.16f, 7), courtSurface);
        platform.GetComponent<Collider>().sharedMaterial = ContactSurface("Court contact", 0, 0.4f);
        for (int i = 0; i < 4; i++)
        {
            bool horizontal = i < 2;
            Shape(PrimitiveType.Cube, "Court Boundary Stripe " + (i + 1), court,
                new Vector3(horizontal ? 6 : (i == 2 ? 2.95f : 9.05f), 0.17f, horizontal ? (i == 0 ? -3.8f : 2.8f) : -0.5f),
                horizontal ? new Vector3(6.1f, 0.015f, 0.07f) : new Vector3(0.07f, 0.015f, 6.6f), cream);
        }
        var red = MakeExperiment("Coral Sphere - Rubber", new Vector3(4.7f, 5.8f, -0.6f),
            Surface("Coral rubber", "EC7460"), 0.75f, 0.92f);
        var gold = MakeExperiment("Gold Sphere - Soft Ball", new Vector3(7.5f, 4.3f, -0.6f),
            Surface("Golden soft ball", "EBC061"), 2.5f, 0.35f);
        red.displayName = "CORAL  /  0.75 kg  /  bounce 0.92";
        gold.displayName = "GOLD  /  2.5 kg  /  bounce 0.35";
        var panel = new GameObject("Experiment Toolbar").AddComponent<ExperimentPanel>();
        panel.experiments = new[] { red, gold };

        var camera = new GameObject("Overview Camera", typeof(Camera), typeof(AudioListener)).GetComponent<Camera>();
        camera.tag = "MainCamera";
        camera.transform.position = new Vector3(22, 23, -27);
        camera.transform.LookAt(new Vector3(0, 0.8f, 0));
        camera.orthographic = true;
        camera.orthographicSize = 15.5f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.20f, 0.29f, 0.34f);
        camera.farClipPlane = 150;
        var sun = new GameObject("Warm Evening Light").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.color = new Color(1, 0.87f, 0.7f);
        sun.intensity = 1.1f;
        sun.shadows = LightShadows.Soft;
        sun.transform.rotation = Quaternion.Euler(42, -50, 0);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), SceneFile);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(SceneFile, true) };
        AssetDatabase.SaveAssets();
        CheckPhysics();
        Debug.Log("CRESCENT_PARK_COMPLETE");
    }

    static Transform Group(string name) => new GameObject(name).transform;

    static Material Surface(string name, string hex)
    {
        string path = "Assets/Surfaces/" + name + ".mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(Shader.Find("Standard"));
            AssetDatabase.CreateAsset(mat, path);
        }
        ColorUtility.TryParseHtmlString("#" + hex, out Color color);
        mat.color = color;
        mat.SetFloat("_Glossiness", 0.12f);
        return mat;
    }

    static PhysicsMaterial ContactSurface(string name, float bounce, float friction)
    {
        string path = "Assets/Surfaces/" + name + ".physicMaterial";
        var mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial>(path);
        if (mat == null)
        {
            mat = new PhysicsMaterial(name);
            AssetDatabase.CreateAsset(mat, path);
        }
        mat.bounciness = bounce;
        mat.dynamicFriction = friction;
        mat.staticFriction = friction;
        mat.bounceCombine = PhysicsMaterialCombine.Maximum;
        return mat;
    }

    static GameObject Shape(PrimitiveType kind, string name, Transform parent, Vector3 at, Vector3 size, Material surface)
    {
        var obj = GameObject.CreatePrimitive(kind);
        obj.name = name;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = at;
        obj.transform.localScale = size;
        obj.GetComponent<Renderer>().sharedMaterial = surface;
        if (kind == PrimitiveType.Cylinder && size.y < 0.2f)
        {
            // Static flat disks need their actual mesh shape, not a compressed capsule.
            UnityEngine.Object.DestroyImmediate(obj.GetComponent<Collider>());
            obj.AddComponent<MeshCollider>().sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
        }
        return obj;
    }

    static void PicnicTable(Vector3 at, float angle)
    {
        var table = new GameObject("Picnic Table " + furniture.childCount).transform;
        table.SetParent(furniture);
        table.position = at;
        table.rotation = Quaternion.Euler(0, angle, 0);
        Shape(PrimitiveType.Cube, "Cube Tabletop", table, new Vector3(0, 1.1f, 0), new Vector3(2.7f, 0.16f, 1.25f), timber);
        for (int side = -1; side <= 1; side += 2)
        {
            Shape(PrimitiveType.Cube, "Cube Bench " + side, table, new Vector3(0, 0.6f, side), new Vector3(2.7f, 0.14f, 0.45f), timber);
            var leg = Shape(PrimitiveType.Cube, "Angled Cube Support " + side, table,
                new Vector3(side * 0.9f, 0.53f, 0), new Vector3(0.18f, 1.05f, 2.3f), metal);
            leg.transform.localRotation = Quaternion.Euler(0, 0, side * 12);
        }
    }

    static void Cedar(Vector3 at, float height, int number)
    {
        var tree = new GameObject("Cedar " + number).transform;
        tree.SetParent(plants, false);
        tree.localPosition = at;
        Shape(PrimitiveType.Cylinder, "Cylinder Trunk", tree, new Vector3(0, height * 0.25f, 0),
            new Vector3(0.35f, height * 0.25f, 0.35f), trunk);
        for (int tier = 0; tier < 3; tier++)
        {
            // Custom cone meshes distinguish these cedar silhouettes from sphere-canopy trees.
            float radius = 1.2f - tier * 0.25f;
            var cone = new GameObject("Cone Foliage Tier " + tier, typeof(MeshFilter), typeof(MeshRenderer));
            cone.transform.SetParent(tree, false);
            cone.transform.localPosition = new Vector3(0, height * 0.25f + tier * 0.55f, 0);
            string meshPath = "Assets/Surfaces/CedarMesh-" + number + "-" + tier + ".asset";
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            if (mesh == null)
            {
                mesh = ConeMesh(radius, height * 0.55f);
                AssetDatabase.CreateAsset(mesh, meshPath);
            }
            cone.GetComponent<MeshFilter>().sharedMesh = mesh;
            cone.GetComponent<MeshRenderer>().sharedMaterial = foliage;
        }
    }

    static Mesh ConeMesh(float radius, float height)
    {
        const int sides = 12;
        var vertices = new Vector3[sides + 2];
        vertices[0] = Vector3.up * height;
        for (int i = 0; i < sides; i++)
        {
            float angle = i * Mathf.PI * 2 / sides;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }
        vertices[sides + 1] = Vector3.zero;
        var triangles = new int[sides * 6];
        for (int i = 0; i < sides; i++)
        {
            int next = (i + 1) % sides + 1;
            int index = i * 6;
            triangles[index] = 0; triangles[index + 1] = next; triangles[index + 2] = i + 1;
            triangles[index + 3] = sides + 1; triangles[index + 4] = i + 1; triangles[index + 5] = next;
        }
        var mesh = new Mesh { name = "Faceted cedar cone", vertices = vertices, triangles = triangles };
        mesh.RecalculateNormals();
        return mesh;
    }

    static void FlowerBed(Vector3 at, int index)
    {
        Shape(PrimitiveType.Cube, "Raised Flower Bed " + index, plants, at + Vector3.up * 0.2f, new Vector3(3, 0.4f, 1.3f), paving);
        for (int i = 0; i < 6; i++)
            Shape(PrimitiveType.Sphere, "Blossom " + index + "." + i, plants,
                at + new Vector3(-1.15f + i * 0.46f, 0.54f, i % 2 == 0 ? -0.22f : 0.22f), Vector3.one * 0.52f, pink);
    }

    static DropExperiment MakeExperiment(string name, Vector3 position, Material color, float mass, float elasticity)
    {
        var ball = Shape(PrimitiveType.Sphere, name, court, position, Vector3.one * 1.15f, color);
        ball.GetComponent<SphereCollider>().sharedMaterial = ContactSurface(name + " Bounce", elasticity, 0.12f);
        var body = ball.AddComponent<Rigidbody>();
        body.mass = mass;
        body.useGravity = true;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        return ball.AddComponent<DropExperiment>();
    }

    [MenuItem("Crescent Park/Check Physics")]
    public static void CheckPhysics()
    {
        EditorSceneManager.OpenScene(SceneFile);
        var balls = UnityEngine.Object.FindObjectsByType<DropExperiment>(FindObjectsSortMode.None);
        if (balls.Length != 2) throw new Exception("Expected two drop experiments.");
        var bodies = balls.Select(x => x.GetComponent<Rigidbody>()).ToArray();
        if (bodies.Any(b => !b.useGravity || b.isKinematic || b.GetComponent<SphereCollider>().sharedMaterial == null))
            throw new Exception("Invalid ball physics components.");
        if (GameObject.Find("Park Ground Plane").GetComponent<MeshCollider>() == null)
            throw new Exception("Ground Mesh Collider missing.");
        var bounced = new bool[2];
        var mode = Physics.simulationMode;
        try
        {
            Physics.simulationMode = SimulationMode.Script;
            Physics.SyncTransforms();
            for (int step = 0; step < 600; step++)
            {
                Physics.Simulate(0.02f);
                for (int i = 0; i < 2; i++)
                {
                    if (bodies[i].position.y < 0.5f) throw new Exception("Ball fell through a collision surface: " + bodies[i].name + " position " + bodies[i].position + " at step " + step);
                    if (bodies[i].linearVelocity.y > 1) bounced[i] = true;
                }
            }
            if (bounced.Any(b => !b)) throw new Exception("Bounce check failed.");
            Directory.CreateDirectory("Evidence");
            File.WriteAllText("Evidence/PhysicsValidation.txt", "PASS - Both Rigidbody spheres have gravity, Sphere Colliders and Physics Materials.\nPASS - Ground Plane has a Mesh Collider.\nPASS - Both balls rebounded under physics.\nPASS - Neither ball fell through the court during 12 simulated seconds.\nCoral: mass 0.75 kg; bounciness 0.92; Maximum combine.\nGold: mass 2.5 kg; bounciness 0.35; Maximum combine.\n");
            Debug.Log("CRESCENT_PHYSICS_PASS");
        }
        finally
        {
            Physics.simulationMode = mode;
            EditorSceneManager.OpenScene(SceneFile);
        }
    }

    public static void Photograph()
    {
        EditorSceneManager.OpenScene(SceneFile);
        var camera = Camera.main;
        var texture = new RenderTexture(1440, 900, 24);
        camera.targetTexture = texture;
        camera.Render();
        RenderTexture.active = texture;
        var screenshot = new Texture2D(1440, 900, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, 1440, 900), 0, 0);
        screenshot.Apply();
        Directory.CreateDirectory("Evidence");
        File.WriteAllBytes("Evidence/CrescentPark.png", screenshot.EncodeToPNG());
        camera.targetTexture = null;
        RenderTexture.active = null;
        UnityEngine.Object.DestroyImmediate(screenshot);
        UnityEngine.Object.DestroyImmediate(texture);
        Debug.Log("CRESCENT_SCREENSHOT_SAVED");
    }

    public static void FinishDelivery()
    {
        Generate();
        Photograph();
    }
}
