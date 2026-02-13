using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AIBox.UI;

namespace AIBox
{
    public class DivineLawsWindow : MonoBehaviour
    {
        private static GameObject window;
        private static InputField lawInput;
        private static Text listText;
        private static Text kingdomNameText;
        private static GameObject kingdomSelectorRow;
        private static Kingdom selectedKingdom;
        private static int kingdomIndex = 0;
        private static bool isGlobalMode = true;

        public static void ShowWindow()
        {
            if (window != null)
            {
                Destroy(window);
                window = null;
                return;
            }

            CreateWindow();
        }

        private static void CreateWindow()
        {
            // Main window using WindowManager 
            window = WindowManager.Instance.CreateWindow("Divine Laws", new Vector2(250, 200), Vector2.zero);

            Transform header = window.transform.Find("Header");
            if (header != null)
            {
                Image headerImg = header.GetComponent<Image>();
                if (headerImg != null)
                {
                    headerImg.color = new Color(0.2f, 0.2f, 0.2f, 1f); 
                }

                // Add close button
                GameObject closeBtn = new GameObject("CloseButton");
                closeBtn.transform.SetParent(header, false);
                RectTransform closeRect = closeBtn.AddComponent<RectTransform>();
                closeRect.anchorMin = new Vector2(1, 0.5f);
                closeRect.anchorMax = new Vector2(1, 0.5f);
                closeRect.pivot = new Vector2(1, 0.5f);
                closeRect.sizeDelta = new Vector2(16, 16);
                closeRect.anchoredPosition = new Vector2(-2, 0);

                Image closeBg = closeBtn.AddComponent<Image>();
                closeBg.color = new Color(0.6f, 0.1f, 0.1f, 1f);

                Button closeButton = closeBtn.AddComponent<Button>();
                closeButton.onClick.AddListener(() => {
                    Destroy(window);
                    window = null;
                });

                GameObject xObj = new GameObject("X");
                xObj.transform.SetParent(closeBtn.transform, false);
                Text xText = xObj.AddComponent<Text>();
                xText.text = "X";
                xText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                xText.fontSize = 10;
                xText.color = Color.white;
                xText.alignment = TextAnchor.MiddleCenter;

                RectTransform xRect = xObj.GetComponent<RectTransform>();
                xRect.anchorMin = Vector2.zero;
                xRect.anchorMax = Vector2.one;
                xRect.offsetMin = Vector2.zero;
                xRect.offsetMax = Vector2.zero;
            }

            // Scroll container for content
            GameObject scrollObj = new GameObject("ScrollView");
            scrollObj.transform.SetParent(window.transform, false);
            RectTransform scrollRT = scrollObj.AddComponent<RectTransform>();
            scrollRT.anchorMin = Vector2.zero;
            scrollRT.anchorMax = Vector2.one;
            scrollRT.offsetMin = new Vector2(5, 5);
            scrollRT.offsetMax = new Vector2(-5, -25); 

            ScrollRect sr = scrollObj.AddComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollObj.transform, false);
            RectTransform vRT = viewport.AddComponent<RectTransform>();
            vRT.anchorMin = Vector2.zero;
            vRT.anchorMax = Vector2.one;
            vRT.offsetMin = Vector2.zero;
            vRT.offsetMax = Vector2.zero;
            viewport.AddComponent<RectMask2D>();

            // Content container
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            RectTransform contentRT = content.AddComponent<RectTransform>();
            contentRT.anchorMin = new Vector2(0, 1);
            contentRT.anchorMax = new Vector2(1, 1);
            contentRT.pivot = new Vector2(0.5f, 1);
            contentRT.sizeDelta = new Vector2(0, 400);

            VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = false;
            vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.spacing = 3;
            vlg.padding = new RectOffset(5, 5, 5, 5);

            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            sr.content = contentRT;
            sr.viewport = vRT;

            // Mode toggle buttons
            GameObject modeRow = CreateRow(content.transform, 20);
            CreateSmallButton("Global", modeRow.transform, () => { 
                isGlobalMode = true; 
                RefreshLists(); 
            });
            CreateSmallButton("Kingdom", modeRow.transform, () => { 
                isGlobalMode = false; 
                RefreshLists(); 
            });

            // Kingdom selector
            GameObject kingdomRow = CreateRow(content.transform, 20);
            kingdomSelectorRow = kingdomRow;
            
            CreateSmallButton("<", kingdomRow.transform, () => { 
                CycleKingdom(-1);
            });
            
            // Kingdom name label
            GameObject kingdomNameObj = new GameObject("KingdomName");
            kingdomNameObj.transform.SetParent(kingdomRow.transform, false);
            kingdomNameText = kingdomNameObj.AddComponent<Text>();
            kingdomNameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            kingdomNameText.fontSize = 10;
            kingdomNameText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            kingdomNameText.alignment = TextAnchor.MiddleCenter;
            LayoutElement nameLE = kingdomNameObj.AddComponent<LayoutElement>();
            nameLE.flexibleWidth = 2;
            
            CreateSmallButton(">", kingdomRow.transform, () => { 
                CycleKingdom(1);
            });
            
            // Initialize kingdom selection
            if(World.world.kingdoms.list.Count > 0) {
                kingdomIndex = 0;
                selectedKingdom = World.world.kingdoms.list[0];
            }
            UpdateKingdomDisplay();

            // Input field
            lawInput = CreateInput(content.transform, "Type law here...");

            // Action buttons
            GameObject actionRow = CreateRow(content.transform, 18);
            CreateSmallButton("Add", actionRow.transform, () => {
                if (!string.IsNullOrWhiteSpace(lawInput.text))
                {
                    if (isGlobalMode)
                    {
                        if(!WorldDataManager.Instance.GlobalDivineLaws.ContainsKey(lawInput.text))
                            WorldDataManager.Instance.GlobalDivineLaws.Add(lawInput.text, true);
                    }
                    else if (selectedKingdom != null)
                    {
                        var kd = WorldDataManager.Instance.GetKingdomData(selectedKingdom);
                        if (!kd.StandingOrders.Contains(lawInput.text))
                             kd.StandingOrders += $"\n{lawInput.text}";
                    }
                    lawInput.text = "";
                    RefreshLists();
                }
            });
            CreateSmallButton("Clear", actionRow.transform, () => {
                if (isGlobalMode)
                {
                    WorldDataManager.Instance.GlobalDivineLaws.Clear();
                }
                else if (selectedKingdom != null)
                {
                    var kd = WorldDataManager.Instance.GetKingdomData(selectedKingdom);
                    kd.StandingOrders = "";
                }
                RefreshLists();
            });

            // List display
            GameObject listBg = new GameObject("ListBg");
            listBg.transform.SetParent(content.transform, false);
            RectTransform listRT = listBg.AddComponent<RectTransform>();
            listRT.sizeDelta = new Vector2(0, 100);
            LayoutElement listLE = listBg.AddComponent<LayoutElement>();
            listLE.minHeight = 80;
            listLE.flexibleHeight = 1;

            Image listImg = listBg.AddComponent<Image>();
            listImg.color = new Color(0.08f, 0.08f, 0.08f, 0.9f);

            GameObject listTextObj = new GameObject("Text");
            listTextObj.transform.SetParent(listBg.transform, false);
            RectTransform textRT = listTextObj.AddComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = new Vector2(3, 3);
            textRT.offsetMax = new Vector2(-3, -3);

            listText = listTextObj.AddComponent<Text>();
            listText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            listText.fontSize = 10;
            listText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            listText.alignment = TextAnchor.UpperLeft;

            RefreshLists();
        }

        private static void CycleKingdom(int direction)
        {
            var kingdoms = World.world.kingdoms.list;
            if (kingdoms.Count == 0) return;
            
            kingdomIndex += direction;
            if (kingdomIndex < 0) kingdomIndex = kingdoms.Count - 1;
            if (kingdomIndex >= kingdoms.Count) kingdomIndex = 0;
            
            selectedKingdom = kingdoms[kingdomIndex];
            UpdateKingdomDisplay();
            RefreshLists();
        }

        private static void UpdateKingdomDisplay()
        {
            if (kingdomNameText == null) return;
            
            if (selectedKingdom != null)
            {
                kingdomNameText.text = selectedKingdom.name;
            }
            else
            {
                kingdomNameText.text = "No kingdoms";
            }
        }

        private static void RefreshLists()
        {
            if (listText == null) return;

            // Show/hide kingdom selector based on mode
            if (kingdomSelectorRow != null)
            {
                kingdomSelectorRow.SetActive(!isGlobalMode);
            }

            if (isGlobalMode)
            {
                if (WorldDataManager.Instance.GlobalDivineLaws.Count == 0)
                {
                    listText.text = "<i>No global laws</i>";
                }
                else
                {
                    listText.text = "<b>GLOBAL LAWS:</b>\n";
                    int i = 1;
                    foreach(var kvp in WorldDataManager.Instance.GlobalDivineLaws)
                    {
                        listText.text += $"{i}. {kvp.Key}\n";
                        i++;
                    }
                }
            }
            else
            {
                if (selectedKingdom == null)
                {
                    listText.text = "<i>Select a kingdom</i>";
                }
                else
                {
                    var kd = WorldDataManager.Instance.GetKingdomData(selectedKingdom);
                    if (string.IsNullOrEmpty(kd.StandingOrders))
                    {
                        listText.text = $"<i>No orders for {selectedKingdom.name}</i>";
                    }
                    else
                    {
                        listText.text = $"<b>{selectedKingdom.name}:</b>\n{kd.StandingOrders}";
                    }
                }
            }
        }

        private static GameObject CreateRow(Transform parent, float height)
        {
            GameObject row = new GameObject("Row");
            row.transform.SetParent(parent, false);
            RectTransform rt = row.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, height);
            LayoutElement le = row.AddComponent<LayoutElement>();
            le.minHeight = height;
            le.preferredHeight = height;

            HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = true;
            hlg.childForceExpandHeight = true;
            hlg.spacing = 3;

            return row;
        }

        private static void CreateSmallButton(string text, Transform parent, UnityEngine.Events.UnityAction onClick)
        {
            GameObject btn = new GameObject("Btn_" + text);
            btn.transform.SetParent(parent, false);

            Button button = btn.AddComponent<Button>();
            Image btnImage = btn.AddComponent<Image>();
            btnImage.color = new Color(0.25f, 0.25f, 0.25f, 1f);

            GameObject btnText = new GameObject("Text");
            btnText.transform.SetParent(btn.transform, false);
            RectTransform textRect = btnText.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text txt = btnText.AddComponent<Text>();
            txt.text = text;
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 10;
            txt.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            txt.alignment = TextAnchor.MiddleCenter;

            button.targetGraphic = btnImage;
            button.onClick.AddListener(onClick);
        }

        private static InputField CreateInput(Transform parent, string placeholder)
        {
            GameObject inputObj = new GameObject("InputField");
            inputObj.transform.SetParent(parent, false);
            RectTransform inputRect = inputObj.AddComponent<RectTransform>();
            inputRect.sizeDelta = new Vector2(0, 22);
            LayoutElement le = inputObj.AddComponent<LayoutElement>();
            le.minHeight = 22;
            le.preferredHeight = 22;

            Image inputBg = inputObj.AddComponent<Image>();
            inputBg.color = new Color(0.15f, 0.15f, 0.15f, 1f);

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(inputObj.transform, false);
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(3, 0);
            textRect.offsetMax = new Vector2(-3, 0);

            Text text = textObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 10;
            text.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            text.supportRichText = false;

            GameObject placeholderObj = new GameObject("Placeholder");
            placeholderObj.transform.SetParent(inputObj.transform, false);
            RectTransform placeholderRect = placeholderObj.AddComponent<RectTransform>();
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = new Vector2(3, 0);
            placeholderRect.offsetMax = new Vector2(-3, 0);

            Text placeholderText = placeholderObj.AddComponent<Text>();
            placeholderText.text = placeholder;
            placeholderText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            placeholderText.fontSize = 10;
            placeholderText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            placeholderText.fontStyle = FontStyle.Italic;

            InputField input = inputObj.AddComponent<InputField>();
            input.textComponent = text;
            input.placeholder = placeholderText;

            return input;
        }
    }
}

