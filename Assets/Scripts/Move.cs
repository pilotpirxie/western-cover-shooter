using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]private CharacterController controller;
    [SerializeField]private Vector3 playerVelocity;
    
    [SerializeField]private float currentSpeed;
    [SerializeField]private float walkSpeed = 2.0f;
    [SerializeField]private float runSpeed = 2.0f;
    
    [SerializeField]private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    Vector2 rotation = new Vector2 (0, 0);
    public float rotateSpeed = 3;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        
        rotation.y += Input.GetAxis ("Mouse X");
        rotation.x += -Input.GetAxis ("Mouse Y") / 1.5f;
        transform.eulerAngles = rotation * rotateSpeed;
        
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            controller.Move(transform.forward * (Time.deltaTime * currentSpeed));
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            controller.Move(transform.forward * (Time.deltaTime * -currentSpeed));
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            controller.Move(transform.right * (Time.deltaTime * -currentSpeed));
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            controller.Move(transform.right * (Time.deltaTime * currentSpeed));
        }
        
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}