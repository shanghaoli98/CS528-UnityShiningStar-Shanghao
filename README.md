## Description

<img src=".\Webpage\Weixin Image_20240407201903.png" alt="Weixin Image_20240407201903" style="zoom:50%;" />

Shining Star Project is a demonstration of celestial data visualization using Unity. It showcases the visualization of star data and constellations in a 3D environment optimized for the [Omicron-Unity CAVE2 Simulator](https://github.com/uic-evl/omicron-unity), providing an immersive experience for users.

webpage containing all the info：https://shanghaoli98.github.io/CS528-UnityShiningStar-Shanghao/

### Video Demo

<iframe width="560" height="315" src="https://www.youtube.com/embed/kxJn792j1cA?si=Vb3jNX07FmWZw56s" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>



## Features

- Visualization of star data imported from CSV files.
- Display of constellations based on star pairings.
- Dynamic updating of star positions based on their velocities.
- Elapsed time display to track the passage of time within the simulation.

## Installation

To run the Phase 1 Unity Project locally, follow these steps:

1. Clone the repository using the following command:

   `git clone https://github.com/shanghaoli98/CS528-UnityShiningStar-Shanghao.git`

2. Open the project in Unity.

3. Open the scene named `ShiningStar.unity (Assets/ShiningStar.unity)`.

4. Click the play button on the top to run the project. 

## Usage

- Upon launching the project, users will see a 3D environment displaying stars and constellations.

- Use mouse and keyboard controls to navigate the scene.

- Explore the constellations and observe the movement of stars over time.

- move in CAVE:

  - WASD move the CAVE through the virtual space
  - QE turn the CAVE within the virtual space
  - RF raise and lower the CAVE within the virtual space
  - IJKL move the user within the CAVE (and even out of the CAVE if you move too far)
  - U and O rotate the user within the CAVE

- menu：

  - right click to open the menu

  - left click to close/back to last menu

  - ↑↓ to move the option in the menu

  - right click to select an option in the menu

    <img src=".\Webpage\Weixin Image_20240407201953.png" alt="Weixin Image_20240407201953" style="zoom: 50%;" /><img src=".\Webpage\Weixin Image_20240407201957.png" alt="Weixin Image_20240407201957" style="zoom: 50%;" /><img src=".\Webpage\Weixin Image_20240407202001.png" alt="Weixin Image_20240407202001" style="zoom: 50%;" />


Note: There are three-level menus; the third menu contains the interactions. In the tiered menu, you can use the ↑ and ↓ keys to toggle and right-click different options. By default, the application will display five constellations. Once you select any constellation from the menu, only the selected constellation will be displayed. You can also click the circle next to Reticulum to display the information about *Reticulum*.

You can click **Time Pause** and **Time Start** to pause and start the evolution of the constellations, click **Reset** to reset the location, orientation, and time back to the starting point, click **StarLarge** and **StarSmall** to increase or decrease the distances between the stars, and click **Exoplanet Color Schema** to change the color scheme from stellar type to the number of known planets in the system.

## Development

- The project is structured with separate scripts for managing star data, constellation visualization, and time tracking.
- Star data is imported from CSV files and parsed into StarData objects.
- Constellations are drawn based on predefined star pairings.
- Stars update their positions dynamically based on velocity and elapsed time.

## Dataset Source

The star dataset used in this project is sourced from the [ATHYG Database](https://github.com/astronexus/ATHYG-Database). This repository provides a comprehensive collection of star data, including positions, magnitudes, and spectral classifications. The `athyg_26_reduced_m10` dataset, which contains stars up to 100 light years away, serves as the primary data source for our celestial simulation.

For constellation information, we utilize data from Stellarium's Skycultures repository, available at [Stellarium Skycultures](https://github.com/Stellarium/stellarium/tree/master/skycultures). This dataset contains constellation details, including star pairs and full constellation names, which are essential for drawing constellations in our simulation environment.



