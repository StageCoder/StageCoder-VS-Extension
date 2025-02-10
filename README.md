# Visual Studio
 This extension is a collection of functions that makes your presentation more streamlined and easier to watch.
 
 ## Typing out code
 The first function makes it possible to type out a snippet, character by character, by pressing any button.   
 This is the perfect extension for people showing code on stage, in a classroom, or a workshop.  
            
<img class="demo" src="https://github.com/StageCoder/StageCoderWeb/blob/main/StageCoderWeb/StageCoderWeb/wwwroot/StageCoderDemo2.gif?raw=true" />
        
Steps:  
1. Select some code<br />
2. Right-click and select <strong>Create snippet</strong><br />
3. Name the snippet<br />
4. Now type the name of the snippet in a comment in the code //Snippetname (most comment types work)<br />
5. Make sure the cursor is on the same line as the comment
Press CTRL + TAB + TAB<br />
The Extension now takes over, and you can press any button (except TAB) to type one character at a time.<br />
6. To exit the extension, press TAB.<br /><br />
The snippets are saved in a " Snippets" folder in the solution folder.<br />
<h2>Commands</h2>
CTRL + TAB + TAB is the default key combination. However, you can remap it in Tools | Options | Keyboard.<br />
In some cases, you need to remap it to make it work.
<br /><br />
There are three commands:<br />
<b>StageCoder.Reloadsnippets</b><br />
If you copy snippets or manipulate them outside the extension, this will reload the snippets.<br /><br />
<b>StageCoder.Replacecode</b><br />
It will replace the comment with the snippet without typing each character.<br /><br />
<b>StageCoder.Typecode</b><br />
It will enable you to press any button (except TAB) to type one character at a time taken from the snippet.
<br />
<h2>Stream Deck</h2>
While presenting, we can also use a Stream Deck by using the "Visual Studio"-plugin by Nicollas R.<br />
Adding the commands above can trigger the different commands by pressing the Stream Deck button.<br /><br />
To configure the type command, do the following:<br />
1. Install the Visual Studio plugin from the plugin store.<br />
2. Drag <strong>Execute Command</strong> to a button.<br />
3. Add a Title (This can be anything)<br />
4. Add a Command(Name) in this case <strong>StageCoder.Typecode</strong><br />
5. Command args can be used in three ways.<br />
<strong>Empty</strong> - Will use the current line in Visual Studio to figure out the snippet name.<br />
<strong>SnippetName</strong> - By supplying it with a snippet name, you can trigger that particular snippet.<br />
<strong>[clipboard]</strong> - Will take the current text on the clipboard and use that as a snippet.

## Highlighting code
The second function makes it possible to highlight code by blurring the rest of the code.
This is perfect for focusing on a specific part of the code while presenting.
<img class="demo" src="https://github.com/StageCoder/StageCoderWeb/blob/main/StageCoderWeb/StageCoderWeb/wwwroot/Highlight.gif?raw=true" /> 
        

