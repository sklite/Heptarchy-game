using Assets.Scripts.MapObjects.Castles;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CastleSc : MonoBehaviour
{
    CastleInfo info = new CastleInfo();

    BasePlayer currentOwner;

    GameObject label;
    UnityEngine.UI.Text populationText;
    Gradient gradient;

    SpriteRenderer circleSprite, castleColorSprite;

    GameObject mapCastles;

    void Awake()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        circleSprite = spriteRenderers.First(render => render.name == "CircleSprite");
        //Цвет щитка в центре замка
        if (spriteRenderers.Any(render => render.name == "ColorSprite"))
            castleColorSprite = spriteRenderers.First(render => render.name == "ColorSprite");
    }

    void Start()
    {


        mapCastles = GameObject.Find("Castles");
        InitText();
        info.selected = false;

        gradient = new Gradient();

        var colorKey = new[]{
            new GradientColorKey(Color.white, 0),
            new GradientColorKey(Color.black, 0.5f),
            new GradientColorKey(Color.white,1)
        };

        var alphaKey = new[]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        };

        gradient.SetKeys(colorKey, alphaKey);


        transform.SetParent(mapCastles.transform);

    }

    public void InitText()
    {

        var widthBy2 = SettingsSc.ScreenWidth / 2;
        var heightBy2 = SettingsSc.ScreenHeight / 2;

        float x = (widthBy2 + this.transform.position.x) / (SettingsSc.ScreenWidth);
        float y = (heightBy2 + this.transform.position.y) / (SettingsSc.ScreenHeight);

        y -= 0.0003f;
        y -= 0.05f;

        label = new GameObject("CastleSc population label");
        label.AddComponent<UnityEngine.UI.Text>();

        label.transform.SetParent(mapCastles.transform);
        label.transform.position = new Vector3(Mathf.Abs(x), Mathf.Abs(y), 0.0f);
        populationText = label.GetComponent<UnityEngine.UI.Text>();
        populationText.text = info.BasePopulation.ToString();
        //populationText = TextAnchor.UpperCenter;

        //label.transform.parent = mapCastles.transform;
    }


    void Update()
    {

        if (SettingsSc.IsPaused)
            return;

        if (CurrentPopulation < info.BasePopulation)
            CurrentPopulation += info.growthRate;
        populationText.text = Mathf.RoundToInt(CurrentPopulation).ToString();
        if (info.selected)
        {
            var timeFraction = Time.realtimeSinceStartup - (int)Time.realtimeSinceStartup;
            info.CurrentColor = gradient.Evaluate(timeFraction);
            circleSprite.color = info.CurrentColor;
            if (castleColorSprite != null)
                castleColorSprite.color = info.CurrentColor;
        }
    }

    public void Select()
    {
        currentOwner.ControlledCities.Add(gameObject);
        info.selected = true;
    }

    public void Deselect()
    {
        info.selected = false;
        currentOwner.DeselectCity(gameObject);
        circleSprite.color = info.CastleColor;
        if (castleColorSprite != null)
            castleColorSprite.color = info.CurrentColor;
    }

    void SetColor(Color col)
    {
        info.CastleColor = col;
        info.CurrentColor = info.CastleColor;

        circleSprite.color = info.CastleColor;
        if (castleColorSprite != null)
            castleColorSprite.color = info.CastleColor;
    }

    public void SetBaseOwner(BasePlayer owner)
    {
        info.Owner = owner;
        SetOwner(owner);
    }

    public void SetOwner(BasePlayer newOwner)
    {
        if (currentOwner != null)
            currentOwner.LostCity(gameObject);
        currentOwner = newOwner;
        UpdateGrowthRate();
        currentOwner.ControlledCities.Add(gameObject);
        SetColor(newOwner.GetPlayerColor());
        Deselect();
    }

    public BasePlayer GetOwner()
    {
        return currentOwner;
    }


    public float CurrentPopulation
    {
        get
        {
            return info.CurrentPopulation;
        }
        set
        {
            info.CurrentPopulation = value;
        }
    }

    public float BasePopulation
    {
        get
        {
            return info.BasePopulation;
        }
        set
        {
            info.BasePopulation = value;
        }
    }

    public CastleSize CastleType
    {
        get { return info.Type; }
        set { info.Type = value; }
    }

    public bool IsSelected
    {
        get
        {
            return info.selected;
        }
    }


    internal void Visit(ArmySc armyScript)
    {
        if (armyScript.GetOwner() == currentOwner)
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

        if (armyScript.Destination != gameObject && currentOwner.IsGaia)
            return;


        if (armyScript.Amount > CurrentPopulation)
        {
            currentOwner.LostCity(gameObject);
            SetOwner(armyScript.GetOwner());
            armyScript.GetOwner().ControlledCities.Add(gameObject);
            //setOwner(armyInput.getOwner());
        }

        CurrentPopulation = Mathf.Abs(CurrentPopulation - armyScript.Amount);
        armyScript.Destroy();



    }

    internal void SetBasePopulation(float p)
    {
        info.BasePopulation = p;
        CurrentPopulation = info.BasePopulation;
        UpdateGrowthRate();

    }

    void UpdateGrowthRate()
    {
        if (currentOwner == null)
            return;
        if (currentOwner.IsGaia)
            info.growthRate = info.BasePopulation / 8000;
        else
            info.growthRate = info.BasePopulation / 2000;
    }

    internal void UpdateTransformInfo(Transform transform)
    {
        info.Position = transform;
    }

    public CastleInfo GetInfo()
    {
        return info;
    }
}
