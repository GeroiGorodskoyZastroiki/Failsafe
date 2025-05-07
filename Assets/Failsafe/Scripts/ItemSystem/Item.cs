using UnityEngine;

/// <summary>
/// ������� ����� ��� ���� ���������
/// ��������� ���������� ����������
/// �������� ������ ��������
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    public Vector3 baseSize = Vector3.one; // ������� ������ ��� �������
    private Material originalMaterial;
    private Renderer itemRenderer;

    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
        itemRenderer = GetComponent<Renderer>();
        originalMaterial = itemRenderer.material;
    }

    public void ToggleHighlight(bool state)
    {
        itemRenderer.material = state ? highlightMaterial : originalMaterial;
    }
}