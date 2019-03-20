
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitRoomRequest : BaseRequest {
    private RoomPanel _roomPanel;
    public override void Awake()
    {
        //设置RequestCode和ActionCode
        //requestCode = RequestCode.Room;
        //actionCode = ActionCode.QuitRoom;
        requestType = RequestType.Room;
        actionType = ActionType.QuitRoom;
        base.Awake();
    }

     void Start()
    {
        _roomPanel = this.GetComponent<RoomPanel>();
    }
    /// <summary>
    /// 发送退出房间请求
    /// </summary>
    public override void SendRequest()
    {
        string data = "null";
        //构造退出房间请求对象
        Request quitRoomRequest = new Request((int)RequestType.Room, (int)ActionType.QuitRoom, data);
        //编码为二进制流
        byte[] dataBytes = ConverterTool.SerialRequestObj(quitRoomRequest);
        GameFacade.Instance.ClientManager.SendMsgToServer(dataBytes);
        //GameFacade.Instance.ClientManager.SendRequestToServer(requestCode, actionCode, data);
    }
    /// <summary>
    /// 处理退出房间请求的响应
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        ReturnType returnType = (ReturnType)(int.Parse(data));
        if (returnType == ReturnType.Successful)
        {
            //交给RoomPanel
            _roomPanel.HandleQuitRoomResponse();
        }
    }
}
