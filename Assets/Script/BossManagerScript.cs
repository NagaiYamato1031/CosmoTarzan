using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManagerScript : MonoBehaviour
{
	// プレイヤーの位置を捕捉する
	public GameObject player;

	// 体力
	HPScript hpScript;

	// Start is called before the first frame update
	void Start()
	{
		hpScript = gameObject.AddComponent<HPScript>();
	}

	// Update is called once per frame
	void Update()
	{
		// 同じ高さにして x 軸の角度を変えない
		Vector3 playerPos = player.transform.position;
		playerPos.y = transform.position.y;
		transform.LookAt(playerPos);
	}
}
