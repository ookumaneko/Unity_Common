using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer
{
    float m_currentTime;
    float m_duration;

    public Timer(float duration)
    {
        m_currentTime = 0.0f;
        m_duration = duration;
    }

    public bool IsTimeUp
    {
        get { return (m_currentTime >= m_duration); }
    }

    public float RemainingTime
    {
        get { return Mathf.Max(0, m_duration - m_currentTime); }
    }

    public float Duration { get { return m_duration; } }
    public float CurrentTime { get { return m_currentTime; } }
    public float Rate { get { return Mathf.Clamp((m_currentTime / m_duration), 0, 1); } }

    public void SetDuration(float duration, bool isReset = true)
    {
        m_duration = duration;
        if (isReset)
        {
            m_currentTime = 0.0f;
        }
    }

    public void SetToTimeup()
    {
        m_currentTime = m_duration;
    }

    public void Reset()
    {
        m_currentTime = 0.0f;
    }

    public void AddRemainingTime(float amonuntToAdd)
    {
        m_duration += amonuntToAdd;
    }

    public void Update(float delta)
    {
        m_currentTime = Mathf.Clamp(m_currentTime + delta, 0.0f, m_duration);
    }
}
