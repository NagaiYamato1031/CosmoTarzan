using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	// 移動速度
	public float moveSpeed;
	// 移動速度制限
	public float MaxMoveSpeed;
	// ジャンプ高度
	public float jumpPower;
	// 空中移動速度
	public float airSpeed;
	// 空中での最大速度
	public float MaxAirSpeed;
	// ジャンプを弱める高度
	public float MaxHeight;
	// ジャンプ後に滞空するためのベクトル
	public float hoveringPower;
	// 重力の基本値
	public float Gravity = -9.8f;



	// 物理演算
	private Rigidbody rb;


	// フラグをまとめた構造体
	[System.Serializable]
	private struct Flags
	{
		// 移動入力しているか
		[SerializeField]
		public bool isInputMove;
		// ジャンプ入力しているか
		[SerializeField]
		public bool isInputJump;
		// 地面についているか
		[SerializeField]
		public bool isGround;
		// ジャンプで上昇中か
		[SerializeField]
		public bool isJumping;
	}

	// まとめた構造体
	[SerializeField]
	Flags flags_;

	// 重力ベクトル
	[SerializeField]
	Vector3 gravity = Vector3.zero;
	// 動く際に計算されているベクトル
	[SerializeField]
	Vector3 moveVelocity = Vector3.zero;
	// ジャンプベクトル
	[SerializeField]
	Vector3 jumpVelocity = Vector3.zero;

	// 入力を受け取る
	PlayerInputScript inputScript;
	// 接地判定
	PlayerGroundCheckScript groundCheckScript;

	// ワイヤー挙動
	// 名称も構造も後で変える
	TestWire testWire;

	// 追加で作成する
	// 最初からつけておけばいいと思う
	HPScript hpScript;

	// Start is called before the first frame update
	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		rb.useGravity = false;
		inputScript = gameObject.GetComponent<PlayerInputScript>();
		groundCheckScript = gameObject.GetComponent<PlayerGroundCheckScript>();
		testWire = gameObject.GetComponent<TestWire>();
		// 試験的にここで機能を追加する
		hpScript = gameObject.AddComponent<HPScript>();
	}

	// Update is called once per frame
	void Update()
	{
		// 入力をフラグ構造体へ
		flags_.isInputMove = inputScript.isInputMove;
		flags_.isInputJump = inputScript.isInputJump;
		flags_.isGround = groundCheckScript.isGround;

		// 地面にいるときにリセットをかける
		if (flags_.isGround || testWire.isWirering)
		{
			gravity = Vector3.zero;
			//jumpVelocity = Vector3.zero;
		}
		// ワイヤー中は重力を受けないようにする
		if (testWire.isWirering)
		{
			gravity = Vector3.zero;
			jumpVelocity = Vector3.zero;
		}

		// 重力を加算
		gravity.y += Gravity;

		// 移動処理
		Move();
		// ジャンプ処理
		Jump();

		// ここで全てのベクトルを合成して速度作成
		Vector3 velocity = Vector3.zero;
		velocity += gravity;
		velocity += moveVelocity;
		velocity += testWire.wireVelocity;
		velocity += jumpVelocity;

		// 速度に設定
		rb.velocity = velocity;

		// 動いた時に線を描画
		Debug.DrawLine(transform.position, transform.position + rb.velocity * 10.0f, Color.red);
	}

	private void Move()
	{
		// 速度を緩める
		moveVelocity = moveVelocity * 0.98f;
		// 速度を消す
		if (moveVelocity.magnitude < 1.0f || flags_.isGround)
		{
			moveVelocity = Vector3.zero;
		}
		// 移動させる
		if (flags_.isInputMove)
		{
			// y 成分を抜いた値で計算
			Vector3 vel = moveVelocity;
			vel.y = 0.0f;
			// 地面
			if (flags_.isGround)
			{
				// 速度制限を設ける
				if (vel.magnitude < MaxMoveSpeed)
				{
					moveVelocity = inputScript.destinate * moveSpeed;
				}
			}
			// 空中
			else
			{
				// 速度制限を設ける
				if (vel.magnitude < MaxAirSpeed)
				{
					moveVelocity = inputScript.destinate * airSpeed;
				}
			}
			vel = rb.velocity;
			vel.y = 0.0f;
			inputScript.destinate = vel.normalized;
		}
		// 向いている方向を設定
		transform.LookAt(transform.position + inputScript.destinate);
	}

	// ジャンプした元の位置を保存
	private float jumpPosition = 0.0f;

	private void Jump()
	{
		if (flags_.isInputJump && flags_.isGround)
		{
			// ポジション保存
			jumpPosition = transform.position.y;
			flags_.isJumping = true;
			// ベクトル加算
			jumpVelocity.y = jumpPower;
		}
		// ジャンプしていて
		if (flags_.isJumping)
		{
			// 目的の高さまで上昇した時
			if (jumpPosition + MaxHeight < transform.position.y)
			{
				// ジャンプした情報を消す
				flags_.isJumping = false;
				jumpVelocity.y = hoveringPower;
			}
		}

		// 上昇中でない時
		if (!flags_.isJumping)
		{
			// 地面にいる時
			if (flags_.isGround)
			{
				jumpVelocity = Vector3.zero;
			}
		}
	}
}