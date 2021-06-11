using Assets.Scripts.MapObjects.Castles;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CastleSc : MonoBehaviour
{
    public CastleType CastleType;
    public int CastleNumber;

    CastleInfo _info = new CastleInfo();
    BasePlayer _currentOwner;
    TextMeshPro _populationText;
    Gradient _gradient;
    SpriteRenderer _circleSprite, _castleColorSprite;
    GameObject _mapCastles;

    void Awake()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _circleSprite = spriteRenderers.First(render => render.name == "CircleSprite");
        //Цвет щитка в центре замка
        if (spriteRenderers.Any(render => render.name == "ColorSprite"))
            _castleColorSprite = spriteRenderers.First(render => render.name == "ColorSprite");
    }

    void Start()
    {
        _mapCastles = GameObject.Find("Castles");
        InitText();
        _info.selected = false;
        _info.Type = CastleType;

        _gradient = new Gradient();

        var colorKey = new[]{
            new GradientColorKey(Color.white, 0),
            new GradientColorKey(Color.black, 0.5f),
            new GradientColorKey(Color.white,1)
        };

        var alphaKey = new[]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        };

        _gradient.SetKeys(colorKey, alphaKey);


        transform.SetParent(_mapCastles.transform);

    }

    public void InitText()
    {
        _populationText = GetComponentInChildren<TextMeshPro>();
        //var widthBy2 = SettingsSc.ScreenWidth / 2;
        //var heightBy2 = SettingsSc.ScreenHeight / 2;

        //   float x = (widthBy2 + transform.position.x) / (SettingsSc.ScreenWidth);
        //    float y = (heightBy2 + transform.position.y) / (SettingsSc.ScreenHeight);

        //   y -= 0.0003f;
        //   y -= 0.05f;

        //_label.transform.SetParent(_mapCastles.transform);
        //_label.transform.position = new Vector3(Mathf.Abs(x), Mathf.Abs(y), 0.0f);
        //_populationText = _label.GetComponent<UnityEngine.UI.Text>();
        //_populationText.text = _info.BasePopulation.ToString();
        //populationText = TextAnchor.UpperCenter;

        //label.transform.parent = mapCastles.transform;
    }


    void Update()
    {

        if (SettingsSc.IsPaused)
            return;

        if (CurrentPopulation < _info.BasePopulation)
            CurrentPopulation += _info.growthRate;
        _populationText.text = Mathf.RoundToInt(CurrentPopulation).ToString();
        if (_info.selected)
        {
            var timeFraction = Time.realtimeSinceStartup - (int)Time.realtimeSinceStartup;
            _info.CurrentColor = _gradient.Evaluate(timeFraction);
            _circleSprite.color = _info.CurrentColor;
            if (_castleColorSprite != null)
                _castleColorSprite.color = _info.CurrentColor;
        }
    }

    public void Select()
    {
        _currentOwner.ControlledCities.Add(gameObject);
        _info.selected = true;
    }

    public void Deselect()
    {
        _info.selected = false;
        _currentOwner.DeselectCity(gameObject);
        _circleSprite.color = _info.CastleColor;
        if (_castleColorSprite != null)
            _castleColorSprite.color = _info.CurrentColor;
    }

    void SetColor(Color col)
    {
        _info.CastleColor = col;
        _info.CurrentColor = _info.CastleColor;

        _circleSprite.color = _info.CastleColor;
        if (_castleColorSprite != null)
            _castleColorSprite.color = _info.CastleColor;

        var castleRegion = GetComponentInChildren<LakeSc>();
        castleRegion.SetColor(col);
    }

    public void SetBaseOwner(BasePlayer owner)
    {
        _info.Owner = owner;
        SetOwner(owner);
    }

    public void SetOwner(BasePlayer newOwner)
    {
        if (_currentOwner != null)
            _currentOwner.LostCity(gameObject);
        _currentOwner = newOwner;
        UpdateGrowthRate();
        _currentOwner.ControlledCities.Add(gameObject);
        SetColor(newOwner.GetPlayerColor());
        Deselect();
    }

    public BasePlayer GetOwner()
    {
        return _currentOwner;
    }


    public float CurrentPopulation
    {
        get => _info.CurrentPopulation;
        set => _info.CurrentPopulation = value;
    }

    public float BasePopulation
    {
        get
        {
            return _info.BasePopulation;
        }
        set
        {
            _info.BasePopulation = value;
        }
    }

    //public CastleSize CastleType
    //{
    //    get { return _info.Type; }
    //    set { _info.Type = value; }
    //}

    public bool IsSelected
    {
        get
        {
            return _info.selected;
        }
    }


    internal void Visit(ArmySc armyScript)
    {
        if (armyScript.GetOwner() == _currentOwner)
        {
            if (armyScript.Destination == gameObject)
            {
                CurrentPopulation += armyScript.Amount;
                armyScript.Destroy();
                return;
            }
            else
                return;
        }

        if (armyScript.Destination != gameObject && _currentOwner.IsGaia)
            return;


        if (armyScript.Amount > CurrentPopulation)
        {
            _currentOwner.LostCity(gameObject);
            SetOwner(armyScript.GetOwner());
            armyScript.GetOwner().ControlledCities.Add(gameObject);
            //setOwner(armyInput.getOwner());
        }

        CurrentPopulation = Mathf.Abs(CurrentPopulation - armyScript.Amount);
        armyScript.Destroy();



    }

    internal void SetBasePopulation(float p)
    {
        _info.BasePopulation = p;
        CurrentPopulation = _currentOwner.IsGaia
            ? _info.BasePopulation / 20
            :_info.BasePopulation;
        UpdateGrowthRate();

    }

    void UpdateGrowthRate()
    {
        if (_currentOwner == null)
            return;
        _info.growthRate = _currentOwner.IsGaia 
            ? _info.BasePopulation / 8000 
            : _info.BasePopulation / 2000;
    }

    internal void UpdateTransformInfo(Transform transform)
    {
        _info.Position = transform;
    }

    public CastleInfo GetInfo()
    {
        return _info;
    }
}
