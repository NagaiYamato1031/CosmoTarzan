using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class PlayerInputScript : MonoBehaviour
{
	// 移動入力されているか
	[SerializeField]
	public bool isInputMove;
	// ジャンプが入力されたか
	[SerializeField]
	public bool isInputJump;
	// 移動入力の最低値
	[SerializeField]
	public float deadZone = 0.2f;
	// 入力された方向
	[SerializeField]
	public Vector3 destinate = Vector3.forward;

	// キーボード入力は値が徐々に上がるので、
	// 下がり始めたら検知できるように値を格納する
	[SerializeField]
	private Vector2 prevAxis = Vector2.zero;
	// 前回の入力値との差を格納
	[SerializeField]
	private Vector2 prevCollect = Vector2.zero;

	// 移動入力を計算
	void Update()
	{
		// 移動入力を検知
		CheckInputMove();
		// ジャンプ入力を検知
		CheckInputJump();
	}


	// 移動入力を計算
	private void CheckInputMove()
	{
		Vector3 direct;

		// スティック入力を受け取る
		// 横方向
		Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		// 大きさを取る
		if (-deadZone < axis.x && axis.x < deadZone &&
			(axis.x <= prevCollect.x || prevCollect.x <= axis.x))
		{
			axis.x = 0.0f;
		}
		// 左右
		Vector3 moveX = Camera.main.transform.right * axis.x;
		// 縦方向
		// 大きさを取る
		if (-deadZone < axis.y && axis.y < deadZone &&
			(axis.y <= prevCollect.y || prevCollect.y <= axis.y))
		{
			axis.y = 0.0f;
		}
		// 前後
		Vector3 moveZ = Camera.main.transform.forward * axis.y;

		// 前回の入力値との差を保存する
		prevCollect = axis - prevAxis;
		// 値を取得
		prevAxis = axis;

		//moveX.Normalize();
		//moveZ.Normalize();
		// ベクトル合算
		direct = moveX + moveZ;
		direct.y = 0;
		direct.Normalize();

		// 方向が入力されている時
		if (direct.Equals(Vector3.zero))
		{
			isInputMove = false;
		}
		else
		{
			// 情報を格納
			destinate = direct;
			isInputMove = true;
		}
	}

	private void CheckInputJump()
	{
		// スペースとパッドの  に設定する
		if (Input.GetButtonDown("Jump"))
		{
			isInputJump = true;
		}
		else
		{
			// 押していない状態に戻す
			isInputJump = false;
		}
	}
}
