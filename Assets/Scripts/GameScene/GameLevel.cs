using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    private static GameLevel instance = new GameLevel();
    public static GameLevel Instance => instance;

    private GameLevel() { }

    // 1.切换到游戏场景时，动态加载玩家

    // 2.需要通过游戏管理器 来判断 游戏是否胜利
}
