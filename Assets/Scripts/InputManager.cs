using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void MouseClickEventHandler(Vector3 position);
    public delegate void MouseMoveEventHandler(Vector3 position);

    public static event MouseClickEventHandler OnMouseClick;
    public static event MouseMoveEventHandler OnMouseMove;
    public static event MouseMoveEventHandler OnMouseMoveDelta;

    private Vector3 lastMousePosition;
    private Vector3 moveDelta;

    void Start()
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
                OnMouseClick(mousePosition);
            }
        }

        if (mousePosition != lastMousePosition)
        {
            if (OnMouseMove != null)
            {
                OnMouseMove(mousePosition);
            }

            if (OnMouseMoveDelta != null)
            {
                moveDelta = mousePosition - lastMousePosition;
                OnMouseMoveDelta(moveDelta);
            }
        }

        lastMousePosition = mousePosition;
    }
}