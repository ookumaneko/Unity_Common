using UnityEngine;
using System.Collections.Generic;

public class BaseFadeLoopText<T>
    : MonoBehaviour
{
    [SerializeField]
    protected float m_fadeDuration = 0.0f;

    [SerializeField]
    protected float m_startAlpha = 1.0f;

    [SerializeField]
    protected float m_targetAlpha = 0.0f;

    [SerializeField]
    protected bool m_isStartOnLoad = false;

    //[SerializeField]
    //AnimationCurve m_fadeCurve = null;

    protected T m_text;
    protected Tweener<Color> m_colourTweener;
    protected bool m_isEnabled = false;

    void Start()
    {
        if (m_isStartOnLoad)
        {
            StartLoop();
        }
        else
        {
            Setup();
        }
    }

    private void Setup()
    {
        SetupTextComponent();

        if (m_colourTweener == null)
        {
            m_colourTweener = new Tweener<Color>(Color.Lerp);
        }
    }

    protected virtual void SetupTextComponent()
    {
    }

    void Update()
    {
        if (!m_isEnabled)
        {
            return;
        }

        m_colourTweener.Update(Time.deltaTime);
        UpdateTextColour();
    }

    public void StartLoop()
    {
        m_isEnabled = true;
        Setup();
        m_colourTweener.Start(new Color(1.0f, 1.0f, 1.0f, m_startAlpha), new Color(1.0f, 1.0f, 1.0f, m_targetAlpha), m_fadeDuration, OnFadeEnd);
        UpdateTextColour();
    }

    protected virtual void UpdateTextColour()
    {
        //m_text.color = m_colourTweener.CurrentValue;
    }

    private void OnFadeEnd()
    {
        m_colourTweener.StartReverse(OnFadeEnd);
        UpdateTextColour();
    }
}
