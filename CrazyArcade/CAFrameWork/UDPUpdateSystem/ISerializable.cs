using System;
using CrazyArcade.CAFramework;

namespace CrazyArcade.CAFrameWork.UDPUpdateSystem
{
	public interface ISerializable: IEntity
	{
		int UpdateFieldWithStream(Byte[] stream, int offset); //return new offset
		int type();
	}
}

