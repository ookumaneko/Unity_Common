using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//-------------------------------------------------------------------------
// InputManagerの設定情報を自動生成するクラス
//-------------------------------------------------------------------------
public class KeyDefineCreator 
    : AssetPostprocessor
{
    private const string ITEM_NAME = DefineCreatorUtility.ITEM_NAME + "InputKeys";  // コマンド名
    private const string PATH = DefineCreatorUtility.PATH + "KeyDefine.cs";      // ファイルパス

    private static readonly string FILENAME = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
    private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)

    /// <summary>
    /// 入力キー名を定数で管理するクラスを作成します
    /// </summary>
    [MenuItem(ITEM_NAME)]
    public static void Create()
    {
        if (!CanCreate())
        {
            return;
        }

        CreateScript();
        DefineCreatorUtility.ShowCompleteDialog(FILENAME );
    }

    /// <summary>
    /// スクリプトを作成します
    /// </summary>
    public static void CreateScript()
    {
        var builder = new StringBuilder();

        DefineCreatorUtility.AppendDeclarationString(DefineCreatorUtility.FileDeclearType.Class, builder,
                                                     FILENAME_WITHOUT_EXTENSION, "入力キー名を定数で管理するクラス"
                                                     );
        AppendKeys(builder);
        DefineCreatorUtility.AppendEndDeclarationString(builder);
        DefineCreatorUtility.CreateFile(PATH, builder);
    }

    private static void AppendKeys(StringBuilder builder)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        var axesProperty = serializedObject.FindProperty("m_Axes");
        List<string> nameList = new List<string>();

        int length = axesProperty.arraySize;
        for (int i = 0; i < length; ++i)
        {
            var axisProperty = axesProperty.GetArrayElementAtIndex(i);
            string name = GetChildProperty(axisProperty, "m_Name").stringValue;

            if (nameList.Contains(name))
            {
                continue;
            }

            nameList.Add(name);
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", name.Replace(" ", ""), name).AppendLine();
        }
    }

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    /// <summary>
    /// 管理するクラスを作成できるかどうかを取得します
    /// </summary>
    [MenuItem(ITEM_NAME, true)]
    public static bool CanCreate()
    {
        return DefineCreatorUtility.CanCreate();
    }
}