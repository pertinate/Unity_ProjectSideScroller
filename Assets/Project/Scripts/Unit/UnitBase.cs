using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UnitBase : MonoBehaviour {
    public static UnitBase _TEMPINSTANCE;
    protected const float _GRAVITY = -4.81f;
    protected Vector2 _VELOCITY;
    protected const float skinWidth = .015f;

    protected BoxCollider2D collider;
    protected RaycastOrigins raycastOrigins;
    protected CollisionInfo collisions;

    public LayerMask collisionMask;

    public float moveSpeed = 3f;
    public float jumpHeight = 3f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    protected bool canJump = false;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing, verticalRaySpacing, velocityXSmoothing;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    void Awake()
    {
        _TEMPINSTANCE = this;
        collider = GetComponent<BoxCollider2D>();
        collisions.Reset();
        CalculateRaySpacing();
    }
    void Update()
    {
        Move(Joystick.instance.InputDirection);
    }

    void FixedUpdate()
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        ApplyGravity();
        ApplyVerticalCollisions();

        ApplyHorizontalCollisions();


        transform.Translate(_VELOCITY);
    }
    protected void Move(Vector3 velocity)
    {
        float targetVelocityX = velocity.x * moveSpeed;
        _VELOCITY.x = Mathf.SmoothDamp(_VELOCITY.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne) * Time.deltaTime;
    }
    void ApplyGravity()
    {
        _VELOCITY.y += _GRAVITY * Time.deltaTime;
    }
    public void PlayerJump()
    {
        _VELOCITY.y = jumpHeight;
    }

    void ApplyVerticalCollisions()
    {
        float directionY = Mathf.Sign(_VELOCITY.y);
        
        float rayLength = Mathf.Abs(_VELOCITY.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + _VELOCITY.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            print(directionY);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                _VELOCITY.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void ApplyHorizontalCollisions()
    {
        float directionX = Mathf.Sign(_VELOCITY.x);
        float rayLength = Mathf.Abs(_VELOCITY.x) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                _VELOCITY.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topLeft = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    protected struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
        }
    }
}
