using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CSVWriter
{
    const string _SEPARATOR = ",";

    string m_path;
    string m_currentLine;
    List<string> m_data;

    public CSVWriter(string path)
    {
        m_path = path;
        m_currentLine = "";
        m_data = new List<string>();
    }

    public void Clear()
    {
        m_currentLine = "";
        m_data.Clear();
    }

    public void AddData(string data, bool isEndOfLine)
    {
        if (isEndOfLine)
        {
            m_currentLine += data;
            m_data.Add(m_currentLine);
            m_currentLine = "";
        }
        else
        {
            m_currentLine += data + _SEPARATOR;
        }
    }

    public void Save()
    {
        Save(m_data);
    }

    public void Save(List<string> list)
    {
        if (File.Exists(m_path))
        {
            File.Delete(m_path);
        }

        using (var stream = new System.IO.StreamWriter(m_path))
        {
            int count = m_data.Count;
            for (int ii = 0; ii < count; ++ii)
            {
                stream.WriteLine(m_data[ii]);
            }
        }

        m_data.Clear();
    }
}

