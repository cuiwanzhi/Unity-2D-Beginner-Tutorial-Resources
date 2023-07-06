using UnityEngine;
using System;
using ConfigNamespace;
using System.ComponentModel;

class RubyAnimatorAttr {
	static public readonly string speed = "Speed";
	static public readonly string lookX = "Look X";
	static public readonly string lookY = "Look Y";
	static public readonly string hit = "Hit";
	static public readonly string launch = "Launch";
}

public class RubyController : MonoBehaviour {

	/// <summary>
	/// 游戏帧率设置
	/// </summary>
	/// 
	public int frame = 100;

	/// <summary>
	/// 最大生命值
	/// </summary>SetPropertyAttribute
	public int maxHealth = 10;

	/// <summary>
	/// 当前生命值
	/// </summary>
	[SerializeField]
	private int _currentHealth = 1;
	/// <summary>
	/// 当前生命值(0-this.maxHealth)
	/// </summary>
	public int currentHealth {
		get => this._currentHealth;
		set => this._currentHealth = Math.Clamp(value, 0, this.maxHealth);
	}

	public float moveSpeed = 0.1f;

	/// <summary>
	/// 声音播放器
	/// </summary>
	public AudioSource audioSource;

	/// <summary>
	/// 伤害音效
	/// </summary>
	public AudioClip hitClip;

	/// <summary>
	/// 死亡音效
	/// </summary>
	public AudioClip deathClip;

	/// <summary>
	/// 奔跑音效
	/// </summary>
	// public AudioClip runClip;


	////////////////////////////// 以下是私有属性 //////////////////////////////

	/// <summary>
	/// 是否是无敌状态
	/// </summary>
	protected bool isInvincible = false;

	/// <summary>
	/// 静止不动时，的朝向
	/// </summary>
	Vector2 lookDirection = new Vector2(1, 0);
	/// <summary>
	/// 移动方向
	/// </summary>
	Vector2 moveDirection;

	private Rigidbody2D _rigidbody2d;
	private Vector2? _position;
	private float _vertical;
	private float _horizontal;
	private Animator _animator;



	void Awake() {
		// 因为在Inspector里设置的值不会走set方法初始化, 但是_currentHealth又能拿到正确的值，就很离谱
		this.currentHealth = this._currentHealth;
	}

	// Start is called before the first frame update
	void Start() {
		Application.targetFrameRate = this.frame;
		this._rigidbody2d = this.GetComponent<Rigidbody2D>();
		this._position = this.transform.position;
		this._animator = this.gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update() {
		this._vertical = Input.GetAxis("Vertical");
		this._horizontal = Input.GetAxis("Horizontal");

		if (Input.GetKeyDown(KeyCode.C) /* || Input.GetAxis("Fire1") != 0 */) {
			this.Launch();
		}

		// 需要从当前位置投射一条射线，判断是否碰到了物体
		if (Input.GetKeyDown(KeyCode.X) /* || Input.GetAxis("Fire2") != 0 */) {
			this.CheckNPC();
		}
	}


	void FixedUpdate() {
		var speed = 0f;
		Vector2 position = this._position ?? this.transform.position;
		float frameTime = (1.0f / this.frame);
		var lastFrameTime = Time.deltaTime;

		// 由于每次只能面向一个方向，所以设置一个xy其中一个方向时，需要将其他方向上的值清空，优先显示左右方向，所以左右方向放在后面。
		// 另外x > -1都为面向右边，只有x<= -1才会面向左边，这是一个bug。
		if (this._vertical != 0) {
			speed += Math.Abs(this.moveSpeed * (lastFrameTime / frameTime));
			position.y += _vertical * speed;
			this.lookDirection.Set(0, this._vertical > 0 ? 1 : -1);
		}

		if (this._horizontal != 0) {
			speed += Math.Abs(this.moveSpeed * (lastFrameTime / frameTime));
			position.x += _horizontal * speed;
			this.lookDirection.Set(this._horizontal > 0 ? 1 : -1, 0);
		}
		this._animator.SetFloat(RubyAnimatorAttr.lookX, this.lookDirection.x);
		this._animator.SetFloat(RubyAnimatorAttr.lookY, this.lookDirection.y);
		this._position = position;
		this._rigidbody2d.position = this._position ?? this._rigidbody2d.position;
		this._position = null;
		this._animator.SetFloat(RubyAnimatorAttr.speed, speed);
		this.PlayFootstep(speed > 0);
	}

	/// <summary>
	/// 检查是否碰到了NPC
	/// </summary>
	private void CheckNPC() {
		var hitDirection = this.lookDirection;
		var hitPosition = this._rigidbody2d.position + hitDirection * 0.5f;
		var hit = Physics2D.Raycast(hitPosition, hitDirection, 0.5f, LayerMask.GetMask("NPC"));
		if (hit.collider != null) {
			var npc = hit.collider.GetComponent<NPCController>();
			if (npc != null) {
				npc.ShowDialog(this);
			}
		}
	}

	/// /// <summary>
	/// 更改血量
	/// </summary>
	/// <param name="value">增量</param>
	/// <param name="timeInvincible">可选参数，扣除血量后的无敌时间。单位s</param>
	/// <returns>更改是否改变血成功</returns>
	public bool changeHealth(int value, float? timeInvincible = null) {
		if (value < 0) {
			// 扣血时，需要判断是不是无敌时间
			if (this.isInvincible) {
				return false;
			}
			this.isInvincible = true;
			this.Invoke("CancelInvincible", RubyConfig.CHANGE_HEALTH_TIME_INVINCIBLE);
			this._animator.SetTrigger(RubyAnimatorAttr.hit);
			AudioSource.PlayClipAtPoint(this.hitClip, this.transform.position);
		}

		var oldHealth = this.currentHealth;
		this.currentHealth += value;
		var result = this.currentHealth != oldHealth;
		if (this.currentHealth <= 0) {
			// 死亡
			Debug.Log($"{nameof(this.name)}: {this.name}死亡!");
			AudioSource.PlayClipAtPoint(this.deathClip, this.transform.position);
		}
		if (result) {
			Debug.Log($"{nameof(this.name)}: {this.name},{(value > 0 ? "加血" : "扣血")}成功!当前血量{this.currentHealth}");
		} else {
			// 扣血失败
			Debug.Log($"{nameof(this.name)}: {this.name},{(value > 0 ? "加血" : "扣血")}失败!");
		}
		return result;
	}

	/// <summary>
	/// 取消无敌状态
	/// </summary>
	private void CancelInvincible() {
		this.isInvincible = false;
	}

	/// <summary>
	/// 发射子弹
	/// </summary>
	private void Launch() {
		Projectile projectile = ProjectilePoolManager.Instance.GetItem();
		if (!projectile) return;
		this._animator.SetTrigger(RubyAnimatorAttr.launch);
		projectile?.Launch(this._rigidbody2d.position, this.lookDirection, 300);
	}


	/// <summary>
	/// 在主角身上播放音频
	/// </summary>
	/// <param name="clip">需要播放的音频</param>
	/// <param name="volume">音量,默认1</param>
	public void PlaySound(AudioClip clip, float volume = 1f) {
		this.audioSource.PlayOneShot(clip, volume);
	}

	/// <summary>
	/// 是否播放脚步声
	/// </summary>
	private void PlayFootstep(bool isPlay) {
		if (isPlay) {
			if (this.audioSource.isPlaying) return;
			this.audioSource.Play();
			this.audioSource.loop = true;
		} else {
			this.audioSource.Stop();
		}
	}
}
