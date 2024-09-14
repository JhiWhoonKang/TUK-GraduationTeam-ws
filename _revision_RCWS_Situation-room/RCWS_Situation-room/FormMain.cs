using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace RCWS_Situation_room
{
    public partial class FormMain : Form
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

        /* TCP */
        private TcpClient TCP_CLIENT;
        private StreamWriter STREAM_WRITER;
        private StreamReader STREAM_READER;
        private NetworkStream NETWORK_STREAM;
        //private Thread receiveThread;
        /* */

        /* motion control */
        private HashSet<Keys> PRESSEDKEYS = new HashSet<Keys>();
        int ROTATION_SPEED;
        /* */

        /* Packet */
        Packet.SEND_PACKET SEND_DATA;
        Packet.RECEIVED_PACKET RECEIVED_DATA;
        /* */

        /* AZEL Class */
        private Draw DRAW = new Draw();
        private bool pp_flag = false;
        private bool pp_del_flag = false;
        Point clickLocation;
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

        private bool pwr_flag = false;
        double HSB_BODY_VEL_VALUE;
        double HSB_OPTICAL_VEL_VALUE;

        Point startPoint = Point.Empty;
        Point endPoint = Point.Empty;

        private bool RCWS_CONNECT_FLAG = false;
        private bool CAMERA_CONNECT_FLAG = false;
        #endregion

        public FormMain(StreamWriter streamWriter, FormDataSetting formDataSetting)
        {
            InitializeComponent();

            RTB_RECEIVED_DISPLAY.ReadOnly = true;
            RTB_SEND_DISPLAY.ReadOnly = true;

            RECEIVED_DATA.WEAPON_TILT = 0;

            TB_DISTANCE.ReadOnly = true;
            TB_OPTICAL_ELEVATION.ReadOnly = true;
            TB_RCWS_AZIMUTH.ReadOnly = true;
            TB_WEAPON_ELEVATION.ReadOnly = true;
            TB_SENTRY_AZIMUTH.ReadOnly = true;
            TB_SENTRY_ELEVATION.ReadOnly = true;

            BTN_POWER.BackColor = Color.Red;
            BTN_POWER.ForeColor = Color.White;

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

            HSB_BODY_VEL_VALUE = 1.0;
            HSB_OPTICAL_VEL_VALUE = 1.0;

            RTB_RECEIVED_DISPLAY.Visible = false;
            RTB_SEND_DISPLAY.Visible = false;

            TB_MAGNIFICATION.Text = "No Data";

            this.Focus();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            BTN_SETTING.Visible = false;
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
                ErrorHandleDisplay("Joy Error" + ex.Message + "\r\n");
            }
        }

        float ax_X;
        sbyte optical_magnification = 1;
        bool optical_magnification_flag_zi = false;
        bool optical_magnification_flag_zo = false;
        private void Joy_Run()
        {
            var joystick = new Joystick(DIRECT_INPUT, JOYSTICK_GUID);

            joystick.Acquire();

            SEND_DATA.Button = 0xfc100000;

            int pre_1 = SEND_DATA.BODY_PAN;
            int pre_2 = SEND_DATA.OPTICAL_TILT;
            uint pre_3 = SEND_DATA.Button;

            while (true)
            {
                joystick.Poll();
                var state = joystick.GetCurrentState();
                var buttons = state.Buttons;

                ax_X = ((float)(state.X - 32511) / (float)(33024));
                if (ax_X < 0.2025 && ax_X > -0.2025)
                {
                    SEND_DATA.BODY_PAN = 0;
                }
                else
                {
                    if (ax_X >= 0.2)
                    {
                        //SEND_DATA.BodyPan = (int)(ax_X * 400 - 80);
                        //SEND_DATA.BODY_PAN = (int)((ax_X * 625 - 125) * HSB_BODY_VEL_VALUE);
                        SEND_DATA.BODY_PAN = (int)((ax_X * 625 - 125) * panSpeed);
                    }
                    else if (ax_X <= -0.2)
                    {
                        //SEND_DATA.BodyPan = (int)(ax_X * 400 + 80);
                        //SEND_DATA.BODY_PAN = (int)((ax_X * 625 + 125) * HSB_BODY_VEL_VALUE);
                        SEND_DATA.BODY_PAN = (int)((ax_X * 625 + 125) * panSpeed);
                    }
                }

                float ax_Y = ((float)(state.Y - 32511) / (float)(33024));
                if (ax_Y < 0.22 && ax_Y > -0.22)
                {
                    SEND_DATA.OPTICAL_TILT = 0;
                }
                else
                {
                    if (ax_Y >= 0.22)
                    {
                        //SEND_DATA.BodyTilt = (int)(ax_Y * 100 - 20);
                        //SEND_DATA.OPTICAL_TILT = (int)((ax_Y * 100 - 20) * HSB_OPTICAL_VEL_VALUE);
                        SEND_DATA.OPTICAL_TILT = (int)((ax_Y * 100 - 20) * tiltSpeed);
                    }
                    else if (ax_Y <= -0.22)
                    {
                        //SEND_DATA.BodyTilt = (int)(ax_Y * 100 + 20);
                        //SEND_DATA.OPTICAL_TILT = (int)((ax_Y * 100 + 20) * HSB_OPTICAL_VEL_VALUE);
                        SEND_DATA.OPTICAL_TILT = (int)((ax_Y * 100 + 20) * tiltSpeed);
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
                }
                else if (buttons[2] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000004));
                }

                if (buttons[3] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x08;
                }
                else if (buttons[3] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000008));
                }

                if (buttons[4] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x10;
                }
                else if (buttons[4] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000010));
                }

                if (buttons[5] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x20;
                }
                else if (buttons[5] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000020));
                }

                if (buttons[6] == true && !optical_magnification_flag_zo) // 축소
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x20000;
                    
                    optical_magnification_flag_zo = true;

                    if (optical_magnification <= define.MIN_MAGNIFICATION)
                    {
                        optical_magnification = define.MIN_MAGNIFICATION;
                    }

                    else
                    {
                        optical_magnification -= 1;
                    }

                    if(optical_magnification ==1 || optical_magnification==2)
                    {
                        UpdateUI(() => ReceiveDisplay(TB_MAGNIFICATION.Text="4.5배율"));
                    }

                    else if(optical_magnification==3)
                    {
                        UpdateUI(() => ReceiveDisplay(TB_MAGNIFICATION.Text = "5.7배율"));
                    }

                    else
                    {
                        UpdateUI(() => ReceiveDisplay(TB_MAGNIFICATION.Text = "8.2배율"));
                    }
                }
                else if (buttons[6] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00020000));
                    optical_magnification_flag_zo = false;
                }

                if (buttons[7] == true)
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x80;                    
                }
                else if (buttons[7] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000080));
                }

                if (buttons[8] == true && !optical_magnification_flag_zi) // 확대
                {
                    SEND_DATA.Button = SEND_DATA.Button | 0x40000;
                    
                    optical_magnification_flag_zi = true;

                    if (optical_magnification >= define.MAX_MAGNIFICATION)
                    {
                        optical_magnification = define.MAX_MAGNIFICATION;
                    }

                    else
                    {
                        optical_magnification += 1;
                    }

                    if (optical_magnification == 1 || optical_magnification == 2)
                    {
                        UpdateUI(() => ReceiveDisplay(TB_MAGNIFICATION.Text = "4.5배율"));
                    }

                    else if (optical_magnification == 3)
                    {
                        UpdateUI(() => ReceiveDisplay(TB_MAGNIFICATION.Text = "5.7배율"));
                    }

                    else
                    {
                        UpdateUI(() => ReceiveDisplay(TB_MAGNIFICATION.Text = "8.2배율"));
                    }
                }
                else if (buttons[8] == false)
                {
                    SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00040000));
                    optical_magnification_flag_zi = false;
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

                if (pre_1 != SEND_DATA.BODY_PAN || pre_2 != SEND_DATA.OPTICAL_TILT || pre_3 != SEND_DATA.Button)
                {
                    byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                    STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                    STREAM_WRITER.BaseStream.FlushAsync();

                    pre_1 = SEND_DATA.BODY_PAN;
                    pre_2 = SEND_DATA.OPTICAL_TILT;
                    pre_3 = SEND_DATA.Button;
                }

                Thread.Sleep(5);
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
                ErrorHandleDisplay("Error in MapPictureBox_MouseMove: " + ex.Message + "\r\n");
                //MessageBox.Show("Error in MapPictureBox_MouseMove: " + ex.Message);
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

        private HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private readonly SemaphoreSlim keySemaphore = new SemaphoreSlim(1, 1);
        double panSpeed = 1;
        double tiltSpeed = 1;
        double real_panSpeed = 0.0;
        double real_tiltSpeed = 0.0;
        int stop_panSpeed = 0;
        int stop_tiltSpeed = 0;
        bool control_flag = false;

        private async void GUI_KeyDown(object sender, KeyEventArgs e)
        {
            await keySemaphore.WaitAsync();

            try
            {
                if (pressedKeys.Add(e.KeyCode))
                {
                    await HandleKeyPress(e.KeyCode, true);
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
                if (pressedKeys.Remove(e.KeyCode))
                {
                    await HandleKeyPress(e.KeyCode, false);
                }
            }
            finally
            {
                keySemaphore.Release();
            }
        }

        private async Task HandleKeyPress(Keys keyCode, bool isKeyDown)
        {
            if (RCWS_CONNECT_FLAG)
            {
                if (isKeyDown)
                {
                    if (keyCode == Keys.A || keyCode == Keys.Left)
                    {
                        real_panSpeed = -Math.Abs(panSpeed * 500);
                        SEND_DATA.BODY_PAN = (int)(real_panSpeed);
                    }
                    if (keyCode == Keys.D || keyCode == Keys.Right)
                    {
                        real_panSpeed = Math.Abs(panSpeed * 500);
                        SEND_DATA.BODY_PAN = (int)(real_panSpeed);
                    }
                    if (keyCode == Keys.W || keyCode == Keys.Up)
                    {
                        real_tiltSpeed = Math.Abs(tiltSpeed * 80);
                        SEND_DATA.OPTICAL_TILT = (int)(real_tiltSpeed);
                    }
                    if (keyCode == Keys.S || keyCode == Keys.Down)
                    {
                        real_tiltSpeed = -Math.Abs(tiltSpeed * 80);
                        SEND_DATA.OPTICAL_TILT = (int)(real_tiltSpeed);
                    }
                    if (keyCode == Keys.Z)
                    {
                        real_tiltSpeed = Math.Abs(tiltSpeed) - 0.1;
                        real_panSpeed = Math.Abs(panSpeed) - 0.1;
                        if (tiltSpeed <= 0.1)
                        {
                            tiltSpeed = 0.1;
                        }
                        if (panSpeed <= 0.1)
                        {
                            panSpeed = 0.1;
                        }
                    }
                    if (keyCode == Keys.X)
                    {
                        real_tiltSpeed = Math.Abs(tiltSpeed) + 0.1;
                        real_panSpeed = Math.Abs(panSpeed) + 0.1;
                    }

                    if (keyCode == Keys.D1)
                    {
                        //HSB_BODY_VEL_VALUE = 5;
                        //HSB_OPTICAL_VEL_VALUE = 2;
                        panSpeed = 0.04;
                        tiltSpeed = 0.1;                        
                    }

                    if (keyCode == Keys.D2)
                    {
                        //HSB_BODY_VEL_VALUE = 50;
                        //HSB_OPTICAL_VEL_VALUE = 20;
                        panSpeed = 0.3;
                        tiltSpeed = 0.3;
                    }

                    if (keyCode == Keys.D3)
                    {
                        //HSB_BODY_VEL_VALUE = 100;
                        //HSB_OPTICAL_VEL_VALUE = 30;
                        panSpeed = 0.5;
                        tiltSpeed = 0.5;
                    }

                    if (keyCode == Keys.D4)
                    {
                        //HSB_BODY_VEL_VALUE = 200;
                        //HSB_OPTICAL_VEL_VALUE = 40;
                        panSpeed = 1;
                        tiltSpeed = 1;
                    }

                    if (keyCode == Keys.D5)
                    {
                        //HSB_BODY_VEL_VALUE = 50;
                        //HSB_OPTICAL_VEL_VALUE = 10;
                        panSpeed = 0.5;
                        tiltSpeed = 0.3;
                    }
                    //LB_BODY_ROTATION_VEL.Text = panSpeed.ToString();
                    //LB_OPTICAL_ROTATION_VEL.Text = tiltSpeed.ToString();                    

                    byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                    await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                    await STREAM_WRITER.BaseStream.FlushAsync();
                }

                else
                {
                    if (keyCode == Keys.A || keyCode == Keys.D)
                    {
                        SEND_DATA.BODY_PAN = stop_panSpeed;
                    }
                    if (keyCode == Keys.W || keyCode == Keys.S)
                    {
                        SEND_DATA.OPTICAL_TILT = stop_tiltSpeed;
                    }

                    byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                    await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                    await STREAM_WRITER.BaseStream.FlushAsync();
                }                               
            }            
        }
        #endregion

        #region TCP
        private async void BTN_RCWS_CONNECT_Click(object sender, EventArgs e)
        {
            await Task.Run(() => Try_TCP_Connect());
        }

        private async Task Try_TCP_Connect()
        {
            TCP_CLIENT = new TcpClient();

            try
            {
                UpdateUI(() => SendDisplay("Connecting..."));
                await TCP_CLIENT.ConnectAsync(define.SERVER_IP, define.TCPPORT);

                RCWS_CONNECT_FLAG = true;

                NETWORK_STREAM = TCP_CLIENT.GetStream();
                STREAM_READER = new StreamReader(NETWORK_STREAM);
                STREAM_WRITER = new StreamWriter(NETWORK_STREAM);
                STREAM_WRITER.AutoFlush = true;

                InitializeJoystick();
            }
            catch (Exception ex)
            {
                ErrorHandleDisplay(ex.Message + "\r\n");
                return;
            }

            UpdateUI(() =>
            {
                ReceiveDisplay("Server Connected");
                TB_MAGNIFICATION.Text = "4.5배율";
                BTN_RCWS_CONNECT.BackColor = Color.Green;
                BTN_RCWS_CONNECT.ForeColor = Color.White;
            });

            await ReceiveDataAsync();
        }

        private void UpdateUI(Action action)
        {
            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        private async Task ReceiveDataAsync()
        {
            try
            {
                while (true)
                {
                    byte[] receivedData = new byte[Marshal.SizeOf(typeof(Packet.RECEIVED_PACKET))];
                    await NETWORK_STREAM.ReadAsync(receivedData, 0, receivedData.Length);
                    
                    UpdateUI(() => RTB_RECEIVED_DISPLAY.AppendText(BitConverter.ToString(receivedData) + "\r\n"));

                    RECEIVED_DATA = TcpReturn.BytesToStruct<Packet.RECEIVED_PACKET>(receivedData);
                    ProcessReceivedData(RECEIVED_DATA);
                }
            }
            catch (Exception ex)
            {
                ErrorHandleDisplay(ex.Message + "\r\n");
            }
        }

        private void ProcessReceivedData(Packet.RECEIVED_PACKET receivedData)
        {
            ReceiveDisplay($"OpticalTilt: {receivedData.OPTICAL_TILT}, BodyTilt: {receivedData.WEAPON_TILT}" +
                $", BodyPan: {receivedData.BODY_PAN}, Sentry Azimuth: {receivedData.SENTRY_AZIMUTH}, Sentry Elevation: {receivedData.SENTRY_ELEVATION}" +
                $", Permission: {receivedData.PERMISSION}, Take Aim: {receivedData.TAKE_AIM}, Fire: {receivedData.FIRE}, Mode: {receivedData.MODE}");

            UpdateParam(receivedData);
            formDataSetting.Add_RX_ListView(receivedData);
        }
        
        private void UpdateParam(Packet.RECEIVED_PACKET receivedData)
        {
            UpdateUI(() =>
            {
                if (pp_del_flag)
                {
                    DRAW.DeleteAllPinPoints();
                    pp_del_flag = false;
                }

                if (pp_flag)
                {
                    float centerX = PB_AZIMUTH.Width / 2;
                    float centerY = PB_AZIMUTH.Height / 3 * 2;
                    float radians = -receivedData.BODY_PAN * (float)(Math.PI / 180) + (float)(Math.PI / 2);
                    float x = centerX + receivedData.DISTANCE / define.LENGTH_SCALE * (float)Math.Cos(radians);
                    float y = centerY - receivedData.DISTANCE / define.LENGTH_SCALE * (float)Math.Sin(radians);
                    float radius = 10;

                    DRAW.AddPinPoint(new Point((int)x, (int)y), radius);
                    pp_flag = false;
                }

                PB_AZIMUTH.Invalidate();
                PB_AZIMUTH.Refresh();

                TB_RCWS_AZIMUTH.Text = receivedData.BODY_PAN.ToString();                
                TB_WEAPON_ELEVATION.Text = receivedData.WEAPON_TILT.ToString();
                TB_OPTICAL_ELEVATION.Text = receivedData.OPTICAL_TILT.ToString();
                TB_SENTRY_AZIMUTH.Text = receivedData.SENTRY_AZIMUTH.ToString();
                TB_SENTRY_ELEVATION.Text = receivedData.SENTRY_ELEVATION.ToString();
                TB_DISTANCE.Text = receivedData.DISTANCE.ToString();

                LB_BODY_ROTATION_VEL.Text = SEND_DATA.BODY_PAN.ToString();
                LB_OPTICAL_ROTATION_VEL.Text = SEND_DATA.OPTICAL_TILT.ToString();

                // PERMISSION | 0: 상황실 1: 요청 2: 초소
                // MODE | 0: 수동 1: auto scan 2: 레이저 트래킹 3: 사람 추적
                if (RECEIVED_DATA.PERMISSION == 0)
                {
                    BTN_PERMISSION.ForeColor = Color.White;
                    BTN_PERMISSION.BackColor = Color.Green;

                    if (RECEIVED_DATA.MODE == 0)
                    {
                        BTN_PERMISSION.Text = "Controlable";
                    }

                    else if (RECEIVED_DATA.MODE == 1)
                    {
                        BTN_PERMISSION.Text = "Auto Scan Mode";
                    }

                    else if (RECEIVED_DATA.MODE == 3)
                    {
                        BTN_PERMISSION.Text = "Tracking Mode";
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
                    BTN_PERMISSION.ForeColor = Color.Black;
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
            }            
            );            
        }
        
        private void CB_AUTO_AIM_ENABLED_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_AUTO_AIM_ENABLED.Checked)
            {
                CB_AUTO_AIM_ENABLED.Text = "Activated";

                SEND_DATA.Button = SEND_DATA.Button | 0x40;
            }
            else
            {
                CB_AUTO_AIM_ENABLED.Text = "Deactivated";

                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00000040));
            }
        }

        private void CB_AUTO_TRACKING_ENABLED_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_AUTO_TRACKING_ENABLED.Checked)
            {
                CB_AUTO_TRACKING_ENABLED.Text = "Activated";

                SEND_DATA.Button = SEND_DATA.Button | 0x10000;
            }
            else
            {
                CB_AUTO_TRACKING_ENABLED.Text = "Deactivated";

                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00010000));
            }
        }

        private void HSB_BODY_VEL_Scroll(object sender, ScrollEventArgs e)
        {
            //HSB_BODY_VEL_VALUE = HSB_BODY_VEL.Value / 1000.0;
            //LB_BODY_ROTATION_VEL.Text = HSB_BODY_VEL_VALUE.ToString();
            panSpeed = HSB_BODY_VEL.Value / 1000.0;
            //LB_BODY_ROTATION_VEL.Text = HSB_BODY_VEL_VALUE.ToString();
        }

        private void HSB_OPTICAL_VEL_Scroll(object sender, ScrollEventArgs e)
        {
            //HSB_OPTICAL_VEL_VALUE= HSB_OPTICAL_VEL.Value / 50.0;
            //LB_OPTICAL_ROTATION_VEL.Text = HSB_OPTICAL_VEL_VALUE.ToString();
            tiltSpeed = HSB_OPTICAL_VEL.Value / 50.0;
            //LB_OPTICAL_ROTATION_VEL.Text = HSB_OPTICAL_VEL_VALUE.ToString();
        }

        private void PBL_VIDEO_MouseDown(object sender, MouseEventArgs e)
        {
            if(RCWS_CONNECT_FLAG)
            {
                if (e.Button == MouseButtons.Left)
                {
                    IS_DRAGGING = true;
                    startPoint = e.Location;
                    SEND_DATA.D_X1 = (short)(e.X);
                    SEND_DATA.D_Y1 = (short)(e.Y);
                }
            }            
        }

        private async void PBL_VIDEO_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if(RCWS_CONNECT_FLAG)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (IS_DRAGGING)
                        {
                            endPoint = e.Location;
                            SEND_DATA.D_X2 = (short)(e.X);
                            SEND_DATA.D_Y2 = (short)(e.Y);

                            SEND_DATA.Button = (SEND_DATA.Button | 0x00001000);
                            SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00002000));
                            byte[] commandBytes = TcpReturn.StructToBytes(SEND_DATA);
                            await STREAM_WRITER.BaseStream.WriteAsync(commandBytes, 0, commandBytes.Length);
                            await STREAM_WRITER.BaseStream.FlushAsync();
                            SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00003000));

                            IS_DRAGGING = false;
                            startPoint = Point.Empty;
                            endPoint = Point.Empty;
                            PBL_VIDEO.Invalidate();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error in Video Picture Box: " + ex.Message);
            }
        }

        private void PBL_VIDEO_MouseMove(object sender, MouseEventArgs e)
        {
            if(RCWS_CONNECT_FLAG)
            {
                if (IS_DRAGGING)
                {
                    endPoint = e.Location;
                    PBL_VIDEO.Invalidate();
                }
            }            
        }        
        #endregion

        #region UDP, Video

        private void BTN_CAMERA_CONNECT_Click(object sender, EventArgs e)
        {            
            Task.Run(() => UdpConnect());
        }

        private void UdpConnect()
        {
            MemoryStream ms = new MemoryStream();
            
            try
            {
                StartReceiving(define.SERVER_IP, define.UDPPORT);

                this.Invoke((MethodInvoker)delegate
                {
                    BTN_CAMERA_CONNECT.ForeColor = Color.White;
                    BTN_CAMERA_CONNECT.BackColor = Color.Green;
                });
            }

            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    BTN_CAMERA_CONNECT.BackColor = Color.Red;
                    BTN_CAMERA_CONNECT.ForeColor = Color.White;
                });

                ErrorHandleDisplay(ex.Message);
            }
            finally
            {
                ms.Dispose();
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

                        int newWidth = (int)(receivedImage.Width * define.VIDEO_SCALE);
                        int newHeight = (int)(receivedImage.Height * define.VIDEO_SCALE);

                        using (var resizedImage = new Bitmap(newWidth, newHeight))
                        {
                            using (var graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                graphics.DrawImage(receivedImage, 0, 0, newWidth, newHeight);
                            }

                            PBL_VIDEO.Image = (Image)resizedImage.Clone();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandleDisplay(ex.Message);
                }
            });
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
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
            }

            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ErrorHandleDisplay(ex.Message);
                });
            }

            finally
            {
                UDP_CLIENT.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            }
        }

        private async void PBI_Video_MouseClick(object sender, MouseEventArgs e)
        {
            if(RCWS_CONNECT_FLAG)
            {
                if (e.Button == MouseButtons.Left)
                {
                    SEND_DATA.C_X1 = (short)(e.X);
                    SEND_DATA.C_Y1 = (short)(e.Y);
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
        }

        private void PBI_VIDEO_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            Pen redPen = new Pen(Color.Red, 3);
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Red;

            /* 삼각형 */
            float At1_X = define.VIDEO_WIDTH / 2;
            float At1_Y = define.VIDEO_HEIGHT / 12 + 40;

            float At2_X = define.VIDEO_WIDTH / 2 - 10;
            float At2_Y = define.VIDEO_HEIGHT / 12 + 50;

            float At3_X = define.VIDEO_WIDTH / 2 + 10;
            float At3_Y = define.VIDEO_HEIGHT / 12 + 50;

            g.DrawLine(redPen, At1_X, At1_Y, At2_X, At2_Y);
            g.DrawLine(redPen, At2_X, At2_Y, At3_X, At3_Y);
            g.DrawLine(redPen, At3_X, At3_Y, At1_X, At1_Y);

            // 기총
            int Et1_X = define.VIDEO_WIDTH / 24 * 21 - 10;
            int Et1_Y = define.VIDEO_HEIGHT / 2;

            int Et2_X = define.VIDEO_WIDTH / 24 * 21 - 20;
            int Et2_Y = define.VIDEO_HEIGHT / 2 + 10;

            int Et3_X = define.VIDEO_WIDTH / 24 * 21 - 20;
            int Et3_Y = define.VIDEO_HEIGHT / 2 - 10;

            g.DrawLine(redPen, new Point(Et1_X, Et1_Y), new Point(Et2_X, Et2_Y));
            g.DrawLine(redPen, new Point(Et2_X, Et2_Y), new Point(Et3_X, Et3_Y));
            g.DrawLine(redPen, new Point(Et3_X, Et3_Y), new Point(Et1_X, Et1_Y));

            // 광학
            int O_Et1_X = define.VIDEO_WIDTH / 24 * 3 + 10;
            int O_Et1_Y = define.VIDEO_HEIGHT / 2;

            int O_Et2_X = define.VIDEO_WIDTH / 24 * 3 + 20;
            int O_Et2_Y = define.VIDEO_HEIGHT / 2 + 10;

            int O_Et3_X = define.VIDEO_WIDTH / 24 * 3 + 20;
            int O_Et3_Y = define.VIDEO_HEIGHT / 2 - 10;

            g.DrawLine(redPen, new Point(O_Et1_X, O_Et1_Y), new Point(O_Et2_X, O_Et2_Y));
            g.DrawLine(redPen, new Point(O_Et2_X, O_Et2_Y), new Point(O_Et3_X, O_Et3_Y));
            g.DrawLine(redPen, new Point(O_Et3_X, O_Et3_Y), new Point(O_Et1_X, O_Et1_Y));
            /* */

            /* 방위각 */
            // # 막대기
            float lineA_X = define.VIDEO_WIDTH / 2;
            float lineA_Y = define.VIDEO_HEIGHT / 12 + 15;

            int spacingA = 10;
            int lineLength = 20;

            for (int i = -36; i <= 36; i++)
            {
                float line_X = lineA_X + (i * spacingA) + (-RECEIVED_DATA.BODY_PAN) * 2;
                //g.DrawLine(redPen, new Point(line_X, lineA_Y), new Point(line_X, lineA_Y + lineLength));          
                g.DrawLine(redPen, line_X, lineA_Y, line_X, lineA_Y + lineLength);
            }            

            // # 숫자            
            float A_Y = lineA_Y - 20;
            
            float A_180X = lineA_X - 385 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_160X = lineA_X - 345 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_140X = lineA_X - 305 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_120X = lineA_X - 265 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_100X = lineA_X - 225 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_80X = lineA_X - 180 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_60X = lineA_X - 140 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_40X = lineA_X - 100 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A_20X = lineA_X - 60 + 5 + (-RECEIVED_DATA.BODY_PAN) * 2;

            float A_0X = lineA_X - 7 + (-RECEIVED_DATA.BODY_PAN) * 2;

            float A20X = lineA_X + 40 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A40X = lineA_X + 80 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A60X = lineA_X + 120 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A80X = lineA_X + 160 - 10 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A100X = lineA_X + 200 - 15 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A120X = lineA_X + 240 - 15 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A140X = lineA_X + 280 - 15 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A160X = lineA_X + 320 - 15 + (-RECEIVED_DATA.BODY_PAN) * 2;
            float A180X = lineA_X + 360 - 15 + (-RECEIVED_DATA.BODY_PAN) * 2;

            g.DrawString((-180).ToString(), font, brush, A_180X, A_Y);
            g.DrawString((-160).ToString(), font, brush, A_160X, A_Y);
            g.DrawString((-140).ToString(), font, brush, A_140X, A_Y);
            g.DrawString((-120).ToString(), font, brush, A_120X, A_Y); //
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
            g.DrawString((120).ToString(), font, brush, A120X, A_Y); //
            g.DrawString((140).ToString(), font, brush, A140X, A_Y);
            g.DrawString((160).ToString(), font, brush, A160X, A_Y);
            g.DrawString((180).ToString(), font, brush, A180X, A_Y);

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

            /* 영역 사각형 */
            if (IS_DRAGGING && startPoint != Point.Empty && endPoint != Point.Empty)
            {
                Rectangle rect = new Rectangle(
                    Math.Min(startPoint.X, endPoint.X),
                    Math.Min(startPoint.Y, endPoint.Y),
                    Math.Abs(startPoint.X - endPoint.X),
                    Math.Abs(startPoint.Y - endPoint.Y));

                e.Graphics.DrawRectangle(Pens.Red, rect);
            }

            /* 광학 고각 */
            // # 막대기
            float O_lineE_X = define.VIDEO_WIDTH / 24 * 3 - 20;
            float O_lineE_Y = define.VIDEO_HEIGHT / 2;

            int O_spacingE = 10;

            for (int i = -6; i <= 6; i++)
            {
                float line_Y = O_lineE_Y + (i * O_spacingE) + (RECEIVED_DATA.OPTICAL_TILT) * 2;
                g.DrawLine(redPen, O_lineE_X, line_Y, O_lineE_X + lineLength, line_Y);
            }

            // # 숫자
            float O_E_X = O_lineE_X - 30;
            float O_E_30Y = O_lineE_Y + 30 + 20 + (RECEIVED_DATA.OPTICAL_TILT) * 2;
            float O_E_20Y = O_lineE_Y + 20 + 10 + (RECEIVED_DATA.OPTICAL_TILT) * 2;5    e                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            float O_E_10Y = O_lineE_Y + 10 + (RECEIVED_DATA.OPTICAL_TILT) * 2;

            float O_E_0Y = O_lineE_Y - 10 + (RECEIVED_DATA.OPTICAL_TILT) * 2;

            float O_E10Y = O_lineE_Y - 10 - 20 + (RECEIVED_DATA.OPTICAL_TILT) * 2;
            float O_E20Y = O_lineE_Y - 20 - 30 + (RECEIVED_DATA.OPTICAL_TILT) * 2;
            float O_E30Y = O_lineE_Y - 30 - 40 + (RECEIVED_DATA.OPTICAL_TILT) * 2;

            g.DrawString((-30).ToString(), font, brush, O_E_X, O_E_30Y);
            g.DrawString((-20).ToString(), font, brush, O_E_X, O_E_20Y);
            g.DrawString((-10).ToString(), font, brush, O_E_X, O_E_10Y);

            g.DrawString((0).ToString(), font, brush, O_E_X, O_E_0Y);

            g.DrawString((10).ToString(), font, brush, O_E_X, O_E10Y);
            g.DrawString((20).ToString(), font, brush, O_E_X, O_E20Y);
            g.DrawString((30).ToString(), font, brush, O_E_X, O_E30Y);


            // #####
            if (RECEIVED_DATA.BODY_PAN >= 10)
            {
                if (RECEIVED_DATA.BODY_PAN >= 15)
                {
                    g.DrawString(("! Max Azimuth !\nTurn Left").ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 5);
                }
                else
                {
                    g.DrawString(("! Azimuth Warning !\nTurn Left").ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 5);
                }
            }

            if (RECEIVED_DATA.BODY_PAN <= -175)
            {
                if (RECEIVED_DATA.BODY_PAN <= -180)
                {
                    g.DrawString((("! Max Azimuth !\nTurn Right")).ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 5);
                }
                else
                {
                    g.DrawString((("! Azimuth Warning !\nTurn Right")).ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 5);
                }
            }
            // #####
            if (RECEIVED_DATA.OPTICAL_TILT >= 25)
            {
                if (RECEIVED_DATA.OPTICAL_TILT >= 30)
                {
                    g.DrawString(("! Max Optical Elevation !\nGo Down").ToString(), font, brush, PBL_VIDEO.Width / 11 * 1, PBL_VIDEO.Height / 12 * 5);
                }
                else
                {
                    g.DrawString(("! Optical Elevation Warning !\nGo Down").ToString(), font, brush, PBL_VIDEO.Width / 11 * 1, PBL_VIDEO.Height / 12 * 5);
                }
            }

            if (RECEIVED_DATA.OPTICAL_TILT <= -5)
            {
                if (RECEIVED_DATA.OPTICAL_TILT <= -10)
                {
                    g.DrawString((("! Max Optical Elevation !\nGo Up")).ToString(), font, brush, PBL_VIDEO.Width / 11 * 1, PBL_VIDEO.Height / 12 * 7);
                }
                else
                {
                    g.DrawString((("! Optical Elevation Warning !\nGo Up")).ToString(), font, brush, PBL_VIDEO.Width / 11 * 1, PBL_VIDEO.Height / 12 * 7);
                }
            }
            // #####
            if (RECEIVED_DATA.WEAPON_TILT >= 25)
            {
                if (RECEIVED_DATA.WEAPON_TILT >= 30)
                {
                    g.DrawString(("! Max Weapon Elevation !\nGo Down").ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 12 * 5);
                }
                else
                {
                    g.DrawString(("! Weapon Elevation Warning !\nGo Down").ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 12 * 5);
                }
            }

            if (RECEIVED_DATA.WEAPON_TILT <= -15)
            {
                if (RECEIVED_DATA.WEAPON_TILT <= -20)
                {
                    g.DrawString((("! Max Weapon Elevation !\nGo Up")).ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 12 * 7);
                }
                else
                {
                    g.DrawString((("! Weapon Elevation Warning !\nGo Up")).ToString(), font, brush, PBL_VIDEO.Width / 5 * 4, PBL_VIDEO.Height / 12 * 7);
                }
            }

            redPen.Dispose();
            font.Dispose();
        }
        #endregion

        #region AZEL GUI

        List<Point> endPoints = new List<Point>();

        private void pictureBox_azimuth_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            int centerX = PB_AZIMUTH.Width / 2;
            int centerY = PB_AZIMUTH.Height / 3 * 2;

            /* X 모양 */
            Pen redPen = new Pen(Color.Red, 8);
            g.DrawLine(redPen, new Point(centerX - 10, centerY - 10), new Point(centerX + 10, centerY + 10));
            g.DrawLine(redPen, new Point(centerX + 10, centerY - 10), new Point(centerX - 10, centerY + 10));
            redPen.Dispose();
            /* */

            /* */
            float lineLength = RECEIVED_DATA.DISTANCE / define.LENGTH_SCALE;
            /* */

            /* Body Pan 막대기 */
            double radianAngleRCWS = RECEIVED_DATA.BODY_PAN * Math.PI / 180.0;
            int endXRCWS = centerX + (int)(lineLength * Math.Sin(radianAngleRCWS));
            int endYRCWS = centerY - (int)(lineLength * Math.Cos(radianAngleRCWS));
            g.DrawLine(Pens.Red, new Point(centerX, centerY), new Point(endXRCWS, endYRCWS));
            /* */            

            endPoints.Add(new Point(endXRCWS, endYRCWS));

            //foreach (var point in endPoints)
            //{
            //    g.DrawLine(Pens.Red, new Point(endXRCWS, endYRCWS), point);
            //}


            DRAW.Drawing(e.Graphics);

            /* Pin Point */

            /* */
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Red;
            g.DrawString(RECEIVED_DATA.DISTANCE.ToString()+"cm", font, brush, endXRCWS + 5, endYRCWS);
        }

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
            PB_AZIMUTH.Invalidate();
        }

        private void deletePinPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
        private void CleanupResources()
        {
            STREAM_WRITER?.Dispose();
            STREAM_READER?.Dispose();
            NETWORK_STREAM?.Dispose();
            TCP_CLIENT?.Close();
        }

        private void BTN_DISCONNECT_Click(object sender, EventArgs e)
        {
            CleanupResources();
            SEND_DATA.Button = SEND_DATA.Button | 0x04;
            Application.Exit();
            //THREAD.Join();
        }        

        private void Setting_Click(object sender, EventArgs e)
        {
            FormSettingMain SettingForm = new FormSettingMain();
            SettingForm.Show();
        }                         
        
        private void BTN_POWER_Click(object sender, EventArgs e)
        {
            pwr_flag = !pwr_flag;
            if (pwr_flag == false)
            {
                BTN_POWER.BackColor = Color.Red;
                SEND_DATA.Button = SEND_DATA.Button | 0x100000;
            }
            else
            {
                BTN_POWER.BackColor = Color.Green;
                SEND_DATA.Button = (uint)(SEND_DATA.Button & ~(0x00100000));                
            }
        }
        #endregion        

        private void ErrorHandleDisplay(string message)
        {
            UpdateUI(() => ReceiveDisplay("Connect ERROR: " + message));
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

    }
}