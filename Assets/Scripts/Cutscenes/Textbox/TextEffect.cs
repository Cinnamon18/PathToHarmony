using CharTween;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes.Textboxes {
	public sealed class TextEffect {

		private static readonly List<TextEffect> _allEffects = new List<TextEffect>();

		public static readonly TextEffect Rainbow = new TextEffect('r', 
			(i, c) => {
				return DOTween.Sequence()
				 .Append(c.DOColor(i, Color.magenta, 0)) // ensures there's no white flash at start
				 .Append(c.DOColor(i, Color.red, 0.35f))
				 .Append(c.DOColor(i, Color.yellow, 0.35f))
				 .Append(c.DOColor(i, Color.green, 0.35f))
				 .Append(c.DOColor(i, Color.blue, 0.35f))
				 .Append(c.DOColor(i, Color.magenta, 0.35f))
				 .SetLoops(-1, LoopType.Restart)
				 .SetEase(Ease.Linear);
			});

		public static readonly TextEffect Wave = new TextEffect('w',
			(i, c) => {
				return c.DOCircle(i, 5.0f, 0.5f)
							.SetEase(Ease.Linear)
							.SetLoops(-1, LoopType.Restart);
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