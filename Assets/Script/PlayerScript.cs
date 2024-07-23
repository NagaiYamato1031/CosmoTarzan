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
	// 物理演算
	private Rigidbody rb;


	// フラグをまとめた構造体
	[System.Serializable]
	private struct Flags
	{
		// 地面についているか
		[SerializeField]
		public bool isGround;
	}

	// まとめた構造体
	[SerializeField]
	Flags flags_;

	[SerializeField]
	Vector3 velocity_ = Vector3.zero;

	// 入力を受け取る
	PlayerInputScript inputScript;


	// Start is called before the first frame update
	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		inputScript = gameObject.GetComponent<PlayerInputScript>();
	}

	// Update is called once per frame
	void Update()
	{
		// 移動処理
		Move();
		// ジャンプ処理
		Jump();

		// 動いた時に線を描画
		if (inputScript.isInputMove)
		{
			Debug.DrawLine(transform.position, transform.position + inputScript.destinate * moveSpeed, Color.red);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		// 床に当たっている時
		if (collision.gameObject.tag == "Floor")
		{
			flags_.isGround = true;
		}
	}


	private void OnCollisionExit(Collision collision)
	{
		// 床から離れた時
		if (collision.gameObject.tag == "Floor")
		{
			flags_.isGround = false;
		}
	}

	private void Move()
	{
		// 移動させる
		if (inputScript.isInputMove)
		{
			// 横移動を計測
			Vector3 vel = rb.velocity;
			vel.y = 0.0f;
			// 地面
			if (flags_.isGround)
			{
				// 速度制限を設ける
				if (vel.magnitude < MaxMoveSpeed)
				{
					velocity_ = inputScript.destinate * moveSpeed;
				}
			}
			// 空中
			else
			{
				// 速度制限を設ける
				if (vel.magnitude < MaxAirSpeed)
				{
					velocity_ = inputScript.destinate * airSpeed;
				}
			}
			velocity_.y = rb.velocity.y;
		}
		else
		{
			velocity_.x = 0.0f;
			velocity_.z = 0.0f;
			velocity_.y = rb.velocity.y;
		}
		rb.velocity = velocity_;
		transform.LookAt(transform.position + inputScript.destinate);

	}
	private void Jump()
	{
		if (inputScript.isInputJump && flags_.isGround)
		{
			rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
			rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
		}
	}
}