using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Horror
{	
	public class XmlTable<T> where T : class, new()
	{
        /// <summary>
        /// XML�t�@�C���̊g���q
        /// </summary>
		public const string _FILE_EXTENSION = ".xml";
		
		protected T[] m_data;
		
		public XmlTable()
		{
			m_data = null;
		}
		
        /// <summary>
        /// �f�[�^�̐�
        /// </summary>
		public int Size
		{
			get { return m_data.Length; }
		}
		
        /// <summary>
        /// xml�t�@�C���̓ǂݍ���
        /// </summary>
        /// <param name="filepath">xml�t�@�C���ւ̃p�X</param>
        /// <param name="extention">�t�@�C���̊g���q</param>
		public virtual void Load( string filepath, string extention = _FILE_EXTENSION )
		{
            XmlSerializer serializer = new XmlSerializer( typeof( T[] ) );
            System.IO.FileStream fileStream = System.IO.File.OpenRead( filepath  + extention );
            m_data = ( serializer.Deserialize( fileStream ) as T[] );	
            fileStream.Close();
		}
		
        /// <summary>
        /// �ǂݍ��񂾃f�[�^���l������
        /// </summary>
        /// <param name="index">���ڂ̃f�[�^�������Ă�����</param>
        /// <returns></returns>
		public T GetData(int index)
		{
			if ( m_data == null )
			{
				return null;
			}	
			
			int length = m_data.Length;
			if ( index < 0 || index >= length )
			{
				return null;	
			}
			
			return m_data[ index ];
		}

        /// <summary>
        /// �ێ����Ă���f�[�^��j������
        /// </summary>
        public void Clear()
        {
            int size = m_data.Length;
            for (int i = 0; i < size; ++i)
            {
                m_data[i] = null;
            }

            m_data = null;
        }
	}
}

