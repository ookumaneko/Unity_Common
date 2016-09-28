using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIImageEntryAnimation 
	: UIEntryAnimation<UnityEngine.UI.Image>
{
    protected override Color Colour
    {
        get
        {
            return m_target.color;
        }
    }

    protected override void UpdateColour()
    {
        m_target.color = m_colourTweener.CurrentValue;

        int length = m_childUIElements.Length;
        for (int i = 0; i < length; ++i)
        {
            m_childUIElements[i].color = m_colourTweener.CurrentValue;
        }
    }
}
