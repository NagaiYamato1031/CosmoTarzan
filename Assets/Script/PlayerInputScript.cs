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

	// 接地判定
	//private PlayerGroundCheckScript groundCheckScript;

	private void Start()
	{
		//groundCheckScript = gameObject.GetComponent<PlayerGroundCheckScript>();
	}

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
		// スティック入力も受け取る
		// 横方向
		Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		Vector2 tmp = axis;
		// 大きさを取る
		// デッドゾーン以下の時
		if (-deadZone < axis.x && axis.x < deadZone)
		{
			tmp.x = 0.0f;
			axis.x = 0.0f;
		}
		// 前回の入力より絶対値が小さい時
		else if ((axis.x < 0.0f ? -axis.x : axis.x) < (prevAxis.x < 0.0f ? -prevAxis.x : prevAxis.x))
		{
			axis.x = 0.0f;
		}
		// 縦方向
		if (-deadZone < axis.y && axis.y < deadZone)
		{
			tmp.y = 0.0f;
			axis.y = 0.0f;
		}
		else if ((axis.y < 0.0f ? -axis.y : axis.y) < (prevAxis.y < 0.0f ? -prevAxis.y : prevAxis.y))
		{
			axis.y = 0.0f;
		}
		// 値を取得
		prevAxis = tmp;
		// 方向が入力されていない時
		if (axis.Equals(Vector3.zero))
		{
			isInputMove = false;
			return;
		}
		// 左右
		Vector3 moveX = Camera.main.transform.right * axis.x;
		// 前後
		Vector3 moveZ = Camera.main.transform.forward * axis.y;

		// ベクトル合算
		direct = moveX + moveZ;
		direct.y = 0;

		// 情報を格納
		destinate = direct.normalized;
		isInputMove = true;
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
