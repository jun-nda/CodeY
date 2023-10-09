using System;
using UnityEngine;

public class PlayerControllor : MonoBehaviour
{
	[SerializeField] private Camera camera;
	public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

	public void Init(InputManager inputManager)
	{
		InputManager.OnMouseMoveDelta += HandleMouseMove;
	}

	void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
        mouseLook += smoothV;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(- mouseLook.y, mouseLook.x, 0);

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void HandleMouseMove(Vector3 vec)
	{
        Debug.Log("HandleMouseMove" + vec);
	}

    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}