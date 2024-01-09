using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;

    public HexGrid hexGrid;

    private Color activeColor;
    int activeElevation;
    
    void Awake () {
        SelectColor(0);
    }

    void Update () {
        if (Input.GetMouseButton(0)) {
            HandleInput();
        }
    }

    // 点击屏幕，选中的cell变色
    void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            EditCell(hexGrid.GetCell(hit.point));
        }
    }

    void EditCell (HexCell cell) {
        cell.color = activeColor;
        cell.Elevation  = activeElevation;
        // hexGrid.Refresh();
    }
    
    public void SelectColor (int index) {
        activeColor = colors[index];
    }
    
    public void SetElevation (float elevation) {
        Debug.Log(elevation);
        activeElevation = (int)elevation;
    }
}
