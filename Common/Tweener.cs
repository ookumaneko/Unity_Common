using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TweenType
{
    Liner,
    Ease,
    Max
}

public class Tweener<T>
{
    public delegate T LerpFunc<TLerp>(T start, T end, float amount);
    public delegate void TweenEndFunc();

    LerpFunc<T> m_lerpFunction;
    TweenEndFunc m_tweenEndFunction;

    public T CurrentValue { get; private set; }
    T m_start;
    T m_target;
    float m_duration;
    float m_timer;
    TweenType m_mode;
    public bool IsActive { get; private set; }

    public T StartValue { get { return m_start; } }
    public T TargetValue { get { return m_target; } }

    public float Duration
    { 
        get { return m_duration; }
        set { m_duration = Mathf.Clamp(value, 0.0f, float.MaxValue); }
    }

    public float RemainingTime { get { return Mathf.Max(0.0f, (m_duration - m_timer) ); } }

    public Tweener(LerpFunc<T> lerpFunction)
    {
        m_tweenEndFunction = null;
        m_lerpFunction = lerpFunction;
        CurrentValue = default(T);
        m_start = default(T);
        m_target = default(T);
        m_duration = 0.0f;
        m_timer = 0.0f;
        IsActive = false;
    }

    public Tweener(LerpFunc<T> lerpFunction, T initialValue)
    {
        m_tweenEndFunction = null;
        m_lerpFunction = lerpFunction;
        CurrentValue = initialValue;
        m_start = initialValue;
        m_target = initialValue;
        m_duration = 0.0f;
        m_timer = 0.0f;
        IsActive = false;
    }

    public void Start(T start, T end, float duration, TweenEndFunc onTweenEndFunc = null)
    {
        Start(start, end, duration, TweenType.Liner, onTweenEndFunc);
    }

    public void Start(T start, T end, float duration, TweenType mode, TweenEndFunc onTweenEndFunc = null)
    {
        m_start = start;
        m_target = end;
        m_duration = duration;
        m_timer = 0.0f;
        m_mode = mode;
        CurrentValue = m_start;
        m_tweenEndFunction = onTweenEndFunc;

        if (duration == 0.0f)
        {
            IsActive = false;
        }
        else
        {
            IsActive = true;
        }
    }

    public void StartReverse(TweenEndFunc onTweenEndFunc = null)
    {
        Start(m_target, m_start, m_duration, m_mode, onTweenEndFunc);
    }

    public void Update(float delta)
    {
        if (!IsActive)
        {
            return;
        }

        m_timer += delta;
        if (m_timer >= m_duration)
        {
            m_timer = m_duration;
            IsActive = false;
            if (m_tweenEndFunction != null)
            {
                m_tweenEndFunction();
            }
        }

        float percentage = Mathf.Min(m_timer / m_duration, 1.0f);
        ProcessInterpolation(percentage);
        //CurrentValue = m_lerpFunction(m_start, m_target, percentage);
    }

    private void ProcessInterpolation(float percentage)
    {
        if (m_mode == TweenType.Ease)
        {
            CurrentValue = m_lerpFunction(CurrentValue, m_target, percentage);
            return;
        }

        CurrentValue = m_lerpFunction(m_start, m_target, percentage);
    }
}
