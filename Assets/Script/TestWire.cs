using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestWire : MonoBehaviour
{
	// ワイヤー場所に生成するゲームオブジェクトのプレハブ
	public GameObject wirePoint;

	// 最終的に算出されたワイヤーによる速度
	public Vector3 wireVelocity;
	// ワイヤーアクション中か判断するフラグ
	public bool isWirering = false;


	// 使うゲームオブジェクト
	private GameObject currentWire;


	// ワイヤーの長さがこれを超えると変わる
	[SerializeField]
	private float MaxLength = 40.0f;
	// 範囲内の力
	[SerializeField]
	private float inRangePower = 10.0f;
	// 範囲外の時の力
	[SerializeField]
	private float outRangePower = 40.0f;

	// 範囲外かどうかのフラグ
	[SerializeField]
	private bool isOutRange = false;

	// 実際には地形に固定するので消すもの
	[SerializeField]
	private Vector3 offset = new Vector3(0.0f, 10.0f, 0.0f);

	// Start is called before the first frame update
	void Start()
	{
		currentWire = Instantiate(wirePoint, transform.position + offset, Quaternion.identity);
		currentWire.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		wireVelocity = Vector3.zero;

		// キー入力でワイヤーの場所を固定してみる
		if (Input.GetKeyDown(KeyCode.Z))
		{
			currentWire.SetActive(true);
			currentWire.transform.position = transform.position + offset;
		}
		// ワイヤーまでの距離
		Vector3 AtoB = currentWire.transform.position - transform.position;

		// ワイヤーまでの線を描画
		if (currentWire.activeSelf)
		{
			isOutRange = MaxLength < AtoB.magnitude;
		}


		// 瞬間的に AtoB を加速度に入れる
		if (Input.GetKeyDown(KeyCode.X))
		{
			//currentWire.SetActive(false);
			wireVelocity = AtoB;
			isWirering = true;
		}
		// 継続的に AtoB を加速度に入れる
		else if (Input.GetKey(KeyCode.C))
		{
			if (isOutRange)
			{
				wireVelocity = AtoB.normalized * outRangePower;
			}
			else
			{
				wireVelocity = AtoB.normalized * inRangePower;
			}
			isWirering = true;
		}

		// ボタンを離したことを検知
		if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.C))
		{
			isWirering = false;
		}

		// ワイヤーまでの線を描画
		if (currentWire.activeSelf)
		{
			if (isOutRange)
			{
				Debug.DrawLine(currentWire.transform.position, transform.position, Color.red);
			}
			else
			{
				Debug.DrawLine(currentWire.transform.position, transform.position, Color.white);
			}
		}
	}
}
