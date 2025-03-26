using UnityEngine;
using UnityEngine.Assertions;


public class CameraRig : MonoBehaviour
{
    [SerializeField] private OVRCameraRig cameraRig;

    public OVRCameraRig OVRCameraRig => cameraRig;
    public Transform CenterEyeAnchor => cameraRig.centerEyeAnchor;
    public Transform LeftHandAnchor => cameraRig.leftHandAnchor;
    public Transform RightHandAnchor => cameraRig.rightHandAnchor;
    public Transform LeftControllerAnchor => cameraRig.leftControllerAnchor;
    public Transform RightControllerAnchor => cameraRig.rightControllerAnchor;

    private static CameraRig _instance;

    public static CameraRig Instance
    {
        get
        {
            if (_instance == null && Application.isPlaying)
            {
                var existingInstances = FindObjectsOfType<CameraRig>(true);

                // We don't handle multiple singletons in the scene, make the user clean it up
                Assert.IsFalse(existingInstances.Length > 1,
                    $"There are {existingInstances.Length} instances of {typeof(OVRCameraRig)} in the scene. Only one instance may exist.");

                if (existingInstances.Length > 0) _instance = existingInstances[0];
            }

            return _instance;
        }
    }

    private void Awake()
    {
        FindDependencies();

        if (cameraRig == null)
        {
            Debug.LogError($"{nameof(CameraRig)}: could not find a reference to OVRCameraRig.");
            Destroy(this);
        }

        Debug.Log("OVRCamerarig " + Instance);
    }

    private void OnValidate()
    {
        FindDependencies();
    }

    private void FindDependencies()
    {
        if (cameraRig == null) cameraRig = GetComponent<OVRCameraRig>();
    }
}
