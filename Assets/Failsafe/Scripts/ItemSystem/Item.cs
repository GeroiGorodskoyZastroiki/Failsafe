using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� ����� ��� ���� ���������
/// ��������� ���������� ����������
/// �������� ������ ��������
/// </summary>
public class Item : MonoBehaviour
{
    public int usesCount = -1;
    [SerializeField] private Material highlightMaterial;
    public Vector3 baseSize = Vector3.one; // ������� ������ ��� �������

    public BaseEffect[] effects = null;
    private Material originalMaterial;
    private Renderer itemRenderer;

    public void ToggleHighlight(bool state)
    {
        itemRenderer.material = state ? highlightMaterial : originalMaterial;
    }

    public void Use()
    {
        foreach (var effect in effects) 
        {
            effect.Apply();
        }
        Debug.Log("������� �����������");
    }

    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
        itemRenderer = GetComponent<Renderer>();
        originalMaterial = itemRenderer.material;
    }
}