using UnityEngine;

namespace RhythmosEditor
{
    internal static class Textures
    {
        public static Texture2D Rhythmos;
        public static Texture2D Pixel { private set; get; }

        public static Texture2D Add { private set; get; }
        public static Texture2D Copy { private set; get; }
        public static Texture2D Delete { private set; get; }

        public static Texture2D Undo { private set; get; }
        public static Texture2D Redo { private set; get; }
        public static Texture2D LeftArrow { private set; get; }
        public static Texture2D RightArrow { private set; get; }

        public static Texture2D TimelineBg { private set; get; }
        public static Texture2D Play { private set; get; }
        public static Texture2D Stop { private set; get; }
        public static Texture2D ToStart { private set; get; }
        public static Texture2D ToEnd { private set; get; }
        public static Texture2D MuteOn { private set; get; }
        public static Texture2D MuteOff { private set; get; }
        public static Texture2D Loop { private set; get; }
        public static Texture2D Metronome { private set; get; }

        public static Texture2D TrackArrow { private set; get; }

        static Textures()
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

            // Add, remove copy
            if (Add == null)
            {
                Add = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAA8SURBVCiRY2RAA////2dkYGD4B+UyMTIy/keWZ0LXQAgMQg2M/////09YGQU2sODQBA9WDCehCwzBeAAAtfcPFoiW37QAAAAASUVORK5CYII="
               );
                Add.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Copy == null)
            {
                Copy = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAAX0lEQVQoz5WSwQ3AIAwDE4SE2KL7ZP95ro+WKk1VIPczFjixUHEASAYujplfJInujKGqOhKqP/gbw+sSzAbYctFd/VoasGHebdk0AehBN3f5IUb2+PKnxVWt0a/ZL3ECaKF4DsFD7e0AAAAASUVORK5CYII="
               );
                Copy.hideFlags = HideFlags.HideAndDontSave;
            }


            if (Delete == null)
            {
                Delete = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAA0AAAANCAYAAABy6+R8AAAATElEQVQoz2NkQAP/////jy7GyMjIyIAPoGvCagguCVwAbiuxmmDqGPE5g6DfCGlClmfCJYlOIwMmBjLAwGqagIMmnBLwyTMSqxE5jgDowj2TcM5HmAAAAABJRU5ErkJggg=="
                );
                Delete.hideFlags = HideFlags.HideAndDontSave;
            }

           

            if (Undo == null)
            {
                Undo = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAAAoAAAAMCAYAAABbayygAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAADBSURBVBiVhdChSkRxEMXh89+yRQTbsiwmX0EMght8A5vBqsm+xWLZpxB8AtlkUtAgaDEZTPsKK4JFVz/D3gvL1atTZjjzG2bmJI3AEAdNfRkoGGGOCbbbwAGeLeK9yrfoJ0lpwKtJzpPcJ3lIcpbkI8lm2wnrVd3DDCetNy8NjvHU+ZdMHpNsdLCG7h/gSpLXgrsk01LKD+9QklwnmQV7lXf7vzxV+7pVi6f4wgWOcIgrfOK4uWYXN3jDCy6xU/e/AYWqrU8AntDYAAAAAElFTkSuQmCC"
               );
                Undo.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Redo == null)
            {
                Redo = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAoAAAAMCAYAAABbayygAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAADCSURBVBiVhdC/LsRREMXxc5FIyG4tISIeQxQiodFQqzSUStuJwpvoV6VRKHiArUS8goIEjeCjcH+JXP+mmcyZb+bMTNIEdrDe6i20jFO8YoDSArO49BkvNd9grmPG0UtylaSfZCvJbZKnJBullLuv0w5xj5laz3+zrI1rHP+5fJKxJItJRv+BE0kek0z/BmAyyVTBMEkvyVopxQ/gSZKFYAlvOGiPwHb96WYn7OMd59jFHoZVO2otVnCGBzzjAqtd/wOA/6mnL97oLwAAAABJRU5ErkJggg=="
                );
                Redo.hideFlags = HideFlags.HideAndDontSave;
            }

            if (RightArrow == null)
            {
                RightArrow = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAKCAYAAACALL/6AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABuSURBVBiVnZDBCYNAEEXfBq9agmAN1uDRNGQDHqzApoT0kFQQ8CTPQzyIuLLmwVzm82f+DERQB7WI6WeG11blHYPqR633Qgpf9QkQVBNTLkD3SD7qx3w7UgY0kWkjUAFvoA0hTJe7/3lrr+bH/gq+a5Ea4pRcagAAAABJRU5ErkJggg=="
                );
                RightArrow.hideFlags = HideFlags.HideAndDontSave;
            }

            if (LeftArrow == null)
            {
                LeftArrow = TextureUtility.CreateFromBase64(
                   "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAKCAYAAACALL/6AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABqSURBVBiVpdC9CcIAFEXha2pdINlBcAo3ySZC9skasXEBwQ2sLT4LmxBMeJBTvncP7ydZgCNuy/pf0OKORyV8xsuPbQFXvBUIenwqYWhKx+1Z6TCTLknGJF2SZ5K+MqnFVHrrTDphWOt/AXlqu+IyRuI9AAAAAElFTkSuQmCC"
                );
                LeftArrow.hideFlags = HideFlags.HideAndDontSave;
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
                Play.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Stop == null)
            {
                Stop = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAAYSURBVCiRY/z///9/BhIAEymKRzWMJA0AOz0EFPD6CVsAAAAASUVORK5CYII="
                );
                Stop.hideFlags = HideFlags.HideAndDontSave;
            }

            if (ToStart == null)
            {
                ToStart = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAkAAAAICAYAAAArzdW1AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABjSURBVBiVjc4hDsJQFETRSYNA41kBipXhGpbQLSCrkXXdFLIGcipakh/yBNfOzczExpgCXDClknDGA284/ISnJH2SW5JjG8ATd7wUdLv7SbJUv9qm8TuHAUvb9NfxUmrkK+YV6aaQbZ/wnpEAAAAASUVORK5CYII="
                    );
                ToStart.hideFlags = HideFlags.HideAndDontSave;
            }

            if (ToEnd == null)
            {
                ToEnd = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAkAAAAICAYAAAArzdW1AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABVSURBVBiVjdCxDYNAAATBEyJwBTRABaY6t0JICh3YRTl3wBCglwhebzZe6VYXfDClAhaIkx0rxpZU+GHG0JIKX7ywQVdrSdIneVx3b8/9DX/j2brgAPhAqQwaUiHCAAAAAElFTkSuQmCC"
                    );
                ToEnd.hideFlags = HideFlags.HideAndDontSave;
            }

            if (Loop == null)
            {
                Loop = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAKCAYAAACALL/6AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAN1wAADdcBQiibeAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACjSURBVBiVrdA9agJxEAXw34qw5eYAghcQb5EiYCCYzhuky0EschFTCfa5g/1ewSIka/NS+BdkWdYmDx7DvJnHfFRJ9PCEb3z1CzDFHGvURVviBa844A0NOnxKcswwfpLMkrQ32rHKZacTPsqEBZ6xwQ7veCixUZxtEoWrJI83+ZVtkkwH7toPHXvFZKz4L4Yqya/LS7s7vTW6CbY4F2GMZ2z/AD4KcbR6O5QHAAAAAElFTkSuQmCC"
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

           

        }
    }
}
