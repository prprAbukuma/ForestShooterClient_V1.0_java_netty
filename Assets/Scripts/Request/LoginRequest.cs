﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : BaseRequest {
    private LoginPanel _loginPanel;

    public override void Awake()
    {
        //【注意】保证先设置正确RequestCode和ActionCode，再添加到列表中
        //设置枚举类型
        requestType = RequestType.User;
        actionType = ActionType.Login;
        base.Awake();
    }
    private void Start()
    {
        //初始化
        _loginPanel = this.GetComponent<LoginPanel>();
      
    }
    /// <summary>
    /// 发送登录请求
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    public void SendRequest(string username, string password)
    {
        //包装数据【非封包】
        string data = username + "#" + password;//用#做分割,服务器按#分割即可
        //封装为Request对象
        Request loginRequest = new Request((int)requestType,(int)actionType,data);
        //发送请求.【但只能发送二进制流】
        //先转换为二进制流
        byte[] loginRequestBytes = ConverterTool.SerialRequestObj(loginRequest);
        GameFacade.Instance.ClientManager.SendMsgToServer(loginRequestBytes);
    }

    /// <summary>
    /// 处理登录请求的来自服务器端的响应
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        //【测试成功-用netty框架后，不再因为新开线程不能调用Unity主线程中的内容而写标志位使程序复杂】
        //RoomListPanel roomListPanel = GameFacade.Instance.UiManager.PushPanel(PanelType.RoomList) as RoomListPanel;

        //解析到来自服务器的响应【按#拆分】
        string[] info = data.Split('#');
        ReturnType returnType = (ReturnType)(int.Parse(info[0]));//先获得是登录成功还是失败
        if (returnType == ReturnType.Failed)
        {
            //登录失败-剩余交给LoginPanel的HandleLoginResponse处理
            _loginPanel.HandleLoginResponse(false);
        }
        else if (returnType == ReturnType.Successful)
        {
            //登录成功-剩余交给LoginPanel的HandleLoginResponse处理
            _loginPanel.HandleLoginResponse(true);
            //解析战绩数据-给PlayerManager的User对象和Score对象
            string[] playerInfo = data.Split('#');//直接从下标1开始，0是ReturnCode，已经用过了
            User user = new User();
            Score score = new Score();
            user.Id = int.Parse(playerInfo[1]);//UID
            user.Username = playerInfo[2];//用户名
            score.TotalCount = int.Parse(playerInfo[3]);//用户总场数
            score.WinCount = int.Parse(playerInfo[4]);//用户胜利数
            //赋值给PlayerManager
            GameFacade.Instance.PlayerManager.currentLoginedUser = user;
            GameFacade.Instance.PlayerManager.currentUserScore = score;
        }


    }
}