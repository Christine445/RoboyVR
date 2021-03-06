Introduction
============  

Usage
-------

The Virtual Reality user interface will display all given and desired data in a structured environment to help both developers and external visitors to gain further insight into Roboy and how he works. For the developer side, it is important to display the data in a coherent way to clearly communicate the current state of Roboy to aid in implementation and debugging scenarios. Goals for the visitor include designing a visually appealing interface which does not overload the user with unnecessary and missleading information but provides selected information and explanations to satisfy the visitor's interest. Important aspects include structuring and grouping the given data in sets which the user can change between and activate dynamically, providing an intuitive control system which does not need much further explanation and visualizing the given data in a clear and understandable way. 

Structure
---------

In general, the Scene contains two types of objects:

- Scene related Objects: Roboy, the Background, the Camera
- UI related Objects: Canvases, screens, container objects, UI elements such as panels, buttons, images

The UI has three main layers:

- Front-end containing all UI objects
- Core: containing logic, update methods and operations on given data set
- Back-end: Provider of data either through a connection to ROS (or method dummies sending fake data)

Current Implementation
----------------------

The user interface as of now does only contain basic front-end elements such as screens, panels and camera overlay canvases, as well as parts of the basic Core area. These include basic support of the Vive Controllers, the display of all elements, both in the scene and part of the UI on the SteamVR glasses. Many UI elements are not interactive as of now but in future updates this will be changed. 