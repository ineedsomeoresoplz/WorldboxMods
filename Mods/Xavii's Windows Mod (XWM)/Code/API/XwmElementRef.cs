using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using XaviiWindowsMod.Xwm;

namespace XaviiWindowsMod.API
{
    public class XwmElementRef
    {
        internal XwmElementRef(string id, string name, string type, GameObject gameObject, RectTransform rectTransform, Image image, Text text, Button button, InputField inputField, XwmPointerRelay pointerRelay)
        {
            Id = id;
            Name = name;
            Type = type;
            GameObject = gameObject;
            RectTransform = rectTransform;
            Image = image;
            Text = text;
            Button = button;
            InputField = inputField;
            PointerRelay = pointerRelay;
        }

        public string Id { get; }
        public string Name { get; internal set; }
        public string Type { get; }
        public GameObject GameObject { get; }
        public RectTransform RectTransform { get; }
        public Image Image { get; }
        public Text Text { get; }
        public Button Button { get; }
        public InputField InputField { get; }
        internal XwmPointerRelay PointerRelay { get; }
        public Transform Transform => GameObject != null ? GameObject.transform : null;
        public RectTransform ParentRectTransform => RectTransform != null ? RectTransform.parent as RectTransform : null;
        public bool HasRectTransform => RectTransform != null;
        public bool HasImage => Image != null;
        public bool HasText => Text != null;
        public bool HasButton => Button != null;
        public bool HasInputField => InputField != null;
        public bool HasPointerRelay => PointerRelay != null;
        public int ChildCount => Transform != null ? Transform.childCount : 0;

        public bool Valid => GameObject != null;

        public bool IsActive
        {
            get => GameObject != null && GameObject.activeSelf;
            set
            {
                if (GameObject != null)
                {
                    GameObject.SetActive(value);
                }
            }
        }

        public bool IsVisibleInHierarchy => GameObject != null && GameObject.activeInHierarchy;

        public Vector2 Position
        {
            get
            {
                if (RectTransform == null)
                {
                    return Vector2.zero;
                }

                return new Vector2(RectTransform.anchoredPosition.x, -RectTransform.anchoredPosition.y);
            }
            set
            {
                if (RectTransform != null)
                {
                    RectTransform.anchoredPosition = new Vector2(value.x, -value.y);
                }
            }
        }

        public float X
        {
            get => Position.x;
            set
            {
                Vector2 current = Position;
                Position = new Vector2(value, current.y);
            }
        }

        public float Y
        {
            get => Position.y;
            set
            {
                Vector2 current = Position;
                Position = new Vector2(current.x, value);
            }
        }

        public Vector2 Size
        {
            get => RectTransform != null ? RectTransform.sizeDelta : Vector2.zero;
            set
            {
                if (RectTransform != null)
                {
                    RectTransform.sizeDelta = value;
                }
            }
        }

        public float Width
        {
            get => Size.x;
            set
            {
                Vector2 size = Size;
                size.x = value;
                Size = size;
            }
        }

        public float Height
        {
            get => Size.y;
            set
            {
                Vector2 size = Size;
                size.y = value;
                Size = size;
            }
        }

        public Vector2 AnchorMin => RectTransform != null ? RectTransform.anchorMin : Vector2.zero;
        public Vector2 AnchorMax => RectTransform != null ? RectTransform.anchorMax : Vector2.zero;
        public Vector2 Pivot => RectTransform != null ? RectTransform.pivot : new Vector2(0.5f, 0.5f);
        public float Rotation => RectTransform != null ? RectTransform.localEulerAngles.z : 0f;
        public Vector2 Scale => RectTransform != null ? new Vector2(RectTransform.localScale.x, RectTransform.localScale.y) : Vector2.one;
        public string HierarchyPath => BuildHierarchyPath();

        public string GetText()
        {
            if (InputField != null)
            {
                return InputField.text;
            }

            if (Text != null)
            {
                return Text.text;
            }

            return string.Empty;
        }

        public bool HasTextValue()
        {
            return !string.IsNullOrEmpty(GetText());
        }

        public void SetText(string value)
        {
            string resolved = value ?? string.Empty;
            if (InputField != null)
            {
                InputField.text = resolved;
            }

            if (Text != null)
            {
                Text.text = resolved;
            }
        }

        public void AppendText(string suffix)
        {
            string current = GetText();
            SetText(current + (suffix ?? string.Empty));
        }

        public void PrependText(string prefix)
        {
            string current = GetText();
            SetText((prefix ?? string.Empty) + current);
        }

        public void ClearText()
        {
            SetText(string.Empty);
        }

        public int GetFontSize()
        {
            Text primary = ResolvePrimaryText();
            return primary != null ? primary.fontSize : 0;
        }

        public void SetFontSize(int size)
        {
            int resolved = Mathf.Max(1, size);
            Text primary = ResolvePrimaryText();
            if (primary != null)
            {
                primary.fontSize = resolved;
            }

            Text placeholder = ResolvePlaceholderText();
            if (placeholder != null && placeholder != primary)
            {
                placeholder.fontSize = resolved;
            }

            if (Text != null && Text != primary && Text != placeholder)
            {
                Text.fontSize = resolved;
            }
        }

        public string GetFontType()
        {
            Text primary = ResolvePrimaryText();
            return primary != null && primary.font != null ? primary.font.name : string.Empty;
        }

        public void SetFontType(string fontType)
        {
            Font resolved = XwmUiBootstrap.ResolveFont(fontType, XwmUiBootstrap.DefaultFont);
            if (resolved == null)
            {
                return;
            }

            Text primary = ResolvePrimaryText();
            if (primary != null)
            {
                primary.font = resolved;
            }

            Text placeholder = ResolvePlaceholderText();
            if (placeholder != null && placeholder != primary)
            {
                placeholder.font = resolved;
            }

            if (Text != null && Text != primary && Text != placeholder)
            {
                Text.font = resolved;
            }
        }

        public bool GetTextScaled()
        {
            Text primary = ResolvePrimaryText();
            return primary != null && primary.resizeTextForBestFit;
        }

        public void SetTextScaled(bool scaled)
        {
            Text primary = ResolvePrimaryText();
            if (primary != null)
            {
                primary.resizeTextForBestFit = scaled;
                primary.resizeTextMinSize = 1;
                primary.resizeTextMaxSize = 512;
            }

            Text placeholder = ResolvePlaceholderText();
            if (placeholder != null && placeholder != primary)
            {
                placeholder.resizeTextForBestFit = scaled;
                placeholder.resizeTextMinSize = 1;
                placeholder.resizeTextMaxSize = 512;
            }

            if (Text != null && Text != primary && Text != placeholder)
            {
                Text.resizeTextForBestFit = scaled;
                Text.resizeTextMinSize = 1;
                Text.resizeTextMaxSize = 512;
            }
        }

        public bool GetTextWrapped()
        {
            Text primary = ResolvePrimaryText();
            return primary != null && primary.horizontalOverflow == HorizontalWrapMode.Wrap;
        }

        public void SetTextWrapped(bool wrapped)
        {
            HorizontalWrapMode mode = wrapped ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            Text primary = ResolvePrimaryText();
            if (primary != null)
            {
                primary.horizontalOverflow = mode;
            }

            Text placeholder = ResolvePlaceholderText();
            if (placeholder != null && placeholder != primary)
            {
                placeholder.horizontalOverflow = mode;
            }

            if (Text != null && Text != primary && Text != placeholder)
            {
                Text.horizontalOverflow = mode;
            }

            if (InputField != null)
            {
                InputField.lineType = wrapped ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine;
            }
        }

        public string GetPlaceholder()
        {
            if (InputField == null || InputField.placeholder == null)
            {
                return string.Empty;
            }

            Text placeholderText = InputField.placeholder as Text;
            return placeholderText != null ? placeholderText.text : string.Empty;
        }

        public void SetPlaceholder(string value)
        {
            if (InputField == null || InputField.placeholder == null)
            {
                return;
            }

            Text placeholderText = InputField.placeholder as Text;
            if (placeholderText != null)
            {
                placeholderText.text = value ?? string.Empty;
            }
        }

        public void SetColor(Color color)
        {
            if (Image != null)
            {
                Image.color = color;
            }

            if (Text != null)
            {
                Text.color = color;
            }
        }

        public Color GetColor()
        {
            if (Image != null)
            {
                return Image.color;
            }

            if (Text != null)
            {
                return Text.color;
            }

            return Color.white;
        }

        public float GetAlpha()
        {
            return GetColor().a;
        }

        public void SetAlpha(float alpha)
        {
            float resolved = Mathf.Clamp01(alpha);
            if (Image != null)
            {
                Color imageColor = Image.color;
                imageColor.a = resolved;
                Image.color = imageColor;
            }

            if (Text != null)
            {
                Color textColor = Text.color;
                textColor.a = resolved;
                Text.color = textColor;
            }
        }

        public void SetTextColor(Color color)
        {
            if (InputField != null && InputField.textComponent != null)
            {
                InputField.textComponent.color = color;
            }

            if (Text != null)
            {
                Text.color = color;
            }
        }

        public void SetImageColor(Color color)
        {
            if (Image != null)
            {
                Image.color = color;
            }
        }

        public void SetColor(string color)
        {
            Color parsed = XwmPropertyUtility.ParseColor(color, Color.white);
            SetColor(parsed);
        }

        public Sprite GetSprite()
        {
            return Image != null ? Image.sprite : null;
        }

        public void SetSprite(Sprite sprite)
        {
            if (Image != null)
            {
                Image.sprite = sprite;
            }
        }

        public void SetSprite(string resourcePath)
        {
            if (Image == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                Image.sprite = null;
                return;
            }

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite != null)
            {
                Image.sprite = sprite;
            }
        }

        public bool TrySetSprite(string resourcePath)
        {
            if (Image == null || string.IsNullOrWhiteSpace(resourcePath))
            {
                return false;
            }

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                return false;
            }

            Image.sprite = sprite;
            return true;
        }

        public void SetInteractable(bool interactable)
        {
            if (Button != null)
            {
                Button.interactable = interactable;
            }

            if (InputField != null)
            {
                InputField.interactable = interactable;
            }
        }

        public bool GetInteractable()
        {
            if (Button != null)
            {
                return Button.interactable;
            }

            if (InputField != null)
            {
                return InputField.interactable;
            }

            return false;
        }

        public void SetRaycastTarget(bool enabled)
        {
            if (Image != null)
            {
                Image.raycastTarget = enabled;
            }

            if (Text != null)
            {
                Text.raycastTarget = enabled;
            }

            if (InputField != null && InputField.textComponent != null)
            {
                InputField.textComponent.raycastTarget = enabled;
            }
        }

        public bool GetRaycastTarget()
        {
            if (Image != null)
            {
                return Image.raycastTarget;
            }

            if (Text != null)
            {
                return Text.raycastTarget;
            }

            if (InputField != null && InputField.textComponent != null)
            {
                return InputField.textComponent.raycastTarget;
            }

            return false;
        }

        public int GetLayer()
        {
            if (RectTransform == null)
            {
                return 0;
            }

            return RectTransform.GetSiblingIndex();
        }

        public void SetLayer(int layer)
        {
            if (RectTransform == null || RectTransform.parent == null)
            {
                return;
            }

            int maxIndex = RectTransform.parent.childCount - 1;
            int resolved = Mathf.Clamp(layer, 0, Mathf.Max(0, maxIndex));
            RectTransform.SetSiblingIndex(resolved);
        }

        public void SetAnchors(Vector2 min, Vector2 max)
        {
            if (RectTransform != null)
            {
                RectTransform.anchorMin = min;
                RectTransform.anchorMax = max;
            }
        }

        public void SetPivot(Vector2 pivot)
        {
            if (RectTransform != null)
            {
                RectTransform.pivot = pivot;
            }
        }

        public void SetRotation(float rotation)
        {
            if (RectTransform != null)
            {
                RectTransform.localEulerAngles = new Vector3(0f, 0f, rotation);
            }
        }

        public void SetScale(Vector2 scale)
        {
            if (RectTransform != null)
            {
                RectTransform.localScale = new Vector3(scale.x, scale.y, 1f);
            }
        }

        public void MoveBy(Vector2 delta)
        {
            Position = Position + delta;
        }

        public void ResizeBy(Vector2 delta)
        {
            Size = Size + delta;
        }

        public void SetOffsets(Vector2 min, Vector2 max)
        {
            if (RectTransform == null)
            {
                return;
            }

            RectTransform.offsetMin = min;
            RectTransform.offsetMax = max;
        }

        public void Stretch(float left, float right, float top, float bottom)
        {
            if (RectTransform == null)
            {
                return;
            }

            RectTransform.anchorMin = new Vector2(0f, 0f);
            RectTransform.anchorMax = new Vector2(1f, 1f);
            RectTransform.offsetMin = new Vector2(left, bottom);
            RectTransform.offsetMax = new Vector2(-right, -top);
        }

        public Rect GetLocalRect()
        {
            return RectTransform != null ? RectTransform.rect : new Rect();
        }

        public Rect GetWorldRect()
        {
            if (RectTransform == null)
            {
                return new Rect();
            }

            Vector3[] corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            float minX = corners[0].x;
            float minY = corners[0].y;
            float maxX = corners[2].x;
            float maxY = corners[2].y;
            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }

        public void SetAsFirstSibling()
        {
            if (RectTransform != null)
            {
                RectTransform.SetAsFirstSibling();
            }
        }

        public void SetAsLastSibling()
        {
            if (RectTransform != null)
            {
                RectTransform.SetAsLastSibling();
            }
        }

        public void Focus()
        {
            if (InputField == null)
            {
                return;
            }

            InputField.ActivateInputField();
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(InputField.gameObject);
            }
        }

        public void Blur()
        {
            if (InputField == null)
            {
                return;
            }

            InputField.DeactivateInputField();
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == InputField.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void SetCharacterLimit(int limit)
        {
            if (InputField != null)
            {
                InputField.characterLimit = Mathf.Max(0, limit);
            }
        }

        public int GetCharacterLimit()
        {
            return InputField != null ? InputField.characterLimit : 0;
        }

        public bool SetPage(int page)
        {
            XwmPageLayoutComponent component = ResolveComponentFromElementOrHost<XwmPageLayoutComponent>();
            if (component == null)
            {
                return false;
            }

            component.SetPage(page);
            return true;
        }

        public bool NextPage()
        {
            XwmPageLayoutComponent component = ResolveComponentFromElementOrHost<XwmPageLayoutComponent>();
            if (component == null)
            {
                return false;
            }

            component.NextPage();
            return true;
        }

        public bool PreviousPage()
        {
            XwmPageLayoutComponent component = ResolveComponentFromElementOrHost<XwmPageLayoutComponent>();
            if (component == null)
            {
                return false;
            }

            component.PreviousPage();
            return true;
        }

        public int GetCurrentPage()
        {
            XwmPageLayoutComponent component = ResolveComponentFromElementOrHost<XwmPageLayoutComponent>();
            return component != null ? component.CurrentPage : 0;
        }

        public void ConnectDrag(Action<Vector2> callback)
        {
            if (callback == null || GameObject == null)
            {
                return;
            }

            XwmRuntimeDragDetector detector = GameObject.GetComponent<XwmRuntimeDragDetector>();
            if (detector == null && RectTransform != null)
            {
                detector = RectTransform.GetComponentInParent<XwmRuntimeDragDetector>();
            }

            if (detector != null)
            {
                detector.PositionChanged += callback;
            }
        }

        public void ConnectClick(UnityAction callback)
        {
            if (callback == null)
            {
                return;
            }

            if (Button != null)
            {
                Button.onClick.AddListener(callback);
                return;
            }

            if (PointerRelay != null)
            {
                PointerRelay.PointerClicked += _ => callback.Invoke();
            }
        }

        public void ConnectTyping(UnityAction<string> callback)
        {
            if (InputField != null && callback != null)
            {
                InputField.onValueChanged.AddListener(callback);
            }
        }

        public void ConnectSubmit(UnityAction<string> callback)
        {
            if (InputField != null && callback != null)
            {
                InputField.onEndEdit.AddListener(callback);
            }
        }

        public void ConnectPointerClick(Action<PointerEventData> callback)
        {
            if (PointerRelay != null && callback != null)
            {
                PointerRelay.PointerClicked += callback;
            }
        }

        public void ConnectPointerEnter(Action<PointerEventData> callback)
        {
            if (PointerRelay != null && callback != null)
            {
                PointerRelay.PointerEntered += callback;
            }
        }

        public void ConnectPointerExit(Action<PointerEventData> callback)
        {
            if (PointerRelay != null && callback != null)
            {
                PointerRelay.PointerExited += callback;
            }
        }

        public void ConnectPointerDown(Action<PointerEventData> callback)
        {
            if (PointerRelay != null && callback != null)
            {
                PointerRelay.PointerDown += callback;
            }
        }

        public void ConnectPointerUp(Action<PointerEventData> callback)
        {
            if (PointerRelay != null && callback != null)
            {
                PointerRelay.PointerUp += callback;
            }
        }

        public bool IsType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            return string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return string.Equals(Id, id, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return string.Equals(Name, name, StringComparison.OrdinalIgnoreCase);
        }

        public bool Matches(string idOrName)
        {
            return IsId(idOrName) || IsName(idOrName);
        }

        public bool HasComponent<T>() where T : Component
        {
            return GetComponent<T>() != null;
        }

        public T GetComponent<T>() where T : Component
        {
            return GameObject != null ? GameObject.GetComponent<T>() : null;
        }

        public T[] GetComponents<T>() where T : Component
        {
            if (GameObject == null)
            {
                return new T[0];
            }

            return GameObject.GetComponents<T>();
        }

        public T[] GetComponentsInChildren<T>(bool includeInactive = false) where T : Component
        {
            if (GameObject == null)
            {
                return new T[0];
            }

            return GameObject.GetComponentsInChildren<T>(includeInactive);
        }

        public T[] GetComponentsInParent<T>(bool includeInactive = false) where T : Component
        {
            if (GameObject == null)
            {
                return new T[0];
            }

            return GameObject.GetComponentsInParent<T>(includeInactive);
        }

        public T AddOrGetComponent<T>() where T : Component
        {
            if (GameObject == null)
            {
                return null;
            }

            T component = GameObject.GetComponent<T>();
            if (component == null)
            {
                component = GameObject.AddComponent<T>();
            }

            return component;
        }

        public bool RemoveComponent<T>() where T : Component
        {
            if (GameObject == null)
            {
                return false;
            }

            T component = GameObject.GetComponent<T>();
            if (component == null)
            {
                return false;
            }

            UnityEngine.Object.Destroy(component);
            return true;
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            if (Transform != null)
            {
                Transform.SetParent(parent, worldPositionStays);
            }
        }

        public GameObject GetChild(int index)
        {
            if (Transform == null || index < 0 || index >= Transform.childCount)
            {
                return null;
            }

            return Transform.GetChild(index).gameObject;
        }

        public List<GameObject> GetChildren(bool includeInactive = true)
        {
            List<GameObject> output = new List<GameObject>();
            if (Transform == null)
            {
                return output;
            }

            for (int i = 0; i < Transform.childCount; i++)
            {
                Transform child = Transform.GetChild(i);
                if (child == null)
                {
                    continue;
                }

                if (!includeInactive && !child.gameObject.activeInHierarchy)
                {
                    continue;
                }

                output.Add(child.gameObject);
            }

            return output;
        }

        public bool IsChildOf(Transform potentialParent)
        {
            return Transform != null && potentialParent != null && Transform.IsChildOf(potentialParent);
        }

        public void DestroySelf()
        {
            if (GameObject != null)
            {
                UnityEngine.Object.Destroy(GameObject);
            }
        }

        private Text ResolvePrimaryText()
        {
            if (InputField != null && InputField.textComponent != null)
            {
                return InputField.textComponent;
            }

            return Text;
        }

        private Text ResolvePlaceholderText()
        {
            if (InputField == null || InputField.placeholder == null)
            {
                return null;
            }

            return InputField.placeholder as Text;
        }

        private string BuildHierarchyPath()
        {
            if (Transform == null)
            {
                return string.Empty;
            }

            List<string> names = new List<string>();
            Transform current = Transform;
            while (current != null)
            {
                names.Add(current.name);
                current = current.parent;
            }

            names.Reverse();
            return string.Join("/", names);
        }

        private T ResolveComponentFromElementOrHost<T>() where T : Component
        {
            if (GameObject == null)
            {
                return null;
            }

            T component = GameObject.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            if (!XwmTypeLibrary.IsHelperType(Type))
            {
                return null;
            }

            if (RectTransform != null && RectTransform.parent != null)
            {
                component = RectTransform.parent.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}
