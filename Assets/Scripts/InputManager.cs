using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void MouseClickEventHandler( Vector3 position );
	public delegate void MouseMoveEventHandler( Vector3 position );
	public delegate void SpaceKeyDownHandler( );
	public delegate void EscapeKeyDownHandler( );
	public delegate void ShiftKeyDownHandler( );

	public static event MouseClickEventHandler OnMouseClick;
	public static event MouseMoveEventHandler OnMouseMove;
	public static event MouseMoveEventHandler OnMouseMoveDelta;
	public static event SpaceKeyDownHandler OnSpaceKeyDown;
	public static event EscapeKeyDownHandler OnEscapeKeyDown;
	public static event ShiftKeyDownHandler OnShiftKeyDown;

	private Vector3 lastMousePosition;
	private Vector3 moveDelta;

	void Start( )
	{
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (OnMouseClick != null)
            {
                OnMouseClick?.Invoke(mousePosition);
            }
        }

        if (mousePosition != lastMousePosition)
        {
            if (OnMouseMove != null)
            {
                OnMouseMove?.Invoke(mousePosition);
            }

            if (OnMouseMoveDelta != null)
            {
                moveDelta = mousePosition - lastMousePosition;
                OnMouseMoveDelta?.Invoke(moveDelta);
            }
        }

        // 监听键盘输入事件
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Input.GetKeyDown(KeyCode.Space)");
            OnSpaceKeyDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapeKeyDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            OnShiftKeyDown?.Invoke();
        }

        lastMousePosition = mousePosition;
    }
}