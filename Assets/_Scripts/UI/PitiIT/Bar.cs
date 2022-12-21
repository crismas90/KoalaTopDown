using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [field:SerializeField]
    public int MaxValue { get; private set; }

    [field: SerializeField]
    public int Value { get; private set; }

    [SerializeField]
    private RectTransform _topBar;

    [SerializeField]
    private RectTransform _bottomBar;

    [SerializeField]
    private float _animationSpeed = 10f;

    private float _fullWidth;
    private float TargetWidth => Value * _fullWidth / MaxValue;

    private Coroutine _adjustBarWidthCoroutine;



    private void Start()
    {
        _fullWidth = _topBar.rect.width;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Change(20);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Change(-20);
        }
    }


    private IEnumerator AdjustBarWidth(int amount)
    {
        var suddenChangeBar = amount >= 0 ? _bottomBar : _topBar;
        var slowChangeBar = amount >= 0 ? _topBar : _bottomBar;
        suddenChangeBar.SetWidth(TargetWidth);
        while (Mathf.Abs(suddenChangeBar.rect.width - slowChangeBar.rect.width) > 1f)
        {
            slowChangeBar.SetWidth(Mathf.Lerp(slowChangeBar.rect.width, TargetWidth, Time.deltaTime * _animationSpeed));
            yield return null;
        }
        slowChangeBar.SetWidth(TargetWidth);
    }


    public void Change(int amount)
    {
        Value = Mathf.Clamp(Value + amount, 0, MaxValue);
        if (_adjustBarWidthCoroutine != null)
        {
            StopCoroutine(_adjustBarWidthCoroutine);
        }

        _adjustBarWidthCoroutine = StartCoroutine(AdjustBarWidth(amount));
    }
}
