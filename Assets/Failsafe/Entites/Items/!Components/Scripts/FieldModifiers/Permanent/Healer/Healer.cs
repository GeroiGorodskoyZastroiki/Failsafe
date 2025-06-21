using Failsafe.Player;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public HealData Data;

    public void Heal()
    {
        //GetComponentInParent<PlayerLifetimeScope>().Health += HealData.HealAmount;
        Debug.Log("Healed");
    }
}
