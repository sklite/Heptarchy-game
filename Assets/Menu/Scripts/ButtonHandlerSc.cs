using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Menu.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandlerSc : MonoBehaviour
{
    // Use this for initialization
    GameObject[] _colorButtons;
    GameObject[] _aiButton;
    ColorPicker _colorPicker;

    List<string> _aiTexts;
    // Start is called before the first frame update
    void Start()
    {
        _colorButtons = GameObject.FindGameObjectsWithTag("ColorButton").OrderBy(bt => bt.name).ToArray();

        _aiButton = GameObject.FindGameObjectsWithTag("AiButton").OrderBy(bt => bt.name).ToArray();
        _colorPicker = new ColorPicker();
        var allColors = _colorPicker.GetAllColors();

        for (int i = 0; i < _colorButtons.Length; i++)
        {
            var buttonColor = allColors[i];
            _colorButtons[i].GetComponent<Image>().color = buttonColor;
        }

        _aiTexts = new List<string>
        {
            "Easy",
            "Normal",
            "None"
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonAiPressed(GameObject button)
    {
        var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText.text == _aiTexts[0])
            buttonText.text = _aiTexts[1];
        else if (buttonText.text == _aiTexts[1])
            buttonText.text = _aiTexts[2];
        else
            buttonText.text = _aiTexts[0];

        var startButton = GameObject.Find("PlayBt");
        var startButtonScript = startButton.GetComponent<Button>();

        bool allButtonsNone = true;
        foreach (var aiButt in _aiButton)
        {
            if (aiButt.GetComponentInChildren<TextMeshProUGUI>().text != "None")
            {
                allButtonsNone = false;
                break;
            }
        }

        startButtonScript.interactable = !allButtonsNone;
    }

    public void ButtonColorPressed(GameObject button)
    {
        var image = button.GetComponent<Image>();
        var assignedColors = GetAssignedColors();
        image.color = _colorPicker.GetNextColor(image.color, assignedColors);
    }

    public void StartButtonPressed()
    {
        CreateGameInfo();
        //Application.LoadLevel("1");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    void CreateGameInfo()
    {
        List<PlayerInfo> playerInfos = new List<PlayerInfo>();
        AiFromButtonsCreator playerInfoCreator = new AiFromButtonsCreator();
        for (int i = 0; i < _aiButton.Length; i++)
        {
            if (_aiButton[i].GetComponentInChildren<TextMeshProUGUI>().text == "None")
                continue;
            var playerInfo = playerInfoCreator.GetPlayer(_aiButton[i], _colorButtons[i]);
            playerInfos.Add(playerInfo);
        }
        GameInfo.Players = playerInfos.ToArray();
    }

    Color[] GetAssignedColors()
    {
        var result = new Color[_colorButtons.Length];

        for (int i = 0; i < _colorButtons.Length; i++)
        {
            result[i] = _colorButtons[i].GetComponent<Image>().color;
        }
        return result;
    }

    class ColorPicker
    {
        List<Color> _colorStack;
        public ColorPicker()
        {
            _colorStack = new List<Color>
            {
                Color.red,
                Color.green,
                Color.yellow,
                new Color(255/255.0f, 159/255.0f,255/255.0f),
                new Color(165/255.0f, 42/255.0f,42/255.0f),
                Color.cyan,
                new Color(167/255.0f, 113/255.0f,254/255.0f),
            };
        }

        public Color GetNextColor(Color col, Color[] assignedColors)
        {
            var colorIndex = _colorStack.IndexOf(col);
            colorIndex++;
            var selectedColor = colorIndex == _colorStack.Count ? _colorStack[0] : _colorStack[colorIndex];
            while (assignedColors.Contains(selectedColor))
            {
                if (colorIndex < _colorStack.Count - 1)
                    colorIndex++;
                else
                    colorIndex = 0;

                selectedColor = _colorStack[colorIndex];
            }
            return selectedColor;

        }

        public List<Color> GetAllColors()
        {
            return _colorStack;
        }
    }
}
