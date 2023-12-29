using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float sprintSpeed = 14f;
    public float maxVelocityChange = 10f;

    [Space]
    public float airControl = 0.5f;

    [Space]  // Corrected attribute name

    public float jumpHeight = 30f;
    private Vector2 input;

    private Rigidbody rb;

    private bool sprinting;
    private bool jumping;

    private bool grounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        input.Normalize();

        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");

    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);

            }
            else if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                Vector3 velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 8.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 8.2f * Time.fixedDeltaTime);  // Corrected syntax
                rb.velocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                Vector3 velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 8.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 8.2f * Time.fixedDeltaTime);  // Corrected syntax
                rb.velocity = velocity1;
            }
        }
        

        grounded= false;
    }

    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return velocityChange;
        }
        else
        {
            return new Vector3();
        }
    }
}
