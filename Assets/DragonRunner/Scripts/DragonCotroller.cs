using UnityEngine;

public class DragonCotroller : MonoBehaviour
{
    // 상수
    static readonly int RUN = Animator.StringToHash("IsRun");
    static readonly int JUMP = Animator.StringToHash("IsJump");
    static readonly int DEAD = Animator.StringToHash("Dead");

    // 변수
    [SerializeField] float baseSpeed;
    [SerializeField] float jumpPower;
    private float speed = 0f;
    private bool isJump = false;

    // gameObject 컴포넌트
    Rigidbody2D rigid;
    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (RunnerGameManager.Instance.IsPlaying)
        {
            // 드래곤은 항상 오른쪽으로 이동
            transform.position += speed * Time.deltaTime * Vector3.right;
            
            // 스페이스 키를 통한 점프
            if (!isJump && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    }

    // 초기 속력과 애니메이션 설정
    public void Init()
    {
        speed = baseSpeed;
        animator.SetBool(RUN, true);
    }

    // 드래곤에게 위로 힘을 주어 점프
    private void Jump()
    {
        rigid.AddForce(jumpPower * rigid.mass * Vector2.up, ForceMode2D.Impulse);
        animator.SetBool(JUMP, true);
        isJump = true;
    }

    // 드래곤 사망 시 애니메이션 적용
    private void DeadAnimation()
    {
        animator.SetTrigger(DEAD);
        rigid.AddForce(baseSpeed * 0.5f * rigid.mass * (Vector2.up + Vector2.left), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!RunnerGameManager.Instance.IsPlaying) return;

        // 장애물에 닿으면 게임 오버
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            RunnerGameManager.Instance.GameOver();
            DeadAnimation();
        }
        
        // 땅에 닿으면 다시 점프 가능
        if (isJump && collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            animator.SetBool(JUMP, false);
        }
    }
}
