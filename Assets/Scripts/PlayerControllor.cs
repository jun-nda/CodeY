using System;
using UnityEngine;
using Common.UIScript;

public class PlayerControllor : MonoBehaviour
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
		EventManager.AddListener<CaharacterPause>(SetCharacterLockState);

		/// 武器收起动画结束事件回调
		EventManager.AddListener<WeaponPickDownFinished>(PickDownWeaponFinished);

		/// 换枪事件
		EventManager.AddListener<ChangeWeapon>(ChangeWeapon);

		characterController = GetComponent<CharacterController>( );

	}

	public void Init( )
	{
		InputManager.OnMouseMoveDelta += HandleMouseMove;
		InputManager.OnSpaceKeyDown += HandleSpaceKeyDown;

		isInited = true;

		// TODO 目前是写死的
		LoadWeapon( "Prefabs/Weapon1" );
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

	void Update( )
	{
		if ( characterLock == false && isInited == true)
		{
			CameraControl( );
			PlayerMovementControl( );
		}

		// 测试换枪
		 if (Input.GetKeyDown(KeyCode.K))
		{
			Debug.Log("Input.GetKeyDown(KeyCode.K)");
			ChangeWeapon( new ChangeWeapon("Prefabs/Pistol1") );
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

		//Debug.Log("characterAnimator" + characterAnimator);
		//Debug.Log("UpdateMoveMent   " + forwardDirection.magnitude + " tmp_Movement = " + tmp_Movement);
		if ( characterAnimator != null )
		{
			characterAnimator.SetFloat("Velocity", forwardDirection.magnitude * CurrentSpeed * 50, 0.25f, Time.deltaTime);
			characterAnimator.SetFloat("shifting", - rightDirection.magnitude * tmp_Horizontal, 0.25f, Time.deltaTime);
		}
		else
		{
			characterAnimator = GetComponentInChildren<Animator>( );
		}
	}

	/// <summary>
	/// 加载枪械，重设Weapon以及characterAnimator 参数是枪械的路径
	/// </summary>
	public void LoadWeapon( string str )
	{
		characterAnimator = null;
        Weapon = ResManager.InstantiateGameObjectSync(str);
        Weapon.transform.SetParent(playerCamera.transform);
		Weapon.transform.localScale = new Vector3(1, 1, 1);
		
		Debug.Log("1111111LoadWeapon1111" + characterAnimator);
		characterAnimator = GetComponentInChildren<Animator>( );
		Debug.Log("==========LoadWeapon=============" + characterAnimator);
	}

	/// <summary>
	/// 换枪，没有武器就拿默认的，有武器先执行PickDown
	/// </summary>
	public void ChangeWeapon( ChangeWeapon eventData )
	{
		string weaponName = eventData.WeaponNmae;
		// 当前没有武器
		if ( Weapon == null )
		{
			LoadWeapon( weaponName );
			NextWeapon = null;
		}
		else
		{
			NextWeapon = weaponName;
			characterAnimator.Play("PickDown");
		}
	}

	/// <summary>
	/// 收枪结束，要是有要拿的下一把枪就拿
	/// </summary>
	public void PickDownWeaponFinished( WeaponPickDownFinished EventData )
	{
		if ( Weapon != null )
		{
			GameObject.Destroy(Weapon);
			characterAnimator = null;
		}

		if ( NextWeapon != null )
		{
			LoadWeapon( NextWeapon );
			NextWeapon = null;
		}
	}
}