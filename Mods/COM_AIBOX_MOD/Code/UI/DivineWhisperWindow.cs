using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AIBox.UI
{
    public class DivineWhisperWindow : MonoBehaviour
    {
        private static GameObject window;
        private static InputField inputField;
        private static Kingdom targetKingdom;
        private static Text titleText;
        private static bool isGlobalMode;

        public static void ShowFor(Kingdom kingdom)
        {
            targetKingdom = kingdom;
            isGlobalMode = false;
            
            if (window == null)
            {
                CreateWindow();
            }
            
            window.SetActive(true);
            inputField.text = "";
            inputField.placeholder.GetComponent<Text>().text = $"Command...";
            
            if (titleText != null)
            {
                titleText.text = $"WHISPER TO {kingdom.name.ToUpper()}";
            }
            
            inputField.ActivateInputField();
        }

        public static void ShowGlobal()
        {
            targetKingdom = null;
            isGlobalMode = true;
            
            if (window == null)
            {
                CreateWindow();
            }
            
            window.SetActive(true);
            inputField.text = "";
            inputField.placeholder.GetComponent<Text>().text = $"Command to ALL...";
            
            if (titleText != null)
            {
                titleText.text = "WHISPER TO ALL KINGDOMS";
            }
            
            inputField.ActivateInputField();
        }

        private static void CreateWindow()
        {
            window = new GameObject("DivineWhisperWindow");
            window.transform.SetParent(DebugConfig.instance.transform, false);

            RectTransform rt = window.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(160, 70);

            Image bg = window.AddComponent<Image>();
            bg.color = new Color(0.12f, 0.12f, 0.12f, 0.95f);

            var dw = window.AddComponent<DraggableWindow>();
            dw.TargetWindow = window.transform;

            GameObject closeBtnGO = new GameObject("CloseButton");
            closeBtnGO.transform.SetParent(window.transform, false);
            
            RectTransform closeBtnRT = closeBtnGO.AddComponent<RectTransform>();
            closeBtnRT.anchorMin = new Vector2(1, 1);
            closeBtnRT.anchorMax = new Vector2(1, 1);
            closeBtnRT.pivot = new Vector2(1, 1);
            closeBtnRT.sizeDelta = new Vector2(14, 14);
            closeBtnRT.anchoredPosition = new Vector2(-2, -2);

            Image closeBtnImg = closeBtnGO.AddComponent<Image>();
            closeBtnImg.color = new Color(0.6f, 0.15f, 0.15f);

            Button closeBtn = closeBtnGO.AddComponent<Button>();
            closeBtn.onClick.AddListener(() => Hide());

            GameObject closeTextObj = new GameObject("X");
            closeTextObj.transform.SetParent(closeBtnGO.transform, false);
            Text closeText = closeTextObj.AddComponent<Text>();
            closeText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            closeText.text = "X";
            closeText.fontSize = 9;
            closeText.fontStyle = FontStyle.Bold;
            closeText.color = Color.white;
            closeText.alignment = TextAnchor.MiddleCenter;

            RectTransform closeTextRT = closeTextObj.GetComponent<RectTransform>();
            closeTextRT.anchorMin = Vector2.zero;
            closeTextRT.anchorMax = Vector2.one;
            closeTextRT.offsetMin = Vector2.zero;
            closeTextRT.offsetMax = Vector2.zero;

            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(window.transform, false);
            titleText = titleObj.AddComponent<Text>();
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.text = "DIVINE WHISPER";
            titleText.fontSize = 8;
            titleText.fontStyle = FontStyle.Bold;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;

            RectTransform titleRT = titleObj.GetComponent<RectTransform>();
            titleRT.anchorMin = new Vector2(0, 0.78f);
            titleRT.anchorMax = new Vector2(1, 1);
            titleRT.offsetMin = new Vector2(4, 0);
            titleRT.offsetMax = new Vector2(-18, -2);

            GameObject inputGO = new GameObject("WhisperInput");
            inputGO.transform.SetParent(window.transform, false);
            
            Image inputBg = inputGO.AddComponent<Image>();
            inputBg.color = new Color(0.08f, 0.08f, 0.08f);
            
            inputField = inputGO.AddComponent<InputField>();
            
            GameObject inputTextObj = new GameObject("Text");
            inputTextObj.transform.SetParent(inputGO.transform, false);
            Text inputText = inputTextObj.AddComponent<Text>();
            inputText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            inputText.color = Color.white;
            inputText.fontSize = 8;
            inputText.alignment = TextAnchor.MiddleLeft;
            
            RectTransform inputTextRT = inputTextObj.GetComponent<RectTransform>();
            inputTextRT.anchorMin = Vector2.zero;
            inputTextRT.anchorMax = Vector2.one;
            inputTextRT.offsetMin = new Vector2(3, 1);
            inputTextRT.offsetMax = new Vector2(-3, -1);

            GameObject placeholderObj = new GameObject("Placeholder");
            placeholderObj.transform.SetParent(inputGO.transform, false);
            Text placeholderText = placeholderObj.AddComponent<Text>();
            placeholderText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            placeholderText.color = new Color(0.4f, 0.4f, 0.4f);
            placeholderText.fontSize = 8;
            placeholderText.fontStyle = FontStyle.Italic;
            placeholderText.text = "Command...";
            placeholderText.alignment = TextAnchor.MiddleLeft;
            
            RectTransform placeholderRT = placeholderObj.GetComponent<RectTransform>();
            placeholderRT.anchorMin = Vector2.zero;
            placeholderRT.anchorMax = Vector2.one;
            placeholderRT.offsetMin = new Vector2(3, 1);
            placeholderRT.offsetMax = new Vector2(-3, -1);

            inputField.textComponent = inputText;
            inputField.placeholder = placeholderText;
            inputField.targetGraphic = inputBg;

            RectTransform inputRT = inputGO.GetComponent<RectTransform>();
            inputRT.anchorMin = new Vector2(0, 0.38f);
            inputRT.anchorMax = new Vector2(1, 0.75f);
            inputRT.offsetMin = new Vector2(6, 0);
            inputRT.offsetMax = new Vector2(-6, 0);

            GameObject sendBtnGO = new GameObject("SendButton");
            sendBtnGO.transform.SetParent(window.transform, false);
            Button sendBtn = sendBtnGO.AddComponent<Button>();
            Image sendBtnImg = sendBtnGO.AddComponent<Image>();
            sendBtnImg.color = new Color(0.2f, 0.2f, 0.2f);

            RectTransform sendBtnRT = sendBtnGO.GetComponent<RectTransform>();
            sendBtnRT.anchorMin = new Vector2(0.25f, 0.06f);
            sendBtnRT.anchorMax = new Vector2(0.75f, 0.32f);
            sendBtnRT.offsetMin = Vector2.zero;
            sendBtnRT.offsetMax = Vector2.zero;

            GameObject sendTextObj = new GameObject("Text");
            sendTextObj.transform.SetParent(sendBtnGO.transform, false);
            Text sendText = sendTextObj.AddComponent<Text>();
            sendText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            sendText.color = Color.white;
            sendText.text = "SEND";
            sendText.fontSize = 8;
            sendText.fontStyle = FontStyle.Bold;
            sendText.alignment = TextAnchor.MiddleCenter;

            RectTransform sendTextRT = sendText.GetComponent<RectTransform>();
            sendTextRT.anchorMin = Vector2.zero;
            sendTextRT.anchorMax = Vector2.one;
            sendTextRT.offsetMin = Vector2.zero;
            sendTextRT.offsetMax = Vector2.zero;

            sendBtn.onClick.AddListener(OnSendWhisper);

            window.SetActive(false);
        }

        private static void OnSendWhisper()
        {
            string msg = inputField.text.Trim();
            
            if (string.IsNullOrEmpty(msg))
            {
                window.SetActive(false);
                return;
            }

            if (isGlobalMode)
            {
                int count = 0;
                foreach(var k in World.world.kingdoms.list.Where(x => x.isAlive() && x.isCiv()))
                {
                    var kData = WorldDataManager.Instance.GetKingdomData(k);
                    if (kData != null)
                    {
                        kData.PendingDivineWhisper = msg;
                        ForceKingdomThink(k);
                        count++;
                    }
                }
                WorldTip.showNow($"Whisper sent to {count} kingdoms", false, "top", 1.5f);
            }
            else if (targetKingdom != null)
            {
                var kData = WorldDataManager.Instance.GetKingdomData(targetKingdom);
                if (kData != null)
                {
                    kData.PendingDivineWhisper = msg;
                    ForceKingdomThink(targetKingdom);
                    WorldTip.showNow($"Whisper sent to {targetKingdom.name}", false, "top", 1.5f);
                }
            }

            window.SetActive(false);
        }

        public static void Hide()
        {
            if (window != null)
            {
                window.SetActive(false);
            }
        }

        // Force the kingdom to process AI immediately after receiving a whisper
        private static void ForceKingdomThink(Kingdom k)
        {
            if (KingdomController.Instance != null && KingdomController.Instance.IsAIEnabled)
            {
                KingdomController.Instance.QueueRequest(k);
            }
        }
    }
}

