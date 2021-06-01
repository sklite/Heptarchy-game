using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Menu.Scripts
{
    class PauseMenuDrawerSc : MonoBehaviour
    {


        private static GUIStyle rectStyle;
        Texture2D image;
        string text = "ButtonText";

        public GUISkin GuiSkin;
        // Use this for initialization
        void Start()
        {

            rectStyle = new GUIStyle();
            var rectTexture = new Texture2D(1, 1);
            rectTexture.SetPixel(0, 0, new Color(190, 230, 240, 0.8f));
            rectTexture.Apply();
            rectStyle.normal.background = rectTexture;

        }

        // Update is called once per frame
        void Update()
        {



        }
        void OnGUI()
        {
            GUI.skin = GuiSkin;
            if (SettingsSc.IsPaused)
            {
                DrawMenu();
            }

            if (SettingsSc.IsFinished)
            {

                DrawFinish(/*Settings.Winner*/);

            }
            GUI.skin = null;
        }


        internal void DrawMenu()
        {

            float menuWidth = Screen.width / 2.0f;
            float menuHeight = Screen.height * 0.68f;

            float menuLeft = GetCenteredPosition(menuWidth);
            float menuTop = 30;

            float buttonWidth = menuWidth * 0.67f;
            float buttonHeight = 45;

            float buttonLeft = GetCenteredPosition(buttonWidth);
            float buttonTop = 175;

            float distanceBetweenButtons = 25;

            GUI.Box(new Rect(menuLeft, menuTop, menuWidth, menuHeight), "");

            if (GUI.Button(new Rect(buttonLeft, buttonTop, buttonWidth, buttonHeight), "Restart"))
            {
                var spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<CastleSpawnerSc>();
                spawner.CleanMap(SettingsSc.Stored);
                SettingsSc.SetPause(false);
                //Application.LoadLevel(1);
            }

            if (GUI.Button(new Rect(buttonLeft, buttonTop + buttonHeight + distanceBetweenButtons, buttonWidth, buttonHeight), "To menu"))
            {
                //Application.LoadLevel(0);
                SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(buttonLeft, buttonTop + 2 * (buttonHeight + distanceBetweenButtons), buttonWidth, buttonHeight), "Cancel"))
            {
                SettingsSc.SetPause(false);
            }
        }

        internal void DrawFinish()
        {

        }

        float GetCenteredPosition(float width)
        {
            float screenCenter = Screen.width / 2;
            return screenCenter - width / 2;
        }
    }
}
