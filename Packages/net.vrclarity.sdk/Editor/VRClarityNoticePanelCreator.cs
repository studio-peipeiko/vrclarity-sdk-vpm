using StudioPeipeiko.VRClarity.Runtime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace StudioPeipeiko.VRClarity.Editor
{
    /// <summary>Notice Panel color theme (matches the VRClarity dashboard light/dark themes).</summary>
    public enum NoticePanelTheme { Dark, Light }

    /// <summary>
    /// Notice Panel size variants. Standard = full size, Compact = a smaller version with a simplified header,
    /// Minimal = a square badge with the logo, title, "Installed", and URL stacked vertically (no disclosure text).
    /// </summary>
    public enum NoticePanelSize { Standard, Compact, Minimal }

    public static class VRClarityNoticePanelCreator
    {
        private const string LogoDarkPath  = "Packages/net.vrclarity.sdk/Runtime/Assets/logo-dark.png";
        private const string LogoLightPath = "Packages/net.vrclarity.sdk/Runtime/Assets/logo-light.png";
        private const string FontAssetPath = "Packages/net.vrclarity.sdk/Runtime/Assets/Fonts/NotoSansJP-Regular SDF.asset";

        private const float WorldScale = 0.0008f;        // 1 canvas unit = 0.8mm; default down-scale of every panel
        private const float MinimalWorldScale = 0.0005f; // the Minimal badge is rendered smaller (0.5mm per canvas unit)

        // ── Palette (matched to the VRClarity dashboard light/dark themes) ─────

        private struct Palette
        {
            public Color Background;
            public Color Accent;
            public Color Body;
            public Color Subtle;
            public string AccentHex; // injected into the inline <color> tag in the body text
            public string LogoPath;  // theme-specific logo (arrow accents tuned for the background)
            public float  FaceDilate; // glyph thickening; dark-on-light renders thinner, so > 0 for light
        }

        // Dark: panel surface #0C1524 / accent #5B9BF2 / body #EDF1F5 / subtle #6C788A
        private static readonly Palette DarkPalette = new Palette
        {
            Background = new Color(0.047f, 0.082f, 0.141f, 0.97f),
            Accent     = new Color(0.357f, 0.608f, 0.949f, 1.00f),
            Body       = new Color(0.929f, 0.945f, 0.961f, 1.00f),
            Subtle     = new Color(0.424f, 0.471f, 0.541f, 1.00f),
            AccentHex  = "5B9BF2",
            LogoPath   = LogoDarkPath,
            FaceDilate = 0f, // light-on-dark already reads with enough weight
        };

        // Light: surface #ced5db / accent #6B8CC8 (softened, desaturated blue so it blends
        // into the world rather than the saturated #2563EB) / body #495163 / subtle #64748B
        private static readonly Palette LightPalette = new Palette
        {
            Background = new Color(0.973f, 0.980f, 0.988f, 0.97f),
            Accent     = new Color(0.420f, 0.549f, 0.784f, 1.00f),
            Body       = new Color(0.188f, 0.216f, 0.275f, 1.00f),
            Subtle     = new Color(0.392f, 0.455f, 0.545f, 1.00f),
            AccentHex  = "6B8CC8",
            LogoPath   = LogoLightPath,
            FaceDilate = 0.08f, // thicken dark-on-light text so it doesn't read too thin
        };

        // ── Layout (size variants) ────────────────────────────────────────────

        private struct Layout
        {
            public float Width;
            public float Height;
            public float TopBorderHeight;

            public float   LogoSize;
            public Vector2 LogoPos;

            public string  TitleText;
            public float   TitleSize;
            public Vector2 TitlePos;
            public Vector2 TitleSizeDelta;
            public TextAlignmentOptions TitleAlign;

            public bool    ShowCatchphrase;
            public float   CatchSize;
            public Vector2 CatchPos;
            public Vector2 CatchSizeDelta;

            public bool    ShowDivider;
            public float   DividerY;
            public Vector2 DividerSizeDelta;

            public bool    ShowBody;
            public Vector2 BodyOffsetMin;
            public Vector2 BodyOffsetMax;
            public float   BodyFontLarge;
            public float   BodyFontSmall;
            public float   BodyLineSpacing;
            public string  BodyMetricText; // the small detail sentence; varies by size so it fits the panel height

            public bool    ShowFooter;
            public float   FooterSize;
            public float   FooterY;
            public Vector2 FooterSizeDelta;
        }

        // Standard layout (the original, known-good panel — keep values identical).
        private static readonly Layout StandardLayout = new Layout
        {
            Width = 570f, Height = 350f, TopBorderHeight = 4f,
            LogoSize = 90f, LogoPos = new Vector2(30f, -20f),
            TitleText = "VRClarity",
            TitleSize = 40f, TitlePos = new Vector2(150f, -27f),
            TitleSizeDelta = new Vector2(-180f, 50f), TitleAlign = TextAlignmentOptions.BottomLeft,
            ShowCatchphrase = true, CatchSize = 18f,
            CatchPos = new Vector2(150f, -75f), CatchSizeDelta = new Vector2(-180f, 34f),
            ShowDivider = true, DividerY = -122f, DividerSizeDelta = new Vector2(-60f, 2f),
            ShowBody = true,
            BodyOffsetMin = new Vector2(44f, 54f), BodyOffsetMax = new Vector2(-44f, -144f),
            BodyFontLarge = 22f, BodyFontSmall = 18f, BodyLineSpacing = 6f,
            BodyMetricText = "滞在時間・訪問回数などの\n個人を特定しない統計データを取得しています",
            ShowFooter = true, FooterSize = 16f, FooterY = 18f, FooterSizeDelta = new Vector2(-60f, 28f),
        };

        // Compact layout: drops the catchphrase and divider, tighter header, smaller body.
        private static readonly Layout CompactLayout = new Layout
        {
            Width = 520f, Height = 190f, TopBorderHeight = 3f,
            LogoSize = 56f, LogoPos = new Vector2(20f, -14f),
            TitleText = "VRClarity",
            TitleSize = 33f, TitlePos = new Vector2(90f, -18f),
            TitleSizeDelta = new Vector2(-108f, 56f), TitleAlign = TextAlignmentOptions.MidlineLeft,
            ShowCatchphrase = false,
            ShowDivider = false,
            ShowBody = true,
            BodyOffsetMin = new Vector2(30f, 14f), BodyOffsetMax = new Vector2(-30f, -78f),
            BodyFontLarge = 18f, BodyFontSmall = 15f, BodyLineSpacing = 5f,
            BodyMetricText = "個人を特定しない統計データを取得しています",
            ShowFooter = true, FooterSize = 13f, FooterY = 10f, FooterSizeDelta = new Vector2(-44f, 18f),
        };

        // Minimal layout: a square badge with the logo, title, "Installed", and the site URL stacked
        // vertically and centered. No data-collection disclosure — a supplementary marker alongside a full
        // panel. Rendered by CreateSquareStack; LogoPos/TitlePos/CatchPos/FooterY use only the Y offset
        // (X is auto-centered).
        private static readonly Layout MinimalLayout = new Layout
        {
            Width = 250f, Height = 250f, TopBorderHeight = 4f,
            LogoSize = 85f, LogoPos = new Vector2(0f, -35f),
            TitleText = "VRClarity",
            TitleSize = 35f, TitlePos = new Vector2(0f, -115f),
            TitleSizeDelta = new Vector2(0f, 48f), TitleAlign = TextAlignmentOptions.Top,
            ShowCatchphrase = false,
            CatchSize = 20f, CatchPos = new Vector2(0f, -160f), CatchSizeDelta = new Vector2(0f, 28f),
            ShowDivider = false,
            ShowBody = false,
            ShowFooter = true, FooterSize = 15f, FooterY = 30f, FooterSizeDelta = new Vector2(0f, 24f),
        };

        // ── Menu items ───────────────────────────────────────────────────────

        [MenuItem("GameObject/VRClarity/Create Notice Panel", false, 11)]
        private static void CreateDarkStandardMenu(MenuCommand menuCommand) =>
            CreateFromMenu(menuCommand, NoticePanelTheme.Dark, NoticePanelSize.Standard);

        [MenuItem("GameObject/VRClarity/Create Notice Panel (Compact)", false, 12)]
        private static void CreateDarkCompactMenu(MenuCommand menuCommand) =>
            CreateFromMenu(menuCommand, NoticePanelTheme.Dark, NoticePanelSize.Compact);

        [MenuItem("GameObject/VRClarity/Create Notice Panel (Minimal)", false, 13)]
        private static void CreateDarkMinimalMenu(MenuCommand menuCommand) =>
            CreateFromMenu(menuCommand, NoticePanelTheme.Dark, NoticePanelSize.Minimal);

        [MenuItem("GameObject/VRClarity/Create Notice Panel (Light)", false, 14)]
        private static void CreateLightStandardMenu(MenuCommand menuCommand) =>
            CreateFromMenu(menuCommand, NoticePanelTheme.Light, NoticePanelSize.Standard);

        [MenuItem("GameObject/VRClarity/Create Notice Panel (Light, Compact)", false, 15)]
        private static void CreateLightCompactMenu(MenuCommand menuCommand) =>
            CreateFromMenu(menuCommand, NoticePanelTheme.Light, NoticePanelSize.Compact);

        [MenuItem("GameObject/VRClarity/Create Notice Panel (Light, Minimal)", false, 16)]
        private static void CreateLightMinimalMenu(MenuCommand menuCommand) =>
            CreateFromMenu(menuCommand, NoticePanelTheme.Light, NoticePanelSize.Minimal);

        private static void CreateFromMenu(MenuCommand menuCommand, NoticePanelTheme theme, NoticePanelSize size)
        {
            var panel = CreatePanel(menuCommand.context as GameObject, theme, size);
            Undo.RegisterCreatedObjectUndo(panel, "Create VRClarity Notice Panel");
            Selection.activeGameObject = panel;
        }

        // ── Panel creation ───────────────────────────────────────────────────

        /// <summary>Creates and returns a panel. Undo registration and Selection changes are the caller's responsibility.</summary>
        public static GameObject CreatePanel(
            GameObject parent = null,
            NoticePanelTheme theme = NoticePanelTheme.Dark,
            NoticePanelSize size = NoticePanelSize.Standard)
        {
            var font    = GetOrCreateFontAsset();
            var palette = theme == NoticePanelTheme.Light ? LightPalette : DarkPalette;
            var layout  =
                size == NoticePanelSize.Minimal ? MinimalLayout :
                size == NoticePanelSize.Compact ? CompactLayout :
                                                  StandardLayout;

            var root   = new GameObject(BuildName(theme, size));
            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            var rootRt = root.GetComponent<RectTransform>();
            rootRt.sizeDelta  = new Vector2(layout.Width, layout.Height);
            rootRt.localScale = Vector3.one * (size == NoticePanelSize.Minimal ? MinimalWorldScale : WorldScale);

            // Add a component that lets the font be swapped from the Inspector
            var panelComp = root.AddComponent<VRClarityNoticePanel>();
            var so = new SerializedObject(panelComp);
            so.FindProperty("_font").objectReferenceValue = font;
            so.ApplyModifiedProperties();

            CreateBackground(root, palette);
            CreateTopBorder(root, palette, layout);
            if (size == NoticePanelSize.Minimal)
            {
                CreateSquareStack(root, font, palette, layout);
            }
            else
            {
                CreateHeader(root, font, palette, layout);
                if (layout.ShowDivider) CreateDivider(root, palette, layout);
                if (layout.ShowBody) CreateBody(root, font, palette, layout);
                if (layout.ShowFooter) CreateFooter(root, font, palette, layout);
            }

            if (palette.FaceDilate > 0f) ApplyFaceWeight(root, font, palette.FaceDilate);

            GameObjectUtility.SetParentAndAlign(root, parent);
            return root;
        }

        /// <summary>
        /// Assigns a copy of the font material to every text and slightly thickens the glyphs.
        /// Dark-on-light text reads thinner than light-on-dark, so this compensates on the light theme.
        /// </summary>
        private static void ApplyFaceWeight(GameObject root, TMP_FontAsset font, float dilate)
        {
            if (font == null || font.material == null) return;

            var mat = new Material(font.material) { name = font.material.name + " (VRClarity Light)" };
            mat.SetFloat(ShaderUtilities.ID_FaceDilate, dilate);

            foreach (var tmp in root.GetComponentsInChildren<TextMeshProUGUI>(true))
                tmp.fontSharedMaterial = mat;
        }

        private static string BuildName(NoticePanelTheme theme, NoticePanelSize size)
        {
            string sizeTag =
                size == NoticePanelSize.Compact ? "Compact" :
                size == NoticePanelSize.Minimal ? "Minimal" :
                                                  null;
            string themeTag = theme == NoticePanelTheme.Light ? "Light" : null;

            string suffix =
                themeTag != null && sizeTag != null ? $" ({themeTag}, {sizeTag})" :
                themeTag != null                    ? $" ({themeTag})" :
                sizeTag  != null                    ? $" ({sizeTag})" :
                                                      "";
            return "VRClarity Notice Panel" + suffix;
        }

        // ── Font asset ───────────────────────────────────────────────────────

        private static TMP_FontAsset GetOrCreateFontAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
            if (asset == null)
                Debug.LogError("[VRClarity] フォントアセットが見つかりません。\n" +
                    $"パッケージが正しくインポートされているか確認してください。\n{FontAssetPath}");
            return asset;
        }

        // ── Panel parts ──────────────────────────────────────────────────────

        private static void CreateBackground(GameObject parent, Palette palette)
        {
            var go = CreateChild(parent, "Background");
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            go.AddComponent<Image>().color = palette.Background;
        }

        private static void CreateTopBorder(GameObject parent, Palette palette, Layout layout)
        {
            var go = CreateChild(parent, "TopBorder");
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot     = new Vector2(0.5f, 1f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(0f, layout.TopBorderHeight);
            go.AddComponent<Image>().color = palette.Accent;
        }

        private static void CreateHeader(GameObject parent, TMP_FontAsset font, Palette palette, Layout layout)
        {
            var logoRt = CreateChild(parent, "Logo").GetComponent<RectTransform>();
            logoRt.anchorMin = new Vector2(0f, 1f);
            logoRt.anchorMax = new Vector2(0f, 1f);
            logoRt.pivot     = new Vector2(0f, 1f);
            logoRt.anchoredPosition = layout.LogoPos;
            logoRt.sizeDelta = new Vector2(layout.LogoSize, layout.LogoSize);
            var logoImg = logoRt.gameObject.AddComponent<RawImage>();
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(palette.LogoPath);
            if (tex != null) logoImg.texture = tex;

            var titleTmp = CreateTMP(parent, "Title", font);
            var titleRt  = titleTmp.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot     = new Vector2(0f, 1f);
            titleRt.anchoredPosition = layout.TitlePos;
            titleRt.sizeDelta = layout.TitleSizeDelta;
            titleTmp.text      = layout.TitleText.Replace("{ACCENT}", palette.AccentHex);
            titleTmp.fontSize  = layout.TitleSize;
            titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.color     = palette.Body;
            titleTmp.alignment = layout.TitleAlign;

            if (!layout.ShowCatchphrase) return;

            var catchTmp = CreateTMP(parent, "Catchphrase", font);
            var catchRt  = catchTmp.GetComponent<RectTransform>();
            catchRt.anchorMin = new Vector2(0f, 1f);
            catchRt.anchorMax = new Vector2(1f, 1f);
            catchRt.pivot     = new Vector2(0f, 1f);
            catchRt.anchoredPosition = layout.CatchPos;
            catchRt.sizeDelta = layout.CatchSizeDelta;
            catchTmp.text      = "ワールドクリエイターのための、分析ツール。";
            catchTmp.fontSize  = layout.CatchSize;
            catchTmp.color     = palette.Subtle;
            catchTmp.alignment = TextAlignmentOptions.TopLeft;
        }

        private static void CreateDivider(GameObject parent, Palette palette, Layout layout)
        {
            var go = CreateChild(parent, "Divider");
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot     = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, layout.DividerY);
            rt.sizeDelta = layout.DividerSizeDelta;
            go.AddComponent<Image>().color =
                new Color(palette.Accent.r, palette.Accent.g, palette.Accent.b, 0.35f);
        }

        private static void CreateBody(GameObject parent, TMP_FontAsset font, Palette palette, Layout layout)
        {
            var tmp = CreateTMP(parent, "Body", font);
            var rt  = tmp.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = layout.BodyOffsetMin;
            rt.offsetMax = layout.BodyOffsetMax;
            // First line stays at the base size; the detail lines use a smaller <size> so the
            // longer metric sentence fits the text area on one line without wrapping mid-word.
            tmp.text =
                $"このワールドは <color=#{palette.AccentHex}>VRClarity SDK</color> を導入しています\n\n" +
                $"<size={layout.BodyFontSmall:0}>{layout.BodyMetricText}\n" +
                "（UserID・displayName 等は送信されません）</size>";
            tmp.fontSize    = layout.BodyFontLarge;
            tmp.color       = palette.Body;
            tmp.alignment   = TextAlignmentOptions.TopLeft;
            tmp.lineSpacing = layout.BodyLineSpacing;
        }

        private static void CreateFooter(GameObject parent, TMP_FontAsset font, Palette palette, Layout layout)
        {
            var tmp = CreateTMP(parent, "Footer", font);
            var rt  = tmp.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(1f, 0f);
            rt.pivot     = new Vector2(0.5f, 0f);
            rt.anchoredPosition = new Vector2(0f, layout.FooterY);
            rt.sizeDelta = layout.FooterSizeDelta;
            tmp.text      = "vrclarity.net";
            tmp.fontSize  = layout.FooterSize;
            tmp.color     = palette.Subtle;
            tmp.alignment = TextAlignmentOptions.BottomRight;
        }

        // Square "Minimal" badge: logo, title, "Installed", and URL stacked vertically and centered.
        // X positions are auto-centered; only the Y offsets from the layout are used.
        private static void CreateSquareStack(GameObject parent, TMP_FontAsset font, Palette palette, Layout layout)
        {
            // Logo — horizontally centered near the top
            var logoRt = CreateChild(parent, "Logo").GetComponent<RectTransform>();
            logoRt.anchorMin = new Vector2(0.5f, 1f);
            logoRt.anchorMax = new Vector2(0.5f, 1f);
            logoRt.pivot     = new Vector2(0.5f, 1f);
            logoRt.anchoredPosition = new Vector2(0f, layout.LogoPos.y);
            logoRt.sizeDelta = new Vector2(layout.LogoSize, layout.LogoSize);
            var logoImg = logoRt.gameObject.AddComponent<RawImage>();
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(palette.LogoPath);
            if (tex != null) logoImg.texture = tex;

            // Title "VRClarity" — centered
            var titleTmp = CreateTMP(parent, "Title", font);
            var titleRt  = titleTmp.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot     = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = layout.TitlePos;
            titleRt.sizeDelta = layout.TitleSizeDelta;
            titleTmp.text      = "VRClarity";
            titleTmp.fontSize  = layout.TitleSize;
            titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.color     = palette.Body;
            titleTmp.alignment = TextAlignmentOptions.Top;

            // "Installed" — accent tag, centered below the title
            var tagTmp = CreateTMP(parent, "InstalledTag", font);
            var tagRt  = tagTmp.GetComponent<RectTransform>();
            tagRt.anchorMin = new Vector2(0f, 1f);
            tagRt.anchorMax = new Vector2(1f, 1f);
            tagRt.pivot     = new Vector2(0.5f, 1f);
            tagRt.anchoredPosition = layout.CatchPos;
            tagRt.sizeDelta = layout.CatchSizeDelta;
            tagTmp.text      = "<cspace=2>Installed</cspace>";
            tagTmp.fontSize  = layout.CatchSize;
            tagTmp.color     = palette.Accent;
            tagTmp.alignment = TextAlignmentOptions.Top;

            // URL — subtle, centered near the bottom
            var urlTmp = CreateTMP(parent, "Footer", font);
            var urlRt  = urlTmp.GetComponent<RectTransform>();
            urlRt.anchorMin = new Vector2(0f, 0f);
            urlRt.anchorMax = new Vector2(1f, 0f);
            urlRt.pivot     = new Vector2(0.5f, 0f);
            urlRt.anchoredPosition = new Vector2(0f, layout.FooterY);
            urlRt.sizeDelta = layout.FooterSizeDelta;
            urlTmp.text      = "vrclarity.net";
            urlTmp.fontSize  = layout.FooterSize;
            urlTmp.color     = palette.Subtle;
            urlTmp.alignment = TextAlignmentOptions.Bottom;
        }

        // ── Utility ──────────────────────────────────────────────────────────

        private static GameObject CreateChild(GameObject parent, string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform, false);
            go.AddComponent<RectTransform>();
            return go;
        }

        private static TextMeshProUGUI CreateTMP(GameObject parent, string name, TMP_FontAsset font)
        {
            var tmp = CreateChild(parent, name).AddComponent<TextMeshProUGUI>();
            if (font != null) tmp.font = font;
            return tmp;
        }
    }
}
