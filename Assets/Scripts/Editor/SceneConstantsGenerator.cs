using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

// https://www.reddit.com/r/Unity3D/comments/1888oax/i_beg_you_dont_use_the_buildindex_for_loading/
public static class SceneConstantsGenerator
{
    private const string OUTPUT_PATH = "Assets/Scripts/Generated/Scenes.cs";

    [MenuItem("Tools/Generate Scene Constants")]
    public static void Generate()
    {
        var scenes = EditorBuildSettings.scenes;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("// AUTO-GENERATED FILE — do not modify manually");
        sb.AppendLine("public static class Scenes");
        sb.AppendLine("{");

        for (int i = 0; i < scenes.Length; i++)
        {
            string path = scenes[i].path;
            string name = Path.GetFileNameWithoutExtension(path);

            // Garantir que o nome é válido como identificador C#
            string safeName = name.Replace(" ", "_").Replace("-", "_");

            sb.AppendLine($"    public const int {safeName} = {i};");
        }

        sb.AppendLine("}");

        Directory.CreateDirectory(Path.GetDirectoryName(OUTPUT_PATH));
        File.WriteAllText(OUTPUT_PATH, sb.ToString());

        AssetDatabase.Refresh();
        Debug.Log("Scenes.cs regenerado com sucesso!");
    }
}
