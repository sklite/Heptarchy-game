using Assets.Scripts.Players.Ai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Menu.Scripts
{
    class AiFromButtonsCreator
    {
        Dictionary<string, AiEnumeration> _aiEnumDict;

        public AiFromButtonsCreator()
        {
            _aiEnumDict = new Dictionary<string, AiEnumeration>()
            {
                {"None", AiEnumeration.NoAi},
                {"Easy", AiEnumeration.Easy},
                {"Normal", AiEnumeration.Normal}
            };

        }

        public PlayerInfo GetPlayer(GameObject aiButton, GameObject colorButton)
        {
            var ai = GetAi(aiButton);
            var color = GetColor(colorButton);

            return new PlayerInfo()
            {
                AiSkill = ai,
                Color = color
            };
        }


        AiEnumeration GetAi(GameObject aiButton)
        {
            return _aiEnumDict[aiButton.GetComponentInChildren<TextMeshProUGUI>().text];
        }

        Color GetColor(GameObject colorButton)
        {
            return colorButton.GetComponent<Image>().color;
        }
    }
}
