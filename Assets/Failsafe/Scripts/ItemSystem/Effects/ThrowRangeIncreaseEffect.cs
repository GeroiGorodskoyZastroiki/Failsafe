using UnityEngine;

public class ThrowRangeIncreaseEffect : BaseEffect
{
    public float ThrowRangeIncreaseAmount;
    public override void Apply()
    {
        //to-do ����� ���������� ��������� ������ ���������
        Debug.Log($"��������� ������ ��������� �������� �� {ThrowRangeIncreaseAmount}");
    }
}