using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 加血道具
/// </summary>
public class HealthCollectible : MonoBehaviour {
	/// <summary>
	/// 增加血量的值
	/// </summary>
	public int addHealthValue = 1;

	/// <summary>
	/// 音频剪辑
	/// </summary>
	public AudioClip audioClip;

	/// <summary>
	/// 音量大小
	/// </summary>
	public float volumeScale = 1f;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// 碰撞检测，如果碰撞对象上挂有RubyController，就会调用他的changeHealth方法。同时如果加血成功，就会销毁自身
	/// </summary>
	/// <param name="col"></param>
	void OnTriggerEnter2D(Collider2D col) {
		if (col.name != "Ruby") return;
		var rubyController = col.GetComponent<RubyController>();
		if ((bool)(rubyController?.changeHealth(this.addHealthValue))) {
			if (this.audioClip != null) {
				AudioSource.PlayClipAtPoint(this.audioClip, this.transform.position, this.volumeScale);
			}
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Sent each frame where another object is within a trigger collider
	/// attached to this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerStay2D(Collider2D other) {
		this.OnTriggerEnter2D(other);
	}
}
