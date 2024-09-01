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
		// カーソルを中央に固定して表示を消す
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		// ESC キーが押された時
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		// 左クリックしたとき
		else if (Input.GetMouseButtonDown(0))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// カーソルが見えている時は更新しない
		if (Cursor.visible)
		{
			return;
		}


		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		// X方向に一定量移動していれば横回転
		if (0.01f < Mathf.Abs(mx))
		{
			// 回転軸はワールド座標のY軸
			transform.RotateAround(player.position, Vector3.up, mx);
		}

		// Y方向に一定量移動していれば縦回転
		if (0.01f < Mathf.Abs(my))
		{
			// 回転軸はカメラ自身のX軸
			transform.RotateAround(player.position, transform.right, -my);
		}
		// プレイヤーに追従させる
		transform.position = player.position - transform.forward * offset.magnitude;

	}
}
