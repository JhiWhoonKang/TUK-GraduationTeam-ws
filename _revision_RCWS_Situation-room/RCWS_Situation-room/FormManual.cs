using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCWS_Situation_room
{
    public partial class FormManual : Form
    {
        public FormManual()
        {
            InitializeComponent();
        }

        private void FormManual_Load(object sender, EventArgs e)
        {
            RTB_MANUAL.Text = ("Manual\n\n" +
                "고각 및 방위각 단위 | [degree]\n" +
                "거리 단위 | [cm]\n\n" +
                "Control Authority Status\n" +
                "\t상황실 권한 | 초록색\n" +
                "\t초소 권한 | 파란색\n" +
                "\t권한 요청 | 노란색\n\n" +
                "Weapon Status\n" +
                "\tTake Aim(조준) | 초록색: 미조준, 빨간색: 조준 완료\n" +
                "\tFire(격발) | 초록색: 미격발, 빨간색: 격발\n\n" +
                "Option\n" +
                "\t자동 조준\n" +
                "\t사람 자동 트래킹\n" +
                "\t회전 속도 | MIN: 1, MAX 1000\n" +
                "\tHome: Home 위치로 이동\n\n");
        }
    }
}
