using UnityEngine;
using System.Collections.Generic;

public class UIFadeLoopText3D
    : BaseFadeLoopText<TextMesh>
{
    protected override void SetupTextComponent()
    {
        m_text = GetComponent<TextMesh>();
    }

    protected override void UpdateTextColour()
    {
        m_text.color = m_colourTweener.CurrentValue;
    }
}
