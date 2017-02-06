using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class TagDefineCreator
{
    private const string ITEM_NAME = DefineCreatorUtility.ITEM_NAME + "Tags";  // コマンド名
    private const string PATH = DefineCreatorUtility.PATH + "TagDefine.cs";    // ファイルパス

    private static readonly string FILENAME = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
    private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)

    /// <summary>
    /// タグ名を定数で管理するクラスを作成します
    /// </summary>
    [MenuItem(ITEM_NAME)]
    public static void Create()
    {
        if (!CanCreate())
        {
            return;
        }

        CreateScript();
        DefineCreatorUtility.ShowCompleteDialog(FILENAME);
    }

    /// <summary>
    /// スクリプトを作成します
    /// </summary>
    public static void CreateScript()
    {
        var builder = new StringBuilder();

        DefineCreatorUtility.AppendDeclarationString( DefineCreatorUtility.FileDeclearType.Class, builder, 
                                                           FILENAME_WITHOUT_EXTENSION, "タグ名を定数で管理するクラス"
                                                           );
        foreach (var n in InternalEditorUtility.tags.
                Select(c => new { var = DefineCreatorUtility.RemoveInvalidChars(c), val = c })
                )
        {
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", n.var, n.val).AppendLine();
        }

        DefineCreatorUtility.AppendEndDeclarationString(builder);
        DefineCreatorUtility.CreateFile( PATH, builder );
    }

    /// <summary>
    /// タグ名を定数で管理するクラスを作成できるかどうかを取得します
    /// </summary>
    [MenuItem(ITEM_NAME, true)]
    public static bool CanCreate()
    {
        return DefineCreatorUtility.CanCreate();
    }
}
