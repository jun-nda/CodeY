using System;
using System.Collections.Generic;
using System.Text;
///using CamelHotCLR;
///using CsamelFramework.DataStructure;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
///using HotRes.Common;
using Object = UnityEngine.Object;

namespace Common.UIScript
{
    /// <summary>
    /// UI Panel管理中心，Singleton 线程安全，但不完全是LazyLoad
    /// </summary>
    public class PanelManager
    {
        private PanelManager()
        {
        }
        
        public static PanelManager Inst { get; } = new PanelManager();

        /// <summary>
        /// 严格使用栈结构管理Panel的进出；
        /// 尽量避免点击关闭按钮，延迟(等等服务器数据)popPanel的情况。
        /// </summary>
        private readonly ExtStack<PanelBase> panelStack = new ExtStack<PanelBase>();

        private Transform rootTrans;
        private GameObject canvasPanelRes;

        /// <summary>
        /// 务必确保每一个场景有且只有一个MainCamera
        /// </summary>
       
        private int sortOrder;

        private Camera mainCamera;
        public Camera MainCamera
        {
            get { return mainCamera; }
        }
        
        private Camera uiCamera;
        public Camera UICamera
        {
            get { return uiCamera; }
        }
        
        ///private RawImage blurImage;

        public void Init()
        {
            canvasPanelRes = ResManager.InstantiateGameObjectSync("Common/Prefab/CanvasPanel");

            var go = ResManager.InstantiateGameObjectSync("Common/Prefab/UIRoot");

            Object.DontDestroyOnLoad(go);
            rootTrans = go.transform;

            uiCamera = go.transform.Find("UICamera").GetComponent<Camera>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            sortOrder = 0;
            //blurImage = go.transform.Find("DefaultPanel").gameObject.GetComponent<RawImage>();
            if (Camera.main != null)
            {
                mainCamera = Camera.main;
                var cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
                cameraData.cameraStack.Add(uiCamera);
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                throw new Exception(scene.name + " Scene must need a base Camera with Tag 'MainCamera'");
            }
            var cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            cameraData.cameraStack.Add(uiCamera);
        }

        private void ResetSortOrder(Canvas canvas, GameObject panelObj)
        {
            Renderer[] renderers = panelObj.GetComponentsInChildren<Renderer>(true);
            canvas.worldCamera = uiCamera;
            canvas.sortingLayerName = "UIPanel";
            if (renderers.Length > 0)
            {
                List<Renderer> list = new List<Renderer>();
                foreach (var renderer in renderers)
                {
                    list.Add(renderer);
                }
                list.Sort((a, b) => {
                    return a.sortingLayerName == b.sortingLayerName ? a.sortingOrder - b.sortingOrder : LayerMask.NameToLayer(a.sortingLayerName) - LayerMask.NameToLayer(b.sortingLayerName);
                });
                string oldName = "";
                int oldOrder = 0;
                bool hasSet = false;
                foreach (var renderer in renderers)
                {
                    if (!hasSet && renderer.sortingLayerName != "Default" && (renderer.sortingLayerName != "UIPanel" || renderer.sortingOrder >= 0))
                    {
                        hasSet = true;
                        sortOrder++;
                        canvas.sortingOrder = sortOrder;
                    }
                    if (renderer.sortingLayerName != oldName || renderer.sortingOrder != oldOrder)
                    {
                        oldName = renderer.sortingLayerName;
                        oldOrder = renderer.sortingOrder;
                        sortOrder++;
                    }
                    renderer.sortingLayerName = "UIPanel";
                    renderer.sortingOrder = sortOrder;
                }
                if (!hasSet)
                {
                    sortOrder++;
                    canvas.sortingOrder = sortOrder;
                }
            }
            else
            {
                sortOrder++;
                canvas.sortingOrder = sortOrder;
            }
        }

        public PanelBase PushPanel(string res)
        {
            //Debug.Log("PushPanel  " + res);
            GameObject canvasPanel = GameObject.Instantiate(canvasPanelRes);
            canvasPanel.transform.SetParent(rootTrans);
            var resGo = ResManager.LoadGameObjectSync(res);
            canvasPanel.name = resGo.name + "Canvas";
            var go = GameObject.Instantiate(resGo, canvasPanel.transform.GetChild(0), false);
            var panel = go.GetComponent<PanelBase>();
            if (panel == null)
            {
                throw new Exception("UI Panel must use a PanelBase Component.");
            }
            go.name = resGo.name;
            var rect = go.transform as RectTransform;
            var resRect = resGo.transform as RectTransform;
            //rect.anchoredPosition = resRect.anchoredPosition;
            rect.offsetMin = resRect.offsetMin;
            rect.offsetMax = resRect.offsetMax;

            ResetSortOrder(canvasPanel.GetComponent<Canvas>(), go);
            panelStack.Push(panel);
            panel.FadeIn();
            CheckMainCamera(panel, true);
            return panel;
        }

        public PanelBase GetTopPanel()
        {
            return panelStack.Peek();
        }

        public T PushPanel<T>(string res) where T : PanelBase
        {
            return PushPanel(res) as T;
        }

        public void PopPanel()
        {
            if (panelStack.Count > 0)
            {
                var panel = panelStack.Pop();
                panel.OnPop();
                panel.FadeOut();
                CheckMainCamera(panel, false);
            }
        }
        
        public void PopAllPanels()
        {
            while (panelStack.Count > 0)
            {
                var panel = panelStack.Pop();
                panel.OnPop();
                panel.FadeOut();
                CheckMainCamera(panel, false);
            }
        }

        /// <summary>
        /// Pop Specified panel.
        /// </summary>
        /// <param name="panel"> target panel will be poped.</param>
        public void PopPanel(PanelBase panel)
        {
            panelStack.Pop(panel);
            panel.OnPop();
            panel.FadeOut();
            CheckMainCamera(panel, false);
        }

        private void AfterMainCameraRenderTexture()
        {
            mainCamera.enabled = false;
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            uiCamera.tag = "MainCamera";
        }

        public void CheckMainCamera(PanelBase panel, bool isPush)
        {
            /*
            var tp = panel.panelAttr.disableMainCameraRender.disableMainCameraType;
            if (tp != PanelBase.PanelAttr.DisableMainCameraType.EnableMainCamera)
            {
                if (isPush)
                {
                    switch (tp)
                    {
                        case PanelBase.PanelAttr.DisableMainCameraType.DisableMainCamera:
                            var blit = panel.gameObject.AddComponent<BlitRT>();
                            blurImage.texture = blit.renderTexture;
                            blurImage.enabled = true;
                            blit.callback = AfterMainCameraRenderTexture;
                            break;
                        case PanelBase.PanelAttr.DisableMainCameraType.DisableAndGaussianBlur:
                            var gaussianBlur = panel.gameObject.AddComponent<GaussianBlurRT>();
                            blurImage.texture = gaussianBlur.renderTexture;
                            blurImage.enabled = true;
                            gaussianBlur.callback = AfterMainCameraRenderTexture;
                            break;
                    }
                }
                else
                {
                    uiCamera.tag = "UICamera";
                    uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
                    mainCamera.enabled = true;
                    blurImage.enabled = false;
                }
            }
            */
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"stackCount {panelStack.Count}");
            return sb.ToString();
        }

        /// <summary>
        /// 检查当前是否存在这个名称的面板（防止弹出多个）
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public bool CheckHavePanel( string panelName )
		{
            int i = 0;
            while (panelStack.Count > 0 && panelStack.Count >= i)
            {
                var panel = panelStack.CheckByIndex(i);
				if ( panelName == panel.name )
				{
                    return true;
				}

                i ++;
            }

            return false;
		}
    }
}