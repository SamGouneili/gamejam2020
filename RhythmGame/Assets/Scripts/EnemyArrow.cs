using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyArrow : MonoBehaviour
{
    BattleState BS;
    GameObject currImage;

    public Sprite UP;
    public Sprite DOWN;
    public Sprite RIGHT;
    public Sprite LEFT;
    public Sprite X;

    // Start is called before the first frame update
    void Start()
    {
        BS = FindObjectOfType<BattleState>();
        currImage = GameObject.Find("EnemyArrow");
    }

    // Update is called once per frame
    void Update()
    {
        BattleState.AttackDirection currDir = BS.GetEnemyAttackDirection();
        if (currDir == BattleState.AttackDirection.Up)
        {
            currImage.GetComponent<Image>().sprite = UP;
        }
        else if (currDir == BattleState.AttackDirection.Down)
        {
            currImage.GetComponent<Image>().sprite = DOWN;
        }
        else if (currDir == BattleState.AttackDirection.Right)
        {
            currImage.GetComponent<Image>().sprite = RIGHT;
        }
        else if (currDir == BattleState.AttackDirection.Left)
        {
            currImage.GetComponent<Image>().sprite = LEFT;
        }
        else if (currDir == BattleState.AttackDirection.None)
        {
            currImage.GetComponent<Image>().sprite = X;
        }
    }
}
