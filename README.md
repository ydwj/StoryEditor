## StoryEditor

-The Story Editor is a visual script Editor based on Unity X Node that lets you quickly build a text conversation without any code, and allows you to determine the direction of the Story based on your own method of triggering a click event or multiple options. 

-You can also combine the Story Editor and the Unity Timeline system to complete rich cutscenes.

### Use Note

- #### 1. Select ChatNode and press Ctrl+Z to go back to the previous step, Graph will crash unrecoverable directly!

 - The program will report an error: Index was out of range, probably because Xnode and ReorderableList clash in the way they draw lists.
   Some users on Github are currently experiencing these issues and have already submitted them, so hopefully the author will fix the Bug in a follow-up update.

 - Question page: https://github.com/Siccity/xNode/issues/279

- #### 2. When adding more elements to ChatNode, the mobile interface becomes very stagnant.

 - This is because the rendering method adopted by Xnode Graph is still inherited from Unity's EditorWindow class, and OnGUI (), which manages the rendering of window elements,      will be forced to refresh when the mouse moves, while OnGUI () will be called for each individual Node within the Graph, so the actual refresh times are too Big , leading to    Graph latching.

 - So when there are too many elements in a conversation list, you need to actively click the Minimize button to Minimize the nodes and avoid Graph being too jammed.
 
 - There are also user-submitted issues on Github, but there is no reliable solution.If there are bosses have solutions, please send an E-mail: 605716894@qq.com, or in making the    problem of the reply: https://github.com/Siccity/xNode/issues/125


### Guide

- #### It's very easy to learn! There is also a guide within the project!Enjoy it !
