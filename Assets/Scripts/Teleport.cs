using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] bool isGoingMainFloor;

    TeleportManager teleportManager;

    private void Awake()
    {
        teleportManager = TeleportManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int floorIndex = isGoingMainFloor ? 0 : Random.Range(1, 4);
            teleportManager.TeleportPlayer((FloorState)floorIndex);
        }
    }
}
