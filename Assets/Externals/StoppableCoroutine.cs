using System.Collections;
using UnityEngine;

namespace StoppableCoroutines {
	public class StoppableCoroutine {
		bool terminated = false;
		IEnumerator payload;
		Coroutine nested;
		MonoBehaviour mb;
		public StoppableCoroutine(MonoBehaviour mb, IEnumerator aCoroutine) {
			payload = aCoroutine;
			nested = mb.StartCoroutine(wrapper());
			this.mb = mb;
		}
		public Coroutine WaitFor() {
			return mb.StartCoroutine(wait());
		}
		public void Stop() {
			terminated = true;
			mb.StopCoroutine(nested);
		}
		private IEnumerator wrapper() {
			while (payload.MoveNext())
				yield return payload.Current;
			terminated = true;
		}
		private IEnumerator wait() {
			while (!terminated)
				yield return null;
		}
	}

	public static class MonoBehaviourExtension {
		public static StoppableCoroutine StartStoppableCoroutine(this MonoBehaviour mb, IEnumerator coroutine) {
			return new StoppableCoroutine(mb, coroutine);
		}

	}
}