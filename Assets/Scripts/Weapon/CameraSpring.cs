using UnityEngine;
using GameData;
using Common.UIScript;

public class CameraSpring : MonoBehaviour
{
	private Vector3 Values;
	private Vector2 MinRecoilRange = new(-3.0f, -5.0f);
	private Vector2 MaxRecoilRange = new(3.0f, 1.0f);

	private Vector3 cameraRotation;

	private float Frequence = 25;
	private float Damp = 15;
	private Vector3 dampVaules;
	public float RecoilFadeOutTime = 0.3f;
	public AnimationCurve RecoilCurve;
	public Vector2 RecoilRange;

	private float currentRecoilTime;
	private Vector2 currentRecoil;
	private bool isLock;

	void Start()
	{
		/// 注册开面板事件，设置characterLock参数，当设置为false时，update函数不再运行，将鼠标释放出来
		EventManager.AddListener<CaharacterPause>(SetCharacterLockState);

	}

	/// <summary>
	/// TODO 将后坐力参数传入 MinRecoilRange MaxRecoilRange
	/// </summary>
	public void SetData()
	{

	}

	public void SetCharacterLockState(CaharacterPause eventData)
	{
		isLock = eventData.Value;
	}


	public void UpdateSpring(float _deltaTime, Vector3 _target)
	{
		Values -= _deltaTime * Frequence * dampVaules;
		dampVaules = Vector3.Lerp(dampVaules, Values - _target, Damp * _deltaTime);
	}

	void Update()
	{
		if (isLock == false)
		{
			CameraControl( );

			UpdateSpring(Time.deltaTime, Vector3.zero);
			transform.localRotation = Quaternion.Slerp(transform.localRotation,
				transform.localRotation * Quaternion.Euler(Values), Time.deltaTime * 10);
		}
	}

	private void CameraControl()
	{
		float smoothing = SettingManager.Inst.GetSmoothing( );

		var tmp_MouseX = Input.GetAxis("Mouse X");
		var tmp_MouseY = Input.GetAxis("Mouse Y");

		CalculateRecoilOffset( );

		cameraRotation.y += tmp_MouseX * smoothing;
		cameraRotation.x -= tmp_MouseY * smoothing;

		Debug.Log("currentRecoil = " + currentRecoil);
		cameraRotation.y += currentRecoil.y;
		cameraRotation.x -= currentRecoil.x;

		cameraRotation.x = Mathf.Clamp(cameraRotation.x, -65, 65);
		transform.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
	}

	private void CalculateRecoilOffset()
	{
		currentRecoilTime += Time.deltaTime;
		float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
		float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
		Debug.Log("tmp_RecoilValue = " + tmp_RecoilValue);
		currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
	}

	public void Fire()
	{
		Values = new Vector3(
			UnityEngine.Random.Range(MinRecoilRange.y, MaxRecoilRange.y),
			UnityEngine.Random.Range(MinRecoilRange.x, MaxRecoilRange.x),
			0
			);

		RecoilRange.y = Values.y / 10;
		currentRecoil += RecoilRange;
		currentRecoilTime = 0;
	}
}