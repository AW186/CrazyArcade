﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CrazyArcade.CAFramework;
using Microsoft.Xna.Framework;

namespace CrazyArcade.CAFrameWork.UDPUpdateSystem
{
	public class UDPUpdateSystem: IGameSystem
	{
		private UdpClient client;
		private ISceneDelegate sceneDelegate;
		private Byte[] state;
		private IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
		private Mutex mu = new Mutex();
		public UDPUpdateSystem(UdpClient client, ISceneDelegate sceneDelegate)
		{
			this.client = client;
			this.sceneDelegate = sceneDelegate;
			Thread thread = new Thread(new ThreadStart(() => {
				long stateID = -1;
				while (true)
				{
					Byte[] stream = this.client.Receive(ref remoteEndpoint);
					if (stream.Length < 8) {
						Console.WriteLine(stream[0]);
						continue;
					}
					long newID = this.getStreamId(stream);
					if (newID > stateID)
					{
						stateID = newID;
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
		}

		public void RemoveAll()
		{
		}

		public void RemoveSprite(IEntity sprite)
		{
		}

		private int getObjectID(Byte[] stream, int offset)
		{
			return stream[offset];
		}
		private int getObjectType(Byte[] stream, int offset)
		{
			return stream[offset+1];
		}
		private long getStreamId(Byte[] stream)
		{
			long networkLong = 0;
			for (int i = 0; i < 8; i++)
			{
				networkLong += ((long)stream[i]) << ((7 - i) * 8);
			}
			return (long)IPAddress.NetworkToHostOrder(networkLong);
		}

		public void Update(GameTime time)
		{
			mu.WaitOne();

			mu.ReleaseMutex();
		}
	}
}

