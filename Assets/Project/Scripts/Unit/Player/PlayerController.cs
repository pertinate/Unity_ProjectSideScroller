using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PhysicsObject {
    public static PlayerController instance;
    protected Joystick joystick;
    public float jumpTakeOffSpeed = 7;
    public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;
    public float maxSpeed = 7;
    public float gravity = -7f;
    private SpriteRenderer spriteRenderer;
    public Vector3 velocity;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        joystick = Joystick.instance;
        gravity = -(2 * jumpTakeOffSpeed) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

    }

    void Update()
    {
        if (collisions.above || collisions.below)
            velocity.y = 0;
        if (JumpButton.instance.canJump)
            velocity.y = jumpTakeOffSpeed;
        float targetVelocityX = joystick.InputDirection.x * maxSpeed;
        bool facingRight = (spriteRenderer.flipX ? (targetVelocityX > 0.01f) : (targetVelocityX < -0.01f));
        if (facingRight)
            spriteRenderer.flipX = !spriteRenderer.flipX;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        Move(velocity * Time.deltaTime);
    }
    /*public bool Jump()
    {
        if (grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            return true;
        }
        else
            return false;
    }
    protected void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = joystick.InputDirection.x;
        targetVelocity = move * maxSpeed;
        bool facingRight = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (facingRight)
            spriteRenderer.flipX = !spriteRenderer.flipX;
    }*/
}
