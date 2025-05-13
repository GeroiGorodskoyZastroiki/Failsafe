using UnityEngine;

/// <summary>
/// ������������ ���� ������
/// ��������� ���������/��������� ���������
/// </summary>
public class InventoryInputHandler : MonoBehaviour
{
    [SerializeField] private float lookDownThreshold = 0.9f;
    /// <summary>
    /// ����� ������� ���������, ������� ������� ���� ����
    /// </summary>
    [SerializeField] private float openInventoryOffset = 0.10f;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private PhysicsPickup physicsPickup;

    private InventoryBeltHandler beltHandler;
    private InventoryDragHandler dragHandler;
    private InventoryManager inventoryManager;
    private bool isInventoryOpen;

    private void Start()
    {
        inventoryManager = gameObject.GetComponent<InventoryManager>();
        if (beltHandler is null)
        {
            beltHandler = gameObject.GetComponent<InventoryBeltHandler>();
        }
        beltHandler.inventoryManager = inventoryManager;
        if (dragHandler is null)
        {
            dragHandler = gameObject.GetComponent<InventoryDragHandler>();
        }
        dragHandler.playerCamera = PlayerCamera;
        dragHandler.inventoryManager = inventoryManager;
        dragHandler.physicsPickup = physicsPickup;

    }
    private void Update()
    {
        CheckInventoryToggle();
    }
    private void CheckInventoryToggle()
    {
        float lookDot = Vector3.Dot(PlayerCamera.transform.forward, Vector3.down);
        bool shouldOpen = lookDot > (isInventoryOpen ? lookDownThreshold - openInventoryOffset : lookDownThreshold);

        if (shouldOpen != isInventoryOpen)
        {
            isInventoryOpen = shouldOpen;
            if (physicsPickup.GetCurrentObject(out var currentObject))
            {
                dragHandler.SetDraggingItem(currentObject);
            }
            UpdateInventoryVisibility(isInventoryOpen);
        }
    }

    private void UpdateInventoryVisibility(bool visible)
    {
        inventoryManager.SetInventoryState(visible);
        dragHandler.enabled = visible;

        // to-do �������� ��������
        // to-do ���� ��������-��������

        // ������ ��������� �������
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            dragHandler.TryEndDrag();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // ������ ���� ������
        PlayerCamera.cullingMask = visible ?
            PlayerCamera.cullingMask | (1 << LayerMask.NameToLayer("Inventory")) :
            PlayerCamera.cullingMask & ~(1 << LayerMask.NameToLayer("Inventory"));
    }
}