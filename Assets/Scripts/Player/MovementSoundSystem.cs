using UnityEngine;

public class MovementSoundSystem : MonoBehaviour
{
    [Header("Movement Sounds")]
    public AudioClip[] slideSounds;
    public AudioClip[] landingSounds;
    
    private AudioSource audioSource;
    private SurfaceDetection surfaceDetection;
    private PlayerStateManager stateManager;
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        surfaceDetection = GetComponent<SurfaceDetection>();
        stateManager = GetComponent<PlayerStateManager>();
    }
    
    public void PlaySlideSound()
    {
        if (slideSounds.Length > 0)
        {
            float surfaceFriction = surfaceDetection.GetSurfaceFriction();
            audioSource.volume = 1f - surfaceFriction;
            audioSource.PlayOneShot(slideSounds[Random.Range(0, slideSounds.Length)]);
        }
    }
    
    public void PlayLandingSound()
    {
        if (landingSounds.Length > 0)
        {
            audioSource.PlayOneShot(landingSounds[Random.Range(0, landingSounds.Length)]);
        }
    }
}
