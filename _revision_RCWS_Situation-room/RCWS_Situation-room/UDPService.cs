using System;
using System.Drawing;
using RCWS_Situation_room.Data;
using RCWS_Situation_room.define;

namespace RCWS_Situation_room
{
    public class UDPService
    {
        private readonly FormMain _formMain;
        private readonly UDPCmd _udpCmd;

        public event Action<byte[]> VideoReceived;

        public UDPService(FormMain formMain)
        {
            _formMain = formMain;
            _udpCmd = new UDPCmd();
            _udpCmd.VideoReceived += OnVideoReceived;
        }

        public void InitializeReceiving()
        {
            _udpCmd.StartReceiving(Define.SERVER_IP, Define.TEST_UDPPORT);
            _formMain.UpdateUI(() =>
            {
                _formMain.BTN_CAMERA_CONNECT.ForeColor = Color.White;
                _formMain.BTN_CAMERA_CONNECT.BackColor = Color.Green;
            });
        }

        private void OnVideoReceived(byte[] data)
        {
            VideoReceived?.Invoke(data);
        }
    }
}