using System;
using System.Windows.Forms;

namespace RCWS_Situation_room
{
    public partial class FormDataSetting : Form
    {
        string currentTime;

        public FormDataSetting()
        {
            InitializeComponent();            
        }

        private void FormDataSetting_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;

            LV_RX.View = View.Details;
            LV_RX.GridLines = true;
            LV_RX.FullRowSelect = true;
            LV_RX.CheckBoxes = true;            
            LV_RX.Scrollable = true;

            LV_RX.Columns.Add("TIME", 100, HorizontalAlignment.Center);
            LV_RX.Columns.Add("OPTICAL_TILT", 100, HorizontalAlignment.Center);
            LV_RX.Columns.Add("WEAPON_TILT", 100, HorizontalAlignment.Center);
            LV_RX.Columns.Add("BODY_PAN", 80, HorizontalAlignment.Center);
            LV_RX.Columns.Add("SENTRY_AZIMUTH", 140, HorizontalAlignment.Center);
            LV_RX.Columns.Add("SENTRY_ELEVATION", 140, HorizontalAlignment.Center);
            LV_RX.Columns.Add("DISTANCE", 80, HorizontalAlignment.Center);
            LV_RX.Columns.Add("PERMISSION", 100, HorizontalAlignment.Center);
            LV_RX.Columns.Add("TAKE_AIM", 80, HorizontalAlignment.Center);
            LV_RX.Columns.Add("FIRE", 70, HorizontalAlignment.Center);
            LV_RX.Columns.Add("MODE", 70, HorizontalAlignment.Center);

            LV_TX.View = View.Details;
            LV_TX.GridLines = true;
            LV_TX.FullRowSelect = true;
            LV_TX.CheckBoxes = true;
            LV_TX.Scrollable = true;

            LV_TX.Columns.Add("TIME", 100, HorizontalAlignment.Center);
            LV_TX.Columns.Add("BODY_PAN", 100, HorizontalAlignment.Center);
            LV_TX.Columns.Add("WEAPON_TILT", 100, HorizontalAlignment.Center);
            LV_TX.Columns.Add("Button", 70, HorizontalAlignment.Center);
            LV_TX.Columns.Add("C_X1", 70, HorizontalAlignment.Center);
            LV_TX.Columns.Add("C_Y1", 70, HorizontalAlignment.Center);
            LV_TX.Columns.Add("D_X1", 70, HorizontalAlignment.Center);
            LV_TX.Columns.Add("D_Y1", 70, HorizontalAlignment.Center);
            LV_TX.Columns.Add("D_X2", 70, HorizontalAlignment.Center);
            LV_TX.Columns.Add("D_Y2", 70, HorizontalAlignment.Center);
        }

        public void Add_RX_ListView(Packet.RECEIVED_PACKET receivedData)
        {
            if (LV_RX.InvokeRequired)
            {
                LV_RX.Invoke(new Action(() => Add_RX_ListView(receivedData)));
            }
            else
            {
                currentTime = DateTime.Now.ToString("HH:mm:ss");

                ListViewItem item = new ListViewItem(currentTime);
                item.SubItems.Add(receivedData.OPTICAL_TILT.ToString());
                item.SubItems.Add(receivedData.WEAPON_TILT.ToString());
                item.SubItems.Add(receivedData.BODY_PAN.ToString());
                item.SubItems.Add(receivedData.SENTRY_AZIMUTH.ToString());
                item.SubItems.Add(receivedData.SENTRY_ELEVATION.ToString());
                item.SubItems.Add(receivedData.PERMISSION.ToString());
                item.SubItems.Add(receivedData.TAKE_AIM.ToString());
                item.SubItems.Add(receivedData.FIRE.ToString());
                item.SubItems.Add(receivedData.MODE.ToString());

                LV_RX.Items.Add(item);
            }
        }

        public void Add_TX_ListView(Packet.SEND_PACKET sendData)
        {            
            if (LV_TX.InvokeRequired)
            {
                LV_TX.Invoke(new Action(() => Add_TX_ListView(sendData)));
            }
            else
            {
                currentTime = DateTime.Now.ToString("HH:mm:ss");

                ListViewItem item = new ListViewItem(sendData.BODY_PAN.ToString());
                item.SubItems.Add(sendData.OPTICAL_TILT.ToString());
                item.SubItems.Add(sendData.Button.ToString());
                item.SubItems.Add(sendData.C_X1.ToString());
                item.SubItems.Add(sendData.C_Y1.ToString());
                item.SubItems.Add(sendData.D_X1.ToString());
                item.SubItems.Add(sendData.D_Y1.ToString());
                item.SubItems.Add(sendData.D_X2.ToString());
                item.SubItems.Add(sendData.D_Y2.ToString());

                LV_TX.Items.Add(item);
            }
        }
    }
}