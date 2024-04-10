////using System;
////using System.Collections.Generic;
////using System.ComponentModel;
////using System.Data;
////using System.Drawing;
////using System.IO;
////using System.Linq;
////using System.Net.Sockets;
////using System.Net;
////using System.Text;
////using System.Threading;
////using System.Threading.Tasks;
////using System.Windows.Forms;
////using OpenCvSharp;
////using System.Diagnostics;
////using System.Collections;

////namespace RCWS_Server
////{
////    public partial class Video : Form
////    {
////        private const int Port = 5002;
////        private const string ServerIP = "127.0.0.1";
////        private UdpClient _udpClient;
////        private IPEndPoint _remoteEndPoint;

////        public Video()
////        {
////            InitializeComponent();

////            // UdpClient 초기화
////            _udpClient = new UdpClient(Port);
////            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

////            pictureBox_Display.SizeMode = PictureBoxSizeMode.StretchImage;

////            // 영상 수신 및 디코딩 스레드
////            Thread receiveAndDisplayThread = new Thread(ReceiveAndDisplay);
////            receiveAndDisplayThread.IsBackground = true;
////            receiveAndDisplayThread.Start();
////        }

////        // 영상 수신 및 디코딩 함수
////        private async void ReceiveAndDisplay()
////        {
////            using (UdpClient udpClient = new UdpClient(Port))
////            using (TcpClient tcpClient = new TcpClient())
////            { 
////                try
////                {
////                    await tcpClient.ConnectAsync(IPAddress.Parse(ServerIP), Port);

////                    while (true)
////                    {
////                        var receivedUdpResult = await udpClient.ReceiveAsync();
////                        byte[] receivedBytes = receivedUdpResult.Buffer;

////                        Mat image = ConvertByteArrayToImage(receivedBytes);

////                        if (!image.Empty())
////                        {
////                            pictureBox_Display.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
////                        }
////                    }
////                }
////                catch (Exception ex)
////                {
////                    Console.WriteLine($"Error: {ex.Message}");
////                }
////            }
////        }

////        // 바이트 배열을 Mat 객체로 변환하는 함수
////        private Mat ConvertByteArrayToImage(byte[] byteArray)
////        {
////            MemoryStream ms = new MemoryStream(byteArray);
////            var bitmap = new System.Drawing.Bitmap(ms);
////            return OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
////        }
////    }
////}

//namespace RCWS_Server
//{
//    public partial class Video : Form
//    {
//        private const int Width = 1080;
//        private const int Height = 720;
//        private const int Port = 5001;
//        private const string ServerIP = "127.0.0.1";

//        private UdpClient _udpClient;
//        private IPEndPoint _endPoint;
//        private CancellationTokenSource _cancellationTokenSource;
//        private VideoCapture _capture;

//        public Video()
//        {
//            InitializeComponent();

//            _udpClient = new UdpClient();
//            _endPoint = new IPEndPoint(IPAddress.Parse(ServerIP), Port);
//            _cancellationTokenSource = new CancellationTokenSource();
//            _capture = new VideoCapture(0);

//            if (!_capture.IsOpened())
//            {
//                MessageBox.Show("Camera Open failed");
//                Application.Exit();
//            }
//            else
//            {
//                MessageBox.Show("Camera Open Success");
//            }

//            Thread.Sleep(2000);

//            Thread captureThread = new Thread(() => CaptureAndSend(_capture));
//            captureThread.IsBackground = true;
//            captureThread.Start();
//        }

//        private void CaptureAndSend(VideoCapture capture)
//        {
//            Console.WriteLine("Client start");

//            while (true)
//            {
//                try
//                {
//                    using (var image = new Mat())
//                    {
//                        capture.Read(image);
//                        if (image.Empty()) continue;

//                        var imageData = ConvertImageToByteArray(image);

//                        if (imageData != null)
//                        {
//                            _udpClient.Send(imageData, imageData.Length, _endPoint);

//                            // ACK를 수신할 때까지 대기합니다.
//                            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
//                            byte[] ackBuffer = _udpClient.Receive(ref serverEndPoint);
//                            bool ackReceived = BitConverter.ToBoolean(ackBuffer, 0);
//                            if (!ackReceived) throw new Exception("ACK message not received");
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Client Error: " + ex.Message);
//                }
//            }
//        }

//        private static byte[] ConvertImageToByteArray(Mat image)
//        {
//            using (var ms = new System.IO.MemoryStream())
//            {
//                using (var bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image))
//                {
//                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//                }
//                return ms.ToArray();
//            }
//        }
//    }
//}

////using OpenCvSharp;
////using System;
////using System.IO;
////using System.Net;
////using System.Net.Sockets;
////using System.Text;
////using System.Threading;
////using System.Threading.Tasks;

////namespace RCWS_Server
////{
////    public struct Command
////    {
////        public int X { get; set; }
////        public int Y { get; set; }
////        public string Name { get; set; }
////    }

////    class Program
////    {
////        private const int UdpPort = 9000;
////        private const int TcpPort = 9001;
////        private const string ServerIP = "192.168.0.2";

////        private static async Task ReceiveImageAsync(UdpClient udpClient)
////        {
////            while (true)
////            {
////                try
////                {
////                    var receivedUdpResult = await udpClient.ReceiveAsync();
////                    byte[] receivedBytes = receivedUdpResult.Buffer;

////                    Mat image = ConvertByteArrayToImage(receivedBytes);
////                    if (!image.Empty())
////                    {
////                        Cv2.ImShow("Received Image", image);
////                        Cv2.WaitKey(30);
////                    }
////                }
////                catch (Exception ex)
////                {
////                    Console.WriteLine(ex.Message);
////                }
////            }
////        }

////        private static Mat ConvertByteArrayToImage(byte[] byteArray)
////        {
////            MemoryStream ms = new MemoryStream(byteArray);
////            var bitmap = new System.Drawing.Bitmap(ms);
////            return OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
////        }

////        static async Task Main(string[] args)
////        {
////            Console.WriteLine("Video Receiver started...");

////            using (UdpClient udpClient = new UdpClient(UdpPort))
////            using (TcpClient tcpClient = new TcpClient())
////            {
////                try
////                {
////                    await tcpClient.ConnectAsync(IPAddress.Parse(ServerIP), TcpPort);
////                    Console.WriteLine("Connected to server.");

////                    // Start receiving images asynchronously
////                    Task imageReceivingTask = ReceiveImageAsync(udpClient);

////                    // Send commands
////                    using (var stream = tcpClient.GetStream())
////                    {
////                        while (true)
////                        {
////                            Command command = GetUserInput();
////                            byte[] buffer = Encoding.ASCII.GetBytes($"{command.X},{command.Y},{command.Name}");
////                            await stream.WriteAsync(buffer, 0, buffer.Length);
////                        }
////                    }
////                }
////                catch (Exception ex)
////                {
////                    Console.WriteLine($"Error: {ex.Message}");
////                }
////            }
////        }

////        private static Command GetUserInput()
////        {
////            Command command = new Command();
////            Console.Write("X: ");
////            command.X = int.Parse(Console.ReadLine());
////            Console.Write("Y: ");
////            command.Y = int.Parse(Console.ReadLine());
////            Console.Write("Name: ");
////            command.Name = Console.ReadLine();
////            return command;
////        }
////    }
////}

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;

namespace RCWS_Server
{
    public partial class Video : Form
    {
        private const int VideoPort = 5001;
        private UdpClient _udpClient;
        private IPEndPoint _remoteEndPoint;
        private VideoCapture _capture;

        public Video(StreamWriter streamWriter)
        {
            InitializeComponent();

            _udpClient = new UdpClient(VideoPort);
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            _capture = new VideoCapture(0);
            if (!_capture.IsOpened())
            {
                MessageBox.Show("카메라를 여는 데 실패했습니다.");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("카메라가 성공적으로 열렸습니다.");
            }
        }

        private void btnConnectUDP_Click(object sender, EventArgs e)
        {
            Thread thread2 = new Thread(UdpConnect);
            thread2.IsBackground = true;
            thread2.Start();
        }

        private void UdpConnect()
        {
            while (true)
            {
                try
                {
                    using (var image = new Mat())
                    {
                        _capture.Read(image);
                        if (image.Empty()) continue;

                        var imageData = ConvertImageToByteArray(image);

                        if (imageData != null)
                        {
                            _udpClient.Send(imageData, imageData.Length, _remoteEndPoint);
                            writeUdpRichTextbox("이미지 데이터를 클라이언트에게 보냈습니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    writeUdpRichTextbox("오류: " + ex.Message);
                }
            }
        }

        // 40,060
        private static byte[] ConvertImageToByteArray(Mat image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                using (var bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image))
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                return ms.ToArray();
            }
        }

        private void writeUdpRichTextbox(string str)
        {
            richUdpConnectionStatus.Invoke((MethodInvoker)delegate { richUdpConnectionStatus.AppendText(str + "\r\n"); });
            richUdpConnectionStatus.Invoke((MethodInvoker)delegate { richUdpConnectionStatus.ScrollToCaret(); });
        }
    }
}
