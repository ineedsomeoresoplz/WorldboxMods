using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NCMS.Utils;

namespace AIBox.UI
{
    public class GlobalLogWindow : MonoBehaviour 
    {
        private static GameObject window;
        private static GameObject logContent;
        private static GameObject briefingSection;

        public static void Open()
        {
            if (window != null) {
                window.SetActive(true);
                return;
            }
            window = WindowManager.Instance.CreateWindow("LOGS", new Vector2(300, 300), Vector2.zero);
            
            // Script to handle updates
            GlobalLogWindow script = window.AddComponent<GlobalLogWindow>();
            script.SetupUI(window);
        }

        private void SetupUI(GameObject window)
        {
            // Close Button
            WindowHelper.CreateHeader(window.transform, "GLOBAL LOGS", () => {
                Destroy(window);
                window = null; 
            });

            // Resize Handle
            RectTransform windowRT = window.GetComponent<RectTransform>();
            ResizableWindow.CreateResizeHandle(window.transform, windowRT, new Vector2(200, 200), new Vector2(600, 600));

            // Global Whisper Button 
            GameObject globalWhisperBtn = new GameObject("GlobalWhisperBtn");
            globalWhisperBtn.transform.SetParent(window.transform, false);
            
            RectTransform gwRT = globalWhisperBtn.AddComponent<RectTransform>();
            gwRT.anchorMin = new Vector2(0, 1);
            gwRT.anchorMax = new Vector2(0, 1);
            gwRT.pivot = new Vector2(0, 0.5f);
            gwRT.sizeDelta = new Vector2(50, 14);
            gwRT.anchoredPosition = new Vector2(5, -10);
            
            Image gwImg = globalWhisperBtn.AddComponent<Image>();
            gwImg.color = new Color(0.3f, 0.3f, 0.35f);
            
            Button gwBtn = globalWhisperBtn.AddComponent<Button>();
            gwBtn.onClick.AddListener(() => DivineWhisperWindow.ShowGlobal());
            
            GameObject gwTextObj = new GameObject("Text");
            gwTextObj.transform.SetParent(globalWhisperBtn.transform, false);
            Text gwText = gwTextObj.AddComponent<Text>();
            gwText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            gwText.text = "ALL CMD";
            gwText.fontSize = 7;
            gwText.color = Color.white;
            gwText.alignment = TextAnchor.MiddleCenter;
            
            RectTransform gwTextRT = gwTextObj.GetComponent<RectTransform>();
            gwTextRT.anchorMin = Vector2.zero;
            gwTextRT.anchorMax = Vector2.one;
            gwTextRT.offsetMin = Vector2.zero;
            gwTextRT.offsetMax = Vector2.zero;

            // Scroll View
            GameObject scrollObj = new GameObject("Scroll");
            scrollObj.transform.SetParent(window.transform, false);
            
            RectTransform scrollRT = scrollObj.AddComponent<RectTransform>();
            scrollRT.anchorMin = Vector2.zero;
            scrollRT.anchorMax = Vector2.one;
            scrollRT.offsetMin = new Vector2(10, 10);
            scrollRT.offsetMax = new Vector2(-10, -40); 

            ScrollRect sr = scrollObj.AddComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;
            sr.scrollSensitivity = 20f;
            
            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollObj.transform, false);
            RectTransform vRT = viewport.AddComponent<RectTransform>();
            vRT.anchorMin = Vector2.zero;
            vRT.anchorMax = Vector2.one;
            vRT.offsetMin = Vector2.zero;
            vRT.offsetMax = Vector2.zero;
            
            viewport.AddComponent<RectMask2D>();

            // MAIN Content (Vertical Stack: Briefing then Grid)
            GameObject mainContent = new GameObject("MainContent");
            mainContent.transform.SetParent(viewport.transform, false);
            RectTransform mainRT = mainContent.AddComponent<RectTransform>();
            
            // Critical for ScrollRect content
            mainRT.anchorMin = new Vector2(0, 1);
            mainRT.anchorMax = new Vector2(1, 1);
            mainRT.pivot = new Vector2(0.5f, 1);
            mainRT.sizeDelta = Vector2.zero;

            VerticalLayoutGroup vlg = mainContent.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.spacing = 10; 
            
            ContentSizeFitter csf = mainContent.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            sr.content = mainRT;
            sr.viewport = vRT;

            // Briefing Section
            briefingSection = new GameObject("BriefingSection");
            briefingSection.transform.SetParent(mainContent.transform, false);
            VerticalLayoutGroup bVlg = briefingSection.AddComponent<VerticalLayoutGroup>();
            bVlg.padding = new RectOffset(5,5,5,5);
            bVlg.spacing = 2;
            Image bBg = briefingSection.AddComponent<Image>();
            bBg.color = new Color(0.2f, 0.2f, 0.3f, 0.8f); 
            
            // Briefing auto-height
            ContentSizeFitter bCsf = briefingSection.AddComponent<ContentSizeFitter>();
            bCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // 2-COLUMN MASONRY
            logContent = new GameObject("LogContainer");
            logContent.transform.SetParent(mainContent.transform, false);
            
            // Container holds 2 columns side-by-side
            HorizontalLayoutGroup hlg = logContent.AddComponent<HorizontalLayoutGroup>();
            hlg.childControlHeight = false; 
            hlg.childControlWidth = true;
            hlg.childForceExpandHeight = false;
            hlg.childForceExpandWidth = true;
            hlg.spacing = 5;

            // Column 1
            GameObject col1 = new GameObject("LeftCol");
            col1.transform.SetParent(logContent.transform, false);
            VerticalLayoutGroup v1 = col1.AddComponent<VerticalLayoutGroup>();
            v1.spacing = 5;
            v1.childControlHeight = true;
            v1.childControlWidth = true;
            v1.childForceExpandHeight = false;
            v1.childForceExpandWidth = true;
            ContentSizeFitter cf1 = col1.AddComponent<ContentSizeFitter>();
            cf1.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Column 2
            GameObject col2 = new GameObject("RightCol");
            col2.transform.SetParent(logContent.transform, false);
            VerticalLayoutGroup v2 = col2.AddComponent<VerticalLayoutGroup>();
            v2.spacing = 5;
            v2.childControlHeight = true;
            v2.childControlWidth = true;
            v2.childForceExpandHeight = false;
            v2.childForceExpandWidth = true;
            ContentSizeFitter cf2 = col2.AddComponent<ContentSizeFitter>();
            cf2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            RefreshLogs();
        }

        private float timer;
        private bool isWhispering = false; 

        private void Update()
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) {
                if(EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null || 
                   EventSystem.current.currentSelectedGameObject.GetComponentInParent<InputField>() != null) {
                    isWhispering = true;
                    return;
                }
            }

            isWhispering = false;

            timer += Time.deltaTime;
            if(timer > 0.2f) {
                timer = 0;
                RefreshLogs();
            }
        }

        private void RefreshLogs()
        {
            if (logContent == null) return;
            
            UpdateBriefing();

            // Validation: correct layout?
            if(logContent.GetComponent<HorizontalLayoutGroup>() == null) {
                Destroy(window);
                window = null;
                return;
            }

            Transform c1 = logContent.transform.GetChild(0);
            Transform c2 = logContent.transform.GetChild(1);
            
            if (WorldDataManager.Instance == null) return;

            var sortedKingdoms = WorldDataManager.Instance.KingdomData.Keys.ToList();
            // Shuffle randomly so different kingdoms appear at the top each time
            for (int i = sortedKingdoms.Count - 1; i > 0; i--) {
                int j = UnityEngine.Random.Range(0, i + 1);
                var temp = sortedKingdoms[i];
                sortedKingdoms[i] = sortedKingdoms[j];
                sortedKingdoms[j] = temp;
            } 
            
            // Track active items to remove stale ones later
            HashSet<string> activeKingdomNames = new HashSet<string>();

            List<(Kingdom k, ThinkingLogEntry log, Transform existing)> itemsToProcess = new List<(Kingdom, ThinkingLogEntry, Transform)>();
            
            foreach(var k in sortedKingdoms)
            {
                var data = WorldDataManager.Instance.GetKingdomData(k);
                if (k == null || !k.isAlive() || data.ThinkingHistory == null || data.ThinkingHistory.Count == 0) continue;
                
                activeKingdomNames.Add(k.name);
                var lastLog = data.ThinkingHistory.Last();
                
                // Try find existing item in either column
                Transform existing = c1.Find("Log_" + k.name);
                if(existing == null) existing = c2.Find("Log_" + k.name);

                if(existing != null && existing.Find("Controls") == null) {
                    DestroyImmediate(existing.gameObject);
                    existing = null; 
                }

                if(existing != null) {
                    UpdateLogItemComponents(existing.gameObject, k, lastLog);
                } else {
                    itemsToProcess.Add((k, lastLog, null));
                }
            }
            
            CleanupColumn(c1, activeKingdomNames);
            CleanupColumn(c2, activeKingdomNames);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(c1.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(c2.GetComponent<RectTransform>());
            
            foreach(var item in itemsToProcess)
            {
                int c1Count = c1.childCount;
                int c2Count = c2.childCount;
                Transform parentCol = (c1Count <= c2Count) ? c1 : c2;
                CreateLogItem(item.k, item.log, parentCol);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(c1.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(c2.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(logContent.GetComponent<RectTransform>());
        }

        private void CleanupColumn(Transform col, HashSet<string> activeNames)
        {
            // Iterate backwards to safely remove
            for(int i = col.childCount - 1; i >= 0; i--) {
                Transform child = col.GetChild(i);
                // "Log_" + k.name
                string kName = child.name.Replace("Log_", "");
                if(!activeNames.Contains(kName)) {
                    Destroy(child.gameObject);
                }
            }
        }

        private void UpdateBriefing()
        {
            Transform contentObjTrans = briefingSection.transform.Find("BriefContent");
            if(contentObjTrans != null) {
                GameObject contentObj = contentObjTrans.gameObject;
                GameObject textObj = contentObj.transform.Find("BriefText").gameObject;
                Text contentText = textObj.GetComponent<Text>();
                
            if(KingdomController.Instance != null && !KingdomController.Instance.IsGeneratingBriefing) {
                 if(string.IsNullOrEmpty(KingdomController.Instance.LastGlobalBriefing) || KingdomController.Instance.LastGlobalBriefing == "No intelligence reports yet.") {
                    contentText.text = "Click here to request a report.";
                } else {
                    contentText.text = KingdomController.Instance.LastGlobalBriefing;
                }
            } else if(KingdomController.Instance != null && KingdomController.Instance.IsGeneratingBriefing) {
                 contentText.text = "The Report is compiling the latest reports from all kingdoms...";
            } else {
                contentText.text = "Report is offline (KingdomController missing).";
            }
                
                // Force rebuild of height
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentObj.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(briefingSection.GetComponent<RectTransform>());
                // Rebuild Main Content too!
                if(briefingSection.transform.parent != null) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(briefingSection.transform.parent.GetComponent<RectTransform>());
                }
                return;
            }

            // Title
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(briefingSection.transform, false);
            Text title = titleObj.AddComponent<Text>();
            
            // Safe Font Loading
            Font arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (arial != null) title.font = arial;
            
            title.fontSize = 6;
            title.fontStyle = FontStyle.Bold;
            title.color = Color.cyan;
            title.text = "KINGDOM REPORT";
            title.alignment = TextAnchor.MiddleCenter;
            
            LayoutElement tLe = titleObj.AddComponent<LayoutElement>();
            tLe.minWidth = 100;
            tLe.minHeight = 12;

            // Briefing Clickable Container
            GameObject newContentObj = new GameObject("BriefContent");
            newContentObj.transform.SetParent(briefingSection.transform, false);
            
            // Background Image for Click Detection
            Image bg = newContentObj.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.01f); 
            
            Button btn = newContentObj.AddComponent<Button>();
            
            LayoutElement cLe = newContentObj.AddComponent<LayoutElement>();
            cLe.minHeight = 30; 
            cLe.flexibleWidth = 1;

            // Add Layout Group to control text child
            VerticalLayoutGroup cVlg = newContentObj.AddComponent<VerticalLayoutGroup>();
            cVlg.childControlHeight = true;
            cVlg.childControlWidth = true;
            cVlg.childForceExpandHeight = false; 
            cVlg.childForceExpandWidth = true;
            cVlg.padding = new RectOffset(4, 4, 4, 4);

            // Add Size Fitter to Resize this container based on Text child
            ContentSizeFitter cFitter = newContentObj.AddComponent<ContentSizeFitter>();
            cFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            GameObject textObjNew = new GameObject("BriefText");
            textObjNew.transform.SetParent(newContentObj.transform, false);
            Text contentTextNew = textObjNew.AddComponent<Text>();
            
            if (arial != null) contentTextNew.font = arial;
            contentTextNew.fontSize = 5;
            contentTextNew.fontStyle = FontStyle.Italic;
            contentTextNew.alignment = TextAnchor.UpperLeft;
            contentTextNew.color = new Color(0.9f, 0.9f, 1f); 
            
            if(KingdomController.Instance != null) {
                if(string.IsNullOrEmpty(KingdomController.Instance.LastGlobalBriefing) || KingdomController.Instance.LastGlobalBriefing == "No intelligence reports yet.") {
                    contentTextNew.text = "Click here to request a report.";
                } else {
                    contentTextNew.text = KingdomController.Instance.LastGlobalBriefing;
                }
            } else {
                contentTextNew.text = "Report is offline (KingdomController missing).";
            }
            
            contentTextNew.horizontalOverflow = HorizontalWrapMode.Wrap;
            contentTextNew.verticalOverflow = VerticalWrapMode.Overflow; // Allow height calculation 
            
            // Button Action
            btn.onClick.AddListener(() => {
                if(KingdomController.Instance != null && !KingdomController.Instance.IsGeneratingBriefing) {
                    KingdomController.Instance.RequestGlobalBriefing();
                    if(contentTextNew != null) contentTextNew.text = "The Report is compiling the latest reports from all kingdoms...";
                }
            });
        }

        private void CreateLogItem(Kingdom k, ThinkingLogEntry entry, Transform parent)
        {
            // Container
            GameObject itemObj = new GameObject("Log_" + k.name);
            itemObj.transform.SetParent(parent, false);
            
            // Background
            Image bg = itemObj.AddComponent<Image>();
            bg.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0,0,1,1), Vector2.zero);
            bg.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);

            // Layout
            VerticalLayoutGroup vlg = itemObj.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.spacing = 2;
            vlg.padding = new RectOffset(4,4,4,4);

            // Size Fitter 
            ContentSizeFitter csfItem = itemObj.AddComponent<ContentSizeFitter>();
            csfItem.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Get King info
            string kingName = "The Crown";
            string race = "Unknown";
            try {
                if(k.king != null) {
                    kingName = k.king.getName();
                    string assetId = k.king.asset.id.ToLower();
                    if (assetId.Contains("human")) race = "Human";
                    else if (assetId.Contains("orc")) race = "Orc";
                    else if (assetId.Contains("elf")) race = "Elf";
                    else if (assetId.Contains("dwarf")) race = "Dwarf";
                    else race = assetId.Replace("unit_", "");
                }
            } catch {}

            // Extract received messages from InputContext
            string receivedMessages = "";
            if(!string.IsNullOrEmpty(entry.InputContext)) {
                int inboxStart = entry.InputContext.IndexOf("INBOX");
                if(inboxStart >= 0) {
                    int inboxEnd = entry.InputContext.IndexOf("INTERNAL:", inboxStart);
                    if(inboxEnd < 0) inboxEnd = entry.InputContext.Length;
                    string inboxSection = entry.InputContext.Substring(inboxStart, inboxEnd - inboxStart);
                    var lines = inboxSection.Split('\n');
                    foreach(var line in lines) {
                        if(line.Contains("MSG:") || line.Contains("OFFER from")) {
                            receivedMessages += line.Trim() + "\n";
                        }
                    }
                }
            }

            // Header with crown emoji and timer
            GameObject headerObj = new GameObject("Header");
            headerObj.transform.SetParent(itemObj.transform, false);
            Text headerText = headerObj.AddComponent<Text>();
            headerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            headerText.fontSize = 8; 
            headerText.fontStyle = FontStyle.Bold;
            headerText.color = Color.yellow;
            
            var kData = WorldDataManager.Instance.GetKingdomData(k);
            float timeRemaining = kData.NextThinkTime - (KingdomController.Instance != null ? KingdomController.Instance.SimTime : 0);
            string timerStr = timeRemaining > 0 ? $" [{timeRemaining:F0}s]" : " [READY]";
            if(kData.AI_IsThinking) timerStr = " [THINKING...]";
            
            headerText.text = $"[Crown] {kingName} of {k.name} ({race}){timerStr}";
            headerText.alignment = TextAnchor.UpperLeft; 
            headerText.horizontalOverflow = HorizontalWrapMode.Wrap;
            LayoutElement hLe = headerObj.AddComponent<LayoutElement>();
            hLe.flexibleWidth = 1;
            ContentSizeFitter hFit = headerObj.AddComponent<ContentSizeFitter>();
            hFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Received Messages 
            GameObject msgObj = new GameObject("Messages");
            msgObj.transform.SetParent(itemObj.transform, false);
            Text msgText = msgObj.AddComponent<Text>();
            msgText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            msgText.fontSize = 6;
            msgText.color = new Color(0.7f, 0.9f, 1f);
            msgText.text = $"[Mail] {receivedMessages.Trim()}";
            msgText.alignment = TextAnchor.UpperLeft; 
            msgText.horizontalOverflow = HorizontalWrapMode.Wrap;
            LayoutElement mLe = msgObj.AddComponent<LayoutElement>();
            mLe.flexibleWidth = 1;
            ContentSizeFitter mFit = msgObj.AddComponent<ContentSizeFitter>();
            mFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            if(string.IsNullOrEmpty(receivedMessages) || receivedMessages.Contains("No new messages")) {
                msgObj.SetActive(false);
            }

            // Reasoning
            GameObject reasonObj = new GameObject("Reasoning");
            reasonObj.transform.SetParent(itemObj.transform, false);
            Text reasonText = reasonObj.AddComponent<Text>();
            reasonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            reasonText.fontSize = 7;
            reasonText.fontStyle = FontStyle.Italic;
            reasonText.color = new Color(0.95f, 0.95f, 0.85f);
            reasonText.text = $"[Think] \"{entry.Reasoning}\"";
            reasonText.alignment = TextAnchor.UpperLeft; 
            reasonText.horizontalOverflow = HorizontalWrapMode.Wrap;
            LayoutElement rLe = reasonObj.AddComponent<LayoutElement>();
            rLe.flexibleWidth = 1;
            ContentSizeFitter rFit = reasonObj.AddComponent<ContentSizeFitter>();
            rFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Decisions Summary
            GameObject decObj = new GameObject("Decisions");
            decObj.transform.SetParent(itemObj.transform, false);
            Text decText = decObj.AddComponent<Text>();
            decText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            decText.fontSize = 6;
            decText.color = new Color(0.6f, 1f, 0.6f);
            string displayAction = !string.IsNullOrEmpty(entry.DecisionSummary) ? entry.DecisionSummary : entry.ParsedDecision;
            decText.text = $"[Action] {displayAction}";
            decText.alignment = TextAnchor.UpperLeft; 
            decText.horizontalOverflow = HorizontalWrapMode.Wrap;
            LayoutElement dLe = decObj.AddComponent<LayoutElement>();
            dLe.flexibleWidth = 1;
            ContentSizeFitter dFit = decObj.AddComponent<ContentSizeFitter>();
            dFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            // Controls Row
            GameObject controlsObj = new GameObject("Controls");
            controlsObj.transform.SetParent(itemObj.transform, false);
            HorizontalLayoutGroup cHlg = controlsObj.AddComponent<HorizontalLayoutGroup>();
            cHlg.childControlHeight = true;
            cHlg.childControlWidth = true;
            cHlg.childForceExpandHeight = false;
            cHlg.childForceExpandWidth = true;
            cHlg.spacing = 5;

            LayoutElement cLeRow = controlsObj.AddComponent<LayoutElement>();
            cLeRow.minHeight = 18;
            cLeRow.flexibleWidth = 1;

            // Show Data
            GameObject btnObj = new GameObject("BtnShowData");
            btnObj.transform.SetParent(controlsObj.transform, false);
            Button btn = btnObj.AddComponent<Button>();
            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.25f, 0.25f, 0.25f);
            
            Text btnText = new GameObject("Text").AddComponent<Text>();
            btnText.transform.SetParent(btnObj.transform, false);
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.fontSize = 10;
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.text = "Data";
            btnText.color = Color.white;
            
            RectTransform btnTextRT = btnText.GetComponent<RectTransform>();
            btnTextRT.anchorMin = Vector2.zero;
            btnTextRT.anchorMax = Vector2.one;
            btnTextRT.offsetMin = Vector2.zero;
            btnTextRT.offsetMax = Vector2.zero;
            
            LayoutElement btnLe = btnObj.AddComponent<LayoutElement>();
            btnLe.minHeight = 18;
            btnLe.flexibleWidth = 1;

            // Divine Whisper
            GameObject btnWhisperObj = new GameObject("BtnWhisper");
            btnWhisperObj.transform.SetParent(controlsObj.transform, false);
            Button btnWhisper = btnWhisperObj.AddComponent<Button>();
            Image btnWhisperImg = btnWhisperObj.AddComponent<Image>();
            btnWhisperImg.color = new Color(0.3f, 0.3f, 0.3f);
            
            Text btnWhisperText = new GameObject("Text").AddComponent<Text>();
            btnWhisperText.transform.SetParent(btnWhisperObj.transform, false);
            btnWhisperText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnWhisperText.fontSize = 10;
            btnWhisperText.alignment = TextAnchor.MiddleCenter;
            btnWhisperText.text = "Divine Whisper";
            btnWhisperText.color = Color.white;
            
            RectTransform btnWhisperTextRT = btnWhisperText.GetComponent<RectTransform>();
            btnWhisperTextRT.anchorMin = Vector2.zero;
            btnWhisperTextRT.anchorMax = Vector2.one;
            btnWhisperTextRT.offsetMin = Vector2.zero;
            btnWhisperTextRT.offsetMax = Vector2.zero;

            LayoutElement btnWhisperLe = btnWhisperObj.AddComponent<LayoutElement>();
            btnWhisperLe.minHeight = 18;
            btnWhisperLe.flexibleWidth = 1;

            // Context Data
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
            
            contextObj.SetActive(false);

            // Button Logic
            btn.onClick.AddListener(() => {
                bool isActive = contextObj.activeSelf;
                contextObj.SetActive(!isActive);
                btnText.text = isActive ? "Show World Data" : "Hide World Data";
            });

            btnWhisper.onClick.AddListener(() => {
                DivineWhisperWindow.ShowFor(k);
            });
        }
        
        private void UpdateLogItemComponents(GameObject itemObj, Kingdom k, ThinkingLogEntry entry)
        {            
            // Validations
            if(itemObj == null) return;
            
            string kingName = "The Crown";
            string race = "Unknown";
            try {
                if(k.king != null) {
                    kingName = k.king.getName();
                    string assetId = k.king.asset.id.ToLower();
                    if (assetId.Contains("human")) race = "Human";
                    else if (assetId.Contains("orc")) race = "Orc";
                    else if (assetId.Contains("elf")) race = "Elf";
                    else if (assetId.Contains("dwarf")) race = "Dwarf";
                    else race = assetId.Replace("unit_", "");
                }
            } catch {}
            
            string receivedMessages = "";
            if(!string.IsNullOrEmpty(entry.InputContext)) {
                int inboxStart = entry.InputContext.IndexOf("INBOX");
                if(inboxStart >= 0) {
                    int inboxEnd = entry.InputContext.IndexOf("INTERNAL:", inboxStart);
                    if(inboxEnd < 0) inboxEnd = entry.InputContext.Length;
                    string inboxSection = entry.InputContext.Substring(inboxStart, inboxEnd - inboxStart);
                    var lines = inboxSection.Split('\n');
                    foreach(var line in lines) {
                        if(line.Contains("MSG:") || line.Contains("OFFER from")) {
                            receivedMessages += line.Trim() + "\n";
                        }
                    }
                }
            }
            
            // Find Children
            Text headerText = itemObj.transform.Find("Header")?.GetComponent<Text>();
            if(headerText != null) {
                var data = WorldDataManager.Instance.GetKingdomData(k);
                float timeRemaining = data.NextThinkTime - (KingdomController.Instance != null ? KingdomController.Instance.SimTime : 0);
                string timerStr = timeRemaining > 0 ? $" [{timeRemaining:F0}s]" : " [READY]";
                if(data.AI_IsThinking) timerStr = " [THINKING...]";
                headerText.text = $"[Crown] {kingName} of {k.name} ({race}){timerStr}";
            }
            
            GameObject msgObj = itemObj.transform.Find("Messages")?.gameObject;
            if(msgObj != null) {
                Text msgText = msgObj.GetComponent<Text>();
                if(!string.IsNullOrEmpty(receivedMessages) && !receivedMessages.Contains("No new messages")) {
                    msgObj.SetActive(true);
                    msgText.text = $"[Mail] {receivedMessages.Trim()}";
                } else {
                    msgObj.SetActive(false);
                }
            }
            
            Text reasonText = itemObj.transform.Find("Reasoning")?.GetComponent<Text>();
            if(reasonText != null) reasonText.text = $"[Think] \"{entry.Reasoning}\"";
            
            Text decText = itemObj.transform.Find("Decisions")?.GetComponent<Text>();
            if(decText != null) {
                string displayTxt = !string.IsNullOrEmpty(entry.DecisionSummary) ? entry.DecisionSummary : entry.ParsedDecision;
                decText.text = $"[Action] {displayTxt}";
            }
            
            Text contextText = itemObj.transform.Find("ContextData")?.GetComponent<Text>();
            if(contextText != null) contextText.text = entry.InputContext;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemObj.GetComponent<RectTransform>());
        }
    }
}



