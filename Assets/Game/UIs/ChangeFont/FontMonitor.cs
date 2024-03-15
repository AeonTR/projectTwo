using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class FontMonitor : MonoBehaviour
{
    [Serializable]
    struct FontReplace
    {
        public Font cnFont;
        public Font twFont;
    }

    [SerializeField]
    private FontReplace[] fontsList = new FontReplace[] { };

    private Dictionary<Font, Font> replaceFontDic = new Dictionary<Font, Font>();
    private Dictionary<Font, HashSet<Text>> textTrackerDic;
    private Dictionary<Text, Font> changeTexDic = new Dictionary<Text, Font>();

    void Start ()
    {

        System.Type typetracker = typeof(FontUpdateTracker);
        FieldInfo info = typetracker.GetField("m_Tracked", BindingFlags.Static | BindingFlags.NonPublic);
        if (null == info)
        {
            Debug.LogError("[FontMontior] not found m_Tracked");
            return;
        }

        textTrackerDic =  info.GetValue(null) as Dictionary<Font, HashSet<Text>>;
        if (null == textTrackerDic)
        {
            Debug.LogError("[FontMontior] textTrackerDic is null");
            return;
        }

        for (int i = 0; i < fontsList.Length; i++)
        {
            if (!replaceFontDic.ContainsKey(fontsList[i].cnFont) && null != fontsList[i].twFont)
            {
                replaceFontDic.Add(fontsList[i].cnFont, fontsList[i].twFont);
            }
        }

        if (replaceFontDic.Count <= 0)
        {
            Debug.LogError("[FontMontior] replaceFontDic is empty");
        }
    }

    void LateUpdate ()
    {
        if (null == textTrackerDic)
        {
            return;
        }

        bool isChange = false;
        foreach (var kv in textTrackerDic)
        {
            Font twFont = null;
            if (replaceFontDic.TryGetValue(kv.Key, out twFont) && kv.Key != twFont)
            {
                foreach (var text in kv.Value)
                {
                    if (!changeTexDic.ContainsKey(text))
                    {
                        isChange = true;
                        changeTexDic.Add(text, twFont);
                    }
                }
            }
        }

        foreach (var kv in changeTexDic)
        {
            kv.Key.font = kv.Value;
        }

        if (isChange)
        {
            changeTexDic.Clear();
        }
    }
}
