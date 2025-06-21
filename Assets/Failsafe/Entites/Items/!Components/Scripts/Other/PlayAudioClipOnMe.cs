using UnityEngine;

public class PlayAudioClipOnMe : MonoBehaviour
{
    public void PlayAtMyPosition(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position);
    }
}
