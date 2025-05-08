using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� ����� ��� ���� ���������
/// ��������� ���������� ����������
/// �������� ������ ��������
/// </summary>
public class Item : MonoBehaviour
{
    //� �� ��� ������: ���� �������� (� �.�. �����), ��������� �� ��� ����� ��������, ��������� ����� � ����, ��������� ��� � ������ � ���������. ������� �� �������� (https://docs.google.com/document/d/1sTkx0TCpQNYyGT-ZiLcsQcQ3usvauy_Qi0h-b4-ZqNM/edit?tab=t.0#heading=h.2slexxz8qzx)
    //��������, ������� ����� �������� � ��������� ����� �� ���� ������ � ����� � ����. ��������� �������� ��� ������ �� � ��������� ����� ����� ������������ (��� ���������)
    //��� ��������, ������� ����� ������ � ����� ����� ������/�������� (� �.� ��, ������� ����� ������������)

    //�����������:
    //enum ��� ������� ��������
    //����� ����� ����������� ����� Item: ���-��, ��� ����� ����� � ���� � �������� � ���������
    //��������� IUsable: ���� ����������� Use

    public ItemRarity Rarity;
    public ItemSize Size;

    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
}