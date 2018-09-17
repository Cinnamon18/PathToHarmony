# Path to Harmony
A fantasy turn based tactics game, developed with Georgia Tech's VGDev club.

## Installation
* You may need [blender](https://www.blender.org/) installed to view the models. Will update to confirm if this is an issue.

## Style/Convention suggestions
* Try to keep lines to a reasonable length, ~120 characters.
* I use [VSCode](https://code.visualstudio.com/docs/other/unity) as my editor.
  * If your editor uses omnisharp there's a config file in the repo to keep styles nice and consistent. The key chord is alt + shift + f in vscode
* Read through the `Util` class before reimplementing a common function/field
  * There are some instances of axes being swapped. `Util.WorldToGrid` and `Util.GridToWorld` handles this. Sorry. I would blame blender but it's 100% my fault
