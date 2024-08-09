using System.Text.Json;

namespace GammaRaySignaling;

public class SignalMessage
{
    public const string SigNameHello = "hello";
    public const string SigNameOnHello = "on_hello";
    public const string SigNameCreateRoom = "create_room";
    public const string SigNameOnCreatedRoom = "on_created_room";
    public const string SigNameJoinRoom = "join_room";
    public const string SigNameOnJoinedRoom = "on_joined_room";
    public const string SigNameOnRemoteJoinedRoom = "on_remote_joined_room";
    public const string SigNameLeaveRoom = "leave_room";
    public const string SigNameOnLeftRoom = "on_left_room";
    public const string SigNameOnRemoteLeftRoom = "on_remote_left_room";
    public const string SigNameInviteClient = "invite_client";
    public const string SigNameOnInvitedToRoom = "on_invited_to_room";
    public const string SigNameOnRemoteInvitedToRoom = "on_remote_invited_to_room";
    public const string SigNameHeartBeat = "heart_beat";
    public const string SigNameOnHeartBeat = "on_heart_beat";
    public const string SigNameOfferSdp = "offer_sdp";
    public const string SigNameAnswerSdp = "answer_sdp";
    public const string SigNameIce = "ice";
    public const string SigNameError = "sig_error";
    public const string SigNameForceIFrame = "force_iframe";
    public const string SigNameCommand = "sig_command";
    public const string SigNameOnCommandResponse = "sig_on_command_response";
    public const string SigNameReqControl = "sig_req_control";
    public const string SigNameUnderControl = "sig_under_control";
    public const string SigNameOnDataChannelReady = "on_data_channel_ready";
    public const string SigNameOnRejectControl = "on_reject_control";

    public const string CmdQueryServerStatus = "query_status";

    public const string KeySigName = "sig_name";
    public const string KeyClientId = "client_id";
    public const string KeyRoomId = "room_id";
    public const string KeyRemoteClientId = "remote_client_id";
    public const string KeyControlClientId = "controller_client_id";
    public const string KeySelfClientId = "self_client_id";
    public const string KeyIndex = "index";
    public const string KeyPlatform = "platform";
    public const string KeySdp = "sdp";
    public const string KeyIce = "ice";
    public const string KeyMid = "mid";
    public const string KeySdpMLineIndex = "sdp_m_line_index";
    public const string KeyId = "id";
    public const string KeyName = "name";
    public const string KeyClients = "clients";
    public const string KeyRoom = "room";
    public const string KeyRooms = "rooms";
    public const string KeyAllowReSend = "allow_resend";
    
    public class SigErrorMessage
    {
        public string SigName = "";
        public int SigCode;
        public string SigInfo = "";
    }
    
    // SigHelloMessage hello消息
    // client -> server
    public class SigHelloMessage {
        public string SigName = "";
        public string ClientId = "";
        public string Platform = "";
        public bool AllowReSend = false;
    }
    
    // SigOnHelloMessage hello回复
    // server -> client
    public class SigOnHelloMessage
    {
        public string SigName = "";
        public string ClientId = "";
    }
    
    // SigCreateRoomMessage 请求创建一个房间，如果已经存在，则直接返回
    // client -> server
    public class SigCreateRoomMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RemoteClientId = "";
    }
    
    // SigOnCreatedRoomMessage 创建完成回调给发起创建者
    // server -> client
    public class SigOnCreatedRoomMessage {
        public string SigName = "";
        public string ClientId = "";
        public string RemoteClientId = "";
        public string RoomId = "";
        public string Platform = "";
        public List<Client> Clients = [];
    }
    
    // SigJoinRoomMessage 加入一个房间
    // client -> server
    public class SigJoinRoomMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RemoteClientId = "";
        public string RoomId = "";
    }
    
    // SigOnJoinedRoomMessage 加入一个房间后，回调给请求加入的人
    // server -> client
    public class SigOnJoinedRoomMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RemoteClientId = "";
        public string RoomId = "";
        public List<Client> Clients = [];
    }
    
    // SigOnRemoteJoinedRoomMessage 其他成员加入
    // server -> client
    public class SigOnRemoteJoinedRoomMessage
    {
        public string SigName = "";
        public string RemoteClientId = "";
        public string RoomId = "";
        public List<Client> Clients = [];
    }
    
    // SigLeaveRoomMessage 请求离开房间
    // client -> server
    public class SigLeaveRoomMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RoomId = "";
    }
    
    // SigOnLeftRoomMessage 自己离开房间
    // server -> client
    public class SigOnLeftRoomMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RoomId = "";
        public List<Client> Clients = [];
    }

    // SigOnRemoteLeftRoomMessage 其他成员离开
    // server -> client
    public class SigOnRemoteLeftRoomMessage
    {
        public string SigName = "";
        public string RemoteClientId = "";
        public string RoomId = "";
        public List<Client> Clients = [];
    }

    // SigInviteClientMessage 邀请其他人加入房间
    // client -> server
    public class SigInviteClientMessage 
    {
        public string SigName = "";
        public string ClientId = ""; 
        public string RemoteClientId = ""; 
        public string RoomId = "";
    }
    
    // SigOnInvitedToRoomMessage 被邀请的人收到这个回调
    // server -> peer client
    public class SigOnInvitedToRoomMessage
    {
        public string SigName = "";
        public string InvitorClientId = "";
        public string SelfClientId = "";
        public string RoomId = "";
        public List<Client> Clients = [];
    }
    
    // SigOnRemoteInvitedToRoomMessage 发起邀请的人收到这个回调
    // server -> request client
    public class SigOnRemoteInvitedToRoomMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RemoteClientId = "";
        public string RoomId = "";
        public List<Client> Clients = [];
    }
    
    // SigHeartBeatMessage 心跳
    // client -> server
    public class SigHeartBeatMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public long Index = 0;
        public string Platform = "";
    }
    
    // SigOnHeartBeatMessage 心跳回复
    // server -> client
    public class SigOnHeartBeatMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public long Index = 0;
        public string Platform = "";
    }

    // SigOfferSdpMessage 客户端发过来的Sdp
    // client -> server -> remote client
    public class SigOfferSdpMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RoomId = "";
        public string Sdp = "";
    }
    
    // SigAnswerSdpMessage 服务端响应的Sdp
    // remote client -> server -> client
    public class SigAnswerSdpMessage
    {
        public string SigName = "";
        public string  ClientId = "";
        public string RoomId = "";
        public string Sdp = "";
    }
    
    // SigIceMessage 两端交互的ICE
    // client <---> remote client
    public class SigIceMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RoomId = "";
        public string Ice = "";
        public string Mid = "";
        public long SdpMLineIndex = 0;
    }

    // SigForceIFrameMessage 产生关键帧
    // client <---> remote client
    public class SigForceIFrameMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RoomId = "";
    }
    
    // SigCommandMessage 控制或命令
    public class SigCommandMessage
    {
        public string SigName = "";
        public string Command = "";
        public Dictionary<string, string>? Extra = null;
    }
    
    // SigOnCommandResponseMessage 执行结果
    public class SigOnCommandResponseMessage
    {
        public string SigName = "";
        public string Command = "";
        public Dictionary<string, string>? Info = null;
    }
    
    // SigReqControlMessage client -> server 请求控制
    public class SigReqControlMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string RemoteClientId = "";
    }
    
    // SigUnderControlMessage server -> client 请求控制
    public class SigUnderControlMessage
    {
        public string SigName = "";
        public string SelfClientId = "";
        public string ControllerId = "";
    }
    
    // SigOnRemoteDataChannelReadyMessage server -> client 数据通道已经建立
    public class SigOnRemoteDataChannelReadyMessage
    {
        public string SigName = "";
        public string SelfClientId = "";
        public string ControllerId = "";
        public string RoomId = "";
    }
    
    // SigOnRejectControlMessage server -> client
    public class SigOnRejectControlMessage
    {
        public string SigName = "";
        public string ClientId = "";
        public string ControllerId = "";
        public string RoomId = "";
    }

    public static string MakeOnSigKnownErrorMessage(int code)
    {
        return MakeOnSigErrorMessage(code, Errors.ErrorString(code));
    }

    public static string MakeOnSigErrorMessage(int code, string info)
    {
        var msg = new SigErrorMessage
        {
            SigName = SigNameError,
            SigCode = code,
            SigInfo = info
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnHelloMessage(string clientId)
    {
        var msg = new SigOnHelloMessage
        {
            SigName = SigNameOnHello,
            ClientId = clientId,
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnCreatedRoomMessage(string clientId, string remoteClientId, Room room)
    {
        var msg = new SigOnCreatedRoomMessage
        {
            SigName = SigNameOnCreatedRoom,
            ClientId = clientId,
            RemoteClientId = remoteClientId,
            RoomId = room.Id,
            Clients = room.GetClients(),
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnHeartBeatMessage(SigHeartBeatMessage hbMsg)
    {
        var msg = new SigOnHeartBeatMessage
        {
            SigName = SigNameOnHeartBeat,
            ClientId = hbMsg.ClientId,
            Index = hbMsg.Index
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnJoinedRoomMessage(Room room, string clientId, string remoteClientId)
    {
        var msg = new SigOnJoinedRoomMessage
        {
            SigName = SigNameOnJoinedRoom,
            RoomId = room.Id,
            ClientId = clientId,
            RemoteClientId = remoteClientId,
            Clients = room.GetClients(),
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnRemoteJoinedRoomMessage(Room room, string remoteClientId)
    {
        var msg = new SigOnRemoteJoinedRoomMessage
        {
            SigName = SigNameOnRemoteJoinedRoom,
            RoomId = room.Id,
            RemoteClientId = remoteClientId,
            Clients = room.GetClients(),
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnLeftRoomMessage(Room room, string clientId)
    {
        var msg = new SigOnLeftRoomMessage
        {
            SigName = SigNameOnLeftRoom,
            ClientId = clientId,
            RoomId = room.Id,
            Clients = room.GetClients(),
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnRemoteClientLeftMessage(Room room, string leftClientId)
    {
        var msg = new SigOnRemoteLeftRoomMessage
        {
            SigName = SigNameOnRemoteLeftRoom,
            RemoteClientId = leftClientId,
            RoomId = room.Id,
            Clients = room.GetClients()
        };
        return JsonSerializer.Serialize(msg);
    }
    
    // MakeOnInvitedToRoomMessage 通知被邀请者，已经加入Room了
    public static string MakeOnInvitedToRoomMessage(Room room, string invitorClientId, string selfId)
    {
        var msg = new SigOnInvitedToRoomMessage
        {
            SigName = SigNameOnInvitedToRoom,
            InvitorClientId = invitorClientId,
            SelfClientId = selfId,
            RoomId = room.Id,
            Clients = room.GetClients(),
        };
        return JsonSerializer.Serialize(msg);
    }
    
    // MakeOnRemoteInvitedToRoomMessage 通知到发起邀请者,对方已经被邀请进Room了
    public static string MakeOnRemoteInvitedToRoomMessage(Room room, string clientId, string remoteClientId)
    {
        var msg = new SigOnRemoteInvitedToRoomMessage
        {
            SigName = SigNameOnRemoteInvitedToRoom,
            ClientId = clientId,
            RemoteClientId = remoteClientId,
            RoomId = room.Id,
            Clients = room.GetClients(),
        };
        return JsonSerializer.Serialize(msg);
    }

    public static string MakeOnCommandResponseMessage(SigOnCommandResponseMessage msg)
    {
        return JsonSerializer.Serialize(msg);
    }
}