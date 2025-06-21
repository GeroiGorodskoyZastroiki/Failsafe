using UnityEngine;

public class GameObjectDestructor : MonoBehaviour
{
    public void DestroyGO(int delay)
    {
        Destroy(gameObject, delay);
    }
}
