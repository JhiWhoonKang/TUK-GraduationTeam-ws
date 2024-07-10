using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using SharpDX.DirectInput;

namespace RCWS_Situation_room
{
    public partial class GUI : Form
    {
        #region define variables
        /* map */
        private Bitmap MAP_IMAGE;
        private float CURRENT_SCALE = 1.0f;
        private float ZOOM_FACTOR = 1.1f;
        private bool IS_DRAGGING = false;
        private int LAST_X;
        private int LAST_Y;
        /* */

        /* motion control */
        private HashSet<Keys> PRESSEDKEYS = new HashSet<Keys>();

        StreamWriter STREAM_WRITER;
        StreamReader STREAM_READER;
        private NetworkStream NETWORK_STREAM;
        /* */

        /* Packet */
        Packet.SEND_PACKET SEND_DATA;
        Packet.RECEIVED_PACKET RECEIVED_DATA;
        Packet.SEND_PACKET_UDP SEND_DATA_UDP;
        /* */

        /* AZEL Class */
        private Draw DRAW = new Draw();
        private Point lastposition;
        /* */
        
        /* Video */
        UdpClient UDP_CLIENT;
        IPEndPoint ENDPOINT;        
        /* */

        /* CS */
        FormDataSetting formDataSetting;
        /* */

        /* 조이스틱 */
        private DirectInput DIRECT_INPUT;
        private Guid JOYSTICK_GUID;
        private Joystick JOYSTICK;
        Thread THREAD;
        /* */
        #endregion

        public GUI(StreamWriter streamWriter, FormDataSetting formDataSetting)
        {
            InitializeComponent();

            this.formDataSetting = formDataSetting;
            this.STREAM_WRITER = streamWriter;

            MAP_IMAGE = new Bitmap(Properties.Resources.demomap);
            UpdateMapImage();

            SEND_DATA = new Packet.SEND_PACKET();
            RECEIVED_DATA = new Packet.RECEIVED_PACKET();

            PB_MAP.SizeMode = PictureBoxSizeMode.AutoSize;
            PB_MAP.MouseWheel += MapPictureBox_MouseWheel;
            PB_MAP.MouseDown += MapPictureBox_MouseDown;
            PB_MAP.MouseMove += MapPictureBox_MouseMove;
            PB_MAP.MouseUp += MapPictureBox_MouseUp;
            
            KeyDown += new KeyEventHandler(GUI_KeyDown);
            KeyUp += new KeyEventHandler(GUI_KeyUp);

            this.Focus();
        }

        #region Joystick
        private void InitializeJoystick()
        {
            try
            {
                DIRECT_INPUT = new DirectInput();

                JOYSTICK_GUID = Guid.Empty;

                foreach (var deviceInstance in DIRECT_INPUT.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                    JOYSTICK_GUID = deviceInstance.InstanceGuid;

                if (JOYSTICK_GUID == Guid.Empty)
                {
                    foreach (var deviceInstance in DIRECT_INPUT.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                        JOYSTICK_GUID = deviceInstance.InstanceGuid;
                }

                if (JOYSTICK_GUID == Guid.Empty)
                {
                    RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.AppendText("No Joy" + "\r\n"); });
                    return;
                }

                JOYSTICK = new Joystick(DIRECT_INPUT, JOYSTICK_GUID);
                JOYSTICK.SetCooperativeLevel(this.Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);

                RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.AppendText("Joy Connected" + "\r\n"); });

                THREAD = new Thread(Joy_Run);
                THREAD.Start();
            }
            catch (Exception ex)
            {
                RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.AppendText("Joy Error" + ex.Message + "\r\n"); });
            }
        }

        private void Joy_Run()
        {
            var joystick = new Joystick(DIRECT_INPUT, JOYSTICK_GUID);
            
            joystick.Acquire();

            SEND_DATA.Button = 0xfc000004;

            int pre_1 = SEND_DATA.BodyPan;
            int pre_2 = SEND_DATA.BodyTilt;
            uint pre_3 = SEND_DATA.Button;

            while (true)
            {
                joystick.Poll();
                var state = joystick.GetCurrentState();
                var buttons = state.Buttons;

                float ax_X = ((float)(state.X - 32511) / (float)(33024));
                if (ax_X < 0.2025 && ax_X > -0.2025)
                {
                    SEND_DATA.BodyPan = 0;
                }
                else
                {
                    if (ax_X >= 0.2025)
                    {
                        SEND_DATA.BodyPan = (int)(ax_X * 400 - 80);
                    }
                    else if (ax_X <= -0.2025)
                    {
                        SEND_DATA.BodyPan = (int)(ax_X * 400 + 80);
                    }
                }

                float ax_Y = ((float)(state.Y - 32511) / (float)(33024));
                if (ax_Y < 0.22 && ax_Y > -0.22)
                {
                    SEND_DATA.BodyTilt = 0;
                }
                else
                {
                    if (ax_Y >= 0.22)
                    {
                        SEND_DATA.BodyTilt = (int)(ax_Y * 100 - 20);
                    }
                    else if (ax_Y <= -0.22)
                    {
                        SEND_DATA.BodyTilt = (int)(ax_Y * 100 + 20);
                    }
                }

                if (buttons[0] == true)
                {
                    SEND_DATA.Button = (SEND_DATA.Button | 0x01);
                }
                else if (buttons[0] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000001));
                }

                if (buttons[1] == true)
                {
                    SEND_DATA.Button = (SEND_DATA.Button | 0x02);
                }
                else if (buttons[1] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000002));
                }

                if (buttons[2] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x04;
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000038));
                }

                if (buttons[3] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x08;
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000034));
                }

                if (buttons[4] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x10;
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x0000002C));
                }

                if (buttons[5] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x20;
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x0000001C));
                }

                if (buttons[6] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x40;
                }
                else if (buttons[6] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000040));
                }

                if (buttons[7] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x80;
                }
                else if (buttons[7] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000080));
                }

                if (pre_1 != SEND_DATA.BodyPan || pre_2 != SEND_DATA.BodyTilt || pre_3 != SEND_DATA.Button)
                {
                    byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                    STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                    STREAM_WRITER.BaseStream.FlushAsync();

                    pre_1 = SEND_DATA.BodyPan;
                    pre_2 = SEND_DATA.BodyTilt;
                    pre_3 = SEND_DATA.Button;
                }

                Thread.Sleep(1);
            }
        }
        #endregion

        #region Map
        private void UpdateMapImage()
        {
            int newWidth = (int)(MAP_IMAGE.Width * CURRENT_SCALE);
            int newHeight = (int)(MAP_IMAGE.Height * CURRENT_SCALE);
            var resizedImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(MAP_IMAGE, new Rectangle(0, 0, newWidth, newHeight));
            }
            PB_MAP.Image = resizedImage;
        }

        private void MapPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                CURRENT_SCALE *= ZOOM_FACTOR;
            }
                
            else
            {
                CURRENT_SCALE /= ZOOM_FACTOR;
            }
                
            UpdateMapImage();
        }

        private void MapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IS_DRAGGING = true;
                LAST_X = e.X;
                LAST_Y = e.Y;
            }
        }

        private void MapPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (IS_DRAGGING)
                {
                    int deltaX = e.X - LAST_X;
                    int deltaY = e.Y - LAST_Y;

                    int map_newX = PB_MAP.Location.X + deltaX;
                    int map_newY = PB_MAP.Location.Y + deltaY;

                    PB_MAP.Location = new Point(map_newX, map_newY);
                    LAST_X = e.X;
                    LAST_Y = e.Y;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error in MapPictureBox_MouseMove: " + ex.Message);
            }
        }

        private void MapPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IS_DRAGGING = false;
            }
        }
        #endregion

        #region Motion Control
        private readonly SemaphoreSlim keySemaphore = new SemaphoreSlim(1, 1);
        private async void GUI_KeyDown(object sender, KeyEventArgs e)
        {
            await keySemaphore.WaitAsync();
            
            try
            {
                if (PRESSEDKEYS.Add(e.KeyCode))
                {
                    await SendCommandStructure();
                }
            }
            finally
            {
                keySemaphore.Release();
            }
        }

        private async void GUI_KeyUp(object sender, KeyEventArgs e)
        {
            await keySemaphore.WaitAsync();

            try
            {
                if (PRESSEDKEYS.Remove(e.KeyCode))
                {
                    await SendCommandStructure();
                }
            }
            finally
            {
                keySemaphore.Release();
            }
        }

        private async Task SendCommandStructure()
        {
            //sendStruct.BodyPan = 0;
            //sendStruct.BodyTilt = 0;
            //sendStruct.Fire = 0;
            //sendStruct.TakeAim = 0;

            ///* motion data */
            //if (pressedKeys.Contains(Keys.A))
            //    sendStruct.BodyPan = 1;

            //if (pressedKeys.Contains(Keys.D))
            //    sendStruct.BodyPan = -1;

            //if (pressedKeys.Contains(Keys.W))
            //    sendStruct.BodyTilt = 1;

            //if (pressedKeys.Contains(Keys.S))
            //    sendStruct.BodyTilt = -1;
            ///* */

            ///* weapon data */
            //if (pressedKeys.Contains(Keys.F))
            //    sendStruct.Fire = 1;

            //if (pressedKeys.Contains(Keys.T))
            //    sendStruct.TakeAim = 1;

            //if (pressedKeys.Contains(Keys.C))
            //{
            //    if (sendStruct.Permission == 1)
            //        sendStruct.Permission = 2;
            //    else
            //        sendStruct.Permission = 1;
            //}
            ///* */

            ///* 이외 키 무효화 */
            //else
            //{

            //}
            ///* */

            //SendDisplay($"Pan: {sendStruct.BodyPan}, Tilt: {sendStruct.BodyTilt}, Permission: {sendStruct.Permission}, TakeAim: {sendStruct.TakeAim}, Fire: {sendStruct.Fire}\n");

            byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
            await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
            await STREAM_WRITER.BaseStream.FlushAsync();
        }
        #endregion

        #region TCP
        private async void btn_RCWS_Connect_Click(object sender, EventArgs e)
        {
            await Task.Run(() => TcpConnectAsync());
        }

        private async Task TcpConnectAsync()
        {
            TcpClient tcpClient = new TcpClient();

            try
            {
                SendDisplay("Connecting...");
                tcpClient.Connect(define.SERVER_IP, define.TCPPORT);

                NETWORK_STREAM = tcpClient.GetStream();
                STREAM_READER = new StreamReader(NETWORK_STREAM);
                STREAM_WRITER = new StreamWriter(NETWORK_STREAM);
                STREAM_WRITER.AutoFlush = true;
                InitializeJoystick();
            }
            catch (Exception ex)
            {
                ReceiveDisplay("Connect ERROR: " + ex.Message);
                return;
            }

            ReceiveDisplay("Server Connected");

            try
            {
                while (true)
                {
                    byte[] receivedData = new byte[Marshal.SizeOf(typeof(Packet.RECEIVED_PACKET))];
                    await NETWORK_STREAM.ReadAsync(receivedData, 0, receivedData.Length);
                    RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.AppendText(receivedData + "\r\n"); });

                    RECEIVED_DATA = TcpReturn.BytesToStruct<Packet.RECEIVED_PACKET>(receivedData);

                    ReceiveDisplay($"OpticalTilt: {RECEIVED_DATA.OpticalTilt}, OpticalPan: {RECEIVED_DATA.OpticalPan}, BodyTilt: {RECEIVED_DATA.BodyTilt}" +
                        $", BodyPan: {RECEIVED_DATA.BodyPan}");

                    /*
                    string dataToShow = $"OpticalTilt: {receivedStruct.OpticalTilt}, OpticalPan: {receivedStruct.OpticalPan}, BodyTilt: {receivedStruct.BodyTilt}" +
                        $", BodyPan: {receivedStruct.BodyPan}";

                    formDataSetting.DisplayReceivedData(dataToShow);
                    await Task.Run(() => ProcessReceivedData(receivedData));

                    pictureBox_azimuth.Invalidate();
                    pictureBox_azimuth.Refresh();

                    tb_body_azimuth.Text = receivedStruct.BodyPan.ToString();
                    tb_body_elevation.Text = receivedStruct.BodyTilt.ToString();
                    tb_optical_azimuth.Text = receivedStruct.OpticalPan.ToString();
                    tb_optical_elevation.Text = receivedStruct.OpticalTilt.ToString();

                    tb_Magnification.Text = receivedStruct.Magnification.ToString();
                    tb_Pointdistance.Text = receivedStruct.pointdistance.ToString();
                    tb_Distance.Text = receivedStruct.distance.ToString();

                    tb_RemainingBullets.Text = receivedStruct.Remaining_bullets.ToString();
                    tb_gunvoltage.Text = receivedStruct.GunVoltage.ToString();

                    if (receivedStruct.TakeAim == 0 || receivedStruct.TakeAim == 2)
                    {
                        btn_takeaim.BackColor = Color.Green;
                        btn_takeaim.Text = "Controlable";
                    }

                    else if (receivedStruct.TakeAim == 1)
                    {
                        btn_takeaim.BackColor = Color.Red;
                        btn_takeaim.Text = "Uncontrolable";
                    }

                    else
                    {
                        btn_takeaim.BackColor = Color.Empty;
                        btn_takeaim.Text = "No Aim data. RETRY";
                    }

                    if (receivedStruct.Fire == 0 || receivedStruct.Fire == 2)
                    {
                        btn_fire.BackColor = Color.Green;
                        btn_fire.Text = "Controlable";
                    }

                    else if (receivedStruct.Fire == 1)
                    {
                        btn_fire.BackColor = Color.Red;
                        btn_fire.Text = "Uncontrolable";
                    }

                    else
                    {
                        btn_fire.BackColor = Color.Empty;
                        btn_fire.Text = "No Fire data. RETRY";
                    }

                    if (receivedStruct.Permission == 0 || receivedStruct.Permission == 2)
                    {
                        btn_Permission.BackColor = Color.Green;
                        btn_Permission.Text = "Controlable";
                    }

                    else if (receivedStruct.Permission == 1)
                    {
                        btn_Permission.BackColor = Color.Red;
                        btn_Permission.Text = "Uncontrolable";
                    }

                    else
                    {
                        btn_Permission.BackColor = Color.Empty;
                        btn_Permission.Text = "No Permission data. RETRY";
                    }
                    */
                }
            }

            catch (Exception ex)
            {
                ReceiveDisplay("Connect ERROR: " + ex.Message);
                return;
            }
        }

        private void SendDisplay(string str)
        {
            RTB_SEND_DISPLAY.Invoke((MethodInvoker)delegate { RTB_SEND_DISPLAY.AppendText(str + "\r\n"); });
            RTB_SEND_DISPLAY.Invoke((MethodInvoker)delegate { RTB_SEND_DISPLAY.ScrollToCaret(); });
        }

        private void ReceiveDisplay(string str)
        {
            RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.AppendText(str + "\r\n"); });
            RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.ScrollToCaret(); });
        }
        #endregion

        #region UDP, Video
        private void UdpConnect()
        {
            MemoryStream ms = new MemoryStream();
            
            try
            {
                StartReceiving(define.SERVER_IP, define.UDPPORT);
            }
            catch (Exception ex)
            {
                btn_Camera_connect.BackColor = Color.Red;
                btn_Camera_connect.ForeColor = Color.White;
                
                SendDisplay("Send Data: " + ex.Message);
            }
            btn_Camera_connect.BackColor = Color.Green;
            btn_Camera_connect.ForeColor = Color.White;
            btn_Camera_connect.Enabled = false;
        }

        private void pbI_Video_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private async void pbI_Video_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //SendUdpData(define.SERVER_IP, define.UDPPORT2, true, false, e.X, e.Y);
                SEND_DATA_UDP.X = e.X;
                SEND_DATA_UDP.Y = e.Y;
                SEND_DATA_UDP.Left_or_Right = 1;
                byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA_UDP);
                await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                await STREAM_WRITER.BaseStream.FlushAsync();
            }

            if (e.Button == MouseButtons.Right)
            {
                //SendUdpData(define.SERVER_IP, define.UDPPORT2, false, true);
                SEND_DATA_UDP.Left_or_Right = 2;
                byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA_UDP);
                await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                await STREAM_WRITER.BaseStream.FlushAsync();
            }
        }

        private void StartReceiving(string ip, int port)
        {
            UDP_CLIENT = new UdpClient(8000);
            ENDPOINT = new IPEndPoint(IPAddress.Parse(ip), port);

            UDP_CLIENT.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }       

        private void Showvideo(byte[] Data_)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    using (var ms = new MemoryStream(Data_))
                    {
                        var receivedImage = System.Drawing.Image.FromStream(ms);
                        pbI_Video.Image = receivedImage;
                    }
                }
                catch (Exception ex)
                {
                    SendDisplay("Connect ERROR: " + ex.Message);
                }
            });
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            byte[] receivedData = UDP_CLIENT.EndReceive(ar, ref ENDPOINT);

            if (receivedData.Length < 65000)
            {
                Showvideo(receivedData);
            }
            else
            {
                byte[] dgram = UDP_CLIENT.Receive(ref ENDPOINT);
                if (dgram.Length < 65000)
                {
                    Showvideo(receivedData.Concat(dgram).ToArray());
                }
                else
                {
                    byte[] dgram_ = receivedData.Concat(dgram).ToArray();
                    byte[] dgram__ = UDP_CLIENT.Receive(ref ENDPOINT);
                    Showvideo(dgram_.Concat(dgram__).ToArray());
                }
            }
            UDP_CLIENT.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void SendUdpData(string serverIp, int port, bool mouse_L, bool mouse_R)
        {
            try
            {
                UdpClient client = new UdpClient();

                byte[] bytes = new byte[2];
                bytes[0] = Convert.ToByte(mouse_L);
                bytes[1] = Convert.ToByte(mouse_R);

                client.Send(bytes, bytes.Length, serverIp, port);

                client.Close();
            }
            catch (Exception ex)
            {   
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }                
        #endregion

        #region AZEL GUI

        private void pictureBox_azimuth_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            int centerX = pictureBox_azimuth.Width / 2;
            int centerY = pictureBox_azimuth.Height / 3 * 2;

            /* */
            Pen redPen = new Pen(Color.Red, 8);
            g.DrawLine(redPen, new Point(centerX - 10, centerY - 10), new Point(centerX + 10, centerY + 10));
            g.DrawLine(redPen, new Point(centerX + 10, centerY - 10), new Point(centerX - 10, centerY + 10));
            redPen.Dispose();
            /* */

            /* */
            int lineLength = centerX * 2;
            //g.DrawEllipse(Pens.Black, 0, 0, pictureBox_azimuth.Width - 1, pictureBox_azimuth.Height - 1);
            /* */

            /* Body Pan */
            double radianAngleRCWS = RECEIVED_DATA.BodyPan * Math.PI / 180.0;
            int endXRCWS = centerX + (int)(lineLength * Math.Sin(radianAngleRCWS));
            int endYRCWS = centerY - (int)(lineLength * Math.Cos(radianAngleRCWS));
            g.DrawLine(Pens.Red, new Point(centerX, centerY), new Point(endXRCWS, endYRCWS));
            /* */

            /* Optical Pan */
            double radianAngleOptical = RECEIVED_DATA.OpticalPan * Math.PI / 180.0;
            int endXOptical = centerX + (int)(lineLength * Math.Sin(radianAngleOptical));
            int endYOptical = centerY - (int)(lineLength * Math.Cos(radianAngleOptical));
            g.DrawLine(Pens.Blue, new Point(centerX, centerY), new Point(endXOptical, endYOptical));
            /* */

            DRAW.Drawing(e.Graphics);
        }

        Point clickLocation;
        private void pictureBox_azimuth_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                clickLocation = e.Location;
                contextMenuStrip1.Show(pictureBox_azimuth, clickLocation);

                Bitmap bmp;
                if (pictureBox_azimuth.Image == null) bmp = new Bitmap(pictureBox_azimuth.Width, pictureBox_azimuth.Height);
                else bmp = new Bitmap(pictureBox_azimuth.Image);

                pictureBox_azimuth.Image = bmp;
            }
        }

        private void suspectedEnemyActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string imagePath = "이미지 파일 경로";

            if (File.Exists(imagePath)) DrawImage(imagePath);
            else MessageBox.Show("Cannot draw Image: ");
        }

        private void enemyMovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string imagePath = "이미지 파일 경로";

            if (File.Exists(imagePath)) DrawImage(imagePath);
            else MessageBox.Show("Cannot draw Image: ");
        }

        private void enemyConcentrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string imagePath = "이미지 파일 경로";

            if (File.Exists(imagePath)) DrawImage(imagePath);
            else MessageBox.Show("Cannot draw Image: ");
        }

        private void addPinPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DRAW.AddPinPoint(lastposition);
            pictureBox_azimuth.Invalidate();
        }

        private void deletePinPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DRAW.DeletePinPoint(lastposition);
            pictureBox_azimuth.Invalidate();
        }

        void DrawImage(string path)
        {
            using (Graphics g = Graphics.FromImage(pictureBox_azimuth.Image))
            {
                try
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                    {
                        Point clickLocation = new Point(pictureBox_azimuth.Width / 2, pictureBox_azimuth.Height / 2);
                        g.DrawImage(image, clickLocation.X - image.Width / 2, clickLocation.Y - image.Height / 2);
                        pictureBox_azimuth.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot draw Image: " + ex.Message);
                }
            }
        }
        #endregion

        #region GUI Button
        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();

            THREAD.Join();
        }

        private void btn_Camera_Connect_Click(object sender, EventArgs e)
        {
            Task.Run(() => UdpConnect());
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            Setting SettingForm = new Setting();
            SettingForm.Show();
        }
        #endregion
    }
}