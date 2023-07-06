/*
 * @Description: 这是一个游戏的配置文件
 * @Author: cuiwanzhi
 * @Date: 2023-01-05 00:21:58
 * @FilePath: \2D Beginner Tutorial Resources\Assets\Script\Config.cs
 */

namespace ConfigNamespace {

    /// <summary>
	/// 玩家角色配置类
	/// </summary>
    public class RubyConfig {
        /// <summary>
        /// 玩家角色扣血后的默认无敌时间
        /// </summary>
        static public readonly float CHANGE_HEALTH_TIME_INVINCIBLE = 2.0f;
		/// <summary>
		/// 初始时，最多同时存在的子弹数量
		/// </summary>
        static public readonly int INIT_MAX_PROJECT_NUM = 2;
    }
}
