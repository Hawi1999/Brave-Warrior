using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        ChestManager.SpawnReWardChest(ColorChest.Silver, TypeChest.Start, transform.position);
    }
    private void Update()
    {
        
    }
}
