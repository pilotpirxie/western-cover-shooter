using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private UnityEngine.CharacterController characterController;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Vector3 playerVelocity;
    
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 2.0f;
    
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    Vector2 rotation = new Vector2 (0, 0);
    public float rotateSpeed = 3;

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        
        rotation.y += Input.GetAxis ("Mouse X");
        rotation.x += -Input.GetAxis ("Mouse Y") / 1.5f;
        transform.eulerAngles = rotation * rotateSpeed;
        
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(transform.forward * (Time.deltaTime * currentSpeed));
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(transform.forward * (Time.deltaTime * -currentSpeed));
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(transform.right * (Time.deltaTime * -currentSpeed));
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(transform.right * (Time.deltaTime * currentSpeed));
        }
        
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        characterAnimator.SetFloat("runningMultiplier", Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f);
        characterAnimator.SetBool("isMoving", Mathf.Abs(characterController.velocity.x) > 0.5f || Mathf.Abs(characterController.velocity.z) > 0.5f);
        cameraAnimator.SetBool("isZooming", Input.GetKey(KeyCode.Mouse1));
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}