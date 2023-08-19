using System;
using CrazyArcade.CAFramework;

namespace CrazyArcade.CAFrameWork.UDPUserInputSystem
{
	public interface UDPInputSource: IEntity
	{
		Byte UdpContent();
	}
}

