using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 3;

    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    //키보드 입력에 의한 플레이어의 이동을 실행
    void Move()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        if (xMove == 0f && yMove == 0f)
        {
            animator.SetBool("IsMove", false);
            return;
        }

        Vector3 moveVector = new Vector3(xMove, yMove).normalized * speed * Time.deltaTime;
        transform.position += moveVector;

        UpdateFacingDirection(moveVector.x);
        animator.SetBool("IsMove", true);
    }

    void UpdateFacingDirection(float x)
    {
        if (x == 0f) return;

        if(x < 0f) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
