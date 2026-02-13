using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XaviiWindowsMod.Xwm
{
    internal class XwmKeyDispatcher : MonoBehaviour
    {
        public Action Tick;

        private void Update()
        {
            Tick?.Invoke();
        }
    }

    internal class XwmPointerRelay : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> PointerClicked;
        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerUp;
        public event Action<PointerEventData> PointerEntered;
        public event Action<PointerEventData> PointerExited;

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClicked?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke(eventData);
        }
    }

    internal class XwmRuntimeDragDetector : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public RectTransform Target;
        public bool Enabled = true;
        public Action<Vector2> PositionChanged;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Target == null)
            {
                Target = transform as RectTransform;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Enabled || Target == null)
            {
                return;
            }

            Target.anchoredPosition += eventData.delta;
            PositionChanged?.Invoke(new Vector2(Target.anchoredPosition.x, -Target.anchoredPosition.y));
        }
    }

    internal class XwmCornerComponent : MonoBehaviour
    {
        public float Radius = 8f;
    }

    internal class XwmScaleComponent : MonoBehaviour
    {
        public float Scale = 1f;
    }

    internal class XwmGradientComponent : MonoBehaviour
    {
        public Color TopColor = Color.white;
        public Color BottomColor = Color.gray;
        public float Angle = 90f;
    }

    internal class XwmPageLayoutComponent : MonoBehaviour
    {
        public int CurrentPage;

        public void SetPage(int page)
        {
            if (page < 0)
            {
                page = 0;
            }

            CurrentPage = page;
            Apply();
        }

        public void NextPage()
        {
            SetPage(CurrentPage + 1);
        }

        public void PreviousPage()
        {
            SetPage(Mathf.Max(0, CurrentPage - 1));
        }

        public void Apply()
        {
            RectTransform rect = transform as RectTransform;
            if (rect == null)
            {
                return;
            }

            int current = 0;
            for (int i = 0; i < rect.childCount; i++)
            {
                RectTransform child = rect.GetChild(i) as RectTransform;
                if (child == null)
                {
                    continue;
                }

                if (!IsPageCandidate(child.gameObject))
                {
                    continue;
                }

                child.gameObject.SetActive(current == CurrentPage);
                current++;
            }
        }

        private bool IsPageCandidate(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }

            Graphic graphic = gameObject.GetComponent<Graphic>();
            if (graphic != null)
            {
                return true;
            }

            if (gameObject.GetComponent<Button>() != null)
            {
                return true;
            }

            if (gameObject.GetComponent<InputField>() != null)
            {
                return true;
            }

            if (gameObject.transform.childCount > 0)
            {
                return true;
            }

            return false;
        }
    }

    internal class XwmScrollingFrameComponent : MonoBehaviour
    {
        public ScrollRect ScrollRect;
        public RectTransform Viewport;
        public RectTransform Content;
        public float BottomPadding;
        private readonly Vector3[] _corners = new Vector3[4];

        private void OnEnable()
        {
            RefreshBounds();
        }

        private void LateUpdate()
        {
            RefreshBounds();
        }

        public void RefreshBounds()
        {
            if (Content == null || Viewport == null)
            {
                return;
            }

            float viewportWidth = Mathf.Max(1f, Viewport.rect.width);
            float viewportHeight = Mathf.Max(1f, Viewport.rect.height);

            float minY = 0f;
            float maxX = viewportWidth;
            bool hasAny = false;
            CollectBounds(Content, ref hasAny, ref minY, ref maxX);

            float requiredHeight = hasAny ? Mathf.Max(viewportHeight, -minY + BottomPadding) : viewportHeight;
            float requiredWidth = hasAny ? Mathf.Max(viewportWidth, maxX) : viewportWidth;

            Vector2 sizeDelta = Content.sizeDelta;
            float targetWidthDelta = requiredWidth - viewportWidth;
            float targetHeightDelta = requiredHeight;
            if (Mathf.Abs(sizeDelta.x - targetWidthDelta) > 0.1f || Mathf.Abs(sizeDelta.y - targetHeightDelta) > 0.1f)
            {
                sizeDelta.x = targetWidthDelta;
                sizeDelta.y = targetHeightDelta;
                Content.sizeDelta = sizeDelta;
                LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
            }

            Vector2 anchoredPosition = Content.anchoredPosition;
            float minX = Mathf.Min(0f, viewportWidth - requiredWidth);
            float maxY = Mathf.Max(0f, requiredHeight - viewportHeight);
            bool horizontal = ScrollRect != null && ScrollRect.horizontal;
            bool vertical = ScrollRect == null || ScrollRect.vertical;
            anchoredPosition.x = horizontal ? Mathf.Clamp(anchoredPosition.x, minX, 0f) : 0f;
            anchoredPosition.y = vertical ? Mathf.Clamp(anchoredPosition.y, 0f, maxY) : 0f;
            if (Vector2.SqrMagnitude(Content.anchoredPosition - anchoredPosition) > 0.0001f)
            {
                Content.anchoredPosition = anchoredPosition;
            }
        }

        private void CollectBounds(RectTransform parent, ref bool hasAny, ref float minY, ref float maxX)
        {
            if (parent == null)
            {
                return;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                RectTransform child = parent.GetChild(i) as RectTransform;
                if (child == null || !child.gameObject.activeInHierarchy)
                {
                    continue;
                }

                child.GetWorldCorners(_corners);
                for (int c = 0; c < _corners.Length; c++)
                {
                    Vector3 local = Content.InverseTransformPoint(_corners[c]);
                    if (!hasAny || local.y < minY)
                    {
                        minY = local.y;
                    }

                    if (!hasAny || local.x > maxX)
                    {
                        maxX = local.x;
                    }

                    hasAny = true;
                }

                CollectBounds(child, ref hasAny, ref minY, ref maxX);
            }
        }
    }

    internal class XwmRoundedImageComponent : MonoBehaviour
    {
        public float Radius = 8f;
        private RectTransform _rect;
        private Image _image;
        private int _lastWidth = -1;
        private int _lastHeight = -1;
        private float _lastRadius = -1f;
        private Sprite _originalSprite;
        private Image.Type _originalImageType;
        private bool _capturedOriginal;
        private static readonly Dictionary<string, Sprite> SpriteCache = new Dictionary<string, Sprite>(StringComparer.Ordinal);

        private void OnEnable()
        {
            ApplyNow();
        }

        private void LateUpdate()
        {
            ApplyNow();
        }

        private void OnRectTransformDimensionsChange()
        {
            ApplyNow();
        }

        public void ApplyNow()
        {
            if (_rect == null)
            {
                _rect = transform as RectTransform;
            }

            if (_image == null)
            {
                _image = GetComponent<Image>();
            }

            if (_rect == null || _image == null)
            {
                return;
            }

            if (!_capturedOriginal)
            {
                _originalSprite = _image.sprite;
                _originalImageType = _image.type;
                _capturedOriginal = true;
            }

            int width = Mathf.Max(2, Mathf.RoundToInt(Mathf.Abs(_rect.rect.width)));
            int height = Mathf.Max(2, Mathf.RoundToInt(Mathf.Abs(_rect.rect.height)));
            float clampedRadius = Mathf.Clamp(Radius, 0f, Mathf.Min(width, height) * 0.5f);

            if (width == _lastWidth && height == _lastHeight && Mathf.Abs(clampedRadius - _lastRadius) <= 0.01f)
            {
                return;
            }

            _lastWidth = width;
            _lastHeight = height;
            _lastRadius = clampedRadius;

            if (clampedRadius <= 0.001f)
            {
                _image.sprite = _originalSprite;
                _image.type = _originalImageType;
                return;
            }

            Sprite rounded = GetRoundedSprite(width, height, clampedRadius);
            if (rounded != null)
            {
                _image.sprite = rounded;
                _image.type = Image.Type.Simple;
                _image.preserveAspect = false;
            }
        }

        private static Sprite GetRoundedSprite(int width, int height, float radius)
        {
            int roundedRadius = Mathf.RoundToInt(radius);
            string key = width + "x" + height + ":" + roundedRadius;
            if (SpriteCache.TryGetValue(key, out Sprite cached))
            {
                return cached;
            }

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            float r = Mathf.Max(0f, radius);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float px = x + 0.5f;
                    float py = y + 0.5f;
                    float nearestX = Mathf.Clamp(px, r, width - r);
                    float nearestY = Mathf.Clamp(py, r, height - r);
                    float dx = px - nearestX;
                    float dy = py - nearestY;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float alpha = r <= 0.001f ? 1f : Mathf.Clamp01(r + 0.5f - distance);
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f);
            SpriteCache[key] = sprite;
            return sprite;
        }
    }

    internal class XwmGradientEffect : BaseMeshEffect
    {
        public Color TopColor = Color.white;
        public Color BottomColor = Color.gray;
        public float Angle = 90f;
        private readonly List<UIVertex> _vertices = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive() || vh == null || vh.currentVertCount == 0)
            {
                return;
            }

            _vertices.Clear();
            vh.GetUIVertexStream(_vertices);
            if (_vertices.Count == 0)
            {
                return;
            }

            float radians = Angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            float minProjection = float.MaxValue;
            float maxProjection = float.MinValue;

            for (int i = 0; i < _vertices.Count; i++)
            {
                Vector3 position = _vertices[i].position;
                float projection = position.x * direction.x + position.y * direction.y;
                if (projection < minProjection)
                {
                    minProjection = projection;
                }

                if (projection > maxProjection)
                {
                    maxProjection = projection;
                }
            }

            float range = Mathf.Max(0.0001f, maxProjection - minProjection);
            for (int i = 0; i < _vertices.Count; i++)
            {
                UIVertex vertex = _vertices[i];
                Vector3 position = vertex.position;
                float projection = position.x * direction.x + position.y * direction.y;
                float t = Mathf.Clamp01((projection - minProjection) / range);
                Color gradient = Color.Lerp(BottomColor, TopColor, t);
                Color baseColor = vertex.color;
                vertex.color = new Color(baseColor.r * gradient.r, baseColor.g * gradient.g, baseColor.b * gradient.b, baseColor.a * gradient.a);
                _vertices[i] = vertex;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(_vertices);
        }
    }
}
