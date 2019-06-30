using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildUnitButton : MonoBehaviour
{
    [Inject]
    private UnitsConfig _unitsConfig;
    [Inject]
    private UnitFactoryFacade _factoryFacade;


    [SerializeField]
    private MobileUnitType _buildableUnitType;


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
        _factoryFacade.FactoryModel.BuildProgressUpdated += OnBuildProgressUpdated;

        _progressImage.fillAmount = 0;
        _queueText.text = string.Empty;

        _costText.text = _unitsConfig.GetConfigByType(_buildableUnitType).cost + "$";
    }

    private void OnBuildProgressUpdated(MobileUnitType typeId, float progress)
    {
        if (typeId == _buildableUnitType)
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
        var unitsCountInQueue = _factoryFacade.FactoryModel.GetUnitsCountInBuildQueue(_buildableUnitType);
        _queueText.text = unitsCountInQueue <= 0 ? string.Empty : unitsCountInQueue.ToString();
    }

    private void OnClick()
    {
        _factoryFacade.TryBuildUnit(_buildableUnitType);

        ShowUnitsInQueueCount();
    }

    private void Stop()
    {
        _factoryFacade.FactoryModel.BuildProgressUpdated -= OnBuildProgressUpdated;
    }
}
