using TMPro;
using UnityEngine;

public class Point : MonoBehaviour
{
    public int number { get; private set; }
    private Transform pointTransform;
    public TextMeshProUGUI numbetText;
   
    public void Init(int value,string number)
    {
        this.number = value;
        this.numbetText.text = number;
        pointTransform = GetComponent<Transform>();
        gameObject.name = "Point " + value;
    }
    public Transform PointPosition
    {
        get { return pointTransform; }
    }
}
