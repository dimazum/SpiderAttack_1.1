using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharState
{
    Idle,
    Run,
    Rubilovo,
    IdleBlink
}


public class MoveManager : MonoBehaviour
{

    private Rigidbody2D rb;
    private float move;                   //перемещение из инпута
    public bool richtDirect;         //направление
    public float speed = 2f;                  //скорость
    RaycastHit2D hit;
    public float horizontalRayRange = 0.4f;



    public GameObject explosion; //анмация хватания объекта

    public GameObject stairs;  //ссылка на лестницу
    public GameObject support; //ссылка на опору
    public GameObject bomb;//ссылка на бомбу


    public LayerMask tumanMask;

    public Sprite[] destroySp;
    public Sprite[] backgroundSp;
    public Sprite[] snotSp;


    Animator animator;
    private bool isGrounded; //нахождение на земле
    private bool canUP;      //возможность ползти по лестнице
    private bool canMove;
    private bool canMoveUp; //есть ли сверху препятствие
    private bool underMeStairs;

    public CharState state
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }


    void Awake()
    {
        //Application.targetFrameRate = -1;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        richtDirect = false;

        isGrounded = false;
        canUP = false;
        canMove = false;
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            move = Input.GetAxisRaw("Horizontal");

            if (canMove)
            {
                Vector2 vector2 = transform.right * move;
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + vector2, speed * Time.deltaTime);
                state = CharState.Run;
            }

            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right * move, horizontalRayRange, 1 << 10);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right * move, Color.red, horizontalRayRange);

            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("block"))
                {
                    canMove = false;
                    state = CharState.Rubilovo;
                }
            }
            else
            {
                canMove = true;
            }

            if (move > 0 && !richtDirect) Flip();
            if (move < 0 && richtDirect) Flip();
        }

        else if (Input.GetButton("Vertical"))
        {
            move = Input.GetAxisRaw("Vertical");

            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up * move, 1f, 1 << 10);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up * move, Color.red, horizontalRayRange);

            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("block"))
                {
                    canMove = false;
                    state = CharState.Rubilovo;
                }
            }
            else
            {
                canMove = true;
            }
        }
        else
        {
            state = CharState.IdleBlink;
        }
    }

    public void HitBlock()
    {
        GameObject currenrblock = hit.collider?.gameObject;
        if (currenrblock != null)
        {
            BlockGroundDefault blockGroundDefault = currenrblock.GetComponent<BlockGroundDefault>();
            blockGroundDefault?.Hit();
        }
    }

    void Flip()
    {
        richtDirect = !richtDirect;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
