using System;
using UnityEngine;

public class Projectile : PoolItemBase {
	/// <summary>
	/// 命中目标后的回调方法
	/// </summary>
	public Action<Projectile> callBack;

	/// <summary>
	/// 发射时候播放的音效
	/// </summary>
	public AudioClip launchAudioClip;

	/// <summary>
	/// 声音大小
	/// </summary>
	public float launchAudioVolume = 1f;

	private Rigidbody2D rigidbody2d;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {
		this.rigidbody2d = this.gameObject.GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
	}

	/// <summary>
	/// 子弹的发射方法
	/// </summary>
	/// <param name="startPostion">子弹的初始位置</param>
	/// <param name="direction">子弹的飞行方向</param>
	/// <param name="force">作用力</param>
	/// <param name="callBack">命中目标后的回调</param>
	public void Launch(Vector2 startPostion, Vector2 direction, float force, Action<Projectile> callBack = null) {
		this.callBack = callBack;
		this.rigidbody2d.position = startPostion;
		// 添加一个力，使其移动
		this.rigidbody2d.AddForce(direction * force);
		// 播放音效
		AudioSource.PlayClipAtPoint(this.launchAudioClip, startPostion, this.launchAudioVolume);
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this, 命中目标回调。
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other) {
		// 使用了图层设置，将子弹和玩家分成了两个图层，并且设置了这两个图层之间不会相互碰撞Player / Projectile
		this.callBack?.Invoke(this);
		ProjectilePoolManager.Instance.Retrieve(this);
		EnemyControllerBase enemyControllerBase = other.GetComponent<EnemyControllerBase>();
		// TODO 默认先用1作为伤害值
		enemyControllerBase?.OnHit(1);
	}

	/// <summary>
	/// 当对象被回收时候调用的函数
	/// </summary>
	public override void onRetrieve() {
		this.gameObject.SetActive(false);
		this.rigidbody2d.Sleep();
	}

	/// <summary>
	/// 重置对象状态以便复用
	/// </summary>
	public override void ResetState() {
		this.gameObject.SetActive(true);
		this.rigidbody2d.WakeUp();
	}
}
