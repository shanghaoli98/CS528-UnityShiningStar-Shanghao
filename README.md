## Description

Shining Star Project is a demonstration of celestial data visualization using Unity. It showcases the visualization of star data and constellations in a 3D environment optimized for the [Omicron-Unity CAVE2 Simulator](https://github.com/uic-evl/omicron-unity), providing an immersive experience for users.

webpage containing all the info：https://shanghaoli98.github.io/CS528-UnityShiningStar-Shanghao/

### Video Demo

<video src="C:\Users\uicel\test_shanghao\Video\2024-02-11 16-26-05.mkv"></video>

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

3. Ensure that you have the necessary assets and dependencies configured.

4. Run the project in the Unity Editor or build it for your desired platform.

## Usage

- Upon launching the project, users will see a 3D environment displaying stars and constellations.
- Use mouse and keyboard controls to navigate the scene.
- Explore the constellations and observe the movement of stars over time.

## Development

- The project is structured with separate scripts for managing star data, constellation visualization, and time tracking.
- Star data is imported from CSV files and parsed into StarData objects.
- Constellations are drawn based on predefined star pairings.
- Stars update their positions dynamically based on velocity and elapsed time.

## Dataset Source

The star dataset used in this project is sourced from the [ATHYG Database](https://github.com/astronexus/ATHYG-Database). This repository provides a comprehensive collection of star data, including positions, magnitudes, and spectral classifications. The `athyg_26_reduced_m10` dataset, which contains stars up to 100 light years away, serves as the primary data source for our celestial simulation.

For constellation information, we utilize data from Stellarium's Skycultures repository, available at [Stellarium Skycultures](https://github.com/Stellarium/stellarium/tree/master/skycultures). This dataset contains constellation details, including star pairs and full constellation names, which are essential for drawing constellations in our simulation environment.