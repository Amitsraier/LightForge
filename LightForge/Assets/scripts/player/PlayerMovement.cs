using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public bool EnableGizmos;

    [Header("Movement")]
    public float ExcelerationSpeed;

    public float MaxSpeed;

    public float DecelerationSpeed;

    public float TurnAroundMultiplier;

    public float HorizontalDeadZone;

    [HideInInspector]
    public float HorizontalInput;


    [Header("Jump")]
    public float JumpForce;

    public float FallGravityScale;
    public float JumpGravityScale;

    public float ReleaseJumpMultiplier;

    [Header("Ground detection")]
    public GameObject Detector;

    public float DetectorRadius;

    public LayerMask PlatformLayer;


    [System.Serializable]
    public struct PlayerInputs
    {
        public InputActionReference Horizontal;
        public InputActionReference Vertical;

        public InputActionReference Jump;
        public InputActionReference ReleaseJump;
    }

    [Header("Inputs")]
    public PlayerInputs Inputs;

    private Vector2 PlayerVelocity;

    [HideInInspector]
    public CapsuleCollider2D coll;
    [HideInInspector]
    public Rigidbody2D rb;

    private void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        Inputs.ReleaseJump.action.canceled += ReleaseJump;

        rb.gravityScale = FallGravityScale;

        Inputs.Horizontal.action.started += OnTurnAround;
    }

    void Update()
    {
        HorizontalInput = Inputs.Horizontal.action.ReadValue<float>();

        PlayerVelocity.x += HorizontalInput * ExcelerationSpeed * Time.deltaTime;

        PlayerVelocity.x = Mathf.Clamp(PlayerVelocity.x, -MaxSpeed, MaxSpeed);

        if (Mathf.Abs(HorizontalInput) < HorizontalDeadZone) PlayerVelocity.x = Mathf.Lerp(PlayerVelocity.x, 0, DecelerationSpeed * Time.deltaTime);

        if (Inputs.Jump.action.ReadValue<float>() == 1 && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }


        if (rb.velocity.y > 0) rb.gravityScale = JumpGravityScale;
        if (rb.velocity.y < 0) rb.gravityScale = FallGravityScale;

        rb.velocity = new Vector2(PlayerVelocity.x, rb.velocity.y + PlayerVelocity.y);
    }

    private void ReleaseJump(InputAction.CallbackContext context)
    {
        if (rb.velocity.y < 0) return;

        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * ReleaseJumpMultiplier);
    }

    void OnTurnAround(InputAction.CallbackContext context)
    {
        PlayerVelocity.x *= TurnAroundMultiplier;
    }

    public bool IsGrounded()
    {
        return Physics2D.CircleCast(Detector.transform.position, DetectorRadius, Vector2.zero, 0, PlatformLayer);
    }

    public int GetGroundAngle()
    {
        return Mathf.RoundToInt(Vector2.Angle(Physics2D.CircleCast(Detector.transform.position, DetectorRadius, Vector2.zero, 0, PlatformLayer).normal, Vector2.up));
    }

    private void OnDrawGizmos()
    {
        if (!EnableGizmos) return;

        Gizmos.color = IsGrounded() ? Color.green : Color.red;

        Gizmos.DrawWireSphere(Detector.transform.position, DetectorRadius);
    }
}
