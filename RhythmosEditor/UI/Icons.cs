using UnityEngine;
using RhythmosEditor.Utils;

namespace RhythmosEditor.UI
{
    internal static class Icons
    {
        public static Texture2D Rhythmos { private set; get; }
        public static Texture2D Pixel { private set; get; }

        public static Texture2D Add { private set; get; }
        public static Texture2D Duplicate { private set; get; }
        public static Texture2D Trash { private set; get; }
        public static Texture2D Delete { get; private set; }

        public static Texture2D Undo { private set; get; }
        public static Texture2D Redo { private set; get; }
        public static Texture2D LeftArrow { private set; get; }
        public static Texture2D RightArrow { private set; get; }

        public static Texture2D TimelineBg { private set; get; }
        public static Texture2D Play { private set; get; }
        public static Texture2D Stop { private set; get; }

        public static Texture2D ToStart { get; private set; }
        public static Texture2D ToEnd { private set; get; }
        public static Texture2D ToNext { private set; get; }
        public static Texture2D ToPrevious { private set; get; }


        public static Texture2D MuteOn { private set; get; }
        public static Texture2D MuteOff { private set; get; }
        public static Texture2D Loop { private set; get; }
        public static Texture2D Metronome { private set; get; }

        public static Texture2D TrackArrow { private set; get; }
        public static Texture2D AddNote { get; private set; }


        static Icons()
        {
            Load();
        }

        public static void Load()
        {

            if (Pixel == null)
            {
                Pixel = TextureUtility.CreateTexture(1, 1, Color.white);
                Pixel.hideFlags = HideFlags.HideAndDontSave;
            }

            // Undo redo
            if (Undo == null)
            {
                Undo = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAsklEQVQ4EWNgGNTg////okC8iixHAjWGAvErIP5PkgFA9WBbQRphgGgDgBrgtsI046CfAcUDQQYzEW06qkJJIHcmqhCUBzQZrxeA8okwV2E1ACYIVOQLxCCnogQikCsIEgMBmFqcNFAN2DXoCsC6iTEAXSOMj2wAyYEI1CwIMwhEk2wAUE8QsgEsIA7QVJBBjFBcAaQdgFgHiMWBGBd4DZcAGpAHxOeBmFgAT0hwQ8hlAAAgQ90EuGA56gAAAABJRU5ErkJggg=="
               );
            }

            if (Redo == null)
            {
                Redo = TextureUtility.Clone(Undo, true);
            }


            // Add, remove copy
            if (Add == null)
            {
                Add = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAA8SURBVCiRY2RAA////2dkYGD4B+UyMTIy/keWZ0LXQAgMQg2M/////09YGQU2sODQBA9WDCehCwzBeAAAtfcPFoiW37QAAAAASUVORK5CYII="
               );
            }

            if (Duplicate == null)
            {
                Duplicate = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAmElEQVQ4Ea1SQQ6AMAjbjA+b/3+UUpYaJF3ExF0YbSkMbe3vc84zKlH2tsIhiQQudeictDKlbhPsITAFua7TSSky1u0QQx3yHUAkKMgxNkKt8XPSSOSimGcdc7WDRjIaiPt6AmWQMea+A7oTRM479mP3e3nUWvw+gRk9/lA3Y6fgfHd/w8DLJVY+K82lAclK9CWqZ1SKobkAvGOcGDoYiisAAAAASUVORK5CYII="
               );
            }

            if (Delete == null)
            {
                Delete = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAgUlEQVQ4Ed1SWwrAIAzrdoiJXmAIu//1unYYcbUO2acFCaZNWh9EawUzn7I0kj2ZcMeTYb5sru5LASAgIUQAqQi+Q8mhC+qTw8VO2BKOAGaK32IYDUxc8Q7RBG4TNUSD7jq+Rvc6L1MpsJcYHW5soi2a+PWMuRhUMUYUHn8hg1sEb3Lp/7Ea3+5VAAAAAElFTkSuQmCC"
                );
            }



            if (Trash == null)
            {
                Trash = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAtElEQVQ4EbVSSwrCQAwdxMv0hq69gdCFd+kNXAtuXXiP8b0hgZfMVGqhgZLkfZLptKUcEbXWGY/G/NceOtWQe+VObCAIGw2jr0XuAcYTUUXR1lD92U0KOrY5w3zjAMT0y0S+qaDvdCQc9Dpn8o65tl2iN3vy2oC3Dcu526EDnsK+rM6Z8EN0RQdc8H4fI5dRNv6qA0KdLyiQaEa8nqDpIbpnI/s1PGgh8v+By0bRf/8wYUfzBQw7vy/hWrqSAAAAAElFTkSuQmCC"
                );
            }


            if (RightArrow == null)
            {
                RightArrow = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAbElEQVQ4EWNgGP7gPwToke1TqAEgyposQ5AMADGNcRqCphAf1w+rIfh0oMn9AfLLYYYwwRgk0t8x1KPZgov7BSiB4gVGmElACRcYG43ejcQ3YmRkPI/EJ8xEcgpF0ahL2CocKoAuUMQhNUiEAcbOwmp8h6fKAAAAAElFTkSuQmCC"
                );
            }

            if (LeftArrow == null)
            {
                LeftArrow = TextureUtility.Clone(RightArrow, true);
            }

            // Track
            if (TimelineBg == null)
            {
                TimelineBg = TextureUtility.CreateSpecialDegrade(1, 40 - 3, Color.white);
                TimelineBg.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Play == null)
            {
                Play = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABBSURBVCiRtdExCgAgDATB4MdsfLiF/1pLEUPYxusHcpcAFjDChhMHeVPDBNSwADkU4IJNr/PjJF1az6ofN4FuO2+iwRRXVS+qCQAAAABJRU5ErkJggg=="
                );
                //Play.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Stop == null)
            {
                Stop = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAAYSURBVCiRY/z///9/BhIAEymKRzWMJA0AOz0EFPD6CVsAAAAASUVORK5CYII="
                );
            }

            if (ToEnd == null)
            {
                ToEnd = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAgUlEQVQ4EWNgGAXgEPj///9mIJZCDw484l+Acl/g6oEcEHgHxGlwQSADLIpb/D9cLVQhjIK7BiYApTHEcRkAUg92DZoBGOIgA5jgpqAy/gC571GFwDzs4mg2rQLyRUHKCYnDLYAqfAWkQ+GCCANwiaMEItxWNANwiaNGI7KmEcgGALeW/L/esyeIAAAAAElFTkSuQmCC"
                 );
            }

            if (ToStart == null)
            {
                ToStart = TextureUtility.Clone(ToEnd, true);
            }


            if (ToNext == null)
            {
                ToNext = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAgElEQVQ4EWNgGF7gPwQU4PIVUPoaCOOSZ4Aa8A9IzwRiLnSFUPn/6OJwPkwBlL4KpA3gkkAGTB5ZDIUNU4BEfweyy4GYCaQQJo6iCZkDU4CF3oXLALDJyIZQxMZiM0VeAAWiPrKLYBYgi6GwoQoojsZ8FFOROEAL8CckJLVDiAkA+QnpQiHgFwQAAAAASUVORK5CYII="
                );
            }

            if (ToPrevious == null)
            {
                ToPrevious = TextureUtility.Clone(ToNext, true);
            }

            if (Loop == null)
            {
                Loop = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA2klEQVQ4EcVQOwrCQBDNqmDhISxTJQgqeAhbu1h6A2/gcawCHsM7CPZKgljK+l4yuy5DMCSFDsy+N7Nv3n6i6BdhrV30PgfDM6TtZYC5OYcZXw2wn1WqerkBLsi0Lqu1EP4QvAMzb4qilA0C+RW5YqHCGbBdhgZOtweZIhNuAvkEnsbgXyTIHQtGk0Hqm0KgW4di8Jg1g5KBHtC1MeaE3lL3Xd1qQCFMzm4AOA54+w1CcRMfqeYWTzui95L+RPAJHArfCH4AQzk/pWPk3gGD/NlDBwNqY2/wV/IGCj1CAPPD5kwAAAAASUVORK5CYII="
                );
                Loop.hideFlags = HideFlags.HideAndDontSave;
            }


            if (MuteOn == null)
            {
                MuteOn = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAB9SURBVCiRzdIxDgFhGIThZ0WiJAoXEJ3ECRSqDb3eFVxOQekWDqB3ABkFm2wkfqsz5XzzZqb4KCjJPMklyabxeoXwCmfMMCkCSbY4YPR+67dCS5wwKM1sN0y/hT9O+h/ginQGqqo6Yogx9rj/VJ2kTnLLU7uu0OL1GuvGewD+mDPbf3taywAAAABJRU5ErkJggg=="
                );
                MuteOn.hideFlags = HideFlags.HideAndDontSave;
            }


            if (MuteOff == null)
            {
                MuteOff = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACnSURBVCiRhdA7akJREAbgsYzuIZDOxiqVuoBEsBUEIS4jnQtzAzZCSMBVyIU8LJXPwpFcr0f8y8N88zgREYFOFIInrDGsP3bwiV6jeITKKa/NTl18oYcW3nHwn0uQaJBogm+XKYIlZolmDVQEK+zwlmh684YaUEObnFTdA/CbxRvM0S+BRXb7KaDnK1CD52/dN9a7jRK+YIs/jPGB9j30mLcN8BARcQROWjEdkTKWbAAAAABJRU5ErkJggg=="
              );
                MuteOff.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Metronome == null)
            {
                Metronome = TextureUtility.CreateFromBase64("iVBORw0KGgoAAAANSUhEUgAAAAoAAAAMCAYAAABbayygAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAADUSURBVBiVddExK4VhGMbx3zmdHKtRkTIpEynFa7Iok7LYjOooi89g9SmsZ/IJTimTlOGkTEQUO4vL8j7O2yv/eobnuu/nvq7uRxL1WU0yTnKdZKmhS6Jrwj5GGGNXm8arUZKVJFWSYWtir5ME+njELKZwjwXM4QTzxXoNN/jGJ56xiAOc4qlXN1Z1vsIVtrGJAYYlw2Wdrdz3knwkWS+aJJ0kL0n6jcbzJHft9SzjAV+17THeMI2ZkqWLLbxjB0c4xC1esdHc40X+56xpXf35hQm/tR+YpL4l3BuSTQAAAABJRU5ErkJggg==");
                Metronome.hideFlags = HideFlags.HideAndDontSave;
            }


            if (TrackArrow == null)
            {
                TrackArrow = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAYAAAAICAYAAADaxo44AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABMSURBVAiZhYmxCYBAEMByz+MgFi7nJu9gFm5g4Q5i//lGsDkwVUhClYSSxd/Rk94LsCWjoVZ19+NQJwDUWb3VR10AKkBEXOr6+gkwAPIfL+kXBhVFAAAAAElFTkSuQmCC"
                );
                TrackArrow.hideFlags = HideFlags.HideAndDontSave;
            }


            // Note edit

            if (AddNote == null)
            {
                AddNote = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAApElEQVQ4EWNgoDb4//+/KhCDwD9izGYiRhE+NUPMAGC4vAZhZC+xIHOIYIugqyEqDKA2/4dpBvJBAOwSogyAacRGY/MCI7pCRkZGUZAYyFoQDeRjqAFJSgDxUiD+BsQggJGQgGIYgQgyEKRZAIhvADEy+AmWJEDAwiAJqE4dTe01ND5WLswA9Oj5A1RdilUHNkGgu2WA+BgQfwTifUBshU0dTcQA3jds1IxyolkAAAAASUVORK5CYII="
                );
                AddNote.hideFlags = HideFlags.HideAndDontSave;
            }


        }
    }
}
