using System;
using CrazyArcade.CAFramework;

namespace CrazyArcade.CAFrameWork.UDPUpdateSystem
{
	public interface IDeserializable: IEntity
	{
		int UpdateFieldWithStream(Byte[] stream, int offset); //return new offset
		int GetType();
	}
}

