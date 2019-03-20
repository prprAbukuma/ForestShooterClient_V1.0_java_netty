﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRequest : BaseRequest {

    private RegisterPanel _registerPanel;

    public override void Awake()
    {
        //设置枚举类型
        requestType = RequestType.User;
        actionType = ActionType.Resgiter;
        base.Awake();
    }

    private void Start()
    {
        //初始化组件
        _registerPanel = this.GetComponent<RegisterPanel>();
    }

    /// <summary>
    /// 发送注册请求
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public void SendRequest(string username,string password)
    {
        //组拼数据
        string data = username + "#" + password;
        //发送请求至服务
        Request registerRequest = new Request((int)RequestType.User,(int)ActionType.Resgiter,data);
        byte[] dataBytes = ConverterTool.SerialRequestObj(registerRequest);
        GameFacade.Instance.ClientManager.SendMsgToServer(dataBytes);
        //GameFacade.Instance.ClientManager.SendRequestToServer(requestCode, actionCode, data);
    }
    /// <summary>
    /// 处理注册请求来自服务器端的响应
    /// </summary>
    /// <param name="data">响应数据</param>
    public override void OnResponse(string data)
    {
        //解析响应数据至ReturnCode
        ReturnType returnType = (ReturnType)(int.Parse(data));
        //:具体操作交给RegisterPanel处理
        _registerPanel.HandleRegisterResponse(returnType);
    }
}