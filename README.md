# AUTODARTS-DESKTOP

Autodarts-desktop manages several extension-tools for https://autodarts.io.
It handles download, setup, use and version-check.
 - You can easily start tools like caller or extern to transfer thrown darts to other web-dart-platforms.
 - Desired tools can be downloaded in the download-area.
 - The administration of app-parameters is simplified by setup windows - no more console/batch-file handling. =)

## Presentation
![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/Main.png?raw=true)


## Examples
autodarts-caller (Setup-page):
 - calls your thrown score/ possible checkouts accordingly to the state of an https://autodarts.io match.
 ![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/SetupCaller.png?raw=true)
  - for more info about autodarts-caller visit https://github.com/lbormann/autodarts-caller

Autodarts-extern (Setup-page):
 - automates multiple dart-web-platforms accordingly to the state of an https://autodarts.io match.
 ![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/SetupExtern.png?raw=true)
  - for more info about autodarts-extern visit https://github.com/lbormann/autodarts-extern

You can also use other assistant-programs:
 - dartboards-client (for webcam support with dartboards.online)
 - visual-darts-zoom (for an image zoom onto thrown darts)
 ![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/vdz.png?raw=true)
  - for more info about visual-darts-zoom visit https://lehmann-bo.de/
  - for more info about dartboards-client visit https://dartboards.online/


Moreover you can start custom-apps, e.g. OBS (Open Brodcast System) - to compose a customized virtual-camera:

![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/OBS2.png?raw=true)
With OBS it is possible to display the autodarts-match in the virtual camera.


![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/OBS1.png?raw=true)
It is also possible, to integrate a player camera into the view.


![alt text](https://github.com/Semtexmagix/autodarts-desktop/blob/master/OBS3.png?raw=true)
or .. a full screen mode for the board.

All of this can be easily realized via OBS scenes: The scenes can be switched during the game or via hotkeys.
If you start OBS by the custom-app function, you can use "--startvirtualcam" as start-agument so obs automatically starts in visual-camera mode. 


## BUGS

It may be buggy. You can give me feedback in Discord (Autodarts.io) ---> Reepa86


## TODOs
- cross-platform
- autostart + (tray?)
- Add new app: droidcam (android) + epoccam (iOS)


### Done
- alot
- refactor setup-areas for using AppManager
- close custom-app on exit


## LAST WORDS
Thanks to Timo for awesome https://autodarts.io.
Thanks to Wusaaa for the caller and the extern tools and also for the many help =D 
