using UnityEngine;
using System.Collections;

public class DiceScript : MonoBehaviour
{
    public Rigidbody rb;
    public GameController gameController;
	public static Vector3 diceVelocity;
	public static bool rolled = false;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}
    void Update()
    {
		diceVelocity = rb.linearVelocity;
    }

    public void DiceRoll()
    {
        // 랜덤 힘과 회전 적용
        float forceX = Random.Range(0, 50);
        float forceY = Random.Range(800, 900);
        float forceZ = Random.Range(0, 50);

        rb.AddForce(new Vector3(forceX, forceY, forceZ));
        rb.AddTorque(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 50));

        Debug.Log("Dice rolling...");
		// rolled = true;
		Invoke("setrolled", 1.0f);
    }

	void setrolled()
	{
		rolled = true;
	}

}
