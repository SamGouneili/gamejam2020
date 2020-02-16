using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
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
        currImage = GameObject.Find("PlayerAttackDirection");
    }

    // Update is called once per frame
    void Update()
    {
        BattleState.AttackDirection currDir = BS.GetCurrentAttackDirection();
        if (currDir == BattleState.AttackDirection.Up)
        {
            currImage.GetComponent<SpriteRenderer>().sprite = UP;
        }
        else if (currDir == BattleState.AttackDirection.Down)
        {
            currImage.GetComponent<SpriteRenderer>().sprite = DOWN;
        }
        else if (currDir == BattleState.AttackDirection.Right)
        {
            currImage.GetComponent<SpriteRenderer>().sprite = RIGHT;
        }
        else if (currDir == BattleState.AttackDirection.Left)
        {
            currImage.GetComponent<SpriteRenderer>().sprite = LEFT;
        }
        else if (currDir == BattleState.AttackDirection.None)
        {
            currImage.GetComponent<SpriteRenderer>().sprite = X;
        }
    }
}
