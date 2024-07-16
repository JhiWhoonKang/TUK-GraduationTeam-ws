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
        private float panAngle;
        private float distance;
        private bool pp_flag = false;
        private bool pp_del_flag = false;

        #endregion

        public GUI(StreamWriter streamWriter, FormDataSetting formDataSetting)
        {
            InitializeComponent();

            RECEIVED_DATA.WEAPON_TILT = 0;

            TB_RCWS_AZIMUTH.ForeColor = Color.Black;
            TB_RCWS_AZIMUTH.BackColor = Color.White;
            TB_RCWS_AZIMUTH.Text = "No Data";

            TB_WEAPON_ELEVATION.ForeColor = Color.Black;
            TB_WEAPON_ELEVATION.BackColor = Color.White;
            TB_WEAPON_ELEVATION.Text = "No Data";

            TB_OPTICAL_ELEVATION.ForeColor = Color.Black;
            TB_OPTICAL_ELEVATION.BackColor = Color.White;
            TB_OPTICAL_ELEVATION.Text = "No Data";

            TB_SENTRY_AZIMUTH.ForeColor = Color.Black;
            TB_SENTRY_AZIMUTH.BackColor = Color.White;
            TB_SENTRY_AZIMUTH.Text = "No Data";

            TB_SENTRY_ELEVATION.ForeColor = Color.Black;
            TB_SENTRY_ELEVATION.BackColor = Color.White;
            TB_SENTRY_ELEVATION.Text = "No Data";

            BTN_PERMISSION.ForeColor = Color.Black;
            BTN_PERMISSION.BackColor = Color.White;
            BTN_PERMISSION.Text = "No Data";

            TB_DISTANCE.ForeColor = Color.Black;
            TB_DISTANCE.BackColor = Color.White;
            TB_DISTANCE.Text = "No Data";

            BTN_TAKE_AIM.ForeColor = Color.Black;
            BTN_TAKE_AIM.BackColor = Color.White;
            BTN_TAKE_AIM.Text = "No Data";

            BTN_FIRE.ForeColor = Color.Black;
            BTN_FIRE.BackColor = Color.White;
            BTN_FIRE.Text = "No Data";

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

            PB_AZIMUTH.Paint += new PaintEventHandler(pictureBox_azimuth_Paint);

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

                if (buttons[8] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x100;
                }
                else if (buttons[8] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000100));
                }

                if (buttons[9] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x200;
                }
                else if (buttons[9] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000200));
                }

                if (buttons[10] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x400;

                    pp_flag = true;                    
                }
                else if (buttons[10] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000400));

                    pp_flag = false;
                }

                if (buttons[11] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x800;

                    pp_del_flag = true;
                }
                else if (buttons[11] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000800));
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

        private void pinpoint(float _pan, float _distance)      
        {
            
            
        }

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
            BTN_RCWS_CONNECT.BackColor = Color.Green;

            try
            {
                while (true)
                {
                    byte[] receivedData = new byte[Marshal.SizeOf(typeof(Packet.RECEIVED_PACKET))];
                    await NETWORK_STREAM.ReadAsync(receivedData, 0, receivedData.Length);
                    RTB_RECEIVED_DISPLAY.Invoke((MethodInvoker)delegate { RTB_RECEIVED_DISPLAY.AppendText(receivedData + "\r\n"); });

                    RECEIVED_DATA = TcpReturn.BytesToStruct<Packet.RECEIVED_PACKET>(receivedData);

                    ReceiveDisplay($"OpticalTilt: {RECEIVED_DATA.OPTICAL_TILT}, BodyTilt: {RECEIVED_DATA.WEAPON_TILT}" +
                        $", BodyPan: {RECEIVED_DATA.BODY_PAN}, Sentry Azimuth: {RECEIVED_DATA.SENTRY_AZIMUTH}, Sentry Elevation: {RECEIVED_DATA.SENTRY_ELEVATION}"+
                        $", Permission: {RECEIVED_DATA.PERMISSION}, Take Aim: {RECEIVED_DATA.TAKE_AIM}, Fire: {RECEIVED_DATA.FIRE}, Mode: {RECEIVED_DATA.MODE}");

                    if(pp_del_flag == true)
                    {
                        DRAW.DeleteAllPinPoints();

                        pp_del_flag = false;
                    }

                    if(pp_flag == true)
                    {
                        float centerX = PB_AZIMUTH.Width / 2;
                        float centerY = PB_AZIMUTH.Height / 3 * 2;
                        float radians = -RECEIVED_DATA.BODY_PAN * (float)(Math.PI / 180) + (float)(Math.PI / 2);
                        float x = centerX + RECEIVED_DATA.DISTANCE / 5 * (float)Math.Cos(radians);
                        float y = centerY - RECEIVED_DATA.DISTANCE / 5 * (float)Math.Sin(radians);
                        float radius = 10;
                                                                         
                        DRAW.AddPinPoint(new Point((int)x, (int)y), radius);
                                              
                        pp_flag = false;
                    }

                    PB_AZIMUTH.Invalidate();
                    PB_AZIMUTH.Refresh();

                    TB_RCWS_AZIMUTH.Text = RECEIVED_DATA.BODY_PAN.ToString();
                    TB_WEAPON_ELEVATION.Text = RECEIVED_DATA.WEAPON_TILT.ToString();
                    TB_OPTICAL_ELEVATION.Text = RECEIVED_DATA.OPTICAL_TILT.ToString();
                    TB_SENTRY_AZIMUTH.Text = RECEIVED_DATA.SENTRY_AZIMUTH.ToString();
                    TB_SENTRY_ELEVATION.Text = RECEIVED_DATA.SENTRY_ELEVATION.ToString();


                    // PERMISSION | 0: 상황실 1: 요청 2: 초소
                    // MODE | 0: 수동 1: auto scan 2: 레이저 트래킹 3: 사람 추적
                    if (RECEIVED_DATA.PERMISSION == 0)
                    {
                        BTN_PERMISSION.ForeColor = Color.White;
                        BTN_PERMISSION.BackColor = Color.Green;                        
                        
                        if(RECEIVED_DATA.MODE == 0)
                        {
                            BTN_PERMISSION.Text = "Controlable";
                        }

                        else if (RECEIVED_DATA.MODE == 1)
                        {
                            BTN_PERMISSION.Text = "Auto Scan Mode";
                        }

                        else if (RECEIVED_DATA.MODE == 3)
                        {
                            BTN_PERMISSION.Text = "Human Tracking Mode";
                        }

                        else
                        {
                            BTN_PERMISSION.ForeColor = Color.Black;
                            BTN_PERMISSION.BackColor = Color.White;
                            BTN_PERMISSION.Text = "Err";
                        }
                    }

                    else if (RECEIVED_DATA.PERMISSION == 1)
                    {
                        BTN_PERMISSION.ForeColor = Color.White;
                        BTN_PERMISSION.BackColor = Color.Yellow;
                        BTN_PERMISSION.Text = "Requesting...";
                    }

                    else if (RECEIVED_DATA.PERMISSION == 2)
                    {
                        BTN_PERMISSION.ForeColor = Color.White;
                        BTN_PERMISSION.BackColor = Color.Blue;

                        if (RECEIVED_DATA.MODE == 2)
                        {
                            BTN_PERMISSION.Text = "Laser Tracking Mode";
                        }                        
                    }

                    else
                    {
                        BTN_PERMISSION.ForeColor = Color.Black;
                        BTN_PERMISSION.BackColor = Color.White;
                        BTN_PERMISSION.Text = "Err";
                    }

                    TB_DISTANCE.Text = RECEIVED_DATA.DISTANCE.ToString();

                    // TAKE_AIM | 0: 초록, 수동 1: 조준중 2: 초록, 오토
                    if (RECEIVED_DATA.TAKE_AIM == 0)
                    {
                        BTN_TAKE_AIM.ForeColor = Color.White;
                        BTN_TAKE_AIM.BackColor = Color.Green;
                        BTN_TAKE_AIM.Text = "Manual Aiming";
                    }

                    else if (RECEIVED_DATA.TAKE_AIM == 1)
                    {
                        BTN_TAKE_AIM.ForeColor = Color.White;
                        BTN_TAKE_AIM.BackColor = Color.Red;
                        BTN_TAKE_AIM.Text = "Aiming Complete";
                    }

                    else if (RECEIVED_DATA.TAKE_AIM == 2)
                    {
                        BTN_TAKE_AIM.ForeColor = Color.White;
                        BTN_TAKE_AIM.BackColor = Color.Green;
                        BTN_TAKE_AIM.Text = "Automatic Aiming";
                    }

                    else
                    {
                        BTN_TAKE_AIM.ForeColor = Color.Black;
                        BTN_TAKE_AIM.BackColor = Color.White;
                        BTN_TAKE_AIM.Text = "Err";
                    }

                    // FIRE | 
                    if (RECEIVED_DATA.FIRE == 0 || RECEIVED_DATA.FIRE == 2)
                    {
                        BTN_FIRE.ForeColor = Color.White;
                        BTN_FIRE.BackColor = Color.Green;
                        BTN_FIRE.Text = "Not Fired";
                    }

                    else if (RECEIVED_DATA.FIRE == 1)
                    {
                        BTN_FIRE.ForeColor = Color.White;
                        BTN_FIRE.BackColor = Color.Red;
                        BTN_FIRE.Text = "Fire";
                    }

                    else
                    {
                        BTN_FIRE.ForeColor = Color.Black;
                        BTN_FIRE.BackColor = Color.White;
                        BTN_FIRE.Text = "Err";
                    }

                    /*
                    string dataToShow = $"OpticalTilt: {receivedStruct.OpticalTilt}, OpticalPan: {receivedStruct.OpticalPan}, BodyTilt: {receivedStruct.BodyTilt}" +
                        $", BodyPan: {receivedStruct.BodyPan}";                    
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
                BTN_CAMERA_CONNECT.BackColor = Color.Red;
                BTN_CAMERA_CONNECT.ForeColor = Color.White;
                
                SendDisplay("Send Data: " + ex.Message);
            }
            BTN_CAMERA_CONNECT.BackColor = Color.Green;
            BTN_CAMERA_CONNECT.ForeColor = Color.White;
            BTN_CAMERA_CONNECT.Enabled = false;
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
                        PBL_VIDEO.Image = receivedImage;
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

        private async void PBI_Video_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SEND_DATA.C_X = (short)(e.X);
                SEND_DATA.C_Y = (short)(e.Y);
                SEND_DATA.Button = (SEND_DATA.Button | 0x00001000);
                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00002000));
                byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                await STREAM_WRITER.BaseStream.FlushAsync();
                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00003000));
            }

            if (e.Button == MouseButtons.Right)
            {
                SEND_DATA.Button = (SEND_DATA.Button | 0x00002000);
                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00001000));
                byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                await STREAM_WRITER.BaseStream.FlushAsync();
                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00003000));
            }
        }
        #endregion

        #region AZEL GUI

        private void pictureBox_azimuth_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            int centerX = PB_AZIMUTH.Width / 2;
            int centerY = PB_AZIMUTH.Height / 3 * 2;

            /* */
            Pen redPen = new Pen(Color.Red, 8);
            g.DrawLine(redPen, new Point(centerX - 10, centerY - 10), new Point(centerX + 10, centerY + 10));
            g.DrawLine(redPen, new Point(centerX + 10, centerY - 10), new Point(centerX - 10, centerY + 10));
            redPen.Dispose();
            /* */

            /* */
            int lineLength = centerX * 2;
            /* */

            /* Body Pan */
            double radianAngleRCWS = RECEIVED_DATA.BODY_PAN * Math.PI / 180.0;
            int endXRCWS = centerX + (int)(lineLength * Math.Sin(radianAngleRCWS));
            int endYRCWS = centerY - (int)(lineLength * Math.Cos(radianAngleRCWS));
            g.DrawLine(Pens.Red, new Point(centerX, centerY), new Point(endXRCWS, endYRCWS));
            /* */

            DRAW.Drawing(e.Graphics);

            /* Pin Point */

            /* */
        }

        Point clickLocation;
        private void pictureBox_azimuth_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                clickLocation = e.Location;
                contextMenuStrip1.Show(PB_AZIMUTH, clickLocation);

                Bitmap bmp;
                if (PB_AZIMUTH.Image == null) bmp = new Bitmap(PB_AZIMUTH.Width, PB_AZIMUTH.Height);
                else bmp = new Bitmap(PB_AZIMUTH.Image);

                PB_AZIMUTH.Image = bmp;
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
            //DRAW.AddPinPoint(lastposition);
            PB_AZIMUTH.Invalidate();
        }

        private void deletePinPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DRAW.DeletePinPoint(lastposition);
            PB_AZIMUTH.Invalidate();
        }

        void DrawImage(string path)
        {
            using (Graphics g = Graphics.FromImage(PB_AZIMUTH.Image))
            {
                try
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                    {
                        Point clickLocation = new Point(PB_AZIMUTH.Width / 2, PB_AZIMUTH.Height / 2);
                        g.DrawImage(image, clickLocation.X - image.Width / 2, clickLocation.Y - image.Height / 2);
                        PB_AZIMUTH.Refresh();
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
            ;
            Application.Exit();
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            //SEND_DATA.Button = (SEND_DATA.Button | 0x00000000);        
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

        int CNT = 0;
        private void ALARM_Tick(object sender, EventArgs e)
        {

        }

        private void GUI_Load(object sender, EventArgs e)
        {           
            TIM_ALARM.Interval = 500;
            //TIM_ALARM.Tick


        }

        private void PBI_VIDEO_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            Pen redPen = new Pen(Color.Red, 3);

            /* 삼각형 */
            float At1_X = define.VIDEO_WIDTH / 2;
            float At1_Y =define.VIDEO_HEIGHT / 12 + 40;

            float At2_X = define.VIDEO_WIDTH / 2 - 10;
            float At2_Y = define.VIDEO_HEIGHT / 12 + 50;

            float At3_X = define.VIDEO_WIDTH / 2 + 10;
            float At3_Y = define.VIDEO_HEIGHT / 12 + 50;

            g.DrawLine(redPen, At1_X, At1_Y, At2_X, At2_Y);
            g.DrawLine(redPen, At2_X, At2_Y, At3_X, At3_Y);
            g.DrawLine(redPen, At3_X, At3_Y, At1_X, At1_Y);

            int Et1_X = define.VIDEO_WIDTH / 24 * 21 - 10;
            int Et1_Y = define.VIDEO_HEIGHT / 2;

            int Et2_X = define.VIDEO_WIDTH / 24 * 21 - 20;
            int Et2_Y = define.VIDEO_HEIGHT / 2 + 10;

            int Et3_X = define.VIDEO_WIDTH / 24 * 21 - 20;
            int Et3_Y = define.VIDEO_HEIGHT / 2 - 10;

            g.DrawLine(redPen, new Point(Et1_X, Et1_Y), new Point(Et2_X, Et2_Y));
            g.DrawLine(redPen, new Point(Et2_X, Et2_Y), new Point(Et3_X, Et3_Y));
            g.DrawLine(redPen, new Point(Et3_X, Et3_Y), new Point(Et1_X, Et1_Y));
            /* */

            /* 방위각 */
            // # 막대기
            float lineA_X = define.VIDEO_WIDTH / 2;
            float lineA_Y = define.VIDEO_HEIGHT / 12 + 15;

            int spacingA = 10;
            int lineLength = 20;

            for (int i = -20; i <= 20; i++)
            {
                float line_X = lineA_X + (i * spacingA) + (-RECEIVED_DATA.BODY_PAN) * 2;
                //g.DrawLine(redPen, new Point(line_X, lineA_Y), new Point(line_X, lineA_Y + lineLength));          
                g.DrawLine(redPen, line_X, lineA_Y, line_X, lineA_Y + lineLength);
            }

            // # 숫자
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Red;

            float A_Y = lineA_Y - 20;
            float A_100X = lineA_X - 200 - 20 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_80X = lineA_X - 180 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_60X = lineA_X - 140 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_40X = lineA_X - 100 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_20X = lineA_X - 60 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_0X = lineA_X - 7 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A20X= lineA_X + 40 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A40X = lineA_X + 80 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A60X = lineA_X + 120 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A80X = lineA_X + 160 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A100X = lineA_X + 200 - 15 + (-RECEIVED_DATA.BODY_PAN) * 2;

            g.DrawString((-100).ToString(), font, brush, A_100X, A_Y);
            g.DrawString((-80).ToString(), font, brush, A_80X, A_Y);
            g.DrawString((-60).ToString(), font, brush, A_60X, A_Y);
            g.DrawString((-40).ToString(), font, brush, A_40X, A_Y);
            g.DrawString((-20).ToString(), font, brush, A_20X, A_Y);
            g.DrawString((0).ToString(), font, brush, A_0X, A_Y);
            g.DrawString((20).ToString(), font, brush, A20X, A_Y);
            g.DrawString((40).ToString(), font, brush, A40X, A_Y);
            g.DrawString((60).ToString(), font, brush, A60X, A_Y);
            g.DrawString((80).ToString(), font, brush, A80X, A_Y);
            g.DrawString((100).ToString(), font, brush, A100X, A_Y);



            //float startX = lineA_X - 220;
            //float interval = 40;            

            //for (int i = -100; i <= 100; i += 20)
            //{
            //    g.DrawString(i.ToString(), font, brush, startX, A_Y);
            //    startX += interval;
            //}

            /* 고각 */
            // # 막대기
            float lineE_X = define.VIDEO_WIDTH / 24 * 21;
            float lineE_Y = define.VIDEO_HEIGHT / 2;

            int spacingE = 10;

            for (int i = -6; i <= 6; i++)
            {
                float line_Y = lineE_Y + (i * spacingE) + (RECEIVED_DATA.WEAPON_TILT) * 2;
                g.DrawLine(redPen, lineE_X, line_Y, lineE_X + lineLength, line_Y);
            }

            // # 숫자
            float E_X = lineE_X + 20;
            float E_30Y = lineE_Y + 30 + 20 + (RECEIVED_DATA.WEAPON_TILT) * 2;
            float E_20Y = lineE_Y + 20 + 10 + (RECEIVED_DATA.WEAPON_TILT) * 2;
            float E_10Y = lineE_Y + 10 + (RECEIVED_DATA.WEAPON_TILT) * 2;
            float E_0Y = lineE_Y - 10 + (RECEIVED_DATA.WEAPON_TILT) * 2;
            float E10Y = lineE_Y - 10 - 20 + (RECEIVED_DATA.WEAPON_TILT) * 2;
            float E20Y = lineE_Y - 20 - 30 + (RECEIVED_DATA.WEAPON_TILT) * 2;
            float E30Y = lineE_Y - 30 - 40 + (RECEIVED_DATA.WEAPON_TILT) * 2;

            g.DrawString((-30).ToString(), font, brush, E_X, E_30Y);
            g.DrawString((-20).ToString(), font, brush, E_X, E_20Y);
            g.DrawString((-10).ToString(), font, brush, E_X, E_10Y);
            g.DrawString((0).ToString(), font, brush, E_X, E_0Y);
            g.DrawString((10).ToString(), font, brush, E_X, E10Y);
            g.DrawString((20).ToString(), font, brush, E_X, E20Y);
            g.DrawString((30).ToString(), font, brush, E_X, E30Y);

            redPen.Dispose();
            font.Dispose();
        }
    }
}