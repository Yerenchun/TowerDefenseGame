using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色单条数据
/// </summary>
public class RoleInfo
{
    public int id;// 唯一标识
    public string res;// 模型来源
    public int atk;// 攻击力
    public int lockMoney;// 解锁所需金币
    public int type;// 武器类型，如果是刀就是1,2就是枪
    public string tips;// 角色的描述
    public string hitEff;// 受击效果
}
