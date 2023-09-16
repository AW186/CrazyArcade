using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyArcade.Blocks;
using CrazyArcade.CAFramework;
using CrazyArcade.CAFrameWork.CAGame;
using CrazyArcade.CAFrameWork.InputSystem;
using CrazyArcade.Final;
using CrazyArcade.Items;
using CrazyArcade.PlayerStateMachine;
using CrazyArcade.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;

namespace CrazyArcade.CAFrameWork.GameStates
{
    public class PVPSummaryScene : MenuScene
    {
        private int winner;
        public PVPSummaryScene(IGameDelegate gameRef, int winner)
        {
            buttons = new Button[2];
            buttons[0] = new Button("Game Over Main Menu", "Main Menu", Button.GetBasePosition(2f), gameRef.NewGame);
            this.gameRef = gameRef;
            this.Load();
        }

        public override List<Vector2> PlayerPositions => throw new NotImplementedException();

        public override void Load()
        {
            base.Load();
            UI_Singleton.ClearGUI();
            UI_Singleton.AddPreDesignedComposite(new TitleText("Game over title","Player " + winner + " wins"));
            for (int i = 0; i < buttons.Length; i++)
            {
                UI_Singleton.AddPreDesignedComposite(buttons[i]);
            }
        }
    }
}
