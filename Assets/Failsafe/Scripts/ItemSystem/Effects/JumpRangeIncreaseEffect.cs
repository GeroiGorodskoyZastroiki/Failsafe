using UnityEngine;

public class JumpRangeIncreaseEffect : BaseEffect
{
    public float JumpRangeIncreaseAmount;
    public override void Apply()
    {
        //to-do ����� ���������� ��������� ������
        Debug.Log($"��������� ������ ��������� �������� �� {JumpRangeIncreaseAmount}");
    }
}