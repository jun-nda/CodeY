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
	private readonly float gravity = 9.8f;
	private readonly float jumpHeight = 2f;
	private readonly float WalkSpeed = 0.04f;
	private readonly float RunSpeed = 0.08f;

    private float currentRecoilTime;
    private Vector2 currentRecoil;


	public float CurrentSpeed { get; private set; }

	private Vector3 forwardDirection;
	private Vector3 rightDirection;

	private float tmp_Horizontal;
	private float tmp_Vertical;
	private Vector3 movementDirection;

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
			//CameraControl( );
			PlayerMovementControl( );
		}
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
			movementDirection.Normalize();
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
		
		DataManager.Inst.CaharacterSpeed = tmp_Movement.magnitude;
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