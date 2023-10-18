using System;
using UnityEngine;
using Common.UIScript;

public class PlayerControllor : MonoBehaviour
{
	private CharacterController characterController;
	private Animator characterAnimator;

	/// <summary>
	/// 控制角色是否继续更新（此时在开面板，相当于是暂停）
	/// </summary>
	private bool characterLock = false;

	[SerializeField] private Camera camera;
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

		// 隐藏鼠标
		Cursor.lockState = CursorLockMode.Locked;
	}

	/// <summary>
	/// 设置状态
	/// </summary>
	/// <param name="eventData">开关</param>
	void SetCharacterLockState( object eventData )
	{
		bool isLock = (bool)eventData;
		characterLock = isLock;

		Debug.Log("SetCharacterLockState" + characterLock);
	}

	void Update( )
	{
		if ( characterLock == false )
		{
			CameraControl( );
			PlayerMovementControl( );
		}
		else
		{
			// 鼠标解锁
			Cursor.lockState = CursorLockMode.None;
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

		camera.transform.localRotation = Quaternion.Euler(-mouseLook.y, mouseLook.x, 0);
	}

	// 控制角色移动
	private void PlayerMovementControl( )
	{
		CurrentSpeed = WalkSpeed;
		if ( characterController.isGrounded )
		{
			var tmp_Horizontal = Input.GetAxis("Horizontal");
			var tmp_Vertical = Input.GetAxis("Vertical");
			movementDirection = camera.transform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
			movementDirection.y = 0;
			// TODO 奔跑 + 下蹲
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

		characterAnimator.SetFloat("Velocity", characterController.velocity.magnitude, 0.25f, Time.deltaTime);
	}

	void LateUpdate( )
	{
		if ( characterLock == false )
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}