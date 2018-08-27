//Fun fact - did you know you can't yield lambdas in C#?
//This is the horrible, horrible workaround I've cooked up
//Please don't hurt me.

using System.Collections.Generic;

namespace Cutscenes {
	public class CutsceneScript {
		public List<CutsceneScriptLine> script;
		public CutsceneScript(List<CutsceneScriptLine> script) {
			this.script = script;
		}
	}
}