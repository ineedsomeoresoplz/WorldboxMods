using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMS;
using NCMS.Utils;
using System.Linq;

namespace AIBox.UI
{
    public class KingdomDecisionWindow : MonoBehaviour
    {
        private static KingdomDecisionWindow instance;
        private static readonly Color COL_BG = new Color(0.1f, 0.05f, 0.15f, 0.98f); 
        private static readonly Color COL_ACCENT_SELECTED = new Color(0.3f, 0.2f, 0.4f, 1f); 
        private static readonly Color COL_TEXT_MAIN = new Color(1.0f, 1.0f, 1.0f, 1f); 
        private static readonly Color COL_TEXT_DIM = new Color(0.7f, 0.7f, 0.7f, 1f);

        public static void Open(Kingdom k = null) 
        {
            if (instance != null && instance.gameObject != null)
            {
                instance.gameObject.SetActive(true);
                if(k != null) instance.SelectKingdom(k);
                return;
            }

            if (WindowManager.Instance == null) return;
            
            // Match MarketTerminal Size roughly
            GameObject win = WindowManager.Instance.CreateWindow("KINGDOM DECISIONS", new Vector2(250, 150), Vector2.zero);
            
            Image winBg = win.GetComponent<Image>();
            if(winBg != null) {
                winBg.color = COL_BG;
            }

            instance = win.AddComponent<KingdomDecisionWindow>();
            instance.SetupUI(win);
            
            if(k != null) instance.SelectKingdom(k);
        }

        private GameObject sidebarContent;
        private GameObject logContent;
        private Transform mainPanel;
        
        private Kingdom selectedKingdom;
        private float refreshTimer;
        private int lastLogCount = -1;

        public void SetupUI(GameObject window)
        {
            // Root Layout
            GameObject root = new GameObject("RootLayout");
            root.transform.SetParent(window.transform, false);
            RectTransform rRT = root.AddComponent<RectTransform>();
            rRT.anchorMin = Vector2.zero;
            rRT.anchorMax = Vector2.one;
            rRT.offsetMin = new Vector2(10, 4);
            rRT.offsetMax = new Vector2(-10, -34); 
            
            // Sidebar
            GameObject sidebar = new GameObject("Sidebar");
            sidebar.transform.SetParent(root.transform, false);
            RectTransform sbRT = sidebar.AddComponent<RectTransform>();
            sbRT.anchorMin = new Vector2(0, 0); 
            sbRT.anchorMax = new Vector2(0, 1); 
            sbRT.pivot = new Vector2(0, 0.5f);
            sbRT.sizeDelta = new Vector2(100, 0); 
            sbRT.anchoredPosition = new Vector2(0, 0); 
            
            Image sbBg = sidebar.AddComponent<Image>();
            sbBg.color = new Color(0, 0, 0, 0.3f); 

            SetupSidebarScroll(sidebar);

            // Main Panel
            GameObject main = new GameObject("MainContent");
            main.transform.SetParent(root.transform, false);
            RectTransform mainRT = main.AddComponent<RectTransform>();
            mainRT.anchorMin = new Vector2(0, 0);
            mainRT.anchorMax = new Vector2(1, 1); 
            mainRT.pivot = new Vector2(0, 0.5f);
            // Offset Min X = 104 (100 width + 4 spacing)
            mainRT.offsetMin = new Vector2(104, 0); 
            mainRT.offsetMax = new Vector2(0, 0);   
            
            Image mainBg = main.AddComponent<Image>();
            mainBg.color = new Color(0, 0, 0, 0.2f);

            SetupLogScroll(main);
            
            mainPanel = main.transform;

            WindowHelper.CreateHeader(window.transform, "KINGDOM MIND", () => Destroy(window));

            if(WorldDataManager.Instance != null && WorldDataManager.Instance.KingdomData.Count > 0)
            {
               SelectKingdom(WorldDataManager.Instance.KingdomData.Keys.First()); 
            }
            RefreshSidebar();
        }

        public void SelectKingdom(Kingdom k)
        {
            selectedKingdom = k;
            lastLogCount = -1; 
            RefreshSidebar();
            UpdateMainView();
        }

        private void SetupSidebarScroll(GameObject sidebar)
        {
            ScrollRect sr = sidebar.AddComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;
            sr.scrollSensitivity = 10f;
            
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(sidebar.transform, false);
            RectTransform vRT = viewport.AddComponent<RectTransform>();
            vRT.anchorMin = Vector2.zero;
            vRT.anchorMax = Vector2.one;
            vRT.offsetMin = new Vector2(2,2);
            vRT.offsetMax = new Vector2(-2,-2);
            
            viewport.AddComponent<RectMask2D>();

            sidebarContent = new GameObject("Content");
            sidebarContent.transform.SetParent(viewport.transform, false);
            RectTransform cRT = sidebarContent.AddComponent<RectTransform>();
            cRT.anchorMin = new Vector2(0, 1);
            cRT.anchorMax = new Vector2(1, 1);
            cRT.pivot = new Vector2(0.5f, 1);
            
            sr.content = cRT;
            sr.viewport = vRT;
            
            VerticalLayoutGroup vlg = sidebarContent.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = false; 
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = false;       
            vlg.childForceExpandWidth = false;   
            vlg.childAlignment = TextAnchor.UpperCenter; 
            vlg.spacing = 4;
            vlg.padding = new RectOffset(0, 0, 4, 4); 
            
            ContentSizeFitter csf = sidebarContent.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void SetupLogScroll(GameObject mainPanel)
        {
            ScrollRect sr = mainPanel.AddComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;
            sr.scrollSensitivity = 20f;
            
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(mainPanel.transform, false);
            RectTransform vRT = viewport.AddComponent<RectTransform>();
            vRT.anchorMin = Vector2.zero;
            vRT.anchorMax = Vector2.one;
            vRT.offsetMin = new Vector2(4,4);
            vRT.offsetMax = new Vector2(-4,-4);
            
            viewport.AddComponent<RectMask2D>();

            logContent = new GameObject("LogContent");
            logContent.transform.SetParent(viewport.transform, false);
            RectTransform cRT = logContent.AddComponent<RectTransform>();
            cRT.anchorMin = new Vector2(0, 1); 
            cRT.anchorMax = new Vector2(1, 1);
            cRT.pivot = new Vector2(0.5f, 1);
            cRT.sizeDelta = Vector2.zero; 
            
            sr.content = cRT;
            sr.viewport = vRT;
            
            VerticalLayoutGroup vlg = logContent.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = true;
            vlg.childForceExpandWidth = true;
            vlg.spacing = 2;
            
            ContentSizeFitter csf = logContent.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void Update()
        {
            if(selectedKingdom != null && (!selectedKingdom.isAlive() || selectedKingdom.data == null))
            {
                selectedKingdom = null;
            }

            refreshTimer += Time.deltaTime;
            if (refreshTimer > 0.2f) {
                refreshTimer = 0;
                if(selectedKingdom != null) UpdateMainView();
            }
        }

        private void RefreshSidebar()
        {
            foreach(Transform child in sidebarContent.transform) Destroy(child.gameObject);
            
            if(WorldDataManager.Instance == null) return;

            var list = WorldDataManager.Instance.KingdomData.ToList();
            
            foreach(var kvp in list) {
                Kingdom k = kvp.Key;
                KingdomEconomyData data = kvp.Value;
                
                if (k == null || !k.isAlive()) continue;

                GameObject row = new GameObject("Row");
                row.transform.SetParent(sidebarContent.transform, false);
                LayoutElement le = row.AddComponent<LayoutElement>();
                le.minHeight = 32; 
                le.preferredHeight = 32;
                le.preferredWidth = 60; 

                // Background
                Image bg = row.AddComponent<Image>();
                bg.color = (selectedKingdom == k) ? COL_ACCENT_SELECTED : new Color(0,0,0,0.1f);
                
                Button b = row.AddComponent<Button>();
                b.onClick.AddListener(() => { 
                    SelectKingdom(k); 
                });
                
                // --- Row Layout: HGroup ---
                HorizontalLayoutGroup rowH = row.AddComponent<HorizontalLayoutGroup>();
                rowH.childControlWidth = false;
                rowH.childForceExpandWidth = false;
                rowH.padding = new RectOffset(2, 2, 2, 2);
                rowH.spacing = 4;
                
                // Flag
                GameObject flag = new GameObject("Flag");
                flag.transform.SetParent(row.transform, false);
                LayoutElement fLE = flag.AddComponent<LayoutElement>();
                fLE.minWidth = 24; fLE.minHeight = 24; 
                fLE.preferredWidth = 24; fLE.preferredHeight = 24;
                
                // Flag Background
                GameObject fBgObj = new GameObject("FlagBg");
                fBgObj.transform.SetParent(flag.transform, false);
                RectTransform fBgRT = fBgObj.AddComponent<RectTransform>();
                fBgRT.anchorMin = Vector2.zero; fBgRT.anchorMax = Vector2.one;
                fBgRT.offsetMin = Vector2.zero; fBgRT.offsetMax = Vector2.zero;
                
                Image fBg = fBgObj.AddComponent<Image>();
                fBg.sprite = k.getElementBackground(); 
                fBg.color = k.kingdomColor != null ? k.kingdomColor.getColorMain() : Color.gray;

                // Flag Icon
                GameObject fIcoObj = new GameObject("FlagIcon");
                fIcoObj.transform.SetParent(flag.transform, false);
                RectTransform fIcoRT = fIcoObj.AddComponent<RectTransform>();
                fIcoRT.anchorMin = Vector2.zero; fIcoRT.anchorMax = Vector2.one;
                fIcoRT.offsetMin = new Vector2(2,2); fIcoRT.offsetMax = new Vector2(-2,-2); 
                
                Image fIco = fIcoObj.AddComponent<Image>();
                fIco.sprite = k.getElementIcon(); 
                fIco.color = k.kingdomColor != null ? k.kingdomColor.getColorBanner() : Color.white;
            }
        }
        
        private void UpdateMainView()
        {
             if (selectedKingdom == null || !selectedKingdom.isAlive()) {
                 foreach(Transform child in logContent.transform) Destroy(child.gameObject);
                 return;
             }
             
             var data = WorldDataManager.Instance.GetKingdomData(selectedKingdom);
             if (data == null || data.ThinkingHistory == null) return;
             
             int count = data.ThinkingHistory.Count;
             
             // Check if we need a full rebuild
             if(count == lastLogCount) {
                 // Just update the banner status if possible
                 UpdateBannerOnly(data);
                 return; 
             }
             
             lastLogCount = count;
             
             // Full Refresh 
             foreach(Transform child in logContent.transform) Destroy(child.gameObject);
             
             // Create Banner
             CreateStatusBanner(data);
             
             if(count == 0) {
                 return;
             }
             
             foreach(var entry in data.ThinkingHistory)
             {
                 CreateLogItem(entry);
             }
        }
        
        private void UpdateBannerOnly(KingdomEconomyData data)
        {
            if(logContent.transform.childCount > 0) {
                Transform banner = logContent.transform.GetChild(0);
                if(banner.name == "Banner") {
                     Text t = banner.GetComponentInChildren<Text>();
                     if(t != null) {
                         t.text = GetBannerText(data);
                         Image bg = banner.GetComponent<Image>();
                         if(bg != null) bg.color = data.AI_IsThinking ? new Color(0, 0.5f, 0, 0.3f) : new Color(1f, 1f, 1f, 0.1f);
                     }
                }
            }
        }
        
        private string GetBannerText(KingdomEconomyData data) {
            float timeRem = data.NextThinkTime - (KingdomController.Instance != null ? KingdomController.Instance.SimTime : 0);
            string status = data.AI_IsThinking ? "<color=green>THINKING...</color>" : $"Next Council: {Mathf.Max(0, timeRem):F0}s";
            return $"{selectedKingdom.name.ToUpper()} - {status}";
        }

        private void CreateStatusBanner(KingdomEconomyData data)
        {
             GameObject item = new GameObject("Banner");
             item.transform.SetParent(logContent.transform, false);
             VerticalLayoutGroup vlg = item.AddComponent<VerticalLayoutGroup>();
             vlg.padding = new RectOffset(5,5,5,5);
             
             Image bg = item.AddComponent<Image>();
             bg.color = new Color(1f, 1f, 1f, 0.1f);
             if(data.AI_IsThinking) bg.color = new Color(0, 0.5f, 0, 0.3f);
             
             Text t = CreateTextObj(item, GetBannerText(data), 7, Color.white);
             t.alignment = TextAnchor.MiddleCenter;
             t.fontStyle = FontStyle.Bold;
        }

        private void CreateLogItem(ThinkingLogEntry entry)
        {
            // Container
            GameObject itemObj = new GameObject("LogItem");
            itemObj.transform.SetParent(logContent.transform, false);
            
            // Background 
            Image bg = itemObj.AddComponent<Image>();
            bg.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0,0,1,1), Vector2.zero);
            bg.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);

            // Layout Group for Item 
            VerticalLayoutGroup vlg = itemObj.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childAlignment = TextAnchor.UpperLeft; 
            vlg.spacing = 2; 
            vlg.padding = new RectOffset(6,6,6,6);

            // 1. Header: Timestamp + Summary
            GameObject headerObj = new GameObject("Header");
            headerObj.transform.SetParent(itemObj.transform, false);
            
            Text headerText = headerObj.AddComponent<Text>();
            headerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            headerText.fontSize = 7; 
            headerText.color = Color.yellow;
            string displayAction = !string.IsNullOrEmpty(entry.DecisionSummary) ? entry.DecisionSummary : entry.ParsedDecision;
            headerText.text = $"[{entry.Timestamp:F1}] {displayAction}";
            headerText.alignment = TextAnchor.UpperLeft; 
            headerText.horizontalOverflow = HorizontalWrapMode.Wrap;
            headerText.verticalOverflow = VerticalWrapMode.Truncate; 
            headerText.resizeTextForBestFit = false;

            LayoutElement hLe = headerObj.AddComponent<LayoutElement>();
            hLe.flexibleWidth = 1;

            ContentSizeFitter hFit = headerObj.AddComponent<ContentSizeFitter>();
            hFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            hFit.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            // Reasoning
            GameObject reasonObj = new GameObject("Reasoning");
            reasonObj.transform.SetParent(itemObj.transform, false);
            
            Text reasonText = reasonObj.AddComponent<Text>();
            reasonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            reasonText.fontSize = 6;
            reasonText.fontStyle = FontStyle.Italic;
            reasonText.color = new Color(0.9f, 0.9f, 0.9f);
            reasonText.text = $"Reasoning: {entry.Reasoning}";
            reasonText.alignment = TextAnchor.UpperLeft; 
            reasonText.horizontalOverflow = HorizontalWrapMode.Wrap; 
            reasonText.verticalOverflow = VerticalWrapMode.Truncate;

            LayoutElement rLe = reasonObj.AddComponent<LayoutElement>();
            rLe.flexibleWidth = 1;

            ContentSizeFitter rFit = reasonObj.AddComponent<ContentSizeFitter>();
            rFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            rFit.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            // Toggle Button
            GameObject btnObj = new GameObject("BtnExpand");
            btnObj.transform.SetParent(itemObj.transform, false);
            
            Button btn = btnObj.AddComponent<Button>();
            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.25f, 0.25f, 0.25f);
            
            Text btnText = new GameObject("Text").AddComponent<Text>();
            btnText.transform.SetParent(btnObj.transform, false);
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.fontSize = 6;
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.text = "Show World Data";
            btnText.color = Color.white;
            
            // Button Height
            LayoutElement btnLe = btnObj.AddComponent<LayoutElement>();
            btnLe.minHeight = 22; 
            btnLe.preferredHeight = 22;
            btnLe.flexibleWidth = 1;
            
            // Button Text Rect
            RectTransform btnTextRect = btnText.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.offsetMin = Vector2.zero;
            btnTextRect.offsetMax = Vector2.zero;

            // 4. Input Context
            GameObject contextObj = new GameObject("ContextData");
            contextObj.transform.SetParent(itemObj.transform, false);
            
            Text contextText = contextObj.AddComponent<Text>();
            contextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            contextText.fontSize = 5;
            contextText.color = new Color(0.7f, 0.7f, 0.7f);
            contextText.text = entry.InputContext;
            contextText.alignment = TextAnchor.UpperLeft;
            contextText.horizontalOverflow = HorizontalWrapMode.Wrap; 
            contextText.verticalOverflow = VerticalWrapMode.Truncate;
            
            LayoutElement ctxLe = contextObj.AddComponent<LayoutElement>();
            ctxLe.flexibleWidth = 1;

            ContentSizeFitter ctxFit = contextObj.AddComponent<ContentSizeFitter>();
            ctxFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            ctxFit.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            contextObj.SetActive(false); 

            // Click Event
            btn.onClick.AddListener(() => {
                bool isActive = contextObj.activeSelf;
                contextObj.SetActive(!isActive);
                btnText.text = isActive ? "Show World Data" : "Hide World Data";
                LayoutRebuilder.ForceRebuildLayoutImmediate(logContent.GetComponent<RectTransform>());
            });

            // Divine Whisper Button
            GameObject btnWhisperObj = new GameObject("BtnWhisper");
            btnWhisperObj.transform.SetParent(itemObj.transform, false);
            btnWhisperObj.transform.SetSiblingIndex(btnObj.transform.GetSiblingIndex() + 1);
            
            Button btnWhisper = btnWhisperObj.AddComponent<Button>();
            Image btnWhisperImg = btnWhisperObj.AddComponent<Image>();
            btnWhisperImg.color = new Color(0.2f, 0.2f, 0.2f);
            
            Text btnWhisperText = new GameObject("Text").AddComponent<Text>();
            btnWhisperText.transform.SetParent(btnWhisperObj.transform, false);
            btnWhisperText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnWhisperText.fontSize = 6;
            btnWhisperText.alignment = TextAnchor.MiddleCenter;
            btnWhisperText.text = "Divine Whisper";
            btnWhisperText.color = Color.white;
            
            LayoutElement btnWhisperLe = btnWhisperObj.AddComponent<LayoutElement>();
            btnWhisperLe.minHeight = 22;
            btnWhisperLe.preferredHeight = 22;
            btnWhisperLe.flexibleWidth = 1;
            
            RectTransform btnWhisperTextRect = btnWhisperText.GetComponent<RectTransform>();
            btnWhisperTextRect.anchorMin = Vector2.zero;
            btnWhisperTextRect.anchorMax = Vector2.one;
            btnWhisperTextRect.offsetMin = Vector2.zero;
            btnWhisperTextRect.offsetMax = Vector2.zero;

            btnWhisper.onClick.AddListener(() => {
                if(selectedKingdom != null) DivineWhisperWindow.ShowFor(selectedKingdom);
            });
        }

        private void CreateTextItem(string txt, Color col, int size)
        {
            GameObject item = new GameObject("TextItem");
            item.transform.SetParent(logContent.transform, false);
            CreateTextObj(item, txt, size, col);
        }

        private Text CreateTextObj(GameObject parent, string content, int size, Color col)
        {
            GameObject tObj = new GameObject("Text");
            tObj.transform.SetParent(parent.transform, false);
            Text t = tObj.AddComponent<Text>();
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.text = content;
            t.fontSize = size;
            t.color = col;
            t.alignment = TextAnchor.UpperLeft;
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Overflow; 
            
            LayoutElement le = tObj.AddComponent<LayoutElement>();
            le.minWidth = 0; 
            le.flexibleWidth = 1; 
            
            ContentSizeFitter csf = tObj.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            return t;
        }
    }
}

