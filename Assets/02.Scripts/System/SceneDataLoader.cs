using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using static Meta.XR.MRUtilityKit.MRUK;

public class SceneDataLoader : MonoBehaviour
{
    // Scene API data
    private static readonly string[] MeshClassifications =
        { "GlobalMesh", OVRSceneManager.Classification.GlobalMesh };

    public static SceneDataLoader Instance;

    [SerializeField] private OVRSceneManager ovrSceneManagerPrefab;
    [SerializeField] private Transform sceneRoot;
    public UnityEvent<Transform> SceneDataLoaded = new();
    public UnityEvent NoSceneModelAvailable = new();
    public UnityEvent NewSceneModelAvailable = new();
    private OVRSceneManager ovrSceneManager;

    private void Awake()
    {
        Instance = this;
        Assert.IsNotNull(ovrSceneManagerPrefab, $"{nameof(ovrSceneManagerPrefab)} cannot be null.");
        Assert.IsNotNull(sceneRoot, $"{nameof(sceneRoot)} cannot be null.");
    }

    void Start()
    {
        LoadMeshes();
    }

    private void OnValidate()
    {
        if (sceneRoot == null) sceneRoot = transform;
    }

    public void LoadMeshes()
    {
        Debug.Log($"{Application.productName}: Loading scene.");
        // Scene API data.
        StartCoroutine(LoadSceneAPIData());
    }

    private IEnumerator LoadSceneAPIData()
    {
        Debug.Log($"{Application.productName}: Loading scene model.");

        if (ovrSceneManager == null)
        {
            // Scene Manager from previous scene.
            var existingManager = FindObjectOfType<OVRSceneManager>();

            if (existingManager != null) DestroyImmediate(existingManager.gameObject);

            ovrSceneManager = Instantiate(ovrSceneManagerPrefab, transform);
        }

        // Set the initial room root.
        ovrSceneManager.InitialAnchorParent = sceneRoot;

        // Wait for the manager to fully load the scene so we can get its dimensions and create
        ovrSceneManager.SceneModelLoadedSuccessfully += () =>
        {
            Debug.Log($"{Application.productName}: {nameof(SceneDataLoader)}: SceneModelLoadedSuccessfully ");
            OnSceneAPIDataLoaded();
        };
        // Wait until the manager has completed one update to start the loading process.
        ovrSceneManager.SceneCaptureReturnedWithoutError += () =>
        {
            Debug.Log(
                $"{Application.productName}: {nameof(SceneDataLoader)}: SceneCaptureReturnedWithoutError ");
        };
        // Catch the various errors that can occur when the scene capture is started.
        ovrSceneManager.UnexpectedErrorWithSceneCapture += () =>
        {
            Debug.LogError(
                $"{Application.productName}: {nameof(SceneDataLoader)}: UnexpectedErrorWithSceneCapture ");
            NoSceneModelAvailable?.Invoke();
        };
        ovrSceneManager.NoSceneModelToLoad += () =>
        {
            Debug.LogError($"{Application.productName}: {nameof(SceneDataLoader)}: NoSceneModelToLoad ");
            NoSceneModelAvailable?.Invoke();
        };
        ovrSceneManager.NewSceneModelAvailable += () =>
        {
            Debug.Log($"{Application.productName}: {nameof(SceneDataLoader)}: NewSceneModelAvailable ");
            if (ovrSceneManager.LoadSceneModel())
            {
                NewSceneModelAvailable?.Invoke();
            }
        };
        yield return null;
    }

    public void Rescan()
    {
        ovrSceneManager.RequestSceneCapture();
    }




    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (ovrSceneManager != null) ovrSceneManager.SceneModelLoadedSuccessfully -= OnSceneAPIDataLoaded;
    }

    private void OnSceneAPIDataLoaded()
    {
        SceneDataLoaded?.Invoke(sceneRoot);
    }

    private static MeshFilter FindGlobalMeshFilter(Transform root)
    {
        var meshFilters = root.GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 1) return meshFilters[0];

        foreach (var mf in meshFilters)
        {
            var tf = mf.transform;
            while (tf != root)
            {
                if (tf.name.Contains("GlobalMesh", StringComparison.InvariantCultureIgnoreCase)) return mf;
                tf = tf.parent;
            }
        }

        return null;
    }
}

