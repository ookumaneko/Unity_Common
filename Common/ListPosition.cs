using System;

/// <summary>
/// リストの長さと現在のインデックスを管理するクラス
/// </summary>
class ListPosition
{
    /// <summary>
    /// リストの数
    /// </summary>
    protected int m_count;

    /// <summary>
    /// 表示出来る数
    /// </summary>
    protected int m_height;

    /// <summary>
    /// 表示される物の中で一番上のインデックス
    /// </summary>
    protected int m_top;

    /// <summary>
    /// 表示される物の中で一番下のインデックス
    /// </summary>
    protected int m_end;

    /// <summary>
    /// 現在選択している位置
    /// </summary>
    /// <value>
    /// インデックス
    /// </value>
    public int Index { get; protected set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name='count'>
    /// リストの数
    /// </param>
    /// <param name='height'>
    /// リストの表示数
    /// </param>
    /// <param name='index'>
    /// 選択しているインデックス
    /// </param>
    public ListPosition()
    {
        m_count = 0;
        m_height = 0;
        Index = 0;
        m_top = 0;
        m_end = 0;
    }

    /// <summary>
    /// リストの数を獲得、設定する
    /// </summary>
    /// <value>
    /// リスト内の数
    /// </value>
    public int Count
    {
        get { return m_count; }
        set
        {
            m_count = Math.Max(0, value);
            Select(Index, false);
        }
    }

    /// <summary>
    /// リストの表示出来る数
    /// </summary>
    /// <value>
    /// 表示出来る数
    /// </value>
    public int Height
    {
        get { return ((m_height > 0) ? m_height : m_count); } //return m_height; }
        set
        {
            m_height = Math.Max(0, value);
            Reform();
        }
    }

    public int Offset
    {
        get { return Math.Max(0, Index - m_top); }
    }

    public int Top
    {
        get { return m_top; }
    }

    /// <summary>
    /// 設定する
    /// </summary>
    private void Reform()
    {
        //int top = Top;
        int height = Height;

        if (Index < m_top)
        {
            m_top = Math.Max(0, Index);
        }

        m_end = Math.Min(m_top + height, m_count);
        if (Index + 1 >= m_end)
        {
            m_end = Index + 1; //Math.Min( Index + 1, m_count );
            if (m_end > m_count)
            {
                m_end = m_count;
            }
            // ...test
            //				else if ( Index + m_height > m_end )
            //				{
            //					m_end = Math.Min( m_top + m_height, m_count );
            //				}
        }

        m_top = Math.Max(0, m_end - height);
        //if ( m_top < Index )
        //{
        //    m_top = Index;	
        //}
    }

    /// <summary>
    /// 指定したインデックスに移動する
    /// </summary>
    /// <param name='index'>
    /// 移動先のインデックス
    /// </param>
    public void Select(int index, bool isLoop = true)
    {
        if (m_count > 0)
        {
            if (isLoop)
            {
                this.Index = (Clamp(index, -1, m_count) + m_count) % m_count;
            }
            else
            {
                this.Index = Clamp(index, 0, m_count - 1);
            }

            Reform();
        }
        else
        {
            Index = 0;
            m_top = 0;
            m_end = 0;
        }
    }

    // ライブラリが変わっても使えるようにここで定義(後Genericで作るの面倒)
    private int Clamp(int value, int min, int max)
    {
        value = ((value > max) ? max : value);
        value = ((value < min) ? min : value);
        return value;
    }

    /// <summary>
    /// 位置を指定分次(プラス)に移動する
    /// </summary>
    /// <param name='steps'>
    /// 移動数
    /// </param>
    /// <param name='isLoop'>
    /// 一番下まで行った時に上に戻って続けるか?
    /// </param>
    public void Next(int steps = 1, bool isLoop = true)
    {
        Select(Index + steps, isLoop && (Index >= (m_count - 1)));
    }

    /// <summary>
    /// 位置を指定分前(マイナス)に移動する
    /// </summary>
    /// <param name='steps'>
    /// 移動数
    /// </param>
    /// <param name='isLoop'>
    /// 一番上まで行った時に下に移動して続けるか?
    /// </param>
    public void Back(int steps = 1, bool isLoop = true)
    {
        Select(Index - steps, isLoop && (Index == 0));
    }

    /// <summary>
    /// Scroll the specified step.
    /// </summary>
    /// <param name='step'>
    /// Step.
    /// </param>
    public void Scroll(int step)
    {
        if (m_count <= 0)
        {
            return;
        }

        Index = Clamp(Index + step, 0, m_count - 1);

        if (step < 0)
        {
            step = Math.Max(m_top + step, 0) - m_top;
        }
        else if (step > 0)
        {
            step = Math.Min(m_end + step, m_count) - m_end;
        }

        m_top = Clamp(m_top + step, Index, m_count); //m_top += step; // 
        m_end = Clamp(m_end + step, m_top, m_count);  //+= step;
    }

    /// <summary>
    /// リストの表示範囲を獲得する
    /// </summary>
    /// <param name='start'>
    /// 表示の開始位置(インデックス)
    /// </param>
    /// <param name='end'>
    /// 表示の最後の位置(インデックス)
    /// </param>
    public void GetRange(ref int start, ref int end)
    {
        start = m_top;
        end = m_end;
    }

    // TODO?: Scroll
    //void Scroll()
}

