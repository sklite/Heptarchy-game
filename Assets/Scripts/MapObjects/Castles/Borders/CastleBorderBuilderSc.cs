using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CastleBorderBuilderSc : MonoBehaviour
{
    public GameObject BorderPrefab;

    private List<BorderLineSc> _borderLines = new List<BorderLineSc>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBorderLine((Vector3, Vector3) border, GameObject castle)
    {
        var newBorder = Instantiate(BorderPrefab, castle.transform, true);// Quaternion.Euler(new Vector3(0, 0, 0)));
        newBorder.transform.SetParent(gameObject.transform);
        newBorder.transform.localPosition = Vector3.zero;
        newBorder.transform.localScale = Vector3.one;

        var spline = newBorder.GetComponent<SpriteShapeController>().spline;
        spline.Clear();

        //var localA = transform.InverseTransformPoint(border.Item1);
        //spline.InsertPointAt(0, border.Item1);

        //var localB = transform.InverseTransformPoint(border.Item2);
        //spline.InsertPointAt(1, border.Item2);
        var localA = transform.InverseTransformPoint(border.Item1);
        spline.InsertPointAt(0, localA);

        var localB = transform.InverseTransformPoint(border.Item2);
        spline.InsertPointAt(1, localB);

        _borderLines.Add(newBorder.GetComponent<BorderLineSc>());
    }

}
