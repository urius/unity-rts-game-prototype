using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUnitButton : MonoBehaviour
{
    [SerializeField]
    private RobotsFactoryController _factoryController;

    [SerializeField]
    private int _buildableUnitIndex;
    private Button _button;
    private Image _progressImage;
    private Text _costText;
    private Text _queueText;

    void Awake()
    {
        _button = GetComponent<Button>();
        _progressImage = transform.Find("Progress").GetComponent<Image>();
        _costText = transform.Find("CostTxt").GetComponent<Text>();
        _queueText = transform.Find("QueueTxt").GetComponent<Text>();
    }

    void Start()
    {
        _button.onClick.AddListener(OnClick);
        _factoryController.UpdateBuildInfo += OnUpdateBuildInfo;

        _progressImage.fillAmount = 0;
        _queueText.text = string.Empty;

        _costText.text = _factoryController.GetUnitInfo(_buildableUnitIndex).cost + "$";
    }

    private void OnUpdateBuildInfo(int index, float progress)
    {
        if (index == _buildableUnitIndex)
        {
            if (progress >= 1)
            {
                _progressImage.fillAmount = 0;

                ShowUnitsInQueueCount();
            }
            else
            {
                _progressImage.fillAmount = progress < 1 ? progress : 0;
            }
        }
    }

    private void ShowUnitsInQueueCount()
    {
        var unitsCountInQueue = _factoryController.GetUnitsCountInBuildQueue(_buildableUnitIndex);
        _queueText.text = unitsCountInQueue <= 0 ? string.Empty : unitsCountInQueue.ToString();
    }

    void OnClick()
    {
        _factoryController.AddToBuildQueue(_buildableUnitIndex);

        ShowUnitsInQueueCount();
    }
    // Update is called once per frame
    void Update()
    {
    }

    void Stop()
    {
        _factoryController.UpdateBuildInfo -= OnUpdateBuildInfo;
    }
}
