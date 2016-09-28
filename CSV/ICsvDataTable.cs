using UnityEngine;
using System.Collections.Generic;

public abstract class ICsvDataTable<T>
{
    protected List<T> m_data;

    public ICsvDataTable()
    {
        m_data = new List<T>();
    }

    public int Count { get { return m_data.Count; } }

    public T GetData(int index)
    {
        return m_data[index];
    }

    public void Clear()
    {
        m_data.Clear();
    }

    public abstract string GetFileName();
    public abstract void Load(string path);
}
