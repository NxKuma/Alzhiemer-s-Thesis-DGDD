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

    private Camera playerCamera;
    private Transform currentLookedAtDoor;
    private bool isInteracting = false;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("No Camera");
            enabled = false;
            return;
        }

        if (InteractionUI != null)
        {
            InteractionUI.SetActive(false);
        }
    }

    void Update()
    {
        HandleDoorInteraction();
    }

    private void HandleDoorInteraction() {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool isLookingAtDoor = Physics.Raycast(ray, out hit, InteractionDistance, doorLayerMask);
        

        if (isLookingAtDoor)
        {
            Transform doorTransform = GetDoorTransformFromHit(hit.transform);
            
            if (doorTransform != null && doorTransform.name.StartsWith("Door_"))
            {
                if (InteractionUI != null) InteractionUI.SetActive(!isInteracting);

                if (Input.GetKeyDown(interactKey) && !isInteracting)
                {
                    currentLookedAtDoor = doorTransform;
                    RotateDoor(currentLookedAtDoor);
                }
            }
            else
            {
                if (InteractionUI != null) InteractionUI.SetActive(false);
            }
        }
        else
        {
            if (InteractionUI != null) InteractionUI.SetActive(false);
        }
    }

    private Transform GetDoorTransformFromHit(Transform hitTransform)
    {
        if (hitTransform.name.StartsWith("Door_") && IsActualDoorChild(hitTransform))
        {
            return hitTransform;
        }
        
        Transform current = hitTransform;
        while (current != null)
        {
            if (current.name.StartsWith("Door_"))
            {
                Transform actualDoorChild = FindActualDoorChild(current);
                return actualDoorChild != null ? actualDoorChild : current;
            }
            current = current.parent;
        }
        
        return null;
    }

    private bool IsActualDoorChild(Transform transform)
    {
        return transform.name.Contains("Child") || 
               transform.CompareTag("ActualDoor") || 
               transform.GetComponent<DoorReference>() != null;
    }

    private Transform FindActualDoorChild(Transform doorParent)
    {
        if (doorParent.childCount > 0)
        {
            DoorReference doorRef = doorParent.GetComponent<DoorReference>();
            if (doorRef != null && doorRef.actualDoor != null)
            {
                return doorRef.actualDoor;
            }
            
            foreach (Transform child in doorParent)
            {
                if (child.childCount > 0)
                {
                    foreach (Transform grandChild in child)
                    {
                        if (grandChild.name.Contains("Door") || 
                            grandChild.CompareTag("ActualDoor") ||
                            grandChild.GetComponent<BoxCollider>() != null)
                        {
                            return grandChild;
                        }
                    }
                }
                if (child.GetComponent<BoxCollider>() != null && 
                   (child.name.Contains("Door") || child.CompareTag("ActualDoor")))
                {
                    return child;
                }
            }
        }
        
        return null;
    }

    private void RotateDoor(Transform doorTransform)
    {
        if (isInteracting) return;
        isInteracting = true;

        Quaternion targetRotation = doorTransform.rotation * Quaternion.Euler(0, 0, 90);
        StartCoroutine(RotateDoorSmoothly(doorTransform, targetRotation, 0.5f));
    }

    private IEnumerator RotateDoorSmoothly(Transform doorTransform, Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = doorTransform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            doorTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        doorTransform.rotation = targetRotation;
        isInteracting = false;
        Debug.Log($"Door {doorTransform.name} rotated to {targetRotation.eulerAngles}");
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
