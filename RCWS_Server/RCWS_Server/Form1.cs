using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using OpenCvSharp;

namespace RCWS_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        StreamReader streamReader1;  // 데이타 읽기 위한 스트림리더
        StreamWriter streamWriter1;  // 데이타 쓰기 위한 스트림라이터    

        private void btnConnectTCP_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(TcpConnect); // Thread 객채 생성, Form과는 별도 쓰레드에서 connect 함수가 실행
            thread1.IsBackground = true; // Form이 종료되면 thread1도 종료.
            thread1.Start(); // thread1 시작.

        }

        private void btnConnectUDP_Click(object sender, EventArgs e)
        {
            Thread thread2 = new Thread(UdpConnect);
            thread2.IsBackground = true;
            thread2.Start();
        }

        private void TcpConnect()  // thread1에 연결된 함수. 메인폼과는 별도로 동작
        {
            //TcpListener tcpListener1 = new TcpListener(IPAddress.Parse(textBox_TCPIP.Text), int.Parse(textBox_TCPPort.Text)); // 서버 객체 생성 및 IP주소와 Port번호를 할당
            TcpListener tcpListener1 = new TcpListener(IPAddress.Parse("127.0.0.1"), int.Parse("7000")); // 서버 객체 생성 및 IP주소와 Port번호를 할당
            tcpListener1.Start();  // 서버 시작
            writeTcpRichTextbox("서버 준비...클라이언트 기다리는 중...");

            TcpClient tcpClient1 = tcpListener1.AcceptTcpClient(); // 클라이언트 접속 확인
            writeTcpRichTextbox("클라이언트 연결됨...");

            streamReader1 = new StreamReader(tcpClient1.GetStream());  // 읽기 스트림 연결
            streamWriter1 = new StreamWriter(tcpClient1.GetStream());  // 쓰기 스트림 연결
            streamWriter1.AutoFlush = true;  // 쓰기 버퍼 자동으로 뭔가 처리.. 

            while (tcpClient1.Connected)  // 클라이언트가 연결되어 있는 동안
            {
                string receivedDirection = streamReader1.ReadLine(); // 수신데이터를 읽어서 receivedDirection 변수에 저장

                if (!string.IsNullOrEmpty(receivedDirection))
                {
                    // 받은 방향을 로깅
                    writeTcpRichTextbox("Received direction: " + receivedDirection);
                    char direction = receivedDirection[0];

                    switch (direction)
                    {
                        case 'U': // 위로
                                  // 위로 움직이는 로직 구현
                            break;

                        case 'L': // 왼쪽으로
                                  // 왼쪽으로 움직이는 로직 구현
                            break;

                        case 'D': // 아래로
                                  // 아래로 움직이는 로직 구현
                            break;

                        case 'R': // 오른쪽으로
                                  // 오른쪽으로 움직이는 로직 구현
                            break;
                    }
                }
            }
        }

        private UdpClient udpServer;
        private IPEndPoint remoteEndpoint;

        private byte[] buf0Msg;
        private byte[] buf1Msg;
        private byte[] buf2Msg;
        private byte[] buf3Msg;

        string coor_x, coor_y;   
        private void UdpConnect()
        {
            int localPort = int.Parse(textBox_UDPPort.Text);

            udpServer = new UdpClient(localPort);
            udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);

            writeUdpRichTextbox("서버 준비...클라이언트 기다리는 중...");

            while (true)
            {
                try
                {
                    byte[] receivedData = udpServer.Receive(ref remoteEndpoint);
                    if (receivedData[0] == 1)
                    {
                        buf2Msg = new byte[1]
                        {
                            receivedData[2]
                        };

                        buf3Msg = new byte[1]
                        {
                            receivedData[3]
                        };
                    }

                    coor_x = BitConverter.ToString(buf2Msg);
                    coor_y = BitConverter.ToString(buf3Msg);

                    coor_x += ", " + coor_y;

                    writeUdpRichTextbox(coor_x);
                }
                catch (Exception ex)
                {
                    writeUdpRichTextbox("오류: " + ex.Message);
                }
            }
        }

        private void writeTcpRichTextbox(string str)  // richTextbox1 에 쓰기 함수
        {
            richTcpConnectionStatus.Invoke((MethodInvoker)delegate { richTcpConnectionStatus.AppendText(str + "\r\n"); }); // 데이타를 수신창에 표시, 반드시 invoke 사용. 충돌피함.
            richTcpConnectionStatus.Invoke((MethodInvoker)delegate { richTcpConnectionStatus.ScrollToCaret(); });  // 스크롤을 젤 밑으로.
        }

        private void writeUdpRichTextbox(string str)  // richTextbox1 에 쓰기 함수
        {
            richUdpConnectionStatus.Invoke((MethodInvoker)delegate { richUdpConnectionStatus.AppendText(str + "\r\n"); }); // 데이타를 수신창에 표시, 반드시 invoke 사용. 충돌피함.
            richUdpConnectionStatus.Invoke((MethodInvoker)delegate { richUdpConnectionStatus.ScrollToCaret(); });  // 스크롤을 젤 밑으로.
        }

        StreamWriter streamWriter;
        private void btn_Video_Click(object sender, EventArgs e)
        {
            Video video = new Video(streamWriter);
            video.Show();
        }
    }
}