using UnityEngine;

public class HealEffect : BaseEffect
{
    public int HealAmount;
    public override void Apply()
    {
        //to-do ����� ���� ��� ���������
        Debug.Log($"�������� ����������� {HealAmount}");
    }
}