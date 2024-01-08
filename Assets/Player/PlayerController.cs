using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private Animator animator;
    private Vector2 input;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private string currentAnimState = "Idle";
    private void Update()
    {
        float half = Screen.width / 2;
        Vector3 scale = transform.localScale;
        float scaleX = Mathf.Abs(scale.x);
        if (Input.mousePosition.x < half)
        {
            animator.gameObject.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
        }
        else if (Input.mousePosition.x > half)
        {
            animator.gameObject.transform.localScale = new Vector3(scaleX * -1, scale.y, scale.z);
        }


        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.magnitude > 0)
        {
            if (currentAnimState != "Run")
            {
                animator.CrossFade("Run", 0, 0);
                currentAnimState = "Run";
            }
        }
        else
        {
            if (currentAnimState != "Idle")
            {
                animator.CrossFade("Idle", 0, 0);
                currentAnimState = "Idle";
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position
           + new Vector3(input.x, input.y).normalized * speed * Time.deltaTime);
    }
}
