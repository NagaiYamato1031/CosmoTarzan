using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class PlayerGroundCheckScript : MonoBehaviour
{
	// 地面についているか
	public bool isGround;

	private void OnCollisionStay(Collision collision)
	{
		// 床に当たっている時
		if (collision.gameObject.tag == "Floor")
		{
			isGround = true;
		}
	}


	private void OnCollisionExit(Collision collision)
	{
		// 床から離れた時
		if (collision.gameObject.tag == "Floor")
		{
			isGround = false;
		}
	}

}
