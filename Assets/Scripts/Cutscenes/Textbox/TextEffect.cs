using CharTween;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes.Textboxes {

	/// <summary>
	/// Various text effects using a char tag parser. Acts like an enum.
	/// </summary>
	public sealed class TextEffect {

		private static readonly List<TextEffect> _allEffects = new List<TextEffect>();

		public static readonly TextEffect Rainbow = new TextEffect('r',
			(i, c) => {
				return DOTween.Sequence()
				 .Append(c.DOColor(i, Color.magenta, 0.0f)) // ensures there's no white flash at start
				 .Append(c.DOColor(i, Color.red, 0.35f))
				 .Append(c.DOColor(i, Color.yellow, 0.35f))
				 .Append(c.DOColor(i, Color.green, 0.35f))
				 .Append(c.DOColor(i, Color.cyan, 0.35f))
				 .Append(c.DOColor(i, Color.blue, 0.35f))
				 .Append(c.DOColor(i, Color.magenta, 0.35f))
				 .SetLoops(-1, LoopType.Restart)
				 .SetEase(Ease.Linear);
			});

		public static readonly TextEffect Wave = new TextEffect('w',
			(i, c) => {
				return c.DOCircle(i, 3f, 0.5f)
							.SetEase(Ease.Linear)
							.SetLoops(-1, LoopType.Restart);
			});

		public static readonly TextEffect Shake = new TextEffect('s',
			(i, c) => {
				Tweener t = c.DOCircle(i, 3.25f, 0.07f, 3)
							.SetEase(Ease.Linear)
							.SetLoops(-1, LoopType.Restart);
				t.fullPosition += UnityEngine.Random.value;
				return t;
			});

		public readonly char symbol;
		public readonly Func<int, CharTweener, Tween> DoEffect;

		private TextEffect(char symbol, Func<int, CharTweener, Tween> perTick) {
			this.symbol = symbol;
			this.DoEffect = perTick;
			_allEffects.Add(this);
		}

		public static IEnumerable<TextEffect> All {
			get {
				return _allEffects.AsReadOnly();
			}
		}

		public override bool Equals(object obj) {
			var effect = obj as TextEffect;
			return effect != null &&
				   symbol == effect.symbol;
		}

		public override int GetHashCode() {
			return 1691320185 + symbol.GetHashCode();
		}
	}
}