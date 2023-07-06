using UnityEngine;

public abstract class PoolItemBase : MonoBehaviour {
	/// <summary>
	/// 重置对象状态以便复用
	/// </summary>
	public abstract void ResetState();

	/// <summary>
	/// 当对象被回收时候调用的函数
	/// </summary>
	public abstract void onRetrieve();
}