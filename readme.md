# Window Switcher
Name and switch between defined windows.  Originally built to help multibox EQEMU.  Selecting a name in the list will open that window and minimize the rest.

## ALPHA RELEASE
This is an alpha release.  The functionality is rough and bugs are likely.  If you experience bugs or have an idea for an improvement, please create an issue.


![Screenshot](/docs/images/WindowSwitch.JPG)

## Features

* Create names for windows
* Start individual or all windows.

## Motivation
Multiboxing 12+ accounts on EQEMU is hard.  As each windows is named the same, I had no easy way to locate and switch to a specific account.  Every time I'd need to loot something, it would be 30 seconds of Alt+Tab until the right account came up.  

This Application was made to solve that problem.  Paired with MQ2AutoLogin, each account is started through the application and I navigate to the correct account by selecting it in the list.

## Notes

* Names are sorted alphabetically.  I use a naming scheme of "{group #} - {class}" to organize the list.

* There's a 5 second delay between each process when you "Start All".

* Unique names aren't enforced, but are recommended.  You may experience problems saving/deleting on items sharing the same name.

* You need to start the program through the application.  It won't detect pre-existing windows or windows opened in other methods.

* The application won't let you open multiple windows of the same name.

## Contributions
Pull requests are very welcome.  I'm not a .NET dev or a windows guy, so my deepest apologies.

If you're experiencing problems, create an issue and I'll do my best to help, but no promises. 