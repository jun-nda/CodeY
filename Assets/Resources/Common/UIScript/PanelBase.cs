using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UIScript
{
    [DisallowMultipleComponent]
    public class PanelBase : MonoBehaviour
    {
        public static readonly Color blurColor = new Color(0, 0, 0, 0.8f);
        public static readonly Color transparentColor = new Color(0, 0, 0, 0f);

        public PanelAttr panelAttr;
        private Animator _animator;

        public virtual void OnPop() {}
        public virtual void FadeOut()
        {
            ///_animator.gameObject.AddComponent<UIPanelAnimFinish>();
            ///_animator.Play("PopPanel");
            GameObject.Destroy(transform.parent.parent.gameObject);
        }

        public void FadeIn()
        {
            _animator = transform.parent.parent.gameObject.GetComponent<Animator>();
            if (panelAttr.autoScale)
            {
                _animator.Rebind();
                _animator.Play("PushPanelAutoScale");
            }
            if (panelAttr.showBlur || panelAttr.forceClose)
            {
                GameObject forceClose = new GameObject("force");
                forceClose.transform.SetParent(_animator.transform);
                forceClose.transform.SetAsFirstSibling();
                forceClose.transform.localPosition = Vector3.zero;
                forceClose.transform.localScale = Vector3.one;
                forceClose.layer = LayerMask.NameToLayer("UI");
                var rect = forceClose.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                var img = forceClose.AddComponent<Image>();
                img.raycastTarget = true;
                if (panelAttr.showBlur)
                {
                    img.color = blurColor;
                }
                else
                {
                    img.color = transparentColor;
                }
                if (panelAttr.forceClose)
                {
                    var btn = forceClose.AddComponent<Button>();
                    btn.transition = Selectable.Transition.None;
                    btn.onClick.AddListener(OnCloseButtonClick);
                }
            }
        }

        public void OnCloseButtonClick()
        {
            PanelManager.Inst.PopPanel(this);
        }

        [Serializable]
        public class PanelAttr
        {
            public bool autoScale = true;
            public bool showBlur = true;
            public bool forceClose = false;
            public DisableMainCameraRender disableMainCameraRender;
            public enum DisableMainCameraType
            {
                EnableMainCamera = 0,
                DisableMainCamera = 1,
                DisableAndGaussianBlur = 2,
            }
            [Serializable]
            public class DisableMainCameraRender
            {
                [Header("务必确保只有1个面板拥有这个属性\n切换场景前请先关闭面板")]
                public DisableMainCameraType disableMainCameraType = DisableMainCameraType.EnableMainCamera;
            }
        }
    }
}