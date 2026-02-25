using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float InteractionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask doorLayerMask = 1;

    [Header("Visual Feedback")]
    public GameObject InteractionUI;

    [Header("Door Settings")]
    public float openAngle = -90f; // Angle when door is open (-90 for clockwise from above)
    public float animationDuration = 0.5f;

    private Camera playerCamera;
    private Transform currentLookedAtDoor;
    private bool isInteracting = false;
    private SFXManager sfxManager;

    void Start()
    {
        sfxManager = SFXManager.Instance;
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("No Camera");
            enabled = false;
            return;
        }

        if (InteractionUI != null)
        {
            InteractionUI.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    void Update()
    {
        HandleDoorInteraction();
    }

    private void HandleDoorInteraction() {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool isLookingAtDoor = Physics.Raycast(ray, out hit, InteractionDistance, doorLayerMask)
            && hit.transform.name.StartsWith("Door_");

        if (InteractionUI != null && (isLookingAtDoor && !isInteracting)) InteractionUI.GetComponent<CanvasGroup>().alpha = 1f;
        else if (InteractionUI != null) InteractionUI.GetComponent<CanvasGroup>().alpha = 0f;

        if (isLookingAtDoor && Input.GetKeyDown(interactKey) && !isInteracting)
        {
            currentLookedAtDoor = hit.transform;
            ToggleDoor(currentLookedAtDoor);
            
        }
    }

    private void ToggleDoor(Transform doorTransform)
    {
        if (isInteracting) return;

        Transform actualDoor = GetActualDoorChild(doorTransform);
        if (actualDoor == null) return;

        bool isOpen = IsDoorOpen(actualDoor);
        
        if (isOpen)
        {
            CloseDoor(actualDoor);
            _ = sfxManager.PlaySFX("DoorClose");
        }
        else
        {
            OpenDoor(actualDoor);
            _ = sfxManager.PlaySFX("door");
        }
    }

    private Transform GetActualDoorChild(Transform doorParent)
    {
        if (doorParent.childCount > 0)
        {
            Transform doorFrame = doorParent.GetChild(0);
            if (doorFrame.childCount > 0)
            {
                return doorFrame.GetChild(0);
            }
        }
        return doorParent; // Fallback
    }

    private bool IsDoorOpen(Transform doorTransform)
    {
        float currentYRotation = doorTransform.localEulerAngles.y;
        
        if (currentYRotation > 180f)
            currentYRotation -= 360f;
        
        return Mathf.Abs(currentYRotation + openAngle) < 10f;
    }

    private void OpenDoor(Transform doorTransform)
    {
        if (isInteracting) return;
        isInteracting = true;

        DoorState doorState = doorTransform.GetComponent<DoorState>();
        if (doorState == null)
        {
            doorState = doorTransform.gameObject.AddComponent<DoorState>();
            doorState.closedRotation = doorTransform.localRotation;
        }

        Quaternion targetRotation = doorState.closedRotation * Quaternion.Euler(0, 0, -openAngle);
        StartCoroutine(RotateDoorSmoothly(doorTransform, targetRotation, animationDuration, true));
    }

    private void CloseDoor(Transform doorTransform)
    {
        if (isInteracting) return;
        isInteracting = true;

        // Get the original closed rotation from DoorState component
        DoorState doorState = doorTransform.GetComponent<DoorState>();
        if (doorState == null)
        {
            // If no DoorState found, assume current rotation is closed
            doorState = doorTransform.gameObject.AddComponent<DoorState>();
            doorState.closedRotation = doorTransform.localRotation;
        }

        StartCoroutine(RotateDoorSmoothly(doorTransform, doorState.closedRotation, animationDuration, false));
    }

    private IEnumerator RotateDoorSmoothly(Transform doorTransform, Quaternion targetRotation, float duration, bool opening)
    {
        Quaternion startRotation = doorTransform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            doorTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        doorTransform.localRotation = targetRotation;
        isInteracting = false;
        
        Debug.Log($"Door {(opening ? "opened" : "closed")} - Rotation: {doorTransform.localEulerAngles}");
    }

    void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.green;
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Gizmos.DrawRay(ray.origin, ray.direction * InteractionDistance);
        }
    }
}

public class DoorState : MonoBehaviour
{
    [HideInInspector]
    public Quaternion closedRotation;
}