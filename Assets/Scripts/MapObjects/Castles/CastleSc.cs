using System;
using Assets.Scripts.MapObjects.Castles;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.MapObjects.Voronoi;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class CastleSc : MonoBehaviour, IHaveOwner
{
    public CastleType CastleType;
    public int CastleNumber;

    CastleInfo _info = new CastleInfo();
    BasePlayer _currentOwner;
    TextMeshPro _populationText;
    Gradient _gradient;
    SpriteRenderer _circleSprite, _castleColorSprite;
    GameObject _mapCastles;

    VoronoiLakeSc _voronoiLake;
    VoronoiLakeSc VoronoiLake
    {
        get { _voronoiLake ??= gameObject.transform.Find("VoronoiLake").gameObject.GetComponent<VoronoiLakeSc>(); return _voronoiLake; }
    }


    public CastleSc()
    {
        //Border = new List<(Vector3, Vector3)>();
    }

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
    }




    void Update()
    {

        if (SettingsSc.IsPaused)
        {
            BuildBorders();
            return;
        }

       

        if (CurrentPopulation < _info.BasePopulation)
            CurrentPopulation += _info.growthRate;
        _populationText.text = Mathf.RoundToInt(CurrentPopulation).ToString();
        if (_info.selected)
        {
            var timeFraction = Time.realtimeSinceStartup - (int)Time.realtimeSinceStartup;
            _info.CurrentColor = _gradient.Evaluate(timeFraction);
            //_circleSprite.color = _info.CurrentColor;
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
        //_circleSprite.color = _info.CastleColor;
        if (_castleColorSprite != null)
            _castleColorSprite.color = _info.CurrentColor;
    }

    public void SetColor(Color col)
    {
        _info.CastleColor = col;
        _info.CurrentColor = _info.CastleColor;

       // _circleSprite.color = _info.CastleColor;
        if (_castleColorSprite != null)
            _castleColorSprite.color = _info.CastleColor;

        //VoronoiLake.SetColor(col);
        //var castleRegion = GetComponentInChildren<LakeSc>();
        //castleRegion?.SetColor(col);
    }

    public void SetBaseOwner(BasePlayer owner)
    {
        _info.Owner = owner;
        SetOwner(owner);
    }

    public void SetOwner(BasePlayer newOwner)
    {
        VoronoiLake.SetOwner(newOwner);
        _currentOwner?.LostCity(gameObject);
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

    public void BuildBorders()
    {
        VoronoiLake.BuildBorders();
    }

    public void AddBorderLine((Vector3, Vector3) border)
    {
        VoronoiLake.Border.Add(border);
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

    public bool IsSelected => _info.selected;


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
