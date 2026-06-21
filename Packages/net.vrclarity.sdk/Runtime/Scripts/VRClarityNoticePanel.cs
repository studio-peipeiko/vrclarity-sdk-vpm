using TMPro;
using UnityEngine;

namespace StudioPeipeiko.VRClarity.Runtime
{
    /// <summary>
    /// Management component for the VRClarity Notice Panel.
    /// Used to bulk-replace the font across the panel.
    /// It is stripped on VRChat upload, but the font is already applied to each text, so there is no impact.
    /// </summary>
    [DisallowMultipleComponent]
    public class VRClarityNoticePanel : MonoBehaviour
    {
        [SerializeField] internal TMP_FontAsset _font;
    }
}
