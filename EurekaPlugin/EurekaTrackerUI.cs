using Dalamud.Plugin;
using Dalamud.Data;
using Lumina.Excel.GeneratedSheets;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using EurekaPlugin.Models;

namespace EurekaPlugin
{
    class EurekaTrackerUI : IDisposable
    {
        private readonly DalamudPluginInterface pluginInterface;
        private readonly EurekaPlugin plugin;

        private Vector4 RED = new Vector4(0.89f, 0.5f, 0.5f, 1);
        private Vector4 ORANGE = new Vector4(1, 0.65f, 0, 1);
        private Vector4 GREEN = new Vector4(0.33f, 0.76f, 0.67f, 1);
        private Vector4 BLUE = new Vector4(0, 0.48f, 1, 1);

        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private string trackerIdString = string.Empty;
        private bool haveColumnWidthsBeenSet = false;
        private DateTime resetAllConfirmTime = new DateTime(1970, 1, 1);

        public EurekaTrackerUI(DalamudPluginInterface pluginInterface, EurekaPlugin plugin)
        {
            this.pluginInterface = pluginInterface;
            this.plugin = plugin;
        }

        public void Draw()
        {
            try
            {
                DrawTracker();
            } 
            catch (KeyNotFoundException ex)
            {
                PluginLog.LogError(ex, "KeyNotFoundException caught while drawning tracker");
            }
        }

        public void DrawTracker()
        {
            if (!Visible)
                return;

            ImGui.SetNextWindowSize(new Vector2(680, 505), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(680, 315), new Vector2(float.MaxValue, float.MaxValue));
            if (ImGui.Begin("Eureka Tracker", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                if (plugin.getConnection() == null || !plugin.getConnection().IsConnected() || plugin.getConnection().GetTracker() == null || plugin.getConnection().GetTracker().GetMonsters().Count < 10) // If we're not currently connected, draw the connection UI
                {
                    if (plugin.getConnection() != null && plugin.getConnection().GetTrackerId().Equals("INVALID"))
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, RED);
                        ImGui.Text("Invalid Tracker ID");
                        ImGui.PopStyleColor();
                    }
                    else
                    {
                        ImGui.Text("You are not current connected to a tracker. Enter a tracker ID below, or create a new one.");
                    }
                    ImGui.InputText("Tracker ID", ref this.trackerIdString, 6);
                    ImGui.SameLine();
                    if (ImGui.Button("Connect"))
                    {
                        plugin.setConnection(trackerIdString);
                    }
                    if (ImGui.Button("New Anemos Tracker"))
                    {
                        // TODO
                    }
                    if (ImGui.Button("New Pagos Tracker"))
                    {
                        // TODO
                    }
                    if (ImGui.Button("New Pyros Tracker"))
                    {
                        // TODO
                    }
                    if (ImGui.Button("New Hydatos Tracker"))
                    {
                        // TODO
                    }
                }
                else
                {
                    ImGui.Text(plugin.getConnection().GetTrackerId());
                    ImGui.SameLine();
                    if (ImGui.SmallButton("I"))
                    {
                        ImGui.SetClipboardText(plugin.getConnection().GetTrackerId());
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                        ImGui.TextUnformatted("Copy tracker ID");
                        ImGui.PopTextWrapPos();
                        ImGui.EndTooltip();
                    }
                    ImGui.SameLine();
                    if (ImGui.SmallButton("L"))
                    {
                        ImGui.SetClipboardText("https://ffxiv-eureka.com/" + plugin.getConnection().GetTrackerId());
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                        ImGui.TextUnformatted("Copy tracker link");
                        ImGui.PopTextWrapPos();
                        ImGui.EndTooltip();
                    }
                    ImGui.SameLine();
                    if (plugin.getConnection().CanModify())
                    {
                        if (ImGui.SmallButton(plugin.getConnection().GetPassword()))
                        {
                            ImGui.SetClipboardText(plugin.getConnection().GetPassword());
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                            ImGui.TextUnformatted("Copy password");
                            ImGui.PopTextWrapPos();
                            ImGui.EndTooltip();
                        }
                    }
                    else
                    {
                        if (ImGui.SmallButton("Set password from clipboard"))
                        {
                            plugin.getConnection().SetPassword(ImGui.GetClipboardText()); // TODO: Feedback if invalid
                        }
                    }
                    ImGui.SameLine();
                    ImGui.Text(plugin.getConnection().GetViewerCount().ToString() + " currently viewing");
                    ImGui.SameLine();
                    ImGui.Text(Utilities.GetEorzeaTime().Hour.ToString("D2") + ":" + Utilities.GetEorzeaTime().Minute.ToString("D2") + ":" + Utilities.GetEorzeaTime().Second.ToString("D2"));
                    ImGui.SameLine();
                    bool isDay;
                    string nextTimeString;
                    if (Utilities.GetEorzeaTime().Hour < 6 || Utilities.GetEorzeaTime().Hour >= 19)
                    {
                        ImGui.Text("Night");
                        isDay = false;
                        TimeSpan timeUntilDay = Utilities.EorzeaTimeSpanToRealTimeSpan(Utilities.TimeUntilEorzeaDay());
                        nextTimeString = "Day in " + timeUntilDay.Minutes.ToString("D2") + ":" + timeUntilDay.Seconds.ToString("D2");
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                            ImGui.TextUnformatted(nextTimeString);
                            ImGui.PopTextWrapPos();
                            ImGui.EndTooltip();

                        }
                        }
                    else
                    {
                        ImGui.Text("Day");
                        isDay = true;
                        TimeSpan timeUntilNight = Utilities.EorzeaTimeSpanToRealTimeSpan(Utilities.TimeUntilEorzeaNight());
                        nextTimeString = "Night in " + timeUntilNight.Minutes.ToString("D2") + ":" + timeUntilNight.Seconds.ToString("D2");
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                            ImGui.TextUnformatted(nextTimeString);
                            ImGui.PopTextWrapPos();
                            ImGui.EndTooltip();
                        }
                    }
                    ImGui.SameLine();
                    ImGui.Text(plugin.getConnection().GetTracker().GetCurrentWeather().ToFriendlyString());
                    Dictionary<EurekaWeather, TimeSpan> timeUntilWeathers = plugin.getConnection().GetTracker().GetTimeUntilWeathers();
                    if (ImGui.IsItemHovered())
                    {
                        foreach (EurekaWeather weather in timeUntilWeathers.Keys)
                        {
                            TimeSpan timeUntilWeather;
                            try
                            {
                                timeUntilWeather = timeUntilWeathers[weather];
                            } 
                            catch (KeyNotFoundException ex)
                            {
                                PluginLog.LogError(ex, "KeyNotFoundException line 192");
                                break;
                            }
                            ImGui.BeginTooltip();
                            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                            ImGui.TextUnformatted(weather.ToFriendlyString() + " in " + (timeUntilWeather.Minutes + (timeUntilWeather.Hours * 60)).ToString("D2") + ":" + timeUntilWeather.Seconds.ToString("D2"));
                            ImGui.PopTextWrapPos();
                            ImGui.EndTooltip();
                        }
                    }
                    ImGui.SameLine();
                    if(ImGui.SmallButton("Disconnect"))
                    {
                        plugin.getConnection().Leave();
                    }

                    Dictionary<int, EurekaMonster> monsterList = plugin.getConnection().GetTracker().GetMonsters();
                    ImGui.Columns(6, "monsterTable");
                    if (!this.haveColumnWidthsBeenSet)
                    {
                        ImGui.SetColumnWidth(0, ImGui.CalcTextSize("LVL").X + 10f); // Set Lvl Width
                        ImGui.SetColumnWidth(1, ImGui.CalcTextSize("The Emperor of Anemos").X + 20f);
                        ImGui.SetColumnWidth(2, ImGui.CalcTextSize("Demon of the Incunable").X + 20f);
                        ImGui.SetColumnWidth(3, ImGui.CalcTextSize("Not Spawned").X + 20f);
                        ImGui.SetColumnWidth(4, ImGui.CalcTextSize("Thunderstorms in 88:88").X + 20f);
                        ImGui.SetColumnWidth(5, ImGui.CalcTextSize("Reset All").X + 20f);
                        this.haveColumnWidthsBeenSet = true;
                    }

                    ImGui.Separator();
                    ImGui.Text("LVL");
                    ImGui.NextColumn();
                    ImGui.Text("NM");
                    ImGui.NextColumn();
                    ImGui.Text("Spawned By");
                    ImGui.NextColumn();
                    ImGui.Text("Popped At");
                    ImGui.NextColumn();
                    ImGui.Text("Respawn In");
                    ImGui.NextColumn();
                    if (plugin.getConnection().CanModify())
                    {
                        ImGui.PushStyleColor(ImGuiCol.Button, RED);
                        if (resetAllConfirmTime + TimeSpan.FromSeconds(5) < DateTime.Now)
                        {
                            if (ImGui.SmallButton("Reset All"))
                            {
                                resetAllConfirmTime = DateTime.Now;
                            }
                        }
                        else
                        {
                            if (ImGui.SmallButton("Confirm"))
                            {
                                resetAllConfirmTime = new DateTime(1970, 1, 1);
                                plugin.getConnection().ResetAll();
                            }
                        }
                        ImGui.PopStyleColor();
                    }
                    foreach (int monsterId in monsterList.Keys)
                    {
                        ImGui.NextColumn();
                        EurekaMonster monster;
                        try
                        {
                            monster = monsterList[monsterId];
                        }
                        catch (KeyNotFoundException ex)
                        {
                            PluginLog.LogError(ex, "KeyNotFoundException line 256");
                            break;
                        }
                        ImGui.Text(monster.FateLevel.ToString());
                        ImGui.NextColumn();
                        if (ImGui.SmallButton(monster.MonsterName))
                        {
                            // TODO: Wait for Dalamud update, this doesn't work right now
                            //pluginInterface.Framework.Gui.OpenMapWithMapLink(new Dalamud.Game.Chat.SeStringHandling.Payloads.MapLinkPayload(monster.TerritoryTypeId, monster.MapId, monster.XPos, monster.YPos, 0.05f));
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                            ImGui.TextUnformatted(monster.MonsterElement.ToFriendlyString());
                            ImGui.TextUnformatted("FATE Name: " + monster.FateName);
                            if (monster.SpawnRequiredNight)
                            {
                                ImGui.TextUnformatted("Requires Night");
                            }
                            if (monster.SpawnRequiredWeather != EurekaWeather.None)
                            {
                                ImGui.TextUnformatted("Requires " + monster.SpawnRequiredWeather.ToFriendlyString());
                            }
                            if (monster.RewardOne != null)
                            {
                                ImGui.TextUnformatted("Potential Drops:");
                                ImGui.TextUnformatted(monster.RewardOne);
                            }
                            if (monster.RewardTwo != null)
                            {
                                ImGui.TextUnformatted(monster.RewardTwo);
                            }
                            ImGui.PopTextWrapPos();
                            ImGui.EndTooltip();
                        }
                        ImGui.NextColumn();
                        if (ImGui.SmallButton(monster.SpawnMobName)) {
                            // TODO: Wait for Dalamud update, this doesn't work right now
                            //pluginInterface.Framework.Gui.OpenMapWithMapLink(new Dalamud.Game.Chat.SeStringHandling.Payloads.MapLinkPayload(monster.TerritoryTypeId, monster.MapId, monster.SpawnMobXPos, monster.SpawnMobYPos, 0.05f));
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                            ImGui.TextUnformatted(monster.SpawnMobElement.ToFriendlyString());
                            if (monster.SpawnMobRequiredNight)
                            {
                                ImGui.TextUnformatted("Requires Night");
                            }
                            if (monster.SpawnMobRequiredWeather != EurekaWeather.None)
                            {
                                ImGui.TextUnformatted("Requires " + monster.SpawnMobRequiredWeather.ToFriendlyString());
                            }
                            ImGui.PopTextWrapPos();
                            ImGui.EndTooltip();
                        }
                        ImGui.NextColumn();
                        if (!monster.HasSpawnTime())
                        {
                            ImGui.Text(monster.GetSpawnTime().Hour.ToString("D2") + ":" + monster.GetSpawnTime().Minute.ToString("D2"));
                        }
                        else
                        {
                            ImGui.Text("Not Spawned");
                        }
                        ImGui.NextColumn();
                        List<string> respawnNeeds = new List<string>();
                        if (monster.IsKilled())
                        {
                            respawnNeeds.Add("Respawn in: " + (monster.GetTimeUntilReset().Minutes + (monster.GetTimeUntilReset().Hours * 60)).ToString("D2") + ":" + monster.GetTimeUntilReset().Seconds.ToString("D2"));
                        }
                        if (monster.SpawnRequiredWeather != EurekaWeather.None && monster.SpawnRequiredWeather != plugin.getConnection().GetTracker().GetCurrentWeather())
                        {
                            EurekaWeather weather = monster.SpawnRequiredWeather;
                            TimeSpan timeUntilWeather = timeUntilWeathers[weather];
                            respawnNeeds.Add(weather.ToFriendlyString() + " in " + (timeUntilWeather.Minutes + (timeUntilWeather.Hours * 60)).ToString("D2") + ":" + timeUntilWeather.Seconds.ToString("D2"));
                        }
                        else if (monster.SpawnMobRequiredWeather != EurekaWeather.None && monster.SpawnMobRequiredWeather != plugin.getConnection().GetTracker().GetCurrentWeather())
                        {
                            EurekaWeather weather = monster.SpawnMobRequiredWeather;
                            TimeSpan timeUntilWeather = timeUntilWeathers[weather];
                            respawnNeeds.Add(weather.ToFriendlyString() + " in " + (timeUntilWeather.Minutes + (timeUntilWeather.Hours * 60)).ToString("D2") + ":" + timeUntilWeather.Seconds.ToString("D2"));
                        }
                        if (monster.SpawnRequiredNight && isDay)
                        {
                            respawnNeeds.Add(nextTimeString);
                        }
                        else if (monster.SpawnMobRequiredNight && isDay)
                        {
                            respawnNeeds.Add(nextTimeString);
                        }
                        if (respawnNeeds.Count == 0)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Text, GREEN);
                            ImGui.Text("Ready");
                            ImGui.PopStyleColor();
                        }
                        else
                        {
                            if (respawnNeeds[0].StartsWith("Respawn"))
                            {
                                ImGui.PushStyleColor(ImGuiCol.Text, RED);
                            }
                            else
                            {
                                ImGui.PushStyleColor(ImGuiCol.Text, ORANGE);
                            }
                            ImGui.Text(respawnNeeds[0]);
                            ImGui.PopStyleColor();
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.BeginTooltip();
                                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                                foreach (string s in respawnNeeds) {
                                    ImGui.TextUnformatted(s);
                                }
                                ImGui.PopTextWrapPos();
                                ImGui.EndTooltip();
                            }
                        }
                        ImGui.NextColumn();
                        if (plugin.getConnection().CanModify())
                        {
                            if (monster.IsKilled())
                            {
                                ImGui.PushStyleColor(ImGuiCol.Button, RED);
                                if (ImGui.SmallButton("RESET"))
                                {
                                    plugin.getConnection().ResetKill(monsterId);
                                }
                            }
                            else
                            {
                                ImGui.PushStyleColor(ImGuiCol.Button, BLUE);
                                if (ImGui.SmallButton("POP"))
                                {
                                    plugin.getConnection().SetKilled(monsterId, Utilities.CurrentTimestampMilliseconds());
                                }
                            }
                            ImGui.PopStyleColor();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
