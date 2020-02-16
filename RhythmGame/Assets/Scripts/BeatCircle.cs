using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatCircle : MonoBehaviour
{
    BattleState BS;
    GameObject currImage;

    const double TIME_BETWEEN_IMAGES = 0.046153846153;

    double TimeLeft = 0.46153846153;
    bool EnableUpdate = false;

    public Sprite CircleFull;
    public Sprite Circle9;
    public Sprite Circle8;
    public Sprite Circle7;
    public Sprite Circle6;
    public Sprite Circle5;
    public Sprite Circle4;
    public Sprite Circle3;
    public Sprite Circle2;
    public Sprite CircleEmpty;

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
        currImage = GameObject.Find("BeatNotify");
    }

    private void Update()
    {
        if (EnableUpdate)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0)
            {
                currImage.GetComponent<Image>().sprite = CircleEmpty;
                TimeLeft = 0.46153846153;
            }
            else if (TimeLeft <= 0.046153846153)
            {
                currImage.GetComponent<Image>().sprite = Circle2;
            }
            else if (TimeLeft <= 0.046153846153 * 2)
            {
                currImage.GetComponent<Image>().sprite = Circle3;
            }
            else if (TimeLeft <= 0.046153846153 * 3)
            {
                currImage.GetComponent<Image>().sprite = Circle4;
            }
            else if (TimeLeft <= 0.046153846153 * 4)
            {
                currImage.GetComponent<Image>().sprite = Circle5;
            }
            else if (TimeLeft <= 0.046153846153 * 5)
            {
                currImage.GetComponent<Image>().sprite = Circle6;
            }
            else if (TimeLeft <= 0.046153846153 * 6)
            {
                currImage.GetComponent<Image>().sprite = Circle7;
            }
            else if (TimeLeft <= 0.046153846153 * 7)
            {
                currImage.GetComponent<Image>().sprite = Circle8;
            }
            else if (TimeLeft <= 0.046153846153 * 8)
            {
                currImage.GetComponent<Image>().sprite = Circle9;
            }
            else if (TimeLeft <= 0.046153846153 * 9)
            {
                currImage.GetComponent<Image>().sprite = CircleFull;
            }
        }
    }

    public void CycleImages()
    {
        EnableUpdate = true;
    }
}
