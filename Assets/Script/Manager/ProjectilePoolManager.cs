/*
 * @Description: 子弹管理类单例
 * @Author: cuiwanzhi
 * @Date: 2023-03-31 03:29:39
 * @FilePath: /2D Beginner Tutorial Resources/Assets/Script/Manager/ProjectilePoolManager.cs
 */

using System;

public class ProjectilePoolManager : PoolManagerBase<ProjectilePoolManager, Projectile> {
	protected override int getMaxItemNum() => ConfigNamespace.RubyConfig.INIT_MAX_PROJECT_NUM;

	protected override string GetPrefabPath() => "Prefabs/Projectile";
}
