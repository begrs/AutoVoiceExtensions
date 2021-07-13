# AutoVoiceExtensions "AVE"
VSCode Extension and .net 5 Server to use voice commands with no configurations required.
Based on https://github.com/ligershark/VoiceExtension by madskristensen.

The extension will support two modes:
* continuous commands e.g. "go to FILENAME" or "run COMMAND" where "run" and "go" are the keywords the extension recognizes
  * the user is in a voice session and wants to run multiple commands as fluently as possible
  * the user is not expected to use his voice for other tasks and will disable continuous mode to e.g. talk to a college
* direct commands e.g. "AVE run COMMAND"
  * the user is expected to use his voice for other tasks in between commands
  * it is important that regular speech does not trigger unwanted commands



# Info
VSCode Extension and .net 5 Server to use voice commands with no configurations required.

Similar to the project https://github.com/b4rtaz/voice-assistant but actually based on the simple and awesome extension of Visual Sudio https://github.com/ligershark/VoiceExtension by madskristensen.

I used the VS extension by madskristensen during a time where my hand movement was painful and after that I kind of missed the ease of access to many features of VS normally hidden in menus and submenus. VS Code has a much better approch from the get-go with the CRTL-P/CTRL-Shift-P command line.
In this project I want to bring voice to this command line and also stream line the workflow of madskristensen voice extension a bit. In the distant future and if this project is a success, I want to use the same net server to integrate into VS as well if that is possible.


* This project currently does not have a set timeline as I am quite busy with other responsibilities.
* You are always welcome to submit pull request and issues, altough I will probably not resolve any issues until the project is alpha ready.

