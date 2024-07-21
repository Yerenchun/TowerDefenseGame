using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档文件
/// </summary>
public class PlayerData
{
    // 当前拥有的钱
    public int haveMoney = 0;
    // 当前解锁的角色的，id集
    public List<int> haveHero = new List<int>();
}
