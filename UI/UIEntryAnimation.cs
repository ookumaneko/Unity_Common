using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEntryAnimation<T>
	: MonoBehaviour
{
    public enum MoveType
    {
        MoveFrom,
        MoveTo,
        Max
    }

    [SerializeField]
    protected T m_target;

    [SerializeField]
    Vector2 m_location = Vector2.zero;

    [SerializeField]
    float m_duration = 0.0f;

    [SerializeField]
    float m_waitDuration = 0.0f;

    [SerializeField]
    iTween.EaseType m_easyType = iTween.EaseType.easeInSine;

    [SerializeField]
    MoveType m_moveType;

    [SerializeField]
    bool m_isOnEnable = false;

    [SerializeField]
    bool m_isOnStart = false;

    [SerializeField]
    bool m_isMove = true;

    [SerializeField]
    bool m_isScale = false;

    [SerializeField]
    bool m_isFade = true;

    [SerializeField]
    protected bool m_isRemoveEventOnEnd = false;

    [SerializeField]
    protected bool m_isResetTimerOnFadeOut = false;

    [SerializeField]
    protected bool m_isDestroyOnEnd = false;

    [SerializeField]
    protected bool m_isReinitialze = false;

    [SerializeField]
    iTween.LoopType m_loopType = iTween.LoopType.none;

    [SerializeField]
    protected GameObject m_destroyTarget;

    [SerializeField]
    protected UnityEngine.Events.UnityEvent m_onEntryEnd;

    Timer m_timer;
    protected Vector2 m_startPos;
    protected GameObject m_gameObject;
    protected RectTransform m_transform;
    protected Tweener<Color> m_colourTweener = null;
    protected bool m_isInitialized = false;
    protected UnityEngine.UI.Graphic[] m_childUIElements;
    public bool IsTweenEnd { get; private set; }

    public float TotalDuration { get { return (m_duration + m_waitDuration); } }
    protected virtual Color Colour { get { return Color.white; } }

	void Start()
	{
        Initialize();
        if (m_isOnStart)
        {
            StartCutIn();
        }
	}

    private void Initialize()
    {
        if (m_isInitialized && !m_isReinitialze)
        {
            return;
        }

        IsTweenEnd = false;
        m_gameObject = gameObject;
        m_transform = GetComponent<RectTransform>();
        if (m_colourTweener == null)
        {
            m_startPos = m_transform.localPosition;
            m_colourTweener = new Tweener<Color>(Color.Lerp);
            m_timer = new Timer(m_waitDuration);
        }
        
        if (m_target == null)
        {
            m_target = GetComponent<T>();
        }

        if (m_childUIElements == null)
        {
            FindAllChildUI();
        }

        m_isInitialized = true;
    }

    private void FindAllChildUI()
    {
        m_childUIElements = GetComponentsInChildren<UnityEngine.UI.Graphic>();
    }

    void OnEnable()
    {
        Initialize();
        if (m_isOnEnable)
        {
            StartCutIn();
        }
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (!m_timer.IsTimeUp)
        {
            m_timer.Update(Time.deltaTime);
            if (!m_timer.IsTimeUp)
            {
                return;
            }
        }

        if (m_colourTweener.IsActive)
        {
            m_colourTweener.Update(Time.deltaTime);
            UpdateColour();
        }
    }

    public void StartCutIn()
    {
        if (m_isMove)
        {
            StartMove();
        }
        else if (m_isScale)
        {
            StartScale();
        }

        if (m_isFade)
        {
            Color myColour = Colour;
            Color startColour = new Color(myColour.r, myColour.g, myColour.b, 0.0f);
            Color target = new Color(myColour.r, myColour.g, myColour.b, m_location.x);
            //m_colourTweener.Start(ColorDefine.TRANSPARENT_WHITE, ColorDefine.WHITE, m_duration);
            m_colourTweener.Start(startColour, target, m_duration);
            UpdateColour();
        }
    }

    public void StartFadeOut()
    {
        if (m_isMove)
        {
            StartMove();
        }
        else if (m_isScale)
        {
            StartScale();
        }

        if (m_isFade)
        {
            Color myColour = Colour;
            Color target = new Color(myColour.r, myColour.g, myColour.b, 0.0f ); // m_location.x);
            m_colourTweener.Start(myColour, target, m_duration);
            //m_colourTweener.Start(ColorDefine.WHITE, ColorDefine.TRANSPARENT_WHITE, m_duration);
            UpdateColour();
            if (m_isResetTimerOnFadeOut)
            {
                m_timer.Reset();
            }
        }
    }

    protected virtual void StartMove()
    {
        if (m_moveType == MoveType.MoveFrom)
        {
            MoveFrom();
        }
        else if (m_moveType == MoveType.MoveTo)
        {
            MoveTo();
        }
    }

    protected virtual void MoveFrom()
    {
        m_transform.localPosition = m_startPos;
        iTween.MoveFrom(m_gameObject, iTween.Hash(iTweenParam.X, m_location.x,
                                        iTweenParam.Y, m_location.y,
                                        iTweenParam.Time, m_duration,
                                        iTweenParam.EaseType, m_easyType, //iTween.EaseType.linear,
                                        iTweenParam.IsLocal, true,
                                        iTweenParam.LoopType, m_loopType
                                        )
                                        );
    }

    protected virtual void MoveTo()
    {
        m_transform.localPosition = m_startPos;
        iTween.MoveTo(m_gameObject, iTween.Hash(iTweenParam.X, m_location.x,
                                                iTweenParam.Y, m_location.y,
                                                iTweenParam.Time, m_duration,
                                                iTweenParam.EaseType, m_easyType, //iTween.EaseType.linear,
                                                iTweenParam.IsLocal, true,
                                                iTweenParam.LoopType, m_loopType
                                                )
                                        );
    }

    protected virtual void StartScale()
    {
        if (m_moveType == MoveType.MoveFrom)
        {
            ScaleFrom();
        }
        else if (m_moveType == MoveType.MoveTo)
        {
            ScaleTo();
        }
    }

    protected virtual void ScaleTo()
    {
        iTween.ScaleTo(m_gameObject, iTween.Hash(iTweenParam.X, m_location.x,
                                        iTweenParam.Y, m_location.y,
                                        iTweenParam.Time, m_duration,
                                        iTweenParam.EaseType, m_easyType, //iTween.EaseType.linear,
                                        iTweenParam.IsLocal, true,
                                        iTweenParam.OnComplete, "OnTweenEnd",
                                        iTweenParam.LoopType, m_loopType
                                        )
                        );
    }

    protected virtual void ScaleFrom()
    {
        iTween.ScaleFrom(m_gameObject, iTween.Hash(iTweenParam.X, m_location.x,
                                        iTweenParam.Y, m_location.y,
                                        iTweenParam.Time, m_duration,
                                        iTweenParam.EaseType, m_easyType, //iTween.EaseType.linear,
                                        iTweenParam.IsLocal, true,
                                        iTweenParam.OnComplete, "OnTweenEnd",
                                        iTweenParam.LoopType, m_loopType
                                        )
                        );
    }

    protected virtual void UpdateColour()
    {

    }

    public void SetToMoveTo()
    {
        m_moveType = MoveType.MoveTo;
    }

    public void SetToMoveFrom()
    {
        m_moveType = MoveType.MoveFrom;
    }

    public void StartClose()
    {
        StartClose(null);
    }

    public void StartClose(GameObject destroyTarget)
    {
        IsTweenEnd = false;
        if (m_moveType == MoveType.MoveFrom)
        {
            m_moveType = MoveType.MoveTo;
        }
        else
        {
            m_moveType = MoveType.MoveFrom;
        }

        m_isDestroyOnEnd = true;
        m_onEntryEnd = null;
        if (destroyTarget != null)
        {
            m_destroyTarget = destroyTarget;
        }

        StartCutIn();
    }

    protected virtual void OnTweenEnd()
    {
        IsTweenEnd = true;
        if (m_onEntryEnd != null)
        {
            m_onEntryEnd.Invoke();
            if (m_isRemoveEventOnEnd)
            {
                ClearEvents();
            }
        }

        if (m_isDestroyOnEnd && m_destroyTarget != null)
        {
            Destroy(m_destroyTarget);
        }
    }

    public void ClearEvents()
    {
        if (m_onEntryEnd != null)
        {
            m_onEntryEnd.RemoveAllListeners();
            m_onEntryEnd = null;
        }
    }

    public void SetupTypes(bool isMove, bool isScale, bool isFade)
    {
        m_isMove = isMove;
        m_isScale = isScale;
        m_isFade = isFade;
    }
}
