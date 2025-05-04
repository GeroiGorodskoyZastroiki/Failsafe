using FMOD;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemySoundListener : MonoBehaviour, IHearSound
{
    private enum ReactionType { Near, Medium, Far } // Fixed: Changed to a proper enum declaration

    [SerializeField] private float hearingMultiplier = 1.0f;

    public void OnSoundHeard(Vector3 soundPosition, SoundData data, float distance)
    {
        switch (data.soundType)
        {
            case SoundType.Footstep:
                if (distance < data.maxRadius * 0.3f)
                {
                    Debug.Log($"{name} ������� ���� � ������������.");
                    // ������� � state Alert, ��������
                }
                break;

            case SoundType.Explosion:
                Debug.Log($"{name} ������� ������� ������� � ���� ���������.");
                // ������� � ��������� Search
                break;

            case SoundType.Distract:
                Debug.Log($"{name} ������� ������� ���� � �������!");
                // ����������� ������� � ����������� ���������
                break;

            case SoundType.Impact:
                Debug.Log($"{name} �������� �� ���.");
                // ������� � ����� �����
                break;
        }
    }
    private void SuspiciousLook(Vector3 pos) => Debug.Log($"{name} is suspicious near {pos}");
    private void Investigate(Vector3 pos) => Debug.Log($"{name} is investigating {pos}");
    private void MoveToDistractPoint(Vector3 pos) => Debug.Log($"{name} is distracted and moves to {pos}");
    private void Alert(Vector3 pos) => Debug.Log($"{name} is alert and running to {pos}");

   
}
