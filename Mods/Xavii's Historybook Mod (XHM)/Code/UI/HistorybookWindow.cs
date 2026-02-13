using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XaviiHistorybookMod.Code.Data;
using XaviiHistorybookMod.Code.Managers;
using XaviiWindowsMod.API;

namespace XaviiHistorybookMod.Code.UI
{
    public class HistorybookWindow : MonoBehaviour
    {
        private const string ModGuid = "com.xavii.xhm";
        private const string WindowFile = "historybook_window";
        private const string RuntimeId = "com.xavii.xhm::historybook_window";
        private const float UnitRowSpacing = 44f;
        private const float EventRowSpacing = 148f;

        private static readonly Color UnitRowIdleColor = new Color(0.17f, 0.25f, 0.37f, 0.95f);
        private static readonly Color UnitRowSelectedColor = new Color(0.30f, 0.45f, 0.65f, 1f);
        private static readonly Color UnitRowIdleTextColor = new Color(0.90f, 0.95f, 1f, 1f);
        private static readonly Color UnitRowSelectedTextColor = Color.white;

        private HistorybookManager _manager;
        private XwmWindowHandle _windowHandle;
        private Button _toggleButton;
        private long _selectedUnitId = -1;
        private Vector2 _unitTemplatePosition;
        private Vector2 _eventTemplatePosition;
        private readonly List<UnitRowView> _unitRows = new List<UnitRowView>();
        private readonly List<EventRowView> _eventRows = new List<EventRowView>();

        private sealed class UnitRowView
        {
            public XwmElementRef Root;
            public Button Button;
            public Text Label;
            public long UnitId;
        }

        private sealed class EventRowView
        {
            public XwmElementRef Root;
            public Text Title;
            public Text Date;
            public Text Description;
            public Button FollowButton;
            public Text FollowLabel;
        }

        public void Initialize(HistorybookManager manager)
        {
            _manager = manager;
            _manager.OnHistoryChanged += Refresh;
            BuildToggleButton();
            if (!EnsureWindow())
            {
                return;
            }

            SetVisible(false);
            Refresh();
        }

        public void SetVisible(bool visible)
        {
            if (!EnsureWindow())
            {
                return;
            }

            if (visible)
            {
                _windowHandle.Show();
                _windowHandle.BringToFront();
                Refresh();
                return;
            }

            _windowHandle.Hide();
        }

        public void Refresh()
        {
            if (_manager == null)
            {
                return;
            }

            if (!EnsureWindow())
            {
                return;
            }

            List<HistoryRecord> favorites = _manager.GetFavoriteRecords();
            if (_selectedUnitId < 0 || favorites.TrueForAll(record => record.UnitId != _selectedUnitId))
            {
                _selectedUnitId = favorites.Count > 0 ? favorites[0].UnitId : -1;
            }

            if (!_windowHandle.IsVisible)
            {
                return;
            }

            RebuildUnitRows(favorites);
            UpdateUnitSelection();
            RenderTimeline(_selectedUnitId);

            if (favorites.Count > 0)
            {
                _windowHandle.SetText("subtitleLabel", Localize("historybook_select_prompt", "Select a favorite from the left column to open their timeline."));
            }
            else
            {
                _windowHandle.SetText("subtitleLabel", Localize("historybook_no_favorites", "Mark a unit as favorite to start capturing their story."));
            }
        }

        private bool EnsureWindow()
        {
            if (_windowHandle != null && !_windowHandle.IsDestroyed)
            {
                return true;
            }

            _windowHandle = XwmFiles.GetOrLoad(ModGuid, WindowFile, RuntimeId, false);
            if (_windowHandle == null)
            {
                return false;
            }

            _unitRows.Clear();
            _eventRows.Clear();
            BindWindowActions();
            CacheTemplatePositions();
            ApplyStaticTexts();
            _windowHandle.SetActive("unitTemplateButton", false);
            _windowHandle.SetActive("eventTemplateFrame", false);
            _windowHandle.SetActive("emptyStateLabel", true);
            _windowHandle.SetInteractable("deleteEntryButton", false);
            _windowHandle.SetInteractable("exportButton", false);
            return true;
        }

        private void BindWindowActions()
        {
            _windowHandle.ConnectButtonClick("closeButton", () => SetVisible(false));
            _windowHandle.ConnectButtonClick("deleteAllButton", DeleteAllHistories);
            _windowHandle.ConnectButtonClick("deleteEntryButton", DeleteSelectedRecord);
            _windowHandle.ConnectButtonClick("exportButton", ExportSelectedRecord);
        }

        private void CacheTemplatePositions()
        {
            XwmElementRef unitTemplate = _windowHandle.Get("unitTemplateButton");
            if (unitTemplate != null)
            {
                _unitTemplatePosition = unitTemplate.Position;
            }

            XwmElementRef eventTemplate = _windowHandle.Get("eventTemplateFrame");
            if (eventTemplate != null)
            {
                _eventTemplatePosition = eventTemplate.Position;
            }
        }

        private void ApplyStaticTexts()
        {
            _windowHandle.SetText("titleLabel", Localize("historybook_title", "Historybook"));
            _windowHandle.SetText("subtitleLabel", Localize("historybook_select_prompt", "Select a favorite from the left column to open their timeline."));
            _windowHandle.SetText("unitHeaderLabel", Localize("historybook_favorites_header", "Favorite Units"));
            _windowHandle.SetText("timelineTitleLabel", Localize("historybook_timeline_header", "Timeline"));
            _windowHandle.SetText("deleteAllButton", Localize("historybook_delete_all", "Delete All"));
            _windowHandle.SetText("deleteEntryButton", Localize("historybook_delete_entry", "Delete Entry"));
            _windowHandle.SetText("exportButton", Localize("historybook_export_button", "Export timeline"));
            _windowHandle.SetText("emptyStateLabel", Localize("historybook_no_favorites", "Mark a unit as favorite to start capturing their story."));
            _windowHandle.SetText("eventFollowButton", Localize("historybook_follow_relative", "Follow"));
        }

        private void BuildToggleButton()
        {
            Transform parent = CanvasMain.instance.canvas_ui.transform;
            GameObject buttonObject = new GameObject("XHM Historybook Toggle");
            buttonObject.transform.SetParent(parent, false);

            RectTransform rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.sizeDelta = new Vector2(196f, 36f);
            rect.anchoredPosition = new Vector2(18f, -14f);

            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.13f, 0.19f, 0.30f, 0.95f);
            image.raycastTarget = true;

            _toggleButton = buttonObject.AddComponent<Button>();
            _toggleButton.onClick.AddListener(ToggleWindow);

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(buttonObject.transform, false);
            RectTransform labelRect = labelObject.AddComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            Text label = labelObject.AddComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            label.fontSize = 17;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = new Color(0.95f, 0.98f, 1f, 1f);
            label.text = Localize("historybook_open_button", "Historybook");
            label.raycastTarget = false;
        }

        private void ToggleWindow()
        {
            if (!EnsureWindow())
            {
                WorldTip.showNow("Historybook window failed to load.", false);
                return;
            }

            bool visible = _windowHandle.Toggle();
            if (visible)
            {
                _windowHandle.BringToFront();
                Refresh();
            }
        }

        private void RebuildUnitRows(List<HistoryRecord> favorites)
        {
            EnsureUnitRows(favorites.Count);

            for (int i = 0; i < _unitRows.Count; i++)
            {
                UnitRowView row = _unitRows[i];
                bool active = i < favorites.Count;
                if (row.Root != null)
                {
                    row.Root.IsActive = active;
                }

                if (!active)
                {
                    continue;
                }

                HistoryRecord record = favorites[i];
                row.UnitId = record.UnitId;
                string unitLabel = BuildUnitLabel(record);

                if (row.Root != null)
                {
                    row.Root.Position = new Vector2(_unitTemplatePosition.x, _unitTemplatePosition.y + UnitRowSpacing * i);
                    row.Root.SetText(unitLabel);
                }

                if (row.Label != null)
                {
                    row.Label.text = unitLabel;
                }

                if (row.Button != null)
                {
                    row.Button.onClick.RemoveAllListeners();
                    long selectedId = record.UnitId;
                    row.Button.onClick.AddListener(() => SelectRecord(selectedId));
                }
            }
        }

        private void EnsureUnitRows(int count)
        {
            while (_unitRows.Count < count)
            {
                int index = _unitRows.Count;
                XwmElementRef created = _windowHandle.Duplicate("unitTemplateButton", new Vector2(0f, UnitRowSpacing * index));
                if (created == null)
                {
                    break;
                }

                UnitRowView view = new UnitRowView
                {
                    Root = created,
                    Button = created.Button != null ? created.Button : created.GameObject.GetComponent<Button>(),
                    Label = ResolveElementTextComponent(created),
                    UnitId = -1
                };

                view.Root.IsActive = false;
                _unitRows.Add(view);
            }
        }

        private void SelectRecord(long unitId)
        {
            _selectedUnitId = unitId;
            UpdateUnitSelection();
            RenderTimeline(unitId);
        }

        private void UpdateUnitSelection()
        {
            for (int i = 0; i < _unitRows.Count; i++)
            {
                UnitRowView row = _unitRows[i];
                if (row.Root == null || !row.Root.IsActive)
                {
                    continue;
                }

                bool selected = row.UnitId == _selectedUnitId;
                row.Root.SetImageColor(selected ? UnitRowSelectedColor : UnitRowIdleColor);
                row.Root.SetTextColor(selected ? UnitRowSelectedTextColor : UnitRowIdleTextColor);
                if (row.Label != null)
                {
                    row.Label.color = selected ? UnitRowSelectedTextColor : UnitRowIdleTextColor;
                }
            }
        }

        private void RenderTimeline(long unitId)
        {
            if (unitId < 0)
            {
                _windowHandle.SetText("timelineTitleLabel", Localize("historybook_timeline_header", "Timeline"));
                _windowHandle.SetText("emptyStateLabel", Localize("historybook_no_favorites", "Mark a unit as favorite to start capturing their story."));
                _windowHandle.SetActive("emptyStateLabel", true);
                _windowHandle.SetInteractable("deleteEntryButton", false);
                _windowHandle.SetInteractable("exportButton", false);
                HideEventRows();
                return;
            }

            HistoryRecord record = _manager.TryGetRecord(unitId);
            if (record == null)
            {
                _windowHandle.SetText("timelineTitleLabel", Localize("historybook_timeline_header", "Timeline"));
                _windowHandle.SetText("emptyStateLabel", Localize("historybook_select_prompt", "Select a favorite from the left column to open their timeline."));
                _windowHandle.SetActive("emptyStateLabel", true);
                _windowHandle.SetInteractable("deleteEntryButton", false);
                _windowHandle.SetInteractable("exportButton", false);
                HideEventRows();
                return;
            }

            _windowHandle.SetText("timelineTitleLabel", BuildTimelineTitle(record));
            _windowHandle.SetInteractable("deleteEntryButton", true);
            _windowHandle.SetInteractable("exportButton", true);

            List<HistoryEntry> entries = record.Events ?? new List<HistoryEntry>();
            bool hasEvents = entries.Count > 0;
            _windowHandle.SetActive("emptyStateLabel", !hasEvents);
            if (!hasEvents)
            {
                _windowHandle.SetText("emptyStateLabel", Localize("historybook_empty_timeline", "No timeline events yet."));
                HideEventRows();
                return;
            }

            EnsureEventRows(entries.Count);
            for (int i = 0; i < _eventRows.Count; i++)
            {
                EventRowView row = _eventRows[i];
                bool active = i < entries.Count;
                if (row.Root != null)
                {
                    row.Root.IsActive = active;
                }

                if (!active)
                {
                    continue;
                }

                row.Root.Position = new Vector2(_eventTemplatePosition.x, _eventTemplatePosition.y + EventRowSpacing * i);
                ConfigureEventRow(row, entries[i]);
            }
        }

        private void EnsureEventRows(int count)
        {
            while (_eventRows.Count < count)
            {
                int index = _eventRows.Count;
                XwmElementRef created = _windowHandle.Duplicate("eventTemplateFrame", new Vector2(0f, EventRowSpacing * index));
                if (created == null)
                {
                    break;
                }

                EventRowView row = new EventRowView
                {
                    Root = created,
                    Title = FindNamedComponent<Text>(created.Transform, "EventTitleLabel"),
                    Date = FindNamedComponent<Text>(created.Transform, "EventDateLabel"),
                    Description = FindNamedComponent<Text>(created.Transform, "EventDescriptionLabel"),
                    FollowButton = FindNamedComponent<Button>(created.Transform, "EventFollowButton")
                };

                if (row.FollowButton != null)
                {
                    row.FollowLabel = FindNamedComponent<Text>(row.FollowButton.transform, "Label");
                }

                row.Root.IsActive = false;
                _eventRows.Add(row);
            }
        }

        private void HideEventRows()
        {
            for (int i = 0; i < _eventRows.Count; i++)
            {
                EventRowView row = _eventRows[i];
                if (row.Root != null)
                {
                    row.Root.IsActive = false;
                }
            }
        }

        private void ConfigureEventRow(EventRowView row, HistoryEntry entry)
        {
            if (row == null || entry == null)
            {
                return;
            }

            string title = string.IsNullOrWhiteSpace(entry.Title) ? Localize("historybook_timeline_header", "Timeline") : entry.Title;
            string date = Date.getDate(entry.Timestamp);
            string description = string.IsNullOrWhiteSpace(entry.Description) ? Localize("historybook_empty_timeline", "No timeline events yet.") : entry.Description;
            if (!string.IsNullOrWhiteSpace(entry.LocationHint))
            {
                description = description + "\n" + entry.LocationHint;
            }

            if (row.Title != null)
            {
                row.Title.text = title;
            }

            if (row.Date != null)
            {
                row.Date.text = date;
            }

            if (row.Description != null)
            {
                row.Description.text = description;
            }

            if (row.FollowButton != null)
            {
                bool canFollow = entry.RelatedUnitId.HasValue && entry.RelatedUnitId.Value >= 0;
                row.FollowButton.gameObject.SetActive(canFollow);
                row.FollowButton.onClick.RemoveAllListeners();
                if (canFollow)
                {
                    long relatedId = entry.RelatedUnitId.Value;
                    row.FollowButton.onClick.AddListener(() => _manager?.FocusOnUnit(relatedId));
                }
            }

            if (row.FollowLabel != null)
            {
                row.FollowLabel.text = Localize("historybook_follow_relative", "Follow");
            }
        }

        private void DeleteSelectedRecord()
        {
            if (_selectedUnitId < 0 || _manager == null)
            {
                return;
            }

            _manager.DeleteRecord(_selectedUnitId);
            _selectedUnitId = -1;
            WorldTip.showNow("historybook_record_deleted");
            Refresh();
        }

        private void ExportSelectedRecord()
        {
            if (_selectedUnitId < 0 || _manager == null)
            {
                return;
            }

            string path = _manager.ExportRecord(_selectedUnitId);
            if (!string.IsNullOrEmpty(path))
            {
                WorldTip.showNow($"History exported to {path}", false);
            }
            else
            {
                WorldTip.showNow("Failed to export history.", false);
            }
        }

        private void DeleteAllHistories()
        {
            if (_manager == null)
            {
                return;
            }

            _manager.DeleteAll();
            _selectedUnitId = -1;
            WorldTip.showNow("historybook_histories_deleted");
            Refresh();
        }

        private string BuildUnitLabel(HistoryRecord record)
        {
            if (record == null)
            {
                return string.Empty;
            }

            string name = string.IsNullOrWhiteSpace(record.Name) ? $"Unit {record.UnitId}" : record.Name;
            string species = string.IsNullOrWhiteSpace(record.SpeciesName) ? "Unknown" : record.SpeciesName;
            return $"{name} ({species})";
        }

        private string BuildTimelineTitle(HistoryRecord record)
        {
            if (record == null)
            {
                return Localize("historybook_timeline_header", "Timeline");
            }

            string name = string.IsNullOrWhiteSpace(record.Name) ? $"Unit {record.UnitId}" : record.Name;
            if (string.IsNullOrWhiteSpace(record.SpeciesName))
            {
                return name;
            }

            return $"{name} - {record.SpeciesName}";
        }

        private string Localize(string key, string fallback)
        {
            string value = LocalizedTextManager.getText(key);
            if (string.IsNullOrEmpty(value) || value == key)
            {
                return fallback;
            }

            return value;
        }

        private static Text ResolveElementTextComponent(XwmElementRef element)
        {
            if (element == null || element.GameObject == null)
            {
                return null;
            }

            if (element.Text != null)
            {
                return element.Text;
            }

            return element.GameObject.GetComponentInChildren<Text>(true);
        }

        private static T FindNamedComponent<T>(Transform root, string name) where T : Component
        {
            if (root == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            T[] components = root.GetComponentsInChildren<T>(true);
            for (int i = 0; i < components.Length; i++)
            {
                T component = components[i];
                if (component == null)
                {
                    continue;
                }

                if (string.Equals(component.gameObject.name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return component;
                }
            }

            Transform target = FindNamedTransform(root, name);
            if (target == null)
            {
                return null;
            }

            T direct = target.GetComponent<T>();
            if (direct != null)
            {
                return direct;
            }

            return target.GetComponentInChildren<T>(true);
        }

        private static Transform FindNamedTransform(Transform root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (string.Equals(root.gameObject.name, name, StringComparison.OrdinalIgnoreCase))
            {
                return root;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                Transform result = FindNamedTransform(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void OnDestroy()
        {
            if (_manager != null)
            {
                _manager.OnHistoryChanged -= Refresh;
            }

            if (_windowHandle != null && !_windowHandle.IsDestroyed)
            {
                _windowHandle.Destroy();
            }

            if (_toggleButton != null)
            {
                Destroy(_toggleButton.gameObject);
            }

            _unitRows.Clear();
            _eventRows.Clear();
        }
    }
}
