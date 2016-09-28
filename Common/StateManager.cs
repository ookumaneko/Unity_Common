using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager<T>
{
    Dictionary<T, IState<T>> m_gameStates = new Dictionary<T, IState<T>>();
    T m_currentState = default(T);
    T m_nextState = default(T);
    bool m_isChangeStateThisFrame = false;
    //bool m_isDestroyCurrent = false;

    public StateManager()
    {
    }

    //public Dictionary<T, IState<T>> GameStates
    //{
    //    get { return m_gameStates; }
    //    set { m_gameStates = value; }
    //}

    public T CurrentState
    {
        get { return m_currentState; }
        set { m_currentState = value; }
    }

    public IState<T> CurrentGameState
    {
        get { return m_gameStates[m_currentState]; }
        set { m_gameStates[m_currentState] = value; }
    }

    public IState<T> GetState(T type)
    {
        return m_gameStates[type];
    }

    public void PushState(T key, IState<T> toAdd)
    {
        AddState(key, toAdd);
        ChangeState(key);
    }

    public void AddState(T key, IState<T> toAdd)
    {
        if (m_gameStates.ContainsKey(key))
        {
            return;
        }

        m_gameStates.Add(key, toAdd);
        if (m_gameStates.Count == 1)
        {
            m_currentState = key;
            toAdd.OnEnter(key);
        }
    }

    public bool RemoveState(T key)
    {
        return m_gameStates.Remove(key);
    }

    public void Update(float delta)
    {
        if (m_isChangeStateThisFrame)
        {
            m_isChangeStateThisFrame = false;
            _ChangeState(m_nextState);
        }

        IState<T> state = null;
        if (m_gameStates.TryGetValue(m_currentState, out state))
        {
            state.Update(delta);
        }
    }

    public void ChangeState(T newState)
    {
        m_isChangeStateThisFrame = true;
        m_nextState = newState;
    }

    private void _ChangeState(T newState)
    {
        T prevState = m_currentState;
        CurrentGameState.OnExit(newState);
        m_currentState = newState;
        CurrentGameState.OnEnter(prevState);
    }

    public void ChangeState(T newState, bool doCallExit)
    {
        if (doCallExit)
        {
            m_isChangeStateThisFrame = true;
        }

        m_nextState = newState;
    }

    public void Clear()
    {
        foreach (KeyValuePair<T, IState<T>> kvp in m_gameStates)
        {
            kvp.Value.Shutdown();
        }
        m_gameStates.Clear();
    }
}
