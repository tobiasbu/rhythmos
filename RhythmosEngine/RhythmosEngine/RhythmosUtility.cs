using UnityEngine;

namespace RhythmosEngine
{
	/// <summary>
	/// Rhythm utilities functions
	/// </summary>
	public static class RhythmosUtility
	{
		/// <summary>
		/// Checks if the source Rhythm A matches with a destination Rhythm B according with notes layout.
		/// The note duration is ignored.
		/// </summary>
		/// <returns><c>true</c>, if was matched, <c>false</c> otherwise.</returns>
		/// <param name="rhythmA">Rhythm A</param>
		/// <param name="rhythmB">Rhythm B</param>
		static public bool CheckRhythmsMatch(Rhythm rhythmA, Rhythm rhythmB) { 
			
			bool match = false;	
			if (rhythmA != null) {
				if (rhythmB != null) {
			
					if (rhythmA.Count == rhythmB.Count) { // o ritmo tocado tem o mesmo numero de notas
						for (int i = 0; i < rhythmB.Count; i++) {
						
							bool isPause = rhythmA.Notes[i].isRest;
							if (isPause) {
							
								if (isPause == rhythmB.Notes[i].isRest) {
									match = true;
								} else {
									match = false;
									break;
								}
							
							} else {
		
								if (rhythmA.Notes[i].layoutIndex == rhythmB.Notes[i].layoutIndex) {
									match = true;
								} else {
									match = false;
									break;
								}
							}
		
						}
					}
				} else {
					Debug.LogError("RhythmosEngine: RhythmosUtility: The rhythm B is null.\nImpossible to check rhythms match.");
					return false;
				}
				
			} else {
				Debug.LogError("RhythmosEngine: RhythmosUtility: The rhythm A is null.\nImpossible to check rhythms match.");
				return false;
			}
			
			return match;
			
		}

        /// <summary>
        /// Check if the Source rhythm matches with some of the rhythms from a RhythmosDatabase.
        /// </summary>
        /// <returns> the matched <c>Rhythm</c> from the database, <c>null</c> otherwise.</returns>
        /// <param name="rhythmDatabase">RhythmDatabase to search in</param>
        /// <param name="sourceRhythm">Rhythm to check.</param>
        static public Rhythm CheckDatabaseRhythmMatch(RhythmosDatabase rhythmDatabase, Rhythm sourceRhythm) { 

			if (rhythmDatabase != null) {
				if (sourceRhythm != null) {

					// o map de ritmos tem alguem?
					if (rhythmDatabase.RhythmsCount > 0) {
						
						// loop em torno do map de ritmos
						foreach (Rhythm rtmIt in rhythmDatabase.Rhythms) {
							
							bool matched = false;
							
							// verifica se o ritmo FONTE tem a mesma quantidade de notas que o ritmo DESTINO
							if (sourceRhythm.Count == rtmIt.Count) {
								
								// verifica se o ritmo FONTE tem a mesmas notas do que o DESTINO
								matched = CheckRhythmsMatch(sourceRhythm,rtmIt);
								// encontrou um ritmo!
								if (matched) {
									return rtmIt;
								}						
							} else { // nao tem mesma quantidade de notas
								continue;
							}	
						}
					}
				} else {
					Debug.LogError("RhythmosEngine: RhythmosUtility: The source rhythm is null.\nImpossible to check a match from database.");
					return null;
				}
			
			} else {
			
				Debug.LogError("RhythmosEngine: RhythmosUtility: The RhythmosDatabase is null.\nImpossible to check a match from database.");
				return null;
			}
			
			return null;
			
		}

		/// <summary>
		/// Checks the precision of a source rhythm by a "compare" rhythm.
		/// Failure rate is a percentage from 0.0 to 1.0 for precision value.
		/// </summary>
		/// <returns>The precision between 0.0f ... 1.0f of the rhythm matches. If -1, the compare was unsuccessful.</returns>
		/// <param name="rhythmSource">Rhythm source.</param>
		/// <param name="rhythmCompare">Rhythm compare.</param>
		/// <param name="failureRate">Failure Rate.</param>
		static public float CheckRhythmsPrecision(Rhythm rhythmSource, Rhythm rhythmCompare, float failureRate) {
			
			float result = 0;
			if (rhythmSource != null) {
				if (rhythmCompare != null) {
			
						float bpsec = 60f/rhythmCompare.BPM;
						float rightness = 0;
						float timeRtm = 0;
						float timeUsr = 0;
						
						for (int i = 0; i < rhythmCompare.Count; i++) {
			
							// pega a duracao da nota pelo BPM
							float noteByBPM = bpsec * rhythmCompare.Notes[i].duration;
			
							// ponderacao desta nota
							float pdrc = noteByBPM*failureRate;
							
							if (i >= rhythmCompare.Count-1) {
								
								noteByBPM = bpsec*rhythmCompare.Notes[i-1].duration;
								pdrc = noteByBPM*failureRate;
								
								if (timeUsr > timeRtm-pdrc && timeUsr < timeRtm+pdrc) {
									float por = rhythmSource.Notes[i-1].duration/noteByBPM;
									rightness += por;
								}	
							} else {
								timeRtm += noteByBPM;
								timeUsr += (bpsec*rhythmCompare.Notes[i].duration);
								// se esta dentro da ponderacao
								if (rhythmSource.Notes[i].duration > noteByBPM-pdrc && rhythmSource.Notes[i].duration < noteByBPM+pdrc) {
									float por = rhythmSource.Notes[i].duration/noteByBPM;
									rightness += por;
								}
							}
						
						}

					result = rightness;
					result = result/(float)rhythmCompare.Notes.Count;
					if (result > 1f)
					{
						result = 1f;
					}
					else if (result < 0f)
					{
						result = 0f;
					}
						
			
				} else {
					Debug.LogError("RhythmosEngine: RhythmosUtility: The Compare Rhythm is null.\nImpossible to compare.");
					return -1;
				}
			
			} else {
				Debug.LogError("RhythmosEngine: RhythmosUtility: The Source Rhythm is null.\nImpossible to compare.");
				return -1;
			}
			
			return result;
			
		}
		
	}

}


