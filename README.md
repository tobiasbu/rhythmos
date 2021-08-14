# rhythmos-engine

_A simple rhythm tool for Unity_

![RhythmosEngine v1.3](Docs/images/rhythmos.png)

## Table of contents

- [Quick start](#quick-start)
- [Development](#development)
- [About](#about)
- [License](#license)


## Quick start

To start using Rhythmos Engine download the last builds below.

To access documentation [ click here](https://tobiasbu.github.io/rhythmos/index.html).

### Last releases


| Version | Supported Unity version |
| ------- | ----------------------- |
| [v1.3](https://github.com/tobiasbu/rhythmos-engine/releases/tag/v1.3) | >= 2020.3.13f1 |
| [v1.1](https://assetstore.unity.com/packages/tools/audio/rhythmos-engine-39835) | 3.6, 4.6.0f, 5 and 2017  |

For installation instructions access the [Installation Guide here](https://tobiasbu.github.io/rhythmos/articles/downloads-installation-guide.html).


## Development

### Linux - Documentation

1. Install [Mono](https://www.mono-project.com/download/stable/) and [Nuget.exe](https://www.nuget.org/downloads). 

2. Install DocFX: 

```bash
 mono nuget.exe install docfx.console
```

3. Build documentation

```bash
mono PATH/TO/docfx.console/tools/docfx.exe Docs/docfx.json
# serve
mono PATH/TO/docfx.console/tools/docfx.exe Docs/docfx.json --serve
```

## About

This project was created for "Games Project:  Games Engines" class in 2015 at [Unisinos](http://www.unisinos.br/global/en/). It was especially made for my game Koko-Kuba. Since the asset is totally free I decided to make the code open-source. 

I recently refactored all the code of this plugin and if you need a fix, update, feature, feel free to contribute to this repo. See the [roadmap](https://github.com/tobiasbu/rhythmos-engine/projects) for more information.

## License

Created by [Tobias Ulrich](https://github.com/tobiasbu) and license based in [MIT](https://github.com/tobiasbu/rhythmos-engine/blob/main/LICENSE).

