using UnityEngine;
using UnityEngine.UI;

namespace XaviiWindowsMod.API
{
    public class WindowInstance
    {
        private readonly float headerHeight;

        public string Id { get; }
        public GameObject Root { get; }
        public RectTransform RootRect { get; }
        public RectTransform BodyRect { get; }
        public RectTransform Content { get; }
        public ScrollRect ScrollRect { get; }
        public Text Title { get; }
        public Button CloseButton { get; }
        public string name => Root != null ? Root.name : Id;
        public bool IsVisible => Root != null && Root.activeSelf;
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

        internal WindowInstance(string id, GameObject root, RectTransform rootRect, RectTransform bodyRect, RectTransform content, ScrollRect scrollRect, Text title, Button closeButton)
        {
            Id = id;
            Root = root;
            RootRect = rootRect;
            BodyRect = bodyRect;
            Content = content;
            ScrollRect = scrollRect;
            Title = title;
            CloseButton = closeButton;
            headerHeight = 26f;
        }

        public GameObject ContentObject => Content != null ? Content.gameObject : null;

        public void Show()
        {
            if (Root != null)
            {
                Root.SetActive(true);
            }

            ResetScroll();
        }

        public void hide()
        {
            Hide();
        }

        public void Hide()
        {
            if (Root != null)
            {
                Root.SetActive(false);
            }
        }

        public void show()
        {
            Show();
        }

        public void clickHide()
        {
            Hide();
        }

        public bool Toggle()
        {
            if (Root == null)
            {
                return false;
            }

            if (Root.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }

            return Root.activeSelf;
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

        public void ResetScroll()
        {
            if (Content != null)
            {
                Content.anchoredPosition = Vector2.zero;
            }

            if (ScrollRect != null)
            {
                ScrollRect.verticalNormalizedPosition = 1f;
            }
        }

        public void resetScroll()
        {
            ResetScroll();
        }

        public void SetContentHeight(float height)
        {
            if (Content == null)
            {
                return;
            }

            float minHeight = BodyRect != null ? BodyRect.rect.height : 0f;
            float newHeight = Mathf.Max(height, minHeight);
            Vector2 size = Content.sizeDelta;
            size.y = newHeight;
            Content.sizeDelta = size;
        }

        public void SetTitle(string title)
        {
            if (Title != null)
            {
                Title.text = title;
            }
        }

        public void SetSize(Vector2 size)
        {
            if (RootRect == null)
            {
                return;
            }

            RootRect.sizeDelta = size;
            if (BodyRect != null)
            {
                BodyRect.offsetMax = new Vector2(-10, -headerHeight);
            }
        }

        public void MoveBy(Vector2 delta)
        {
            Position = Position + delta;
        }

        public void SetOpacity(float alpha)
        {
            if (Root == null)
            {
                return;
            }

            CanvasGroup group = Root.GetComponent<CanvasGroup>();
            if (group == null)
            {
                group = Root.AddComponent<CanvasGroup>();
            }

            group.alpha = Mathf.Clamp01(alpha);
        }

        public float GetOpacity()
        {
            if (Root == null)
            {
                return 1f;
            }

            CanvasGroup group = Root.GetComponent<CanvasGroup>();
            return group != null ? group.alpha : 1f;
        }

        public void SetInteractable(bool interactable)
        {
            if (Root == null)
            {
                return;
            }

            CanvasGroup group = Root.GetComponent<CanvasGroup>();
            if (group == null)
            {
                group = Root.AddComponent<CanvasGroup>();
            }

            group.interactable = interactable;
            group.blocksRaycasts = interactable;
        }

        public void Destroy()
        {
            if (Root != null)
            {
                Object.Destroy(Root);
            }
        }
    }
}
