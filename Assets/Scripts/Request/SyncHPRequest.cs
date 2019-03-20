
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncHPRequest : BaseRequest {
    private GamePanel _gamePanel;
    public override void Awake()
    {
        // requestCode = RequestCode.Game;
        //actionCode = ActionCode.SyncHP;
        requestType = RequestType.Game;
        actionType = ActionType.SyncHP;
        base.Awake();
    }
    private void Start()
    {
        _gamePanel = this.GetComponent<GamePanel>();
    }

    public override void OnResponse(string data)
    {
        //解析血量信息
        string[] HPdataStr = data.Split('#');
        int p1currentHP = int.Parse(HPdataStr[0]);
        int p2currentHP = int.Parse(HPdataStr[1]);
        //交给gamepanel处理
        _gamePanel.HandleSyncHPSlider(p1currentHP,p2currentHP);
    }

}
