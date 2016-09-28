using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIFadeLoopText 
	: BaseFadeLoopText<UnityEngine.UI.Text>
{

    protected override void SetupTextComponent()
    {
        m_text = GetComponent<UnityEngine.UI.Text>();
    }

    protected override void UpdateTextColour()
    {
        m_text.color = m_colourTweener.CurrentValue;
    }
}
