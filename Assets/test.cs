using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        ChestManager.Instance.ReWardChest(TypeChest.Start, transform.position);
    }
    private void Update()
    {
        
    }
}
