using UnityEngine;

public class SurfaceDetection : MonoBehaviour
{
    public float rayDistance = 2f;
    public LayerMask surfaceLayers;
    
    private RaycastHit surfaceHit;
    
    public string GetCurrentSurface()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out surfaceHit, rayDistance, surfaceLayers))
        {
            return surfaceHit.collider.tag;
        }
        return "Default";
    }
    
    public float GetSurfaceFriction()
    {
        string surface = GetCurrentSurface();
        switch (surface)
        {
            case "Ice":
                return 0.1f;
            case "Metal":
                return 0.8f;
            case "Grass":
                return 0.6f;
            default:
                return 1f;
        }
    }
}
