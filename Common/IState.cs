using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IState<T>
{
    StateManager<T> m_manager;

    public IState(StateManager<T> owner)
    {
        m_manager = owner;
    }

    protected StateManager<T> Manager
    {
        get { return m_manager; }
    }

    public abstract void Update(float delta);
    public abstract void Initialize();
    public abstract void Shutdown();
    public abstract void OnEnter(T prevState);
    public abstract void OnExit(T nextState);

    public virtual void OnSelect() {}
}