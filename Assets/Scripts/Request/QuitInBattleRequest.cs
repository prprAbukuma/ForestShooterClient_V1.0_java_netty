
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitInBattleRequest : BaseRequest {
    private GamePanel _gamePanel;

    public override void Awake()
    {
        //requestCode = RequestCode.Game;
        //actionCode = ActionCode.QuitInBattle;
        requestType = RequestType.Game;
        actionType = ActionType.QuitInBattle;
        base.Awake();
    }

    private void Start()
    {
        _gamePanel = this.GetComponent<GamePanel>();
    }
    /// <summary>
    /// 发送中途停止游戏
    /// </summary>
    public override void SendRequest()
    {
        string data = "null";
        Request quitInBattleRequest = new Request((int)requestType,(int)actionType,data);
        byte[] dataBytes = ConverterTool.SerialRequestObj(quitInBattleRequest);
        GameFacade.Instance.ClientManager.SendMsgToServer(dataBytes);
        
    }
    /// <summary>
    /// 处理中途停止游戏响应
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        //不用解析数据，直接交给GamePanel进行处理
        _gamePanel.HandleQuitInBatttleResponse();
    }
}
