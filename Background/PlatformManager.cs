using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlatformManager : MonoBehaviour
{
    public static readonly bool isMobile = Application.isMobilePlatform;

    PostProcessVolume volume;

    private void Awake()
    {
        volume = FindObjectOfType<PostProcessVolume>();
    }

    private void Start()
    {
        if (isMobile)
            Application.targetFrameRate = 60;

        volume.enabled = !isMobile;
    }
}