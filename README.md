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

## How to use?
- App is currently only available in finnish, but if you are interested in other languages, you can contact the owner

### Installation



#### Android 

1) Click [Releases (Latest)](https://github.com/matikkaeditorinkaantaja/WordLists/releases)  
2) Assets / Wordlists_Android-Signed_vX.X.X.apk  
4) You may be prompted to enable loading apps from internet, when you try to run downloaded .apk file
5) When your device's installer has stopped, you can start the app like any other app
<img align="right" width="200" alt="Home page" src="https://user-images.githubusercontent.com/89461562/201965381-36cda632-d26c-45fe-9e73-80ec08b697ba.png">
 

#### Windows 

- not easy currently, releasing to windows is currently a bit hard in .NET MAUI
- You can copy source code and build it yourself
- Msix package doesn't seem to install currently because of lack of the digital signature



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

<br/>

5) Finally, save words by pressing `Save to database`.


#### How about trainig?
Now, that you have your first word list we can start training.
Click `Harjoitus` in the bottom tab bar to open "Start training menu".
<img width="265" alt="Tab bar" src="https://user-images.githubusercontent.com/89461562/201984917-b681f4b0-9155-4ffc-9bfb-57be88acd77f.png">



<br/>


## Help needed?
If you have any questins don't hesitate to contact owner or open Github issue in the "Issues" -tab. 
Choose "Help wanted" -label, or any fitting to you problem, from the from the right side.

#### Contact information:

Henri Vainio  
email: matikkaeditorinkaantaja(at)gmail.com   
Github: https://github.com/matikkaeditorinkaantaja  
