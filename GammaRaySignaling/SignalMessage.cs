namespace GammaRaySignaling;

public class SignalMessage
{
    public const String SigNameHello = "hello";
    public const String SigNameOnHello = "on_hello";
    public const String SigNameCreateRoom = "create_room";
    public const String SigNameOnCreatedRoom = "on_created_room";
    public const String SigNameJoinRoom = "join_room";
    public const String SigNameOnJoinedRoom = "on_joined_room";
    public const String SigNameOnRemoteJoinedRoom = "on_remote_joined_room";
    public const String SigNameLeaveRoom = "leave_room";
    public const String SigNameOnLeftRoom = "on_left_room";
    public const String SigNameOnRemoteLeftRoom = "on_remote_left_room";
    public const String SigNameInviteClient = "invite_client";
    public const String SigNameOnInvitedToRoom = "on_invited_to_room";
    public const String SigNameOnRemoteInvitedToRoom = "on_remote_invited_to_room";
    public const String SigNameHeartBeat = "heart_beat";
    public const String SigNameOnHeartBeat = "on_heart_beat";
    public const String SigNameOfferSdp = "offer_sdp";
    public const String SigNameAnswerSdp = "answer_sdp";
    public const String SigNameIce = "ice";
    public const String SigNameError = "sig_error";
    public const String SigNameForceIFrame = "force_iframe";
    public const String SigNameCommand = "sig_command";
    public const String SigNameOnCommandResponse = "sig_on_command_response";
    public const String SigNameReqControl = "sig_req_control";
    public const String SigNameUnderControl = "sig_under_control";
    public const String SigNameOnDataChannelReady = "on_data_channel_ready";
    public const String SigNameOnRejectControl = "on_reject_control";

    public const String CmdQueryServerStatus = "query_status";

    public const String KeySigName = "sig_name";
    public const String KeyClientId = "client_id";
    public const String KeyRoomId = "room_id";
    public const String KeyRemoteClientId = "remote_client_id";
    public const String KeyControlClientId = "controller_client_id";
    public const String KeySelfClientId = "self_client_id";
    public const String KeyIndex = "index";
    public const String KeyPlatform = "platform";
    public const String KeySdp = "sdp";
    public const String KeyIce = "ice";
    public const String KeyMid = "mid";
    public const String KeySdpMLineIndex = "sdp_m_line_index";
    public const String KeyId = "id";
    public const String KeyName = "name";
    public const String KeyClients = "clients";
    public const String KeyRoom = "room";
    public const String KeyRooms = "rooms";
    public const String KeyAllowReSend = "allow_resend";
}