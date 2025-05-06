using UnityEngine;

public class FartSound : MonoBehaviour
{
    private AudioSource fartNoise;

    void Start()
    {
        fartNoise = GetComponent<AudioSource>();
        if (fartNoise == null)
        {
            Debug.LogError("AudioSource component missing from FartSound GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("c") && fartNoise != null)
        {
            fartNoise.Play();
        }
    }
}

