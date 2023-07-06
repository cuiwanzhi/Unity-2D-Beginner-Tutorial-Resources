using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour {
	/// <summary>
	/// 每次碰到减少的血量
	/// </summary>
	public int damageNum = 1;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		
	}

	private void OnTriggerEnter(Collider other) {
		
	}

	void OnTriggerStay2D(Collider2D col) {
		var rubyController = col.GetComponent<RubyController>();
		var result = rubyController?.changeHealth(-this.damageNum) ?? false;
	}

}
