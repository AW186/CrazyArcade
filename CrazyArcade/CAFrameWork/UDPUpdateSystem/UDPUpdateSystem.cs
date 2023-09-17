using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CrazyArcade.Blocks;
using CrazyArcade.CAFramework;
using CrazyArcade.Levels;
using Microsoft.Xna.Framework;

namespace CrazyArcade.CAFrameWork.UDPUpdateSystem
{
	
	public class UDPUpdateSystem: IGameSystem
	{
		private UdpClient client;
		private ISceneDelegate sceneDelegate;
		private Byte[] state = new Byte[0];
		private IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
		private Mutex mu = new Mutex();
		private IDeserializable[] entities = new IDeserializable[255];
		public UDPUpdateSystem(UdpClient client, ISceneDelegate sceneDelegate)
		{
			this.client = client;
			this.sceneDelegate = sceneDelegate;
			Thread thread = new Thread(new ThreadStart(() => {
				ulong stateID = 0;
				while (true)
				{
					Byte[] stream = this.client.Receive(ref remoteEndpoint);
					//Preparation State
					if (stream.Length < 8) {
						continue;
					}
					ulong newID = this.getStreamId(stream);
					if (newID > stateID)
					{
						stateID = newID;
						Console.WriteLine(newID);
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

		}

		public void RemoveSprite(IEntity sprite)
		{
		}

		private int getObjectID(int offset)
		{
			return offset >= this.state.Length ? -1 : this.state[offset];
		}
		private int getObjectType(int offset)
		{
			return offset + 1 >= this.state.Length ? -1 : this.state[offset+1];
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
					(offset, this.entities[i]) = make(offset);
					this.sceneDelegate.ToAddEntity(this.entities[i]);	//add new
				} else if (objtype != this.entities[i].GetType())
				{
					this.sceneDelegate.ToRemoveEntity(this.entities[i]);	//remove old 
					(offset, this.entities[i]) = make(offset);
					this.sceneDelegate.ToAddEntity(this.entities[i]);		//add new
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
					return (offset + 5, new DefaultBlock(
						new Vector2(this.state[offset + 3], this.state[offset + 4]),
						CreateLevel.LevelItem.StonePosition));
			}
			return (offset, null);
		}
		const int BLOCK_TYPE = 0;
		public static int BlockType() => BLOCK_TYPE;
	}
}

