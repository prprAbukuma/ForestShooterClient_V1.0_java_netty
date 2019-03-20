
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRequest : BaseRequest {
    private GamePanel _gamePanel;
    public override void Awake()
    {
        //requestCode = RequestCode.Game;
        //actionCode = ActionCode.GameOver;
        requestType = RequestType.Game;
        actionType = ActionType.GameOver;
        base.Awake();
    }
     void Start()
    {
        _gamePanel = this.GetComponent<GamePanel>();    
    }
    /// <summary>
    /// 处理游戏结束响应
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        //解析结果
        ReturnType returnType = (ReturnType)int.Parse(data);
        //根据结果
        if (returnType == ReturnType.Successful)
        {
            //游戏胜利-交给GamePanel
            _gamePanel.HandleGameOverResponse(true);

        }
        else if(returnType==ReturnType.Failed) {
            //游戏失败-交给GamePanel
            _gamePanel.HandleGameOverResponse(false);
        }
    }
}
