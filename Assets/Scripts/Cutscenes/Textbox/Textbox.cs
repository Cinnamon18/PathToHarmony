using CharTween;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.Textboxes {
	public class Textbox : MonoBehaviour {
		private const char ZERO_WIDTH_SPACE = '​';

		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		private Image box;

		private List<Tween> currentEffects = new List<Tween>();

		private IDictionary<TextEffect, MatchCollection> effectSubstrings 
			= new Dictionary<TextEffect, MatchCollection>();

		public void AddText(string speaker, string message) {
			ResetDictionary();

			string line = string.Empty;

			if (!string.IsNullOrEmpty(speaker)) {
				line =  speaker + ": " + message;
			} else {
				line = message;
			}

			char[] chars = message.ToCharArray();

			// parse out tagged strings
			foreach (TextEffect te in TextEffect.All) {
				MatchCollection matches = Util.GetTaggedSubstrings(te.symbol, line);
				foreach (Match match in matches) {
					Debug.Log(match.Value + " : " + te.symbol);
					for (int i = match.Index - 3; i < match.Index; i++) {
						chars[i] = ZERO_WIDTH_SPACE;
					}
					for (int i = match.Index + match.Length; i < match.Index + match.Length + 4; i++) {
						chars[i] = ZERO_WIDTH_SPACE;
					}
				}
				effectSubstrings[te] = matches;
			}

			text.SetText(new string(chars));

			AnimateText();
		}

		public void AnimateText() {
			CharTweener foo = text.GetCharTweener();
			foreach (KeyValuePair<TextEffect, MatchCollection> pair in effectSubstrings) {
				foreach (Match match in pair.Value) {
					for (int i = match.Index; i < (match.Index + match.Length); i++) {

					var timeOffset = Mathf.Lerp(0, 1, i / (float)(foo.CharacterCount - 1));

					Tween tween = pair.Key.DoEffect(i, foo);
					currentEffects.Add(tween);

					tween.fullPosition = timeOffset;
				}
				}
			}
		}

		private void Start() {
			AddText("<r>Rainbow</r><w>Wavy</w><r><w>Both</w></r>");
		}

		public void AddText(string message) {
			foreach (Tween tween in currentEffects) {
				Debug.Log("killing" + tween.ToString());
				tween.Kill();
			}
			currentEffects.Clear();
			AddText(string.Empty, message);
		}

		private void ResetDictionary() {
			effectSubstrings.Clear();
			foreach (TextEffect te in TextEffect.All) {
				effectSubstrings.Add(te, null);
			}
		}
	}
}
