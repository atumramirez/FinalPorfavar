using UnityEngine;

public class GyroLandscapeRotation : MonoBehaviour
{
    private Gyroscope gyro;
    private bool gyroSupported;

    private Quaternion rotationFix;

    void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;

        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            // Landscape: remap device rotation to Unity coordinates
            rotationFix = new Quaternion(0, 0, 1, 0); // flip Z and W
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device");
        }
    }

    void Update()
    {
        if (gyroSupported)
        {
            Quaternion deviceRotation = gyro.attitude;

            // Rotate 90 degrees to align landscape left (home button on the left)
            Quaternion landscapeRotation = new Quaternion(
                deviceRotation.y,
                -deviceRotation.x,
                deviceRotation.z,
                -deviceRotation.w
            );

            transform.localRotation = rotationFix * landscapeRotation;
        }
    }
}
