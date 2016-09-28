using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFacingBillboard
	: MonoBehaviour 
{
    Transform m_camera;
    Transform m_transform;

	void Start()
	{
        m_camera = Camera.main.transform;
        m_transform = transform;
        UpdateRotation();
	}
	
	void Update()
	{
        UpdateRotation();
	}

    private void UpdateRotation()
    {
        m_transform.LookAt(m_transform.position + m_camera.rotation * Vector3.forward, m_camera.rotation * Vector3.up);
    }
}
