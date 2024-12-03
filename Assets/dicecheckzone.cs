using UnityEngine;

public class dicecheckzone : MonoBehaviour
{
    Vector3 diceVelocity;
    public static int number = 0;
    public GameController gameController;

    void FixedUpdate() 
    {
		diceVelocity = DiceScript.diceVelocity;
	}

    void OnTriggerStay(Collider other)
    {
        if (DiceScript.rolled && diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        {
            Debug.Log("Dice number: " + other.gameObject.name);
            number = int.Parse(other.gameObject.name);
            gameController.MovePlayerPiece();
            DiceScript.rolled = false;
        }
    }
}
