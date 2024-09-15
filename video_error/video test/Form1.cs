using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace video_test
{
    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        private const int PORT = 12345;
        private List<byte> receivedData;

        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            receivedData = new List<byte>(); // 초기화

            udpClient = new UdpClient(PORT);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            timer = new Timer();
            timer.Interval = 30; // 30ms 간격으로 타이머 설정
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, PORT);
            byte[] receivedPacket = udpClient.EndReceive(ar, ref remoteEP);

            // 수신된 데이터 저장
            receivedData.AddRange(receivedPacket); // List<byte>에 추가

            // 다음 패킷 수신 대기
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (receivedData.Count > 0)
            {
                // 바이트 배열을 Bitmap으로 변환
                using (var ms = new System.IO.MemoryStream(receivedData.ToArray())) // List<byte>를 배열로 변환
                {
                    // JPEG 형식으로 인코딩된 데이터를 Bitmap으로 변환
                    try
                    {
                        using (var bitmap = new Bitmap(ms))
                        {
                            e.Graphics.DrawImage(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"비트맵 변환 오류: {ex.Message}");
                    }
                }
            }
        }
    }
}
