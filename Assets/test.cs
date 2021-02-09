using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        ChestManager.Instance.ReWardChest(TypeChest.Copper, transform.position);
    }
}
