using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    private bool somethingSelected = false;

    //Вектора в экранных координатах
    Vector3 inputStartPoint, inputFinishPoint;

    Rect selectionRect;

    //TheGameSc gameScript;
    //HumanPlayer humanPlayer;
    GameObject[] castles;
    // Start is called before the first frame update
    void Start()
    {
        //castles = GameObject.FindGameObjectsWithTag("Castles");
        selectionRect = new Rect(0, 0, 0, 0);

        _staticRectTexture = new Texture2D(1, 1);
        _staticRectStyle = new GUIStyle();
        _staticRectTexture.SetPixel(0, 0, new Color(190, 230, 240, 0.2f));
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;


        //gameScript = GameObject.Find("TheGame").GetComponent<TheGameSc>();
        //humanPlayer = gameScript.GetHumanPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    SettingsSc.SetPause(!SettingsSc.IsPaused);

        if (Input.GetMouseButtonDown(0))
        {
            inputStartPoint = Input.mousePosition;
            inputStartPoint.y = Screen.height - inputStartPoint.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var selection = SelectCastles();
            if (selection.Count == 0)
            {
                DeselectAllCastles();
                selectionRect = new Rect(0, 0, 0, 0);
                return;
            }

            if (somethingSelected && SelectionIsSmall() && selection.Count == 1)
            {
                //humanPlayer.SendTroops(selection[0]);
                DeselectAllCastles();
                return;
            }
            DeselectAllCastles();
            foreach (var item in selection)
            {
                //var castleScript = item.GetComponent<CastleSc>();
                //if (castleScript.GetOwner() != humanPlayer)
                //    continue;

                //castleScript.Select();
                //humanPlayer.SelectedCities.Add(item);
                //somethingSelected = true;
            }
            selectionRect = new Rect(0, 0, 0, 0);
        }



        if (Input.GetMouseButton(0))
        {
            inputFinishPoint = Input.mousePosition;
            inputFinishPoint.y = Screen.height - inputFinishPoint.y;

            var minX = Mathf.Min(inputStartPoint.x, inputFinishPoint.x);
            var minY = Mathf.Min(inputStartPoint.y, inputFinishPoint.y);


            selectionRect = new Rect(minX, minY,
                Mathf.Abs(inputStartPoint.x - inputFinishPoint.x),
                Mathf.Abs(inputStartPoint.y - inputFinishPoint.y));


            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
        else
        {

        }
    }

    List<GameObject> SelectCastles()
    {
        var result = new List<GameObject>();
        if (castles == null || castles.Length == 0)
            return result;

        var normalizedSelectionBox = new Rect();
        var topLeft = Camera.main.ScreenToWorldPoint(new Vector3(selectionRect.xMin, selectionRect.yMin, 0));
        var botRight = Camera.main.ScreenToWorldPoint(new Vector3(selectionRect.xMax, selectionRect.yMax, 0));


        topLeft.y *= -1;
        botRight.y *= -1;

        var xMin = Mathf.Min((topLeft.x), (botRight.x));
        var yMin = Mathf.Min((topLeft.y), (botRight.y));

        var xMax = Mathf.Max(topLeft.x, botRight.x);
        var yMax = Mathf.Max(topLeft.y, botRight.y);

        var center = new Vector3((botRight.x + topLeft.x) / 2, (botRight.y + topLeft.y) / 2, 0);
        var borders = new Vector3((xMax - xMin), (yMax - yMin), 0);

        normalizedSelectionBox.xMin = topLeft.x;
        normalizedSelectionBox.xMax = botRight.x;

        normalizedSelectionBox.yMin = topLeft.y;
        normalizedSelectionBox.yMax = botRight.y;

        var bounds = new Bounds(center, borders);
        foreach (var item in castles)
        {
            //var castleScript = item.GetComponent<CastleSc>();
            //if (castleScript.GetOwner() != humanPlayer)
            //    continue;


            if (IsInsideSelection(bounds, item))
                result.Add(item);
        }
        return result;
    }

    void DeselectAllCastles()
    {
        somethingSelected = false;
        foreach (var item in castles)
        {
            //var castleScript = item.GetComponent<CastleSc>();
            //if (castleScript.IsSelected)
            //    castleScript.Deselect();
        }
    }

    bool SelectionIsSmall()
    {
        return selectionRect.width < 50 && selectionRect.height < 50;
    }

    private bool IsInsideSelection(Bounds bounds, GameObject castle)
    {
        var collider = castle.GetComponent<BoxCollider2D>();
        if (collider == null)
            return false;

        if (collider.bounds.Intersects(bounds))
            return true;
        return false;
    }



    public Rect windowRect0 = new Rect(20, 20, 120, 50);
    public Rect windowRect1 = new Rect(20, 100, 120, 50);

    void OnGUI()
    {
        //if (orgBoxPos != Vector2.zero && endBoxPos != Vector2.zero)
        //{
        //    GUI.DrawTexture(new Rect(orgBoxPos.x, Screen.height - orgBoxPos.y, endBoxPos.x - orgBoxPos.x,
        //        -1 * ((Screen.height - orgBoxPos.y) - (Screen.height - endBoxPos.y))), aTexture); // -
        //}

        //GUIDrawRect(new Rect(1, 1, 40, 200), Color.blue);
        //if (selectionRect == null)
        //    return;
        GUIDrawRect(selectionRect, Color.green);
        GUI.Box(selectionRect, GUIContent.none, _staticRectStyle);

        //if (isPause)
        //    GUI.Window(0, MainMenu, TheMainMenu, "Pause Menu");
        // GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");
    }


    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GUIDrawRect(Rect position, Color color)
    {
        //if (_staticRectTexture == null)
        //{
        //    _staticRectTexture = new Texture2D(1, 1);
        //}

        //if (_staticRectStyle == null)
        //{
        //    _staticRectStyle = new GUIStyle();
        //}

        //_staticRectTexture.SetPixel(0, 0, color);
        //_staticRectTexture.Apply();

        //_staticRectStyle.normal.background = _staticRectTexture;
        //  if (position)
        GUI.Box(position, GUIContent.none, _staticRectStyle);


    }
}
