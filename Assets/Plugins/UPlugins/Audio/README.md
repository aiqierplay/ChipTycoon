# AudioManager

## Auido Group 默认保留分组
### Master
主音量组，一般情况下仅用于控制全局音量，不用于播放声音。

### BGM - Background Music
循环背景音乐。

### BGS - Background Sound
与BGM一起播放的环境音: 水声，风声，瀑布声等。

### SE - Sound Effect
短音效，比如武器打击，技能效果，UI音效。

### ME - Music Effect
长音效，播放时BGM暂时停止，比如游戏开始，胜利，失败的音效。

### Voice
人声，语音。

## 音量控制
每个组拥有独立的音量，Master组的音量会影响所有组的音量。

## 结构
# AudioManager - AudioMixer
音频管理器，用于统一管理所有 `AudioGroup`，提供快速调用API。

# AudioGroup - AudioMixerGroup
音频组，用于管理一组 `AudioAgent`，对应同一个输出频道，可以统一控制音量和播放状态。

# AudioAgent - AudioSource
音频代理，用于控制单个音源的音量、播放状态。

## API
* Audio.cs : 不通过 `AudioManager` 快速调用音频管理接口
* TransformExtension.cs : 通过 `Transform` 游戏对象本身来快速调用绑定到对象的音频管理接口
* AudioClipExtension.cs : 通过 `AudioClip` 音频资源本身快速调用播放接口