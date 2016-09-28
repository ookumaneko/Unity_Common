using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Adjusts UI position to 3D world coordinate
/// </summary>
public class UIWorldSpaceAdjuster
	: MonoBehaviour 
{
    [SerializeField]
    public Transform ToFollow;

    [SerializeField]
    Vector2 m_offset;

    [SerializeField]
    RectTransform m_canvasTransform;

    RectTransform m_transform;

    void Start()
    {
        m_transform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (ToFollow == null)
        {
            return;
        }
        
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, ToFollow.position);
        m_transform.anchoredPosition = screenPoint - (m_canvasTransform.sizeDelta / 4.0f) + m_offset;
    }

    public void Setup(Transform toFollow, RectTransform canvasTransform, Vector2 offset)
    {
        this.ToFollow = toFollow;
        this.m_offset = offset;
        m_canvasTransform = canvasTransform;
    }
}
