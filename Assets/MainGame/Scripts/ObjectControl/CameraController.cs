using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 상수
    private static readonly float MOVEX = 28f;
    private static readonly float MOVEY = 17f;

    // 변수
    private Vector3 targetPosition;
    private bool isMove = false;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (isMove)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);

        if((transform.position - targetPosition).magnitude < 0.001f)
        {
            transform.position = targetPosition;
            isMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!MainGameManager.Instance.IsStopped && collision.CompareTag("Player"))
        {
            float xDiffer = collision.transform.position.x - transform.position.x;
            float yDiffer = collision.transform.position.y - transform.position.y;

            if(Mathf.Abs(xDiffer) > Mathf.Abs(yDiffer))
            {
                targetPosition.x = transform.position.x + MOVEX * (xDiffer > 0 ? 1 : -1);
                targetPosition.y = transform.position.y;
            }
            else
            {
                targetPosition.x = transform.position.x;
                targetPosition.y = transform.position.y + MOVEY * (yDiffer > 0 ? 1 : -1);
            }
            targetPosition.z = transform.position.z;

            isMove = true;
        }
    }
}
