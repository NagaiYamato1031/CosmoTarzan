using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public enum BulletType
	{
		Player,
		Enemy
	}

	[SerializeField]
	private BulletType type_ = BulletType.Player;

	// Start is called before the first frame update
	void Start()
	{   
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	// 弾のタイプを設定する
	public void SetType(BulletType type)
	{
		this.type_ = type;
	}

}
