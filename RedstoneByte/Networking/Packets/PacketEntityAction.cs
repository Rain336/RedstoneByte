using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityAction : EntityPacket
    {
        public override int EntityId { get; set; }
        public EntityAction Action { get; set; }
        public int JumpBoost { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Action = (EntityAction) buffer.ReadVarInt();
            JumpBoost = buffer.ReadVarInt();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteVarInt((int) Action);
            buffer.WriteVarInt(Action == EntityAction.StartJumpWithHorse ? JumpBoost : 0);
        }
        
        public enum EntityAction
        {
            StartSneaking,
            StopSneaking,
            LeaveBed,
            StartSprinting,
            StopSprinting,
            StartJumpWithHorse,
            StopJumpWithHorse,
            OpenHorseInventory,
            StartFlyingWithElytra
        }
    }
}