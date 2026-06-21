// Developer tool: regenerate the SDF when PanelCharacterSet is changed.
// The SDF is committed to the repository, so this file is normally not needed.

using TMPro;
using UnityEditor;
using UnityEngine;

namespace StudioPeipeiko.VRClarity.Editor
{
    internal static class VRClarityFontAssetGenerator
    {
        private const string FontAssetPath = "Packages/net.vrclarity.sdk/Runtime/Assets/Fonts/NotoSansJP-Regular SDF.asset";
        private const string FontOtfPath   = "Packages/net.vrclarity.sdk/Editor/Fonts/NotoSansJP-Regular.otf";
        private const string FontTtfPath   = "Packages/net.vrclarity.sdk/Editor/Fonts/NotoSansJP-Regular.ttf";

        // Pre-bake only the characters actually used in the panel to keep the atlas size minimal.
        // Keep this in sync with the text content in VRClarityNoticePanelCreator.
        private const string PanelCharacterSet =
            " ():./CDIKNRSUVacdehilmnprstvyβ、。（）・ーいこさしすせたてなのはまめをれんどイエクタツデリルワド分析導入匿名統計取得等送信滞在時間訪問回数個人特定";

        [MenuItem("Tools/VRClarity/(dev) Regenerate Font Asset", false, 100)]
        private static void RegenerateFontAsset()
        {
            AssetDatabase.DeleteAsset(FontAssetPath);

            var font = LoadSourceFont();
            if (font == null) return;

            var dir = System.IO.Path.GetDirectoryName(FontAssetPath);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
                AssetDatabase.Refresh();
            }

            var fontAsset = TMP_FontAsset.CreateFontAsset(font);
            if (fontAsset == null)
            {
                Debug.LogError("[VRClarity] TMP_FontAsset の生成に失敗しました。");
                return;
            }
            fontAsset.name = "NotoSansJP-Regular SDF";

            fontAsset.TryAddCharacters(PanelCharacterSet, out string missing);
            if (!string.IsNullOrEmpty(missing))
                Debug.LogWarning($"[VRClarity] フォントに含まれない文字があります: {missing}");

            AssetDatabase.CreateAsset(fontAsset, FontAssetPath);
            foreach (var tex in fontAsset.atlasTextures)
            {
                if (tex == null) continue;
                tex.name = fontAsset.name + " Atlas";
                AssetDatabase.AddObjectToAsset(tex, fontAsset);
            }
            if (fontAsset.material != null)
            {
                fontAsset.material.name = fontAsset.name + " Atlas Material";
                AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);
            }
            EditorUtility.SetDirty(fontAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[VRClarity] Font Asset を生成しました: {FontAssetPath}");
        }

        private static Font LoadSourceFont()
        {
            string path = System.IO.File.Exists(FontOtfPath) ? FontOtfPath
                        : System.IO.File.Exists(FontTtfPath) ? FontTtfPath
                        : null;
            if (path == null)
            {
                Debug.LogError("[VRClarity] フォントファイルが見つかりません。\n" +
                    $"以下のいずれかに配置してください:\n  {FontOtfPath}\n  {FontTtfPath}");
                return null;
            }
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            var font = AssetDatabase.LoadAssetAtPath<Font>(path);
            if (font == null)
                Debug.LogError($"[VRClarity] フォントの読み込みに失敗しました: {path}");
            return font;
        }
    }
}
