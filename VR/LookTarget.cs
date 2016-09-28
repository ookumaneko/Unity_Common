using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LookTarget
    : ILookTarget
{
    [SerializeField]
    UnityEngine.UI.Image m_image = null;

    protected override void ResetFill()
    {
        m_image.fillAmount = 0.0f;
    }

    protected override void UpdateFill()
    {
        m_image.fillAmount = m_timer.Rate;
    }
}
