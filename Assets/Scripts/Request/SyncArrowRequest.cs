
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncArrowRequest : BaseRequest {

    private Vector3 _remoteRoleArrowInitPosition;//远端角色箭的生成位置
    private Vector3 _remoteRoleArrowInitEulerAngles;//远端角色箭生成的旋转
    private RoleColor _remoteRoleColor;//远端角色类型
    private bool isNeedSyncArrow = false;//标志位-需要同步箭吗
    public override void Awake()
    {
        //设置类型
        //requestCode = RequestCode.Game;
        //actionCode = ActionCode.SyncArrow;
        requestType = RequestType.Game;
        actionType = ActionType.SyncArrow;
        base.Awake();
    }

    private void Update()
    {
        if (isNeedSyncArrow)
        {
            //同步箭【实例化箭】
            SyncRemoteRoleArrow(_remoteRoleColor, _remoteRoleArrowInitPosition, _remoteRoleArrowInitEulerAngles);   

            isNeedSyncArrow = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void SyncRemoteRoleArrow(RoleColor roleColor,Vector3 position,Vector3 eulerAngles)
    {
        //获得远端角色的箭的预制体
        GameObject remoteRoleArrowPrefab = GameFacade.Instance.PlayerManager.roleDataDict.TryGet(roleColor).ArrowPrefab;
        //在这个客户端进行实例化
        GameObject remoteArr= Instantiate(remoteRoleArrowPrefab, position, Quaternion.Euler(eulerAngles));
        //设置箭的类型-远端箭
        remoteArr.GetComponent<Arrow>().arrowInstantType = ArrowInstantiateType.Remote;
    }
   

    /// <summary>
    /// 发送同步箭请求
    /// </summary>
    /// <param name="roleType">发射这个箭的角色类型</param>
    /// <param name="positon">箭的位置</param>
    /// <param name="rotation">箭的方向</param>
    public void SendRequest(RoleColor roleColor,Vector3 positon,Vector3 rotation)
    {
        string data = (int)roleColor + "*" + positon.x + "#" + positon.y + "#" + positon.z + "*" + rotation.x + "#" + rotation.y + "#" + rotation.z;
        Request syncArrowRequest = new Request((int)requestType, (int)actionType, data);
        byte[] dataBytes = ConverterTool.SerialRequestObj(syncArrowRequest);
        GameFacade.Instance.ClientManager.SendMsgToServer(dataBytes);
        //GameFacade.Instance.ClientManager.SendRequestToServer(requestCode, actionCode, data);
    }
    /// <summary>
    /// 处理同步箭请求的响应
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        //解析数据
        string[] dataArr = data.Split('*');
        RoleColor arrowType = (RoleColor)int.Parse(dataArr[0]);//获得箭是那种角色发的
        string[] positionArr = dataArr[1].Split('#');
        string[] eulerAnglesArr = dataArr[2].Split('#');
        Vector3 position = new Vector3(float.Parse(positionArr[0]), float.Parse(positionArr[1]), float.Parse(positionArr[2]));
        Vector3 eulerAngles = new Vector3(float.Parse(eulerAnglesArr[0]), float.Parse(eulerAnglesArr[1]), float.Parse(eulerAnglesArr[2]));
        //按照这些数据进行创建-改变标志位-后交给Update来处理
        _remoteRoleArrowInitPosition = position;
        _remoteRoleArrowInitEulerAngles = eulerAngles;
        _remoteRoleColor = arrowType;
        isNeedSyncArrow = true;//改变标志位-执行
    }
}
