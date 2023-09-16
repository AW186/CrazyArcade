using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using CrazyArcade.Blocks;
using CrazyArcade.BombFeature;
using CrazyArcade.CAFramework;
using CrazyArcade.CAFrameWork.CAGame;
using CrazyArcade.CAFrameWork.GameStates;
using CrazyArcade.CAFrameWork.InputSystem;
using CrazyArcade.Levels;
using CrazyArcade.PlayerStateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrazyArcade.CAFrameWork.UDPUpdateSystem
{

	public class UDPUpdateSystem : IGameSystem
	{
		private UdpClient client;
		private ISceneDelegate sceneDelegate;
		private Byte[] state = new Byte[0];
		private IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
		private Mutex mu = new Mutex();
		private IDeserializable[] entities = new IDeserializable[255];
		private IGameDelegate gameRef;
		public UDPUpdateSystem(UdpClient client, ISceneDelegate sceneDelegate, IGameDelegate game)
		{
			this.gameRef = game;
			this.client = client;
			this.sceneDelegate = sceneDelegate;
			Thread thread = new Thread(new ThreadStart(() =>
			{
				ulong stateID = 0;
				while (true)
				{
					Byte[] stream = this.client.Receive(ref remoteEndpoint);
					//Preparation State
					if (stream.Length < 8)
					{
						continue;
					}
					ulong newID = this.getStreamId(stream);
					if (newID > stateID)
					{
						stateID = newID;
						//Console.WriteLine(newID);
						mu.WaitOne();
						this.state = stream;
						mu.ReleaseMutex();
					}
				}
			}));
			thread.IsBackground = true;
			thread.Start();
		}

		public void AddSprite(IEntity sprite)
		{
			Console.WriteLine("Added");
		}

		public void RemoveAll()
		{
			//Console.WriteLine("Remove");
		}

		public void RemoveSprite(IEntity sprite)
		{
			if (sprite is OnlineCharacter)
			{
				gameRef.Scene = new PVPSummaryScene(gameRef, (sprite as OnlineCharacter).PlayerID);
			}
		}

		private int getObjectID(int offset)
		{
			return offset >= this.state.Length ? -1 : this.state[offset];
		}
		private int getObjectType(int offset)
		{
			return offset + 1 >= this.state.Length ? -1 : this.state[offset + 1];
		}
		private ulong getStreamId(Byte[] stream)
		{
			ulong networkLong = 0;
			for (int i = 0; i < 8; i++)
			{
				networkLong += ((ulong)stream[i]) << ((7 - i) * 8);
			}
			return networkLong;
		}

		public void Update(GameTime time)
		{
			mu.WaitOne();
			int offset = 8;
			for (int i = 0; i < 255; i++)
			{
				int id = this.getObjectID(offset);
				int objtype = this.getObjectType(offset);
				if (id != i)
				{
					this.sceneDelegate.ToRemoveEntity(this.entities[i]);
					this.entities[i] = null;
				}
				else if (this.entities[i] == null)
				{
					Console.WriteLine("Null");
					(offset, this.entities[i]) = make(offset);
					this.sceneDelegate.ToAddEntity(this.entities[i]);   //add new
				}
				else if (objtype != this.entities[i].GetType())
				{
					Console.WriteLine("Not the type: " + this.entities[i].GetType() + " " + objtype);
					this.sceneDelegate.ToRemoveEntity(this.entities[i]);    //remove old 
					(offset, this.entities[i]) = make(offset);
					this.sceneDelegate.ToAddEntity(this.entities[i]);       //add new
				}
				else offset = this.entities[i].UpdateFieldWithStream(this.state, offset);
			}
			mu.ReleaseMutex();
		}
		private (int, IDeserializable) make(int offset)
		{
			switch (this.getObjectType(offset))
			{
				case BLOCK_TYPE:
					Console.WriteLine("new block: " + this.state[offset + 3] + " " + this.state[offset + 4]);
					return (offset + 5, new DefaultBlock(
						new Vector2(this.state[offset + 3], this.state[offset + 4]),
						CreateLevel.LevelItem.StonePosition));
				case BOMB_TYPE:
					if ((this.state[offset + 4] & 0b10000000) == 1) return (offset + 5, null);
					Console.WriteLine(" " + (int)this.state[2] + " " + (int)this.state[3]);
					return (offset + 5, new WaterBomb(true,
						new Vector2((int)this.state[offset + 2], (int)this.state[offset + 3]),
						(int)this.state[offset + 4] & 0b01111111));
				case PLAYER_TYPE:
					Console.WriteLine("Added Player");
					int[] keySet = new int[5];
					keySet[0] = KeyBoardInput.KeyDown(Keys.Up);
					keySet[1] = KeyBoardInput.KeyDown(Keys.Down);
					keySet[2] = KeyBoardInput.KeyDown(Keys.Left);
					keySet[3] = KeyBoardInput.KeyDown(Keys.Right);
					keySet[4] = KeyBoardInput.KeyUp(Keys.Space);
					OnlineCharacter player = new OnlineCharacter(keySet);
					return (player.UpdateFieldWithStream(this.state, offset), player);
			}
			return (offset, null);
		}
		const int BLOCK_TYPE = 0;
		const int BOMB_TYPE = 1;
		const int PLAYER_TYPE = 2;
		public static int BlockType() => BLOCK_TYPE;
		public static int BombType() => BOMB_TYPE;
		public static int PLayerType() => PLAYER_TYPE;
	}
}

