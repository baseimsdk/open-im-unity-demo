﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn //------------------------------------------------------------

using GameFramework.DataTable;
using GameFramework.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityGameFramework.Runtime;
using Dawn.Game.UI;

namespace Dawn
{
    public static class UIExtension
    {
        static Camera camera;
        public static Camera UICamera
        {
            get
            {
                if (camera == null)
                {
                    camera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
                }
                return camera;
            }
        }
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static bool HasUIOpen(this UIComponent ui, string assertName)
        {
            var groups = ui.GetAllUIGroups();
            foreach (var g in groups)
            {
                foreach (var form in g.GetAllUIForms())
                {
                    Debug.Log(form.UIFormAssetName);
                }
                if (g.GetUIForm(assertName) != null)
                {
                    return true;
                }
            }
            return false;
        }
        public static int OpenUI(this UIComponent ui, string name, object userData = null)
        {
            return ui.OpenUIForm(string.Format("Assets/BundleResources/UI/{0}.prefab", name), "Default", userData);
        }
        public static int OpenUIOverLayer(this UIComponent ui, string name, object userData = null)
        {
            return ui.OpenUIForm(string.Format("Assets/BundleResources/UI/{0}.prefab", name), "OverLayer", userData);
        }

        static int tipUIId = -1;
        public static void Tip(this UIComponent ui, string tip)
        {
            Debug.Log("Tip:" + tip);
            if (tipUIId > 0 && ui.HasUIForm(tipUIId))
            {
                var form = ui.GetUIForm(tipUIId);
                if (form != null)
                {
                    var uitip = form.Logic as UITip;
                    uitip.ShowTip(tip);
                }
            }
            else
            {
                tipUIId = ui.OpenUIOverLayer("Tip", tip);
            }
        }
    }
}
