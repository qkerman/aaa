\page prefabs Prefabs
\tableofcontents
This file is documenting all the prefabs that are used in scenes and instantiated in some scripts.
Also, please note that the prefabs used for imported object are already documented in corresponding scripts.
\section panel Panel
This section relates to the different prefabricated menu panels.
\subsection artworksscrollview ArtworksScrollViewPanel
This prefab designs the artwork importation menu, which shows the files in persistent data path using the file chooser, and depending on the type of file that was selected in the \ref globalpanel . 
\subsection globalpanel GlobalPanel
This prefab designs the importation menu, with buttons for each type of new object, these are 3D artworks, sound, video, 360 video, image, light or trigger. 
The last two of this list are not linked to the file chooser, and uses their own sub-menus with importation settings (\ref lightpanel \ref triggerpanel ).
\subsection lightpanel LightPanel
This prefab designs the light importation menu, with some settings for the new light. This light can be Directionnal, Spot, Area or Point.
\subsection loadcomponent LoadComponentPanel
This prefab concerns the second panel of our titlemenu, in which we can load a gallery.
\subsection maincomponent MainComponentPanel
This prefab concerns the first panel of our titlemenu, in which we choose to create, load or leave the application.
\subsection mainmenu MainMenu
This prefab contains the whole main menu, including the prefab for \ref mainmenupanel containing many other prefabs for sub-menus. 
\subsection mainmenupanel MainMenuPanel
This prefab designs the main menu, with the import/create button, leading to \ref globalpanel , the save and load button, the settings button and the quit button.
\subsection titlemenu TitleMenu
This prefab is our start menu, in which the user can perform various manipulations in the panels \ref maincomponent and \ref loadcomponent .
\subsection triggerpanel TriggerPanel
This prefab designs the trigger importation menu, with some settings for the new trigger. This trigger can be a button trigger, an area trigger or a grab trigger.
The files and folders are shown using \ref listitem .
\section widget Widget
\subsection listitem ListItem
This prefab designs an element of the list created using the file chooser. 
An item is composed of an image describing its type and a text displaying its name in current path.
\subsection playpausestop PlayPauseStop
This prefab allows us to play, pause and stop a sound, a video or a 360Video.
\subsection settings Settings
This prefabs designs a button displayed at the top right corner of each menu from \ref mainmenu .
This button can be used to access a help panel or settings, it is composed of a square "i" icon.
\subsection return Return
This prefabs designs a button displayed at the top left corner of each menu from \ref mainmenu .
This button is used to go back to the last menu, it is composed of a square arrow icon.
\section maingallery MainGallery
This prefab is placed in the scene, is having a script to manage the gallery and is composed of the parent of all imported objects, named Gallery, and contains its borders (limits of the gallery).
\section placementmenu PlacementMenu
This prefab designs a menu for object placement in the gallery, showing its actual transform.