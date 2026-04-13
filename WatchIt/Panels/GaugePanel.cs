using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WatchIt.Panels
{
    public class GaugePanel : UIPanel
    {
        private bool _initialized;
        private float _timer;

        private UIButton _esc;
        private LimitPanel _limitPanel;
        private ProblemPanel _problemPanel;

        private UITextureAtlas _watchItAtlas;
        private UIDragHandle _dragHandle;
        private UISprite _dragSprite;
        private UIButton _limitsButton;
        private UIButton _problemsButton;
        private UIButton _statisticsButton;

        private List<GaugeItem> _gauges;

        
    

        public override void Awake()
        {
            base.Awake();

            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:Awake -> Exception: " + e.Message);
            }
        }

        public override void Start()
        {
            base.Start();

            try
            {
                if (_esc == null)
                {
                    _esc = GameObject.Find("Esc")?.GetComponent<UIButton>();
                }

                if (_limitPanel == null)
                {
                    _limitPanel = GameObject.Find("WatchItLimitPanel")?.GetComponent<LimitPanel>();
                }

                if (_problemPanel == null)
                {
                    _problemPanel = GameObject.Find("WatchItProblemPanel")?.GetComponent<ProblemPanel>();
                }

                if (_esc != null)
                {
                    ModProperties.Instance.PanelDefaultPositionX = ModConfig.Instance.DoubleRibbonLayout ? _esc.absolutePosition.x - 29f : _esc.absolutePosition.x - 13f;
                    ModProperties.Instance.PanelDefaultPositionY = _esc.absolutePosition.y + 50f;
                }

                if (ModConfig.Instance.PositionX == 0f && ModConfig.Instance.PositionY == 0f)
                {
                    ModConfig.Instance.PositionX = ModProperties.Instance.PanelDefaultPositionX;
                    ModConfig.Instance.PositionY = ModProperties.Instance.PanelDefaultPositionY;
                }

                _watchItAtlas = LoadResources();

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:Awake -> Exception: " + e.Message);
            }
        }

        public override void Update()
        {
            base.Update();

            try
            {
                if (!_initialized)
                {
                    UpdateUI();

                    _initialized = true;
                }

                _timer += Time.deltaTime;

                if (_timer > ModConfig.Instance.RefreshInterval)
                {
                    _timer -= ModConfig.Instance.RefreshInterval;

                    if (isVisible)
                    {
                        RefreshGauges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:Update -> Exception: " + e.Message);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            try
            {
                foreach (GaugeItem gauge in _gauges)
                {
                    gauge.DestroyGaugeItem();
                }
                if (_problemsButton != null)
                {
                    Destroy(_problemsButton.gameObject);
                }
                if (_limitsButton != null)
                {
                    Destroy(_limitsButton.gameObject);
                }
                if (_statisticsButton != null)
                {
                    Destroy(_statisticsButton.gameObject);
                }
                if (_dragSprite != null)
                {
                    Destroy(_dragSprite.gameObject);
                }
                if (_dragHandle != null)
                {
                    Destroy(_dragHandle.gameObject);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:OnDestroy -> Exception: " + e.Message);
            }
        }
        public void ForceUpdateUI()
        {
            UpdateUI();
        }

        private UITextureAtlas LoadResources()
        {
            try
            {
                if (_watchItAtlas == null)
                {
                    string[] spriteNames = new string[]
                         {
                            "CircleNormal",
                            "CircleHovered",
                            "CirclePressed",
                            "RectNormal",
                            "RectHovered",
                            "RectPressed",
                            "Drag",
                            "DragHover",
                            "GaugeGreen",
                            "GaugeYellow",
                            "GaugeRed",
                            "Electricity",
                            "ElectricityClassic",
                            "Water",
                            "WaterClassic",
                            "Sewage",
                            "SewageClassic",
                            "Garbage",
                            "GarbageClassic",
                            "ElementarySchool",
                            "ElementarySchoolClassic",
                            "HighSchool",
                            "HighSchoolClassic",
                            "University",
                            "UniversityClassic",
                            "Healthcare",
                            "HealthcareClassic",
                            "Crematorium",
                            "CrematoriumClassic",
                            "FireDepartment",
                            "FireDepartmentClassic",
                            "PoliceDepartment",
                            "PoliceDepartmentClassic",
                            "Jail",
                            "JailClassic",
                            "Heating",
                            "HeatingClassic",
                            "Landfill",
                            "LandfillClassic",
                            "Library",
                            "LibraryClassic",
                            "Cemetery",
                            "CemeteryClassic",
                            "Traffic",
                            "TrafficClassic",
                            "GroundPollution",
                            "GroundPollutionClassic",
                            "DrinkingWaterPollution",
                            "DrinkingWaterPollutionClassic",
                            "NoisePollution",
                            "NoisePollutionClassic",
                            "Fire",
                            "FireClassic",
                            "Crime",
                            "CrimeClassic",
                            "Unemployment",
                            "UnemploymentClassic",
                            "Health",
                            "HealthClassic",
                            "CityAttractiveness",
                            "CityAttractivenessClassic",
                            "Happiness",
                            "HappinessClassic",
                            "Statistics",
                            "StatisticsClassic",
                            "Limits",
                            "LimitsClassic",
                            "ProblemsClassic",
                            "Problems"
                         };
                    
                    _watchItAtlas = ResourceLoader.CreateTextureAtlas("WatchItAtlas", spriteNames, "WatchIt.Icons.");
                }

                return _watchItAtlas;
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:LoadResources -> Exception: " + e.Message);
                return null;
            }
        }

        private void CreateUI()
        {
            try
            {
                zOrder = 0;
                autoSize = false;
                autoLayout = false;
                eventMouseEnter += (component, eventParam) =>
                {
                    opacity = ModConfig.Instance.OpacityWhenHover;
                };
                eventMouseLeave += (component, eventParam) =>
                {
                    opacity = ModConfig.Instance.Opacity;
                };

                _dragHandle = UIUtils.CreateDragHandle(this);
                _dragHandle.tooltip = "Drag to move panel";
                _dragHandle.size = new Vector2(30f, 30f);
                _dragHandle.relativePosition = new Vector3(3f, 3f);
                _dragHandle.eventMouseEnter += (component, eventParam) =>
                {
                    _dragSprite.spriteName = "DragHover";
                };
                _dragHandle.eventMouseLeave += (component, eventParam) =>
                {
                    _dragSprite.spriteName = "Drag";
                };
                _dragHandle.eventMouseUp += (component, eventParam) =>
                {
                    ModConfig.Instance.PositionX = absolutePosition.x;
                    ModConfig.Instance.PositionY = absolutePosition.y;
                    ModConfig.Instance.Save();
                };

                _dragSprite = UIUtils.CreateSprite(this, "Drag", _watchItAtlas, "Drag");
                _dragSprite.isInteractive = false;
                _dragSprite.size = new Vector2(30f, 30f);
                _dragSprite.relativePosition = new Vector3(3f, 3f);

                CreateOrUpdateGauges();
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {
                absolutePosition = new Vector3(ModConfig.Instance.PositionX, ModConfig.Instance.PositionY);
                isVisible = ModConfig.Instance.ShowPanel;
                opacity = ModConfig.Instance.Opacity;

                if (ModConfig.Instance.VerticalLayout)
                {
                    width = ModConfig.Instance.DoubleRibbonLayout ? 72f : 36f;
                }
                else
                {
                    height = ModConfig.Instance.DoubleRibbonLayout ? 72f : 36f;
                }

                if (ModConfig.Instance.ShowDragIcon)
                {
                    _dragHandle.isEnabled = true;
                    _dragSprite.isVisible = true;
                }
                else
                {
                    _dragHandle.isEnabled = false;
                    _dragSprite.isVisible = false;
                }

                if (ModConfig.Instance.DoubleRibbonLayout)
                {
                    _dragHandle.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(21f, 3f) : new Vector3(3f, 21f);
                    _dragSprite.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(21f, 3f) : new Vector3(3f, 21f);
                }
                else
                {
                    _dragHandle.relativePosition = new Vector3(3f, 3f);
                    _dragSprite.relativePosition = new Vector3(3f, 3f);
                }

                CreateOrUpdateGauges();
                RefreshGauges();
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void CreateOrUpdateGauges()
        {
            try
            {
                if (_gauges == null)
                {
                    _gauges = new List<GaugeItem>();
                }
                else
                {
                    foreach (GaugeItem gauge in _gauges)
                    {
                        gauge.DestroyGaugeItem();
                    }

                    _gauges.Clear();

                    Destroy(_limitsButton);
                    Destroy(_problemsButton);
                    Destroy(_statisticsButton);
                }

                if (ModConfig.Instance.ElectricityAvailability)
                {
                    _gauges.Add(CreateGauge("Electricity", GaugeItem.GaugeType.Aspect, GetSpriteName("Electricity"), "Electricity Availability"));
                }
                if (ModConfig.Instance.WaterAvailability)
                {
                    _gauges.Add(CreateGauge("Water", GaugeItem.GaugeType.Aspect, GetSpriteName("Water"), "Water Availability"));
                }
                if (ModConfig.Instance.SewageAvailability)
                {
                    _gauges.Add(CreateGauge("Sewage", GaugeItem.GaugeType.Aspect, GetSpriteName("Sewage"), "Sewage Availability"));
                }
                if (ModConfig.Instance.GarbageAvailability)
                {
                    _gauges.Add(CreateGauge("Garbage", GaugeItem.GaugeType.Aspect, GetSpriteName("Garbage"), "Garbage Availability"));
                }
                if (ModConfig.Instance.ElementarySchoolAvailability)
                {
                    _gauges.Add(CreateGauge("ElementarySchool", GaugeItem.GaugeType.Aspect, GetSpriteName("ElementarySchool"), "Elementary School Availability"));
                }
                if (ModConfig.Instance.HighSchoolAvailability)
                {
                    _gauges.Add(CreateGauge("HighSchool", GaugeItem.GaugeType.Aspect, GetSpriteName("HighSchool"), "High School Availability"));
                }
                if (ModConfig.Instance.UniversityAvailability)
                {
                    _gauges.Add(CreateGauge("University", GaugeItem.GaugeType.Aspect, GetSpriteName("University"), "University Availability"));
                }
                if (ModConfig.Instance.HealthcareAvailability)
                {
                    _gauges.Add(CreateGauge("Healthcare", GaugeItem.GaugeType.Aspect, GetSpriteName("Healthcare"), "Healthcare Availability"));
                }
                if (ModConfig.Instance.CrematoriumAvailability)
                {
                    _gauges.Add(CreateGauge("Crematorium", GaugeItem.GaugeType.Aspect, GetSpriteName("Crematorium"), "Crematorium Availability"));
                }
                if (ModConfig.Instance.FireDepartmentAvailability)
                {
                    _gauges.Add(CreateGauge("FireDepartment", GaugeItem.GaugeType.Aspect, GetSpriteName("FireDepartment"), "Fire Department Availability"));
                }
                if (ModConfig.Instance.PoliceDepartmentAvailability)
                {
                    _gauges.Add(CreateGauge("PoliceDepartment", GaugeItem.GaugeType.Aspect, GetSpriteName("PoliceDepartment"), "Police Department Availability"));
                }
                if (ModConfig.Instance.JailAvailability)
                {
                    _gauges.Add(CreateGauge("Jail", GaugeItem.GaugeType.Aspect, GetSpriteName("Jail"), "Jail Availability"));
                }
                if (ModConfig.Instance.HeatingAvailability)
                {
                    _gauges.Add(CreateGauge("Heating", GaugeItem.GaugeType.Aspect, GetSpriteName("Heating"), "Heating Availability"));
                }
                if (ModConfig.Instance.LandfillUsage)
                {
                    _gauges.Add(CreateGauge("Landfill", GaugeItem.GaugeType.Pillar, GetSpriteName("Landfill"), "Landfill Usage"));
                }
                if (ModConfig.Instance.LibraryUsage)
                {
                    _gauges.Add(CreateGauge("Library", GaugeItem.GaugeType.Pillar, GetSpriteName("Library"), "Library Usage"));
                }
                if (ModConfig.Instance.CemeteryUsage)
                {
                    _gauges.Add(CreateGauge("Cemetery", GaugeItem.GaugeType.Pillar, GetSpriteName("Cemetery"), "Cemetery Usage"));
                }
                if (ModConfig.Instance.TrafficFlow)
                {
                    _gauges.Add(CreateGauge("Traffic", GaugeItem.GaugeType.Pillar, GetSpriteName("Traffic"), "Traffic Flow"));
                }
                if (ModConfig.Instance.GroundPollution)
                {
                    _gauges.Add(CreateGauge("GroundPollution", GaugeItem.GaugeType.Pillar, GetSpriteName("GroundPollution"), "Ground Pollution"));
                }
                if (ModConfig.Instance.DrinkingWaterPollution)
                {
                    _gauges.Add(CreateGauge("DrinkingWaterPollution", GaugeItem.GaugeType.Pillar, GetSpriteName("DrinkingWaterPollution"), "Drinking Water Pollution"));
                }
                if (ModConfig.Instance.NoisePollution)
                {
                    _gauges.Add(CreateGauge("NoisePollution", GaugeItem.GaugeType.Pillar, GetSpriteName("NoisePollution"), "Noise Pollution"));
                }
                if (ModConfig.Instance.FireHazard)
                {
                    _gauges.Add(CreateGauge("Fire", GaugeItem.GaugeType.Pillar, GetSpriteName("Fire"), "Fire Hazard"));
                }
                if (ModConfig.Instance.CrimeRate)
                {
                    _gauges.Add(CreateGauge("Crime", GaugeItem.GaugeType.Pillar, GetSpriteName("Crime"), "Crime Rate"));
                }
                if (ModConfig.Instance.UnemploymentRate)
                {
                    _gauges.Add(CreateGauge("Unemployment", GaugeItem.GaugeType.Pillar, GetSpriteName("Unemployment"), "Unemployment Rate"));
                }
                if (ModConfig.Instance.HealthAverage)
                {
                    _gauges.Add(CreateGauge("Health", GaugeItem.GaugeType.Pillar, GetSpriteName("Health"), "Health Average"));
                }
                if (ModConfig.Instance.CityAttractiveness)
                {
                    _gauges.Add(CreateGauge("CityAttractiveness", GaugeItem.GaugeType.Pillar, GetSpriteName("CityAttractiveness"), "City Attractiveness"));
                }
                if (ModConfig.Instance.Happiness)
                {
                    _gauges.Add(CreateGauge("Happiness", GaugeItem.GaugeType.Pillar, GetSpriteName("Happiness"), "Happiness"));
                }

                int buttonIndex = _gauges.Count;

                bool numberOfGaugesAreOdd = buttonIndex % 2 != 0 ? true : false;

                if (ModConfig.Instance.DoubleRibbonLayout && !numberOfGaugesAreOdd)
                {
                    buttonIndex += 1;
                }

                if (ModConfig.Instance.ShowLimitsButton)
                {
                    buttonIndex++;

                    _limitsButton = UIUtils.CreateButton(this, "Limits", _watchItAtlas, "Circle");
                    _limitsButton.tooltip = "Limits";
                    _limitsButton.size = new Vector2(33f, 33f);

                    if (ModConfig.Instance.DoubleRibbonLayout)
                    {
                        _limitsButton.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(36f * (buttonIndex % 2) + 1.5f, 36f * (buttonIndex / 2) + 36f) : new Vector3(36f * (buttonIndex / 2) + 36f, 36f * (buttonIndex % 2) + 1.5f);
                    }
                    else
                    {
                        _limitsButton.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(1.5f, 36f * buttonIndex + 36f) : new Vector3(36f * buttonIndex + 36f, 1.5f);
                    }

                    _limitsButton.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
                    _limitsButton.normalFgSprite = GetSpriteName("Limits");
                    _limitsButton.hoveredFgSprite = GetSpriteName("Limits");
                    _limitsButton.pressedFgSprite = GetSpriteName("Limits");
                    _limitsButton.disabledFgSprite = GetSpriteName("Limits");

                    _limitsButton.eventClicked += (component, eventParam) =>
                    {
                        if (!eventParam.used)
                        {
                            if (_limitPanel != null)
                            {
                                if (_limitPanel.isVisible)
                                {
                                    _limitPanel.Hide();
                                }
                                else
                                {
                                    _limitPanel.Show();
                                }
                            }

                            eventParam.Use();
                        }
                    };
                }

                if (ModConfig.Instance.ShowProblemsButton)
                {
                    buttonIndex++;

                    _problemsButton = UIUtils.CreateButton(this, "Problems", _watchItAtlas, "Circle");
                    _problemsButton.tooltip = "Problems";
                    _problemsButton.size = new Vector2(33f, 33f);

                    if (ModConfig.Instance.DoubleRibbonLayout)
                    {
                        _problemsButton.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(36f * (buttonIndex % 2) + 1.5f, 36f * (buttonIndex / 2) + 36f) : new Vector3(36f * (buttonIndex / 2) + 36f, 36f * (buttonIndex % 2) + 1.5f);
                    }
                    else
                    {
                        _problemsButton.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(1.5f, 36f * buttonIndex + 36f) : new Vector3(36f * buttonIndex + 36f, 1.5f);
                    }

                    _problemsButton.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
                    _problemsButton.normalFgSprite = GetSpriteName("Problems");
                    _problemsButton.hoveredFgSprite = GetSpriteName("Problems");
                    _problemsButton.pressedFgSprite = GetSpriteName("Problems");
                    _problemsButton.disabledFgSprite = GetSpriteName("Problems");

                    _problemsButton.eventClicked += (component, eventParam) =>
                    {
                        if (!eventParam.used)
                        {
                            if (_problemPanel != null)
                            {
                                if (_problemPanel.isVisible)
                                {
                                    _problemPanel.Hide();
                                }
                                else
                                {
                                    _problemPanel.Show();
                                }
                            }

                            eventParam.Use();
                        }
                    };
                }

                if (ModConfig.Instance.ShowStatisticsButton)
                {
                    buttonIndex++;

                    _statisticsButton = UIUtils.CreateButton(this, "Statistics", _watchItAtlas, "Circle");
                    _statisticsButton.tooltip = "City Statistics";
                    _statisticsButton.size = new Vector2(33f, 33f);

                    if (ModConfig.Instance.DoubleRibbonLayout)
                    {
                        _statisticsButton.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(36f * (buttonIndex % 2) + 1.5f, 36f * (buttonIndex / 2) + 36f) : new Vector3(36f * (buttonIndex / 2) + 36f, 36f * (buttonIndex % 2) + 1.5f);
                    }
                    else
                    {
                        _statisticsButton.relativePosition = ModConfig.Instance.VerticalLayout ? new Vector3(1.5f, 36f * buttonIndex + 36f) : new Vector3(36f * buttonIndex + 36f, 1.5f);
                    }

                    _statisticsButton.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
                    _statisticsButton.normalFgSprite = GetSpriteName("Statistics");
                    _statisticsButton.hoveredFgSprite = GetSpriteName("Statistics");
                    _statisticsButton.pressedFgSprite = GetSpriteName("Statistics");
                    _limitsButton.disabledFgSprite = GetSpriteName("Statistics");

                    _statisticsButton.eventClicked += (component, eventParam) =>
                    {
                        if (!eventParam.used)
                        {
                            UIView.library.ShowModal("StatisticsPanel");

                            eventParam.Use();
                        }
                    };
                }

                if (ModConfig.Instance.VerticalLayout)
                {
                    height = ModConfig.Instance.DoubleRibbonLayout ? (36f * ++buttonIndex / 2) + 36f : 36f * ++buttonIndex + 36f;
                }
                else
                {
                    width = ModConfig.Instance.DoubleRibbonLayout ? (36f * ++buttonIndex / 2) + 36f : 36f * ++buttonIndex + 36f;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:CreateOrUpdateGauges -> Exception: " + e.Message);
            }
        }

        private GaugeItem CreateGauge(string name, GaugeItem.GaugeType type, string spriteName, string toolTip)
        {
            GaugeItem gauge = new GaugeItem();

            try
            {
                gauge.CreateGaugeItem(this, name, type, _gauges.Count, _watchItAtlas, spriteName, toolTip);
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:CreateGauge -> Exception: " + e.Message);
            }

            return gauge;
        }

        private void RefreshGauges()
        {
            try
            {
                foreach (GaugeItem gauge in _gauges)
                {
                    gauge.UpdateGaugeItem();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] GaugePanel:RefreshGauges -> Exception: " + e.Message);
            }
        }

        private string GetSpriteName(string guageName)
        {
            if (ModConfig.Instance.UseClassicIcons)
                guageName += "Classic";
            return guageName;
        }
    }
}
