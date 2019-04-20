using System;

namespace TUA.Utilities
{
    // twitter snowflake
    public struct Snowflake
    {
        public ulong Raw { get; }

        public static readonly DateTime DiscordEpoc = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Snowflake(int worker, int proc)
        {
            Raw = (ulong)DateTime.UtcNow.ToUniversalTime().Subtract(
                new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
            Raw |= (uint)(worker & 0x1F) << 17;
            Raw |= (uint)(proc & 0x1F) << 12;
            Raw |= (ulong)TUAWorld.NextSnowflakeIncrement & 0xFFF;
        }

        public Snowflake(ulong raw) => Raw = raw;
    }
}
