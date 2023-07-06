using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动方向枚举
/// </summary>
public enum BotControllerMoveDirection {
	horizontal,
	vertical,
	/// <summary>
	/// 随机方向
	/// </summary>
	any,
	/// <summary>
	/// 指定角度
	/// </summary>
	angle,
	/// <summary>
	/// 停止不动
	/// </summary>
	stay,
}

enum BotAnimatorKey {
	MoveX,
	MoveY
}


public class BotController : MonoBehaviour, EnemyControllerBase {

	/// <summary>
	/// 伤害值
	/// </summary>
	public int damageNum = 1;

	/// <summary>
	/// 移动方向
	/// </summary>
	// [SerializeField]
	public BotControllerMoveDirection moveDirection = BotControllerMoveDirection.vertical;

	/// <summary>
	/// 最大移动距离
	/// </summary>
	public float moveLineMaxValue = 1;

	/// <summary>
	/// 移动速度
	/// </summary>
	public float moveSpeed = 0.1f;

	/// <summary>
	/// 自定义运动的角度
	/// </summary>
	public float angle = 50f;

	/// <summary>
	/// 音效播放器
	/// </summary>
	public AudioSource audioSource;

	/// <summary>
	/// 被击中时的音效
	/// </summary>
	public AudioClip hitClip;

	/// <summary>
	/// 修复时的音效
	/// </summary>
	public AudioClip fixClip;

	/// <summary>
	/// 舞蹈时的音效
	/// </summary>
	public AudioClip danceClip;


	////////////////////////////// 以下是私有属性 //////////////////////////////

	private Rigidbody2D rigidbody2d;
	/// <summary>
	/// 动画管理器
	/// </summary>
	private Animator animator;
	/// <summary>
	/// 目前移动的距离
	/// </summary>
	private float movelineValue;
	// 声明委托
	private Func<Vector2> moveFunc;
	private Vector2 beginPosition;
	private Dictionary<BotControllerMoveDirection, Func<Vector2>> funcMap;
	private Vector2 endPosition;
	/// <summary>
	/// 当前行程阶段，1为前进，-1为返程
	/// </summary>
	private int direction = 1;

	/// <summary>
	/// 当前是否为移动状态
	/// </summary>
	private bool isMove = true;

	/// <summary>
	/// 烟雾特效
	/// </summary>
	public ParticleSystem hitParticlePrefab;



	/// <summary>
	/// 当前位置。建议在FixedUpdate里改变，以保证物理引擎合理和移动速度均衡。
	/// </summary>
	private Vector2 currPostion {
		get => this.rigidbody2d.position;
		set => this.rigidbody2d.position = value;
	}

	// Start is called before the first frame update
	void Start() {
		this.rigidbody2d = this.GetComponent<Rigidbody2D>();
		this.animator = this.GetComponent<Animator>();
		this.movelineValue = this.moveLineMaxValue;
		this.funcMap = new Dictionary<BotControllerMoveDirection, Func<Vector2>>(){
			{BotControllerMoveDirection.horizontal, this.moveHorizontal},
			{BotControllerMoveDirection.vertical, this.moveVertical},
			// {BotControllerMoveDirection.angle, this.moveAngle},
		};
		this.moveFunc = this.funcMap[this.moveDirection];
		this.beginPosition = this.rigidbody2d.position;
		this.endPosition = beginPosition;
	}

	// Update is called once per frame
	void Update() {
		// this.transform.position = this.position ?? this.transform.position;
	}

	void FixedUpdate() {
		if (isMove) {
			if (this.moveFunc is not null) {
				var position = this.moveFunc();
				this.currPostion = position;
				this.animator.SetBool("isRunning", true);
			} else {
				this.animator.SetBool("isRunning", false);
			}
			var MoveX = this.endPosition.x - this.beginPosition.x;
			var MoveY = this.endPosition.y - this.beginPosition.y;
		}
	}

	/// <summary>
	/// 水平移动
	/// </summary>
	/// <returns></returns>
	private Vector2 moveHorizontal() {
		if (this.endPosition == this.beginPosition) {
			this.endPosition = beginPosition;
			this.endPosition.x += this.moveLineMaxValue;
			this.endPosition.y += 0;
		}
		//经过的行程
		var distanceLen = this.moveLineMaxValue * (Time.fixedDeltaTime / (this.moveLineMaxValue / this.moveSpeed));
		var limitMin = Math.Min(this.beginPosition.x, this.endPosition.x);
		var limitMAx = Math.Max(this.beginPosition.x, this.endPosition.x);
		var currPostion = this.currPostion;
		this.currPostion = new Vector2(1, 1);
		if (currPostion.x < limitMin) {
			this.direction = 1;
		}
		if (currPostion.x > limitMAx) {
			this.direction = -1;
		}
		currPostion.x += distanceLen * this.direction;
		this.animator.SetFloat(BotAnimatorKey.MoveX.ToString(), this.direction);
		this.animator.SetFloat(BotAnimatorKey.MoveY.ToString(), 0);
		return currPostion;
	}

	/// <summary>
	/// 垂直移动
	/// </summary>
	/// <returns></returns>
	private Vector2 moveVertical() {
		// 初始化起点和终点
		if (this.endPosition == this.beginPosition) {
			this.endPosition = beginPosition;
			this.endPosition.x += 0;
			this.endPosition.y += this.moveLineMaxValue;
		}
		//经过的行程
		var distanceLen = this.moveLineMaxValue * (Time.fixedDeltaTime / (this.moveLineMaxValue / this.moveSpeed));
		var limitMin = Math.Min(this.beginPosition.y, this.endPosition.y);
		var limitMAx = Math.Max(this.beginPosition.y, this.endPosition.y);
		var currPostion = this.currPostion;
		if (currPostion.y < limitMin) {
			this.direction = 1;
		}
		if (currPostion.y > limitMAx) {
			this.direction = -1;
		}
		currPostion.y += distanceLen * this.direction;
		this.animator.SetFloat(BotAnimatorKey.MoveX.ToString(), 0);
		this.animator.SetFloat(BotAnimatorKey.MoveY.ToString(), this.direction);
		return currPostion;
	}

	/// <summary>
	/// 特定角度移动
	/// </summary>
	/// <returns></returns>
	private Vector2 moveAngle() {
		// TODO 这个函数暂时不能用

		// 初始化起点和终点
		if (this.endPosition == this.beginPosition) {
			this.endPosition = beginPosition;
			var piAngle = this.angle * 3.1415926 / 180;
			endPosition.x += (float)(Math.Cos(piAngle) * this.moveLineMaxValue);
			endPosition.y += (float)(Math.Sin(piAngle) * this.moveLineMaxValue);
		}
		return this.currPostion;
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only) .
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other) {
		RubyController rubyController = other.gameObject.GetComponent<RubyController>();
		if (rubyController == null) return;
		// 不用管是否扣血成功
		rubyController.changeHealth(-this.damageNum);
	}

	public void OnHit(float damageValue) {
		this.isMove = false;
		this.rigidbody2d.simulated = false;
		this.animator.SetBool("isRunning", false);
		this.audioSource.PlayOneShot(this.hitClip);
		this.Invoke("OnFix", this.hitClip.length);
	}

	/// <summary>
	/// 开始修复，并播放修复音效
	/// </summary>
	private void OnFix() {
		this.audioSource.PlayOneShot(this.fixClip);
		this.Invoke("OnFixEnd", this.fixClip.length);
	}

	/// <summary>
	/// 修复完成,开始跳舞
	/// </summary>
	private void OnFixEnd() {
		this.hitParticlePrefab.Stop();
		this.animator.SetBool("isFix", true);
		this.audioSource.PlayOneShot(this.danceClip);
	}

}