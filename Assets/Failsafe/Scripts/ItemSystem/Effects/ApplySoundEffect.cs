using UnityEngine;

public class ApplySoundEffect : BaseEffect
{
    // to-do �������� �� ���������� ����
    public AudioClip Sound;
    public override void Apply()
    {
        //to-do ����� �����
        Debug.Log($"��� ������ ���� {Sound}");
    }
}