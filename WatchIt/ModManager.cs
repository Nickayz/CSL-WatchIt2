using System;
using UnityEngine;
using WatchIt.Panels;
using WatchIt.Managers;

namespace WatchIt
{
    public class ModManager : MonoBehaviour
    {
        private bool _initialized;
        private float _timer;

        private WarningPanel _warningPanel;
        private GaugePanel _gaugePanel;
        private LimitPanel _limitPanel;
        private ProblemPanel _problemPanel;

        public void Awake()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] ModManager:Awake -> Exception: " + e.Message);
            }
        }

        public void Start()
        {
            try
            {
                if (_warningPanel == null)
                {
                    _warningPanel = GameObject.Find("WatchItWarningPanel")?.GetComponent<WarningPanel>();
                }

                if (_gaugePanel == null)
                {
                    _gaugePanel = GameObject.Find("WatchItGaugePanel")?.GetComponent<GaugePanel>();
                }

                if (_limitPanel == null)
                {
                    _limitPanel = GameObject.Find("WatchItLimitPanel")?.GetComponent<LimitPanel>();
                }

                if (_problemPanel == null)
                {
                    _problemPanel = GameObject.Find("WatchItProblemPanel")?.GetComponent<ProblemPanel>();
                }

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] ModManager:Start -> Exception: " + e.Message);
            }
        }

        public void Update()
        {
            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    UpdateUI();

                    ProblemManager.Instance.UpdateData();

                    _warningPanel?.ForceUpdateUI();
                    _gaugePanel?.ForceUpdateUI();
                    _limitPanel?.ForceUpdateUI();
                    _problemPanel?.ForceUpdateUI();

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }

                _timer += Time.deltaTime;

                if (_timer > ModConfig.Instance.RefreshInterval)
                {
                    _timer -= ModConfig.Instance.RefreshInterval;

                    if (!ProblemManager.Instance.IsUpdatingData)
                    {
                        ProblemManager.Instance.UpdateData();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] ModManager:Update -> Exception: " + e.Message);
            }
        }

        public void OnDestroy()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] ModManager:OnDestroy -> Exception: " + e.Message);
            }
        }

        private void CreateUI()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] ModManager:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Watch It! 2] ModManager:UpdateUI -> Exception: " + e.Message);
            }
        }
    }
}