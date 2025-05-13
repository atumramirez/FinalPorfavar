using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicVolumeController : MonoBehaviour
{
    public Transform targetObject; // The object that reacts to loudness
    public float sensitivity = 100f; // Controls how sensitive the reaction is
    public float maxHeight = 5f; // Max height or scale based on volume

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 1, 44100); // Default mic
        audioSource.loop = true;

        // Wait until microphone starts recording
        while (!(Microphone.GetPosition(null) > 0)) { }

        audioSource.Play();
    }

    void Update()
    {
        float volume = GetLoudnessFromMic() * sensitivity;

        // Clamp and apply to Y scale or position
        float clampedValue = Mathf.Clamp(volume, 0, maxHeight);

        // Option 1: Move the object upward
        targetObject.localPosition = new Vector3(
            targetObject.localPosition.x,
            clampedValue,
            targetObject.localPosition.z
        );

        // Option 2: Scale the object (comment above and uncomment below if preferred)
        // targetObject.localScale = new Vector3(1, 1 + clampedValue, 1);
    }

    float GetLoudnessFromMic()
    {
        float[] data = new float[256];
        audioSource.GetOutputData(data, 0);
        float total = 0f;

        foreach (float s in data)
        {
            total += Mathf.Abs(s);
        }

        return total / data.Length;
    }
}