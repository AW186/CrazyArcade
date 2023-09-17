
using CrazyArcade.CAFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrazyArcade.Final;
using CrazyArcade.Singletons;
using CrazyArcade.Content;
using Microsoft.Xna.Framework;
using CrazyArcade.BombFeature;
using System.Diagnostics;
using CrazyArcade.CAFrameWork.InputSystem;
using Microsoft.Xna.Framework.Input;
using CrazyArcade.CAFrameWork.SoundEffectSystem;
using CrazyArcade.CAFrameWork.Transition;
using CrazyArcade.CAFrameWork.UDPUserInputSystem;
using CrazyArcade.CAFrameWork.UDPUpdateSystem;

namespace CrazyArcade.PlayerStateMachine
{
    public class OnlineCharacter : Character, IInputController, ISavable,
        UDPInputSource, IDeserializable
    {
        const int UP = 1;
		const int DOWN = 2;
		const int LEFT = 3;
		const int RIGHT = 4;
		Byte content = 0;
        private static int playerCount = 1;
        private int playerID;
        public int PlayerID => playerID;
        Dictionary<int, Action> commands = new Dictionary<int, Action>();
        public OnlineCharacter(int[] keySet) : base(false)
        {
            Action[] actions = new Action[5];
            actions[0] = KeyUp;
            actions[1] = KeyDown;
            actions[2] = KeyLeft;
            actions[3] = KeyRight;
            actions[4] = KeySpace;
            this.playerID = playerCount;
            for (int i = 0; i < keySet.Length; i++)
            {
                commands[keySet[i]] = actions[i];
            }
            playerCount++;
		}

        private bool isMoving()
        {
            return content != 0;
        }

        private void KeyUp()
        {
            if (isMoving()) return;
            content = UP;
        }

        private void KeyDown()
        {
            if (isMoving()) return;
			content = DOWN;
        }

        private void KeyLeft()
        {
            if (isMoving()) return;
			content = LEFT;
        }

        private void KeyRight()
        {
            if (isMoving()) return;
			content = RIGHT;
        }

        private void KeySpace()
        {
            content = (byte)(content | (1 << 3));
        }

        public Dictionary<int, Action> GetCommands()
        {
            return commands;
        }

		public byte UdpContent()
		{
            byte res = this.content;
            this.content = 0;
            return res;
		}

        private static uint getNetUInt(byte[] stream, int offset)
        {
			uint networkInt = 0;
			for (int i = 0; i < 4; i++)
			{
				networkInt += ((uint)stream[i + offset]) << ((3 - i) * 8);
			}
			return networkInt;
		}
        private static byte[] dirmap = { 2, 0, 2, 1, 3 };
		public int UpdateFieldWithStream(byte[] stream, int offset)
		{
            uint x_in = getNetUInt(stream, offset + 2);
			uint y_in = getNetUInt(stream, offset + 6);
            this.GameCoord = new Vector2(((float)x_in) / 1024f - 0.5f, ((float)y_in) / 1024f - 0.5f);
            byte diridx = stream[offset + 10];
            this.direction = (Dir)dirmap[diridx];
            switch (diridx) {
                case 1:
                    this.moveInputs.Y = -1;
                    break;
				case 2:
					this.moveInputs.Y = 1;
					break;
				case 3:
					this.moveInputs.X = -1;
					break;
				case 4:
					this.moveInputs.X = 1;
					break;
                default:
                    this.moveInputs = new Vector2(0, 0);
                    break;
			}
            return offset + 11;
        }

		int IDeserializable.GetType() => UDPUpdateSystem.PLayerType();

		public override bool Collide(IExplosion bomb)
		{
			return true;
		}

	}
}