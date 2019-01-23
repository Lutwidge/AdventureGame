using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Camera script : the camera is voluntarily separated from the protagonist in the hierarchy, to be able to be used for effects
public class FollowCamera : MonoBehaviour
{
    [Header("Target to follow")]
    [SerializeField] private Transform targetTransform;
    private Vector3 offset; // Camera offset from target

    [Header("Camera movement settings")]
    [SerializeField] private float rotationSpeed = 5.0f;
    // Hidden because of the custom editor defined below
    [SerializeField, HideInInspector] private bool isSmooth = true;
    [SerializeField, HideInInspector] private float smoothSpeed = 1.0f;
    [SerializeField, HideInInspector] private bool isXLocked = true;
    [SerializeField, HideInInspector] private float minXLockValue = -80.0f;
    [SerializeField, HideInInspector] private float maxXLockValue = 80.0f;

    // Input fields
    private float yAxisRotation = 0.0f;
    private float xAxisRotation = 0.0f;
    private Quaternion newRotation;

    void Start()
    {
        offset = targetTransform.position - transform.position;
    }

    void Update()
    {
        UpdateRotationFromInputs();

        //float desiredAngle = targetTransform.eulerAngles.y;
        //Quaternion rotation;
        //if (isSmooth)
        //{
        //    rotation = Quaternion.Slerp(Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f), Quaternion.Euler(0.0f, desiredAngle, 0.0f), smoothSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    rotation = Quaternion.Euler(0.0f, desiredAngle, 0.0f);
        //}

        // Rotate the offset in the same direction as the target then substract the result from the target position
        transform.position = targetTransform.position - (newRotation * offset);

        transform.LookAt(targetTransform);
    }

    private void UpdateRotationFromInputs()
    {
        // Get inputs and adjust the resulting rotation angles
        yAxisRotation += Input.GetAxis("Mouse X") * rotationSpeed;
        xAxisRotation -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Define camera rotation
        if (isSmooth)
        {
            xAxisRotation = Mathf.Lerp(transform.eulerAngles.x, xAxisRotation, smoothSpeed * Time.deltaTime);
            yAxisRotation = Mathf.Lerp(transform.eulerAngles.y, yAxisRotation, smoothSpeed * Time.deltaTime);
        }
        Debug.Log(xAxisRotation.ToString());
        Debug.Log(yAxisRotation.ToString());
        if (isXLocked)
        {
            xAxisRotation = Mathf.Clamp(xAxisRotation, minXLockValue, maxXLockValue);
        }
        newRotation = Quaternion.Euler(xAxisRotation, yAxisRotation, 0.0f);
    }

#if UNITY_EDITOR
    // Custom editor for the camera, so that the smooth option only appear if "isSmooth" is enabled
    // Used here so that it has access to the private fields & deactivated in builds so that the build is done correctly
    [CustomEditor(typeof(FollowCamera))]
    public class FollowCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // To have the serialized fields

            FollowCamera followCameraScript = target as FollowCamera;

            followCameraScript.isSmooth = GUILayout.Toggle(followCameraScript.isSmooth, "Smooth camera");

            if (followCameraScript.isSmooth)
            {
                followCameraScript.smoothSpeed = EditorGUILayout.FloatField("Smooth speed", followCameraScript.smoothSpeed);
            }

            followCameraScript.isXLocked = GUILayout.Toggle(followCameraScript.isXLocked, "Locked in X");

            if (followCameraScript.isXLocked)
            {
                followCameraScript.minXLockValue = EditorGUILayout.FloatField("Min lock value", followCameraScript.minXLockValue);
                followCameraScript.maxXLockValue = EditorGUILayout.FloatField("Max lock value", followCameraScript.maxXLockValue);
            }
        }
    }
#endif
}
