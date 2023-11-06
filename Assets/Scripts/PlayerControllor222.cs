using System;
using UnityEngine;
using Common.UIScript;
using GameData;

public class PlayerControllor222 : MonoBehaviour
{
	private CharacterController characterController;
	private Animator characterAnimator;
	private GameObject Weapon;

	private bool isInited = false;

	/// <summary>
	/// 切枪时缓存一下下一把枪是啥
	/// </summary>
	private String NextWeapon;

	/// <summary>
	/// 控制角色是否继续更新（此时在开面板，相当于是暂停）初始是暂停
	/// </summary>
	private bool characterLock = true;

	[SerializeField]
	public Camera playerCamera;
	public GameObject PlayerObj;
	public GameObject OtherPlayerObj;

	/// <summary>
	/// 一些配置
	/// </summary>
	private readonly float sensitivity = 5.0f;
	private readonly float gravity = 9.8f;
	private readonly float jumpHeight = 2f;
	private readonly float WalkSpeed = 0.04f;
	private readonly float RunSpeed = 0.08f;

	private Vector3 movementDirection;
	private Vector2 mouseLook;
	private Vector2 smoothV;

	public float CurrentSpeed { get; private set; }

	private Vector3 forwardDirection;
	private Vector3 rightDirection;

	private float tmp_Horizontal;
	private float tmp_Vertical;

	void Start( )
	{
		/// 注册开面板事件，设置characterLock参数，当设置为false时，update函数不再运行，将鼠标释放出来
		EventManager.AddListener<CaharacterPause>(SetCharacterLockState);

		/// 武器收起动画结束事件回调 TODO
		//EventManager.AddListener<WeaponPickDownFinished>(PickDownWeaponFinished);

		/// 换枪事件 TODO
		//EventManager.AddListener<ChangeWeapon>(ChangeWeapon);

		characterController = GetComponent<CharacterController>( );
	}

	public void Init( )
	{
		InputManager.OnMouseMoveDelta += HandleMouseMove;
		InputManager.OnSpaceKeyDown += HandleSpaceKeyDown;

		isInited = true;

		SetCharacterLockState(new CaharacterPause(false));
	}

	/// <summary>
	/// 设置状态 传true是锁住，暂停
	/// </summary>
	public void SetCharacterLockState( CaharacterPause eventData )
	{
		bool isLock = eventData.Value;
		characterLock = isLock;

		if ( characterLock == false )
		{
			if ( characterAnimator != null )
				characterAnimator.enabled = true;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{ 
			if ( characterAnimator != null )
				characterAnimator.enabled = false;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	void Update()
	{
		if (characterLock == false && isInited == true)
		{
			CameraControl( );
			PlayerMovementControl( );
		}
	}

	// 控制相机视角转动
	private void CameraControl( )
	{
		float smoothing = SettingManager.Inst.GetSmoothing();
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
		if (Input.GetKey(KeyCode.LeftShift))
        {
            CurrentSpeed = RunSpeed;
        }
		else
		{
			CurrentSpeed = WalkSpeed;
		}

		if ( characterController.isGrounded )
		{
			tmp_Horizontal = Input.GetAxis("Horizontal");
			tmp_Vertical = Input.GetAxis("Vertical");

			//CameraVec.x = playerCamera.transform.rotation.x;
			//CameraVec.x = playerCamera.transform.rotation.x;
			//Debug.Log("playerCamera " + tmp_Horizontal);
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
		
		DataManager.Inst.SetPlayerSpeed(tmp_Movement.magnitude);
		//Debug.Log("UpdateMoveMent   " + forwardDirection.magnitude + " tmp_Movement = " + tmp_Movement);
		if (characterAnimator != null)
		{
			//Debug.Log("tmp_Movement" + tmp_Movement.magnitude);
			characterAnimator.SetFloat("Velocity", tmp_Movement.magnitude * CurrentSpeed * 2500, 0.25f, Time.deltaTime);
			//characterAnimator.SetFloat("shifting", -rightDirection.magnitude * tmp_Horizontal, 0.25f, Time.deltaTime);
		}
		else
		{
			characterAnimator = GetComponentInChildren<Animator>( );
		}
	}
}