using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    private bool _somethingSelected = false;

    //Вектора в экранных координатах
    Vector3 _inputStartPoint, _inputFinishPoint;

    Rect _selectionRect;

    TheGameSc _gameScript;
    HumanPlayer _humanPlayer;
    GameObject[] _castles;
    // Start is called before the first frame update
    void Start()
    {
        _castles = GameObject.FindGameObjectsWithTag("Castles");
        _selectionRect = new Rect(0, 0, 0, 0);

        _staticRectTexture = new Texture2D(1, 1);
        _staticRectStyle = new GUIStyle();
        _staticRectTexture.SetPixel(0, 0, new Color(190, 230, 240, 0.2f));
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;


        _gameScript = GameObject.Find("TheGame").GetComponent<TheGameSc>();
        _humanPlayer = _gameScript.GetHumanPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    SettingsSc.SetPause(!SettingsSc.IsPaused);

        if (Input.GetMouseButtonDown(0))
        {
            _inputStartPoint = Input.mousePosition;
            _inputStartPoint.y = Screen.height - _inputStartPoint.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var selection = SelectCastles();
            if (selection.Count == 0)
            {
                DeselectAllCastles();
                _selectionRect = new Rect(0, 0, 0, 0);
                return;
            }

            if (_somethingSelected && SelectionIsSmall() && selection.Count == 1)
            {
                _humanPlayer.SendTroops(selection[0]);
                DeselectAllCastles();
                return;
            }
            DeselectAllCastles();
            foreach (var item in selection)
            {
                var castleScript = item.GetComponent<CastleSc>();
                if (castleScript.GetOwner() != _humanPlayer)
                    continue;

                castleScript.Select();
                _humanPlayer.SelectedCities.Add(item);
                _somethingSelected = true;
            }
            _selectionRect = new Rect(0, 0, 0, 0);
        }



        if (Input.GetMouseButton(0))
        {
            _inputFinishPoint = Input.mousePosition;
            _inputFinishPoint.y = Screen.height - _inputFinishPoint.y;



            var minX = Mathf.Min(_inputStartPoint.x, _inputFinishPoint.x);
            var minY = Mathf.Min(_inputStartPoint.y, _inputFinishPoint.y);


            _selectionRect = new Rect(minX, minY,
                Mathf.Abs(_inputStartPoint.x - _inputFinishPoint.x),
                Mathf.Abs(_inputStartPoint.y - _inputFinishPoint.y));

        }
    }

    List<GameObject> SelectCastles()
    {
        var result = new List<GameObject>();
        if (_castles == null || _castles.Length == 0)
            return result;

        var normalizedSelectionBox = new Rect();
        var botLeft = Camera.main.ScreenToWorldPoint(new Vector3(_selectionRect.xMin, Screen.height - _selectionRect.yMin, 0));
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(_selectionRect.xMax, Screen.height - _selectionRect.yMax, 0));

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var xMin = Mathf.Min((botLeft.x), (topRight.x));
        var yMin = Mathf.Min((botLeft.y), (topRight.y));

        var xMax = Mathf.Max(botLeft.x, topRight.x);
        var yMax = Mathf.Max(botLeft.y, topRight.y);

        var center = new Vector3((topRight.x + botLeft.x) / 2, (topRight.y + botLeft.y) / 2, 0);
        var borders = new Vector3((xMax - xMin), (yMax - yMin), 0);

        normalizedSelectionBox.xMin = botLeft.x;
        normalizedSelectionBox.xMax = topRight.x;

        normalizedSelectionBox.yMin = botLeft.y;
        normalizedSelectionBox.yMax = topRight.y;

        var bounds = new Bounds(center, borders);
        print(bounds);
        foreach (var item in _castles)
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
        _somethingSelected = false;
        foreach (var item in _castles)
        {
            var castleScript = item.GetComponent<CastleSc>();
            if (castleScript.IsSelected)
                castleScript.Deselect();
        }
    }

    bool SelectionIsSmall()
    {
        return _selectionRect.width < 50 && _selectionRect.height < 50;
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
        GuiDrawRect(_selectionRect, Color.green);
        GUI.Box(_selectionRect, GUIContent.none, _staticRectStyle);

        //if (isPause)
        //    GUI.Window(0, MainMenu, TheMainMenu, "Pause Menu");
        // GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");
    }


    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GuiDrawRect(Rect position, Color color)
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
