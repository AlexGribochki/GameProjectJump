using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // �������� �����
    [SerializeField] private int lives = 3; // ���������� �����
    [SerializeField] private float jumpForce = 8f; // ���� ������
    private bool isGrounded = false;
    [SerializeField] private float pushForce = 3f; // ���� ������
    private bool hasBounced = false; // �������� �� �����
    private bool isAbilityActive = false; // ������� �� �����������
    private Vector2 pushDirection = Vector2.zero; // ����������� ��� ������
    private int collisionCount = 0;

    public Joystick joystick;
    public Button jumpButton;
    public Button abilityButton;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public static Hero instance { get; private set; }

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (jumpButton != null)
            jumpButton.onClick.AddListener(Jump);

        if (abilityButton != null)
            abilityButton.onClick.AddListener(ToggleAbility);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public void GetDamage(int damageAmount)
    {
        lives -= damageAmount;
        Debug.Log(lives);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGrounded) State = States.idle;

        if (joystick.Horizontal != 0)
            Run();

        if (isAbilityActive && joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            // �������� ����������� ��� ������ �� ���������
            pushDirection = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
        }

        if (Input.GetKeyDown(KeyCode.E))
            ToggleAbility();

        if (isAbilityActive && (joystick.Horizontal == 0 && joystick.Vertical == 0))
        {
            // ���� ����������� �� ������� � �������� � ������� ���������, ������� �����
            Push();
        }
    }

    private void Run()
    {
        if (!isAbilityActive && isGrounded)
        {
            State = States.run;

            Vector3 dir = transform.right * joystick.Horizontal;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
            sprite.flipX = dir.x < 0.0f;
        }
    }

    public void Jump()
    {
        if (!isAbilityActive && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            State = States.jump;
        }
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.2f);
        isGrounded = collider.Length > 0;

        if (!isGrounded) State = States.jump;
    }

    //�����������
    public void ToggleAbility()
    {
        isAbilityActive = !isAbilityActive;

        if (isAbilityActive)
        {
            // �������� ����� �����������
            State = States.ability;
            pushDirection = Vector2.zero; // �������� ����������� ������
        }
        else
        {
            // ��������� ����� �����������
            State = States.idle;
        }
    }

    public void Push()
    {
        if (pushDirection != Vector2.zero)
        {
            // ������� ����� � ��������� �����������
            rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionCount == 0)
        {
            Bounce(collision.contacts[0].normal);
            collisionCount++;
            isAbilityActive = false;
        }
        else if (collisionCount == 1)
        {
            rb.velocity = Vector2.zero;
            collisionCount = 0;
        }
    }

    private void Bounce(Vector2 collisionNormal)
    {
        Vector2 dir = Vector2.Reflect(rb.velocity, collisionNormal);
        rb.velocity = dir;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hasBounced = false;
    }
}

public enum States
{
    idle,
    run,
    jump,
    ability
}