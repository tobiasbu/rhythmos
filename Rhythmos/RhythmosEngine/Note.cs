using System;

namespace RhythmosEngine
{
	/// <summary>
	/// Represent an musical note.
	/// </summary>
	[Serializable]
	public class Note {
	
		/// <summary>
		/// Defines if this note is a rest
		/// </summary>
		public bool isRest;

		/// <summary>
		/// Duration in seconds of this note
		/// </summary>
		public float duration;

		/// <summary>
		/// ID reference of a  <see cref="RhythmosEngine.NoteLayout"/> 
		/// </summary>
		public int layoutIndex;

		/// <summary>
		/// Initializes a new instance of the <see cref="RhythmosEngine.Note"/>.
		/// </summary>
		public Note()
		{
			duration = 1f;
			isRest = true;
			layoutIndex = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RhythmosEngine.Note"/>.
		/// </summary>
		/// <param name="duration">Note Duration.</param>
		/// <param name="isRest">If set to <c>true</c> is rest.</param>
		/// <param name="layoutIndex">Layout index.</param>
		public Note(float duration, bool isRest, int layoutIndex) {
			this.duration = duration;
			this.isRest = isRest;
			this.layoutIndex = layoutIndex;
		}
		
		/// <summary>
		/// Initializes a new instance of <see cref="RhythmosEngine.Note"/> by cloning another one.
		/// </summary>
		/// <param name="note">Note.</param>
		public Note(Note note) {
			duration = note.duration;
			isRest = note.isRest;
			layoutIndex = note.layoutIndex;
		}

		/// <summary>
		/// Clones this Note instance to new one
		/// </summary>
		/// <returns>Cloned Note instace</returns>
		public Note Clone()
		{
			return new Note(duration, isRest, layoutIndex);
		}

	};
}

