using System;
using Assets.Scripts.MapObjects.Castles;
using Assets.Scripts.Players;
using System.Linq;
using Assets.Scripts.Events;
using Assets.Scripts.Interfaces;
using TMPro;
using UnityEngine;

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

    BorderLakeSc _voronoiLake;
    BorderLakeSc VoronoiLake
    {
        get { _voronoiLake ??= gameObject.transform.Find("VoronoiLake").gameObject.GetComponent<BorderLakeSc>(); return _voronoiLake; }
    }

    CastleBorderBuilderSc _castleBorderBuilder;
    CastleBorderBuilderSc CastleBorders
    {
        get { _castleBorderBuilder ??= gameObject.GetComponentInChildren<CastleBorderBuilderSc>(); return _castleBorderBuilder; }
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

        //CastleBorders.SetInitialColor(col);

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

        OwnerChanged?.Invoke(this, new OwnerChangedEventArgs(_currentOwner, newOwner));
    }

    public BasePlayer GetOwner()
    {
        return _currentOwner;
    }



    public void BuildBorders()
    {
        if (SettingsSc.IsPaused)
        {
            CastleBorders.Rebuild();
        }

        VoronoiLake.BuildBorders();
       // CastleBorderBuilder
    }

    public void AddBorderLine((Vector3, Vector3) border, CastleSc oppositeCastle)
    {
        CastleBorders.AddBorderLine(border, gameObject, oppositeCastle);
        VoronoiLake.Border.Add(border);
    }

    public float CurrentPopulation
    {
        get => _info.CurrentPopulation;
        set => _info.CurrentPopulation = value;
    }

    public float BasePopulation
    {
        get => _info.BasePopulation;
        set => _info.BasePopulation = value;
    }

    public event EventHandler<OwnerChangedEventArgs> OwnerChanged;

    public bool IsSelected => _info.selected;


    internal void Visit(ArmyParticleSc armyScript)
    {
        if (armyScript.GetOwner() == _currentOwner)
        {
            if (armyScript.Destination == gameObject)
            {
                CurrentPopulation += armyScript.Amount;
                armyScript.Destroy();
                return;
            }

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
