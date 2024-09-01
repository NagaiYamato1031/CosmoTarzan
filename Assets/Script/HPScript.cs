using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
	// 体力
	[SerializeField]
	private float hp_ = 100.0f;

	// 死んだときの関数の定義
	public delegate void DeadDelegate();

	// 実際の関数
	DeadDelegate deadFunction = null;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	// 初期値を入力
	public void InitHP(float hp = 100.0f)
	{
		hp_ = hp;
	}

	// 死んだときに使う関数をセットする
	public void SetFunction(DeadDelegate deadMethod)
	{
		deadFunction = deadMethod;
	}


	// 体力を減らす
	public void Damage(float damage)
	{
		hp_ -= damage;
		// 体力が無くなった時
		if (hp_ < 0)
		{
			hp_ = 0;
			deadFunction();
		}
	}

}
