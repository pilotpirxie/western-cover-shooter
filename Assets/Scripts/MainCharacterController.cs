using System;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] private UnityEngine.CharacterController characterController;
    
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator crosshairAnimator;
    
    [SerializeField] private Vector3 playerVelocity;
    
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 2.0f;
    
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [SerializeField] private Vector2 rotation = new Vector2 (0, 0);
    
    [SerializeField] private float currentRotateSpeed;
    [SerializeField] private float zoomingRotateSpeed = 1f;
    [SerializeField] private float normalRotateSpeed = 3f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform instantiateTransform;

    [SerializeField] private int hp = 3;
    [SerializeField] private int maxBullets = 5;
    [SerializeField] private int bullets = 5;
    [SerializeField] private bool isReloading = false;
    
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        
        rotation.y += Input.GetAxis ("Mouse X") * currentRotateSpeed;
        rotation.x += rotation.x - Input.GetAxis ("Mouse Y") * currentRotateSpeed / 1.5f > -45f 
            && rotation.x - Input.GetAxis ("Mouse Y") * currentRotateSpeed / 1.5f < 45f 
            ? -Input.GetAxis ("Mouse Y") * currentRotateSpeed / 1.5f
            : 0f;
        transform.eulerAngles = rotation;
        
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading && bullets > 0)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f)) 
            {
                GameObject newBall = Instantiate(bulletPrefab, instantiateTransform.position, transform.rotation);
                newBall.GetComponent<Rigidbody>().velocity = (hit.point - instantiateTransform.position).normalized * 50f;
            }

            bullets--;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            isReloading = true;
            Invoke("FinishReloading", 3f);
        }
        
        if (Input.GetKey(KeyCode.W)) characterController.Move(transform.forward * (Time.deltaTime * currentSpeed));
        if (Input.GetKey(KeyCode.S)) characterController.Move(transform.forward * (Time.deltaTime * -currentSpeed));
        if (Input.GetKey(KeyCode.A)) characterController.Move(transform.right * (Time.deltaTime * -currentSpeed));
        if (Input.GetKey(KeyCode.D)) characterController.Move(transform.right * (Time.deltaTime * currentSpeed));

        currentSpeed = !Input.GetKey(KeyCode.Mouse1) && Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        currentRotateSpeed = Input.GetKey(KeyCode.Mouse1) ? zoomingRotateSpeed : normalRotateSpeed;
        
        characterAnimator.SetFloat("runningMultiplier", !Input.GetKey(KeyCode.Mouse1) && Input.GetKey(KeyCode.LeftShift) ? 1.75f : 1f);
        characterAnimator.SetBool("isMoving", Mathf.Abs(characterController.velocity.x) > 0.5f || Mathf.Abs(characterController.velocity.z) > 0.5f);
        cameraAnimator.SetBool("isZooming", Input.GetKey(KeyCode.Mouse1));
        crosshairAnimator.SetBool("isZooming", Input.GetKey(KeyCode.Mouse1));
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void FinishReloading()
    {
        isReloading = false;
        bullets = maxBullets;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            hp--;

            if (hp <= 0)
            {
                // Destroy(gameObject);
            }
        }
    }
}