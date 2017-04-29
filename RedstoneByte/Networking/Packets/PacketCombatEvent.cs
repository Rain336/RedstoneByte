using System;
using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketCombatEvent : EntityPacket
    {
        public CombatAction Action { get; set; }
        public override int EntityId { get; set; }
        public int Duration { get; set; }
        public int PlayerId { get; set; }
        public TextBase Message { get; set; }

        public override void CompareExchange(int test, int value)
        {
            base.CompareExchange(test, value);
            if (PlayerId == test)
                PlayerId = value;
        }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Action = (CombatAction) buffer.ReadVarInt();
            switch (Action)
            {
                case CombatAction.Enter:
                    break;

                case CombatAction.Exit:
                    Duration = buffer.ReadVarInt();
                    EntityId = buffer.ReadInt();
                    break;

                case CombatAction.Death:
                    PlayerId = buffer.ReadVarInt();
                    EntityId = buffer.ReadInt();
                    Message = JsonConvert.DeserializeObject<TextBase>(buffer.ReadString());
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt((int) Action);
            switch (Action)
            {
                case CombatAction.Enter:
                    break;

                case CombatAction.Exit:
                    buffer.WriteVarInt(Duration);
                    buffer.WriteInt(EntityId);
                    break;

                case CombatAction.Death:
                    buffer.WriteVarInt(PlayerId);
                    buffer.WriteInt(EntityId);
                    buffer.WriteString(JsonConvert.SerializeObject(Message));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum CombatAction
        {
            Enter,
            Exit,
            Death
        }
    }
}