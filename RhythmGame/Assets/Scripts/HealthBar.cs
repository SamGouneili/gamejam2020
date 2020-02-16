using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    BattleState BS;
    GameObject currImage; 

    public Sprite Health100;
    public Sprite Health75;
    public Sprite Health50;
    public Sprite Health25;
    public Sprite Health0;

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
        currImage = GameObject.Find("Health");
    }

    // Update is called once per frame
    void Update()
    {
        double currHealth = BS.GetPlayerHealth();
        if (currHealth > (BS.GetPlayerMaxHealth() * 0.75))
		{
            currImage.GetComponent<Image>().sprite = Health100;
        }
        else if (currHealth > (BS.GetPlayerMaxHealth() * 0.50))
		{
            currImage.GetComponent<Image>().sprite = Health75;
        }
        else if (currHealth > (BS.GetPlayerMaxHealth() * 0.25))
		{
            currImage.GetComponent<Image>().sprite = Health50;
        }
        else if (currHealth > (BS.GetPlayerMaxHealth() * 0.0))
		{
            currImage.GetComponent<Image>().sprite = Health25;
        }
        else if (currHealth == (BS.GetPlayerMaxHealth() * 0.0))
		{
            currImage.GetComponent<Image>().sprite = Health0;
        }
    }
}
