using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVReader
{
	public const string _EXTENSION = ".csv";
	string m_commentString = "//";
    List<List<string>> m_data = new List<List<string>>();
    TextAsset m_textAsset = null;
    bool m_isReadFromDisc;

	public CSVReader(string comment = "//", bool isReadFromDisc = false)
	{
        m_textAsset = null;
		m_data = new List<List<string>>(10);
		m_commentString = comment;
        m_isReadFromDisc = isReadFromDisc;
	}
		
	public bool Load(string fileName) 
	{			
		m_data.Clear();
		//StreamReader stream = new StreamReader( fileName );        
        //TextAsset datum = Resources.Load<TextAsset>(fileName);
        //StringReader reader = new StringReader(datum.text);

        TextReader reader = CreateTextReader(fileName);

		int counter = 0;
		string line = "";
		while ( ( line = reader.ReadLine()) != null ) 
		{	
			if ( line.Contains( m_commentString ) )
			{
				continue;
			}
				
			string[] fileds = line.Split( ',' );
			m_data.Add( new List<string>() );

			foreach( var field in fileds )
			{
                if (field.Contains(m_commentString) || field == "")
				{
					continue;	
				}
				m_data[ counter ].Add( field );
			}
			counter++;
		}

        Resources.UnloadAsset(m_textAsset);
        m_textAsset = null;
		Resources.UnloadUnusedAssets();
		return true;
	}

    private TextReader CreateTextReader(string fileName)
    {
        if (m_isReadFromDisc)
        {
            return new StreamReader(fileName);
        }

        m_textAsset = Resources.Load<TextAsset>(fileName);
        return new StringReader(m_textAsset.text);
    }

	public void Clear()
	{
		m_data.Clear();	
	}

	public int RowCount
	{
        get { return m_data.Count; }
	}
		
	public int GetColumnCount(int row)
	{
        if (RowCount == 0)
		{
			return 0;
		}
			
		return m_data[ row ].Count;
	}

    public List<string> GetData(int row)
    {
        return m_data[row];
    }

	public string GetString(int row, int col)
	{
		return m_data[ row ][ col ];	
	}

    public bool GetBool(int row, int col)
    {
        string data = GetString(row, col);
        return bool.Parse( data );
    }

	public int GetInt(int row, int col)
	{
		string data = GetString( row, col );
		return int.Parse( data );
	}
		
	public float GetFloat(int row, int col)
	{
		string data = GetString( row, col );
		return float.Parse( data );
	}

    public void SetData(string[,] data)
    {
        int height = data.GetLength(0);
        int width = data.GetLength(1);

        for (int y = 0; y < height; ++y)
        {
            m_data.Add(new List<string>());
            for (int x = 0; x < width; ++x)
            {
                m_data[y].Add(data[y, x]);   
            }
        }
    }
}

