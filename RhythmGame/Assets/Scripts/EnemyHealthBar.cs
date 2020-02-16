using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    BattleState BS;
    GameObject currImage;

    public Sprite HealthFull;
    public Sprite Health2;
    public Sprite Health3;
    public Sprite Health4;
    public Sprite Health5;
    public Sprite Health6;
    public Sprite Health75percent;
    public Sprite Health8;
    public Sprite Health9;
    public Sprite Health10;
    public Sprite Health11;
    public Sprite Health12;
    public Sprite Health50percent;
    public Sprite Health14;
    public Sprite Health15;
    public Sprite Health16;
    public Sprite Health17;
    public Sprite Health18;
    public Sprite Health25percent;
    public Sprite Health20;
    public Sprite Health21;
    public Sprite Health22;
    public Sprite Health23;
    public Sprite Health24;
    public Sprite Health25;
    public Sprite Health26;
    public Sprite Health0;

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
        currImage = GameObject.Find("EnemyHealth");
    }

    // Update is called once per frame
    void Update()
    {
        double currHealth = BS.GetEnemyHealth();
        double maxHealth = BS.GetEnemyMaxHealth();
        if (currHealth == (maxHealth))
        {
            currImage.GetComponent<Image>().sprite = HealthFull;
        }
        else if (currHealth >= (maxHealth / 27 * 25))
        {
            currImage.GetComponent<Image>().sprite = Health2;
        }
        else if (currHealth >= (maxHealth / 27 * 24))
        {
            currImage.GetComponent<Image>().sprite = Health3;
        }
        else if (currHealth >= (maxHealth / 27 * 23))
        {
            currImage.GetComponent<Image>().sprite = Health4;
        }
        else if (currHealth >= (maxHealth / 27 * 22))
        {
            currImage.GetComponent<Image>().sprite = Health5;
        }
        else if (currHealth >= (maxHealth / 27 * 21))
        {
            currImage.GetComponent<Image>().sprite = Health6;
        }
        else if (currHealth >= (maxHealth / 27 * 20))
        {
            currImage.GetComponent<Image>().sprite = Health75percent;
        }
        else if (currHealth >= (maxHealth / 27 * 19))
        {
            currImage.GetComponent<Image>().sprite = Health8;
        }
        else if (currHealth >= (maxHealth / 27 * 18))
        {
            currImage.GetComponent<Image>().sprite = Health9;
        }
        else if (currHealth >= (maxHealth / 27 * 17))
        {
            currImage.GetComponent<Image>().sprite = Health10;
        }
        else if (currHealth >= (maxHealth / 27 * 16))
        {
            currImage.GetComponent<Image>().sprite = Health11;
        }
        else if (currHealth >= (maxHealth / 27 * 15))
        {
            currImage.GetComponent<Image>().sprite = Health12;
        }
        else if (currHealth >= (maxHealth / 27 * 14))
        {
            currImage.GetComponent<Image>().sprite = Health50percent;
        }
        else if (currHealth >= (maxHealth / 27 * 13))
        {
            currImage.GetComponent<Image>().sprite = Health14;
        }
        else if (currHealth >= (maxHealth / 27 * 12))
        {
            currImage.GetComponent<Image>().sprite = Health15;
        }
        else if (currHealth >= (maxHealth / 27 * 11))
        {
            currImage.GetComponent<Image>().sprite = Health16;
        }
        else if (currHealth >= (maxHealth / 27 * 10))
        {
            currImage.GetComponent<Image>().sprite = Health17;
        }
        else if (currHealth >= (maxHealth / 27 * 9))
        {
            currImage.GetComponent<Image>().sprite = Health18;
        }
        else if (currHealth >= (maxHealth / 27 * 8))
        {
            currImage.GetComponent<Image>().sprite = Health25percent;
        }
        else if (currHealth >= (maxHealth / 27 * 7))
        {
            currImage.GetComponent<Image>().sprite = Health20;
        }
        else if (currHealth >= (maxHealth / 27 * 6))
        {
            currImage.GetComponent<Image>().sprite = Health21;
        }
        else if (currHealth >= (maxHealth / 27 * 5))
        {
            currImage.GetComponent<Image>().sprite = Health22;
        }
        else if (currHealth >= (maxHealth / 27 * 4))
        {
            currImage.GetComponent<Image>().sprite = Health23;
        }
        else if (currHealth >= (maxHealth / 27 * 3))
        {
            currImage.GetComponent<Image>().sprite = Health24;
        }
        else if (currHealth >= (maxHealth / 27 * 2))
        {
            currImage.GetComponent<Image>().sprite = Health25;
        }
        else if (currHealth >= (maxHealth / 27))
        {
            currImage.GetComponent<Image>().sprite = Health26;
        }
        else if (currHealth < (maxHealth / 27))
        {
            currImage.GetComponent<Image>().sprite = Health0;
        }
    }
}
