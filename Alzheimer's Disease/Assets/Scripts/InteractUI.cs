using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject _containerGameObject;
    [SerializeField] private Interactor _playerInteract;

    private void Update()
    {
        if (_playerInteract.GetNPCInteractable() != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        _containerGameObject.SetActive(true);
    }

    private void Hide()
    {
        _containerGameObject.SetActive(false);
    }
}
