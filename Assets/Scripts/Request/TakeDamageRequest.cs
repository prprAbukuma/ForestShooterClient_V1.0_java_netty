
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageRequest:BaseRequest  {

    public override void Awake()
    {
        //设置类型
        //requestCode = RequestCode.Game;
        //actionCode = ActionCode.TakeDamage;
        requestType = RequestType.Game;
        actionType = ActionType.TakeDamage;
        base.Awake();
    }
    /// <summary>
    /// 发送执行伤害请求
    /// </summary>
    /// <param name="damageVal">伤害值</param>
    public void SendRequest(int damageVal)
    {
        string data = damageVal.ToString();
        Request takeDamageRequest = new Request((int)requestType,(int)actionType,data);
        byte[] dataBytes = ConverterTool.SerialRequestObj(takeDamageRequest);
        GameFacade.Instance.ClientManager.SendMsgToServer(dataBytes);
        //GameFacade.Instance.ClientManager.SendRequestToServer(requestCode, actionCode, data);
    }


}
