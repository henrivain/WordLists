# WordListsApp
Way to learn word lists easily!
 
## Why?
I wrote this app because I didn't find any other service that could meet my all expectations.   
This was also a great opportunity to learn new ui framework .NET MAUI which I am interested in.   

## What I wanted the app to do?
- Learn words in flipcard style
- Set words as "learned" so I don't need to always go through every word
- Maybe write the words and then validate my input?
- Make app usable in offline mode
- App is currently only available in finnish, but if you are interested in other languages, you can contact the owner

## How to use?

### Installation

#### Android 

1) Click [Releases (Latest)](https://github.com/matikkaeditorinkaantaja/WordLists/releases)  
2) Assets / Wordlists_Android-Signed_vX.X.X.apk  
4) You may be prompted to enable loading apps from internet, when you try to run downloaded .apk file
5) When your device's installer has stopped, you can start the app like any other app

 




<img width="250" align="right" alt="image" src="https://user-images.githubusercontent.com/89461562/202189770-b4a60536-8fb5-454d-963a-458c8d254f36.png">  

#### Windows  
Releasing to windows is currently a bit hard in .NET MAUI. 
You need to build the app yourself.

<img width="250" align="right" alt="image" src="https://user-images.githubusercontent.com/89461562/202190552-c8b4c425-6334-4cbd-a0b8-cb0b8fee0c6e.png">

1) If you don't already have [visual studio 2022](https://visualstudio.microsoft.com/) you need to download it (vs2022 community might be free for you)
2) In installer, select ".NET Multi-platform App UI development"
3) Download source code from [Github](https://github.com/henrivain/WordLists/releases), by clicking "Source code (Zip)"
4) Extract downloaded folder
5) Open extracted solution (.sln) file by double "Open a project or solution" in visual studio and select the file
6) After visual studio has loaded the project, select "Windows Machine" in the middle top selector
7) Hit <kbd>F5</kbd> and app will build and run. It will be also installed to your machine, so you can run it from windows like normal app 

<img width="500" alt="image" src="https://user-images.githubusercontent.com/89461562/202192943-4f0a73c5-73f2-4580-8dfb-173d89641949.png">





#### IOS
- I have no intention to release for ios, but you can build it if you have suitable devices
- App is not tested in ios, so you have to fix all bugs yourself
<img align="right" width="200" alt="Home page" src="https://user-images.githubusercontent.com/89461562/201965381-36cda632-d26c-45fe-9e73-80ec08b697ba.png">
<hr>


### Launching for the first time  

#### Let's generate your first word list!
After launching the app you are in the home page  
By default you dont have any wordlists to train, so let's create one for you!
Go to word list generator page by clicking "Luo uusi sanasto"  &rarr;

You will be navigated to word list generator page  
The page will look something like this one below  

<br/>

<img align="left" width="256" alt="List generator page" src="https://user-images.githubusercontent.com/89461562/201971138-96f62cbe-c255-45aa-b0fc-ff0043c934f2.jpg">

1) Give your list name by typing it to `Otsikko` field

<br/>

2) You can specify which languages the list uses in `Kielet` field.  
  For example for finnish and english use "fi-en".  
  Right items are seen as native language, left ones as foreign language.

<br/>

3) Create the words  
you have three ways to do so:
```
- from clipboard
- by adding on at the time
- (generate from image, this will be added later)
```
> Note that generating from image or from clipboard will overwrite all already added words.
> You can reorder words by dragging and dropping. 
> You can add new words by pressing `uusi sana` button.

<br/>

4) `Vaihda kielet` changes right side words to left side and left side words to right.

6) Finally, save words by pressing `Save to database`.

<br/>



#### How about training?


<img width="252" align="right" alt="image" src="https://user-images.githubusercontent.com/89461562/201992525-ecfd3adb-584d-4d82-ac9a-2784e6ddce3f.png">

Now, that you have your first word list we can start training.  
Click `Harjoitus` in the bottom tab bar to open "Start training menu".

<img width="265" alt="Tab bar" src="https://user-images.githubusercontent.com/89461562/201991394-a2004117-8887-41de-8719-a5678f48a478.png">

If you just added your word collection, you might need to press `Näytä kaikki` button to show your word list.
If your list isn't showing, try to add it again and remember to save. Also reopening the app might help.
The word list should be listed in the bottom of the view like in the image on the right side &rarr;

Start training 
- Android users will swipe to left on top of the list
- Windows users can the press in the listing

Pen icon starts training, where you write the words 
- You will be asked to choose how many questions you want.
- App reviews your answers automatically.
Cards icon means you can train the words in the "flipcard" style.
- You can set status for every word by pressing coloured buttons in the bottom
- App saves progression automatically when you reach the end, or if you press `Tallenna eteneminen` button
- If green check mark is visible, progression is saved.

<img width="250" alt="image" src="https://user-images.githubusercontent.com/89461562/201997258-a4759af9-7e88-4478-beee-8af3fd1dafaf.png">

#### Great! You can now add new lists and start training
- Remember to check out for new updates here in [Worlists Github page](https://github.com/matikkaeditorinkaantaja/WordLists)
- `Hallitse` menu includes more actions to handle your word lists, like exporting and importing word lists to new device

<hr/>

<br/>


## Help needed?
If you have any questins don't hesitate to contact owner or open Github issue in the "Issues" -tab. 
Choose "Help wanted" -label, or any fitting to you problem, from the from the right side.

#### Contact information:

Henri Vainio  
email: matikkaeditorinkaantaja(at)gmail.com   
Github: https://github.com/matikkaeditorinkaantaja  
