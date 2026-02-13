using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AIBox.UI
{
    public class AIDataViewerWindow : MonoBehaviour
    {
        private static AIDataViewerWindow instance;
        private static GameObject window;
        
        private GameObject content;
        private Kingdom selectedKingdom;
        private int kingdomIndex = 0;
        private Text kingdomNameText;
        private Text dataText;
        private ScrollRect scrollRect;
        private GameObject flagContainer;

        public static void Open()
        {
            if (window != null)
            {
                Destroy(window);
                window = null;
                instance = null;
                return;
            }

            if (WindowManager.Instance == null) return;

            window = WindowManager.Instance.CreateWindow("KINGDOM DATA", new Vector2(220, 180), Vector2.zero);
            instance = window.AddComponent<AIDataViewerWindow>();
            instance.SetupUI();
        }

        private void SetupUI()
        {
            // Root Container
            GameObject root = new GameObject("Root");
            root.transform.SetParent(window.transform, false);
            RectTransform rootRT = root.AddComponent<RectTransform>();
            rootRT.anchorMin = Vector2.zero;
            rootRT.anchorMax = Vector2.one;
            rootRT.offsetMin = new Vector2(5, 5);
            rootRT.offsetMax = new Vector2(-5, -25);

            VerticalLayoutGroup rootVLG = root.AddComponent<VerticalLayoutGroup>();
            rootVLG.childControlHeight = false;
            rootVLG.childControlWidth = true;
            rootVLG.childForceExpandHeight = false;
            rootVLG.childForceExpandWidth = true;
            rootVLG.spacing = 5;
            rootVLG.padding = new RectOffset(5, 5, 5, 5);

            // Kingdom Selector Row
            GameObject selectorRow = new GameObject("SelectorRow");
            selectorRow.transform.SetParent(root.transform, false);
            LayoutElement selectorLE = selectorRow.AddComponent<LayoutElement>();
            selectorLE.minHeight = 32;
            selectorLE.preferredHeight = 32;

            HorizontalLayoutGroup selectorHLG = selectorRow.AddComponent<HorizontalLayoutGroup>();
            selectorHLG.childControlWidth = true;
            selectorHLG.childControlHeight = true;
            selectorHLG.childForceExpandWidth = true;
            selectorHLG.childForceExpandHeight = true;
            selectorHLG.spacing = 5;

            // Prev Button
            CreateNavigationButton("<", selectorRow.transform, () => CycleKingdom(-1));
            
            // Flag Container
            flagContainer = new GameObject("FlagContainer");
            flagContainer.transform.SetParent(selectorRow.transform, false);
            LayoutElement flagLE = flagContainer.AddComponent<LayoutElement>();
            flagLE.minWidth = 32; 
            flagLE.minHeight = 32;
            flagLE.preferredWidth = 32; 
            flagLE.preferredHeight = 32;
            flagLE.flexibleWidth = 0;  
            
            // Kingdom Name Text
            GameObject nameTextObj = new GameObject("KingdomName");
            nameTextObj.transform.SetParent(selectorRow.transform, false);
            kingdomNameText = nameTextObj.AddComponent<Text>();
            kingdomNameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            kingdomNameText.fontSize = 11;
            kingdomNameText.color = Color.white;
            kingdomNameText.alignment = TextAnchor.MiddleCenter;
            kingdomNameText.fontStyle = FontStyle.Bold;
            LayoutElement nameTxtLE = nameTextObj.AddComponent<LayoutElement>();
            nameTxtLE.flexibleWidth = 2;  

            // Next Button
            CreateNavigationButton(">", selectorRow.transform, () => CycleKingdom(1));

            // Scrollable Data Area
            GameObject scrollObj = new GameObject("ScrollView");
            scrollObj.transform.SetParent(root.transform, false);
            LayoutElement scrollLE = scrollObj.AddComponent<LayoutElement>();
            scrollLE.flexibleHeight = 0.3f;
            scrollLE.minHeight = 20;

            scrollRect = scrollObj.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.scrollSensitivity = 40f;

            Image scrollBg = scrollObj.AddComponent<Image>();
            scrollBg.color = new Color(0.05f, 0.05f, 0.05f, 0.9f);

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollObj.transform, false);
            RectTransform vRT = viewport.AddComponent<RectTransform>();
            vRT.anchorMin = Vector2.zero;
            vRT.anchorMax = Vector2.one;
            vRT.offsetMin = new Vector2(3, 3);
            vRT.offsetMax = new Vector2(-3, -3);
            viewport.AddComponent<RectMask2D>();

            // Content
            content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            RectTransform cRT = content.AddComponent<RectTransform>();
            cRT.anchorMin = new Vector2(0, 1);
            cRT.anchorMax = new Vector2(1, 1);
            cRT.pivot = new Vector2(0.5f, 1);
            cRT.sizeDelta = Vector2.zero;

            scrollRect.content = cRT;
            scrollRect.viewport = vRT;

            VerticalLayoutGroup cVLG = content.AddComponent<VerticalLayoutGroup>();
            cVLG.childControlHeight = true;
            cVLG.childControlWidth = true;
            cVLG.childForceExpandHeight = false;
            cVLG.childForceExpandWidth = true;
            cVLG.padding = new RectOffset(3, 3, 3, 3);

            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Data Text
            GameObject dataTextObj = new GameObject("DataText");
            dataTextObj.transform.SetParent(content.transform, false);
            dataText = dataTextObj.AddComponent<Text>();
            dataText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            dataText.fontSize = 9;
            dataText.color = new Color(0.9f, 0.9f, 0.9f);
            dataText.alignment = TextAnchor.UpperLeft;
            dataText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dataText.verticalOverflow = VerticalWrapMode.Overflow;

            LayoutElement dataLE = dataTextObj.AddComponent<LayoutElement>();
            dataLE.flexibleWidth = 1;

            ContentSizeFitter dataCSF = dataTextObj.AddComponent<ContentSizeFitter>();
            dataCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Close button
            WindowHelper.CreateHeader(window.transform, "KINGDOM DATA", () => {
                Destroy(window);
                window = null;
                instance = null;
            });

            // Initialize
            InitializeKingdom();
        }

        private void CreateButton(string text, Transform parent, UnityEngine.Events.UnityAction onClick, float width)
        {
            GameObject btnObj = new GameObject("Btn_" + text);
            btnObj.transform.SetParent(parent, false);
            
            Button btn = btnObj.AddComponent<Button>();
            Image img = btnObj.AddComponent<Image>();
            img.color = new Color(0.25f, 0.25f, 0.25f);
            
            btn.targetGraphic = img;
            btn.onClick.AddListener(onClick);

            LayoutElement le = btnObj.AddComponent<LayoutElement>();
            le.minWidth = width;
            le.preferredWidth = width;

            GameObject txtObj = new GameObject("Text");
            txtObj.transform.SetParent(btnObj.transform, false);
            Text txt = txtObj.AddComponent<Text>();
            txt.text = text;
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 12;
            txt.color = Color.white;
            txt.alignment = TextAnchor.MiddleCenter;

            RectTransform txtRT = txtObj.GetComponent<RectTransform>();
            txtRT.anchorMin = Vector2.zero;
            txtRT.anchorMax = Vector2.one;
            txtRT.offsetMin = Vector2.zero;
            txtRT.offsetMax = Vector2.zero;
        }

        private void CreateNavigationButton(string text, Transform parent, UnityEngine.Events.UnityAction onClick)
        {
            GameObject btnObj = new GameObject("Btn_" + text);
            btnObj.transform.SetParent(parent, false);
            
            Button btn = btnObj.AddComponent<Button>();
            Image img = btnObj.AddComponent<Image>();
            img.color = new Color(0.25f, 0.25f, 0.25f);
            
            btn.targetGraphic = img;
            btn.onClick.AddListener(onClick);

            GameObject txtObj = new GameObject("Text");
            txtObj.transform.SetParent(btnObj.transform, false);
            Text txt = txtObj.AddComponent<Text>();
            txt.text = text;
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 12;
            txt.color = Color.white;
            txt.alignment = TextAnchor.MiddleCenter;

            RectTransform txtRT = txtObj.GetComponent<RectTransform>();
            txtRT.anchorMin = Vector2.zero;
            txtRT.anchorMax = Vector2.one;
            txtRT.offsetMin = Vector2.zero;
            txtRT.offsetMax = Vector2.zero;
        }

        private void InitializeKingdom()
        {
            var kingdoms = World.world.kingdoms.list.Where(k => k != null && k.isAlive() && k.isCiv()).ToList();
            if (kingdoms.Count > 0)
            {
                kingdomIndex = 0;
                selectedKingdom = kingdoms[0];
                UpdateDisplay();
            }
            else
            {
                kingdomNameText.text = "No kingdoms";
                dataText.text = "";
            }
        }

        private void CycleKingdom(int direction)
        {
            var kingdoms = World.world.kingdoms.list.Where(k => k != null && k.isAlive() && k.isCiv()).ToList();
            if (kingdoms.Count == 0) return;

            kingdomIndex += direction;
            if (kingdomIndex < 0) kingdomIndex = kingdoms.Count - 1;
            if (kingdomIndex >= kingdoms.Count) kingdomIndex = 0;

            selectedKingdom = kingdoms[kingdomIndex];
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (selectedKingdom == null) return;

            kingdomNameText.text = selectedKingdom.name;

            if (flagContainer != null)
            {
                foreach (Transform child in flagContainer.transform) Destroy(child.gameObject);

                GameObject fBgObj = new GameObject("FlagBg");
                fBgObj.transform.SetParent(flagContainer.transform, false);
                RectTransform fBgRT = fBgObj.AddComponent<RectTransform>();
                fBgRT.anchorMin = Vector2.zero; fBgRT.anchorMax = Vector2.one;
                fBgRT.offsetMin = Vector2.zero; fBgRT.offsetMax = Vector2.zero;

                Image fBg = fBgObj.AddComponent<Image>();
                fBg.sprite = selectedKingdom.getElementBackground();
                fBg.color = selectedKingdom.kingdomColor != null ? selectedKingdom.kingdomColor.getColorMain() : Color.gray;

                GameObject fIcoObj = new GameObject("FlagIcon");
                fIcoObj.transform.SetParent(flagContainer.transform, false);
                RectTransform fIcoRT = fIcoObj.AddComponent<RectTransform>();
                fIcoRT.anchorMin = Vector2.zero; fIcoRT.anchorMax = Vector2.one;
                fIcoRT.offsetMin = new Vector2(3, 3); fIcoRT.offsetMax = new Vector2(-3, -3);

                Image fIco = fIcoObj.AddComponent<Image>();
                fIco.sprite = selectedKingdom.getElementIcon();
                fIco.color = selectedKingdom.kingdomColor != null ? selectedKingdom.kingdomColor.getColorBanner() : Color.white;
            }

            var kData = WorldDataManager.Instance.GetKingdomData(selectedKingdom);
            if (kData != null)
            {
                dataText.text = BuildKingdomDataDisplay(selectedKingdom, kData);
            }
            else
            {
                dataText.text = "No data available.";
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        }

        private string BuildKingdomDataDisplay(Kingdom k, KingdomEconomyData data)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("<b>== REALM ==</b>");
            sb.AppendLine($"Population: {k.getPopulationTotal()}");
            sb.AppendLine($"Cities: {k.cities.Count}");
            
            int armyVal = 0;
            try { armyVal = k.cities.Sum(c => c.units.Count(u => u.isWarrior())); } catch {}
            sb.AppendLine($"Army: {armyVal}");
            sb.AppendLine();

            // CULTURE & RELIGION
            sb.AppendLine("<b>== IDENTITY ==</b>");
            try {
                var (cultN, _, _) = CultureReligionHelper.GetKingdomCulture(k);
                var (relN, _, _) = CultureReligionHelper.GetKingdomReligion(k);
                sb.AppendLine($"Culture: {cultN}");
                sb.AppendLine($"Religion: {relN}");
                
                var demos = CultureReligionHelper.GetDemographics(k);
                if(demos.Count > 0) {
                     var dStr = string.Join(", ", demos.OrderByDescending(x => x.Value).Take(3).Select(x => $"{x.Key}:{x.Value:F0}%"));
                     sb.AppendLine($"Demos: {dStr}");
                }
            } catch {}
            sb.AppendLine();
            
            // ECONOMY
            sb.AppendLine("<b>== ECONOMY ==</b>");
            sb.AppendLine($"Gold Reserves: {data.GoldReserves:N0}");
            sb.AppendLine($"GDP (Wealth): {data.Wealth:N0}");
            sb.AppendLine($"National Debt: {data.NationalDebt:N0}");
            sb.AppendLine($"Expenses: {data.Expenses:N0}/tick");
            sb.AppendLine();
            
            // POLICIES
            sb.AppendLine("<b>== POLICIES ==</b>");
            sb.AppendLine($"Tax Rate: {data.TaxRate:P0}");
            sb.AppendLine($"Policy: {data.CurrentPolicy}");
            sb.AppendLine($"Ideology: {data.EconomicSystem}");
            sb.AppendLine();
            
            // CURRENCY
            sb.AppendLine("<b>== CURRENCY ==</b>");
            sb.AppendLine($"Name: {data.CurrencyName}");
            sb.AppendLine($"Value: {data.CurrencyValue:F2}");
            sb.AppendLine($"Supply: {data.CurrencySupply:N0}");
            sb.AppendLine();
            
            // TREND
            sb.AppendLine("<b>== TREND ==</b>");
            if (data.WealthHistory != null && data.WealthHistory.Count > 1)
            {
                float prev = data.WealthHistory[data.WealthHistory.Count - 2];
                float change = data.Wealth - prev;
                string trend = change > 100 ? "BOOM!" : change > 0 ? "Growing" : change > -100 ? "Stagnant" : "RECESSION!";
                sb.AppendLine($"GDP Change: {(change >= 0 ? "+" : "")}{change:N0} ({trend})");
            }
            sb.AppendLine($"Phase: {data.CurrentPhase}");
            sb.AppendLine();

            // RESOURCES
            sb.AppendLine("<b>== INVENTORY ==</b>");
            Dictionary<string, int> totalResources = new Dictionary<string, int>();
            foreach(City c in k.cities) {
                foreach(var res in AssetManager.resources.list) {
                    int amt = c.getResourcesAmount(res.id);
                    if(amt > 0) {
                        if(!totalResources.ContainsKey(res.id)) totalResources[res.id] = 0;
                        totalResources[res.id] += amt;
                    }
                }
            }
            
            List<string> resList = new List<string>();
            foreach(var kvp in totalResources.OrderByDescending(x => x.Value).Take(8)) {
                resList.Add($"{kvp.Key}: {kvp.Value}");
            }
            sb.AppendLine(resList.Count > 0 ? string.Join(", ", resList) : "None");
            sb.AppendLine();
            
            // DIPLOMACY
            sb.AppendLine("<b>== DIPLOMACY ==</b>");
            foreach(Kingdom other in World.world.kingdoms.list) {
                if(other == k || !other.isAlive() || !other.isCiv()) continue;
                
                string status = "Neutral";
                if(k.isEnemy(other)) status = "<color=red>WAR</color>";
                else if(k.hasAlliance() && other.hasAlliance() && k.getAlliance() == other.getAlliance()) 
                    status = "<color=green>ALLIED</color>";
                
                var oData = WorldDataManager.Instance.GetKingdomData(other);
                if(oData != null && oData.CurrencyID == data.CurrencyID) status += " (COIN)";
                
                sb.AppendLine($"• {other.name}: {status}");
            }
            
            return sb.ToString();
        }

        private void Update()
        {
            // Auto-refresh every 2 seconds
            if (selectedKingdom != null && Time.frameCount % 120 == 0)
            {
                UpdateDisplay();
            }
        }
    }
}
