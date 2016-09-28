using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> 
    : MonoBehaviour where T : MonoBehaviour 
{

    protected const bool IsSearchObjectOnNullInstance = true;
	protected static T m_instance = null;

    public static bool HasInstance { get { return (m_instance != null); } }

	public static T Instance 
    {
		get 
        {
            if (m_instance == null && IsSearchObjectOnNullInstance)
            {
				m_instance = ( FindObjectOfType( typeof(T) ) as T );
			}

			return m_instance;
		}
	}

    public virtual void Shutdown()
    {
        m_instance = null;
    }

    void OnDisable()
    {
        m_instance = null;
    }

	void OnApplicationQuit() 
    {
		m_instance = null;
	}
	
	void OnDestroy()
	{
        if (m_instance == this)
        {
            m_instance = null;
        }
	}

    protected virtual void OnAwake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            InitializeOnAwake();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void InitializeOnAwake()
    {
    }
}

