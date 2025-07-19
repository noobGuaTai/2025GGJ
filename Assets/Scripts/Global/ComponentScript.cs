/// <summary>
/// 挂载在怪物上触发对应效果，同时在怪物的FSM中注册方法
/// </summary>
class ComponentScript
{
    AggressiveEnemy aggressiveEnemy;// 怪物本体具有伤害
    KilledByCoinEnemy killedByCoinEnemy;// 可被高速硬币砸死
    KnockedBackEnemy knockedBackEnemy;// 可被大泡泡击退
    SwallowedEnemy swallowedEnemy;// 可被泡泡吞
    KilledByEnemyDrop killedByEnemyDrop;// 可被重物砸死
}