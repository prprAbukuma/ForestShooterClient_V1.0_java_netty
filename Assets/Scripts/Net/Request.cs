using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

/// <summary>
/// 消息对象 -请求
/// </summary>

[ProtoContract]
public class Request  {
    [ProtoMember(1)]
    public int requestType;
    [ProtoMember(2)]
    public int actionType;
    [ProtoMember(3)]
    public string data;

    public Request() { }
    public Request(int requestType, int actionType, string data)
    {
        this.requestType = requestType;
        this.actionType = actionType;
        this.data = data;
    }
}
