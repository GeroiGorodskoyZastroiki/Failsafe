using UnityEngine;

public class MoveSpeedIncreaseEffect : BaseEffect
{
    public float MoveSpeedIncreaseAmount;
    public override void Apply()
    {
        //to-do ����� ���������� �������� ���������
        Debug.Log($"�������� ��������� �������� �� {MoveSpeedIncreaseAmount}");
    }
}