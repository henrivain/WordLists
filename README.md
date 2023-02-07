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
Finally I was able to make release build for Windows. YOU MIGHT NEED TO INSTALL [winui app sdk](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/downloads) to run the app. If app runs without, that's great!

##### To install for Windows using setup exe
1) Go to Releases > Latest in the Github page right side
2) Download file which name contains "Windows10Setup.exe" from the release assets.
3) Double click the downloaded file and follow the instructions.

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
- by adding one at the time
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
