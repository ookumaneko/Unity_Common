using System;
using System.Collections.Generic;
using System.Diagnostics;

public abstract class Singleton<T> where T : class, new()
{
    private static T m_instance;

    protected Singleton()
    {
        // make sure we only have one instance
        Debug.Assert( m_instance == null, string.Format("Singleton of type {0} already instantiated.", typeof(T))) ;
        m_instance = this as T;

        // validate that the cast worked
        Debug.Assert( m_instance != null, string.Format( "Singleton of type {0} failed to be instantiated.",GetType())) ;
    }

    public static T Instance
    {
        get { return m_instance ?? (m_instance = new T()); }
		protected set { m_instance = value; }
    }

    public virtual void Shutdown()
    {
        m_instance = null;
    }
}
