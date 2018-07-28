﻿using HMUI;
using SongBrowserPlugin.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;
using Image = UnityEngine.UI.Image;


namespace SongBrowserPlugin.UI
{
    public static class UIBuilder
    {
        /// <summary>
        /// Create an empty BeatSaber VRUI view controller.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T CreateViewController<T>(string name) where T : VRUIViewController
        {
            T vc = new GameObject(name).AddComponent<T>();

            vc.rectTransform.anchorMin = new Vector2(0f, 0f);
            vc.rectTransform.anchorMax = new Vector2(1f, 1f);
            vc.rectTransform.sizeDelta = new Vector2(0f, 0f);
            vc.rectTransform.anchoredPosition = new Vector2(0f, 0f);

            return vc;
        }

        /// <summary>
        /// Create empty FlowCoordinator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T CreateFlowCoordinator<T>(string name) where T : FlowCoordinator
        {
            T vc = new GameObject(name).AddComponent<T>();

            return vc;
        }

        /// <summary>
        /// Clone a Unity Button into a Button we control.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="buttonTemplate"></param>
        /// <param name="buttonInstance"></param>
        /// <returns></returns>
        static public Button CreateUIButton(RectTransform parent, Button buttonTemplate)
        {
            Button btn = UnityEngine.Object.Instantiate(buttonTemplate, parent, false);
            UnityEngine.Object.DestroyImmediate(btn.GetComponent<GameEventOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();
            btn.name = "CustomUIButton";

            return btn;
        }

        /// <summary>
        /// Generic create sort button.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="templateButtonName"></param>
        /// <param name="buttonText"></param>
        /// <param name="iconName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="action"></param>
        public static SongSortButton CreateSortButton(RectTransform rect, Button buttonTemplate, Sprite iconSprite, string buttonText, float fontSize, float x, float y, float w, float h, SongSortMode sortMode, System.Action<SongSortMode> onClickEvent)
        {
            SongSortButton sortButton = new SongSortButton();
            Button newButton = UIBuilder.CreateUIButton(rect, buttonTemplate);

            newButton.interactable = true;
            (newButton.transform as RectTransform).anchoredPosition = new Vector2(x, y);
            (newButton.transform as RectTransform).sizeDelta = new Vector2(w, h);

            /*RectTransform iconTransform = newButton.GetComponentsInChildren<RectTransform>(true).First(c => c.name == "Icon");
            iconTransform.gameObject.SetActive(false);

            HorizontalLayoutGroup hgroup = iconTransform.parent.GetComponent<HorizontalLayoutGroup>();
            hgroup.padding = new RectOffset();
            hgroup.childForceExpandWidth = true;
            hgroup.childForceExpandHeight = true;
            iconTransform.sizeDelta = new Vector2(5f, 5f);
            iconTransform.localScale = new Vector2(1f, 1f);
            iconTransform.anchoredPosition = new Vector2(x, y);*/

            UIBuilder.SetButtonText(ref newButton, buttonText);
            UIBuilder.SetButtonIconEnabled(ref newButton, false);
            UIBuilder.SetButtonIcon(ref newButton, iconSprite);
            UIBuilder.SetButtonTextSize(ref newButton, fontSize);

            newButton.onClick.RemoveAllListeners();
            newButton.onClick.AddListener(delegate ()
            {
                onClickEvent(sortMode);
            });
            
            sortButton.Button = newButton;
            sortButton.SortMode = sortMode;

            return sortButton;
        }

        /// <summary>
        /// Generate TextMesh.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        static public TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 position)
        {
            TextMeshProUGUI textMesh = new GameObject("TextMeshProUGUI_GO").AddComponent<TextMeshProUGUI>();
            textMesh.rectTransform.SetParent(parent, false);
            textMesh.text = text;
            textMesh.fontSize = 4;
            textMesh.color = Color.white;
            textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            textMesh.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            textMesh.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            textMesh.rectTransform.sizeDelta = new Vector2(60f, 10f);
            textMesh.rectTransform.anchoredPosition = position;

            return textMesh;
        }

        /// <summary>
        /// Adjust a Button text.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="text"></param>
        static public void SetButtonText(ref Button button, string text)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {

                button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            }

        }

        /// <summary>
        /// Adjust button text size.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="fontSize"></param>
        static public void SetButtonTextSize(ref Button button, float fontSize)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().fontSize = fontSize;
            }
        }

        /// <summary>
        /// Set a button icon.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        static public void SetButtonIcon(ref Button button, Sprite icon)
        {
            if (button.GetComponentsInChildren<UnityEngine.UI.Image>().Count() > 1)
            {
                Console.WriteLine("SETTING ICONS");
                button.GetComponentsInChildren<Image>().First(x => x.name == "Icon").sprite = icon;
                //button.GetComponentsInChildren<Image>().First(x => x.name == "Icon").transform.Rotate(0, 0, 90);
            }            
        }

        /// <summary>
        /// Disable a button icon.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="enabled"></param>
        static public void SetButtonIconEnabled(ref Button button, bool enabled)
        {
            if (button.GetComponentsInChildren<UnityEngine.UI.Image>().Count() > 1)
            {
                //button.GetComponentsInChildren<Image>().First(x => x.name == "Icon").gameObject.SetActive(enabled);
                button.GetComponentsInChildren<UnityEngine.UI.Image>()[1].enabled = enabled;
            }
        }

        /// <summary>
        /// Adjust button background color.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="background"></param>
        static public void SetButtonBackground(ref Button button, Sprite background)
        {
            if (button.GetComponentsInChildren<Image>().Any())
            {
                button.GetComponentsInChildren<UnityEngine.UI.Image>()[0].sprite = background;
            }

        }

        /// <summary>
        /// Adjust button border.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="color"></param>
        static public void SetButtonBorder(ref Button button, Color color)
        {
            if (button.GetComponentsInChildren<Image>().Any())
            {
                button.GetComponentsInChildren<UnityEngine.UI.Image>()[0].color = color;
            }
        }
    }
}