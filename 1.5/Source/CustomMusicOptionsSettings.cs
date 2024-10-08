using CustomMusic;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace CustomMusicOptions
{
    public enum SongType
    {
        Normal,
        Combat,
        HorrorRelax,
        HorrorTension,
        HorrorCombat
    }

    public class CustomMusicOptionsSettings : ModSettings
    {
        public static bool DisableVanillaMusic = false;
        public static Dictionary<CustomSongDef, SongType> CustomSongTypes = new Dictionary<CustomSongDef, SongType>();

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("CustomMusicOptions_DisableVanillaMusic".Translate(), ref DisableVanillaMusic);

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref DisableVanillaMusic, "DisableVanillaMusic", false);
        }
    }
}
