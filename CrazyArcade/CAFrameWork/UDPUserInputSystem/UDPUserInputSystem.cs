using System;
using System.Collections.Generic;
using System.Net.Sockets;
using CrazyArcade.CAFramework;
using Microsoft.Xna.Framework;

namespace CrazyArcade.CAFrameWork.UDPUserInputSystem
{
	public class UDPUserInputSystem: IGameSystem
	{
		private UdpClient client;
		private List<UDPInputSource> inputSources = new List<UDPInputSource>();
		public UDPUserInputSystem(UdpClient client)
		{
			this.client = client;
		}

		public void AddSprite(IEntity sprite)
		{
			if (sprite is UDPInputSource)
			{
				inputSources.Add(sprite as UDPInputSource);
			}
		}

		public void RemoveAll()
		{
		}

		public void RemoveSprite(IEntity sprite)
		{
		}

		public void Update(GameTime time)
		{
			Console.WriteLine("Prep sending");
			Byte content = 0;
			foreach (UDPInputSource source in inputSources)
			{
				content = (Byte)(content | source.UdpContent());
			}
			Byte[] buf = new Byte[1];
			buf[0] = content;
			client.Send(buf, 1);
			Console.WriteLine("Sent");
		}
	}
}

