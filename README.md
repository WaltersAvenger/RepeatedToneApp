# RepeatedToneApp
This WinForms app is for repeating a custom tone in order to keep KRK speakers from idling from playing quiet sound,

There are 4 sliders and a stop and start button on the application:
  -Interval between tones in seconds.
      -Min:5
      -Max:1800
  -Frequency of tone in hZ
      -Min:10
      -Max:24000
  -Amplitude on a scale of 0-20
      -Min:0
      -Max:20
  -Sound duration in seconds
      -Min:1
      -Max:30
  -Start button will start the tone.
  -Stop button will stop the tone.

If a tone's settings are changed while in the middle of a loop or delay, the settings will be applied on the next loop of the tone.
The settings can be immediately applied if stopping then starting the tone again.

THe application can be exited out of which will minimize it to taskbar. To bring the application back up, double click 
the icon in the taskbar. To fully exit, right click the icon and hit exit.
