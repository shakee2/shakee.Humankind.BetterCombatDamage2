using System;
using System.Collections.Generic;
using Amplitude.Mercury.UI;

namespace HumankindModTool
{
	// Token: 0x02000004 RID: 4
	public class GameOptionInfo
    {
        public string Key { get; set; }
        public string GroupKey { get; set; }
		public bool editbleInGame {get; set;} //edit
        public string Title { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public UIControlType ControlType { get; set; }
        public List<GameOptionStateInfo> States { get; set; } = new List<GameOptionStateInfo>();
    	
	}
}
