using CustomMusic;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace CustomMusicOptions
{
    public static class SongUtility
    {
        public static SongType GetSongType(this SongDef def)
        {
            if (def is CustomSongDef)
            {
                if (CustomMusicOptionsSettings.CustomSongTypes.ContainsKey((CustomSongDef)def))
                {
                    return CustomMusicOptionsSettings.CustomSongTypes[(CustomSongDef)def];
                }
            }
            return def.tense ? SongType.Combat : SongType.Normal;
        }

        public static void SetSongType(this CustomSongDef def, SongType songType)
        {
            CustomMusicOptionsSettings.CustomSongTypes[def] = songType;
        }

        public static Texture2D GetTexture(this SongType songType)
        {
            switch (songType)
            {
                case SongType.Normal:
                    return ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore");
                case SongType.Combat:
                    return ContentFinder<Texture2D>.Get("UI/Icons/Colonistbar/Attacking");
                case SongType.HorrorRelax:
                    return ContentFinder<Texture2D>.Get("Things/Building/PitGate/PitGate");
                case SongType.HorrorTension:
                    return ContentFinder<Texture2D>.Get("Things/Building/FleshmassHeart");
                case SongType.HorrorCombat:
                    return ContentFinder<Texture2D>.Get("Things/Pawn/Devourer/Devourer_south");
                default:
                    throw new NotImplementedException();
            }
        }

        public static Color GetColor(this SongType songType)
        {
            return songType == SongType.Combat ? Color.red : Color.white;
        }

        public static bool AppropriateNowForCombatStatus(this SongDef def, MusicManagerPlay music)
        {
            if (music.DangerMusicMode)
            {
                return def.GetSongType() == SongType.Combat;
            }
            else
            {
                return def.GetSongType() == SongType.Normal;
            }
        }

        public static void DoSongTypeWidget(Vector2 position, ref bool tense, float size, bool disabled, bool paintable, Texture2D texChecked, Texture2D texUnchecked, CustomSongDef def)
        {
            if (Widgets.ButtonImage(new Rect(position.x, position.y, size, size), def.GetSongType().GetTexture(), def.GetSongType().GetColor()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (SongType songType in typeof(SongType).GetEnumValues())
                {
                    options.Add(new FloatMenuOption(songType.ToString(), delegate
                    {
                        def.SetSongType(songType);
                        def.tense = songType == SongType.Combat || songType == SongType.HorrorCombat;
                    }, songType.GetTexture(), songType.GetColor()));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }
        }
    }
}
