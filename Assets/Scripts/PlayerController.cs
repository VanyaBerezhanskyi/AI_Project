using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Camera mainCamera;
    private bool isShooting = false;
    private ShootingСharacter shootingCharacter;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        shootingCharacter = GetComponent<ShootingСharacter>();
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isShooting)
        {
            shootingCharacter.Shoot();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePos = context.ReadValue<Vector2>();

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 lookPoint = hitInfo.point;
            lookPoint.y = transform.position.y;
            
            transform.LookAt(lookPoint);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        isShooting = context.performed;
    }
}
