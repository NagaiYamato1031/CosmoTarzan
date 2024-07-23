using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    // プレイヤーを追従する
    public Transform player;
    // オブジェクトとのオフセット
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
		// 初期オフセットの計算
		offset = transform.position - player.position;
	}

    // Update is called once per frame
    void Update()
    {
		// プレイヤーを中心にカメラを回転
		Quaternion rotation = Quaternion.Euler(0, 0, 0);
		transform.position = player.position + rotation * offset;
		transform.LookAt(player.position);
	}
}
