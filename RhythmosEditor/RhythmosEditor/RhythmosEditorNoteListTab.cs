//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;

namespace RhythmosEditor
{
			
	internal class RhythmosEditorNoteListTab {
		
		
		private static List<NoteLayout> entryList;
		
		[SerializeField]
		private UndoNoteLayoutManager _undoManager;
		
		private int _selected = -1;
		private int _lastselected = -1;
		float vSbarValue = 0;
		
		public RhythmosEditorNoteListTab() {
			
			if (entryList == null)
				entryList = new List<NoteLayout>();
				
			if (_undoManager == null)
				_undoManager = new UndoNoteLayoutManager();
			else
				_undoManager.Clear();
			
			
		}
		
		//Public functions
		public void AddEntry(NoteLayout entry) {
			entryList.Add(entry);
		}
		
		public void RemoveEntry(NoteLayout entryToRemove) {
			entryList.Remove(entryToRemove);
		}
		
		public void LoadList(List<NoteLayout> ListToLoad) {
			entryList = ListToLoad;
		}
		
		public void Clear()	{
			entryList.Clear();
		}
		
		public void ReinsertEntry(NoteLayout entry, int index) {
			
			entryList.RemoveAt(index);
			entryList.Insert(index,entry);
			
		}
		
		public void DeleteAt(int index) {
			
			if (entryList.Count != 0) {

				entryList.RemoveAt(index);
				
				
			} else {
				
				_selected = -1;
				
			}
		}
		
		public int GetSelectedItem() {
			
			return _selected;
			
		}
		
		public bool SelectionChanged() {
			
			if (_lastselected != _selected)
				return true;
			else
				return false;
			
		}
		
		public List<NoteLayout> GetList() {
			
			return entryList;
			
		}
		
		public static List<NoteLayout> List {
			
			get {return entryList;}
			set {entryList = value;}
			
		}
		
		public void ClearUndoRedo() {
		
			_undoManager.Clear();
		
		}
		
		public XmlElement CreateXMLElement(ref XmlDocument xmlDoc) {
			
			XmlElement elmNoteList = xmlDoc.CreateElement("NoteList");
			
			foreach (NoteLayout nt in entryList) {
				
				string audioStr = "0";
				if (nt.Clip != null)
					audioStr = AssetDatabase.GetAssetPath(nt.Clip);
				
				XmlElement elmNoteEntry = xmlDoc.CreateElement("NoteEntry");
				
				XmlNode node0 = XMLUtility.CreateNodeByName(ref xmlDoc,"name",nt.Name);
				//XmlNode node1 = XMLUtility.CreateNodeByName(ref xmlDoc,"id",nt.m_id.ToString());
				XmlNode node2 = XMLUtility.CreateNodeByName(ref xmlDoc,"color",nt.Color.ToString());
				XmlNode node3 = XMLUtility.CreateNodeByName(ref xmlDoc,"audioclip",audioStr);
				
				elmNoteEntry.AppendChild(node0);
				//elmNoteEntry.AppendChild(node1);
				elmNoteEntry.AppendChild(node2);
				elmNoteEntry.AppendChild(node3);
				
				elmNoteList.AppendChild(elmNoteEntry);
				
			}
			
			return elmNoteList;
			
		}
		
		public void LoadXMLElement(ref XmlDocument xmlDoc) {
			
			XmlNodeList noteList = xmlDoc.GetElementsByTagName("NoteEntry");
			
			entryList.Clear();
			
			foreach (XmlNode node in noteList) {
				
				Dictionary<string,string> subDictionary = new Dictionary<string,string>();
				XmlNodeList noteContet = node.ChildNodes;
				
				foreach (XmlNode noteItens in noteContet) {
					
					if(noteItens.Name == "name" || noteItens.Name == "audioclip" || noteItens.Name == "color") {
						subDictionary.Add(noteItens.Name,noteItens.InnerText);
					}
					
				}
				
				if (subDictionary.Count == 3) {
					
					NoteLayout nt = new NoteLayout();
					
					foreach (KeyValuePair<string,string> entry in subDictionary) {
						
						if (entry.Key == "name")
							nt.Name = entry.Value;
						else if (entry.Key == "color")
							nt.Color = ColorUtility.ParseColor(entry.Value);
						else if (entry.Key == "audioclip") {
							
							if (entry.Value == "0")
								nt.Clip = null;
							else {
								
								AudioClip a = (AudioClip)AssetDatabase.LoadAssetAtPath(entry.Value, typeof(AudioClip));
								
								if (a != null)
									nt.Clip =  a;
								else 
									nt.Clip = null;
							}
						}
						
						//else if (entry.Key == "id")
						//nt.m_id = int.Parse(entry.Value);
						
						
						
					}
					AddEntry(nt);
				}
				
				
			}
			
		}
		
		void PerformUndo() {
		
			UndoNoteLayout undo = _undoManager.Undo();
			
			if (undo.undoAction != "none") {
				
				if (undo.undoAction == "New Note Layout") {

					if (entryList.Count != 0) {
						entryList.RemoveAt(undo.index);
					}
					
				} else if (undo.undoAction == "Remove Note Layout") {
					
					entryList.Insert(undo.index,undo.note);
					
				} else {
					
					
					entryList[undo.index] = undo.note;
					
				}
				
				_undoManager.RecordNoteRedo(undo.note,undo.undoAction,undo.index);
				_undoManager.RemoveLastUndo();
				
			}
		
		}
		
		void PerformRedo() {
			
			UndoNoteLayout redo = _undoManager.Redo();
			
			if (redo.undoAction != "none") {
				
				if (redo.undoAction == "New Note Layout") {
					
					entryList.Insert(redo.index,redo.note);
					
					
				} else if (redo.undoAction == "Remove Note Layout") {
					
					if (entryList.Count != 0) {
						entryList.RemoveAt(redo.index);
					}
					
				} else {

					entryList[redo.index] = redo.note;
					
				}
				
				_undoManager.RecordNote(redo.note,redo.undoAction,redo.index, false);
				_undoManager.RemoveLastRedo();
				
			}
			
		}
		
		public void DrawHeader(Rect Area, ref Texture2D undo, ref Texture2D redo) {
			
			GUILayout.BeginArea(new Rect(Area.x,Area.y,Area.width,Area.height),"");
			
			
			GUILayout.BeginHorizontal();
			
			if (_undoManager.UndoCount == 0)
				GUI.enabled = false;
			else
				GUI.enabled = true;
			
			if (GUILayout.Button(new GUIContent(undo,"Undo"),GUILayout.Width(24))) {
				PerformUndo();
			}
			
			if (_undoManager.RedoCount == 0)
				GUI.enabled = false;
			else
				GUI.enabled = true;
				
			
			
			if (GUILayout.Button(new GUIContent(redo,"Redo"),GUILayout.Width(24))) {
				PerformRedo();
			}
			
			GUI.enabled = true;
			
			if (GUILayout.Button("New Note Layout",GUILayout.Width(120))) {
				
				entryList.Add(new NoteLayout("New Note " + (entryList.Count+1)));
				_undoManager.RecordNote(entryList[entryList.Count-1],"New Note Layout",entryList.Count-1,true);
				vSbarValue = entryList.Count;
				
			}

			GUILayout.EndHorizontal();
			
			GUILayout.EndArea();
			
		}
		
		public void Draw(Rect Area, float ItemHeight, Color BackgroundColor, Color SelectedItemColor) {
			
			float _y = 32;
			NoteLayout _s;
			float totalsize = 0;
			Rect listBox = new Rect(10, Area.y + 16 + 1, Area.width-5, Area.height-32-2);
			
			
			GUI.color = BackgroundColor;
			
			
			float entryWidth = 0;
			totalsize = (ItemHeight * entryList.Count);
			_y = -(ItemHeight *vSbarValue)+5;
			
			if (totalsize >= Area.height-32) 
				entryWidth = Area.width-30;
			else 
				entryWidth = Area.width-15;
			
			GUILayout.BeginArea(new Rect(5,Area.y,entryWidth,16), "");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("ID",EditorStyles.boldLabel,GUILayout.Width(entryWidth*0.05f));
			GUILayout.Label("Name",EditorStyles.boldLabel,GUILayout.Width(entryWidth*0.35f));
			GUILayout.Label("Color",EditorStyles.boldLabel,GUILayout.Width(entryWidth*0.1f));
			GUILayout.Label("Audio Clip",EditorStyles.boldLabel,GUILayout.Width(entryWidth*0.3f));
			GUILayout.Label("Remove",EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();		
			GUILayout.EndArea();
			
			GUI.Box(new Rect(5,Area.y + 16,Area.width, Area.height-32), "");
			
			//Draw the listbox.
			GUI.BeginGroup(listBox, "");
			
			GUI.color = Color.black;
			
			GUI.color = Color.white;
			
			
			// entryList.Count*((Area.height-16-50)/totalsize);
			
			//iinit = //(int)(Mathf.Lerp(0,entryList.Count,vSbarValue));
			//iinit+(int)(Mathf.Ceil(((Area.height-16-50)/totalsize)*entryList.Count));
			
			//if (imax > entryList.Count)
			//	imax = entryList.Count;
			
			
			//Loop through to draw the entries and check for selection.
			for(int i = 0;/*(int)Mathf.Ceil(vSbarValue)*/ i < entryList.Count; i++) {
				//Get the list entry's name
				_s = entryList[i];
				
				//Get the selection's area.
				Rect _entryBox;
				
				
				_entryBox = new Rect(0, _y, entryWidth, ItemHeight);
				
	
				
				GUILayout.BeginArea(_entryBox,"");
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(i.ToString() + ".",GUILayout.Width(_entryBox.width*0.05f));
				EditorGUI.BeginChangeCheck();
				string name = EditorGUILayout.TextField(_s.Name,GUILayout.Width(_entryBox.width*0.35f));
				if (EditorGUI.EndChangeCheck()) {
					_undoManager.RecordNote(_s,"Set Name",i,true);
					_s.Name = name;
					ReinsertEntry(_s,i);
					
				}
				EditorGUI.BeginChangeCheck();
				Color color = EditorGUILayout.ColorField(_s.Color,GUILayout.Width(_entryBox.width*0.1f));
				if (EditorGUI.EndChangeCheck()) {
					_undoManager.RecordNote(_s,"Set Color",i,true);
					_s.Color = color;
					ReinsertEntry(_s,i);
				}
				EditorGUI.BeginChangeCheck();
				AudioClip clip = (AudioClip)EditorGUILayout.ObjectField(_s.Clip,typeof(AudioClip),false,GUILayout.Width(_entryBox.width*0.3f));
				if (EditorGUI.EndChangeCheck()) {
					_undoManager.RecordNote(_s,"Set Clip",i,true);
					_s.Clip = clip;
					ReinsertEntry(_s,i);
				}
				
				if (GUILayout.Button("Remove")) {
					_undoManager.RecordNote(_s,"Remove Note Layout",i,true);
					DeleteAt(i);
				}
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
				

					
				
				_y += ItemHeight;
			}
			
			
			
			
			if (totalsize >= Area.height-32) {
				float viewableRatio = ((Area.height-32-2)/totalsize);
				vSbarValue = GUI.VerticalScrollbar(new Rect(Area.width-21, 0, 30, Area.height-32-2), vSbarValue,entryList.Count*viewableRatio,0,entryList.Count);
			} else {
				vSbarValue = 0;
			}
			
			
			if (Event.current != null) {
				if(Event.current.type == EventType.ScrollWheel) {
					
					if (listBox.Contains(Event.current.mousePosition)) {
						if (Event.current.delta.y > 0) {
							
							vSbarValue += 1;
							
							if (vSbarValue >= entryList.Count)
								vSbarValue = entryList.Count;
							
						} else if (Event.current.delta.y < 0) {
							
							vSbarValue -= 1;
							
							if (vSbarValue <= 0)
								vSbarValue = 0;
							
						}
					}
					
				}
			}
			
			GUI.EndGroup();
			
			
			GUI.color = Color.white;

			
		}
		
	}
	
	}
