using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCWS_Situation_Room_GUI.Model
{
    class RCWSMainGUIModel
    {
        public bool RCWSConnected { get; set; }
        public bool RCWSDisconnected { get; set; }
        public bool EMSActive { get; set; }
        public bool SettingActive { get; set; }
    }
}
