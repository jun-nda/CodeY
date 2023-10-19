using System;
using UnityEngine;
using Common.UIScript;

public class PlayerControllor : MonoBehaviour
{
	private CharacterController characterController;
	private Animator characterAnimator;

	/// <summary>
	/// 控制角色是否继续更新（此时在开面板，相当于是暂停）初始是暂停
	/// </summary>
	private bool characterLock = true;

	[SerializeField] private Camera playerCamera;
	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;
	public float gravity;
	public float jumpHeight;

	private Vector3 movementDirection;
	private Vector2 mouseLook;
	private Vector2 smoothV;

	public float WalkSpeed;
	public float RunSpeed;
	public float CurrentSpeed { get; private set; }

	private Vector3 forwardDirection;
	private Vector3 rightDirection;

	private float tmp_Horizontal;
	private float tmp_Vertical;

	void Start( )
	{
		/// 注册开面板事件，设置characterLock参数，当设置为false时，update函数不再运行，将鼠标释放出来
		EventManager.Register("fpsLock", SetCharacterLockState);
		characterAnimator = GetComponentInChildren<Animator>( );
		characterController = GetComponent<CharacterController>( );
	}

	public void Init( )
	{
		InputManager.OnMouseMoveDelta += HandleMouseMove;
		InputManager.OnSpaceKeyDown += HandleSpaceKeyDown;
		InputManager.OnEscapeKeyDown += HandleEscapeKeyDown;

		characterAnimator.enabled = false;
	}

	/// <summary>
	/// 设置状态
	/// </summary>
	/// <param name="eventData">开关</param>
	public void SetCharacterLockState( object eventData )
	{
		bool isLock = (bool)eventData;
		characterLock = isLock;

		if ( characterLock == false )
		{
			characterAnimator.enabled = true;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{ 
			characterAnimator.enabled = false;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

	}

	void Update( )
	{
		if ( characterLock == false )
		{
			CameraControl( );
			PlayerMovementControl( );
		}
	}

	// 控制相机视角转动
	private void CameraControl( )
	{
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
		mouseLook += smoothV;

		mouseLook.y = Mathf.Clamp(mouseLook.y, -60f, 90f);

		playerCamera.transform.localRotation = Quaternion.Euler(-mouseLook.y, mouseLook.x, 0);
	}

	// 控制角色移动
	private void PlayerMovementControl( )
	{
		CurrentSpeed = WalkSpeed;
		if ( characterController.isGrounded )
		{
			tmp_Horizontal = Input.GetAxis("Horizontal");
			tmp_Vertical = Input.GetAxis("Vertical");

			//CameraVec.x = playerCamera.transform.rotation.x;
			//CameraVec.x = playerCamera.transform.rotation.x;
			Debug.Log("playerCamera " + tmp_Horizontal);
			//movementDirection = playerCamera.transform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
			//movementDirection.y = 0;

			//Vector3 moveDirection = new Vector3(tmp_Horizontal, 0f, tmp_Vertical).normalized;
			// TODO 奔跑 + 下蹲

			forwardDirection = playerCamera.transform.forward;
			rightDirection = playerCamera.transform.right;
			forwardDirection.y = 0f;
			rightDirection.y = 0f;
			forwardDirection.Normalize();
			rightDirection.Normalize();

			forwardDirection *= tmp_Vertical;
			rightDirection *= tmp_Horizontal;

			// 计算摄像机在水平方向上的移动距离
			movementDirection = forwardDirection + rightDirection;
		}

		UpdateMoveMent();
	}

	private void HandleMouseMove( Vector3 vec )
	{
		//Debug.Log("HandleMouseMove" + vec);
	}

	// ESC打开设置面板
	private void HandleEscapeKeyDown( )
	{
		characterLock = true;
		Debug.Log("OnEscapeKeyDown");
	}

	// 空格键跳跃
	private void HandleSpaceKeyDown( )
	{
		if ( characterController.isGrounded )
		{
			movementDirection.y = jumpHeight;
			UpdateMoveMent();
		}
	}

	//更新移动
	void UpdateMoveMent( )
	{
		//这里是在计算重力下坠
		movementDirection.y -= gravity * Time.deltaTime;
		var tmp_Movement = CurrentSpeed * movementDirection;
		characterController.Move(tmp_Movement);

		characterAnimator.SetFloat("Velocity", forwardDirection.magnitude, 0.25f, Time.deltaTime);
		characterAnimator.SetFloat("shifting", - rightDirection.magnitude * tmp_Horizontal, 0.25f, Time.deltaTime);
	}
}