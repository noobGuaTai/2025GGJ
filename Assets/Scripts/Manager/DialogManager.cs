using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string playerLine;
    public string enemyResponse;
}

public class DialogManager : MonoSingleton<DialogManager>
{
    public Dictionary<string, string> mushroomManDialogues;
    public Dictionary<string, string> chickenManDialogues;
    public Dictionary<string, string> gyroscopeManDialogues;
    private Dictionary<string, string> traderDialogues;

    void Start()
    {
        InitializeDialogues();
    }

    private void InitializeDialogues()
    {
        mushroomManDialogues = new Dictionary<string, string>
        {
            { "你好", "我认识你吗？好像有点熟悉" },
            { "你的头怎么尖尖的？", "那我问你，那我问你，你是男的还是女的？" },
            { "你天天这样来回走有意义吗？", "不知道，我从来没有想过这个问题" },
            { "你长得好像一个蘑菇", "哈哈哈，我本来就是的" },
            { "我们打一架，谁会赢？", "被小蘑菇击败的概率可能并不高，但绝对不会是零" },
            { "你手上拿的是什么？", "这是钥匙，可以打开身后的门，但是我不能给你" },
            { "你在听什么音乐？", "哈哈哈，游戏音乐《拍硬档》，你也听一听吗？" },
            { "你敢跳进刺谷吗？", "不要，我会受伤的" },
            { "你好吃吗？", "哈哈哈，鸡人说是的" },
            { "你经常被揍吗？", "哈哈哈，毕竟是教学关小怪，没办法的" },
            { "你很可爱", "谢谢，策划原本想让我长得讨厌一点的，但是感谢画师幸运的温柔" },
            { "你长得很像《超级马里洛》中的角色", "你也是《探险岛》和《爆炸人》角色的缝合，不过别担心后面会出现原创角色" },
            { "你对泡泡怎么看？", "我不喜欢" },
            { "你比我想象的更加友善", "非常感谢" },
            { "你每天的微信步数是多少？", "鸡人总是比我多一些" }
        };

        chickenManDialogues = new Dictionary<string, string>
        {
            { "你好", "少套近乎！" },
            { "鸡你太…", "我知道你想说什么，但是闭嘴" },
            { "你的速度有多快？", "是的，我可以达到这个游戏中的最快速度" },
            { "你好吃吗？", "你是不是觉得自己很幽默？" },
            { "蘑菇人好吃吗？", "很好吃，我和他合作会更好吃" },
            { "你的学历是什么？", "二本，和策划一样，难怪他能设计出这种破游戏" },
            { "你喜欢聊天吗？", "我的耐心很快就会因此耗尽" },
            { "你对泡泡怎么看？", "看到我的喙了吗？我会一个不少的将它们刺破！" },
            { "你也有钥匙", "是的，但它打不开这一关的门，这是策划的恶趣味！" },
            { "你觉得这个关卡怎么样？", "这一关的设计中，策划这家伙突然灵感爆发了" },
            { "你的速度还是太慢了。", "但这你过来我们试试" },
            { "你什么时候吃饭？", "早晨，中午，晚上…还有，别提蠢问题" },
            { "你是一只鸡", "我不是难道你是？" },
            { "我能拿到你的钥匙吗？", "你疑似有点挑衅了" },
            { "你放下钥匙休息休息吧", "别以为我不知道你在想什么" }
        };

        gyroscopeManDialogues = new Dictionary<string, string>
        {
            { "你好", "（微笑）" },
            { "你为什么一直旋转？", "因为我是一个陀螺，旋转使我安心" },
            { "陀螺也可以做人吗？", "《鬼魂小倩3》中的黑山老妖甚至是一间房屋，他做得我做不得？" },
            { "你认识蘑菇人和鸡人吗？", "我刚到此地，未曾听说，有机会请向我介绍" },
            { "如何评价这个游戏", "此游戏并不完美，但我因此而生，也没什么可以奢求的了" },
            { "请向我介绍这里", "我也是初到此地，只知道地下可能存在一个洞穴，其他无甚可言，还望见谅" },
            { "你为什么会在这里？", "我来寻找一处隐藏的奇观，它就在这附近，这是我与旧友的承诺" },
            { "这个游戏的Logo看起来很有趣", "是的，这是在模仿玛格利特的《人类之子》" },
            { "你是怎么知道自己在游戏中的？", "我们每一个角色在被创造之初便被告知了" },
            { "你放下钥匙休息休息吧", "别以为我不知道你在想什么" }
        };

        traderDialogues = new Dictionary<string, string>
        {
            { "你好", "（微笑）" },
            { "你认识蘑菇人和鸡人吗？", "我刚到此地，未曾听说，有机会请向我介绍" },
            { "如何评价这个游戏", "此游戏并不完美，但我因此而生，也没什么可以奢求的了" },
            { "这个游戏的Logo看起来很有趣", "是的，这是在模仿玛格利特的《人类之子》" },
            { "你是怎么知道自己在游戏中的？", "我们每一个角色在被创造之初便被告知了" },
        };
    }

    /// <summary>
    /// 随机获取指定敌人的一个对话组（玩家输入和敌人回应）
    /// </summary>
    /// <param name="enemyName">敌人的名称，例如 "蘑菇人"</param>
    /// <returns>返回一个随机的 Dialogue 对象，如果敌人不存在或没有对话，则返回 null</returns>
    public Dialogue GetRandomDialogue(string enemyName)
    {
        Dictionary<string, string> dialogues;

        switch (enemyName)
        {
            case "enemy1":
                dialogues = mushroomManDialogues;
                break;
            case "enemy2":
                dialogues = chickenManDialogues;
                break;
            case "enemy3":
                dialogues = gyroscopeManDialogues;
                break;
            case "enemy4":
                dialogues = traderDialogues;
                break;
            case "enemy5":
                dialogues = traderDialogues;
                break;
            default:
                return null;
        }

        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogWarning($"敌人 \"{enemyName}\" 没有对话。");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, dialogues.Count);
        var keys = new List<string>(dialogues.Keys);
        string selectedPlayerLine = keys[randomIndex];
        string selectedEnemyResponse = dialogues[selectedPlayerLine];

        return new Dialogue
        {
            playerLine = selectedPlayerLine,
            enemyResponse = selectedEnemyResponse
        };
    }

}
