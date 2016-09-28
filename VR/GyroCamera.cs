using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GyroCamera 
	: MonoBehaviour 
{
    [SerializeField]
    bool m_isDisableAfterStart = true;

    Transform m_transform;
    readonly Quaternion _BASE_ROTATION = Quaternion.Euler(90, 0, 0);//Quaternion.identity; //Quaternion.Euler(90, 0, 0);
    bool m_isSupported;

	void Start () 
	{
        if (UnityEngine.VR.VRSettings.enabled)
        {
            this.enabled = false;
            return;
        }

        m_transform = transform;
        m_isSupported = SystemInfo.supportsGyroscope;
        if (m_isSupported)
        {
            Input.gyro.enabled = true;
        }

        if (m_isDisableAfterStart)
        {
            this.enabled = false;
        }
	}
    
    void Update()
    {
        if (!m_isSupported)
        {
            return;
        }

        Quaternion gyro = Input.gyro.attitude;
        m_transform.localRotation = _BASE_ROTATION * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));
    }
}
