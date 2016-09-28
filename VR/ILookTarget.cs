using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class ILookTarget
    : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected float m_eventStartTime = 0.0f;

    [SerializeField]
    protected UnityEngine.Events.UnityEvent m_lookEvent;

    [SerializeField]
    protected List<GameObject> m_destructTargets = null;

    [SerializeField]
    protected bool m_isDestroySelftOnFull = true;
    
    protected bool m_isPressed;
    protected bool m_isResetFillOnExit = true;
    protected bool m_isFull = false;
    protected Timer m_timer;

    public bool IsTargeted { get { return (m_isPressed || m_isFull); } }
    public bool IsNotTargeted { get { return !IsTargeted; } }

    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        m_isPressed = false;
        m_timer = new Timer(m_eventStartTime);
    }

    void Update()
    {
        if (m_isPressed)
        {
            OnStay();
        }
    }

    public virtual void Destruct()
    {
        int count = m_destructTargets.Count;
        for (int i = 0; i < count; ++i)
        {
            Destroy(m_destructTargets[i]);
        }
        m_destructTargets.Clear();
    }

    public virtual void Enter()
    {
        m_isPressed = true;
        ResetFill();
        m_timer.Reset();
    }

    protected abstract void ResetFill();
    protected abstract void UpdateFill();

    public virtual void Exit()
    {
        if (m_isFull)
        {
            return;
        }

        m_isPressed = false;
        if (m_isResetFillOnExit)
        {
            ResetFill();
        }
        m_timer.Reset();
    }

    public virtual void OnStay()
    {
        m_timer.Update(Time.deltaTime);
        UpdateFill();

        //Debug.Log("Rate = " + m_timer.Rate);
        if (m_timer.IsTimeUp)
        {            
            OnTimeUp();
        }
        //Debug.Log("Stay");
    }

    protected virtual void OnTimeUp()
    {
        if (m_isFull)
        {
            return;
        }

        m_isFull = true;
        m_isPressed = false;
        m_lookEvent.Invoke();
        this.enabled = false;
        DestroyTargets();

        if (m_isDestroySelftOnFull)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void DestroyTargets()
    {
        int count = m_destructTargets.Count;
        for (int i = 0; i < count; ++i)
        {
            Destroy(m_destructTargets[i].gameObject);
        }

        m_destructTargets.Clear();
    }

    //public void OnPointerDown(PointerEventData data)
    //{
    //    m_isPressed = true;
    //}

    //public void OnPointerUp(PointerEventData data)
    //{
    //    m_isPressed = false;
    //}

    public virtual void OnPointerEnter(PointerEventData data)
    {
        //m_image.color = Color.red;
        Enter();
    }

    public virtual void OnPointerExit(PointerEventData data)
    {
        //m_image.color = Color.white;
        Exit();
    }
}
