using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PinchZoomer
    : MonoBehaviour
{
    const int _PINCH_TOUCH_COUNT = 2;

    [SerializeField]
    float m_perspectiveZoomSpeed = 0.5f;

    [SerializeField]
    float m_orthoZoomSpeed = 0.5f;

    [SerializeField]
    float m_minValue = 1.0f;

    [SerializeField]
    float m_maxValue = 80.0f;

    void Update()
    {
        if (Input.touchCount != _PINCH_TOUCH_COUNT)
        {
            return;
        }

        // Store both touches.
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        Camera camera = Camera.main;

        // If the camera is orthographic...
        if (camera.orthographic)
        {
            // ... change the orthographic size based on the change in distance between the touches.
            camera.orthographicSize += deltaMagnitudeDiff * m_orthoZoomSpeed;

            // Make sure the orthographic size never drops below zero.
            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
        }
        else
        {
            // Otherwise change the field of view based on the change in distance between the touches.
            camera.fieldOfView += deltaMagnitudeDiff * m_perspectiveZoomSpeed;

            // Clamp the field of view to make sure it's between 0 and 180.
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, m_minValue, m_maxValue);
        }
    }
}