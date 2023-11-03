using UnityEngine;
using System;
using UnityEngine.UI;

public class SimpleSlider : MonoBehaviour
{
    private float currentNum = 0;
    private float maxNum = 0;    
    private float minNum = 0;        
    private float delta = 0;          
    public Action<float> callBack;
    public Slider slider;

    public void Awake()
    {
        slider.onValueChanged.AddListener(onSliderValueChanged);
    }

    public void SetData ( float _currentNum , float _maxNum , float _minNum, float _delta, Action<float> _callBack )
    {
        currentNum = _currentNum;
        maxNum = _maxNum;
        minNum = _minNum;
        slider.maxValue = maxNum;
        slider.minValue = minNum;

        delta = _delta;
        callBack = _callBack;
        slider.value = currentNum;

    }

    public void SetData ( int _currentNum , int _maxNum , int _minNum, int _delta)
    {
        currentNum = _currentNum;
        maxNum = _maxNum;
        minNum = _minNum;
        delta = _delta;

        slider.maxValue = maxNum;
        slider.minValue = minNum;
        slider.value = currentNum;
    }

    public void onSliderValueChanged (float value)
    {
        var num = value;
        if (num != currentNum)
        {
            currentNum = num;
            callBack?.Invoke( num );
        }
    }
    
    public void OnPlusBtnClick ( )
    {
        if ( currentNum + delta < maxNum )
        {
            slider.value = currentNum + delta;
        }
        else
        {
            slider.value = 1;
        }
    }
    
    public void OnSubBtnClick ( )
    {
        if ( currentNum - delta > minNum)
        {
            slider.value = currentNum - delta;
        }
        else
        {
            slider.value = 0;
        }
    }

    public float GetSliderValue ( )
    {
        return slider.value;
    }

    public void OnDestroy ( )
    {
        callBack = null;
        slider.onValueChanged.RemoveAllListeners();
    }
}