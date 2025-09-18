using Godot;

[Tool]
public partial class GuildMember : Resource
{
    protected GuildMemberType guildMemberType;
    public GuildMemberType GuildMemberType
    {
        get => guildMemberType;
        set => guildMemberType = value;
    }

    protected GuildMemberStats guildMemberStats;
    public GuildMemberStats GuildMemberStats
    {
        get => guildMemberStats;
        set => guildMemberStats = value;
    }
}