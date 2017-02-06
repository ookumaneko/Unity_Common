using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

public static class DefineCreatorUtility
{
    public enum FileDeclearType
    {
        Class,
        Enum
    }

    /// <summary>
    /// 無効な文字を管理する配列
    /// </summary>
    public static readonly string[] INVALID_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

    public static readonly string ON_COMPLETE_MESSAGE = "作成が完了しました";

    public const string ITEM_NAME = "Tools/Create/";  // コマンド名
    public const string PATH = "Assets/Scripts/Defines/";      // ファイルパス

    public static readonly string SUMMARY = "/// ";
    public static readonly string START_SUMMARY = SUMMARY + "<summary>";
    public static readonly string END_SUMMARY = SUMMARY + "</summary>";
    public static readonly string CLASS_DECLARATION = "public static class {0}";
    public static readonly string ENUM_DECLARATION = "public enum {0}";
    public static readonly string OPEN_BRACE = "{";
    public static readonly string CLOSE_BRACE = "}";

    public static void ShowCompleteDialog(string fileName)
    {
        EditorUtility.DisplayDialog(fileName, ON_COMPLETE_MESSAGE, "OK");
    }

    /// <summary>
    /// 無効な文字を削除します
    /// </summary>
    public static string RemoveInvalidChars(string text)
    {
        Array.ForEach(INVALID_CHARS, c => text = text.Replace(c, string.Empty));
        return text;
    }

    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    public static void AppendDeclarationString(FileDeclearType type, StringBuilder builder, string fileNameWithoutExtension, string description)
    {
        builder.AppendLine(START_SUMMARY);
        builder.AppendLine(SUMMARY + description);
        builder.AppendLine(END_SUMMARY);
        AppendFileType(type, builder, fileNameWithoutExtension);
        builder.AppendLine(OPEN_BRACE);
    }

    private static void AppendFileType(FileDeclearType type, StringBuilder builder, string name)
    {
        if ( type == FileDeclearType.Class ) builder.AppendFormat(CLASS_DECLARATION, name).AppendLine();
        else if (type == FileDeclearType.Enum) builder.AppendFormat(ENUM_DECLARATION, name).AppendLine();
    }

    public static void AppendEndDeclarationString(StringBuilder builder)
    {
        builder.AppendLine(CLOSE_BRACE);
    }

    public static void CreateFile(string path, StringBuilder builder)
    {
        var directoryName = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }
}
