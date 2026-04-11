using ColossalFramework;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using WatchIt.Managers;

namespace WatchIt.Panels
{
    public class ProblemFilteringPanel : UIPanel
    {
        private UITextureAtlas _notificationsAtlas;
        private UILabel _title;
        private UIButton _close;
        private UIDragHandle _dragHandle;

        private UITabContainer _tabContainer;

        private UIPanel _problemFilteringPanel;
        private UIScrollablePanel _problemFilteringScrollablePanel;
        private UIScrollbar _problemFilteringScrollbar;

        private UISlicedSprite _problemScrollbarTrack;
        private UISlicedSprite _problemScrollbarThumb;


        private UICheckBox[] _checkboxes1;
        private UICheckBox[] _checkboxes2;

        public static Notification.ProblemStruct _problemComparator = Notification.ProblemStruct.All;

        private static readonly PositionData<Notification.Problem1>[] _keyNotifications1 = Utils.GetOrderedEnumData<Notification.Problem1>("Text");
        private static readonly PositionData<Notification.Problem2>[] _keyNotifications2 = Utils.GetOrderedEnumData<Notification.Problem2>("Text");

        public override void Awake()
        {
            base.Awake();

            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It!] ProblemFilteringPanel:Awake -> Exception: " + e.Message);
            }
        }

        public override void Start()
        {
            base.Start();

            try
            {
                _notificationsAtlas = ResourceLoader.GetAtlas("Notifications");
                LoadConfig();

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It!] ProblemFilteringPanel:Start -> Exception: " + e.Message);
            }
        }

        public override void Update()
        {
            base.Update();
            ForceUpdate();

        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            try
            {
                
               
                if (_problemScrollbarThumb != null)
                {
                    Destroy(_problemScrollbarThumb.gameObject);
                }
                if (_problemScrollbarTrack != null)
                {
                    Destroy(_problemScrollbarTrack.gameObject);
                }
                if (_problemFilteringScrollbar != null)
                {
                    Destroy(_problemFilteringScrollbar.gameObject);
                }
                if (_problemFilteringScrollablePanel != null)
                {
                    Destroy(_problemFilteringScrollablePanel.gameObject);
                }
                if (_problemFilteringPanel != null)
                {
                    Destroy(_problemFilteringPanel.gameObject);
                }
                if (_tabContainer != null)
                {
                    Destroy(_tabContainer.gameObject);
                }
                if (_dragHandle != null)
                {
                    Destroy(_dragHandle.gameObject);
                }
                if (_close != null)
                {
                    Destroy(_close.gameObject);
                }
                if (_title != null)
                {
                    Destroy(_title.gameObject);
                }
                foreach(UICheckBox checkbox in _checkboxes1)
                {
                    Destroy(checkbox.gameObject);
                }
                foreach (UICheckBox checkbox in _checkboxes2)
                {
                    Destroy(checkbox.gameObject);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It!] ProblemFilteringPanel:OnDestroy -> Exception: " + e.Message);
            }
        }

        public void ForceUpdate()
        {
            try
            {
                UpdateProblemComparator();

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It!] ProblemFilteringPanel:ForceUpdate -> Exception: " + e.Message);
            }
        }

        private void CreateUI()
        {
            try
            {
                backgroundSprite = "MenuPanel2";
                isVisible = false;
                canFocus = true;
                isInteractive = true;
                width = 750f;
                height = 650f;
                relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2f), Mathf.Floor((GetUIView().fixedHeight - height) / 2f));

                _title = UIUtils.CreateMenuPanelTitle(this, "Problem Filtering");
                _close = UIUtils.CreateMenuPanelCloseButton(this);
                _dragHandle = UIUtils.CreateMenuPanelDragHandle(this);



                _tabContainer = UIUtils.CreateTabContainer(this);
                _tabContainer.width = width - 40f;
                _tabContainer.height = 0f;
                _tabContainer.relativePosition = new Vector3(20f, 85f);


                             
                _problemFilteringPanel = UIUtils.CreatePanel(this, "ProblemList");
                _problemFilteringPanel.width = width - 40f;
                _problemFilteringPanel.height = height - 145f;
                _problemFilteringPanel.relativePosition = new Vector3(20f, 85f);

                _problemFilteringScrollablePanel = UIUtils.CreateScrollablePanel(_problemFilteringPanel, "ProblemScrollablePanel");
                _problemFilteringScrollablePanel.autoLayout = true;
                _problemFilteringScrollablePanel.autoLayoutDirection = LayoutDirection.Vertical;
                _problemFilteringScrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                _problemFilteringScrollablePanel.builtinKeyNavigation = true;
                _problemFilteringScrollablePanel.clipChildren = true;
                _problemFilteringScrollablePanel.width = _problemFilteringScrollablePanel.parent.width - 25f;
                _problemFilteringScrollablePanel.height = _problemFilteringScrollablePanel.parent.height;
                _problemFilteringScrollablePanel.relativePosition = new Vector3(0f, 0f);

                _problemFilteringScrollbar = UIUtils.CreateScrollbar(_problemFilteringPanel, "ProblemScrollbar");
                _problemFilteringScrollbar.orientation = UIOrientation.Vertical;
                _problemFilteringScrollbar.incrementAmount = 25f;
                _problemFilteringScrollbar.width = 20f;
                _problemFilteringScrollbar.height = _problemFilteringScrollbar.parent.height;
                _problemFilteringScrollbar.relativePosition = new Vector3(_problemFilteringScrollbar.parent.width - 20f, 0f);

                _problemScrollbarTrack = UIUtils.CreateSlicedSprite(_problemFilteringScrollbar, "ProblemScrollbarTrack");
                _problemScrollbarTrack.spriteName = "ScrollbarTrack";
                _problemScrollbarTrack.fillDirection = UIFillDirection.Vertical;
                _problemScrollbarTrack.width = _problemScrollbarTrack.parent.width;
                _problemScrollbarTrack.height = _problemScrollbarTrack.parent.height;
                _problemScrollbarTrack.relativePosition = new Vector3(0f, 0f);

                _problemScrollbarThumb = UIUtils.CreateSlicedSprite(_problemFilteringScrollbar, "ProblemScrollbarThumb");
                _problemScrollbarThumb.spriteName = "ScrollbarThumb";
                _problemScrollbarThumb.fillDirection = UIFillDirection.Vertical;
                _problemScrollbarThumb.width = _problemScrollbarThumb.parent.width - 5f;
                _problemScrollbarThumb.relativePosition = new Vector3(0f, 0f);

                _problemFilteringScrollablePanel.verticalScrollbar = _problemFilteringScrollbar;
                _problemFilteringScrollbar.trackObject = _problemScrollbarTrack;
                _problemFilteringScrollbar.thumbObject = _problemScrollbarThumb;


                //create problem type checkboxes for problem1 enums           
                if(_checkboxes1 == null)
                {
                    _checkboxes1 = new UICheckBox[_keyNotifications1.Length];
                }
                for (int i = 0; i < _keyNotifications1.Length; i++)
                {
                    _checkboxes1[i] = CreateCheckBox(_problemFilteringScrollablePanel, _keyNotifications1[i].enumName,
                        ( _problemComparator & _keyNotifications1[i].enumValue ).IsNotNone);
                }


                //create problem type checkboxs for problem2 enums
                if (_checkboxes2 == null)
                {
                    _checkboxes2 = new UICheckBox[_keyNotifications2.Length];
                }
                for(int i=0; i < _keyNotifications2.Length; i++)
                {
                    _checkboxes2[i] = CreateCheckBox(_problemFilteringScrollablePanel, _keyNotifications2[i].enumName,
                        (_problemComparator & _keyNotifications2[i].enumValue).IsNotNone);
                }

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It!] ProblemPanel:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateProblemComparator()
        {
            _problemComparator = Notification.ProblemStruct.None;

            for (int i=0; i < _keyNotifications1.Length; i++)
            {
                if (_checkboxes1[i].isChecked)
                {
                    _problemComparator |= _keyNotifications1[i].enumValue;
                }
            }
            for(int i=0; i < _keyNotifications2.Length; i++)
            {
                if (_checkboxes2[i].isChecked)
                {
                    _problemComparator |= _keyNotifications2[i].enumValue;
                }
            }
        }

        private UICheckBox CreateCheckBox(UIComponent parent, string text, bool check)
        {
            UICheckBox checkBox = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsCheckBoxTemplate")) as UICheckBox;

            checkBox.text = text;
            checkBox.isChecked = check;

            checkBox.eventCheckChanged += (component, eventParam) =>
            {
                ForceUpdate();
                SaveConfig();
            };

            return checkBox;
        }

        private void SaveConfig()
        {
            List<string> disabled = new List<string>();
            for (int i = 0; i < _keyNotifications1.Length; i++)
                if(( _problemComparator & _keyNotifications1[i].enumValue ).IsNone )
                    disabled.Add( _keyNotifications1[i].enumName );
            for (int i = 0; i < _keyNotifications2.Length; i++)
                if(( _problemComparator & _keyNotifications2[i].enumValue ).IsNone )
                    disabled.Add( _keyNotifications2[i].enumName );

            ModConfig.Instance.FilteredProblems = String.Join(",", disabled.ToArray());
            ModConfig.Instance.Save();
        }

        public void LoadConfig()
        {
            Notification.ProblemStruct disabled = Notification.ProblemStruct.None;
            string[] items = ModConfig.Instance.FilteredProblems.Split(',');
            foreach( string item in items )
            {
                for (int i = 0; i < _keyNotifications1.Length; i++)
                {
                    if( item == _keyNotifications1[i].enumName )
                    {
                        disabled |= _keyNotifications1[i].enumValue;
                        break;
                    }
                }
                for (int i = 0; i < _keyNotifications2.Length; i++)
                {
                    if( item == _keyNotifications2[i].enumName )
                    {
                        disabled |= _keyNotifications2[i].enumValue;
                        break;
                    }
                }
            }
            _problemComparator = Notification.ProblemStruct.All & ~(disabled);
        }
    }
}
