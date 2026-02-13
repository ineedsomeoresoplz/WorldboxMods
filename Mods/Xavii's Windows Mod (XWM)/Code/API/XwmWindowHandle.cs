using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using XaviiWindowsMod.Xwm;

namespace XaviiWindowsMod.API
{
    public class XwmWindowHandle
    {
        private sealed class ClipboardElementSnapshot
        {
            public string RelativePath;
            public string Id;
            public string Name;
            public string Type;
        }

        private sealed class KeyComboBinding
        {
            public KeyCode Trigger;
            public KeyCode[] Held;
            public Action Callback;
        }

        private static GameObject _clipboardTemplate;
        private static bool _clipboardRootActive;
        private static List<ClipboardElementSnapshot> _clipboardSnapshots;
        private static readonly Vector2 DefaultCopyOffset = new Vector2(20f, 20f);

        private readonly List<XwmElementRef> _elements;
        private readonly Dictionary<string, XwmElementRef> _elementsById;
        private readonly Dictionary<string, List<XwmElementRef>> _elementsByName;
        private readonly Dictionary<string, List<XwmElementRef>> _elementsByType;
        private readonly Dictionary<KeyCode, Action> _keyDownHandlers;
        private readonly Dictionary<KeyCode, Action> _keyUpHandlers;
        private readonly Dictionary<KeyCode, Action> _keyHoldHandlers;
        private readonly List<KeyComboBinding> _keyCombos;
        private readonly XwmKeyDispatcher _keyDispatcher;
        private CanvasGroup _rootCanvasGroup;
        private bool _destroyed;

        internal XwmWindowHandle(string runtimeId, string modTarget, string fileName, GameObject rootObject, RectTransform rootRect, List<XwmElementRef> elements)
        {
            RuntimeId = runtimeId;
            ModTarget = modTarget;
            FileName = fileName;
            RootObject = rootObject;
            RootRect = rootRect;
            _elements = elements ?? new List<XwmElementRef>();
            _elementsById = new Dictionary<string, XwmElementRef>(StringComparer.OrdinalIgnoreCase);
            _elementsByName = new Dictionary<string, List<XwmElementRef>>(StringComparer.OrdinalIgnoreCase);
            _elementsByType = new Dictionary<string, List<XwmElementRef>>(StringComparer.OrdinalIgnoreCase);
            _keyDownHandlers = new Dictionary<KeyCode, Action>();
            _keyUpHandlers = new Dictionary<KeyCode, Action>();
            _keyHoldHandlers = new Dictionary<KeyCode, Action>();
            _keyCombos = new List<KeyComboBinding>();

            for (int i = 0; i < _elements.Count; i++)
            {
                Register(_elements[i]);
            }

            if (RootObject != null)
            {
                _keyDispatcher = RootObject.GetComponent<XwmKeyDispatcher>();
                if (_keyDispatcher == null)
                {
                    _keyDispatcher = RootObject.AddComponent<XwmKeyDispatcher>();
                }

                _keyDispatcher.Tick = TickInput;
                _rootCanvasGroup = RootObject.GetComponent<CanvasGroup>();
                if (_rootCanvasGroup == null)
                {
                    _rootCanvasGroup = RootObject.AddComponent<CanvasGroup>();
                }
            }
        }

        public string RuntimeId { get; }
        public string ModTarget { get; }
        public string FileName { get; }
        public GameObject RootObject { get; }
        public RectTransform RootRect { get; }
        public IReadOnlyList<XwmElementRef> Elements => _elements;
        public int ElementCount => _elements.Count;
        public bool IsVisible => RootObject != null && RootObject.activeSelf;
        public bool IsDestroyed => _destroyed || RootObject == null;
        public XwmElementRef RootElement => _elements.Count > 0 ? _elements[0] : null;

        public Vector2 Position
        {
            get => RootRect != null ? new Vector2(RootRect.anchoredPosition.x, -RootRect.anchoredPosition.y) : Vector2.zero;
            set
            {
                if (RootRect != null)
                {
                    RootRect.anchoredPosition = new Vector2(value.x, -value.y);
                }
            }
        }

        public Vector2 Size
        {
            get => RootRect != null ? RootRect.sizeDelta : Vector2.zero;
            set
            {
                if (RootRect != null)
                {
                    RootRect.sizeDelta = value;
                }
            }
        }

        public float Opacity
        {
            get => ResolveCanvasGroup() != null ? ResolveCanvasGroup().alpha : 1f;
            set
            {
                CanvasGroup group = ResolveCanvasGroup();
                if (group != null)
                {
                    group.alpha = Mathf.Clamp01(value);
                }
            }
        }

        public bool BlocksRaycasts
        {
            get => ResolveCanvasGroup() != null && ResolveCanvasGroup().blocksRaycasts;
            set
            {
                CanvasGroup group = ResolveCanvasGroup();
                if (group != null)
                {
                    group.blocksRaycasts = value;
                }
            }
        }

        public bool Interactable
        {
            get => ResolveCanvasGroup() != null && ResolveCanvasGroup().interactable;
            set
            {
                CanvasGroup group = ResolveCanvasGroup();
                if (group != null)
                {
                    group.interactable = value;
                }
            }
        }

        public void Show()
        {
            if (_destroyed || RootObject == null)
            {
                return;
            }

            RootObject.SetActive(true);
            RootObject.transform.SetAsLastSibling();
        }

        public void Hide()
        {
            if (_destroyed || RootObject == null)
            {
                return;
            }

            RootObject.SetActive(false);
        }

        public bool Toggle()
        {
            if (_destroyed || RootObject == null)
            {
                return false;
            }

            if (RootObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }

            return RootObject.activeSelf;
        }

        public void BringToFront()
        {
            if (RootRect != null)
            {
                RootRect.SetAsLastSibling();
            }
        }

        public void SendToBack()
        {
            if (RootRect != null)
            {
                RootRect.SetAsFirstSibling();
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

        public void SetScale(Vector2 scale)
        {
            if (RootRect != null)
            {
                RootRect.localScale = new Vector3(scale.x, scale.y, 1f);
            }
        }

        public void SetRotation(float rotation)
        {
            if (RootRect != null)
            {
                RootRect.localEulerAngles = new Vector3(0f, 0f, rotation);
            }
        }

        public void Destroy()
        {
            if (_destroyed)
            {
                return;
            }

            _destroyed = true;
            if (_keyDispatcher != null)
            {
                _keyDispatcher.Tick = null;
            }

            _elements.Clear();
            _elementsById.Clear();
            _elementsByName.Clear();
            _elementsByType.Clear();
            _keyDownHandlers.Clear();
            _keyUpHandlers.Clear();
            _keyHoldHandlers.Clear();
            _keyCombos.Clear();

            if (RootObject != null)
            {
                UnityEngine.Object.Destroy(RootObject);
            }

            if (WindowService.Instance != null)
            {
                WindowService.Instance.UnregisterRuntime(RuntimeId);
            }
        }

        public XwmElementRef Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
            {
                return null;
            }

            if (_elementsById.TryGetValue(idOrName, out XwmElementRef idMatch))
            {
                return idMatch;
            }

            if (_elementsByName.TryGetValue(idOrName, out List<XwmElementRef> byName) && byName.Count > 0)
            {
                return byName[0];
            }

            return null;
        }

        public bool TryGet(string idOrName, out XwmElementRef element)
        {
            element = Get(idOrName);
            return element != null;
        }

        public bool Contains(string idOrName)
        {
            return Get(idOrName) != null;
        }

        public List<XwmElementRef> GetAll(string idOrName)
        {
            List<XwmElementRef> output = new List<XwmElementRef>();
            if (string.IsNullOrWhiteSpace(idOrName))
            {
                return output;
            }

            if (_elementsById.TryGetValue(idOrName, out XwmElementRef idMatch))
            {
                output.Add(idMatch);
                return output;
            }

            if (_elementsByName.TryGetValue(idOrName, out List<XwmElementRef> nameMatches))
            {
                output.AddRange(nameMatches);
            }

            return output;
        }

        public List<XwmElementRef> GetAllByType(string type)
        {
            List<XwmElementRef> output = new List<XwmElementRef>();
            if (string.IsNullOrWhiteSpace(type))
            {
                return output;
            }

            if (_elementsByType.TryGetValue(type, out List<XwmElementRef> indexed))
            {
                output.AddRange(indexed);
            }

            return output;
        }

        public List<XwmElementRef> Query(Func<XwmElementRef, bool> predicate)
        {
            List<XwmElementRef> output = new List<XwmElementRef>();
            if (predicate == null)
            {
                return output;
            }

            for (int i = 0; i < _elements.Count; i++)
            {
                XwmElementRef element = _elements[i];
                if (element != null && predicate(element))
                {
                    output.Add(element);
                }
            }

            return output;
        }

        public XwmElementRef Find(string selector)
        {
            List<XwmElementRef> all = FindAll(selector);
            return all.Count > 0 ? all[0] : null;
        }

        public List<XwmElementRef> FindAll(string selector)
        {
            List<XwmElementRef> output = new List<XwmElementRef>();
            if (string.IsNullOrWhiteSpace(selector))
            {
                return output;
            }

            for (int i = 0; i < _elements.Count; i++)
            {
                XwmElementRef element = _elements[i];
                if (element != null && XwmElementSelector.Matches(element, selector))
                {
                    output.Add(element);
                }
            }

            return output;
        }

        public bool Rename(string idOrName, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return false;
            }

            XwmElementRef element = Get(idOrName);
            if (element == null)
            {
                return false;
            }

            string oldName = element.Name;
            if (!string.IsNullOrWhiteSpace(oldName) && _elementsByName.TryGetValue(oldName, out List<XwmElementRef> oldList))
            {
                oldList.Remove(element);
                if (oldList.Count == 0)
                {
                    _elementsByName.Remove(oldName);
                }
            }

            element.Name = newName;
            if (element.GameObject != null)
            {
                element.GameObject.name = newName;
            }

            if (!_elementsByName.TryGetValue(newName, out List<XwmElementRef> list))
            {
                list = new List<XwmElementRef>();
                _elementsByName[newName] = list;
            }

            if (!list.Contains(element))
            {
                list.Add(element);
            }

            return true;
        }

        public int ForEach(Action<XwmElementRef> callback)
        {
            if (callback == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < _elements.Count; i++)
            {
                XwmElementRef element = _elements[i];
                if (element == null)
                {
                    continue;
                }

                callback(element);
                count++;
            }

            return count;
        }

        public static bool HasClipboard()
        {
            return _clipboardTemplate != null && _clipboardSnapshots != null && _clipboardSnapshots.Count > 0;
        }

        public static void ClearClipboard()
        {
            if (_clipboardTemplate != null)
            {
                UnityEngine.Object.Destroy(_clipboardTemplate);
                _clipboardTemplate = null;
            }

            _clipboardSnapshots = null;
            _clipboardRootActive = false;
        }

        public bool Copy(string idOrName)
        {
            if (_destroyed || RootObject == null)
            {
                return false;
            }

            XwmElementRef source = Get(idOrName);
            if (source == null || source.GameObject == null || source.Transform == null)
            {
                return false;
            }

            List<ClipboardElementSnapshot> snapshots = CaptureSnapshots(source);
            if (snapshots.Count == 0)
            {
                return false;
            }

            GameObject template = UnityEngine.Object.Instantiate(source.GameObject);
            template.transform.SetParent(null, false);
            template.hideFlags = HideFlags.HideAndDontSave;
            template.SetActive(false);

            ClearClipboard();
            _clipboardTemplate = template;
            _clipboardSnapshots = snapshots;
            _clipboardRootActive = source.GameObject.activeSelf;
            return true;
        }

        public XwmElementRef Paste(string parentIdOrName = null)
        {
            return Paste(parentIdOrName, DefaultCopyOffset);
        }

        public XwmElementRef Paste(string parentIdOrName, Vector2 offset)
        {
            if (_destroyed || RootObject == null || !HasClipboard())
            {
                return null;
            }

            RectTransform parent = ResolveParentRect(parentIdOrName);
            if (parent == null)
            {
                return null;
            }

            return CloneSubtree(_clipboardTemplate, _clipboardSnapshots, parent, offset, _clipboardRootActive);
        }

        public XwmElementRef Duplicate(string idOrName)
        {
            return Duplicate(idOrName, DefaultCopyOffset);
        }

        public XwmElementRef Duplicate(string idOrName, Vector2 offset)
        {
            if (_destroyed || RootObject == null)
            {
                return null;
            }

            XwmElementRef source = Get(idOrName);
            if (source == null || source.GameObject == null || source.Transform == null)
            {
                return null;
            }

            RectTransform parent = source.Transform.parent as RectTransform;
            if (parent == null)
            {
                parent = RootRect;
            }

            List<ClipboardElementSnapshot> snapshots = CaptureSnapshots(source);
            if (snapshots.Count == 0)
            {
                return null;
            }

            return CloneSubtree(source.GameObject, snapshots, parent, offset, source.GameObject.activeSelf);
        }

        public bool ConnectButtonClick(string idOrName, UnityAction callback)
        {
            if (callback == null)
            {
                return false;
            }

            List<XwmElementRef> matches = GetAll(idOrName);
            if (matches.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < matches.Count; i++)
            {
                matches[i].ConnectClick(callback);
            }

            return true;
        }

        public bool ConnectTyping(string idOrName, UnityAction<string> callback)
        {
            return ConnectInput(idOrName, callback, (element, action) => element.ConnectTyping(action));
        }

        public bool ConnectSubmit(string idOrName, UnityAction<string> callback)
        {
            return ConnectInput(idOrName, callback, (element, action) => element.ConnectSubmit(action));
        }

        public bool ConnectPointerClick(string idOrName, Action<PointerEventData> callback)
        {
            return ConnectPointer(idOrName, callback, (element, action) => element.ConnectPointerClick(action));
        }

        public bool ConnectPointerEnter(string idOrName, Action<PointerEventData> callback)
        {
            return ConnectPointer(idOrName, callback, (element, action) => element.ConnectPointerEnter(action));
        }

        public bool ConnectPointerExit(string idOrName, Action<PointerEventData> callback)
        {
            return ConnectPointer(idOrName, callback, (element, action) => element.ConnectPointerExit(action));
        }

        public bool ConnectPointerDown(string idOrName, Action<PointerEventData> callback)
        {
            return ConnectPointer(idOrName, callback, (element, action) => element.ConnectPointerDown(action));
        }

        public bool ConnectPointerUp(string idOrName, Action<PointerEventData> callback)
        {
            return ConnectPointer(idOrName, callback, (element, action) => element.ConnectPointerUp(action));
        }

        public bool ConnectDrag(string idOrName, Action<Vector2> callback)
        {
            if (callback == null)
            {
                return false;
            }

            List<XwmElementRef> matches = GetAll(idOrName);
            bool connected = false;
            for (int i = 0; i < matches.Count; i++)
            {
                matches[i].ConnectDrag(callback);
                connected = true;
            }

            return connected;
        }

        public void ConnectKeyDown(KeyCode key, Action callback)
        {
            AddKeyHandler(_keyDownHandlers, key, callback);
        }

        public void ConnectKeyUp(KeyCode key, Action callback)
        {
            AddKeyHandler(_keyUpHandlers, key, callback);
        }

        public void ConnectKeyHeld(KeyCode key, Action callback)
        {
            AddKeyHandler(_keyHoldHandlers, key, callback);
        }

        public void ConnectKeyCombo(KeyCode trigger, Action callback, params KeyCode[] held)
        {
            if (callback == null)
            {
                return;
            }

            _keyCombos.Add(new KeyComboBinding
            {
                Trigger = trigger,
                Held = held == null ? new KeyCode[0] : (KeyCode[])held.Clone(),
                Callback = callback
            });
        }

        public void DisconnectKeyDown(KeyCode key, Action callback)
        {
            RemoveKeyHandler(_keyDownHandlers, key, callback);
        }

        public void DisconnectKeyUp(KeyCode key, Action callback)
        {
            RemoveKeyHandler(_keyUpHandlers, key, callback);
        }

        public void DisconnectKeyHeld(KeyCode key, Action callback)
        {
            RemoveKeyHandler(_keyHoldHandlers, key, callback);
        }

        public void ClearAllKeyBindings()
        {
            _keyDownHandlers.Clear();
            _keyUpHandlers.Clear();
            _keyHoldHandlers.Clear();
            _keyCombos.Clear();
        }

        public bool SetText(string idOrName, string text)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetText(text);
                return true;
            });
        }

        public int SetTextAll(string idOrName, string text)
        {
            return ApplyMany(idOrName, element =>
            {
                element.SetText(text);
                return true;
            });
        }

        public bool SetFontType(string idOrName, string fontType)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetFontType(fontType);
                return true;
            });
        }

        public int SetFontTypeAll(string idOrName, string fontType)
        {
            return ApplyMany(idOrName, element =>
            {
                element.SetFontType(fontType);
                return true;
            });
        }

        public bool SetTextScaled(string idOrName, bool scaled)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetTextScaled(scaled);
                return true;
            });
        }

        public int SetTextScaledAll(string idOrName, bool scaled)
        {
            return ApplyMany(idOrName, element =>
            {
                element.SetTextScaled(scaled);
                return true;
            });
        }

        public bool SetTextWrapped(string idOrName, bool wrapped)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetTextWrapped(wrapped);
                return true;
            });
        }

        public int SetTextWrappedAll(string idOrName, bool wrapped)
        {
            return ApplyMany(idOrName, element =>
            {
                element.SetTextWrapped(wrapped);
                return true;
            });
        }

        public bool SetColor(string idOrName, Color color)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetColor(color);
                return true;
            });
        }

        public bool SetColor(string idOrName, string color)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetColor(color);
                return true;
            });
        }

        public int SetColorAll(string idOrName, Color color)
        {
            return ApplyMany(idOrName, element =>
            {
                element.SetColor(color);
                return true;
            });
        }

        public bool SetSprite(string idOrName, string resourcePath)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetSprite(resourcePath);
                return true;
            });
        }

        public bool SetInteractable(string idOrName, bool interactable)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetInteractable(interactable);
                return true;
            });
        }

        public int SetInteractableAll(string idOrName, bool interactable)
        {
            return ApplyMany(idOrName, element =>
            {
                element.SetInteractable(interactable);
                return true;
            });
        }

        public bool SetLayer(string idOrName, int layer)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetLayer(layer);
                return true;
            });
        }

        public bool SetAnchors(string idOrName, Vector2 min, Vector2 max)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetAnchors(min, max);
                return true;
            });
        }

        public bool SetPivot(string idOrName, Vector2 pivot)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetPivot(pivot);
                return true;
            });
        }

        public bool SetRotation(string idOrName, float rotation)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetRotation(rotation);
                return true;
            });
        }

        public bool SetScale(string idOrName, Vector2 scale)
        {
            return ApplyOne(idOrName, element =>
            {
                element.SetScale(scale);
                return true;
            });
        }

        public bool SetPosition(string idOrName, Vector2 position)
        {
            return ApplyOne(idOrName, element =>
            {
                if (element.RectTransform == null)
                {
                    return false;
                }

                element.Position = position;
                return true;
            });
        }

        public int SetPositionAll(string idOrName, Vector2 position)
        {
            return ApplyMany(idOrName, element =>
            {
                if (element.RectTransform == null)
                {
                    return false;
                }

                element.Position = position;
                return true;
            });
        }

        public bool SetSize(string idOrName, Vector2 size)
        {
            return ApplyOne(idOrName, element =>
            {
                if (element.RectTransform == null)
                {
                    return false;
                }

                element.Size = size;
                return true;
            });
        }

        public int SetSizeAll(string idOrName, Vector2 size)
        {
            return ApplyMany(idOrName, element =>
            {
                if (element.RectTransform == null)
                {
                    return false;
                }

                element.Size = size;
                return true;
            });
        }

        public bool SetActive(string idOrName, bool active)
        {
            return ApplyOne(idOrName, element =>
            {
                if (element.GameObject == null)
                {
                    return false;
                }

                element.IsActive = active;
                return true;
            });
        }

        public int SetActiveAll(string idOrName, bool active)
        {
            return ApplyMany(idOrName, element =>
            {
                if (element.GameObject == null)
                {
                    return false;
                }

                element.IsActive = active;
                return true;
            });
        }

        public bool SetPage(string idOrName, int page)
        {
            return ApplyOne(idOrName, element => element.SetPage(page));
        }

        public bool NextPage(string idOrName)
        {
            return ApplyOne(idOrName, element => element.NextPage());
        }

        public bool PreviousPage(string idOrName)
        {
            return ApplyOne(idOrName, element => element.PreviousPage());
        }

        public int GetCurrentPage(string idOrName)
        {
            XwmElementRef element = Get(idOrName);
            return element != null ? element.GetCurrentPage() : 0;
        }

        internal void TickInput()
        {
            if (_destroyed || RootObject == null || !RootObject.activeInHierarchy)
            {
                return;
            }

            DispatchMap(_keyDownHandlers, Input.GetKeyDown);
            DispatchMap(_keyUpHandlers, Input.GetKeyUp);
            DispatchMap(_keyHoldHandlers, Input.GetKey);

            for (int i = 0; i < _keyCombos.Count; i++)
            {
                KeyComboBinding combo = _keyCombos[i];
                if (combo == null || combo.Callback == null || !Input.GetKeyDown(combo.Trigger))
                {
                    continue;
                }

                if (!AreHeld(combo.Held))
                {
                    continue;
                }

                combo.Callback.Invoke();
            }
        }

        private static bool AreHeld(KeyCode[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return true;
            }

            for (int i = 0; i < keys.Length; i++)
            {
                if (!Input.GetKey(keys[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static void DispatchMap(Dictionary<KeyCode, Action> map, Func<KeyCode, bool> poll)
        {
            if (map == null || map.Count == 0 || poll == null)
            {
                return;
            }

            List<KeyCode> keys = new List<KeyCode>(map.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                KeyCode key = keys[i];
                if (!map.TryGetValue(key, out Action callback) || callback == null)
                {
                    continue;
                }

                if (poll(key))
                {
                    callback.Invoke();
                }
            }
        }

        private void Register(XwmElementRef element)
        {
            if (element == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(element.Id))
            {
                _elementsById[element.Id] = element;
            }

            if (!string.IsNullOrWhiteSpace(element.Name))
            {
                if (!_elementsByName.TryGetValue(element.Name, out List<XwmElementRef> names))
                {
                    names = new List<XwmElementRef>();
                    _elementsByName[element.Name] = names;
                }

                names.Add(element);
            }

            if (!string.IsNullOrWhiteSpace(element.Type))
            {
                if (!_elementsByType.TryGetValue(element.Type, out List<XwmElementRef> types))
                {
                    types = new List<XwmElementRef>();
                    _elementsByType[element.Type] = types;
                }

                types.Add(element);
            }
        }

        private List<ClipboardElementSnapshot> CaptureSnapshots(XwmElementRef source)
        {
            List<ClipboardElementSnapshot> snapshots = new List<ClipboardElementSnapshot>();
            if (source == null || source.Transform == null)
            {
                return snapshots;
            }

            Transform sourceRoot = source.Transform;
            for (int i = 0; i < _elements.Count; i++)
            {
                XwmElementRef element = _elements[i];
                if (element == null || element.Transform == null || element.GameObject == null)
                {
                    continue;
                }

                Transform candidate = element.Transform;
                if (candidate != sourceRoot && !candidate.IsChildOf(sourceRoot))
                {
                    continue;
                }

                string relativePath = BuildRelativePath(sourceRoot, candidate);
                if (relativePath == null)
                {
                    continue;
                }

                snapshots.Add(new ClipboardElementSnapshot
                {
                    RelativePath = relativePath,
                    Id = element.Id,
                    Name = element.Name,
                    Type = element.Type
                });
            }

            snapshots.Sort(CompareSnapshotPaths);
            return snapshots;
        }

        private static string BuildRelativePath(Transform root, Transform target)
        {
            if (root == null || target == null)
            {
                return null;
            }

            if (root == target)
            {
                return string.Empty;
            }

            List<int> indices = new List<int>();
            Transform current = target;
            while (current != null && current != root)
            {
                Transform parent = current.parent;
                if (parent == null)
                {
                    return null;
                }

                indices.Add(current.GetSiblingIndex());
                current = parent;
            }

            if (current != root)
            {
                return null;
            }

            indices.Reverse();
            if (indices.Count == 0)
            {
                return string.Empty;
            }

            string path = indices[0].ToString();
            for (int i = 1; i < indices.Count; i++)
            {
                path += "/" + indices[i];
            }

            return path;
        }

        private static int CompareSnapshotPaths(ClipboardElementSnapshot left, ClipboardElementSnapshot right)
        {
            string leftPath = left != null ? left.RelativePath : string.Empty;
            string rightPath = right != null ? right.RelativePath : string.Empty;

            int depthCompare = PathDepth(leftPath).CompareTo(PathDepth(rightPath));
            if (depthCompare != 0)
            {
                return depthCompare;
            }

            return string.Compare(leftPath, rightPath, StringComparison.Ordinal);
        }

        private static int PathDepth(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return 0;
            }

            int depth = 1;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    depth++;
                }
            }

            return depth;
        }

        private RectTransform ResolveParentRect(string parentIdOrName)
        {
            if (string.IsNullOrWhiteSpace(parentIdOrName))
            {
                return RootRect;
            }

            XwmElementRef element = Get(parentIdOrName);
            if (element == null || element.RectTransform == null)
            {
                return null;
            }

            if (string.Equals(element.Type, XwmTypeLibrary.ScrollingFrame, StringComparison.OrdinalIgnoreCase))
            {
                XwmScrollingFrameComponent scrollingFrame = element.GameObject != null ? element.GameObject.GetComponent<XwmScrollingFrameComponent>() : null;
                if (scrollingFrame != null && scrollingFrame.Content != null)
                {
                    return scrollingFrame.Content;
                }
            }

            return element.RectTransform;
        }

        private XwmElementRef CloneSubtree(GameObject template, List<ClipboardElementSnapshot> snapshots, RectTransform parent, Vector2 offset, bool activeState)
        {
            if (template == null || snapshots == null || snapshots.Count == 0 || parent == null)
            {
                return null;
            }

            GameObject clone = UnityEngine.Object.Instantiate(template, parent, false);
            clone.hideFlags = HideFlags.None;

            ClipboardElementSnapshot rootSnapshot = null;
            for (int i = 0; i < snapshots.Count; i++)
            {
                ClipboardElementSnapshot snapshot = snapshots[i];
                if (snapshot != null && string.IsNullOrWhiteSpace(snapshot.RelativePath))
                {
                    rootSnapshot = snapshot;
                    break;
                }
            }

            string rootName = rootSnapshot != null ? rootSnapshot.Name : clone.name;
            rootName = MakeUniqueName(parent, BuildCopyNameSeed(rootName));
            clone.name = rootName;

            RectTransform cloneRect = clone.GetComponent<RectTransform>();
            if (cloneRect != null)
            {
                cloneRect.anchoredPosition += new Vector2(offset.x, -offset.y);
            }

            clone.SetActive(activeState);

            XwmElementRef createdRoot = null;
            for (int i = 0; i < snapshots.Count; i++)
            {
                ClipboardElementSnapshot snapshot = snapshots[i];
                if (snapshot == null)
                {
                    continue;
                }

                Transform target = ResolveRelativePath(clone.transform, snapshot.RelativePath);
                if (target == null)
                {
                    continue;
                }

                string resolvedName = string.IsNullOrWhiteSpace(snapshot.RelativePath)
                    ? rootName
                    : (string.IsNullOrWhiteSpace(snapshot.Name) ? target.gameObject.name : snapshot.Name);
                target.gameObject.name = resolvedName;

                string resolvedType = string.IsNullOrWhiteSpace(snapshot.Type) ? XwmTypeLibrary.Frame : snapshot.Type;
                string resolvedId = BuildUniqueId(snapshot.Id);
                XwmElementRef created = CreateRuntimeElement(resolvedId, resolvedName, resolvedType, target.gameObject);
                if (created == null)
                {
                    continue;
                }

                _elements.Add(created);
                Register(created);

                if (string.IsNullOrWhiteSpace(snapshot.RelativePath))
                {
                    createdRoot = created;
                }
            }

            return createdRoot;
        }

        private static string BuildCopyNameSeed(string sourceName)
        {
            string resolved = string.IsNullOrWhiteSpace(sourceName) ? "Element" : sourceName.Trim();
            return resolved + "_copy";
        }

        private static string MakeUniqueName(Transform parent, string desiredName)
        {
            string resolved = string.IsNullOrWhiteSpace(desiredName) ? "Element_copy" : desiredName.Trim();
            if (parent == null)
            {
                return resolved;
            }

            if (!HasChildNamed(parent, resolved))
            {
                return resolved;
            }

            int index = 2;
            string candidate = resolved + "_" + index;
            while (HasChildNamed(parent, candidate))
            {
                index++;
                candidate = resolved + "_" + index;
            }

            return candidate;
        }

        private static bool HasChildNamed(Transform parent, string name)
        {
            if (parent == null || string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child == null)
                {
                    continue;
                }

                if (string.Equals(child.name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private string BuildUniqueId(string sourceId)
        {
            string resolved = string.IsNullOrWhiteSpace(sourceId) ? "node" : sourceId.Trim();
            string baseId = resolved.EndsWith("_copy", StringComparison.OrdinalIgnoreCase) ? resolved : resolved + "_copy";
            string candidate = baseId;
            int suffix = 2;
            while (_elementsById.ContainsKey(candidate))
            {
                candidate = baseId + "_" + suffix;
                suffix++;
            }

            return candidate;
        }

        private static Transform ResolveRelativePath(Transform root, string relativePath)
        {
            if (root == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return root;
            }

            string[] segments = relativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Transform current = root;
            for (int i = 0; i < segments.Length; i++)
            {
                int childIndex;
                if (!int.TryParse(segments[i], out childIndex) || childIndex < 0 || childIndex >= current.childCount)
                {
                    return null;
                }

                current = current.GetChild(childIndex);
                if (current == null)
                {
                    return null;
                }
            }

            return current;
        }

        private static XwmElementRef CreateRuntimeElement(string id, string name, string type, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Image image = gameObject.GetComponent<Image>();
            Text text = gameObject.GetComponent<Text>();
            Button button = gameObject.GetComponent<Button>();
            InputField inputField = gameObject.GetComponent<InputField>();
            XwmPointerRelay pointerRelay = gameObject.GetComponent<XwmPointerRelay>();
            if (pointerRelay == null)
            {
                pointerRelay = gameObject.AddComponent<XwmPointerRelay>();
            }

            string resolvedName = string.IsNullOrWhiteSpace(name) ? gameObject.name : name;
            string resolvedType = string.IsNullOrWhiteSpace(type) ? XwmTypeLibrary.Frame : type;
            return new XwmElementRef(id, resolvedName, resolvedType, gameObject, rectTransform, image, text, button, inputField, pointerRelay);
        }

        private static void AddKeyHandler(Dictionary<KeyCode, Action> map, KeyCode key, Action callback)
        {
            if (callback == null)
            {
                return;
            }

            if (map.TryGetValue(key, out Action existing))
            {
                map[key] = existing + callback;
            }
            else
            {
                map[key] = callback;
            }
        }

        private static void RemoveKeyHandler(Dictionary<KeyCode, Action> map, KeyCode key, Action callback)
        {
            if (callback == null)
            {
                return;
            }

            if (!map.TryGetValue(key, out Action existing))
            {
                return;
            }

            existing -= callback;
            if (existing == null)
            {
                map.Remove(key);
            }
            else
            {
                map[key] = existing;
            }
        }

        private bool ApplyOne(string idOrName, Func<XwmElementRef, bool> action)
        {
            if (action == null)
            {
                return false;
            }

            XwmElementRef element = Get(idOrName);
            return element != null && action(element);
        }

        private int ApplyMany(string idOrName, Func<XwmElementRef, bool> action)
        {
            if (action == null)
            {
                return 0;
            }

            List<XwmElementRef> matches = GetAll(idOrName);
            int changed = 0;
            for (int i = 0; i < matches.Count; i++)
            {
                if (action(matches[i]))
                {
                    changed++;
                }
            }

            return changed;
        }

        private static bool ConnectInput(string idOrName, UnityAction<string> callback, Action<XwmElementRef, UnityAction<string>> binder, XwmWindowHandle handle)
        {
            if (callback == null || binder == null || handle == null)
            {
                return false;
            }

            List<XwmElementRef> matches = handle.GetAll(idOrName);
            bool connected = false;
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].InputField == null)
                {
                    continue;
                }

                binder(matches[i], callback);
                connected = true;
            }

            return connected;
        }

        private bool ConnectInput(string idOrName, UnityAction<string> callback, Action<XwmElementRef, UnityAction<string>> binder)
        {
            return ConnectInput(idOrName, callback, binder, this);
        }

        private bool ConnectPointer(string idOrName, Action<PointerEventData> callback, Action<XwmElementRef, Action<PointerEventData>> binder)
        {
            if (callback == null || binder == null)
            {
                return false;
            }

            List<XwmElementRef> matches = GetAll(idOrName);
            bool connected = false;
            for (int i = 0; i < matches.Count; i++)
            {
                binder(matches[i], callback);
                connected = true;
            }

            return connected;
        }

        private CanvasGroup ResolveCanvasGroup()
        {
            if (_rootCanvasGroup != null)
            {
                return _rootCanvasGroup;
            }

            if (RootObject == null)
            {
                return null;
            }

            _rootCanvasGroup = RootObject.GetComponent<CanvasGroup>();
            if (_rootCanvasGroup == null)
            {
                _rootCanvasGroup = RootObject.AddComponent<CanvasGroup>();
            }

            return _rootCanvasGroup;
        }
    }
}
