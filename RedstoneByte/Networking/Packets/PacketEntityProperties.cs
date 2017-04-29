using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityProperties : EntityPacket
    {
        public override int EntityId { get; set; }
        public readonly List<Property> Properties = new List<Property>();

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            var length = buffer.ReadVarInt();
            for (var i = 0; i < length; i++)
            {
                Properties.Add(Property.ReadFromBuffer(buffer));
            }
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
        }

        public sealed class Property
        {
            public readonly string Key;
            public readonly double Value;
            public readonly List<PropertyAction> Actions = new List<PropertyAction>();

            public Property(string key, double value)
            {
                Key = key;
                Value = value;
            }

            public static Property ReadFromBuffer(IByteBuffer buffer)
            {
                var result = new Property(buffer.ReadString(), buffer.ReadDouble());
                var length = buffer.ReadVarInt();
                for (var i = 0; i < length; i++)
                {
                    result.Actions.Add(PropertyAction.ReadFromBuffer(buffer));
                }
                return result;
            }

            public static Property MaxHealth(double value = 20.0, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 1024.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Max Health must be between 0.0 and 1024.0");

                var result = new Property("generic.maxHealth", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property FollowRange(double value = 32.0, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 2048.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Follow Range must be between 0.0 and 2048.0");

                var result = new Property("generic.followRange", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property KnockbackResistance(double value = 0.0, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Knockback Resistance must be between 0.0 and 1.0");

                var result = new Property("generic.knockbackResistance", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property MovementSpeed(double value = 0.699999988079071, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 1024.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Movement Speed must be between 0.0 and 1024.0");

                var result = new Property("generic.movementSpeed", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property AttackDamage(double value = 2.0, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 2048.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Attack Damage must be between 0.0 and 2048.0");

                var result = new Property("generic.attackDamage", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property AttackSpeed(double value = 4.0, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 1024.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Attack Speed must be between 0.0 and 1024.0");

                var result = new Property("generic.attackSpeed", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property HorseJumpStrength(double value = 0.7, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 2.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Horse Jump Strength must be between 0.0 and 2.0");

                var result = new Property("horse.jumpStrength", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }

            public static Property ZombieSpawnReinforcementsChance(double value = 0.0, params PropertyAction[] actions)
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The Zombie Spawn Reinforcements Chance must be between 0.0 and 1.0");

                var result = new Property("zombie.spawnReinforcements", value);
                if(actions.Length > 0)
                    result.Actions.AddRange(actions);
                return result;
            }
        }

        public sealed class PropertyAction
        {
            public readonly Guid Guid;
            public readonly double Amount;
            public readonly Operation Operation;

            public PropertyAction(Guid guid, double amount, Operation operation)
            {
                Guid = guid;
                Amount = amount;
                Operation = operation;
            }

            public static PropertyAction ReadFromBuffer(IByteBuffer buffer)
            {
                return new PropertyAction(buffer.ReadGuid(), buffer.ReadDouble(), (Operation) buffer.ReadByte());
            }
        }

        public enum Operation
        {
            AddSub,
            AddSubPercent,
            Multiply
        }
    }
}