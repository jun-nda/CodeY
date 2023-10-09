using System;
using UnityEngine;

public class PlayerControllor : MonoBehaviour
{
    private CharacterController characterController;
	[SerializeField] private Camera camera;
	public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    public float gravity;
    public float jumpHeight;

    private Vector3 movementDirection;
    private Vector2 mouseLook;
    private Vector2 smoothV;
    
    public float WalkSpeed;
    public float CurrentSpeed { get; private set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

	public void Init(InputManager inputManager)
	{
		InputManager.OnMouseMoveDelta += HandleMouseMove;
	}

	void Update()
    {
        CurrentSpeed = WalkSpeed;
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
		mouseLook += smoothV;

		mouseLook.y = Mathf.Clamp(mouseLook.y, -60f, 90f);

		camera.transform.localRotation = Quaternion.Euler(- mouseLook.y, mouseLook.x, 0);

		if (characterController.isGrounded)
		{
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection = camera.transform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            movementDirection.y = 0;
            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = jumpHeight;
            }
		}

        movementDirection.y -= gravity * Time.deltaTime;
        var tmp_Movement = CurrentSpeed * movementDirection;
        characterController.Move(tmp_Movement);
	}

    private void HandleMouseMove(Vector3 vec)
	{
        //Debug.Log("HandleMouseMove" + vec);
	}

    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}