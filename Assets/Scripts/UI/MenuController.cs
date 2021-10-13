using HexagonDemo.Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace HexagonDemo.Menu {
    public class MenuController : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] Slider colorCountSlider;
        [SerializeField] TextMeshProUGUI colorCountValueText;

        [Header("Map")]
        [SerializeField] MapSettings _mapSettings;
        [SerializeField] Slider gridWidthSlider;
        [SerializeField] Slider gridHeightSlider;
        [SerializeField] TextMeshProUGUI gridWidthValueText;
        [SerializeField] TextMeshProUGUI gridHeightValueText;

        [Header("Bomb")]
        [SerializeField] Slider bombTimeSlider;
        [SerializeField] TextMeshProUGUI bombTimeValueText;

        [Header("Buttons")]
        [SerializeField] Button startButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button quitButton;
        [SerializeField] Button closeSettingsButton;

        [SerializeField] GameObject settingsPanel;

        private void Start()
        {
            

            colorCountSlider.wholeNumbers = true;
            colorCountSlider.maxValue = _mapSettings.ColorCountMax;
            colorCountSlider.minValue = _mapSettings.ColorCountMin;
            colorCountSlider.value = _mapSettings.ColorCount;
            colorCountValueText.text = ((int)colorCountSlider.value).ToString();
            colorCountSlider.onValueChanged.AddListener(delegate { ColorCountSliderChanged(); });

            gridWidthSlider.wholeNumbers = true;
            gridWidthSlider.maxValue = _mapSettings.GridWidthMax;
            gridWidthSlider.minValue = _mapSettings.GridWidthMin;
            gridWidthSlider.value = _mapSettings.GridWidth;
            gridWidthValueText.text = ((int)gridWidthSlider.value).ToString();
            gridWidthSlider.onValueChanged.AddListener(delegate { GridWidthSliderChanged(); });


            gridHeightSlider.wholeNumbers = true;
            gridHeightSlider.maxValue = _mapSettings.GridHeightMax;
            gridHeightSlider.minValue = _mapSettings.GridHeightMin;
            gridHeightSlider.value = _mapSettings.GridHeight;
            gridHeightValueText.text = ((int)gridHeightSlider.value).ToString();
            gridHeightSlider.onValueChanged.AddListener(delegate { GridHeightSliderChanged(); });


            bombTimeSlider.wholeNumbers = true;
            bombTimeSlider.maxValue = _mapSettings.BombTimeMax;
            bombTimeSlider.minValue = _mapSettings.BombTimeMin;
            bombTimeSlider.value = _mapSettings.BombTime;
            bombTimeValueText.text = ((int)bombTimeSlider.value).ToString();
            bombTimeSlider.onValueChanged.AddListener(delegate { BombTimeSliderChanged(); });




            startButton.onClick.AddListener(delegate { StartButton(); });
            settingsButton.onClick.AddListener(delegate { SettingsButton(); });
            quitButton.onClick.AddListener(delegate { QuitButton(); });
            closeSettingsButton.onClick.AddListener(delegate { CloseSettingsButton(); });

            settingsPanel.gameObject.SetActive(false);
            closeSettingsButton.gameObject.SetActive(false);
        }

        #region SliderAction

        private void ColorCountSliderChanged()
        {
            _mapSettings.ColorCount = (int)colorCountSlider.value;
            colorCountValueText.text = ((int)colorCountSlider.value).ToString();
        }
        private void GridWidthSliderChanged()
        {
            _mapSettings.GridWidth = (int)gridWidthSlider.value;
            gridWidthValueText.text = ((int)gridWidthSlider.value).ToString();
        }
        private void GridHeightSliderChanged()
        {
            if ((int)gridHeightSlider.value % 2 == 0)
            {
                gridHeightSlider.value++;
            }
            _mapSettings.GridHeight = (int)gridHeightSlider.value;
            gridHeightValueText.text = ((int)gridHeightSlider.value).ToString();
        }

        private void BombTimeSliderChanged()
        {
            _mapSettings.BombTime = (int)bombTimeSlider.value;
            bombTimeValueText.text = ((int)bombTimeSlider.value).ToString();
        }
        #endregion

        #region Buttons
        public void StartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void SettingsButton()
        {
            if (settingsPanel.activeSelf)
            {
                settingsPanel.gameObject.SetActive(false);
            }
            else
            {
                closeSettingsButton.gameObject.SetActive(true);
                settingsPanel.gameObject.SetActive(true);
            }
        }
        private void CloseSettingsButton()
        {
            settingsPanel.gameObject.SetActive(false);
            closeSettingsButton.gameObject.SetActive(false);
        }
        public void QuitButton()
        {
            Application.Quit();
        }

        #endregion
    }
}
