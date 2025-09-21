using UnityEngine;

public class DoorInteraction : MonoBehaviour
{

    [Header("Interaction Settings")]
    public float InteractionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask doorLayerMask = 1;

    [Header("Visual Feedback")]
    // bubble
    public GameObject InteractionUI;

    private Camera playerCamera;
    private Transform currentLookedAtDoor;
    private bool isInteracting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        HandleDoorInteraction();
    }

    private void HandleDoorInteraction() {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool isLookingAtDoor = Physics.Raycast(ray, out hit, InteractionDistance, doorLayerMask)
            && hit.transform.name.StartsWith("Door_");

        if (InteractionUI != null) InteractionUI.SetActive(isLookingAtDoor && !isInteracting);

        if (isLookingAtDoor && Input.GetKeyDown(interactKey) && !isInteracting)
        {
            currentLookedAtDoor = hit.transform;
            RotateDoor(currentLookedAtDoor);
        }
    }

    private void RotateDoor(Transform doorTransform)
    {
        if (isInteracting) return;

        isInteracting = true;

        Quaternion targetRotation = doorTransform.rotation * Quaternion.Euler(0, 90, 0);
        StartCoroutine(RotateDoorSmoothly(doorTransform, targetRotation, 0.5f));
    }

    private System.Collections.IEnumerator RotateDoorSmoothly(Transform doorTransform, Quaternion targetRotation, float duration)
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
