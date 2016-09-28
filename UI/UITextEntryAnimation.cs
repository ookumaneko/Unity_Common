using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UIのテキストをアニメーションするクラス
/// </summary>
public class UITextEntryAnimation
    : UIEntryAnimation<UnityEngine.UI.Text>
{
    [SerializeField]
    bool m_isChangeChildColour = false;

    protected override void UpdateColour()
    {
        m_target.color = m_colourTweener.CurrentValue;
        if (m_isChangeChildColour)
        {
            int length = m_childUIElements.Length;
            for (int i = 0; i < length; ++i)
            {
                m_childUIElements[i].color = m_colourTweener.CurrentValue;
            }
        }

        if (!m_colourTweener.IsActive && m_onEntryEnd != null)
        {
            m_onEntryEnd.Invoke();
            if (m_isRemoveEventOnEnd)
            {
                ClearEvents();
            }
        }
    }
}
