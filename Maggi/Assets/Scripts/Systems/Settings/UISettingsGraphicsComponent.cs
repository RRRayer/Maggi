using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class ShadowDistanceTier
{
	public float Distance;
	public string TierDescription;
}

public class UISettingsGraphicsComponent : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    
    private List<Resolution> _resolutionsList = default;
    private Resolution _currentResolution;
	private int _currentResolutionIndex = default;
    
    private int _savedResolutionIndex = default;
    
	[SerializeField] UISettingItemFiller _resolutionsField = default;

    private bool _isFullscreen = default;
    [SerializeField] private UISettingItemFiller _fullscreenField = default;
    
    private int _savedAntiAliasingIndex = default;
    private int _savedShadowDistanceTier = default;
    private bool _savedFullscreenState = default;
    
    [FormerlySerializedAs("ShadowDistanceTierList")]
    [SerializeField] private List<ShadowDistanceTier> _shadowDistanceTierList = new List<ShadowDistanceTier>(); // filled from inspector
    [FormerlySerializedAs("URPAsset")]
    [SerializeField] private UniversalRenderPipelineAsset _uRPAsset = default;
    
	private int _currentShadowQualityIndex = default;
	private List<string> _shadowQualityList = default;
	[SerializeField] private UISettingItemFiller _shadowQualityField = default;
	
	private int _currentAntiAliasingIndex = default;
	private List<string> _currentAntiAliasingList = default;
	[SerializeField] private UISettingItemFiller _antiAliasingField = default;
	
	private int _currentShadowDistanceTier = default;
	[SerializeField] private UISettingItemFiller _shadowDistanceField = default;
    
    

	public event UnityAction<int, int, float, bool> _save = delegate { };

	[SerializeField] private UIGenericButton _saveButton;
	[SerializeField] private UIGenericButton _resetButton;

	void OnEnable()
	{
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
        }

        
		_fullscreenField.OnNextOption += NextFullscreenState;
		_fullscreenField.OnPreviousOption += PreviousFullscreenState;
        
        // _shadowDistanceField.OnNextOption += NextShadowDistanceTier;
        // _shadowDistanceField.OnPreviousOption += PreviousShadowDistanceTier;
        //
        // _antiAliasingField.OnNextOption += NextAntiAliasingTier;
		// _antiAliasingField.OnPreviousOption += PreviousAntiAliasingTier;

		_saveButton.Clicked += SaveSettings;
		_resetButton.Clicked += ResetSettings;

	}
	private void OnDisable()
	{
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveListener(OnResolutionDropdownChanged);
        }


        _fullscreenField.OnNextOption -= NextFullscreenState;
        _fullscreenField.OnPreviousOption -= PreviousFullscreenState;
        
		// _shadowDistanceField.OnNextOption -= NextShadowDistanceTier;
		// _shadowDistanceField.OnPreviousOption -= PreviousShadowDistanceTier;
		//
		// _antiAliasingField.OnNextOption -= NextAntiAliasingTier;
		// _antiAliasingField.OnPreviousOption -= PreviousAntiAliasingTier;

		_saveButton.Clicked -= SaveSettings;
		_resetButton.Clicked -= ResetSettings;
	}

    // 기존 Init() 메서드 수정
    public void Init()
    {
        _resolutionsList = GetResolutionsList();
        _currentResolution = Screen.currentResolution;
        _currentResolutionIndex = GetCurrentResolutionIndex();
        InitializeResolutionDropdown(); // Resolution 드롭다운 초기화
        _isFullscreen = GetCurrentFullscreenState();
        
        
        
        _savedResolutionIndex = _currentResolutionIndex;
    }
    void InitializeResolutionDropdown()
    {
        if (resolutionDropdown == null) return;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        // 각 해상도를 드롭다운 옵션으로 추가
        for (int i = 0; i < _resolutionsList.Count; i++)
        {
            string option = $"{_resolutionsList[i].width} x {_resolutionsList[i].height} @{_resolutionsList[i].refreshRate}Hz";
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

	public void Setup(int currentResolutionIndex)
	{
        _currentResolutionIndex = currentResolutionIndex;
        
		Init();
		SetResolutionField();
        SetFullscreen();
        // SetShadowDistance();
		// SetAntiAliasingField();
	}

	#region Resolution
    void SetResolutionField()
	{
		string displayText = _resolutionsList[_currentResolutionIndex].ToString();
		displayText = displayText.Substring(0, displayText.IndexOf("@"));

		_resolutionsField.FillSettingField(_resolutionsList.Count, _currentResolutionIndex, displayText);
	}
    List<Resolution> GetResolutionsList()
    {
        // 지원되는 해상도만 필터링
        return Screen.resolutions
            .Where(resolution => 
                resolution.width >= 1920 && // 최소 해상도 제한
                resolution.refreshRate >= 60) // 유효한 리프레시 레이트
            .Distinct() // 중복 제거
            .ToList();
    }

    
	int GetCurrentResolutionIndex()
	{
        if (_resolutionsList == null)
        {
            _resolutionsList = GetResolutionsList();
        }
		int index = _resolutionsList.FindIndex(o => o.width == _currentResolution.width && o.height == _currentResolution.height);
		return index;
	}
    void OnResolutionDropdownChanged(int index)
    {
        if (_currentResolutionIndex != index)
        {
            _currentResolutionIndex = index;
            Debug.Log($"OnResolutionDropdownChanged() called {index}");
            OnResolutionChange();
        }
    }

	void OnResolutionChange()
    {
		_currentResolution = _resolutionsList[_currentResolutionIndex];
        
        // 전체화면 모드에서는 FullScreenMode.FullScreenWindow 사용
        FullScreenMode fullScreenMode = _isFullscreen ? 
            FullScreenMode.FullScreenWindow : 
            FullScreenMode.Windowed;

        SetResolutionField();
        
		//Screen.SetResolution(_currentResolution.width, _currentResolution.height, _isFullscreen);
        Screen.SetResolution(_currentResolution.width, _currentResolution.height, FullScreenMode.Windowed);
    
        // 적용까지 약간의 시간이 필요할 수 있음
        StartCoroutine(VerifyResolutionChange(fullScreenMode));
	}
    
    private IEnumerator VerifyResolutionChange(FullScreenMode fullScreenMode)
    {
        // 해상도 변경이 적용될 때까지 잠시 대기
        yield return new WaitForSeconds(5.0f);
    
        if (Screen.currentResolution.width == _currentResolution.width && 
            Screen.currentResolution.height == _currentResolution.height)
        {
            Debug.Log("해상도 변경 성공!");
        }
        else
        {
            Debug.LogWarning($"해상도 변경 실패 - 요청: {_currentResolution.width}x{_currentResolution.height}, " +
            $"현재: {Screen.currentResolution.width}x{Screen.currentResolution.height}");
        
            // 실패 시 기본 해상도로 복구
            Resolution defaultRes = Screen.resolutions[Screen.resolutions.Length - 1];
            Screen.SetResolution(defaultRes.width, defaultRes.height, fullScreenMode);
        }
    }
    
    // void CheckCurrentDisplaySettings()
    // {
    //     Debug.Log($"=== 현재 디스플레이 설정 ===");
    //     Debug.Log($"Screen.currentResolution: {Screen.currentResolution.width}x{Screen.currentResolution.height} @ {Screen.currentResolution.refreshRate}Hz");
    //     Debug.Log($"Screen.width x Screen.height: {Screen.width}x{Screen.height}");
    //     Debug.Log($"Display.main: {Display.main.systemWidth}x{Display.main.systemHeight}");
    //     Debug.Log($"FullScreen 모드: {Screen.fullScreenMode}");
    //
    //     // 지원되는 모든 해상도 출력
    //     Debug.Log("지원되는 해상도 목록:");
    //     foreach (Resolution res in Screen.resolutions)
    //     {
    //         Debug.Log($"- {res.width}x{res.height} @ {res.refreshRate}Hz");
    //     }
    // }


    
	#endregion

    #region fullscreen
    void SetFullscreen()
    {
        if (_isFullscreen)
        {
            _fullscreenField.FillSettingField(2, 1, "On");
        }
        else
        {
            _fullscreenField.FillSettingField(2, 0, "Off");
        }
    }
    bool GetCurrentFullscreenState()
    {
        return Screen.fullScreen;
    }
    void NextFullscreenState()
    {
        _isFullscreen = true;
        OnFullscreenChange();
    }
    void PreviousFullscreenState()
    {
        _isFullscreen = false;
        OnFullscreenChange();
    }
    void OnFullscreenChange()
    {
        Debug.Log($"OnFullscreenChange() called {_isFullscreen}");
        Screen.fullScreen = _isFullscreen;
        SetFullscreen();
    }
    #endregion
    
	#region ShadowDistance
	void SetShadowDistance()
	{
		//_shadowDistanceField.FillSettingField_Localized(_shadowDistanceTierList.Count, _currentShadowDistanceTier, _shadowDistanceTierList[_currentShadowDistanceTier].TierDescription);
	}
	int GetCurrentShadowDistanceTier()
	{
		int tierIndex = -1;
		if (_shadowDistanceTierList.Exists(o => o.Distance == _uRPAsset.shadowDistance))
			tierIndex = _shadowDistanceTierList.FindIndex(o => o.Distance == _uRPAsset.shadowDistance);
		else
		{
			Debug.LogError("Current shadow distance is not in the tier List " + _uRPAsset.shadowDistance);
		}
		return tierIndex;

	}
	void NextShadowDistanceTier()
	{
		_currentShadowDistanceTier++;
		_currentShadowDistanceTier = Mathf.Clamp(_currentShadowDistanceTier, 0, _shadowDistanceTierList.Count);
		OnShadowDistanceChange();
	}
	void PreviousShadowDistanceTier()
	{
		_currentShadowDistanceTier--;
		_currentShadowDistanceTier = Mathf.Clamp(_currentShadowDistanceTier, 0, _shadowDistanceTierList.Count);
		OnShadowDistanceChange();
	}

	void OnShadowDistanceChange()
	{
		_uRPAsset.shadowDistance = _shadowDistanceTierList[_currentShadowDistanceTier].Distance;
		SetShadowDistance();

	}
	#endregion

	#region Anti Aliasing
	void SetAntiAliasingField()
	{
		string optionDisplay = _currentAntiAliasingList[_currentAntiAliasingIndex].Replace("_", "");
		_antiAliasingField.FillSettingField(_currentAntiAliasingList.Count, _currentAntiAliasingIndex, optionDisplay);

	}
	int GetCurrentAntialiasing()
	{
		return _uRPAsset.msaaSampleCount;

	}
	void NextAntiAliasingTier()
	{
		_currentAntiAliasingIndex++;
		_currentAntiAliasingIndex = Mathf.Clamp(_currentAntiAliasingIndex, 0, _currentAntiAliasingList.Count - 1);
		OnAntiAliasingChange();
	}
	void PreviousAntiAliasingTier()
	{
		_currentAntiAliasingIndex--;
		_currentAntiAliasingIndex = Mathf.Clamp(_currentAntiAliasingIndex, 0, _currentAntiAliasingList.Count - 1);
		OnAntiAliasingChange();
	}

	void OnAntiAliasingChange()
	{
		_uRPAsset.msaaSampleCount = _currentAntiAliasingIndex;
		SetAntiAliasingField();

	}
	#endregion

    // SaveSettings() 메서드에 드롭다운 상태 저장 추가
    public void SaveSettings()
    {
        Debug.Log($"SaveSettings() called {resolutionDropdown.value}");
        _savedResolutionIndex = resolutionDropdown.value;
        _currentResolutionIndex = _savedResolutionIndex;
        _savedFullscreenState = _isFullscreen;
        _save.Invoke(_currentResolutionIndex, 0, 0, true);
        
        // _savedAntiAliasingIndex = _currentAntiAliasingIndex;
		// _savedShadowDistanceTier = _currentShadowDistanceTier;
		// float shadowDistance = _shadowDistanceTierList[_currentShadowDistanceTier].Distance;
		
	}
	
    // ResetSettings() 메서드에 드롭다운 리셋 추가
    public void ResetSettings()
    {
        Debug.Log("ResetSettings() called");
        resolutionDropdown.value = _resolutionsList.Count - 1;
        _currentResolutionIndex = _resolutionsList.Count - 1;
        OnResolutionChange();
        _isFullscreen = true;
        OnFullscreenChange();
        
		// _currentAntiAliasingIndex = _savedAntiAliasingIndex;
		// OnAntiAliasingChange();
		// _currentShadowDistanceTier = _savedShadowDistanceTier;
		// OnShadowDistanceChange();
	}
}