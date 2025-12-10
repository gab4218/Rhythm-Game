using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum DateTimeLocalizationSelector
{
    Local, UtcLoc
}

public class StaminaManager : MonoBehaviour
{

    [SerializeField] private int _maxStamina = 5;
    public int currentStamina;
    [SerializeField] private float _timePerStamina = 300f;
    [SerializeField] private DateTimeLocalizationSelector _localization;
    [SerializeField] private GameObject _adButton;
    [SerializeField] private TMP_Text _staminaText, _timerText;

    private bool _charging;

    private DateTime _nextTime, _lastTime;

    public static StaminaManager instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        LoadData();
        StartCoroutine(ChargeStamina());
    }

    private IEnumerator ChargeStamina()
    {
        _charging = true;
        UpdateTexts();
        _adButton.SetActive(true);
        while (currentStamina < _maxStamina)
        {
            DateTime currentTime = Localizator(_localization);
            DateTime nextTime = _nextTime;

            bool staminaToAdd = false;

            while (currentTime > nextTime)
            {
                if (currentStamina >= _maxStamina) break;

                currentStamina++;

                staminaToAdd = true;

                UpdateTexts();

                DateTime timeToAdd = nextTime;

                if (_lastTime > nextTime)
                    timeToAdd = _lastTime;

                nextTime = AddTime(timeToAdd, _timePerStamina);
            }

            if (staminaToAdd == true)
            {
                _nextTime = nextTime;
                _lastTime = currentTime;
            }

            UpdateTexts();
            SaveData();

            yield return null;
        }
        _charging = false;
        UpdateTexts();
        SaveData();
        _adButton.SetActive(false);
    }


    private void UpdateTexts()
    {

        if (currentStamina >= _maxStamina)
        {
            _timerText.text = "Full";
        }
        else
        {

            TimeSpan time = _nextTime - Localizator(_localization);

            _timerText.text = $"{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";
        }

        _staminaText.text = $"{currentStamina}/{_maxStamina}";
    }


    public bool UseStamina()
    {
        if (currentStamina - 1 >= 0)
        {
            currentStamina--;

            

            UpdateTexts();

            if (!_charging)
            {
                _nextTime = AddTime(Localizator(_localization), _timePerStamina);
                StartCoroutine(ChargeStamina());
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddStamina(int s)
    {
        currentStamina = Mathf.Min(_maxStamina, currentStamina + s);
        _nextTime = AddTime(Localizator(_localization), _timePerStamina);
        StartCoroutine(ChargeStamina());
        UpdateTexts();
    }


    DateTime AddTime(DateTime time, float add) => time.AddSeconds(add);

    private DateTime Localizator(DateTimeLocalizationSelector val)
    {
        switch (val)
        {
            case DateTimeLocalizationSelector.Local:
                return DateTime.Now;
            case DateTimeLocalizationSelector.UtcLoc:
                return DateTime.UtcNow;
            default:
                return DateTime.UtcNow;
        }
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("currentStamina", currentStamina);
        PlayerPrefs.SetString("nextStamina", _nextTime.ToString());
        PlayerPrefs.SetString("lastStamina", _lastTime.ToString());
    }

    void LoadData()
    {
        currentStamina = PlayerPrefs.GetInt("currentStamina", _maxStamina);
        _nextTime = StringToDateTime(PlayerPrefs.GetString("nextStamina"));
        _lastTime = StringToDateTime(PlayerPrefs.GetString("lastStamina"));
    }

    DateTime StringToDateTime(string data)
    {
        if (string.IsNullOrEmpty(data))
            return Localizator(_localization);
        else
            return DateTime.Parse(data);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) SaveData();
    }
}
