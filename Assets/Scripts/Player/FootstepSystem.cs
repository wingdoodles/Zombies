using UnityEngine;

[System.Serializable]
public class SurfaceFootsteps
{
    public string surfaceTag;
    public AudioClip[] footstepSounds;
}

public class FootstepSystem : MonoBehaviour
{
    [Header("Surface Footsteps")]
    public SurfaceFootsteps[] surfaceFootsteps;

    [Header("Footstep Settings")]
    public float stepRate = 0.5f;
    public float runStepRate = 0.3f;
    
    private AudioSource audioSource;
    private PlayerStateManager stateManager;
    private CharacterController characterController;
    private float stepCooldown;
    private SurfaceDetection surfaceDetection;
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        stateManager = GetComponent<PlayerStateManager>();
        characterController = GetComponent<CharacterController>();
        surfaceDetection = GetComponent<SurfaceDetection>();
    }
    
    void Update()
    {
        if (!characterController.isGrounded) return;
        
        if (characterController.velocity.magnitude > 0.1f)
        {
            stepCooldown -= Time.deltaTime;
            
            if (stepCooldown <= 0)
            {
                PlayFootstep();
                stepCooldown = stateManager.currentState == PlayerState.Running ? runStepRate : stepRate;
            }
        }
    }
    
    void PlayFootstep()
    {
        string currentSurface = surfaceDetection.GetCurrentSurface();
        AudioClip[] surfaceSounds = GetSurfaceSounds(currentSurface);
        
        if (surfaceSounds != null && surfaceSounds.Length > 0)
        {
            AudioClip randomStep = surfaceSounds[Random.Range(0, surfaceSounds.Length)];
            audioSource.PlayOneShot(randomStep);
        }
    }
    
    private AudioClip[] GetSurfaceSounds(string surfaceTag)
    {
        foreach (var surface in surfaceFootsteps)
        {
            if (surface.surfaceTag == surfaceTag)
                return surface.footstepSounds;
        }
        return surfaceFootsteps[0].footstepSounds; // Default sounds
    }
}
