using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraRayCaster
    : MonoBehaviour
{
    [SerializeField]
    bool m_isRayVisible = false;

    [SerializeField]
    Transform m_crosshair = null;

    [SerializeField]
    float m_minimumCrosshairDistance = 1.05f;

    [SerializeField]
    LayerMask m_raycastTarget = 0;

    Camera m_camera;
    Transform m_cameraTransform;
    Transform m_transform;
    ILookTarget m_target = null;
    Vector3 m_originalScale;

    void Start()
    {
        m_transform = transform;
        m_camera = Camera.main;
        m_cameraTransform = m_camera.transform;

        if (m_crosshair != null)
        {
            m_originalScale = m_crosshair.localScale;
        }
    }
	
	void Update() 
    {
        //RayCastUI();
        Raycast3D();
        //UpdateCrosshiar(m_camera.nearClipPlane * m_minimumCrosshairDistance);
	}

    private void Raycast3D()
    {
        Ray ray = m_camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (m_isRayVisible == true)
        {
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
        }

        int mask = ~(1 << m_raycastTarget);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 100000.0f, mask))
        {
            OnNoTarget();
            return;
        }

        GameObject obj = hit.transform.gameObject;
        ILookTarget target = obj.GetComponent<ILookTarget>();
        if (target == null)
        {
            OnNoTarget();
            return;
        }

        if (target != m_target)
        {
            if (m_target != null)
            {
                m_target.Exit();
            }
            target.Enter();
            m_target = target;            
        }
        else
        {
            m_target.OnStay();
        }

        UpdateCrosshiar(hit.distance);
    }

    private void OnNoTarget()
    {
        UpdateCrosshiar(m_camera.nearClipPlane * m_minimumCrosshairDistance);
        if (m_target != null)
        {
            m_target.Exit();            
        }

        m_target = null;
    }

    private void UpdateCrosshiar(float distance)
    {
        if (m_crosshair == null)
        {
            return;
        }
        //if (GameDefine.IsGearVR)
        //{
        //    return;
        //}

        //Set the new cross hair position based on the distance
        m_crosshair.position = m_transform.position + (m_transform.forward * distance);
        m_crosshair.LookAt(m_transform.position);
        m_crosshair.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

        //Scale the cross hair so it's the same size even when it's in the distance.
        //m_transform.localScale = m_originalScale * distance;
        //m_crosshair.localScale = m_originalScale * distance;
    }

    private void RayCastUI()
    {
        var pointer = new PointerEventData(EventSystem.current);

        // convert to a 2D position
        pointer.position = m_camera.WorldToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        if (raycastResults.Count < 0)
        {
            return;
        }

        GameObject obj = FindUITarget(raycastResults);
        if (obj == null)
        {
            return;
        }

        LookTarget target = obj.GetComponent<LookTarget>();
        if (target == null)
        {
            OnNoTarget();
            return;
        }

        if (target != m_target)
        {
            if (m_target != null)
            {
                m_target.Exit();
            }
            target.Enter();
            m_target = target;
        }
        else
        {
            m_target.OnStay();
        }
    }

    private GameObject FindUITarget(List<RaycastResult> rayCastResults)
    {
        int count = rayCastResults.Count;
        for (int i = 0; i < count; ++i)
        {
            if (rayCastResults[i].gameObject.tag == TagDefine.LookTarget)
            {
                return rayCastResults[i].gameObject;
            }
        }

        return null;
    }
}
