using UnityEngine;
using System.Collections.Generic;

public class DebugManager : MonoBehaviour
{
    [Header("Debug Visualization")]
    public bool showMovementVectors = true;
    public bool showRaycasts = true;
    public bool showCollisions = true;
    public bool showStateInfo = true;
    public bool showPerformanceMetrics = true;
    private PlayerController playerController;
    private PlayerStateManager stateManager;
    private List<CollisionPoint> collisionPoints = new List<CollisionPoint>();

    private struct CollisionPoint
    {
        public Vector3 point;
        public Vector3 normal;
        public float time;
    }
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        stateManager = GetComponent<PlayerStateManager>();
    }

    void OnDrawGizmos()
    {
        if (!playerController || !stateManager) return;

        // Draw collision points
        if (showCollisions)
        {
            Gizmos.color = Color.red;
            collisionPoints.RemoveAll(p => Time.time - p.time > 2f);
            foreach (var point in collisionPoints)
            {
                Gizmos.DrawWireSphere(point.point, 0.1f);
            }
        }

        if (showMovementVectors)
        {
            // Movement vector (Blue)
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + playerController.GetMovementVector());
            
            // Forward vector (Red)
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 2f);
        }
        
        if (showRaycasts)
        {
            // Ground check ray (Green)
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (showCollisions)
        {
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.red, 2f);
            collisionPoints.Add(new CollisionPoint { 
                point = hit.point, 
                normal = hit.normal, 
                time = Time.time 
            });
        }
    }

    void OnGUI()
    {
        if (showStateInfo)
        {
            GUI.Box(new Rect(10, 10, 200, 100), "Player State Info");
            GUI.Label(new Rect(20, 30, 180, 20), $"Current State: {stateManager.currentState}");
            GUI.Label(new Rect(20, 50, 180, 20), $"Velocity: {playerController.GetVelocity():F2}");
            GUI.Label(new Rect(20, 70, 180, 20), $"Grounded: {playerController.IsGrounded()}");
        }
        
        if (showPerformanceMetrics)
        {
            GUI.Box(new Rect(Screen.width - 210, 10, 200, 100), "Performance Metrics");
            GUI.Label(new Rect(Screen.width - 200, 30, 180, 20), $"FPS: {1.0f / Time.deltaTime:F0}");
            GUI.Label(new Rect(Screen.width - 200, 50, 180, 20), $"Frame Time: {Time.deltaTime * 1000:F1}ms");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (showCollisions)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal * 2f, Color.yellow, 2f);
            }
        }
    }
    
}

