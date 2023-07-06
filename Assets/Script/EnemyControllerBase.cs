using UnityEngine;

/// <summary>
/// 敌人对象基类
/// </summary>
public interface EnemyControllerBase{

	/// <summary>
	/// 受到攻击
	/// </summary>
	/// <param name="damageValue">攻击伤害</param>
	public void OnHit(float damageValue);
}